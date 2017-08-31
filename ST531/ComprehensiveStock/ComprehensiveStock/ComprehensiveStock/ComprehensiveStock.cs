using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTLoginEx;
using System.Data.OleDb;

namespace RSERP
{
    public class ComprehensiveStock
    {

        /// <summary>
        /// 库存综合查询
        /// </summary>
        /// <param name="iLoginEx"></param>
        /// <param name="cType">0=库存查询，显示所有；1=请购单导入（不仅显示在制）；2=制造单导入（仅显示现存量、在制、已分配量）；3=仅显示替代料</param>
        /// <param name="cInvCode">物料编码</param>
        /// <param name="cDateL">日期L</param>
        /// <param name="cDateH">日期H</param>
        /// <param name="UFsystem">用友中间数据库名</param>
        /// <param name="cWhCode">仓库代码</param>
        /// <param name="iKeys">查询条件</param>
        /// <returns></returns>
        public string ComprehensiveStockInfo(LoginEx iLoginEx, ushort cType, string cInvCode, string cDateL, string cDateH, string UFsystem, string cWhCode, string iKeys)
        {
            return ComprehensiveStockInfo(iLoginEx, cType, cInvCode, cDateL, cDateH, UFsystem, cWhCode, iKeys, "", 1, false);
        }

        /// <summary>
        /// 库存综合查询
        /// </summary>
        /// <param name="iLoginEx"></param>
        /// <param name="cType">0=库存查询，显示所有；1=请购单导入（不仅显示在制）；2=制造单导入（仅显示现存量、在制、已分配量）；3=仅显示替代料</param>
        /// <param name="cInvCode">物料编码</param>
        /// <param name="cDateL">日期L</param>
        /// <param name="cDateH">日期H</param>
        /// <param name="UFsystem">用友中间数据库名</param>
        /// <param name="cWhCode">仓库代码</param>
        /// <param name="iKeys">查询条件</param>
        /// <param name="tempTableName">临时表名</param>
        /// <param name="CurSotckQtySource">初始库存类型（MRP）：0=无；1=现存量；2=安全库存；3=临时库存</param>
        /// <param name="TimeRange">true=考虑时间范围</param>
        /// <returns></returns>
        public string ComprehensiveStockInfo(LoginEx iLoginEx, ushort cType, string cInvCode, string cDateL, string cDateH, string UFsystem, string cWhCode, string iKeys, string tempTableName, ushort CurSotckQtySource, bool TimeRange)
        {

            //--1=请购单
            //--2=采购订单
            //--3=采购到货单
            //--4=现存量
            //--5=生产订单(在制)
            //--6=销售订单
            //--7=生产订单（需求）
            //--8=替代料
            //--9=委外订单（在制）
            //--10=委外订单（需求）
            //--11=新需求（MRP）

            //销售订单
            string SoDateL = iLoginEx.ReadUserProfileValue("MRP_Parameter", "SoDateL").Length == 0 ? "2000-01-01" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "SoDateL");
            string SoDateH = iLoginEx.ReadUserProfileValue("MRP_Parameter", "SoDateH").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "SoDateH");

