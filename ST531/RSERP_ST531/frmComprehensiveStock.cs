using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using RSERP;
using UTLoginEx;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Reflection;

namespace RSERP_ST531
{
    public partial class frmComprehensiveStockInfo : Form
    {
        private int FormWidth = 0, FormHeight = 0, tabPageWidth = 0, tabPageHeight = 0, dataGridViewWidth = 0, dataGridViewHeight = 0, dataGridViewWidth2 = 0, dataGridViewHeight2 = 0, tab3_dataGridView1Width = 0, tab3_dataGridView1Height = 0, tab4_dataGridView1Width = 0, tab4_dataGridView1Height = 0, tab6_dataGridView1Height = 0, tab6_dataGridView1Width = 0;
        private UTLoginEx.LoginEx iLoginEx = new LoginEx();
        private RSERP.ComprehensiveStock wComprehensiveStock = new ComprehensiveStock();
        private Ini ini = null;
        private bool CellMouseDown = false;
        private int tab5_dataGridView1Width = 0, tab5_dataGridView1Height = 0;
        private short mproType = 0;
        private string mTitle = "";
        private int mOutMonth = 0;
        private string cWhCode = "";
        private string cInvCode = "";
        private int AuthID = 18;//综合库存权限ID
        private bool Tab2_SaveCulomnsWidth = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="proType">0=综合查询；1=呆料预警</param>
        public frmComprehensiveStockInfo(string[] args, short proType)
        {
            InitializeComponent();
            //18:库存综合查询
            iLoginEx.Initialize(args, AuthID);//必须先初始化LoginEx

            mproType = proType;
            SLbAccID.Text = iLoginEx.AccID();
            SLbAccName.Text = iLoginEx.AccName();
            SLbServer.Text = iLoginEx.DBServerHost();
            SLbYear.Text = iLoginEx.iYear();
            SLbUser.Text = iLoginEx.UserId() + "[" + iLoginEx.UserName() + "]";
            SLBLoginDate.Text = iLoginEx.LoginDate();

        }