            //生产订单
            string MoStartDateL = iLoginEx.ReadUserProfileValue("MRP_Parameter", "MoStartDateL").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "MoStartDateL");
            string MoStartDateH = iLoginEx.ReadUserProfileValue("MRP_Parameter", "MoStartDateH").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "MoStartDateH");
            //委外订单
            string OsDateL = iLoginEx.ReadUserProfileValue("MRP_Parameter", "OsDateL").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "OsDateL");
            string OsDateH = iLoginEx.ReadUserProfileValue("MRP_Parameter", "OsDateH").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "OsDateH");
            //采购订单
            string PoDateL = iLoginEx.ReadUserProfileValue("MRP_Parameter", "PoDateL").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "PoDateL");
            string PoDateH = iLoginEx.ReadUserProfileValue("MRP_Parameter", "PoDateH").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "PoDateH");
            //采购到货单
            string PurArrDateL = iLoginEx.ReadUserProfileValue("MRP_Parameter", "PurArrDateL").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "PurArrDateL");
            string PurArrDateH = iLoginEx.ReadUserProfileValue("MRP_Parameter", "PurArrDateH").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "PurArrDateH");
            //请购单
            string PoApDateL = iLoginEx.ReadUserProfileValue("MRP_Parameter", "PoApDateL").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "PoApDateL");
            string PoApDateH = iLoginEx.ReadUserProfileValue("MRP_Parameter", "PoApDateH").Length == 0 ? "2099-12-31" : iLoginEx.ReadUserProfileValue("MRP_Parameter", "PoApDateH");



            OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

            if (myConn.State == System.Data.ConnectionState.Open)
            {
                myConn.Close();
            }
            myConn.Open();

            //用临时表，并分两段，以防止接连接超时
            OleDbCommand myCommand = new OleDbCommand("", myConn);
            myCommand.CommandTimeout = 7200;

            //string ComprehensiveStockST31 = "tempdb..ST31" + iLoginEx.GetMacAddress().Replace(":", "") + iLoginEx.AccID() + DateTime.Now.ToString("yy-MM-dd HH:mm:ss:ffff").Replace("-", "").Replace(" ", "").Replace(":", "");
            string ComprehensiveStockST31 = "";


            if (tempTableName.Length <= 0)
            {
                tempTableName = "zhrserpST31" + iLoginEx.GetMacAddress().Replace(":", "") + iLoginEx.AccID();
            }

            ComprehensiveStockST31 = "tempdb.." + tempTableName;

            string selectSQL = "";
            selectSQL = " if exists (select 1  from tempdb.dbo.sysobjects (nolock) where upper(name)= upper('" + tempTableName + "') and type='U')   \r\n";
            selectSQL += " drop table " + ComprehensiveStockST31 + " ;   \r\n";
            selectSQL += "   \r\n";

            myCommand.CommandText = selectSQL;
            myCommand.ExecuteNonQuery();

            selectSQL = "create table " + ComprehensiveStockST31 + " (  \r\n";
            selectSQL += "  Prod_cInvCode nvarchar(20) null default '',    \r\n";
            selectSQL += "  DocTypeNo tinyint not null default 0,    \r\n";
            selectSQL += "  DocType nvarchar(300) null default '',    \r\n";
            selectSQL += "  cCode  nvarchar(120) null default '',    \r\n";
            selectSQL += "  cpersonname nvarchar(40) null default '',    \r\n";
            selectSQL += "  cDefine30  nvarchar(120) null default '',    \r\n";
            selectSQL += "  dDate datetime null,    \r\n";
            selectSQL += "  cinvcode  nvarchar(20) not null default '',    \r\n";
            selectSQL += "  moQty decimal(28,4) null default 0,    \r\n";
            selectSQL += "  Now_PurArrQty  decimal(28,4) null default 0,    \r\n";
            selectSQL += "  Now_PurQty  decimal(28,4) null default 0,    \r\n";
            selectSQL += "  CurSotckQty  decimal(28,4) null default 0,    \r\n";
            selectSQL += "  useQty  decimal(28,4) null default 0,    \r\n";
            selectSQL += "  toArrQty  decimal(28,4) null default 0,    \r\n";
            selectSQL += "  AltmQty  decimal(28,4) null default 0,    \r\n";
            selectSQL += "  OsQty  decimal(28,4) null default 0)    \r\n";
            selectSQL += "   \r\n";
            selectSQL += "   \r\n";


            myCommand.CommandText = selectSQL;
            myCommand.ExecuteNonQuery();

            if (cType == 0 || cType == 1)
            {
                selectSQL = " ----*到货量********     \r\n";
                selectSQL += "   \r\n";
                selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
                selectSQL += " SELECT  '' as 'Prod_cInvCode',DocTypeNo=3,DocType='采购到货单',Pu_ArrivalVouch.cCode,Pu_ArrivalVouch.cMaker as 'cpersonname',Pu_ArrivalVouchs.cDefine30,Pu_ArrivalVouch.dDate,Pu_ArrivalVouchs.cinvcode,moQty=0,     \r\n";
                selectSQL += "  case when Pu_ArrivalVouchs.iquantity>0 then     \r\n";
                selectSQL += "  case when     \r\n";
                selectSQL += "  (case  when isnull(Pu_ArrivalVouchs.bgsp,0)=0 then CONVERT(DECIMAL(28,6),(Pu_ArrivalVouchs.iquantity-ISNULL(fRefuseQuantity,0)-ISNULL(fInValidInQuan,0)-ISNULL(fValidInQuan,0)))     \r\n";
                selectSQL += "  else isnull(Pu_ArrivalVouchs.iquantity,0) -ISNULL(fRefuseQuantity,0)- isnull(QmArrCheckSrv.FMinQty,0) - isnull(Pu_ArrivalVouchs.fdegradeinquantity,0)     \r\n";
                selectSQL += "  - isnull(Pu_ArrivalVouchs.finvalidinquan,0) - isnull(Pu_ArrivalVouchs.fdtquantity,0) end) >=0 then     \r\n";
                selectSQL += "  (case  when isnull(Pu_ArrivalVouchs.bgsp,0)=0 then CONVERT(DECIMAL(28,6),(Pu_ArrivalVouchs.iquantity-ISNULL(fRefuseQuantity,0)-ISNULL(fInValidInQuan,0)-ISNULL(fValidInQuan,0)))     \r\n";
                selectSQL += "  else isnull(Pu_ArrivalVouchs.iquantity,0) -ISNULL(fRefuseQuantity,0)- isnull(QmArrCheckSrv.FMinQty,0) - isnull(Pu_ArrivalVouchs.fdegradeinquantity,0)     \r\n";
                selectSQL += "  - isnull(Pu_ArrivalVouchs.finvalidinquan,0) - isnull(Pu_ArrivalVouchs.fdtquantity,0) end)     \r\n";
                selectSQL += "  else     \r\n";
                selectSQL += "  0 end     \r\n";
                selectSQL += "  else     \r\n";
                selectSQL += "  case when (case  when isnull(Pu_ArrivalVouchs.bgsp,0)=0 then CONVERT(DECIMAL(28,6),(Pu_ArrivalVouchs.iquantity-ISNULL(fRefuseQuantity,0)-ISNULL(fInValidInQuan,0)-ISNULL(fValidInQuan,0)))     \r\n";
                selectSQL += "  else isnull(Pu_ArrivalVouchs.iquantity,0) -ISNULL(fRefuseQuantity,0)- isnull(QmArrCheckSrv.FMinQty,0) - isnull(Pu_ArrivalVouchs.fdegradeinquantity,0)     \r\n";
                selectSQL += "  - isnull(Pu_ArrivalVouchs.finvalidinquan,0) - isnull(Pu_ArrivalVouchs.fdtquantity,0) end) <=0 then     \r\n";
                selectSQL += "  (case  when isnull(Pu_ArrivalVouchs.bgsp,0)=0 then CONVERT(DECIMAL(28,6),(Pu_ArrivalVouchs.iquantity-ISNULL(fRefuseQuantity,0)-ISNULL(fInValidInQuan,0)-ISNULL(fValidInQuan,0)))     \r\n";
                selectSQL += "  else isnull(Pu_ArrivalVouchs.iquantity,0) -ISNULL(fRefuseQuantity,0)- isnull(QmArrCheckSrv.FMinQty,0) - isnull(Pu_ArrivalVouchs.fdegradeinquantity,0)     \r\n";
                selectSQL += "  - isnull(Pu_ArrivalVouchs.finvalidinquan,0) - isnull(Pu_ArrivalVouchs.fdtquantity,0) end)     \r\n";
                selectSQL += "  else     \r\n";
                selectSQL += "  0     \r\n";
                selectSQL += "  end     \r\n";
                selectSQL += "  end     \r\n";
                selectSQL += "  AS Now_PurArrQty ,Now_PurQty=0,CurSotckQty=0 , useQty=0, toArrQty=0,AltmQty=0,OsQty=0    \r\n";
                selectSQL += "  FROM Pu_ArrivalVouchs (nolock) LEFT JOIN Pu_ArrivalVouch (nolock) ON Pu_ArrivalVouchs.ID=Pu_ArrivalVouch.ID     \r\n";
                selectSQL += "  LEFT JOIN Inventory (nolock) ON Pu_ArrivalVouchs.cInvCode=Inventory.cInvCode     \r\n";
                selectSQL += "  Left Join QmArrCheckSrv (nolock) on QmArrCheckSrv.SOURCEAUTOID=Pu_ArrivalVouchs.autoid     \r\n";
                selectSQL += "  LEFT JOIN ComputationUnit as Unit1 (nolock) on Inventory.cPUComUnitCode=Unit1.cComUnitCode     \r\n";
                selectSQL += "  LEFT JOIN So_Sodetails (nolock) ON PU_ArrivalVouchs.sodid=cast(So_Sodetails.isosid as nvarchar(60)) and PU_ArrivalVouchs.sotype =1     \r\n";
                selectSQL += "  LEFT JOIN v_mps_forecast (nolock) ON PU_ArrivalVouchs.sodid=cast(v_mps_forecast.ForecastDId as nvarchar(60)) and PU_ArrivalVouchs.sotype =2     \r\n";
                selectSQL += "  LEFT JOIN v_ex_order_forPUReport AS v_expo (nolock) ON PU_ArrivalVouchs.sodid=cast(v_expo.autoid as nvarchar(60)) and PU_ArrivalVouchs.sotype =3     \r\n";
                selectSQL += "  left join AA_RequirementClass (nolock) on AA_RequirementClass.cRClassCode=PU_ArrivalVouchs.sodid  and PU_ArrivalVouchs.sotype =4     \r\n";
                selectSQL += "  left join so_somain (nolock) on so_somain.csocode=PU_ArrivalVouchs.sodid and  PU_ArrivalVouchs.sotype =5     \r\n";
                selectSQL += "  left join ex_order (nolock) on ex_order.ccode=PU_ArrivalVouchs.sodid and  PU_ArrivalVouchs.sotype =6     \r\n";
                selectSQL += "  WHERE   len(isnull(PU_ArrivalVouch.cverifier,''))>0   \r\n";
                if (cInvCode.Length > 0)
                {
                    selectSQL += " and Pu_ArrivalVouchs.cInvCode='" + cInvCode + "'  \r\n";
                }
                else
                {
                    if (iKeys.Length > 0)
                    {
                        selectSQL += "  and exists (select 1 from " + iKeys + "  keys where Pu_ArrivalVouchs.cInvCode=keys.cInvCode) \r\n";
                    }
                }
                if (TimeRange)
                {
                    selectSQL += " and Pu_ArrivalVouch.dDate>='" + PurArrDateL + "' and Pu_ArrivalVouch.dDate<='" + PurArrDateH + "' ";
                }
                else
                {
                    if (cDateL.Length > 0)
                    {
                        selectSQL += " and Pu_ArrivalVouch.dDate>='" + cDateL + "' ";
                    }
                    if (cDateH.Length > 0)
                    {
                        selectSQL += " and Pu_ArrivalVouch.dDate<='" + cDateH + "' ";
                    }
                }

                selectSQL += "  and  len(isnull(PU_ArrivalVouch.ccloser,'')+isnull(PU_ArrivalVouchs.cbcloser,''))=0 and (len(isnull(Pu_ArrivalVouchs.cwhcode,''))=0 OR (inventory.bInvBatch=1 and len(ISNULL(Pu_ArrivalVouchs.cbatch,''))=0))     \r\n";
                selectSQL += "  AND ( (isnull(Pu_ArrivalVouch.bnegative,0)<>1 AND len(isnull(Pu_ArrivalVouchs.cbcloser,'')) = 0 ) or     \r\n";
                selectSQL += "  (isnull(Pu_ArrivalVouchs.bgsp,0)=0 AND  isnull(Pu_ArrivalVouch.bnegative,0)=1 AND len(isnull(Pu_ArrivalVouchs.cbcloser,'')) =0 and Pu_ArrivalVouch.iBillType = 1))     \r\n";
                selectSQL += "  and     \r\n";
                selectSQL += "  (     \r\n";
                selectSQL += "  case when Pu_ArrivalVouchs.iquantity>0 then     \r\n";
                selectSQL += "  case when     \r\n";
                selectSQL += "  (case  when isnull(Pu_ArrivalVouchs.bgsp,0)=0 then CONVERT(DECIMAL(28,6),(Pu_ArrivalVouchs.iquantity-ISNULL(fRefuseQuantity,0)-ISNULL(fInValidInQuan,0)-ISNULL(fValidInQuan,0)))     \r\n";
                selectSQL += "  else isnull(Pu_ArrivalVouchs.iquantity,0) -ISNULL(fRefuseQuantity,0)- isnull(QmArrCheckSrv.FMinQty,0) - isnull(Pu_ArrivalVouchs.fdegradeinquantity,0)     \r\n";
                selectSQL += "  - isnull(Pu_ArrivalVouchs.finvalidinquan,0) - isnull(Pu_ArrivalVouchs.fdtquantity,0) end) >=0 then     \r\n";
                selectSQL += "  (case  when isnull(Pu_ArrivalVouchs.bgsp,0)=0 then CONVERT(DECIMAL(28,6),(Pu_ArrivalVouchs.iquantity-ISNULL(fRefuseQuantity,0)-ISNULL(fInValidInQuan,0)-ISNULL(fValidInQuan,0)))     \r\n";
                selectSQL += "  else isnull(Pu_ArrivalVouchs.iquantity,0) -ISNULL(fRefuseQuantity,0)- isnull(QmArrCheckSrv.FMinQty,0) - isnull(Pu_ArrivalVouchs.fdegradeinquantity,0) - isnull(Pu_ArrivalVouchs.finvalidinquan,0)     \r\n";
                selectSQL += "  - isnull(Pu_ArrivalVouchs.fdtquantity,0) end)     \r\n";
                selectSQL += "  else     \r\n";
                selectSQL += "  0 end     \r\n";
                selectSQL += "  else     \r\n";
                selectSQL += "  case when (case  when isnull(Pu_ArrivalVouchs.bgsp,0)=0 then CONVERT(DECIMAL(28,6),(Pu_ArrivalVouchs.iquantity-ISNULL(fRefuseQuantity,0)-ISNULL(fInValidInQuan,0)-ISNULL(fValidInQuan,0)))     \r\n";
                selectSQL += "  else isnull(Pu_ArrivalVouchs.iquantity,0) -ISNULL(fRefuseQuantity,0)- isnull(QmArrCheckSrv.FMinQty,0) - isnull(Pu_ArrivalVouchs.fdegradeinquantity,0) - isnull(Pu_ArrivalVouchs.finvalidinquan,0)     \r\n";
                selectSQL += "  - isnull(Pu_ArrivalVouchs.fdtquantity,0) end) <=0 then     \r\n";
                selectSQL += "  (case  when isnull(Pu_ArrivalVouchs.bgsp,0)=0 then CONVERT(DECIMAL(28,6),(Pu_ArrivalVouchs.iquantity-ISNULL(fRefuseQuantity,0)-ISNULL(fInValidInQuan,0)-ISNULL(fValidInQuan,0)))     \r\n";
                selectSQL += "  else isnull(Pu_ArrivalVouchs.iquantity,0) -ISNULL(fRefuseQuantity,0)- isnull(QmArrCheckSrv.FMinQty,0) - isnull(Pu_ArrivalVouchs.fdegradeinquantity,0)     \r\n";
                selectSQL += "  - isnull(Pu_ArrivalVouchs.finvalidinquan,0) - isnull(Pu_ArrivalVouchs.fdtquantity,0) end)     \r\n";
                selectSQL += "  else     \r\n";
                selectSQL += "  0     \r\n";
                selectSQL += "  end     \r\n";
                selectSQL += "  end)<>0    \r\n";

                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();

                selectSQL = "   \r\n";
                selectSQL += "  ----*请购单********     \r\n";
                selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
                selectSQL += "  SELECT '' as 'Prod_cInvCode',DocTypeNo=1,DocType='请购单',Pu_AppVouch.cCode,Pu_AppVouch.cMaker as 'cpersonname',Pu_AppVouchs.cDefine30,Pu_AppVouch.dDate,Pu_AppVouchs.cInvCode,moQty=0,Now_PurArrQty=0,fQuantity-ISNULL(iReceivedQTY,0) AS Now_PurQty,CurSotckQty=0, useQty=0, toArrQty=0,AltmQty=0,OsQty=0     \r\n";
                selectSQL += "  FROM Pu_AppVouch (nolock) INNER JOIN Pu_AppVouchs (nolock) ON Pu_AppVouch.ID=Pu_AppVouchs.ID     \r\n";
                selectSQL += "  left outer join PurchaseType t (nolock) on t.cPTCode = Pu_AppVouch.cPTCode     \r\n";
                selectSQL += "  LEFT outer JOIN So_Sodetails (nolock) ON Pu_AppVouchs.sodid=cast(So_Sodetails.isosid as nvarchar(60)) and Pu_AppVouchs.sotype =1     \r\n";
                selectSQL += "  --LEFT outer JOIN v_mps_forecast (nolock) ON Pu_AppVouchs.sodid=cast(v_mps_forecast.ForecastDId as nvarchar(60)) and Pu_AppVouchs.sotype =2     \r\n";
                selectSQL += "  LEFT outer JOIN v_ex_order_forPUReport AS v_expo (nolock) ON Pu_AppVouchs.sodid=cast(v_expo.autoid as nvarchar(60)) and Pu_AppVouchs.sotype =3     \r\n";
                selectSQL += "  LEFT outer join AA_RequirementClass (nolock) on AA_RequirementClass.cRClassCode=Pu_AppVouchs.sodid  and Pu_AppVouchs.sotype =4     \r\n";
                selectSQL += "  LEFT outer join so_somain (nolock) on so_somain.csocode=Pu_AppVouchs.sodid and  Pu_AppVouchs.sotype =5     \r\n";
                selectSQL += "  LEFT outer join ex_order (nolock) on ex_order.ccode=Pu_AppVouchs.sodid and  Pu_AppVouchs.sotype =6     \r\n";
                selectSQL += "  WHERE  1=1   \r\n";
                if (cInvCode.Length > 0)
                {
                    selectSQL += " and  Pu_AppVouchs.cInvCode='" + cInvCode + "' \r\n";
                }
                else
                {
                    if (iKeys.Length > 0)
                    {
                        selectSQL += "  and exists (select 1 from " + iKeys + "  keys where Pu_AppVouchs.cInvCode=keys.cInvCode) \r\n";
                    }
                }
                if (TimeRange)
                {
                    selectSQL += " and Pu_AppVouch.dDate>='" + PoApDateL + "' and Pu_AppVouch.dDate<='" + PoApDateH + "' ";
                }
                else
                {
                    if (cDateL.Length > 0)
                    {
                        selectSQL += " and Pu_AppVouch.dDate>='" + cDateL + "' ";
                    }
                    if (cDateH.Length > 0)
                    {
                        selectSQL += " and Pu_AppVouch.dDate<='" + cDateH + "' ";
                    }
                }
                selectSQL += "  and len(isnull(PU_AppVouchs.cbcloser,'')+isnull(PU_AppVouch.cCloser,''))=0 and  Pu_AppVouch.cBusType<>'直运采购' and Pu_AppVouch.cBusType<>'固定资产' and Pu_AppVouch.cBusType<>'委外加工'     \r\n";
                selectSQL += "  and (case when len(ISNULL(Pu_AppVouchs.cbCloser,''))>0 or isnull(t.bPTMPS_MRP,1) = 0 then 4 when len(ISNULL(Pu_AppVouch.cVerifier,''))>0 then 3 when len(ISNULL(Pu_AppVouch.cLocker,''))>0 then 2 else 1 end)<>4     \r\n";
                selectSQL += "  and fQuantity-ISNULL(iReceivedQTY,0)<>0     \r\n";
                selectSQL += "  and len(ISNULL(Pu_AppVouch.cVerifier,''))>0     \r\n";

                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();

                selectSQL = "   \r\n";
                selectSQL += "  ----**采购单****     \r\n";
                selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
                selectSQL += "  SELECT '' as 'Prod_cInvCode',DocTypeNo=2,DocType='采购订单',Po_Pomain.cPoId as 'cCode',person.cpersonname,Po_Podetails.cDefine30,Po_Pomain.dPODate as 'dDate',Po_Podetails.cInvCode,moQty=0,Now_PurArrQty=0,(ISNULL(Po_Podetails.iQuantity,0)-ISNULL(iReceivedQty,0)-ISNULL(iArrQty,0)) as Now_PurQty,     \r\n";
                selectSQL += "  CurSotckQty=0 , useQty=0, toArrQty=0,AltmQty=0,OsQty=0    \r\n";
                selectSQL += "  FROM Po_Pomain (nolock) INNER JOIN Po_Podetails (nolock) ON Po_Pomain.POID=Po_Podetails.POID     \r\n";
                selectSQL += "  left outer join PurchaseType t (nolock) on t.cPTCode = Po_Pomain.cPTCode     \r\n";
                selectSQL += "  LEFT outer JOIN So_Sodetails (nolock) ON Po_Podetails.sodid=cast(So_Sodetails.isosid as nvarchar(60)) and Po_Podetails.sotype =1     \r\n";
                selectSQL += "  LEFT outer JOIN v_ex_order_forPUReport AS v_expo (nolock) ON Po_Podetails.sodid=cast(v_expo.autoid as nvarchar(60)) and Po_Podetails.sotype =3     \r\n";
                selectSQL += "  LEFT outer join AA_RequirementClass (nolock) on AA_RequirementClass.cRClassCode=Po_Podetails.sodid  and Po_Podetails.sotype =4     \r\n";
                selectSQL += "  LEFT outer join so_somain (nolock) on so_somain.csocode=Po_Podetails.sodid and  Po_Podetails.sotype =5     \r\n";
                selectSQL += "  LEFT outer join ex_order (nolock) on ex_order.ccode=Po_Podetails.sodid and  Po_Podetails.sotype =6     \r\n";
                selectSQL += "  left join person  (nolock) on Po_Pomain.cpersoncode = person.cpersoncode  \r\n";
                selectSQL += "  WHERE   1=1  \r\n";
                if (cInvCode.Length > 0)
                {
                    selectSQL += " and Po_Podetails.cInvCode='" + cInvCode + "'  \r\n";
                }
                else
                {
                    if (iKeys.Length > 0)
                    {
                        selectSQL += "  and exists (select 1 from " + iKeys + "  keys where Po_Podetails.cInvCode=keys.cInvCode) \r\n";
                    }
                }
                if (TimeRange)
                {
                    selectSQL += " and Po_Pomain.dPODate>='" + PoDateL + "' and Po_Pomain.dPODate<='" + PoDateH + "' ";
                }
                else
                {
                    if (cDateL.Length > 0)
                    {
                        selectSQL += " and Po_Pomain.dPODate>='" + cDateL + "' ";
                    }
                    if (cDateH.Length > 0)
                    {
                        selectSQL += " and Po_Pomain.dPODate<='" + cDateH + "' ";
                    }
                }
                selectSQL += " and len(isnull(PO_Podetails.cbCloser,'')+isnull(PO_Pomain.cCloser,''))=0 and   Po_Pomain.cBusType<>'直运采购' and Po_Pomain.cBusType<>'固定资产'     \r\n";
                selectSQL += "  and (case when len(ISNULL(po_podetails.cbcloser,''))>0 or isnull(t.bPTMPS_MRP,1) = 0 then 4 when ((len(isnull(Po_Pomain.cVerifier,N''))>0 and len(isnull(Po_Pomain.cChanger,N''))=0)or len(isnull(Po_Pomain.cChangVerifier,N''))>0 ) then 3     \r\n";
                selectSQL += "  when len(ISNULL(Po_Pomain.cLocker,''))>0 then 2 else 1 end)<>4     \r\n";
                selectSQL += "  and (ISNULL(Po_Podetails.iQuantity,0)-ISNULL(iReceivedQty,0)-ISNULL(iArrQty,0))<>0     \r\n";
                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();
            }


            switch (CurSotckQtySource)
            {
                case 0://无库存
                    {

                        break;
                    }
                case 1://现存量
                    {
                        selectSQL = "   \r\n";
                        selectSQL += "  ----*现存量********     \r\n";
                        selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
                        selectSQL += "  select '' as 'Prod_cInvCode',DocTypeNo=4,DocType='现存量','['+c.cWhCode+']'+c.cWhName as 'cCode','' as cpersonname,''  as 'cDefine30',null as 'dDate', k.cInvCode, moQty=0,Now_PurArrQty=0,Now_PurQty=0, CurSotckQty=sum(isnull(k.iQuantity,0) - isnull(k.fStopQuantity,0)), useQty=0, toArrQty=0 ,AltmQty=0,OsQty=0    \r\n";
                        selectSQL += "  from CurrentStock k (nolock) left join Warehouse c (nolock)  on k.cWhCode = c.cWhCode   where 1=1   \r\n";
                        if (cInvCode.Length > 0)
                        {
                            selectSQL += " and k.cInvCode='" + cInvCode + "' \r\n";

                        }
                        else
                        {
                            if (iKeys.Length > 0)
                            {
                                selectSQL += "  and exists (select 1 from " + iKeys + "  keys where k.cInvCode=keys.cInvCode) \r\n";
                            }
                        }
                        cWhCode = cWhCode.Replace("\r", "");
                        cWhCode = cWhCode.Replace("\n", "");
                        cWhCode = cWhCode.Replace("；", ";");

                        if (cWhCode.Length > 0)
                        {
                            string cWhCodeChild = "";
                            string[] paracWhCode = cWhCode.Split(';');
                            if (paracWhCode.Length > 0)
                            {
                                for (int i = 0; i < paracWhCode.Length; i++)
                                {
                                    cWhCodeChild += "'" + paracWhCode[i].ToString() + "',";
                                }

                                selectSQL += " and  k.cWhCode in (" + cWhCodeChild + "'\r\n'" + ") \r\n";
                            }
                            else
                            {
                                selectSQL += " and  k.cWhCode ='" + cWhCode + "' \r\n";
                            }
                        }


                        selectSQL += " and c.bMRP=1 and (isnull(k.iQuantity,0) - isnull(k.fStopQuantity,0))<>0     \r\n";
                        selectSQL += "  group by k.cInvCode,'['+c.cWhCode+']'+c.cWhName     \r\n";
                        selectSQL += "           \r\n";
                        selectSQL += "    \r\n";
                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();

                        if (cInvCode.Length > 0)
                        {

                            //替代料
                            //selectSQL += "  UNION ALL     \r\n";
                            selectSQL = "  ---替代料  \r\n";
                            selectSQL += "   \r\n";
                            selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
                            selectSQL += "  select '' as 'Prod_cInvCode',DocTypeNo=8,DocType='替代料','['+c.cWhCode+']'+c.cWhName as 'cCode','' as cpersonname,''  as 'cDefine30',null as 'dDate',  \r\n";
                            if (cType == 3)
                            {
                                selectSQL += "  k.cInvCode,  \r\n";
                            }
                            else
                            {
                                selectSQL += "  cInvCode='" + cInvCode + "',  \r\n";
                            }
                            selectSQL += "  moQty=0,Now_PurArrQty=0,Now_PurQty=0, CurSotckQty=0, useQty=0, toArrQty=0 ,AltmQty=sum(isnull(k.iQuantity,0) - isnull(k.fStopQuantity,0)),OsQty=0     \r\n";
                            selectSQL += "   from  CurrentStock k (nolock) left join Warehouse c (nolock)  on k.cWhCode = c.cWhCode    where 1=1   \r\n";

                            cWhCode = cWhCode.Replace("\r", "");
                            cWhCode = cWhCode.Replace("\n", "");
                            cWhCode = cWhCode.Replace("；", ";");

                            if (cWhCode.Length > 0)
                            {
                                string cWhCodeChild = "";
                                string[] paracWhCode = cWhCode.Split(';');
                                if (paracWhCode.Length > 0)
                                {
                                    for (int i = 0; i < paracWhCode.Length; i++)
                                    {
                                        cWhCodeChild += "'" + paracWhCode[i].ToString() + "',";
                                    }

                                    selectSQL += " and  k.cWhCode in (" + cWhCodeChild + "'\r\n'" + ") \r\n";
                                }
                                else
                                {
                                    selectSQL += " and  k.cWhCode ='" + cWhCode + "' \r\n";
                                }
                            }

                            selectSQL += " and c.bMRP=1 and (isnull(k.iQuantity,0) - isnull(k.fStopQuantity,0))<>0   \r\n";
                            selectSQL += " and exists (select 1 from (  \r\n";
                            selectSQL += " select cInvCodeAtnm  as 'cInvCode' from " + iLoginEx.pubDB_UT() + "..AlternativeMateriel (nolock) where cAccID='" + iLoginEx.AccID() + "' and AtnmType=0 and Disabled=0 and isMRP=1 and cInvCode='" + cInvCode + "'  \r\n";
                            if (TimeRange)
                            {
                                selectSQL += " and StartDate>='" + SoDateL + "' ";
                            }
                            else
                            {
                                if (cDateL.Length > 0)
                                {
                                    selectSQL += "  and StartDate>='" + cDateL + "'  \r\n";
                                }
                            }
                            selectSQL += " union all  \r\n";
                            selectSQL += " select cInvCode from " + iLoginEx.pubDB_UT() + "..AlternativeMateriel (nolock) where cAccID='" + iLoginEx.AccID() + "' and AtnmType=0 and Disabled=0 and isMRP=1 and isTwoWay=1 and cInvCodeAtnm='" + cInvCode + "'  \r\n";
                            if (TimeRange)
                            {
                                selectSQL += " and StartDate>='" + SoDateL + "' ";
                            }
                            else
                            {
                                if (cDateL.Length > 0)
                                {
                                    selectSQL += "  and StartDate>='" + cDateL + "'  \r\n";
                                }
                            }
                            selectSQL += " )altm where k.cInvCode=altm.cInvCode  \r\n";
                            selectSQL += " )        \r\n";
                            selectSQL += "   group by k.cInvCode,'['+c.cWhCode+']'+c.cWhName       \r\n";

                            myCommand.CommandText = selectSQL;
                            myCommand.ExecuteNonQuery();
                        }

                        break;
                    }
                case 2://安全库存
                    {
                        break;
                    }
                case 3://临时库存
                    {
                        selectSQL = "   \r\n";
                        selectSQL += "  ----*临时库存********     \r\n";
                        selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
                        selectSQL += "  select '' as 'Prod_cInvCode',DocTypeNo=4,DocType='临时库存','['+c.cWhCode+']'+c.cWhName as 'cCode','' as cpersonname,''  as 'cDefine30',null as 'dDate', k.cInvCode, moQty=0,Now_PurArrQty=0,Now_PurQty=0, CurSotckQty=sum(isnull(k.Qty,0)), useQty=0, toArrQty=0 ,AltmQty=0,OsQty=0    \r\n";
                        selectSQL += "  from  zhrs_t_MRP_CurrentStock k (nolock) left join Warehouse c (nolock)  on k.cWhCode = c.cWhCode   where 1=1   \r\n";
                        if (cInvCode.Length > 0)
                        {
                            selectSQL += " and k.cInvCode='" + cInvCode + "' \r\n";

                        }
                        else
                        {
                            if (iKeys.Length > 0)
                            {
                                selectSQL += "  and exists (select 1 from " + iKeys + "  keys where k.cInvCode=keys.cInvCode) \r\n";
                            }
                        }
                        cWhCode = cWhCode.Replace("\r", "");
                        cWhCode = cWhCode.Replace("\n", "");
                        cWhCode = cWhCode.Replace("；", ";");

                        if (cWhCode.Length > 0)
                        {
                            string cWhCodeChild = "";
                            string[] paracWhCode = cWhCode.Split(';');
                            if (paracWhCode.Length > 0)
                            {
                                for (int i = 0; i < paracWhCode.Length; i++)
                                {
                                    cWhCodeChild += "'" + paracWhCode[i].ToString() + "',";
                                }

                                selectSQL += " and  k.cWhCode in (" + cWhCodeChild + "'\r\n'" + ") \r\n";
                            }
                            else
                            {
                                selectSQL += " and  k.cWhCode ='" + cWhCode + "' \r\n";
                            }
                        }


                        selectSQL += " and c.bMRP=1 and isnull(k.Qty,0)<>0     \r\n";
                        selectSQL += "  group by k.cInvCode,'['+c.cWhCode+']'+c.cWhName     \r\n";
                        selectSQL += "           \r\n";
                        selectSQL += "    \r\n";
                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();

                        if (cInvCode.Length > 0)
                        {

                            //替代料
                            //selectSQL += "  UNION ALL     \r\n";
                            selectSQL = "  ---临时库存替代料  \r\n";
                            selectSQL += "   \r\n";
                            selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
                            selectSQL += "  select '' as 'Prod_cInvCode',DocTypeNo=8,DocType='替代料','['+c.cWhCode+']'+c.cWhName as 'cCode','' as cpersonname,''  as 'cDefine30',null as 'dDate',  \r\n";
                            if (cType == 3)
                            {
                                selectSQL += "  k.cInvCode,  \r\n";
                            }
                            else
                            {
                                selectSQL += "  cInvCode='" + cInvCode + "',  \r\n";
                            }
                            selectSQL += "  moQty=0,Now_PurArrQty=0,Now_PurQty=0, CurSotckQty=0, useQty=0, toArrQty=0 ,AltmQty=sum(isnull(k.Qty,0)),OsQty=0     \r\n";
                            selectSQL += "   from  zhrs_t_MRP_CurrentStock k (nolock) left join Warehouse c (nolock)  on k.cWhCode = c.cWhCode    where 1=1   \r\n";

                            cWhCode = cWhCode.Replace("\r", "");
                            cWhCode = cWhCode.Replace("\n", "");
                            cWhCode = cWhCode.Replace("；", ";");

                            if (cWhCode.Length > 0)
                            {
                                string cWhCodeChild = "";
                                string[] paracWhCode = cWhCode.Split(';');
                                if (paracWhCode.Length > 0)
                                {
                                    for (int i = 0; i < paracWhCode.Length; i++)
                                    {
                                        cWhCodeChild += "'" + paracWhCode[i].ToString() + "',";
                                    }

                                    selectSQL += " and  k.cWhCode in (" + cWhCodeChild + "'\r\n'" + ") \r\n";
                                }
                                else
                                {
                                    selectSQL += " and  k.cWhCode ='" + cWhCode + "' \r\n";
                                }
                            }

                            selectSQL += " and c.bMRP=1 and isnull(k.Qty,0) <>0   \r\n";
                            selectSQL += " and exists (select 1 from (  \r\n";
                            selectSQL += " select cInvCodeAtnm  as 'cInvCode' from " + iLoginEx.pubDB_UT() + "..AlternativeMateriel (nolock) where cAccID='" + iLoginEx.AccID() + "' and AtnmType=0 and Disabled=0 and isMRP=1 and cInvCode='" + cInvCode + "'  \r\n";
                            if (TimeRange)
                            {
                                selectSQL += " and StartDate>='" + SoDateL + "' ";
                            }
                            else
                            {
                                if (cDateL.Length > 0)
                                {
                                    selectSQL += "  and StartDate>='" + cDateL + "'  \r\n";
                                }
                            }
                            selectSQL += " union all  \r\n";
                            selectSQL += " select cInvCode from " + iLoginEx.pubDB_UT() + "..AlternativeMateriel (nolock) where cAccID='" + iLoginEx.AccID() + "' and AtnmType=0 and Disabled=0 and isMRP=1 and isTwoWay=1 and cInvCodeAtnm='" + cInvCode + "'  \r\n";
                            if (TimeRange)
                            {
                                selectSQL += " and StartDate>='" + SoDateL + "' ";
                            }
                            else
                            {
                                if (cDateL.Length > 0)
                                {
                                    selectSQL += "  and StartDate>='" + cDateL + "'  \r\n";
                                }
                            }
                            selectSQL += " )altm where k.cInvCode=altm.cInvCode  \r\n";
                            selectSQL += " )        \r\n";
                            selectSQL += "   group by k.cInvCode,'['+c.cWhCode+']'+c.cWhName       \r\n";

                            myCommand.CommandText = selectSQL;
                            myCommand.ExecuteNonQuery();
                        }


                        break;
                    }
            }



            if (cType == 0 || cType == 2)
            {
                selectSQL = "  ----*生产订单(在制)********     \r\n";
                selectSQL += "   \r\n";
                selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
                selectSQL += " select '' as 'Prod_cInvCode',DocTypeNo=5,'生产订单(在制)-' +isnull(d.cDepName,'') as  'DocType' ,c.MoCode as 'cCode',u.cUser_Name as 'cpersonname',cDefine30=isnull(a.SoCode,'')+'('+convert(varchar(8),isnull(a.sortseq,0))+')',b.StartDate as 'dDate',a.InvCode as 'cInvCode',isnull(a.qty,0)-isnull(a.QualifiedInQty,0) as 'moQty',    \r\n";
                selectSQL += "  Now_PurArrQty=0 ,Now_PurQty=0,CurSotckQty=0 , useQty=0, toArrQty=0,AltmQty=0,OsQty=0  from mom_orderdetail a   (nolock)  \r\n";
                selectSQL += "  left join    mom_morder b  (nolock)on a.MoId=b.MoId  and a.ModId=b.ModId   \r\n";
                selectSQL += "  left join    mom_order c  (nolock) on a.MoId=c.MoId   \r\n";
                selectSQL += "  left join    Department d  (nolock) on d.cDepCode=a.MDeptCode   \r\n";
                selectSQL += "  left join " + UFsystem + "..UA_User u  (nolock) on c.CreateUser=u.cUser_ID  \r\n";
                selectSQL += "  where  1=1  \r\n";
                if (cInvCode.Length > 0)
                {
                    selectSQL += " and a.InvCode='" + cInvCode + "' \r\n";

                }
                else
                {
                    if (iKeys.Length > 0)
                    {
                        selectSQL += "  and exists (select 1 from " + iKeys + "  keys where a.InvCode=keys.cInvCode) \r\n";
                    }
                }
                if (TimeRange)
                {
                    selectSQL += " and b.StartDate>='" + MoStartDateL + "' and b.StartDate<='" + MoStartDateH + "' ";
                }
                else
                {
                    if (cDateL.Length > 0)
                    {
                        selectSQL += " and b.StartDate>='" + cDateL + "' ";
                    }
                    if (cDateH.Length > 0)
                    {
                        selectSQL += " and b.StartDate<='" + cDateH + "' ";
                    }
                }
                selectSQL += " and a.Status=3 and len(isnull(a.CloseUser,''))=0 and (isnull(a.qty,0)-isnull(a.QualifiedInQty,0))>0      \r\n";
                selectSQL += "      \r\n";
                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();
            }
            selectSQL = "  ----*销售订单********     \r\n";
            selectSQL += "   \r\n";
            selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
            selectSQL += "  select '' as 'Prod_cInvCode',DocTypeNo=6,'销售订单' as  'DocType',b.cSOCode+'('+convert(varchar,a.iRowNo)+')' as 'cCode',person.cpersonname,cDefine30=convert(varchar,a.iSOsID),b.dDate,a.cInvCode,moQty=0,Now_PurArrQty=0,Now_PurQty=0,CurSotckQty=0,useQty=isnull(a.iQuantity,0)-isnull(a.iFHQuantity,0), toArrQty=0,AltmQty=0,OsQty=0   \r\n";
            selectSQL += "  from  SO_SODetails a (nolock) left join SO_SOMain b (nolock) on a.ID =b.ID   \r\n";
            selectSQL += "  left join person  (nolock) on b.cPersonCode= person.cpersoncode  \r\n";
            selectSQL += "  where 1=1  \r\n";
            if (cInvCode.Length > 0)
            {
                selectSQL += " and a.cInvCode='" + cInvCode + "' \r\n";
            }
            else
            {
                if (iKeys.Length > 0)
                {
                    selectSQL += "  and exists (select 1 from " + iKeys + "  keys where a.cInvCode=keys.cInvCode) \r\n";
                }
            }
            if (TimeRange)
            {
                selectSQL += " and b.dDate>='" + SoDateL + "' and b.dDate<='" + SoDateH + "' ";
            }
            else
            {
                if (cDateL.Length > 0)
                {
                    selectSQL += " and b.dDate>='" + cDateL + "' ";
                }
                if (cDateH.Length > 0)
                {
                    selectSQL += " and b.dDate<='" + cDateH + "' ";
                }
            }
            selectSQL += "  and isnull(b.iStatus,0)=1 and (isnull(a.iQuantity,0)-isnull(a.iFHQuantity,0))>0 and len((isnull(a.cSCloser,'')+isnull(b.cCloser,'')))=0   \r\n";
            selectSQL += "   \r\n";

            myCommand.CommandText = selectSQL;
            myCommand.ExecuteNonQuery();



            selectSQL = "  ----*预测订单********       \r\n";
            selectSQL += "      \r\n";
            selectSQL += "  insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)    \r\n";
            selectSQL += "   select '' as 'Prod_cInvCode',DocTypeNo=13,'预测订单' as  'DocType',b.cCode +'('+convert(varchar,a.iRowNo)+')' as 'cCode',person.cpersonname,cDefine30=convert(varchar,a.autoid),b.dDate,a.cInvCode,moQty=0,Now_PurArrQty=0,Now_PurQty=0,CurSotckQty=0,useQty=isnull(a.iQuantity,0)-isnull(a.fdhquantity,0), toArrQty=0,AltmQty=0,OsQty=0     \r\n";
            selectSQL += "   from  SA_PreOrderDetails a (nolock) left join SA_PreOrderMain b (nolock) on a.ID =b.ID     \r\n";
            selectSQL += "   left join person  (nolock) on b.cPersonCode= person.cpersoncode    \r\n";
            selectSQL += "   where 1=1    \r\n";
            if (cInvCode.Length > 0)
            {
                selectSQL += " and a.cInvCode='" + cInvCode + "' \r\n";
            }
            else
            {
                if (iKeys.Length > 0)
                {
                    selectSQL += "  and exists (select 1 from " + iKeys + "  keys where a.cInvCode=keys.cInvCode) \r\n";
                }
            }
            if (TimeRange)
            {
                selectSQL += " and b.dDate>='" + SoDateL + "' and b.dDate<='" + SoDateH + "' ";
            }
            else
            {
                if (cDateL.Length > 0)
                {
                    selectSQL += " and b.dDate>='" + cDateL + "' ";
                }
                if (cDateH.Length > 0)
                {
                    selectSQL += " and b.dDate<='" + cDateH + "' ";
                }
            }
            selectSQL += "   and len(isnull(b.cVerifier ,''))>0 and (isnull(a.iQuantity,0)-isnull(a.fdhquantity,0))>0 and len((isnull(a.cSCloser,'')+isnull(b.cCloser,'')))=0     \r\n";
            selectSQL += "   \r\n";

            myCommand.CommandText = selectSQL;
            myCommand.ExecuteNonQuery();

            selectSQL = "   \r\n";
            selectSQL += "  ----*生产订单(需求)********     \r\n";
            selectSQL += " insert into " + ComprehensiveStockST31 + "(Prod_cInvCode,DocTypeNo,DocType,cCode,cpersonname,cDefine30,dDate,cinvcode,moQty,Now_PurArrQty,Now_PurQty,CurSotckQty,useQty,toArrQty,AltmQty,OsQty)  \r\n";
            selectSQL += "  select t.InvCode as 'Prod_cInvCode',DocTypeNo=7,'生产订单（需求）' as  'DocType', m.mocode as 'cCode',u.cUser_Name  as 'cpersonname',isnull(t.SoCode,'')+'('+convert(varchar(8),isnull(t.sortseq,0))+')' as 'cDefine30', mm.StartDate as 'dDate',     \r\n";
            selectSQL += "  c.invcode as 'cInvCode',moQty=0,Now_PurArrQty=0,Now_PurQty=0,CurSotckQty=0,useQty=isnull(c.qty,0)-isnull(c.IssQty,0), toArrQty=0 ,AltmQty=0,OsQty=0   \r\n";
            selectSQL += "  from  mom_moallocate c  (nolock)  \r\n";
            selectSQL += "  left join mom_orderdetail t (nolock) on c.modid=t.modid    \r\n";
            selectSQL += "  left join mom_order m  (nolock) on m.moid=t.moid    \r\n";
            selectSQL += "  left join mom_morder mm (nolock) on mm.modid=t.modid and t.ModId=mm.ModId  \r\n";
            selectSQL += "  left join " + UFsystem + "..UA_User u  (nolock) on m.CreateUser=u.cUser_ID  \r\n";
            selectSQL += "   where 1=1  \r\n";
            if (cInvCode.Length > 0)
            {
                selectSQL += " and c.InvCode='" + cInvCode + "'  \r\n";
            }
            else
            {
                if (iKeys.Length > 0)
                {
                    selectSQL += "  and exists (select 1 from " + iKeys + "  keys where c.InvCode=keys.cInvCode) \r\n";
                }
            }
            if (TimeRange)
            {
                selectSQL += " and mm.StartDate>='" + MoStartDateL + "' and mm.StartDate<='" + MoStartDateH + "' ";
            }
            else
            {
                if (cDateL.Length > 0)
                {
                    selectSQL += " and mm.StartDate>='" + cDateL + "' ";
                }
                if (cDateH.Length > 0)
                {
                    selectSQL += " and mm.StartDate<='" + cDateH + "' ";
                }
            }
            selectSQL += "  and  t.Status=3 and len(isnull(t.CloseUser,''))=0 and (isnull(c.qty,0)-isnull(c.IssQty,0))>0   \r\n";
            selectSQL += "   \r\n";
            selectSQL += "   \r\n";
            myCommand.CommandText = selectSQL;
            myCommand.ExecuteNonQuery();


            selectSQL = " CREATE INDEX ComprehensiveStockST31" + iLoginEx.GetMacAddress().Replace(":", "") + "_idx ON " + ComprehensiveStockST31 + "   \r\n";
            selectSQL += " (  \r\n";
            selectSQL += " 	DocTypeNo,cInvCode  \r\n";
            selectSQL += " ) ; \r\n";

            myCommand.CommandText = selectSQL;
            myCommand.ExecuteNonQuery();

            if (myConn.State == System.Data.ConnectionState.Open)
            {
                myConn.Close();
            }

            return ComprehensiveStockST31;
        }

    }
}