        private void MRPTempStok()
        {
            try
            {
                frmTempStockQuery ftStockQuery = new frmTempStockQuery();
                ftStockQuery.ShowDialog(this);

                if (ftStockQuery.GetSQL().Length > 0)
                {

                    OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                    if (myConn.State == System.Data.ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                    myConn.Open();


                    OleDbCommand myCommand = new OleDbCommand(ftStockQuery.GetSQL(), myConn);
                    myCommand.ExecuteNonQuery();

                    this.tab6_dataGridView1.AutoGenerateColumns = true;
                    //设置数据源    


                    DataSet ds = new DataSet();
                    OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                    da.Fill(ds);
                    this.tab6_dataGridView1.DataSource = ds.Tables[0];//数据源 


                    //标准居中
                    this.tab6_dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    //设置自动换行

                    this.tab6_dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    //设置自动调整高度

                    this.tab6_dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    if (myConn.State == System.Data.ConnectionState.Open)
                    {
                        myConn.Close();
                    }


                    for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
                    {
                        tab6_dataGridView1.Columns[i].ReadOnly = true;

                    }


                    for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            tab6_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;
                        }
                    }

                    tab6_dataGridView1.Columns[5].DefaultCellStyle.Format = "#,###0";
                    tab6_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    tab6_dataGridView1.Columns[6].DefaultCellStyle.Format = "#,###0.00";
                    tab6_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    tab6_dataGridView1.Columns[7].DefaultCellStyle.Format = "#,###0";
                    tab6_dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                }

            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "MRPTempStok()");
                frmmsg.ShowDialog(this);
            }
        }

        private void StokWarning()
        {
            try
            {
                frmQuery fQuery = new frmQuery(iLoginEx);
                fQuery.ShowDialog(this);

                if (fQuery.GetSQL().Length > 0)
                {
                    mOutMonth = fQuery.OutMonth();
                    cWhCode = fQuery.WhCode();

                    OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                    if (myConn.State == System.Data.ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                    myConn.Open();


                    OleDbCommand myCommand = new OleDbCommand(fQuery.GetSQL(), myConn);
                    myCommand.ExecuteNonQuery();

                    this.tab5_dataGridView1.AutoGenerateColumns = true;
                    //设置数据源    


                    DataSet ds = new DataSet();
                    OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                    da.Fill(ds);
                    this.tab5_dataGridView1.DataSource = ds.Tables[0];//数据源 


                    //标准居中
                    this.tab5_dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    //设置自动换行

                    this.tab5_dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    //设置自动调整高度

                    this.tab5_dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    if (myConn.State == System.Data.ConnectionState.Open)
                    {
                        myConn.Close();
                    }


                    for (int i = 0; i < tab5_dataGridView1.Columns.Count; i++)
                    {
                        tab5_dataGridView1.Columns[i].ReadOnly = true;

                    }


                    for (int i = 0; i < tab5_dataGridView1.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            tab5_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                        }
                    }

                    tab5_dataGridView1.Columns[0].Width = 120;
                    tab5_dataGridView1.Columns[1].Width = 200;
                    tab5_dataGridView1.Columns[2].Width = 280;

                    tab5_dataGridView1.Columns[3].DefaultCellStyle.Format = "#,###0";
                    tab5_dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    tab5_dataGridView1.Columns[4].DefaultCellStyle.Format = "#,###0.00";
                    tab5_dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    tab5_dataGridView1.Columns[5].DefaultCellStyle.Format = "#,###0";
                    tab5_dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    tab5_dataGridView1.Columns[6].DefaultCellStyle.Format = "#,###0.00";
                    tab5_dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    Tab2_SaveCulomnsWidth = true;

                    for (int i = 7; i < tab5_dataGridView1.Columns.Count; i++)
                    {
                        tab5_dataGridView1.Columns[i].DefaultCellStyle.Format = "#,###0";
                        tab5_dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    //读取用户自定列宽

                    if (tab5_dataGridView1.Columns.Count > 0)
                    {

                        string ColumnsWidths = iLoginEx.ReadUserProfileValue("tab5StokWarning", "ColumnsWidths");
                        string[] ColumnsWidthsPara = ColumnsWidths.Split(';');
                        for (int i = 0; i < ColumnsWidthsPara.Length && i < tab5_dataGridView1.Columns.Count; i++)
                        {
                            if (ColumnsWidthsPara[i].Length > 0)
                            {
                                tab5_dataGridView1.Columns[i].Width = Convert.ToInt32(ColumnsWidthsPara[i]);
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "StokWarning()");
                frmmsg.ShowDialog(this);
            }
        }
        /// <summary>
        /// 库存明细
        /// </summary>  
        private void DedtailQuery(bool showAllData, string DocTypeNo)
        {

            try
            {

                this.Text = mTitle + "   正在查询，请稍候...";
                System.Windows.Forms.Application.DoEvents();

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                string mySelectQuery = "";
                if (showAllData)
                {

                    mySelectQuery = " select   a.DocType as '数据类别', a.cCode as '单号',a.cpersonname as '经办人',a.cDefine30 as '源单号',a.dDate as '日期',a.Prod_cInvCode as '产品编码', pr.cInvName as '产品名称',replace(replace(pr.cinvstd,'''',''),'\"','')  as '产品型号', a.cinvcode as '物料编码',p.cInvName as '物料名称',  \r\n";
                    mySelectQuery += " replace(replace(p.cinvstd,'''',''),'\"','') as '物料规格',p.cInvDefine7 as '图号版次',a.moQty as '在制数', a.Now_PurQty as '采购在途', 0 as '即将到货',a.Now_PurArrQty as '到货在检量',a.CurSotckQty as '现存量',a.AltmQty as '代用料' ,a.useQty as '已分配量'  \r\n";

                }
                else
                {
                    mySelectQuery = "select  a.DocType as '数据类别', a.cCode as '单号',a.cpersonname as '经办人',a.cDefine30 as '源单号',a.dDate as '日期',a.Prod_cInvCode as '产品编码', pr.cInvName as '产品名称',replace(replace(pr.cinvstd,'''',''),'\"','')  as '产品型号', a.cinvcode as '物料编码',p.cInvName as '物料名称',  \r\n";
                    mySelectQuery += " replace(replace(p.cinvstd,'''',''),'\"','') as '物料规格',p.cInvDefine7 as '图号版次',(a.moQty+a.Now_PurQty+a.Now_PurArrQty+a.CurSotckQty+a.useQty+a.AltmQty) as '数量'  \r\n";
                }




                mySelectQuery += "   from  \r\n";
                if (DocTypeNo == "8")
                {
                    mySelectQuery += wComprehensiveStock.ComprehensiveStockInfo(iLoginEx, 3, tab1_cInvCodeL.Text, "", "", iLoginEx.pubDB_UF(), cWhCode, "");
                }
                else
                {
                    mySelectQuery += wComprehensiveStock.ComprehensiveStockInfo(iLoginEx, 0, "", "", "", iLoginEx.pubDB_UF(), cWhCode, "");
                }
                mySelectQuery += " a left join inventory p  (nolock ) on a.cinvcode=p.cinvcode  left  join  inventory pr  (nolock ) on  a.Prod_cInvCode=pr.cinvcode  where 1=1 \r\n";


                if (DocTypeNo != "8")
                {
                    tab1_cinvCodeAny.Text = tab1_cinvCodeAny.Text.Trim();
                    tab1_cinvCodeAny.Text = tab1_cinvCodeAny.Text.Replace("；", ";");

                    if (tab1_cinvCodeAny.Text.Length > 0)
                    {
                        string cinvCodeChild = "";
                        string[] paraCinvCode = tab1_cinvCodeAny.Text.Split(';');
                        if (paraCinvCode.Length > 0)
                        {
                            for (int i = 0; i < paraCinvCode.Length; i++)
                            {
                                cinvCodeChild += "'" + paraCinvCode[i].ToString() + "',";
                            }

                            mySelectQuery += " and  a.cinvcode in (" + cinvCodeChild + "'\r\n'" + ") \r\n";
                        }
                        else
                        {
                            mySelectQuery += " and  a.cinvcode ='" + tab1_cinvCodeAny.Text + "' \r\n";
                        }
                    }
                    else
                    {
                        if (tab1_cInvCodeL.Text.Length > 0 && tab1_cInvCodeH.Text.Length == 0)
                        {
                            mySelectQuery += "  and  a.cinvcode ='" + tab1_cInvCodeL.Text + "' \r\n";
                        }
                        else if (tab1_cInvCodeL.Text.Length == 0 && tab1_cInvCodeH.Text.Length > 0)
                        {
                            mySelectQuery += "  and  a.cinvcode ='" + tab1_cInvCodeH.Text + "' \r\n";
                        }
                        else if (tab1_cInvCodeL.Text.Length > 0 && tab1_cInvCodeH.Text.Length > 0)
                        {
                            mySelectQuery += "  and  a.cinvcode between  '" + tab1_cInvCodeL.Text + "' and '" + tab1_cInvCodeH.Text + "'  \r\n";
                        }
                    }
                }
                if (DocTypeNo.Length > 0)
                {
                    mySelectQuery += "  and  a.DocTypeNo in (" + DocTypeNo + ")";
                }

                if (!showAllData)
                {
                    mySelectQuery += "  order by a.cinvcode,a.dDate,a.cCode   \r\n";
                }
                else
                {
                    mySelectQuery += "  order by a.cinvcode,a.DocTypeNo  ,a.dDate,a.cCode   \r\n";
                }


                OleDbCommand myCommand = new OleDbCommand(mySelectQuery, myConn);
                this.tab4_dataGridView1.AutoGenerateColumns = true;
                //设置数据源    


                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                da.Fill(ds);

                this.tab1_dataGridView1.DataSource = ds.Tables[0];//数据源 


                //标准居中
                this.tab1_dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //设置自动换行

                this.tab1_dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //设置自动调整高度

                this.tab1_dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }


                for (int i = 0; i < tab1_dataGridView1.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        tab1_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                    }
                }

                for (int i = 0; i < tab1_dataGridView1.Columns.Count; i++)
                {
                    tab1_dataGridView1.Columns[i].ReadOnly = true;

                }

                if (!showAllData)
                {
                    if (tab1_dataGridView1.Columns.Count > 0)
                    {
                        tab1_dataGridView1.Columns[12].DefaultCellStyle.Format = "#,###0";
                        tab1_dataGridView1.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                else
                {
                    for (int i = 12; i < tab1_dataGridView1.Columns.Count; i++)
                    {
                        tab1_dataGridView1.Columns[i].DefaultCellStyle.Format = "#,###0";
                        tab1_dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }


                this.Text = mTitle + "   查询完成！共" + (tab1_dataGridView1.RowCount).ToString() + "行";
                System.Windows.Forms.Application.DoEvents();
            }

            catch (Exception ex)
            {
                this.Text = mTitle;
                MessageBox.Show(this, ex.ToString(), "DedtailQuery()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }




        private void tab2_dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0)
                {
                    return;
                }


                if (e.RowIndex < 0)
                {
                    return;
                }

                if (tab2_dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Length == 0)
                {
                    return;
                }
                else
                {

                    tab1_dataGridView1.DataSource = null;
                    tab1_cInvCodeL.Text = tab2_dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    tab1_cInvCodeH.Text = tab1_cInvCodeL.Text;
                    if (e.ColumnIndex >= 0 && e.ColumnIndex <= 4)
                    {
                        tabControl1.SelectedIndex = 1;
                        DedtailQuery(true, "");
                        this.Text = mTitle;
                    }
                    switch (e.ColumnIndex)
                    {

                        case 8:
                            {
                                tabControl1.SelectedIndex = 1;
                                DedtailQuery(chkShowAll.Checked, "3");//在检
                                this.Text = mTitle + "  到货在检量";
                                break;
                            }
                        case 6:
                            {
                                tabControl1.SelectedIndex = 1;
                                DedtailQuery(chkShowAll.Checked, "1,2");//采购在途
                                this.Text = mTitle + "  采购在途";
                                break;
                            }
                        case 9:
                            {
                                tabControl1.SelectedIndex = 1;
                                DedtailQuery(chkShowAll.Checked, "4");//现存量
                                this.Text = mTitle + "  现存量";
                                break;
                            }
                        case 10:
                            {
                                tabControl1.SelectedIndex = 1;
                                DedtailQuery(chkShowAll.Checked, "8");//替代料
                                this.Text = mTitle + "  替代料";
                                break;
                            }
                        case 5:
                            {
                                tabControl1.SelectedIndex = 1;
                                DedtailQuery(chkShowAll.Checked, "5");//在制
                                this.Text = mTitle + "  在制";
                                break;
                            }
                        case 12:
                            {
                                tabControl1.SelectedIndex = 1;
                                DedtailQuery(chkShowAll.Checked, "6,7");//已分配量
                                this.Text = mTitle + "  已分配量";
                                break;
                            }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "tab2_dataGridView1_CellMouseDoubleClick()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 现存量查询
        /// </summary>
        private void CurrentStockQuery()
        {
            try
            {

                //txtcInvCCode.Text = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCCode");//物料分类
                //txtcWhCode.Text = iLoginEx.ReadUserProfileValue("StockQuery", "cWhCode");//仓库代码
                //txtcInvCode.Text = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode");//物料编码
                //txtQtyL.Text = iLoginEx.ReadUserProfileValue("StockQuery", "QtyL");//数量L 
                //txtQtyH.Text = iLoginEx.ReadUserProfileValue("StockQuery", "QtyH");//数量H
                //txtPriceL.Text = iLoginEx.ReadUserProfileValue("StockQuery", "PriceL");//单价L
                //txtPriceH.Text = iLoginEx.ReadUserProfileValue("StockQuery", "PriceH");//单价H
                //txtAmtL.Text = iLoginEx.ReadUserProfileValue("StockQuery", "AmtL");//库存金额L
                //txtAmtH.Text = iLoginEx.ReadUserProfileValue("StockQuery", "AmtH");//库存金额H
                //cmbQureyType.SelectedIndex = iLoginEx.ReadUserProfileValue("StockQuery", "QureyType").Length == 0 ? 0 : Convert.ToInt32(iLoginEx.ReadUserProfileValue("StockQuery", "QureyType"));//0=现存量；1=综合查询
                //cmbABC.SelectedIndex = iLoginEx.ReadUserProfileValue("StockQuery", "InvABC").Length == 0 ? 0 : Convert.ToInt32(iLoginEx.ReadUserProfileValue("StockQuery", "InvABC"));//ABC分类

                tab2_dataGridView2.Visible = true;
                tab2_dataGridView1.Visible = false;

                this.Text = mTitle + "   正在查询，请稍候...";
                System.Windows.Forms.Application.DoEvents();
                string cInvCCode = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCCode");
                if (cInvCCode.IndexOf(' ') != -1)
                {
                    cInvCCode = cInvCCode.Substring(0, cInvCCode.IndexOf(' '));
                }

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                OleDbCommand myCommand = new OleDbCommand("", myConn);
                myCommand.CommandTimeout = 600;

                string selectSQL = "";



                selectSQL = "select  \r\n";
                selectSQL += " case when isnull(i.iInvRCost,0)>=1 then 'A类' else  \r\n";
                selectSQL += " case when isnull(i.iInvRCost,0)>=0.5 then 'B类' else  \r\n";
                selectSQL += " case when isnull(i.iInvRCost,0)>=0.1 then 'C类' else 'D类' end end end as'ABC分类',  \r\n";
                selectSQL += "  a.cWhCode+'-'+w.cWhName as '仓库',a.cBatch as '批次',i.cInvCCode+'-'+cls.cInvCName as '物料分类',a.cInvCode as'物料编码',i.cInvName as '名称',i.cInvStd as '规格型号',a.iQuantity as '现存量' \r\n";
                if (iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Price))
                {
                    selectSQL += "  ,isnull(i.iInvRCost,0) as '单位成本',a.iQuantity*isnull(i.iInvRCost,0) as '库存成本金额' \r\n";
                }
                selectSQL += "  from  CurrentStock a (nolock) left join Warehouse w (nolock)  \r\n";
                selectSQL += "  on a.cWhCode=w.cWhCode  \r\n";
                selectSQL += "  left join Inventory i(nolock) on a.cInvCode=i.cInvCode  \r\n";
                selectSQL += "  left join  InventoryClass cls (nolock) on i.cInvCCode =cls.cInvCCode  \r\n";
                selectSQL += "  where 1=1 \r\n";

                if (cInvCCode.Length > 0)
                {
                    selectSQL += "  and i.cInvCCode='" + cInvCCode + "' \r\n";
                }
                if (iLoginEx.ReadUserProfileValue("StockQuery", "QtyL").Length > 0)
                {
                    selectSQL += "  and a.iQuantity>=" + iLoginEx.ReadUserProfileValue("StockQuery", "QtyL") + " \r\n";
                }
                

                if (iLoginEx.ReadUserProfileValue("StockQuery", "QtyH").Length > 0)
                {
                    selectSQL += "  and a.iQuantity<=" + iLoginEx.ReadUserProfileValue("StockQuery", "QtyH") + " \r\n";
                }

                if (iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Price))//只单价查询权限的，才可以查金额和单
                {
                    if (iLoginEx.ReadUserProfileValue("StockQuery", "PriceL").Length > 0)
                    {
                        selectSQL += "  and isnull(i.iInvRCost,0)>=" + iLoginEx.ReadUserProfileValue("StockQuery", "PriceL") + " \r\n";
                    }

                    if (iLoginEx.ReadUserProfileValue("StockQuery", "PriceH").Length > 0)
                    {
                        selectSQL += "  and isnull(i.iInvRCost,0)<=" + iLoginEx.ReadUserProfileValue("StockQuery", "PriceH") + " \r\n";
                    }

                    if (iLoginEx.ReadUserProfileValue("StockQuery", "AmtL").Length > 0)
                    {
                        selectSQL += "  and a.iQuantity*isnull(i.iInvRCost,0)>=" + iLoginEx.ReadUserProfileValue("StockQuery", "AmtL") + " \r\n";
                    }

                    if (iLoginEx.ReadUserProfileValue("StockQuery", "AmtH").Length > 0)
                    {
                        selectSQL += "  and a.iQuantity*isnull(i.iInvRCost,0)<=" + iLoginEx.ReadUserProfileValue("StockQuery", "AmtH") + " \r\n";
                    }
                }
                switch (Convert.ToInt32(iLoginEx.ReadUserProfileValue("StockQuery", "InvABC")))
                {
                    case 1://A
                        {
                            selectSQL += "  and isnull(i.iInvRCost,0)>=1 \r\n";
                            break;
                        }
                    case 2://B
                        {
                            selectSQL += "  and isnull(i.iInvRCost,0)>=0.5 and isnull(i.iInvRCost,0)<1 \r\n";
                            break;
                        }
                    case 3://C
                        {
                            selectSQL += "  and isnull(i.iInvRCost,0)>=0.1 and isnull(i.iInvRCost,0)<0.5 \r\n";
                            break;
                        }
                    case 4://E
                        {
                            selectSQL += "  and isnull(i.iInvRCost,0)<0.1  \r\n";
                            break;
                        }
                }
                //仓库
                string cinvCodeChild = "";
                string[] paraCinvCode = null;
                if (iLoginEx.ReadUserProfileValue("StockQuery", "cWhCode").Length > 0)
                {
                    paraCinvCode = iLoginEx.ReadUserProfileValue("StockQuery", "cWhCode").Split(';');
                    if (paraCinvCode.Length > 0)
                    {
                        for (int i = 0; i < paraCinvCode.Length; i++)
                        {
                            if (paraCinvCode[i].ToString().Length > 0)
                            {
                                cinvCodeChild += "'" + paraCinvCode[i].ToString() + "', ";
                            }
                        }

                        cinvCodeChild += iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r";
                        selectSQL += " and  a.cWhCode in (" + cinvCodeChild.Replace(", " + iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r", "") + " ) ";
                    }
                    else
                    {
                        selectSQL += " and  a.cWhCode ='" + iLoginEx.ReadUserProfileValue("StockQuery", "cWhCode") + "' ";
                    }
                }

                //物料编码
                cinvCodeChild = "";
                paraCinvCode = null;
                if (iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode").Length > 0)
                {
                    paraCinvCode = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode").Split(';');
                    if (paraCinvCode.Length > 0)
                    {
                        for (int i = 0; i < paraCinvCode.Length; i++)
                        {
                            if (paraCinvCode[i].ToString().Length > 0)
                            {
                                cinvCodeChild += "'" + paraCinvCode[i].ToString() + "', ";
                            }
                        }

                        cinvCodeChild += iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r";
                        selectSQL += " and  a.cInvCode in (" + cinvCodeChild.Replace(", " + iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r", "") + " ) ";
                    }
                    else
                    {
                        selectSQL += " and  a.cInvCode ='" + iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode") + "' ";
                    }
                }



                myCommand.CommandText = selectSQL + " order by a.cWhCode,a.cBatch,i.cInvCCode,a.cInvCode";

                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                da.Fill(ds);

                tab2_dataGridView2.DataSource = null;

                this.tab2_dataGridView2.DataSource = ds.Tables[0];//数据源 

                this.tab2_dataGridView2.AutoGenerateColumns = true;
                //设置数据源    


                //标准居中
                this.tab2_dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //设置自动换行

                this.tab2_dataGridView2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //设置自动调整高度

                this.tab2_dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


                for (int i = 0; i < tab2_dataGridView2.Columns.Count; i++)
                {
                    tab2_dataGridView2.Columns[i].ReadOnly = true;
                }
                tab2_dataGridView2.Columns["现存量"].DefaultCellStyle.Format = "#,###0";
                tab2_dataGridView2.Columns["现存量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                if (iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Price))
                {
                    tab2_dataGridView2.Columns["单位成本"].DefaultCellStyle.Format = "#,###0";
                    tab2_dataGridView2.Columns["单位成本"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    tab2_dataGridView2.Columns["库存成本金额"].DefaultCellStyle.Format = "#,###0";
                    tab2_dataGridView2.Columns["库存成本金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                for (int i = 0; i < tab2_dataGridView2.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        tab2_dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                    }
                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }


                this.Text = mTitle + "   查询完成！共" + (tab2_dataGridView2.RowCount).ToString() + "行";
                System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "CurrentStockQuery()");
                frmmsg.ShowDialog(this);
            }
        }


        /// <summary>
        /// 显示综合库存
        /// </summary>
        private void CompreStok()
        {
            try
            {

                //txtcInvCCode.Text = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCCode");//物料分类
                //txtcWhCode.Text = iLoginEx.ReadUserProfileValue("StockQuery", "cWhCode");//仓库代码
                //txtcInvCode.Text = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode");//物料编码
                //txtQtyL.Text = iLoginEx.ReadUserProfileValue("StockQuery", "QtyL");//数量L 
                //txtQtyH.Text = iLoginEx.ReadUserProfileValue("StockQuery", "QtyH");//数量H
                //txtPriceL.Text = iLoginEx.ReadUserProfileValue("StockQuery", "PriceL");//单价L
                //txtPriceH.Text = iLoginEx.ReadUserProfileValue("StockQuery", "PriceH");//单价H
                //txtAmtL.Text = iLoginEx.ReadUserProfileValue("StockQuery", "AmtL");//库存金额L
                //txtAmtH.Text = iLoginEx.ReadUserProfileValue("StockQuery", "AmtH");//库存金额H
                //cmbQureyType.SelectedIndex = iLoginEx.ReadUserProfileValue("StockQuery", "QureyType").Length == 0 ? 0 : Convert.ToInt32(iLoginEx.ReadUserProfileValue("StockQuery", "QureyType"));//0=现存量；1=综合查询
                //cmbABC.SelectedIndex = iLoginEx.ReadUserProfileValue("StockQuery", "InvABC").Length == 0 ? 0 : Convert.ToInt32(iLoginEx.ReadUserProfileValue("StockQuery", "InvABC"));//ABC分类

                tab2_dataGridView2.Visible = false;
                tab2_dataGridView1.Visible = true;

                this.Text = mTitle + "   正在查询，请稍候...";
                System.Windows.Forms.Application.DoEvents();

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                iLoginEx.WriteUserProfileValue("ST531TAB2", "InCludeSeachKeys", txtInCludeKeys.Text);
                iLoginEx.WriteUserProfileValue("ST531TAB2", "ExCludeSeachKeys", txtExCludeKeys.Text);

                string cinvCodeChild = "", ClassChild = "";
                string[] paraCinvCode = null;
                string InCludeKeys = "", ExCludeKeys = "";

                txtInCludeKeys.Text = txtInCludeKeys.Text.Trim();
                txtInCludeKeys.Text = txtInCludeKeys.Text.Replace("\r", "");
                txtInCludeKeys.Text = txtInCludeKeys.Text.Replace("\n", "");
                txtInCludeKeys.Text = txtInCludeKeys.Text.Replace("；", ";");


                if (txtInCludeKeys.Text.Length > 0)
                {
                    cinvCodeChild = "";
                    ClassChild = "";
                    paraCinvCode = txtInCludeKeys.Text.Split(';');
                    if (paraCinvCode.Length > 0)
                    {
                        for (int i = 0; i < paraCinvCode.Length; i++)
                        {
                            if (paraCinvCode[i].ToString().Length > 0)
                            {
                                cinvCodeChild += "  left(a.cInvCode," + paraCinvCode[i].ToString().Length + ") ='" + paraCinvCode[i].ToString() + "' or ";
                                ClassChild += "  left(cls.cInvCCode," + paraCinvCode[i].ToString().Length + ") ='" + paraCinvCode[i].ToString() + "' or ";
                            }
                        }

                        cinvCodeChild += iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r";
                        InCludeKeys = " and ((" + cinvCodeChild.Replace("or " + iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r", "") + " ) ";

                        ClassChild += iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r";
                        InCludeKeys += " or (" + ClassChild.Replace("or " + iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r", "") + " ) )";
                    }
                    else
                    {
                        InCludeKeys = " and ( ( left(a.cInvCode," + txtInCludeKeys.Text.Length + ") ='" + txtInCludeKeys.Text + "' ) ";
                        InCludeKeys += " or ( left(cls.cInvCCode," + txtInCludeKeys.Text.Length + ") ='" + txtInCludeKeys.Text + "' ) )";
                    }
                }
                else
                {
                    InCludeKeys = "";
                }


                cinvCodeChild = "";
                paraCinvCode = null;

                txtExCludeKeys.Text = txtExCludeKeys.Text.Trim();
                txtExCludeKeys.Text = txtExCludeKeys.Text.Replace("\r", "");
                txtExCludeKeys.Text = txtExCludeKeys.Text.Replace("\n", "");
                txtExCludeKeys.Text = txtExCludeKeys.Text.Replace("；", ";");


                if (txtExCludeKeys.Text.Length > 0)
                {
                    cinvCodeChild = "";
                    ClassChild = "";
                    paraCinvCode = txtExCludeKeys.Text.Split(';');
                    if (paraCinvCode.Length > 0)
                    {
                        for (int i = 0; i < paraCinvCode.Length; i++)
                        {
                            if (paraCinvCode[i].ToString().Length > 0)
                            {
                                cinvCodeChild += "  left(a.cInvCode," + paraCinvCode[i].ToString().Length + ") <>'" + paraCinvCode[i].ToString() + "' and ";
                                ClassChild += "  left(cls.cInvCCode," + paraCinvCode[i].ToString().Length + ") <>'" + paraCinvCode[i].ToString() + "' and ";
                            }
                        }

                        cinvCodeChild += iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r";
                        ExCludeKeys = " and ( (" + cinvCodeChild.Replace("and " + iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r", "") + " ) ";

                        ClassChild += iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r";
                        ExCludeKeys += " and (" + ClassChild.Replace("and " + iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r", "") + " ) )";
                    }
                    else
                    {
                        ExCludeKeys = " and ( left(a.cInvCode," + txtExCludeKeys.Text.Length + ") <>'" + txtExCludeKeys.Text + "' )";
                        ExCludeKeys += " and ( left(cls.cInvCCode," + txtExCludeKeys.Text.Length + ") <>'" + txtExCludeKeys.Text + "' )";
                    }
                }
                else
                {
                    ExCludeKeys = "";
                }


                string mySelectQuery = "";

                string cInvCCode = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCCode");
                if (cInvCCode.IndexOf(' ') != -1)
                {
                    cInvCCode = cInvCCode.Substring(0, cInvCCode.IndexOf(' '));
                }

                cInvCode = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode").IndexOf(';') > -1 ? "" : iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode");

                //合计

                mySelectQuery += "   \r\n";
                mySelectQuery += " select '合计' as  'DocType' , p.cInvCCode+'-'+cls.cInvCName as 'InvCLS','' as 'cCode',null as 'dDate','' as 'cDefine30',a.cinvcode,p.cInvName,replace(replace(p.cinvstd,'''',''),'\"','') as cInvStd,p.cInvDefine7 ,a.moQty,a.Now_PurArrQty,a.Now_PurQty,a.CurSotckQty, (isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)) as 'allSotckQty',a.useQty, a.toArrQty,a.AltmQty, \r\n";
                if (tab1_chkPurQtyState.Checked)
                {
                    //可用量=即将到货+到货在检+现存量-已分配量
                    mySelectQuery += "   (isnull(a.toArrQty,0)+isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)-isnull(a.useQty,0)) as  'AvailableQty'   from   \r\n";
                    tab2_toArrQty.HeaderText = "即将到货(A)";
                    tab2_Now_PurQty.HeaderText = "采购在途";
                }
                else
                {
                    //可用量=采购在途+到货在检+现存量-已分配量
                    mySelectQuery += "   (isnull(a.Now_PurQty,0)+isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)-isnull(a.useQty,0)) as  'AvailableQty'   from   \r\n";
                    tab2_toArrQty.HeaderText = "即将到货";
                    tab2_Now_PurQty.HeaderText = "采购在途(A)";
                }
                mySelectQuery += " (select cinvcode,sum(isnull(moQty,0)) as 'moQty',sum(isnull(Now_PurArrQty,0)) as 'Now_PurArrQty',sum(isnull(Now_PurQty,0)) as 'Now_PurQty',  sum(isnull(CurSotckQty,0)) as 'CurSotckQty',  \r\n";
                mySelectQuery += " sum(isnull(useQty,0)) as 'useQty',sum(isnull(toArrQty,0)) as 'toArrQty',sum(isnull(AltmQty,0)) as 'AltmQty'  \r\n";
                mySelectQuery += "    from " + wComprehensiveStock.ComprehensiveStockInfo(iLoginEx, 0, cInvCode, "", "", iLoginEx.pubDB_UF(), "", "") + " vw    \r\n";
                mySelectQuery += "     where  1=1 ";

                


                if (iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode").Length > 0)
                {
                    cinvCodeChild = "";
                    paraCinvCode = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode").Split(';');
                    if (paraCinvCode.Length > 0)
                    {
                        for (int i = 0; i < paraCinvCode.Length; i++)
                        {
                            cinvCodeChild += "'" + paraCinvCode[i].ToString() + "',";
                        }

                        mySelectQuery += " and  cinvcode in (" + cinvCodeChild + "'\r\n'" + ") \r\n";
                    }
                    else
                    {
                        mySelectQuery += " and  cinvcode ='" + iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode") + "' \r\n";
                    }
                }


                mySelectQuery += "  group by cinvcode) a left join inventory p on a.cinvcode=p.cinvcode    \r\n";
                mySelectQuery += "  left join  InventoryClass cls on p.cInvCCode =cls.cInvCCode  \r\n";
                mySelectQuery += "  where 1=1 " + InCludeKeys + ExCludeKeys;
                if (tab2_chkMissingOnly.Checked)
                {
                    if (tab1_chkPurQtyState.Checked)
                    {
                        //可用量=即将到货+到货在检+现存量-已分配量
                        mySelectQuery += " and   (isnull(a.toArrQty,0)+isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)-isnull(a.useQty,0)) <0 \r\n";
                    }
                    else
                    {
                        //可用量=采购在途+到货在检+现存量-已分配量
                        mySelectQuery += "  and (isnull(a.Now_PurQty,0)+isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)-isnull(a.useQty,0))  <0  \r\n";
                    }

                }

                if (cInvCCode.Length > 0)
                {
                    mySelectQuery += "  and  p.cInvCCode='" + cInvCCode + "' ";
                }

                mySelectQuery += "  order by a.cinvcode   \r\n";

                OleDbCommand myCommand = new OleDbCommand(mySelectQuery, myConn);
                this.tab2_dataGridView1.AutoGenerateColumns = false;//不自动生成列
                //设置数据源    


                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                da.Fill(ds);
                this.tab2_dataGridView1.DataSource = ds.Tables[0];//数据源 


                //标准居中
                this.tab2_dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //设置自动换行

                this.tab2_dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //设置自动调整高度

                this.tab2_dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }


                for (int i = 0; i < tab2_dataGridView1.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        tab2_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                    }
                }



                this.Text = mTitle + "   查询完成！共" + (tab2_dataGridView1.RowCount).ToString() + "行";
                System.Windows.Forms.Application.DoEvents();
            }

            catch (Exception ex)
            {
                this.Text = mTitle;
                MessageBox.Show(this, ex.ToString(), "PDBOM.btnQuery_Click()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        private void toolQuery_Click(object sender, EventArgs e)
        {

            SLBTotal.Text = "";
            string DocTypeNo = "";
            try
            {
                if (tabControl1.SelectedTab == tab5StokWarning)
                {
                    StokWarning();//呆料预警表
                }
                else if (tabControl1.SelectedTab == tab1CompreStok)//库存
                {
                    StockQuery fq = new StockQuery(iLoginEx, AuthID);
                    fq.ShowDialog(this);
                    if (fq.IsQuery)
                    {
                        if (iLoginEx.ReadUserProfileValue("StockQuery", "QureyType") == "1")
                        {
                            label3.Visible = true;
                            tab2_DateLCHK.Visible = true;
                            tab2_DateL.Visible = true;
                            label10.Visible = true;
                            tab2_DateHCHK.Visible = true;
                            tab2_DateH.Visible = true;
                            tab1_chkPurQtyState.Visible = true;
                            tab2_chkMissingOnly.Visible = true;
                            label16.Visible = true;
                            txtInCludeKeys.Visible = true;
                            label21.Visible = true;
                            txtExCludeKeys.Visible = true;
                            CompreStok();
                        }
                        else
                        {
                            label3.Visible = false;
                            tab2_DateLCHK.Visible = false;
                            tab2_DateL.Visible = false;
                            label10.Visible = false;
                            tab2_DateHCHK.Visible = false;
                            tab2_DateH.Visible = false;
                            tab1_chkPurQtyState.Visible = false;
                            tab2_chkMissingOnly.Visible = false;
                            label16.Visible = false;
                            txtInCludeKeys.Visible = false;
                            label21.Visible = false;
                            txtExCludeKeys.Visible = false;

                            CurrentStockQuery();
                            return;
                        }
                    }
                }
                else if (tabControl1.SelectedTab == tab2DedtailQuery)
                {
                    if (tab1_chkShow12.Checked)
                    {
                        DocTypeNo += "1,2,";
                    }
                    if (tab1_chkShow3.Checked)
                    {
                        DocTypeNo += "3,";
                    }
                    if (tab1_chkShow5.Checked)
                    {
                        DocTypeNo += "5,";
                    }
                    if (tab1_chkShow67.Checked)
                    {
                        DocTypeNo += "6,7,";
                    }

                    if (tab1_chkShow4.Checked)
                    {
                        DocTypeNo += "4,";
                    }
                    if (DocTypeNo.Length > 0)
                    {
                        DocTypeNo += "\r\r\n\n";
                        DedtailQuery(true, DocTypeNo.Replace(",\r\r\n\n", ""));
                    }
                    else
                    {
                        MessageBox.Show(this, "未选【采购在途】、【到货在检】、【现存量】、【在制】、【已分配量】中的任何选项", "库存明细", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if (tabControl1.SelectedTab == tab3PU_AppVouchBasis)
                {
                    PU_AppVouchBasis();
                }
                else if (tabControl1.SelectedTab == tab4mom_orderBasis)
                {
                    mom_orderBasis();
                }
                else if (tabControl1.SelectedTab == tab6MRPtempCurrentStock)
                {
                    MRPTempStok();
                }

                for (int i = 0; i < tab1_dataGridView1.Columns.Count; i++)
                {
                    tab1_dataGridView1.Columns[i].ReadOnly = true;

                }
                for (int i = 0; i < tab2_dataGridView1.Columns.Count; i++)
                {
                    tab2_dataGridView1.Columns[i].ReadOnly = true;

                }
                for (int i = 0; i < tab3_dataGridView1.Columns.Count; i++)
                {
                    tab3_dataGridView1.Columns[i].ReadOnly = true;

                }
            }
            catch (Exception ex)
            {
                this.Text = mTitle;
                MessageBox.Show(this, ex.ToString(), "PDBOM.btnQuery_Click()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                toolQuery.Enabled = true;
            }
        }
        /// <summary>
        /// 请购依据
        /// </summary>
        private void PU_AppVouchBasis()
        {
            try
            {
                string cinvCodeChild = "";
                string[] paraCinvCode = null;
                this.Text = mTitle + "   正在查询，请稍候...";
                System.Windows.Forms.Application.DoEvents();

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                OleDbCommand myCommand = new OleDbCommand(" delete PU_AppVouchs_zhrs_mrp  where AutoID not in (select AutoID from PU_AppVouchs (nolock)) \r\n", myConn);
                myCommand.ExecuteNonQuery();

                string mySelectQuery = "";
                mySelectQuery = " select b.cCode as'请购单号',b.dDate as'请购单日期',b.cMaker as'请购制单人',m.mrpdatetime as '核算时间',a.cInvCode as '物料编码',i.cInvName as'物料名称',replace(replace(i.cinvstd,'''',''),'\"','')  as '规格',a.fQuantity as'请购数',m.Now_PurQty as'采购在途',m.toArrQty as '即将到货',m.Now_PurArrQty as '到货在检',m.CurSotckQty as '现存量',\r\n";
                mySelectQuery += " '在库数'=(m.Now_PurArrQty+m.CurSotckQty),m.useQty as'已分配量','可用量'=(m.Now_PurQty+(m.Now_PurArrQty+m.CurSotckQty)-m.useQty) \r\n";
                mySelectQuery += "  from PU_AppVouchs_zhrs_mrp m(nolock)  left join   PU_AppVouchs a(nolock) on m.id=a.id and a.AutoID=m.AutoID \r\n";
                mySelectQuery += " left join  PU_AppVouch b (nolock) on a.id=b.id  \r\n";
                mySelectQuery += " left join inventory i (nolock) on i.cInvCode=a.cInvCode  where 1=1 \r\n";

                tab3_cInvCode.Text = tab3_cInvCode.Text.Trim();
                tab3_cInvCode.Text = tab3_cInvCode.Text.Replace("；", ";");

                if (tab3_cInvCode.Text.Length > 0)
                {
                    cinvCodeChild = "";
                    paraCinvCode = tab3_cInvCode.Text.Split(';');
                    if (paraCinvCode.Length > 0)
                    {
                        for (int i = 0; i < paraCinvCode.Length; i++)
                        {
                            cinvCodeChild += "'" + paraCinvCode[i].ToString() + "',";
                        }

                        mySelectQuery += " and  a.cinvcode in (" + cinvCodeChild + "'\r\n'" + ") \r\n";
                    }
                    else
                    {
                        mySelectQuery += " and  a.cinvcode ='" + tab3_cInvCode.Text + "' \r\n";
                    }
                }

                tab3_cCode.Text = tab3_cCode.Text.Trim();
                tab3_cCode.Text = tab3_cCode.Text.Replace("；", ";");

                if (tab3_cCode.Text.Length > 0)
                {
                    cinvCodeChild = "";
                    paraCinvCode = tab3_cCode.Text.Split(';');
                    if (paraCinvCode.Length > 0)
                    {
                        for (int i = 0; i < paraCinvCode.Length; i++)
                        {
                            cinvCodeChild += "'" + paraCinvCode[i].ToString() + "',";
                        }

                        mySelectQuery += " and  b.cCode in (" + cinvCodeChild + "'\r\n'" + ") \r\n";
                    }
                    else
                    {
                        mySelectQuery += " and  b.cCode ='" + tab3_cCode.Text + "' \r\n";
                    }
                }


                if (tab3_cMaker.Text.Trim().Length > 0)
                {
                    mySelectQuery += " and  b.cMaker ='" + tab3_cMaker.Text + "' \r\n";
                }

                if (tab3_dDateLCHK.Checked && tab3_dDateHCHK.Checked)
                {
                    mySelectQuery += " and  b.dDate between  '" + tab3_dDateL.Value.ToString("yyyy-MM-dd") + "' and '" + tab3_dDateH.Value.ToString("yyyy-MM-dd") + "' \r\n";
                }
                else if (tab3_dDateLCHK.Checked)
                {
                    mySelectQuery += " and  b.dDate >= '" + tab3_dDateL.Value.ToString("yyyy-MM-dd") + "'  \r\n";
                }
                else if (tab3_dDateHCHK.Checked)
                {
                    mySelectQuery += " and  b.dDate <='" + tab3_dDateH.Value.ToString("yyyy-MM-dd") + "'  \r\n";
                }

                myCommand.CommandText = mySelectQuery;

                this.tab3_dataGridView1.AutoGenerateColumns = true;
                //设置数据源    


                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                da.Fill(ds);
                this.tab3_dataGridView1.DataSource = ds.Tables[0];//数据源 


                //标准居中
                this.tab3_dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //设置自动换行

                this.tab3_dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //设置自动调整高度

                this.tab3_dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }

                for (int i = 0; i < tab3_dataGridView1.Columns.Count; i++)
                {
                    tab3_dataGridView1.Columns[i].ReadOnly = true;

                }


                for (int i = 0; i < tab3_dataGridView1.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        tab3_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                    }
                }
                tab3_dataGridView1.Columns[3].Width = 120;
                for (int i = 5; i < tab3_dataGridView1.Columns.Count; i++)
                {
                    tab3_dataGridView1.Columns[i].DefaultCellStyle.Format = "#,###0";
                    tab3_dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                this.Text = mTitle + "   查询完成！共" + (tab3_dataGridView1.RowCount).ToString() + "行";
                System.Windows.Forms.Application.DoEvents();
            }

            catch (Exception ex)
            {
                this.Text = mTitle;
                MessageBox.Show(this, ex.ToString(), "PU_AppVouchBasis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 制告单依据
        /// </summary>
        private void mom_orderBasis()
        {
            try
            {
                string cinvCodeChild = "";
                string[] paraCinvCode = null;
                this.Text = mTitle + "   正在查询，请稍候...";
                System.Windows.Forms.Application.DoEvents();

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();


                string selectSQL = " ";

                selectSQL = "select m.PlanningSortSeq as '排产序号',c.MoCode as '制造单号',b.StartDate as '开工日期',u.cUser_Name as '制单人', m.mrpdatetime as '核算时间',m.sortseq as '制造单行号',a.invcode as '物料编码',i.cInvName as'物料名称',replace(replace(i.cinvstd,'''',''),'\"','')  as '规格',  \r\n";
                selectSQL += " a.qty as '制造单数量',m.moQty as '在制数',m.CursotckQty as '现存量',m.useQty as'已分配量'  \r\n";
                selectSQL += "  from mom_orderdetail_zhrs_mrp  m(nolock)  \r\n";
                selectSQL += "   left join mom_orderdetail a (nolock) on  a.moid=m.moid and a.modid=m.modid and a.partid=m.partid and a.sortseq=m.sortseq  \r\n";
                selectSQL += "   left join mom_morder b (nolock) on a.moid=b.moid and a.modid=b.modid  \r\n";
                selectSQL += "   left join mom_order c  (nolock) on a.moid=c.moid  \r\n";
                selectSQL += "   left join inventory i (nolock)  on i.cInvCode=a.InvCode  \r\n";
                selectSQL += "     left join " + iLoginEx.pubDB_UF() + "..UA_User u(nolock) on c.CreateUser=u.cUser_Id  where 1=1 \r\n";

                tab4_cInvCode.Text = tab4_cInvCode.Text.Trim();
                tab4_cInvCode.Text = tab4_cInvCode.Text.Replace("；", ";");

                if (tab4_cInvCode.Text.Length > 0)
                {
                    cinvCodeChild = "";
                    paraCinvCode = tab4_cInvCode.Text.Split(';');
                    if (paraCinvCode.Length > 0)
                    {
                        for (int i = 0; i < paraCinvCode.Length; i++)
                        {
                            cinvCodeChild += "'" + paraCinvCode[i].ToString() + "',";
                        }

                        selectSQL += " and  a.invcode in (" + cinvCodeChild + "'\r\n'" + ") \r\n";
                    }
                    else
                    {
                        selectSQL += " and  a.invcode ='" + tab4_cInvCode.Text + "' \r\n";
                    }
                }

                tab4_MoCode.Text = tab4_MoCode.Text.Trim();
                tab4_MoCode.Text = tab4_MoCode.Text.Replace("；", ";");

                if (tab4_MoCode.Text.Length > 0)
                {
                    cinvCodeChild = "";
                    paraCinvCode = tab4_MoCode.Text.Split(';');
                    if (paraCinvCode.Length > 0)
                    {
                        for (int i = 0; i < paraCinvCode.Length; i++)
                        {
                            cinvCodeChild += "'" + paraCinvCode[i].ToString() + "',";
                        }

                        selectSQL += " and  c.MoCode in (" + cinvCodeChild + "'\r\n'" + ") \r\n";
                    }
                    else
                    {
                        selectSQL += " and  c.MoCode ='" + tab4_MoCode.Text + "' \r\n";
                    }
                }


                if (tab4_CreateUser.Text.Trim().Length > 0)
                {
                    selectSQL += " and  c.CreateUser ='" + tab4_CreateUser.Text + "' order by  m.PlanningSortSeq,c.MoCode,m.sortseq \r\n";
                }


                OleDbCommand myCommand = new OleDbCommand(selectSQL, myConn);

                this.tab4_dataGridView1.AutoGenerateColumns = true;
                //设置数据源    


                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                da.Fill(ds);
                this.tab4_dataGridView1.DataSource = ds.Tables[0];//数据源 


                //标准居中
                this.tab4_dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //设置自动换行

                this.tab4_dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //设置自动调整高度

                this.tab4_dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }

                for (int i = 0; i < tab4_dataGridView1.Columns.Count; i++)
                {
                    tab4_dataGridView1.Columns[i].ReadOnly = true;

                }


                for (int i = 0; i < tab4_dataGridView1.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        tab4_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                    }
                }
                tab4_dataGridView1.Columns[3].Width = 120;
                for (int i = 5; i < tab4_dataGridView1.Columns.Count; i++)
                {
                    tab4_dataGridView1.Columns[i].DefaultCellStyle.Format = "#,###0";
                    tab4_dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                this.Text = mTitle + "   查询完成！共" + (tab4_dataGridView1.RowCount).ToString() + "行";
                System.Windows.Forms.Application.DoEvents();
            }

            catch (Exception ex)
            {
                this.Text = mTitle;
                MessageBox.Show(this, ex.ToString(), "mom_orderBasis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void toolToExcel_Click(object sender, EventArgs e)
        {
            toolToExcel.Enabled = false;

            try
            {
                this.Text = mTitle;

                if (tabControl1.SelectedTab == tab5StokWarning)
                {
                    iLoginEx.ExportExcel("库存呆料预警表_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "库存呆料预警表", tab1_dataGridView1, 14);
                }
                else if (tabControl1.SelectedTab == tab1CompreStok)
                {
                    if (iLoginEx.ReadUserProfileValue("StockQuery", "QureyType") == "1")
                    {
                        iLoginEx.ExportExcel("库存综合查询_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "库存综合查询", tab2_dataGridView1, 4);
                    }
                    else
                    {
                        iLoginEx.ExportExcel("现存量_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "现存量", tab2_dataGridView2, 6);
                    }
                }
                else if (tabControl1.SelectedTab == tab2DedtailQuery)
                {
                    iLoginEx.ExportExcel("库存综合查询明细_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "库存综合查询明细", tab1_dataGridView1, 8);

                }
                else if (tabControl1.SelectedTab == tab3PU_AppVouchBasis)
                {
                    iLoginEx.ExportExcel("请购来源_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "请购来源", tab3_dataGridView1, 7);
                }
                else if (tabControl1.SelectedTab == tab4mom_orderBasis)
                {
                    iLoginEx.ExportExcel("制造单来源_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "制造单来源", tab4_dataGridView1, 9);
                }

                else if (tabControl1.SelectedTab == tab6MRPtempCurrentStock)
                {
                    iLoginEx.ExportExcel("临时库存表_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "临时库存表", tab6_dataGridView1, 5);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "toolToExcel_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                this.Text = mTitle;
                toolToExcel.Enabled = true;
            }
        }

        private void toolClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPreprocessPurData_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text += "  " + System.Windows.Forms.Application.ProductVersion;
                FormWidth = this.Width;
                FormHeight = this.Height;
                tabPageWidth = tabControl1.Width;
                tabPageHeight = tabControl1.Height;
                dataGridViewWidth = tab1_dataGridView1.Width;
                dataGridViewHeight = tab1_dataGridView1.Height;

                dataGridViewWidth2 = tab2_dataGridView1.Width;
                dataGridViewHeight2 = tab2_dataGridView1.Height;
                tab3_dataGridView1Width = tab3_dataGridView1.Width;
                tab3_dataGridView1Height = tab3_dataGridView1.Height;

                tab5_dataGridView1Width = tab5_dataGridView1.Width;
                tab5_dataGridView1Height = tab5_dataGridView1.Height;

                tab4_dataGridView1Width = tab4_dataGridView1.Width;
                tab4_dataGridView1Height = tab4_dataGridView1.Height;

                tab6_dataGridView1Height = tab6_dataGridView1.Height;
                tab6_dataGridView1Width = tab6_dataGridView1.Width;

                tab2_dataGridView2.Visible = false;

                tab2_dataGridView2.Width = tab2_dataGridView1.Width;
                tab2_dataGridView2.Height = tab2_dataGridView1.Height;

                tab2_dataGridView2.Top = tab2_dataGridView1.Top;
                tab2_dataGridView2.Left = tab2_dataGridView1.Left;


                tab2_DateL.Value = DateTime.Now;
                tab2_DateH.Value = DateTime.Now;
                tab2_DateL.Enabled = false;
                tab2_DateH.Enabled = false;
                tab3_dDateL.Enabled = false;
                tab3_dDateH.Enabled = false;
                toolSave.Enabled = false;

                if (mproType == 0)
                {
                    mTitle = "库存综合查询";
                    tab5StokWarning.Parent = null;
                }
                else
                {
                    tab3PU_AppVouchBasis.Parent = null;
                    tab4mom_orderBasis.Parent = null;
                    //tab1CompreStok.Parent = null; 
                    mTitle = "呆料预警表";
                }
                this.Text = mTitle;



                DateTime dt = DateTime.Now;
                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末//
                tab3_dDateL.Value = startMonth;
                tab3_dDateH.Value = endMonth;

                ini = new Ini(System.Windows.Forms.Application.StartupPath.ToString() + "\\utconfig.ini");
                if (ini.ReadValue("ComprehensiveStock", "ShowProd").Trim().Length == 0)
                {
                    ini.Writue("ComprehensiveStock", "ShowProd", "Y");
                }
                if (ini.ReadValue("ComprehensiveStock", "ShowProd") == "Y")
                {
                    chkShowProd.Checked = true;

                    if (tab1_dataGridView1.Columns.Count > 0)
                    {
                        tab1_dataGridView1.Columns[5].Visible = true;
                        tab1_dataGridView1.Columns[6].Visible = true;
                        tab1_dataGridView1.Columns[7].Visible = true;
                    }
                }
                else
                {
                    chkShowProd.Checked = false;
                    if (tab1_dataGridView1.Columns.Count > 0)
                    {
                        tab1_dataGridView1.Columns[5].Visible = false;
                        tab1_dataGridView1.Columns[6].Visible = false;
                        tab1_dataGridView1.Columns[7].Visible = false;
                    }
                }


                tabControl1_SelectedIndexChanged(null, null);

                txtInCludeKeys.Text = iLoginEx.ReadUserProfileValue("ST531TAB2", "InCludeSeachKeys");
                txtExCludeKeys.Text = iLoginEx.ReadUserProfileValue("ST531TAB2", "ExCludeSeachKeys");
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "frmPreprocessPurData_Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void frmPreprocessPurData_Resize(object sender, EventArgs e)
        {
            IniFile.Ini ini = new IniFile.Ini(System.Windows.Forms.Application.StartupPath.ToString() + "\\utconfig.ini");
            ini.Writue("Window", "AutoAdaptive2", "");
            if (ini.ReadValue("Window", "AutoAdaptive") != "N")
            {
                tabControl1.Width = tabPageWidth + (this.Width - FormWidth);
                tabControl1.Height = tabPageHeight + (this.Height - FormHeight);

                tab1_dataGridView1.Width = dataGridViewWidth + (this.Width - FormWidth);
                tab1_dataGridView1.Height = dataGridViewHeight + (this.Height - FormHeight);

                tab2_dataGridView1.Width = dataGridViewWidth2 + (this.Width - FormWidth);
                tab2_dataGridView1.Height = dataGridViewHeight2 + (this.Height - FormHeight);

                tab3_dataGridView1.Width = tab3_dataGridView1Width + (this.Width - FormWidth);
                tab3_dataGridView1.Height = tab3_dataGridView1Height + (this.Height - FormHeight);

                tab4_dataGridView1.Width = tab4_dataGridView1Width + (this.Width - FormWidth);
                tab4_dataGridView1.Height = tab4_dataGridView1Height + (this.Height - FormHeight);

                tab5_dataGridView1.Width = tab5_dataGridView1Width + (this.Width - FormWidth);
                tab5_dataGridView1.Height = tab5_dataGridView1Height + (this.Height - FormHeight);

                tab6_dataGridView1.Height = tab6_dataGridView1Height + (this.Height - FormHeight);
                tab6_dataGridView1.Width = tab6_dataGridView1Width + (this.Width - FormWidth);

            }
            tab2_dataGridView2.Width = tab2_dataGridView1.Width;
            tab2_dataGridView2.Height = tab2_dataGridView1.Height;

            tab2_dataGridView2.Top = tab2_dataGridView1.Top;
            tab2_dataGridView2.Left = tab2_dataGridView1.Left;
        }

        private void frmPreprocessPurData_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void tab1_cInvCodeL_Leave(object sender, EventArgs e)
        {
            if (tab1_cInvCodeH.Text.Length == 0 && tab1_cInvCodeL.Text.Length > 0)
            {
                tab1_cInvCodeH.Text = tab1_cInvCodeL.Text;
            }
        }

        private void chkShowProd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowProd.Checked)
            {
                if (tab1_dataGridView1.Columns.Count > 0)
                {
                    tab1_dataGridView1.Columns[5].Visible = true;
                    tab1_dataGridView1.Columns[6].Visible = true;
                    tab1_dataGridView1.Columns[7].Visible = true;
                }
                ini.Writue("ComprehensiveStock", "ShowProd", "Y");
            }
            else
            {
                if (tab1_dataGridView1.Columns.Count > 0)
                {
                    tab1_dataGridView1.Columns[5].Visible = false;
                    tab1_dataGridView1.Columns[6].Visible = false;
                    tab1_dataGridView1.Columns[7].Visible = false;
                }
                ini.Writue("ComprehensiveStock", "ShowProd", "N");
            }
        }

        private void tab2_dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < tab2_dataGridView1.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    tab2_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                }
            }
        }

        private void tab1_dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < tab1_dataGridView1.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    tab1_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                }
            }
        }

        private void tab3_dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < tab3_dataGridView1.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    tab3_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                }
            }
        }

        private void tab4_dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < tab4_dataGridView1.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    tab4_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                }
            }
        }

        //private void tab2_cInvCodeL_MouseDoubleClick(object sender, MouseEventArgs e)
        //{


        //    tab2_cInvCodeL.Text = iLoginEx.OpenSelectWindow("物料", "select cInvCode as '料号',cInvName  as '品名',cInvStd  as '规格' from  inventory  (nolock)", tab2_cInvCodeL.Text, 430, 300, 1);
        //    string[] para = tab2_cInvCodeL.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
        //    if (para.Length > 1)
        //    {
        //        tab2_cInvCodeL.Text = para[0];
        //        //wName = para[0];  
        //    }
        //}

        //private void tab2_cInvCodeH_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    tab2_cInvCodeH.Text = iLoginEx.OpenSelectWindow("物料", "select cInvCode as '料号',cInvName  as '品名',cInvStd  as '规格' from  inventory  (nolock)", tab2_cInvCodeH.Text, 430, 300, 1);
        //    string[] para = tab2_cInvCodeH.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
        //    if (para.Length > 1)
        //    {
        //        tab2_cInvCodeH.Text = para[0];
        //        //wName = para[0];  
        //    }
        //}

        private void tab1_cInvCodeL_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tab1_cInvCodeL.Text = iLoginEx.OpenSelectWindow("物料", "select cInvCode as '料号',cInvName  as '品名',cInvStd  as '规格' from  inventory  (nolock)", tab1_cInvCodeL.Text, 430, 300, 1);
            string[] para = tab1_cInvCodeL.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab1_cInvCodeL.Text = para[0];
                //wName = para[0];  
            }
        }

        private void tab1_cInvCodeH_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tab1_cInvCodeH.Text = iLoginEx.OpenSelectWindow("物料", "select cInvCode as '料号',cInvName  as '品名',cInvStd  as '规格' from  inventory  (nolock)", tab1_cInvCodeH.Text, 430, 300, 1);
            string[] para = tab1_cInvCodeH.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab1_cInvCodeH.Text = para[0];
                //wName = para[0];  
            }
        }

        private void tab3_cInvCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tab3_cInvCode.Text = iLoginEx.OpenSelectWindow("物料", "select cInvCode as '料号',cInvName  as '品名',cInvStd  as '规格' from  inventory  (nolock)", tab3_cInvCode.Text, 430, 300, 1);
            string[] para = tab3_cInvCode.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab3_cInvCode.Text = para[0];
                //wName = para[0];  
            }
        }

        private void tab4_cInvCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tab4_cInvCode.Text = iLoginEx.OpenSelectWindow("物料", "select cInvCode as '料号',cInvName  as '品名',cInvStd  as '规格' from  inventory  (nolock)", tab4_cInvCode.Text, 430, 300, 1);
            string[] para = tab4_cInvCode.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab4_cInvCode.Text = para[0];
                //wName = para[0];  
            }
        }

        private void tab3_cCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            tab3_cCode.Text = iLoginEx.OpenSelectWindow("请购单", "select cCode as '请购单号' from Pu_AppVouch (nolock)", tab3_cCode.Text, 430, 300, 1);
            string[] para = tab3_cCode.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab3_cCode.Text = para[0];
                //wName = para[0];  
            }
        }

        private void tab3_cMaker_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            tab3_cMaker.Text = iLoginEx.OpenSelectWindow("制单人", "select cUser_Name as '制单人' from  " + iLoginEx.pubDB_UF() + "..UA_User u  (nolock)", tab3_cMaker.Text, 430, 300, 1);
            string[] para = tab3_cMaker.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab3_cMaker.Text = para[0];
                //wName = para[0];  
            }
        }

        private void tab4_MoCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tab4_MoCode.Text = iLoginEx.OpenSelectWindow("制造单号", "select  MoCode as '制造单号' from mom_order  (nolock)", tab4_MoCode.Text, 430, 300, 1);
            string[] para = tab4_MoCode.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab4_MoCode.Text = para[0];
                //wName = para[0];  
            }
        }

        private void tab4_CreateUser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tab4_CreateUser.Text = iLoginEx.OpenSelectWindow("制单人", "select cUser_ID as '帐号',cUser_Name as '制单人' from  " + iLoginEx.pubDB_UF() + "..UA_User u  (nolock)", tab4_CreateUser.Text, 430, 300, 1);
            string[] para = tab4_CreateUser.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab4_CreateUser.Text = para[0];
                //wName = para[0];  
            }
        }

        private void tab2_DateLCHK_CheckedChanged(object sender, EventArgs e)
        {
            if (tab2_DateLCHK.Checked)
            {
                tab2_DateL.Enabled = true;
            }
            else
            {
                tab2_DateL.Enabled = false;
            }
        }

        private void tab2_DateHCHK_CheckedChanged(object sender, EventArgs e)
        {
            if (tab2_DateHCHK.Checked)
            {
                tab2_DateH.Enabled = true;
            }
            else
            {
                tab2_DateH.Enabled = false;
            }
        }

        private void tab3_dDateLCHK_CheckedChanged(object sender, EventArgs e)
        {
            if (tab3_dDateLCHK.Checked)
            {
                tab3_dDateL.Enabled = true;
            }
            else
            {
                tab3_dDateL.Enabled = false;
            }
        }

        private void tab3_dDateHCHK_CheckedChanged(object sender, EventArgs e)
        {
            if (tab3_dDateHCHK.Checked)
            {
                tab3_dDateH.Enabled = true;
            }
            else
            {
                tab3_dDateH.Enabled = false;
            }
        }

        private void tab1_dataGridView1_CellMouseDown_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            CellMouseDown = true;
            SLBTotal.Text = "";
        }

        private void tab1_dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //if (e.ColumnIndex >= 0 && e.ColumnIndex <= 11)
                //{
                double SelectTotal = 0.0;
                int selectedCellCount = tab1_dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0 && CellMouseDown)
                {
                    SelectTotal = 0.0;
                    for (int i = 0; i < selectedCellCount; i++)
                    {
                        SelectTotal += Convert.ToDouble(Convert.ToString(Convert.IsDBNull(tab1_dataGridView1.SelectedCells[i].Value) ? "" : tab1_dataGridView1.SelectedCells[i].Value) == "" ? "0" : tab1_dataGridView1.SelectedCells[i].Value.ToString());
                    }
                    SLBTotal.Text = string.Format("{0:N0}", SelectTotal);
                }
                //}

            }
            catch
            {
            }
        }

        private void tab1_dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

            tab1_dataGridView1_CellMouseMove(sender, e);
            CellMouseDown = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SLBTotal.Text = "";
            toolDownLoad.Visible = false;
            toolImport.Visible = false;
            toolEdit.Visible = false;
            toolDelete.Visible = false;

            if (tabControl1.SelectedTab == tab6MRPtempCurrentStock)
            {
                toolDownLoad.Visible = true;
                toolImport.Visible = true;
                toolEdit.Visible = true;
                toolDelete.Visible = true;

                toolImport.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Import);
                toolEdit.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Edit);
                toolDelete.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Delete);
            }
        }

        private void tab3_dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            CellMouseDown = true;
            SLBTotal.Text = "";
        }

        private void tab3_dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //if (e.ColumnIndex >= 0 && e.ColumnIndex <= 11)
                //{
                double SelectTotal = 0.0;
                int selectedCellCount = tab3_dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0 && CellMouseDown)
                {
                    SelectTotal = 0.0;
                    for (int i = 0; i < selectedCellCount; i++)
                    {
                        SelectTotal += Convert.ToDouble(Convert.ToString(Convert.IsDBNull(tab3_dataGridView1.SelectedCells[i].Value) ? "" : tab3_dataGridView1.SelectedCells[i].Value) == "" ? "0" : tab3_dataGridView1.SelectedCells[i].Value.ToString());
                    }
                    SLBTotal.Text = string.Format("{0:N0}", SelectTotal);
                }
                //}

            }
            catch
            {
            }
        }

        private void tab3_dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            tab3_dataGridView1_CellMouseMove(sender, e);
            CellMouseDown = false;
        }

        private void tab4_dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            CellMouseDown = true;
            SLBTotal.Text = "";
        }

        private void tab4_dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //if (e.ColumnIndex >= 0 && e.ColumnIndex <= 11)
                //{
                double SelectTotal = 0.0;
                int selectedCellCount = tab4_dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0 && CellMouseDown)
                {
                    SelectTotal = 0.0;
                    for (int i = 0; i < selectedCellCount; i++)
                    {
                        SelectTotal += Convert.ToDouble(Convert.ToString(Convert.IsDBNull(tab4_dataGridView1.SelectedCells[i].Value) ? "" : tab4_dataGridView1.SelectedCells[i].Value) == "" ? "0" : tab4_dataGridView1.SelectedCells[i].Value.ToString());
                    }
                    SLBTotal.Text = string.Format("{0:N0}", SelectTotal);
                }
                //}

            }
            catch
            {
            }
        }

        private void tab4_dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            tab4_dataGridView1_CellMouseMove(sender, e);
            CellMouseDown = false;
        }

        private void tab1_dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (mproType == 0)
            //{
            cInvCode = tab1_dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            CompreStok();
            tabControl1.SelectedIndex = 1;
            //}
        }

        private void tab5_dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            CellMouseDown = true;
            SLBTotal.Text = "";
        }

        private void tab5_dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //if (e.ColumnIndex >= 0 && e.ColumnIndex <= 11)
                //{
                double SelectTotal = 0.0;
                int selectedCellCount = tab5_dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0 && CellMouseDown)
                {
                    SelectTotal = 0.0;
                    for (int i = 0; i < selectedCellCount; i++)
                    {
                        SelectTotal += Convert.ToDouble(Convert.ToString(Convert.IsDBNull(tab5_dataGridView1.SelectedCells[i].Value) ? "" : tab5_dataGridView1.SelectedCells[i].Value) == "" ? "0" : tab5_dataGridView1.SelectedCells[i].Value.ToString());
                    }
                    SLBTotal.Text = string.Format("{0:N0}", SelectTotal);
                }
                //}

            }
            catch
            {
            }
        }

        private void tab5_dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            tab5_dataGridView1_CellMouseMove(sender, e);
            CellMouseDown = false;
        }

        private void tab5_dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (Tab2_SaveCulomnsWidth && tab1_dataGridView1.Columns.Count > 0)
            {

                string ColumnsWidths = "";
                for (int i = 0; i < tab1_dataGridView1.Columns.Count; i++)
                {
                    ColumnsWidths += tab1_dataGridView1.Columns[i].Width.ToString() + ";";
                }
                ColumnsWidths += iLoginEx.Chr(8);
                ColumnsWidths = ColumnsWidths.Replace(";" + iLoginEx.Chr(8), "");

                iLoginEx.WriteUserProfileValue("tab5StokWarning", "ColumnsWidths", ColumnsWidths);
            }
        }
        /// <summary>
        /// 出库明细
        /// </summary>
        private void StockOutList()
        {

            try
            {
                string selectSQL = "select w.cWhName  as '仓库名称',b.cCode as '出库单号',b.dDate as '出库日期',a.cinvcode as '物料编码',i.cInvName as'物料名称',i.cInvStd as '规格型号',  \r\n";
                selectSQL += " a.cmocode as '制造单号',a.invcode as '成品半成品编码',a.iQuantity as '出库数量'   \r\n";
                selectSQL += " from RdRecords a (nolock) left join RdRecord b (nolock)on a.id=b.id   \r\n";
                selectSQL += "  left join Warehouse  w (nolock) on b.cWhCode=w.cWhCode   \r\n";
                selectSQL += "  left join Inventory i (nolock) on i.cInvCode=a.cInvCode   \r\n";
                selectSQL += "  where b.cVouchType='11'   \r\n";

                if (tab1_cInvCodeL.Text.Length > 0 && tab1_cInvCodeH.Text.Length == 0)
                {
                    selectSQL += "  and  a.cinvcode ='" + tab1_cInvCodeL.Text + "' \r\n";
                }
                else if (tab1_cInvCodeL.Text.Length == 0 && tab1_cInvCodeH.Text.Length > 0)
                {
                    selectSQL += "  and  a.cinvcode ='" + tab1_cInvCodeH.Text + "' \r\n";
                }
                else if (tab1_cInvCodeL.Text.Length > 0 && tab1_cInvCodeH.Text.Length > 0)
                {
                    selectSQL += "  and  a.cinvcode between  '" + tab1_cInvCodeL.Text + "' and '" + tab1_cInvCodeH.Text + "'  \r\n";
                }



                selectSQL += " and b.ddate between left(convert(varchar,dateadd(mm,-" + mOutMonth.ToString() + ",getdate()),20),10) and left(convert(varchar,getdate(),20),10)   \r\n";
                selectSQL += "   \r\n";

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                OleDbCommand myCommand = new OleDbCommand(selectSQL, myConn);
                this.tab4_dataGridView1.AutoGenerateColumns = true;
                //设置数据源    


                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                da.Fill(ds);

                this.tab1_dataGridView1.DataSource = ds.Tables[0];//数据源 


                //标准居中
                this.tab1_dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //设置自动换行

                this.tab1_dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //设置自动调整高度

                this.tab1_dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }


                for (int i = 0; i < tab1_dataGridView1.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        tab1_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                    }
                }

                for (int i = 0; i < tab1_dataGridView1.Columns.Count; i++)
                {
                    tab1_dataGridView1.Columns[i].ReadOnly = true;

                }

                tab1_dataGridView1.Columns[tab1_dataGridView1.Columns.Count - 1].DefaultCellStyle.Format = "#,###0";
                tab1_dataGridView1.Columns[tab1_dataGridView1.Columns.Count - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "StockOutList()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tab5_dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {


                if (e.ColumnIndex < 0)
                {
                    return;
                }


                if (e.RowIndex < 0)
                {
                    return;
                }

                if (tab5_dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Length == 0)
                {
                    return;
                }
                else
                {


                    tab1_cInvCodeL.Text = tab5_dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    tab1_cInvCodeH.Text = tab1_cInvCodeL.Text;


                    switch (e.ColumnIndex)
                    {

                        case 9:
                            {
                                tabControl1.SelectedIndex = 2;
                                DedtailQuery(chkShowAll.Checked, "3");//在检
                                this.Text = mTitle + "  到货在检量";
                                break;
                            }
                        case 8:
                            {
                                tabControl1.SelectedIndex = 2;
                                DedtailQuery(chkShowAll.Checked, "1,2");//采购在途
                                this.Text = mTitle + "  采购在途";
                                break;
                            }
                        case 3:
                            {
                                tabControl1.SelectedIndex = 2;
                                DedtailQuery(chkShowAll.Checked, "4");//现存量
                                this.Text = mTitle + "  现存量";
                                break;
                            }
                        case 4:
                            {
                                tabControl1.SelectedIndex = 2;
                                DedtailQuery(chkShowAll.Checked, "4");//现存量
                                this.Text = mTitle + "  现存量";
                                break;
                            }
                        case 10:
                            {
                                tabControl1.SelectedIndex = 2;
                                DedtailQuery(chkShowAll.Checked, "5");//在制
                                this.Text = mTitle + "  在制";
                                break;
                            }
                        case 7:
                            {
                                tabControl1.SelectedIndex = 2;
                                DedtailQuery(chkShowAll.Checked, "6,7");//已分配量
                                this.Text = mTitle + "  已分配量";
                                break;
                            }
                        case 5:
                            {
                                if (Convert.ToDouble(Convert.ToString(tab5_dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == "" ? "0" : Convert.ToString(tab5_dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)) != 0)
                                {
                                    this.Text = mTitle + "  出库明细";
                                    StockOutList();
                                }
                                break;
                            }
                        case 6:
                            {
                                if (Convert.ToDouble(Convert.ToString(tab5_dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == "" ? "0" : Convert.ToString(tab5_dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)) != 0)
                                {
                                    this.Text = mTitle + "  出库明细";
                                    StockOutList();
                                }
                                break;
                            }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "tab5_dataGridView1_CellMouseDoubleClick()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolDownLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab == tab6MRPtempCurrentStock)
                {
                    string PurInportModelFilePath = System.Windows.Forms.Application.StartupPath.ToString() + "\\mrpCurrentStockmodel.rmd";
                    string saveFileName = "";
                    //bool fileSaved = false;
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.DefaultExt = "xls";
                    saveDialog.Filter = "Excel文件|*.xls";
                    saveDialog.FileName = "临时库存导入模板";
                    saveDialog.ShowDialog();
                    saveFileName = saveDialog.FileName;
                    System.IO.File.Copy(PurInportModelFilePath, saveFileName, true);

                    MessageBox.Show(this, "临时库存导入模板下载完成！", "模板下载", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "toolDownLoad_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Null转字符串
        /// </summary>
        /// <param name="iObj"></param>
        /// <returns></returns>
        private string GetValueString(object iObj)
        {
            if (iObj == null)
            {
                return "";
            }
            else
            {
                return iObj.ToString();
            }
        }

        private void toolImport_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tab6MRPtempCurrentStock)
            {
                string cWhCode = "", cInvCode = "";
                string Qty = "0.0", cmtInQty = "0.0", cmtOutQty = "0.0";
                //string ErrorMsg = "";
                toolImport.Enabled = false;

                object missing = Missing.Value;
                //创建一个新的Excel应用对象

                ApplicationClass app = new ApplicationClass();
                long wRow = -1;

                string selectSQL = "";

                ArrayList ErrorMessglist = new ArrayList();
                ArrayList ErrorMessglistDetial = new ArrayList();

                try
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Title = "打开(Open)";
                    ofd.FileName = "";
                    ofd.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);//为了获取特定的系统文件夹，可以使用System.Environment类的静态方法GetFolderPath()。该方法接受一个Environment.SpecialFolder枚举，其中可以定义要返回路径的哪个系统目录  
                    ofd.Filter = "临时库存导入模板(*.xls)|*.xls";
                    ofd.ValidateNames = true;     //文件有效性验证ValidateNames，验证用户输入是否是一个有效的Windows文件名             
                    ofd.CheckFileExists = true;  //验证路径有效性             
                    ofd.CheckPathExists = true; //验证文件有效性
                    ofd.Multiselect = false;
                    if (ofd.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show(this, "未选任何excel文件", "临时库存导入", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;

                    }
                    //按照文件路径以及文件名打开此Excel 
                    Workbook wbook = app.Workbooks.Open(ofd.FileName, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    //设定读取该Excel的第一个工作簿 
                    Worksheet worksheet = (Worksheet)wbook.Worksheets[1];
                    Range range1 = null;


                    int rowCount = worksheet.UsedRange.Rows.Count;
                    int colCount = worksheet.UsedRange.Columns.Count;

                    if (rowCount > 1)
                    {

                        OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());
                        if (myConn.State == System.Data.ConnectionState.Open)
                        {
                            myConn.Close();
                        }
                        myConn.Open();
                        OleDbCommand myCommand = new OleDbCommand("", myConn);

                        for (int r = 2; r <= rowCount; r++)
                        {

                            wRow = r;

                            cWhCode = "";
                            cInvCode = "";
                            Qty = "0.0";
                            cmtInQty = "0.0";
                            cmtOutQty = "0.0";

                            range1 = worksheet.get_Range(worksheet.Cells[r, 1], worksheet.Cells[r, 1]);//仓库代码
                            cWhCode = GetValueString(range1.Text).Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");

                            range1 = worksheet.get_Range(worksheet.Cells[r, 3], worksheet.Cells[r, 3]);//料号
                            cInvCode = GetValueString(range1.Text).Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");

                            range1 = worksheet.get_Range(worksheet.Cells[r, 6], worksheet.Cells[r, 6]);//数量
                            Qty = GetValueString(range1.Value2).Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");

                            range1 = worksheet.get_Range(worksheet.Cells[r, 7], worksheet.Cells[r, 7]);//累计入库数量
                            cmtInQty = GetValueString(range1.Value2).Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");

                            range1 = worksheet.get_Range(worksheet.Cells[r, 8], worksheet.Cells[r, 8]);//累计出库数量
                            cmtOutQty = GetValueString(range1.Value2).Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");

                            Qty = Qty == "-" ? "0" : Qty;
                            cmtInQty = cmtInQty == "-" ? "0" : cmtInQty;
                            cmtOutQty = cmtOutQty == "-" ? "0" : cmtOutQty;

                            Qty = Qty.Length == 0 ? "0" : Qty;
                            cmtInQty = cmtInQty.Length == 0 ? "0" : cmtInQty;
                            cmtOutQty = cmtOutQty.Length == 0 ? "0" : cmtOutQty;



                            myCommand.CommandText = "select 1 from inventory (nolock) where  cInvCode='" + cInvCode + "'  \r\n";
                            OleDbDataReader myReader = myCommand.ExecuteReader();
                            if (myReader.Read())
                            {
                                myReader.Close();
                            }
                            else
                            {
                                myReader.Close();
                                ErrorMessglist.Add("第" + r.ToString() + "行物料" + cInvCode + "不存在");
                                continue;
                            }




                            myCommand.CommandText = "select 1  from zhrs_t_MRP_CurrentStock (nolock) where cWhCode='" + cWhCode + "' and cInvCode='" + cInvCode + "'";
                            myReader = myCommand.ExecuteReader();
                            if (myReader.Read())
                            {
                                selectSQL = "update zhrs_t_MRP_CurrentStock set Qty=" + Qty + ",cmtInQty=" + cmtInQty + ",cmtOutQty=" + cmtOutQty + " where cWhCode='" + cWhCode + "' and cInvCode='" + cInvCode + "'";
                            }
                            else
                            {
                                selectSQL = "insert into zhrs_t_MRP_CurrentStock(cWhCode,cInvCode,Qty,cmtInQty,cmtOutQty)values('" + cWhCode + "','" + cInvCode + "'," + Qty + "," + cmtInQty + "," + cmtOutQty + ")";
                            }
                            myReader.Close();

                            myCommand.CommandText = selectSQL;
                            myCommand.ExecuteNonQuery();


                            SLbState.Text = Convert.ToString(Math.Round(((float)r / (float)rowCount * 100), 2)) + "%，正在导入 " + cInvCode;
                            System.Windows.Forms.Application.DoEvents();
                        }

                        app.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(range1);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(wbook);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                        range1 = null;
                        worksheet = null;
                        wbook = null;
                        app = null;
                        GC.Collect();



                        if (myConn.State == System.Data.ConnectionState.Open)
                        {
                            myConn.Close();
                        }

                        if (ErrorMessglist.Count == 0)
                        {
                            SLbState.Text = "临时库存导入成功";
                            System.Windows.Forms.Application.DoEvents();
                            MessageBox.Show(this, "临时库存导入成功", "临时库存导入()", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            SLbState.Text = "导入失败";
                        }

                    }
                }
                catch (Exception ex)
                {
                    ErrorMessglist.Add("问题出在：\r\n" + "行=" + wRow.ToString() + "\r\n产品编码=" + cInvCode);
                    ErrorMessglistDetial.Add("toolImport_Click():\r\n临时库存导入失败！问题出在：\r\n" + "行=" + wRow.ToString() + "\r\n物料编码=" + cInvCode + "\r\n\r\n" + ex.ToString() + "\r\n\r\n\r\n" + selectSQL);
                }
                finally
                {
                    toolImport.Enabled = true;
                    if (ErrorMessglist.Count > 0)
                    {

                        string msg = "";
                        foreach (var p in ErrorMessglist)
                        {
                            msg += p.ToString() + "\r\n";
                        }
                        msg += "****************************************\r\n\r\n\r\n";
                        foreach (var p in ErrorMessglistDetial)
                        {
                            msg += p.ToString() + "\r\n";
                        }



                        frmMessege frmmsg = new frmMessege("以下产品编码有问题，请检查并重新导入\r\n" + msg, "临时库存导入()");
                        frmmsg.ShowDialog(this);
                    }
                    if (app != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                        GC.Collect();
                    }


                }
            }
        }

        private void toolEdit_Click(object sender, EventArgs e)
        {

            toolSave.Enabled = true;
            if (tabControl1.SelectedTab == tab6MRPtempCurrentStock)
            {
                tab6_dataGridView1.Columns[tab6_dataGridView1.Columns.Count - 1].ReadOnly = false;
                tab6_dataGridView1.Columns[tab6_dataGridView1.Columns.Count - 2].ReadOnly = false;
                tab6_dataGridView1.Columns[tab6_dataGridView1.Columns.Count - 3].ReadOnly = false;
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            try
            {
                toolSave.Enabled = false;
                string selectSQL = "";

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();


                OleDbCommand myCommand = new OleDbCommand("", myConn);

                if (tabControl1.SelectedTab == tab6MRPtempCurrentStock)
                {
                    tab6_dataGridView1.Columns[tab6_dataGridView1.Columns.Count - 1].ReadOnly = true;
                    tab6_dataGridView1.Columns[tab6_dataGridView1.Columns.Count - 2].ReadOnly = true;
                    tab6_dataGridView1.Columns[tab6_dataGridView1.Columns.Count - 3].ReadOnly = true;


                    for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                    {

                        selectSQL = " update zhrs_t_MRP_CurrentStock set Qty=" + tab6_dataGridView1.Rows[i].Cells["现存量"].Value.ToString() + ",cmtInQty=" + tab6_dataGridView1.Rows[i].Cells["本期累计入库数"].Value.ToString() + ",cmtOutQty=" + tab6_dataGridView1.Rows[i].Cells["本期累计出库数"].Value.ToString() + " \r\n";
                        selectSQL += " where cWhCode='" + tab6_dataGridView1.Rows[i].Cells["仓库代码"].Value.ToString() + "' and cInvCode='" + tab6_dataGridView1.Rows[i].Cells["物料编码"].Value.ToString() + "' \r\n";

                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();
                    }
                    tab6_dataGridView1.Update();

                }
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "toolSave_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string selectSQL = "";
                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();


                OleDbCommand myCommand = new OleDbCommand("", myConn);

                if (tabControl1.SelectedTab == tab6MRPtempCurrentStock)
                {

                    for (int i = 0; i < tab6_dataGridView1.SelectedRows.Count; i++)
                    {
                        selectSQL = " delete  zhrs_t_MRP_CurrentStock  where cWhCode='" + tab6_dataGridView1.SelectedRows[i].Cells["仓库代码"].Value.ToString() + "' and cInvCode='" + tab6_dataGridView1.SelectedRows[i].Cells["物料编码"].Value.ToString() + "' \r\n";

                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();
                    }


                    foreach (DataGridViewRow dr in tab6_dataGridView1.SelectedRows)
                    {
                        tab6_dataGridView1.Rows.Remove(dr);
                    }
                    tab6_dataGridView1.Update();
                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "toolDelete_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
