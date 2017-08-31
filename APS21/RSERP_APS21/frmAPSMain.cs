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


namespace RSERP_APS21
{
    public partial class frmRSERP_APS21Main : Form
    {
        private int FormWidth = 0, FormHeight = 0, tabPageWidth = 0, tabPageHeight = 0, dataGridViewWidth = 0, dataGridViewHeight = 0, dataGridViewWidth2 = 0, dataGridViewHeight2 = 0, tab3_dataGridView1Width = 0, tab3_dataGridView1Height = 0, tab4_dataGridView1Width = 0, tab4_dataGridView1Height = 0;
        private int tab6_dataGridView1Width = 0, tab6_dataGridView1Height = 0, tab8_dataGridView1Width = 0, tab8_dataGridView1Height = 0;
        private UTLoginEx.LoginEx iLoginEx = new LoginEx();
        private RSERP.ComprehensiveStock wComprehensiveStock = new ComprehensiveStock();
        private Ini ini = null;
        private bool CellMouseDown = false;
        private string SQLSelect_Temp = "", SQLSelect = "";
        private bool Tab6_SaveCulomnsWidth = false;
        private List<int> APSTab6RowCutList = new List<int>();
        private string APS21_DropTempTable = "", APS21_CreateTempTable = "";
        private string mom_moallocateColumns = "";
        private bool DoOrderSplit = false;
        private string cWhCode = "";
        private string mTitle = "排产管理";
        private const int AuthID = 39;//高级排产

        public frmRSERP_APS21Main(string[] args)
        {
            InitializeComponent();
            //39:高级排产
            iLoginEx.Initialize(args, AuthID);//必须先初始化LoginEx

            SLbAccID.Text = iLoginEx.AccID();
            SLbAccName.Text = iLoginEx.AccName();
            SLbServer.Text = iLoginEx.DBServerHost();
            SLbYear.Text = iLoginEx.iYear();
            SLbUser.Text = iLoginEx.UserId() + "[" + iLoginEx.UserName() + "]";
            SLBLoginDate.Text = iLoginEx.LoginDate();
        }

        /// <summary>
        /// 库存明细
        /// </summary>  
        private void DedtailQuery(bool showAllData, string DocTypeNo)
        {

            try
            {

                this.Text = "排产管理   正在查询，请稍候...";
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

                this.Text = "排产管理   查询完成！共" + (tab1_dataGridView1.RowCount).ToString() + "行";
                System.Windows.Forms.Application.DoEvents();
            }

            catch (Exception ex)
            {
                this.Text = "排产管理";
                MessageBox.Show(this, ex.ToString(), "PDBOM.btnQuery_Click()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void tab2_cInvCodeL_Leave(object sender, EventArgs e)
        {

            if (tab2_cInvCodeH.Text.Length == 0 && tab2_cInvCodeL.Text.Length > 0)
            {
                tab2_cInvCodeH.Text = tab2_cInvCodeL.Text;
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
                if (tab2_dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Length == 0)
                {
                    return;
                }
                else
                {
                    tab1_cInvCodeL.Text = tab2_dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    tab1_cInvCodeH.Text = tab1_cInvCodeL.Text;
                    if (e.ColumnIndex >= 0 && e.ColumnIndex <= 3)
                    {
                        tabControl1.SelectedIndex = 1;
                        DedtailQuery(true, "");
                        this.Text = "排产管理";
                    }
                    switch (e.ColumnIndex)
                    {
                        case 7:
                            {
                                tabControl1.SelectedIndex = 5;
                                DedtailQuery(chkShowAll.Checked, "3");//在检
                                this.Text = mTitle + "  到货在检量";
                                break;
                            }
                        case 5:
                            {
                                tabControl1.SelectedIndex = 5;
                                DedtailQuery(chkShowAll.Checked, "1,2");//采购在途
                                this.Text = mTitle + "  采购在途";
                                break;
                            }
                        case 8:
                            {
                                tabControl1.SelectedIndex = 5;
                                DedtailQuery(chkShowAll.Checked, "4");//现存量
                                this.Text = mTitle + "  现存量";
                                break;
                            }
                        case 9:
                            {
                                tabControl1.SelectedIndex = 5;
                                DedtailQuery(chkShowAll.Checked, "8");//替代料
                                this.Text = mTitle + "  替代料";
                                break;
                            }
                        case 4:
                            {
                                tabControl1.SelectedIndex = 5;
                                DedtailQuery(chkShowAll.Checked, "5");//在制
                                this.Text = mTitle + "  在制";
                                break;
                            }
                        case 11:
                            {
                                tabControl1.SelectedIndex = 5;
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
        /// 显示综合库存
        /// </summary>
        private void CompreStok()
        {
            try
            {
                this.Text = "排产管理   正在查询，请稍候...";
                System.Windows.Forms.Application.DoEvents();

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                string mySelectQuery = "", cInvCode = "";

                if (tab2_cInvCodeL.Text.Length > 0 && tab2_cInvCodeH.Text.Length > 0 && tab2_cInvCodeL.Text == tab2_cInvCodeH.Text)
                {
                    cInvCode = tab2_cInvCodeL.Text;
                }

                //合计

                mySelectQuery += "   \r\n";
                mySelectQuery += " select '合计' as  'DocType' ,'' as 'cCode',null as 'dDate','' as 'cDefine30',a.cinvcode,p.cInvName,replace(replace(p.cinvstd,'''',''),'\"','') as cInvStd,p.cInvDefine7 ,a.moQty,a.Now_PurArrQty,a.Now_PurQty,a.CurSotckQty, (isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)) as 'allSotckQty',a.useQty, a.toArrQty,a.AltmQty, \r\n";
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

                tab2_cinvCodeAny.Text = tab2_cinvCodeAny.Text.Trim();
                tab2_cinvCodeAny.Text = tab2_cinvCodeAny.Text.Replace("；", ";");

                if (tab2_cinvCodeAny.Text.Length > 0)
                {
                    string cinvCodeChild = "";
                    string[] paraCinvCode = tab2_cinvCodeAny.Text.Split(';');
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
                        mySelectQuery += " and  cinvcode ='" + tab2_cinvCodeAny.Text + "' \r\n";
                    }
                }
                else
                {
                    if (tab2_cInvCodeL.Text.Length > 0 && tab2_cInvCodeH.Text.Length == 0)
                    {
                        mySelectQuery += " and  cinvcode ='" + tab2_cInvCodeL.Text + "' \r\n";
                    }
                    else if (tab2_cInvCodeL.Text.Length == 0 && tab2_cInvCodeH.Text.Length > 0)
                    {
                        mySelectQuery += "  and  cinvcode ='" + tab2_cInvCodeH.Text + "' \r\n";
                    }
                    else if (tab2_cInvCodeL.Text.Length > 0 && tab2_cInvCodeH.Text.Length > 0)
                    {
                        mySelectQuery += "  and  cinvcode between  '" + tab2_cInvCodeL.Text + "' and '" + tab2_cInvCodeH.Text + "'  \r\n";
                    }
                }

                mySelectQuery += "  group by cinvcode) a left join inventory p on a.cinvcode=p.cinvcode    \r\n";

                if (tab2_chkMissingOnly.Checked)
                {
                    if (tab1_chkPurQtyState.Checked)
                    {
                        //可用量=即将到货+到货在检+现存量-已分配量
                        mySelectQuery += " where   (isnull(a.toArrQty,0)+isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)-isnull(a.useQty,0)) <0 \r\n";
                    }
                    else
                    {
                        //可用量=采购在途+到货在检+现存量-已分配量
                        mySelectQuery += "  where (isnull(a.Now_PurQty,0)+isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)-isnull(a.useQty,0))  <0  \r\n";
                    }

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

                this.Text = "排产管理   查询完成！共" + (tab2_dataGridView1.RowCount).ToString() + "行";
                System.Windows.Forms.Application.DoEvents();
            }

            catch (Exception ex)
            {
                this.Text = "排产管理";
                MessageBox.Show(this, ex.ToString(), "PDBOM.btnQuery_Click()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void toolQuery_Click(object sender, EventArgs e)
        {
            toolQuery.Enabled = false;
            SLBTotal.Text = "";
            string DocTypeNo = "";
            try
            {
                switch (this.tabControl1.SelectedIndex)
                {
                    case 2://排产管理
                        {
                            cmtab6Paste.Enabled = false;
                            cmtab6RowCut.Enabled = false;
                            cmtab6RowPaste.Enabled = false;
                            cmtab6Priority.Enabled = false;
                            cmtab6Sure2.Enabled = false;
                            cmtab6NotSure.Enabled = false;
                            cmtab6Close.Enabled = false;
                            cmtab6NotColse.Enabled = false;
                            cmtab6ChangeDate.Enabled = false;
                            cmtab6OrderSplit.Enabled = false;
                            cmtab6OrderMerge.Enabled = false;
                            cmtab6Colors.Enabled = false;
                            tab6_StandardCut.Enabled = false;
                            APS21Query(false, "");
                            SaveAPS21_1();
                            APS21Query(false, SQLSelect);
                            toolSave.Enabled = false;
                            break;
                        }
                    case 4:
                        {
                            CompreStok();
                            break;
                        }
                    case 5:
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
                            break;
                        }
                    case 6:
                        {
                            PU_AppVouchBasis();
                            break;
                        }
                    case 7:
                        {
                            mom_orderBasis();
                            break;
                        }
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
                this.Text = "排产管理";
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
                this.Text = "排产管理   正在查询，请稍候...";
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

                this.Text = "排产管理   查询完成！共" + (tab3_dataGridView1.RowCount).ToString() + "行";
                System.Windows.Forms.Application.DoEvents();
            }

            catch (Exception ex)
            {
                this.Text = "排产管理";
                MessageBox.Show(this, ex.ToString(), "PU_AppVouchBasis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 生产订单子件明细表
        /// </summary>
        private void APS21MOMSupply_MoallocateDetailed()
        {
            try
            {
                Tab6_SaveCulomnsWidth = false;
                string selectSQL = "";


                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                OleDbCommand myCommand = new OleDbCommand("", myConn);
                myCommand.CommandTimeout = 3600;

                selectSQL = " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#APS21MOMSMoalloDetail" + iLoginEx.GetMacAddress().Replace(":", "") + "') and type='U')   \r\n";
                selectSQL += " drop table #APS21MOMSMoalloDetail" + iLoginEx.GetMacAddress().Replace(":", "") + " ;   \r\n";
                selectSQL += " CREATE TABLE #APS21MOMSMoalloDetail" + iLoginEx.GetMacAddress().Replace(":", "") + "( \r\n";
                selectSQL += " 	modid int)  \r\n";
                selectSQL += "   \r\n";
                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();

                for (int r = 0; r < tab6_dataGridView1.SelectedRows.Count; r++)
                {
                    selectSQL = " insert into #APS21MOMSMoalloDetail" + iLoginEx.GetMacAddress().Replace(":", "") + "(modid)values(" + Convert.ToString(tab6_dataGridView1.SelectedRows[r].Cells["MoDId"].Value) + ")";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();
                }

                selectSQL = "select o.MoCode as '制造单号', m.sortSeq as '行号',m.InvCode as '产品编码',m.Qty as '排产数量',a.SortSeq as '子件行号',a.OpSeq as '工序行号',rt.Description as '工序说明',a.InvCode as '物料编码', i.cInvName as '物料名称',i.cInvStd as '规格型号',    \r\n";
                selectSQL += "   a.BaseQtyN/case when isnull(a.BaseQtyD,1)=0 then 1 else isnull(a.BaseQtyD,1) end  as '单位用量',a.Qty as '用量',a.IssQty as '已领数',a.TransQty as '已调拨数',    \r\n";
                selectSQL += "   case  when a.WIPType=1 then '入库倒冲' else case when a.WIPType=2 then '工序倒冲' else case when a.WIPType=3 then '领用' else case when a.WIPType=4 then '直接供应'  end end end end as '供应类型',    \r\n";
                selectSQL += "   isnull(a.Remark,a.Define29) as '备注'     \r\n";
                selectSQL += "   from mom_moallocate a (nolock)    \r\n";
                selectSQL += "   left join inventory i (nolock) on a.invcode=i.cinvcode   \r\n";
                selectSQL += "   left join mom_orderdetail m (nolock)on a.modid=m.modid    \r\n";
                selectSQL += "   left join mom_order o (nolock)on o.moid=m.moid   \r\n";
                selectSQL += "   left join (    \r\n";
                selectSQL += "  select OpSeq ,RoutingId=MoDId,Description from sfc_moroutingdetail     \r\n";
                selectSQL += "   union     \r\n";
                selectSQL += "   select OpSeq,RoutingId=PRoutingId,Description from sfc_proutingdetail     \r\n";
                selectSQL += "  union     \r\n";
                selectSQL += "   select OpSeq,RoutingId=EcnRoutingId,Description from ecn_proutingdetail     \r\n";
                selectSQL += "  ) rt on a.OpSeq=rt.OpSeq and m.RoutingId=rt.RoutingId    \r\n";
                selectSQL += "   where  exists (select 1 from #APS21MOMSMoalloDetail" + iLoginEx.GetMacAddress().Replace(":", "") + " tmp where a.modid=tmp.modid)     \r\n";
                selectSQL += "   \r\n";

                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();

                this.tab8_dataGridView1.AutoGenerateColumns = true;
                //设置数据源    


                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                da.Fill(ds);
                this.tab8_dataGridView1.DataSource = ds.Tables[0];//数据源 


                //标准居中
                this.tab8_dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //设置自动换行

                this.tab8_dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //设置自动调整高度

                this.tab8_dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                for (int i = 0; i < tab8_dataGridView1.Columns.Count; i++)
                {
                    tab8_dataGridView1.Columns[i].ReadOnly = true;
                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                tabControl1.SelectedIndex = 3;
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21MOMSupply_MoallocateDetailed()");
                frmmsg.ShowDialog(this);
            }
        }

        /// <summary>
        /// 排产管理--查询
        /// </summary>
        private void APS21Query(bool IsRefresh, string wSQL)
        {
            try
            {
                Tab6_SaveCulomnsWidth = false;

                string selectSQL = null;
                OleDbDataReader myReader = null;
                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                OleDbCommand myCommand = new OleDbCommand("", myConn);
                myCommand.CommandTimeout = 3600;
                SLbState.Text = "";

                APS21_DropTempTable = "  \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_stdbom') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_stdbom;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_bomcomponent') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_bomcomponent;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_bomdown') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_bomdown;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_procmo') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_procmo;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_procerror') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_procerror;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_collectivemo') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_collectivemo;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_recordcount') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_recordcount;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_updhead') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_updhead;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_upddetail') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_upddetail;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_updwipallocateid') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_updwipallocateid;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#tmp_deldid') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #tmp_deldid;  \r\n";
                APS21_DropTempTable += "   \r\n";
                APS21_DropTempTable += " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#STPEUnlockID') and type='U')  \r\n";
                APS21_DropTempTable += " drop table #STPEUnlockID;  \r\n";

                APS21_CreateTempTable = "  \r\n";
                APS21_CreateTempTable += "  create table #tmp_bomcomponent ( OpSeq nchar(4) null,OrgSortSeq  int null default 0,ParentId int null,ComponentId int null,ParentScrap decimal(6,3) null default 0,BaseQtyN decimal(28,6) null default 0,  \r\n";
                APS21_CreateTempTable += " BaseQtyD decimal(28,6) null default 0,CompScrap decimal(6,3) null default 0,Qty decimal(28,6) null default 0,FVFlag tinyint null default 1,ByproductFlag bit null,ProductType tinyint null default 1,  \r\n";
                APS21_CreateTempTable += " WIPType tinyint null default 3,WhCode nvarchar(10) null,StartDemDate datetime null,EndDemDate  datetime null,OpComponentId int null default 0,OffSet smallint null default 0,AuxUnitCode nvarchar(35) null,  \r\n";
                APS21_CreateTempTable += " ChangeRate  decimal(22,6) null default 0,AuxBaseQtyN decimal(28,6) null default 0,AuxQty decimal(28,6) null default 0,MoDId int null,Remark nvarchar(255) null,InvCode nvarchar(20) null,Free1 nvarchar(20) null,  \r\n";
                APS21_CreateTempTable += " Free2 nvarchar(20) null,Free3 nvarchar(20) null,Free4 nvarchar(20) null,Free5 nvarchar(20) null,Free6 nvarchar(20) null,Free7 nvarchar(20) null,Free8 nvarchar(20) null,Free9 nvarchar(20) null,Free10 nvarchar(20) null,  \r\n";
                APS21_CreateTempTable += " Define22 nvarchar(60) null,Define23 nvarchar(60) null,Define24 nvarchar(60) null,Define25 nvarchar(60) null,Define26 float null,Define27 float null,Define28 nvarchar(120) null,Define29 nvarchar(120) null,  \r\n";
                APS21_CreateTempTable += " Define30 nvarchar(120) null,Define31 nvarchar(120) null,Define32 nvarchar(120) null,Define33 nvarchar(120) null,Define34 int null,Define35 int null,Define36 datetime null,Define37 datetime null,  \r\n";
                APS21_CreateTempTable += " CopyFlag bit null default 0,UseQty decimal(28,6) null default 0,AuxUseQty decimal(28,6) null default 0,ParentDId int null,DId int null default 0,CopyDId int IDENTITY(1,1),TopCompFlag bit null default 0,  \r\n";
                APS21_CreateTempTable += " BomType tinyint null default 0,SoType tinyint null default 0,SoDId nvarchar(30) null,SoCode nvarchar(30) null,SoSeq int null,DemandCode  nvarchar(30) null,CopyAllocateId int null default 0);    \r\n";
                APS21_CreateTempTable += "    \r\n";
                APS21_CreateTempTable += "  create index idx_tmp_bomcomponent_1 on #tmp_bomcomponent (MoDId,ParentId,ComponentId);     \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create index idx_tmp_bomcomponent_2 on #tmp_bomcomponent (WhCode);     \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create table #tmp_bomdown ( MoDId int not null,OpSeq nchar(4) null,PartId int null,EffBegDate  datetime null,ParentScrap decimal(28,6) null default 0,BaseQtyN decimal(28,6) null default 0,  \r\n";
                APS21_CreateTempTable += " BaseQtyD decimal(28,6) null default 0,CompScrap decimal(28,6) null default 0,Qty decimal(28,6) null default 0,ByproductFlag bit null,ProductType tinyint null default 1,  \r\n";
                APS21_CreateTempTable += " WIPType tinyint null default 3,WhCode nvarchar(10) null,StartDemDate datetime null,EndDemDate  datetime null,Level smallint null,FVFlag tinyint null default 1,OpComponentId int null default 0,  \r\n";
                APS21_CreateTempTable += " OffSet smallint null default 0,AuxUnitCode nvarchar(35) null,ChangeRate  decimal(22,6) null default 0,AuxBaseQtyN decimal(28,6) null default 0,AuxQty decimal(28,6) null default 0,  \r\n";
                APS21_CreateTempTable += " WIPFlag bit null default 0,Remark nvarchar(255) null,InvCode nvarchar(20) null,Free1 nvarchar(20) null,Free2 nvarchar(20) null,Free3 nvarchar(20) null,Free4 nvarchar(20) null,Free5 nvarchar(20) null,  \r\n";
                APS21_CreateTempTable += " Free6 nvarchar(20) null,Free7 nvarchar(20) null,Free8 nvarchar(20) null,Free9 nvarchar(20) null,Free10 nvarchar(20) null,Define22 nvarchar(60) null,Define23 nvarchar(60) null,Define24 nvarchar(60) null,  \r\n";
                APS21_CreateTempTable += " Define25 nvarchar(60) null,Define26 float null,Define27 float null,Define28 nvarchar(120) null,Define29 nvarchar(120) null,Define30 nvarchar(120) null,Define31 nvarchar(120) null,Define32 nvarchar(120) null,  \r\n";
                APS21_CreateTempTable += " Define33 nvarchar(120) null,Define34 int null,Define35 int null,Define36 datetime null,Define37 datetime null,ParentDId int null,DId int IDENTITY(1,1),StartDate datetime null,LeadTime int null,  \r\n";
                APS21_CreateTempTable += " fAlterBaseNum decimal(28,6) null default 0,iAlterAdvance int null,UseQty decimal(28,6) null default 0,AuxUseQty decimal(28,6) null default 0,BomId int null,RoutingId int null,pwiptype int null,  \r\n";
                APS21_CreateTempTable += " Parentid int null,MinOpSeq nchar(4) null default '0000',rauxuseqty  decimal(28,6) null default 0,ruseqty decimal(28,6) null default 0,TopCompFlag bit null default 0,SoType tinyint null default 0,\r\n";
                APS21_CreateTempTable += " SoDId nvarchar(30) null,SoSeq int null,SoCode nvarchar(30) null,DemandCode  nvarchar(30) null,CalendarId  int null,PDId  int null,BomType tinyint null default 0 ,RoutingType tinyint null default 0);     \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create index idx_tmp_bomdown_1 on #tmp_bomdown (MoDId,PartId);     \r\n";
                APS21_CreateTempTable += "  create index idx_tmp_bomdown_2 on #tmp_bomdown (WIPFlag);     \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create table #tmp_procmo ( MoDId int not null,BomId int null default 0,RoutingId int null default 0,CopyMoDId int null default 0,CopyBomId int null  default 0,CopyRoutingId int null  default 0,  \r\n";
                APS21_CreateTempTable += " STProxyWhFlag bit null default 1,TopFlag bit null default 0);    \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create index idx_tmp_procmo on #tmp_procmo (MoDId);    \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create table #tmp_procerror ( MoDId int not null,MoCode  nvarchar(30)  not null,SortSeq  int not null default 0,errorno nvarchar(20) null,InvCode nvarchar(20) null,Free1 nvarchar(20) null,Free2 nvarchar(20) null,  \r\n";
                APS21_CreateTempTable += " Free3 nvarchar(20) null,Free4 nvarchar(20) null,Free5 nvarchar(20) null,Free6 nvarchar(20) null,Free7 nvarchar(20) null,Free8 nvarchar(20) null,Free9 nvarchar(20) null,Free10 nvarchar(20) null );    \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create index idx_tmp_procerror on #tmp_procerror (MoCode,SortSeq);   \r\n";
                APS21_CreateTempTable += "    \r\n";
                APS21_CreateTempTable += "  create table #tmp_collectivemo ( RootMoDId int null,ParentMoDId int null,MoDId int null,PartId int null,StartDate datetime null,DueDate datetime null,BomId int null,RoutingId int null,AuxUnitCode nvarchar(35) null,  \r\n";
                APS21_CreateTempTable += " ChangeRate  decimal(22,6) null default 0,Qty decimal(28,6) null default 0,AuxQty decimal(28,6) null default 0,Level smallint null,PAllocateId int null,SortSeq int IDENTITY(1,1),ParentIdFlag bit null default 0,  \r\n";
                APS21_CreateTempTable += " OrderFlag bit null default 0,SoType tinyint null default 0,SoDId nvarchar(30) null,SoCode nvarchar(30) null,SoSeq int null,DemandCode  nvarchar(30) null,BomType tinyint null default 0,RoutingType tinyint null default 0,  \r\n";
                APS21_CreateTempTable += " Free1 nvarchar(20) null,Free2 nvarchar(20) null,Free3 nvarchar(20) null,Free4 nvarchar(20) null,Free5 nvarchar(20) null,Free6 nvarchar(20) null,Free7 nvarchar(20) null,Free8 nvarchar(20) null,Free9 nvarchar(20) null,Free10 nvarchar(20) null );    \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create table #tmp_recordcount ( ParentCnt int null,ComponentCnt int null );    \r\n";
                APS21_CreateTempTable += "  create table #tmp_updhead ( MoDId int null,Qty decimal(28,6) null default 0,OldQty decimal(28,6) null default 0,StartDate datetime null,OldStartDate datetime null,OpScheduleType tinyint null,OrderType tinyint null,OrderDId int null,  \r\n";
                APS21_CreateTempTable += " OrderCode nvarchar(30) null,OrderSeq int null,SoType tinyint null default 0,SoDId nvarchar(30) null,SoCode nvarchar(30) null,SoSeq int null,DemandCode  nvarchar(30) null,UpdSoFlag bit null default 0 );    \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create table #tmp_upddetail ( AllocateId int null,Qty decimal(28,6) null default 0,OldQty decimal(28,6) null default 0,StartDemDate datetime null,OldStartDemDate  datetime null,OpScheduleType tinyint null,  \r\n";
                APS21_CreateTempTable += " Offset int null,InvCode nvarchar(20) null,StartDate datetime null,upddateflag bit null default 0,Level tinyint null default 0,PartId int null,SoType tinyint null default 0,SoDId nvarchar(30) null,SoCode nvarchar(30) null,  \r\n";
                APS21_CreateTempTable += " SoSeq int null,DemandCode  nvarchar(30) null,UpdSoFlag bit null default 0, RQty  decimal(28,6) null default 0 );    \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create table #tmp_updwipallocateid ( MoDId int null,AllocateId  int null,DId int IDENTITY(1,1),PartId int null,StartDemDate datetime null,AuxUnitCode nvarchar(35) null,ChangeRate  decimal(22,6) null default 0,  \r\n";
                APS21_CreateTempTable += " Qty decimal(28,6) null default 0,AuxQty decimal(28,6) null default 0);    \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create table #tmp_deldid ( Type tinyint null,DId  int null,Level int null default(0),MoId int null default(0),PAllocateId  int null default(0));    \r\n";
                APS21_CreateTempTable += "   \r\n";
                APS21_CreateTempTable += "  create table #STPEUnlockID ( iSoType tinyint null,iSoDId  nvarchar(40) null,PartId  int null );    \r\n";

                mom_moallocateColumns = "MoDId,SortSeq,OpSeq,ComponentId,FVFlag,BaseQtyN,BaseQtyD,ParentScrap,CompScrap,Qty,IssQty,DeclaredQty,StartDemDate,EndDemDate,WhCode,LotNo,  \r\n";
                mom_moallocateColumns += " WIPType,ByproductFlag,QcFlag,Offset,InvCode,Free1,Free2,Free3,Free4,Free5,Free6,Free7,Free8,Free9,Free10,OpComponentId,Define22,Define23,Define24,Define25,  \r\n";
                mom_moallocateColumns += " Define26,Define27,Define28,Define29,Define30,Define31,Define32,Define33,Define34,Define35,Define36,Define37,AuxUnitCode,ChangeRate,AuxBaseQtyN,AuxQty,ReplenishQty,  \r\n";
                mom_moallocateColumns += " Remark,TransQty,ProductType,SoType,SoDId,SoCode,SoSeq,DemandCode,QmFlag,OrgQty,OrgAuxQty,CostItemCode,CostItemName  \r\n";

                if (!IsRefresh)
                {
                    if (wSQL.Length == 0)
                    {
                        frmAPSQuery fQuery = new frmAPSQuery(iLoginEx);
                        fQuery.ShowDialog(this);

                        if (fQuery.GetSelectSQL().Length <= 0)
                        {
                            return;
                        }
                        wSQL = fQuery.GetSelectSQL();

                        SQLSelect = wSQL;
                    }
                    selectSQL = "if  not exists (select 1  from tempdb.dbo.sysobjects  (nolock) where upper(name)=upper('zhrs_t_aps2101') and type='U')   \r\n";
                    selectSQL += " begin  \r\n";
                    selectSQL += " CREATE TABLE tempdb.dbo.zhrs_t_aps2101(  \r\n";
                    selectSQL += "     cUser_ID int not null,  \r\n";
                    selectSQL += "    ReScheduleType tinyint not null default 0,  --1=拆分（源单）；2=合并（源单）；11=拆分（新单）；22=合并（新单） \r\n";
                    selectSQL += "    ReScheduleSourceMoId int not null default 0, --源制造单ID \r\n";
                    selectSQL += "    ReScheduleSourceMoDId int not null default 0, --源制造单行ID \r\n";
                    selectSQL += "     ViewSort int  null default 0,  \r\n";
                    selectSQL += "   MoClass tinyint not null default 1, \r\n";
                    selectSQL += " 	部门 nvarchar(255) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	优先级 int NULL,  \r\n";
                    selectSQL += " 	制造单号 nvarchar(30) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	行号M int NOT NULL,  \r\n";
                    selectSQL += " 	销售订单 nvarchar(30) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	业务员 nvarchar(40) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	行号S int NULL,  \r\n";
                    selectSQL += " 	产品编码 nvarchar(20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	产品名称 nvarchar(255) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	规格型号 nvarchar(255) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	投产 bit NULL,  \r\n";
                    selectSQL += " 	计划生产日期 datetime NULL,  \r\n";
                    selectSQL += " 	排产数量 decimal(28, 6) NOT NULL,  \r\n";
                    selectSQL += " 	完工数量 decimal(28, 6) NOT NULL,  \r\n";
                    selectSQL += " 	结案 bit NULL,  \r\n";
                    selectSQL += " 	LOGO nvarchar(120) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	软件信息 nvarchar(120) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	备注 nvarchar(120) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	欠料提示 nvarchar(255) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	调库存 bit NULL,  \r\n";
                    selectSQL += " 	MoId int NOT NULL default 0,  \r\n";
                    selectSQL += " 	ModId int NOT NULL default 0,RowColor varchar(1500) not null default 'White',  \r\n";
                    selectSQL += "  rowid int IDENTITY(1,1) NOT NULL)   \r\n";
                    selectSQL += "CREATE NONCLUSTERED INDEX [tempdb.dbo.idx_zhrs_t_aps2101] ON tempdb.dbo.zhrs_t_aps2101 ( cUser_ID,  \r\n";
                    selectSQL += "MoId,ModId )  \r\n";
                    selectSQL += " end \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();


                    myCommand.CommandText = "delete tempdb.dbo.zhrs_t_aps2101 where cUser_ID=" + iLoginEx.UID().ToString() + "";
                    myCommand.ExecuteNonQuery();

                    selectSQL = " insert into tempdb.dbo.zhrs_t_aps2101(cUser_ID,ViewSort,MoClass,部门,优先级,制造单号,行号M,销售订单,业务员,行号S,产品编码,产品名称,规格型号,投产,计划生产日期,排产数量,完工数量,结案,LOGO,软件信息,备注,调库存,MoId,ModId)\r\n" + wSQL + "order by  d.cDepName,c.MoCode,a.sortseq   \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();


                    selectSQL = " insert into zhrs_t_mom_orderdetail_userDefine(MoId,ModId)  \r\n";
                    selectSQL += " select MoId,ModId from tempdb.dbo.zhrs_t_aps2101 m (nolock) where cUser_ID=" + iLoginEx.UID().ToString() + "  \r\n";
                    selectSQL += "  and  not exists(select 1 from zhrs_t_mom_orderdetail_userDefine a (nolock) where a.MoId=m.MoId and a.ModId=m.ModId )  \r\n";

                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();


                    selectSQL = " update tempdb.dbo.zhrs_t_aps2101 set RowColor=m.RowColor   \r\n";
                    selectSQL += " from (select MoID,MoDId,RowColor from zhrs_t_mom_orderdetail_userDefine (nolock) ) m  \r\n";
                    selectSQL += "  where tempdb.dbo.zhrs_t_aps2101.MoID=m.MoID and tempdb.dbo.zhrs_t_aps2101.MoDId=m.MoDId  and tempdb.dbo.zhrs_t_aps2101.cUser_ID=" + iLoginEx.UID().ToString() + " \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();

                    wSQL = "select 优先级,制造单号,行号M,产品编码,产品名称,规格型号,投产,调库存,计划生产日期,排产数量,完工数量,结案,convert(varchar(10), case when MoClass=1 then '标准' else '非标' end) as '类别',销售订单,业务员,行号S,LOGO,软件信息,备注,欠料提示,部门,MoId,ModId,rowid from tempdb.dbo.zhrs_t_aps2101 where  cUser_ID=" + iLoginEx.UID().ToString();
                    wSQL += " order by ViewSort,优先级,产品编码,业务员";
                    SQLSelect_Temp = wSQL;


                }

                myCommand.CommandText = wSQL;
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


                for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
                {
                    tab6_dataGridView1.Columns[i].ReadOnly = true;
                    tab6_dataGridView1.Columns[i].DefaultCellStyle.Font = new Font("宋体", iLoginEx.ReadUserProfileValue("APS", "FontSize") == "" ? 9 : Convert.ToInt32(iLoginEx.ReadUserProfileValue("APS", "FontSize")));
                }

                string DataPropertyNames1 = "结案,行号M,行号S,投产", DataPropertyNames2 = "MoId,ModId", DataPropertyNames3 = "排产数量,完工数量";
                for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
                {
                    if (DataPropertyNames1.IndexOf(tab6_dataGridView1.Columns[i].DataPropertyName) > -1)
                    {
                        tab6_dataGridView1.Columns[i].Width = 50;
                    }

                    if (DataPropertyNames2.IndexOf(tab6_dataGridView1.Columns[i].DataPropertyName) > -1)
                    {
                        tab6_dataGridView1.Columns[i].Width = 5;
                    }

                    if (DataPropertyNames3.IndexOf(tab6_dataGridView1.Columns[i].DataPropertyName) > -1)
                    {
                        tab6_dataGridView1.Columns[i].DefaultCellStyle.Format = "#,###0";
                        tab6_dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                Tab6_SaveCulomnsWidth = true;

                //读取用户自定列宽

                if (tab6_dataGridView1.Columns.Count > 0)
                {

                    string ColumnsWidths = iLoginEx.ReadUserProfileValue("APS", "ColumnsWidths");
                    string[] ColumnsWidthsPara = ColumnsWidths.Split(';');
                    for (int i = 0; i < ColumnsWidthsPara.Length && i < tab6_dataGridView1.Columns.Count; i++)
                    {
                        if (ColumnsWidthsPara[i].Length > 0)
                        {
                            tab6_dataGridView1.Columns[i].Width = Convert.ToInt32(ColumnsWidthsPara[i]);
                        }
                    }
                }



                //显示用户自定的颜色

                if (iLoginEx.ReadUserProfileValue("APSQuery", "chkNoShowColor") != "1")
                {
                    string[] RowColorsPara = null;

                    for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                    {
                        myCommand.CommandText = "select isnull(RowColor,'') as 'RowColor' from tempdb.dbo.zhrs_t_aps2101 (nolock) where cUser_ID=" + iLoginEx.UID().ToString() + " and MoId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value);
                        myReader = myCommand.ExecuteReader();
                        if (myReader.Read())
                        {
                            RowColorsPara = myReader["RowColor"].ToString().Split(';');
                        }
                        myReader.Close();
                        // myReader.Dispose();

                        for (int c = 0; c < RowColorsPara.Length && c < tab6_dataGridView1.Columns.Count; c++)
                        {
                            if (RowColorsPara[c].Length > 0)
                            {
                                if (RowColorsPara[c].Length == 0)
                                {
                                    tab6_dataGridView1.Rows[i].Cells[c].Style.BackColor = ColorTranslator.FromHtml("White");
                                }
                                else
                                {
                                    tab6_dataGridView1.Rows[i].Cells[c].Style.BackColor = ColorTranslator.FromHtml(RowColorsPara[c]);
                                }
                            }
                        }
                    }
                }

                //冻结列
                if (iLoginEx.ReadUserProfileValue("APS", "ColumnFrozen").Length > 0)
                {
                    tab6_dataGridView1.Columns[Convert.ToInt32(iLoginEx.ReadUserProfileValue("APS", "ColumnFrozen"))].Frozen = true;
                }

                tab6_dataGridView1.Columns["MoID"].Visible = false;
                tab6_dataGridView1.Columns["MoDID"].Visible = false;
                tab6_dataGridView1.Columns["RowID"].Visible = false;

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }



                // MessageBox.Show(tab6_dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Displayed).ToString() );    


            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21Query()");
                frmmsg.ShowDialog(this);
            }

        }




        /// <summary>
        /// 排产管理--暂存
        /// </summary>
        private bool APS21Save_TempTable()
        {
            try
            {
                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();
                string RowColors = "";
                string selectSQL = "";

                OleDbCommand myCommand = new OleDbCommand("", myConn);


                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToInt64(tab6_dataGridView1.Rows[i].Cells["排产数量"].Value) < 0)
                    {
                        MessageBox.Show(this, "制造单：" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value) + "，行号：" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value) + "，排产数量不能小于0！", "排产管理", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    if (Convert.ToInt64(tab6_dataGridView1.Rows[i].Cells["排产数量"].Value) < Convert.ToInt64(tab6_dataGridView1.Rows[i].Cells["完工数量"].Value))
                    {
                        MessageBox.Show(this, "制造单：" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value) + "，行号：" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value) + "，排产数量不能小于完工数量！", "排产管理", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    RowColors = "";
                    selectSQL = "";
                    for (int c = 0; c < tab6_dataGridView1.Columns.Count; c++)
                    {
                        if (ColorTranslator.ToHtml(tab6_dataGridView1.Rows[i].Cells[c].Style.BackColor) == "White")
                        {
                            RowColors += ";";
                        }
                        else
                        {
                            RowColors += ColorTranslator.ToHtml(tab6_dataGridView1.Rows[i].Cells[c].Style.BackColor) + ";";
                        }
                        if (!tab6_dataGridView1.Columns[c].ReadOnly)
                        {
                            if (tab6_dataGridView1.Columns[c].GetType().Name == "DataGridViewCheckBoxColumn")
                            {
                                selectSQL += tab6_dataGridView1.Columns[c].DataPropertyName + "=" + Convert.ToByte(tab6_dataGridView1.Rows[i].Cells[c].Value).ToString() + ",";
                            }
                            else
                            {
                                selectSQL += tab6_dataGridView1.Columns[c].DataPropertyName + "='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells[c].Value) + "',";
                            }
                        }
                    }
                    RowColors += iLoginEx.Chr(8);
                    RowColors = RowColors.Replace(";" + iLoginEx.Chr(8), "");

                    if (selectSQL.Length > 0)
                    {
                        selectSQL = " update tempdb.dbo.zhrs_t_aps2101 set RowColor='" + RowColors + "'," + selectSQL + iLoginEx.Chr(8);
                        selectSQL = selectSQL.Replace("," + iLoginEx.Chr(8), "") + " where cUser_ID=" + iLoginEx.UID().ToString() + " and MoId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value);
                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        selectSQL = " update tempdb.dbo.zhrs_t_aps2101 set RowColor='" + RowColors + "' where cUser_ID=" + iLoginEx.UID().ToString() + " and MoId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value);
                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();
                    }


                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21Save_TempTable()");
                frmmsg.ShowDialog(this);
                return false;
            }

        }
        //

        /// <summary>
        /// 排产管理--拆单
        /// </summary>      
        /// <param name="MoDId">生产订单表体ID</param>
        /// <returns></returns>
        private bool APS21_OrderSplit()
        {
            string selectSQL = "";
            string OldMoId = "0", OldMoDId = "0", NewMoId = "0", NewMoDId = "0";
            string Qty = "0";
            string MoCode = "";

            OleDbConnection myConnRead = new OleDbConnection(iLoginEx.ConnString());

            if (myConnRead.State == System.Data.ConnectionState.Open)
            {
                myConnRead.Close();
            }
            myConnRead.Open();



            OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

            if (myConn.State == System.Data.ConnectionState.Open)
            {
                myConn.Close();
            }
            myConn.Open();



            OleDbCommand myCommandRead = new OleDbCommand("", myConnRead);
            OleDbCommand myCommandEx = new OleDbCommand("", myConn);
            OleDbDataReader myReader = null;
            OleDbDataReader myReaderEx = null;


            try
            {

                DoOrderSplit = false;
                string ROWID = "";

                //拆出来的新单
                selectSQL = " select rowid,ReScheduleSourceMoId,ReScheduleSourceMoDId,排产数量,制造单号 from tempdb.dbo.zhrs_t_aps2101  where cUser_ID=" + iLoginEx.UID().ToString() + " and ReScheduleSourceMoDId>0 and ReScheduleType=1 \r\n";
                myCommandRead.CommandText = selectSQL;
                myReader = myCommandRead.ExecuteReader();
                while (myReader.Read())
                {
                    OldMoId = myReader["ReScheduleSourceMoId"].ToString();
                    OldMoDId = myReader["ReScheduleSourceMoDId"].ToString();
                    Qty = myReader["排产数量"].ToString();
                    MoCode = myReader["制造单号"].ToString();
                    ROWID = myReader["rowid"].ToString();

                    selectSQL = "declare @p5 int  \r\n";
                    selectSQL += " set @p5=0  \r\n";
                    selectSQL += " declare @p6 int  \r\n";
                    selectSQL += " set @p6=0  \r\n";
                    selectSQL += " exec sp_GetID @RemoteId=N'00',@cAcc_Id=N'" + iLoginEx.AccID() + "',@cVouchType=N'mom_order',@iAmount=1,@iFatherId=@p5 output,@iChildId=@p6 output  \r\n";
                    selectSQL += " select @p5 as 'maxMoId', @p6 as 'mom_order_iChildId'  \r\n";


                    myCommandEx.CommandText = selectSQL;
                    myReaderEx = myCommandEx.ExecuteReader();
                    if (myReaderEx.Read())
                    {
                        NewMoId = Convert.ToString(myReaderEx["maxMoId"]);
                        //mom_order_iChildId = Convert.ToInt64(myReader["mom_order_iChildId"]);
                        // myReader.Close();
                    }
                    myReaderEx.Close();

                    selectSQL = "declare @p5 int  \r\n";
                    selectSQL += " set @p5=" + NewMoId.ToString() + "  \r\n";
                    selectSQL += " declare @p6 int  \r\n";
                    selectSQL += " set @p6=0  \r\n";
                    selectSQL += " exec sp_GetID @RemoteId=N'00',@cAcc_Id=N'" + iLoginEx.AccID() + "',@cVouchType=N'mom_orderdetail',@iAmount=1,@iFatherId=@p5 output,@iChildId=@p6 output  \r\n";
                    selectSQL += " select @p5 as 'mom_orderdetail_iFatherId', @p6 as 'maxMoDId'  \r\n";


                    myCommandEx.CommandText = selectSQL;
                    myReaderEx = myCommandEx.ExecuteReader();
                    if (myReaderEx.Read())
                    {
                        //mom_orderdetail_iFatherId = Convert.ToInt64(myReader["mom_orderdetail_iFatherId"]);
                        NewMoDId = Convert.ToString(myReaderEx["maxMoDId"]);

                    }
                    myReaderEx.Close();



                    selectSQL = "INSERT INTO mom_order(MoId,MoCode,CreateDate,CreateTime,CreateUser,ModifyDate,ModifyTime,ModifyUser,UpdCount ,Define1,Define2,Define3,Define4,Define5,Define6,Define7,Define8,Define9,Define10,Define11,Define12,Define13,Define14,Define15,Define16 ,VTid) \r\n";
                    selectSQL += " select MoId=" + NewMoId + ",MoCode='" + MoCode + "',CreateDate,CreateTime,CreateUser,ModifyDate,ModifyTime,ModifyUser,UpdCount ,Define1,Define2,Define3,Define4,Define5,Define6,Define7,Define8,Define9,Define10,Define11,Define12,Define13,Define14,Define15,Define16 ,VTid from  mom_order (nolock) \r\n";
                    selectSQL += "  where MoId=" + OldMoId + " \r\n";
                    selectSQL += "  \r\n";
                    selectSQL += "  \r\n";
                    selectSQL += " insert into mom_orderdetail(MoDId,MoId,SortSeq,MoClass,MoTypeId,Qty,MrpQty,AuxUnitCode,AuxQty,ChangeRate,MoLotCode,WhCode,MDeptCode,SoType,SoDId,  \r\n";
                    selectSQL += " SoCode,SoSeq,DeclaredQty,QualifiedInQty,Status,OrgStatus,BomId,RoutingId,CustBomId,DemandId,PlanCode,PartId,InvCode,  \r\n";
                    selectSQL += " Free1,Free2,Free3,Free4,Free5,Free6,Free7,Free8,Free9,Free10,SfcFlag,CrpFlag,QcFlag,RelsDate,RelsUser,CloseDate,  \r\n";
                    selectSQL += " OrgClsDate,Define22,Define23,Define24,Define25,Define26,Define27,Define28,Define29,Define30,Define31,Define32,  \r\n";
                    selectSQL += " Define33,Define34,Define35,Define36,Define37,LeadTime,OpScheduleType,OrdFlag,WIPType,SupplyWhCode,ReasonCode,IsWFControlled,  \r\n";
                    selectSQL += " iVerifyState,iReturnCount,Remark,SourceMoCode,SourceMoSeq,SourceMoId,SourceMoDId,SourceQCCode,SourceQCId,SourceQCDId,  \r\n";
                    selectSQL += " CostItemCode,CostItemName,RelsTime,CloseUser,CloseTime,OrgClsTime,AuditStatus,PAllocateId,DemandCode,CollectiveFlag,OrderType,  \r\n";
                    selectSQL += " OrderDId,OrderCode,OrderSeq,ManualCode,ReformFlag,SourceQCVouchType,OrgQty,FmFlag,MinSN,MaxSN,SourceSvcCode,SourceSvcId,  \r\n";
                    selectSQL += " SourceSvcDId,BomType,RoutingType,BusFlowId,RunCardFlag)  \r\n";
                    selectSQL += " select MoDId=" + NewMoDId + ",MoId=" + NewMoId + ",SortSeq=1,MoClass,MoTypeId,Qty=" + Qty + ",MrpQty=" + Qty + ",AuxUnitCode,AuxQty,ChangeRate,MoLotCode,WhCode,MDeptCode,SoType,SoDId,  \r\n";
                    selectSQL += " SoCode,SoSeq,DeclaredQty,QualifiedInQty,Status,OrgStatus,BomId,RoutingId,CustBomId,DemandId,PlanCode,PartId,InvCode,  \r\n";
                    selectSQL += " Free1,Free2,Free3,Free4,Free5,Free6,Free7,Free8,Free9,Free10,SfcFlag,CrpFlag,QcFlag,RelsDate,RelsUser,CloseDate,  \r\n";
                    selectSQL += " OrgClsDate,Define22,Define23,Define24,Define25,Define26,Define27,Define28,Define29,Define30,Define31,Define32,  \r\n";
                    selectSQL += " Define33,Define34,Define35,Define36,Define37,LeadTime,OpScheduleType,OrdFlag,WIPType,SupplyWhCode,ReasonCode,IsWFControlled,  \r\n";
                    selectSQL += " iVerifyState,iReturnCount,Remark,SourceMoCode,SourceMoSeq,SourceMoId,SourceMoDId,SourceQCCode,SourceQCId,SourceQCDId,  \r\n";
                    selectSQL += " CostItemCode,CostItemName,RelsTime,CloseUser,CloseTime,OrgClsTime,AuditStatus,PAllocateId,DemandCode,CollectiveFlag,OrderType,  \r\n";
                    selectSQL += " OrderDId,OrderCode,OrderSeq,ManualCode,ReformFlag,SourceQCVouchType,OrgQty,FmFlag,MinSN,MaxSN,SourceSvcCode,SourceSvcId,  \r\n";
                    selectSQL += " SourceSvcDId,BomType,RoutingType,BusFlowId,RunCardFlag from mom_orderdetail (nolock) where MoId=" + OldMoId + " and MoDId=" + OldMoDId + "  \r\n";
                    selectSQL += "  \r\n";
                    selectSQL += "  \r\n";
                    selectSQL += " INSERT INTO mom_morder(MoDId, StartDate, DueDate, MoId) \r\n";
                    selectSQL += " select MoDId=" + NewMoDId + ", StartDate, DueDate, MoId=" + NewMoId + " from  mom_morder (nolock) where MoId=" + OldMoId + " and MoDId=" + OldMoDId + " \r\n";

                    myCommandEx.CommandText = selectSQL;
                    myCommandEx.ExecuteNonQuery();

                    APS21_ReLoadBOM(false, MoCode, "1", NewMoId, NewMoDId);//生成子件

                    selectSQL = " update tempdb.dbo.zhrs_t_aps2101 set MoId=" + NewMoId + ",MoDId=" + NewMoDId + " where rowid=" + ROWID;
                    myCommandEx.CommandText = selectSQL;
                    myCommandEx.ExecuteNonQuery();

                    selectSQL = " insert into zhrs_t_mom_orderdetail_userDefine(MoId,ModId)  \r\n";
                    selectSQL += " select MoId,ModId from tempdb.dbo.zhrs_t_aps2101 m (nolock) where m.cUser_ID=" + iLoginEx.UID().ToString() + "  \r\n";
                    selectSQL += "  and  not exists(select 1 from zhrs_t_mom_orderdetail_userDefine a (nolock) where a.MoId=m.MoId and a.ModId=m.ModId ) and m. MoId=" + NewMoId + "and m.MoDId=" + NewMoDId + "  \r\n";

                    myCommandEx.CommandText = selectSQL;
                    myCommandEx.ExecuteNonQuery();


                }
                myReader.Close();

                //拆分后的原单
                selectSQL = " select MoId,MoDId,排产数量 from tempdb.dbo.zhrs_t_aps2101  where cUser_ID=" + iLoginEx.UID().ToString() + " and ReScheduleSourceMoDId=0 and ReScheduleType=1 \r\n";
                myCommandRead.CommandText = selectSQL;
                myReader = myCommandRead.ExecuteReader();
                while (myReader.Read())
                {
                    OldMoId = myReader["MoId"].ToString();
                    OldMoDId = myReader["MoDId"].ToString();
                    Qty = myReader["排产数量"].ToString();
                    //MoCode = myReader["制造单号"].ToString();


                    //将原单号的排产量改为拆分后的数量
                    selectSQL = "update mom_orderdetail set Qty=" + Qty + ", MrpQty=" + Qty + " where MoId=" + OldMoId + " and MoDId=" + OldMoDId + "  \r\n";
                    myCommandEx.CommandText = selectSQL;
                    myCommandEx.ExecuteNonQuery();

                    //重算子件用量
                    selectSQL = "update mom_moallocate set mom_moallocate.Qty=(isnull(mom_moallocate.BaseQtyN,0)/case when isnull(mom_moallocate.BaseQtyD,1)=0 then 1 else isnull(mom_moallocate.BaseQtyD,1) end)*m.Qty from   \r\n";
                    selectSQL += " (select MoDId,Qty from mom_orderdetail (nolock)  where  MoId=" + OldMoId + " and MoDId=" + OldMoDId + "  ) m where m.MoDId=mom_moallocate.MoDId   and  mom_moallocate.MoDId=" + OldMoDId + " \r\n";
                    myCommandEx.CommandText = selectSQL;
                    myCommandEx.ExecuteNonQuery();

                }
                myReader.Close();


                if (myConnRead.State == System.Data.ConnectionState.Open)
                {
                    myConnRead.Close();
                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {


                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21_IntoOperation()");
                frmmsg.ShowDialog(this);

                selectSQL = "delete mom_orderdetail_zhrs_mrp where MoId=" + NewMoId + " \r\n";
                selectSQL += " delete mom_moallocate where  MoDid in (select MoDid from mom_orderdetail (nolock)  where MoId=" + NewMoId + ")  \r\n";
                selectSQL += "delete mom_morder where  MoId=" + NewMoId + " \r\n";
                selectSQL += "delete mom_orderdetail where MoId=" + NewMoId + " \r\n";
                selectSQL += "delete mom_order where  MoId=" + NewMoId + " \r\n";
                myCommandEx.CommandText = selectSQL;
                myCommandEx.ExecuteNonQuery();

                if (myConnRead.State == System.Data.ConnectionState.Open)
                {
                    myConnRead.Close();
                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }

                return false;
            }
        }


        /// <summary>
        /// 排产管理--结案
        /// </summary>
        /// <param name="ActType">true=结案;False=取消结案</param>
        /// <param name="MoDId">生产订单表体ID</param>
        /// <returns></returns>
        private bool APS21_Closed(bool ActType, string MoID, string MoDId)
        {
            try
            {
                bool DoIt = false;

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                string selectSQL = "";

                OleDbCommand myCommand = new OleDbCommand("", myConn);
                OleDbDataReader myReader = null;



                if (ActType)//结案                
                {

                    //如果是已结案，则跳过
                    selectSQL = " select 1 as 'a' from  mom_orderdetail (nolock)  where len(isnull(CloseUser,''))<=0 and moid=" + MoID + " and modid=" + MoDId + "\r\n";
                    myCommand.CommandText = selectSQL;
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        DoIt = true;
                    }
                    myReader.Close();
                    if (DoIt)
                    {
                        selectSQL = "update mom_orderdetail set OrgStatus=Status,Status=4, CloseTime='" + iLoginEx.GetDBServeCurrentDateTime() + "' ,CloseUser='" + iLoginEx.UserId() + "' where moid=" + MoID + "  and modid=" + MoDId + "\r\n";
                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();

                    }
                }
                else
                {
                    //如果是已结案且未完工，则取消结案。如果已完工，则不允许取消结案
                    selectSQL = " select 1 as 'a' from mom_orderdetail (nolock)  where  len(isnull(CloseUser,''))>0 and (isnull(qty,0)-isnull(QualifiedInQty,0))>0 and moid=" + MoID + " and modid=" + MoDId + " \r\n";
                    myCommand.CommandText = selectSQL;
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        DoIt = true;
                    }
                    myReader.Close();
                    if (DoIt)
                    {
                        selectSQL = "update mom_orderdetail set Status=3,CloseUser=null, CloseTime=null  where moid=" + MoID + "  and modid=" + MoDId + "\r\n";
                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();
                    }
                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21_IntoOperation()");
                frmmsg.ShowDialog(this);
                return false;
            }
        }


        /// <summary>
        /// 排产管理--重载BOM（菜单）
        /// </summary>
        /// <param name="ActType">true=投产;False=取消投产</param>
        /// <param name="MoDId">生产订单表体ID</param>
        /// <param name="ComponentInvCode">子件物料编码</param>
        /// <returns></returns>
        private bool APS21_ReLoadBOMByManual(string MoCode, string SortSeq, string MoID, string MoDId)
        {
            try
            {
                //if (MessageBox.Show("您确定要对制造单号：" + MoCode + " ;行号：" + SortSeq + "，进行重载BOM吗？", "重载BOM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{

                bool DoIt = true;

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                string selectSQL = "";

                OleDbCommand myCommand = new OleDbCommand("", myConn);
                myCommand.CommandTimeout = 3600;
                OleDbDataReader myReader = null;

                //如果是调库存，则投产时不需要重载BOM
                selectSQL = " select  ReProduce from zhrs_t_mom_orderdetail_userDefine (nolock)  where  ReProduce=1 and moid=" + MoID + " and modid=" + MoDId + "\r\n";
                myCommand.CommandText = selectSQL;
                myReader = myCommand.ExecuteReader();
                if (myReader.Read())
                {
                    DoIt = false;
                    MessageBox.Show(this, "制造单号：" + MoCode + " ;行号：" + SortSeq + " 为“调库存”的制造单。\r\n\r\n\r\n此类制造单，禁止重载BOM！", "重载BOM", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                myReader.Close();


                if (DoIt)
                {
                    //重载BOM时，需要考虑已发料部分
                    //if (APS21_ReLoadBOM(true, MoCode, SortSeq, MoID, MoDId))
                    // {
                    //    MessageBox.Show(this, "制造单号：" + MoCode + " ;行号：" + SortSeq + "   重载BOM完成！", "重载BOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //  }
                    APS21_ReLoadBOM(true, MoCode, SortSeq, MoID, MoDId);
                }


                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                //}
                return true;
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21_ReLoadBOMByManual()");
                frmmsg.ShowDialog(this);
                return false;
            }
        }

        /// <summary>
        /// 排产管理--投产
        /// </summary>
        /// <param name="ActType">true=投产;False=取消投产</param>
        /// <param name="MoDId">生产订单表体ID</param>
        /// <param name="ComponentInvCode">子件物料编码</param>
        /// <returns></returns>
        private bool APS21_IntoOperation(bool ActType, string MoCode, string SortSeq, string MoID, string MoDId)
        {
            try
            {
                bool DoIt = false;

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                string selectSQL = "";

                OleDbCommand myCommand = new OleDbCommand("", myConn);
                OleDbDataReader myReader = null;



                if (ActType)//投产                
                {

                    //如果是已投产，则跳过
                    selectSQL = " select 1 as 'a' from  mom_orderdetail (nolock)  where  Status<>3 and moid=" + MoID + " and modid=" + MoDId + "\r\n";
                    myCommand.CommandText = selectSQL;
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        DoIt = true;
                    }
                    myReader.Close();
                    if (DoIt)
                    {
                        DoIt = false;
                        if (iLoginEx.ReadSystemSetValue("APS_ReLoadBOMbyIntoOporation") == "1")//投产时，强制重载BOM
                        {
                            DoIt = true;
                        }
                        else
                        {
                            //没有子件，则生成子件
                            selectSQL = " select count(*) as 'rows' from mom_moallocate where modid=" + MoDId + "\r\n";
                            myCommand.CommandText = selectSQL;
                            myReader = myCommand.ExecuteReader();
                            if (myReader.Read())
                            {
                                if (Convert.ToInt32(myReader["rows"]) <= 0)
                                {
                                    DoIt = true;
                                }
                            }
                            myReader.Close();

                            if (DoIt)
                            {
                                //如果是调库存，则投产时不需要重载BOM
                                selectSQL = " select  ReProduce from zhrs_t_mom_orderdetail_userDefine (nolock)  where  ReProduce=1 and moid=" + MoID + " and modid=" + MoDId + "\r\n";
                                myCommand.CommandText = selectSQL;
                                myReader = myCommand.ExecuteReader();
                                if (myReader.Read())
                                {
                                    DoIt = false;
                                }
                                myReader.Close();
                            }
                        }

                        if (DoIt)
                        {
                            //制造单没有子件则生成子件
                            if (APS21_ReLoadBOM(iLoginEx.ReadSystemSetValue("APS_ReLoadBOMbyIntoOporation") == "1" ? true : false, MoCode, SortSeq, MoID, MoDId))//强制重载BOM时，需要考虑已发料部分
                            {
                                selectSQL = "update mom_orderdetail set OrgStatus=Status,Status=3,RelsDate='" + iLoginEx.GetDBServeCurrentDate() + "', RelsTime='" + iLoginEx.GetDBServeCurrentDateTime() + "' ,RelsUser='" + iLoginEx.UserId() + "', iverifystate=0   where moid=" + MoID + "  and modid=" + MoDId + "\r\n";
                                myCommand.CommandText = selectSQL;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            selectSQL = "update mom_orderdetail set OrgStatus=Status,Status=3,RelsDate='" + iLoginEx.GetDBServeCurrentDate() + "', RelsTime='" + iLoginEx.GetDBServeCurrentDateTime() + "' ,RelsUser='" + iLoginEx.UserId() + "', iverifystate=0   where moid=" + MoID + "  and modid=" + MoDId + "\r\n";
                            myCommand.CommandText = selectSQL;
                            myCommand.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    //如果是已投产，则取消投产
                    selectSQL = " select 1 as 'a' from mom_orderdetail (nolock)  where  Status=3 and moid=" + MoID + " and modid=" + MoDId + "\r\n";
                    myCommand.CommandText = selectSQL;
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        DoIt = true;
                    }
                    myReader.Close();
                    if (DoIt)
                    {
                        selectSQL = "update mom_orderdetail set OrgStatus=Status,Status=0,RelsDate=null, RelsTime=null ,RelsUser=null, iverifystate=0   where moid=" + MoID + "  and modid=" + MoDId + "\r\n";
                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();
                    }
                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21_IntoOperation()");
                frmmsg.ShowDialog(this);
                return false;
            }
        }

        /// <summary>
        /// 排产管理--调库存
        /// </summary>
        /// <param name="ActType">true=调库存;False=取消调库存</param>
        /// <param name="MoDId">生产订单表体ID</param>
        /// <param name="ComponentInvCode">子件物料编码</param>
        /// <returns></returns>
        private bool APS21_ReProduce(bool ActType, string MoCode, string SortSeq, string MoID, string MoDId, string ComponentInvCode)
        {
            try
            {
                bool DoIt = false;

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                string selectSQL = "";

                OleDbCommand myCommand = new OleDbCommand("", myConn);
                myCommand.CommandTimeout = 3600;
                OleDbDataReader myReader = null;



                if (ActType)//调库存
                {
                    selectSQL = " select  ReProduce from zhrs_t_mom_orderdetail_userDefine (nolock)  where  ReProduce=0 and moid=" + MoID + " and modid=" + MoDId + "\r\n";
                    myCommand.CommandText = selectSQL;
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        DoIt = true;
                    }
                    myReader.Close();

                    if (DoIt)
                    {
                        selectSQL = "  \r\n";
                        selectSQL += " select 1 as row from mom_orderdetail a(nolock) where (isnull(a.QualifiedInQty,0)>0   \r\n";
                        selectSQL += "   or exists (select 1 from mom_moallocate b(nolock) where isnull(b.IssQty,0)>0 and a.MoDId=b.MoDId)  \r\n";
                        selectSQL += " )   and exists(select 1 from zhrs_t_mom_orderdetail_userDefine u(nolock) where u.ReProduce=0 and a.MoDId=u.MoDId  )\r\n";
                        selectSQL += "  and a.modid=" + MoDId + "  \r\n";
                        myCommand.CommandText = selectSQL;
                        myReader = myCommand.ExecuteReader();
                        if (myReader.Read())
                        {

                            MessageBox.Show(this, "制造单号：" + MoCode + " ;行号：" + SortSeq + "已生产了（或领料）一部分，不能设为“调库存”！", "排产管理", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            myReader.Close();
                            myReader.Dispose();
                            if (myConn.State == System.Data.ConnectionState.Open)
                            {
                                myConn.Close();
                            }
                            return false;
                        }
                        myReader.Close();
                        myReader.Dispose();

                        selectSQL = "delete mom_moallocate where modid=" + MoDId + "  \r\n";
                        selectSQL += " declare @p5 int    \r\n";
                        selectSQL += " set @p5=0    \r\n";
                        selectSQL += " declare @maxAllocateID int    \r\n";
                        selectSQL += " set @maxAllocateID=0    \r\n";
                        selectSQL += " declare @partid int,@maxSortSeq int   \r\n";
                        selectSQL += " set @maxSortSeq=10  \r\n";
                        selectSQL += "   \r\n";
                        selectSQL += " select @partid=partid from bas_part (nolock) where invcode='" + ComponentInvCode + "'  \r\n";
                        selectSQL += " select @maxSortSeq=isnull(max(SortSeq),10)+10  from mom_moallocate (nolock) where modid =" + MoDId + " \r\n"; ;
                        selectSQL += "   \r\n";
                        selectSQL += "  exec sp_GetID @RemoteId=N'00',@cAcc_Id=N'" + iLoginEx.AccID() + "',@cVouchType=N'mom_moallocate',@iAmount=1,@iFatherId=@p5 output,@iChildId=@maxAllocateID output    \r\n";
                        selectSQL += "  select @p5 as 'mom_moallocate_iFatherId', @maxAllocateID as 'maxAllocateID'    \r\n";
                        selectSQL += "   \r\n";
                        selectSQL += "   \r\n";
                        selectSQL += "   \r\n";
                        selectSQL += " insert into mom_moallocate(AllocateId,MoDid,SortSeq,OpSeq,soSeq,ComponentId,FVFlag,BaseQtyN,BaseQtyD,CompScrap,Qty,IssQty,DeclaredQty,  \r\n";
                        selectSQL += " StartDemDate,EndDemDate,WhCode,WIPType,ByproductFlag,QcFlag,Offset,InvCode,OpComponentId,ReplenishQty,TransQty,ProductType,SoType,QmFlag,OrgQty,OrgAuxQty,Remark,Define29)  \r\n";
                        selectSQL += " select AllocateId=@maxAllocateID,m.MoDid,SortSeq=@maxSortSeq,'0000',soSeq=null,ComponentId=@partid,FVFlag=1,BaseQtyN=1,BaseQtyD=1,CompScrap=0,Qty=(qty-QualifiedInQty)*1,IssQty=0,DeclaredQty=0,  \r\n";
                        selectSQL += " StartDemDate=mm.StartDate,EndDemDate=mm.DueDate,WhCode=null,WIPType=3,ByproductFlag=0,QcFlag=0,Offset=0,  \r\n";
                        selectSQL += " InvCode='" + ComponentInvCode + "',OpComponentId=0,ReplenishQty=0,TransQty=0,ProductType=1,m.SoType,QmFlag=0,OrgQty=0,OrgAuxQty=0,Remark='调库存',Define29='调库存' from  mom_orderdetail m left join mom_morder mm on m.moid=mm.moid and m.modid=m.modid where m.moid=" + MoID + " and m.modid=" + MoDId + "  \r\n";
                        selectSQL += "   \r\n";
                        selectSQL += " update zhrs_t_mom_orderdetail_userDefine set ReProduce=1 where  moid=" + MoID + " and modid=" + MoDId + "\r\n";
                        myCommand.CommandText = selectSQL;
                        myCommand.ExecuteNonQuery();
                    }

                }
                else
                {
                    selectSQL = " select  ReProduce from zhrs_t_mom_orderdetail_userDefine (nolock)  where  ReProduce=1 and moid=" + MoID + " and modid=" + MoDId + "\r\n";
                    myCommand.CommandText = selectSQL;
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        DoIt = true;
                    }
                    myReader.Close();

                    if (DoIt)
                    {

                        selectSQL = "  \r\n";
                        selectSQL += " select 1 as row from mom_orderdetail a(nolock) where (isnull(a.QualifiedInQty,0)>0   \r\n";
                        selectSQL += "   or exists (select 1 from mom_moallocate b(nolock) where isnull(b.IssQty,0)>0 and a.MoDId=b.MoDId)  \r\n";
                        selectSQL += " )   and exists(select 1 from zhrs_t_mom_orderdetail_userDefine u(nolock) where u.ReProduce=1 and a.MoDId=u.MoDId  )\r\n";
                        selectSQL += "  and a.modid=" + MoDId + "  \r\n";
                        myCommand.CommandText = selectSQL;
                        myReader = myCommand.ExecuteReader();
                        if (myReader.Read())
                        {

                            MessageBox.Show(this, "制造单号：" + MoCode + " ;行号：" + SortSeq + "已生产了（或领料）一部分，不能取消“调库存”！", "排产管理", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            myReader.Close();
                            myReader.Dispose();
                            if (myConn.State == System.Data.ConnectionState.Open)
                            {
                                myConn.Close();
                            }
                            return false;
                        }
                        myReader.Close();
                        myReader.Dispose();

                        if (APS21_ReLoadBOM(false, MoCode, SortSeq, MoID, MoDId))
                        {
                            selectSQL = " update zhrs_t_mom_orderdetail_userDefine set ReProduce=0 where  moid=" + MoID + " and modid=" + MoDId + "\r\n";
                            myCommand.CommandText = selectSQL;
                            myCommand.ExecuteNonQuery();
                        }
                    }
                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21_ReProduce()");
                frmmsg.ShowDialog(this);
                return false;
            }
        }




        /// <summary>
        /// 重载BOM
        /// </summary>
        /// <param name="isBatch">true=自动重载BOM；false=手动重载BOM（即，点菜单项【重载BOM】）</param>
        /// <param name="CopyComponent"></param>
        /// <param name="MoID"></param>
        /// <param name="MoDId"></param>
        /// <returns></returns>
        private bool APS21_ReLoadBOM(bool CopyComponent, string MoCode, string SortSeq, string MoID, string MoDId)
        {
            try
            {

                int mom_moallocateiAmount = 0;
                string selectSQL = "";
                int maxAllocateID = 0;
                int BomId = 0;
                int RoutingId = 0;
                int mom_moallocate_iFatherId = 0;

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();



                OleDbCommand myCommand = new OleDbCommand("", myConn);
                myCommand.CommandTimeout = 3600;

                myCommand.CommandText = APS21_DropTempTable;
                myCommand.ExecuteNonQuery();
                myCommand.CommandText = APS21_CreateTempTable;
                myCommand.ExecuteNonQuery();


                if (CopyComponent) //重载BOM前，先将原子件复制
                {
                    selectSQL = " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#mom_moallocate" + iLoginEx.GetMacAddress().Replace(":", "_") + "') and type='U')  \r\n";
                    selectSQL += " drop table #mom_moallocate" + iLoginEx.GetMacAddress().Replace(":", "_") + ";  \r\n";

                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();

                    selectSQL = "  \r\n";
                    selectSQL += " CREATE TABLE #mom_moallocate" + iLoginEx.GetMacAddress().Replace(":", "_") + "(  \r\n";
                    selectSQL += " 	[AllocateId] [int] NOT NULL,  \r\n";
                    selectSQL += " 	[MoDId] [int] NOT NULL,  \r\n";
                    selectSQL += " 	[SortSeq] [int] NOT NULL  DEFAULT (0),  \r\n";
                    selectSQL += " 	[OpSeq] [nchar](4) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[ComponentId] [int] NULL,  \r\n";
                    selectSQL += " 	[FVFlag] [tinyint] NULL   DEFAULT (1),  \r\n";
                    selectSQL += " 	[BaseQtyN] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[BaseQtyD] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[ParentScrap] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[CompScrap] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[Qty] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[IssQty] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[DeclaredQty] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[StartDemDate] [datetime] NULL,  \r\n";
                    selectSQL += " 	[EndDemDate] [datetime] NULL,  \r\n";
                    selectSQL += " 	[WhCode] [nvarchar](10) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[LotNo] [nvarchar](60) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[WIPType] [tinyint] NULL   DEFAULT (3),  \r\n";
                    selectSQL += " 	[ByproductFlag] [bit] NULL,  \r\n";
                    selectSQL += " 	[QcFlag] [bit] NULL,  \r\n";
                    selectSQL += " 	[Offset] [smallint] NULL  DEFAULT (0),  \r\n";
                    selectSQL += " 	[InvCode] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free1] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free2] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free3] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free4] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free5] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free6] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free7] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free8] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free9] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Free10] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[OpComponentId] [int] NULL  DEFAULT (0),  \r\n";
                    selectSQL += " 	[Define22] [nvarchar](60) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define23] [nvarchar](60) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define24] [nvarchar](60) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define25] [nvarchar](60) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define26] [float] NULL,  \r\n";
                    selectSQL += " 	[Define27] [float] NULL,  \r\n";
                    selectSQL += " 	[Define28] [nvarchar](120) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define29] [nvarchar](120) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define30] [nvarchar](120) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define31] [nvarchar](120) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define32] [nvarchar](120) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define33] [nvarchar](120) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[Define34] [int] NULL,  \r\n";
                    selectSQL += " 	[Define35] [int] NULL,  \r\n";
                    selectSQL += " 	[Define36] [datetime] NULL,  \r\n";
                    selectSQL += " 	[Define37] [datetime] NULL,	  \r\n";
                    selectSQL += " 	[AuxUnitCode] [nvarchar](35) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[ChangeRate] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[AuxBaseQtyN] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[AuxQty] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[ReplenishQty] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[Remark] [nvarchar](255) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[TransQty] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[ProductType] [tinyint] NULL DEFAULT (1),  \r\n";
                    selectSQL += " 	[SoType] [tinyint] NULL DEFAULT (0),  \r\n";
                    selectSQL += " 	[SoDId] [nvarchar](30) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[SoCode] [nvarchar](30) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[SoSeq] [int] NULL DEFAULT (0),  \r\n";
                    selectSQL += " 	[DemandCode] [nvarchar](30) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[QmFlag] [bit] NULL DEFAULT (0),  \r\n";
                    selectSQL += " 	[OrgQty] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[OrgAuxQty] decimal(28,4) NULL,  \r\n";
                    selectSQL += " 	[CostItemCode] [nvarchar](60) COLLATE Chinese_PRC_CI_AS NULL,  \r\n";
                    selectSQL += " 	[CostItemName] [nvarchar](60) COLLATE Chinese_PRC_CI_AS NULL  \r\n";
                    selectSQL += " )   \r\n";

                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();



                    //仅复那些已发料或已调拨的子件
                    selectSQL = "insert into #mom_moallocate" + iLoginEx.GetMacAddress().Replace(":", "_") + "(AllocateId," + mom_moallocateColumns + ") \r\n";
                    selectSQL += "select AllocateId," + mom_moallocateColumns + " from mom_moallocate (nolock) where isnull(IssQty,0)>0 and  MoDId=" + MoDId + " \r\n";

                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();
                }

                //***************************************生成制造单子件***    begin      ******************************************************************


                myCommand.CommandText = " select BomId=isnull(BomId,0),RoutingId=isnull(RoutingId,0) from mom_orderdetail (nolock) where MoId=" + MoID + " and MoDId=" + MoDId;
                OleDbDataReader myReader = myCommand.ExecuteReader();
                if (myReader.Read())
                {
                    BomId = Convert.ToInt32(myReader["BomId"]);
                    RoutingId = Convert.ToInt32(myReader["RoutingId"]);
                }
                else
                {
                    myReader.Close();
                    return false;
                }
                myReader.Close();



                selectSQL = "exec sp_executesql N'insert into #tmp_procmo(MoDId,BomId,RoutingId,CopyMoDId,CopyBomId,CopyRoutingId,STProxyWhFlag) select  @MoDId,@BomId,@RoutingId,@CopyMoDId,@CopyBomId,@CopyRoutingId,@STProxyWhFlag',N'@MoDId int,@BomId int,@RoutingId int,@CopyMoDId int,@CopyBomId int,@CopyRoutingId int,@STProxyWhFlag bit'  \r\n";
                selectSQL += " ,@MoDId=" + MoDId + ",@BomId=" + BomId.ToString() + ",@RoutingId=" + (RoutingId == 0 ? "null" : RoutingId.ToString()) + ",@CopyMoDId=0,@CopyBomId=0,@CopyRoutingId=0,@STProxyWhFlag=0  \r\n";
                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();



                SLbState.Text = "正在生成 " + MoCode + "(" + SortSeq + ")制造单子件...";
                System.Windows.Forms.Application.DoEvents();

                selectSQL = "exec Usp_MO_GenAllocate   \r\n";//子件
                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();




                SLbState.Text = "正在导入 " + MoCode + "(" + SortSeq + ")制造单子件...";
                System.Windows.Forms.Application.DoEvents();


                selectSQL = "select ParentCnt,ComponentCnt from #tmp_recordcount    \r\n";
                myCommand.CommandText = selectSQL;

                myReader = myCommand.ExecuteReader();
                if (myReader.Read())
                {
                    mom_moallocateiAmount = Convert.ToInt32(myReader["ComponentCnt"]);
                }
                myReader.Close();


                selectSQL = "delete mom_moallocate where modid=" + MoDId + "  \r\n";
                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();

                selectSQL = "declare @p5 int  \r\n";
                selectSQL += " set @p5=0  \r\n";
                selectSQL += " declare @p6 int  \r\n";
                selectSQL += " set @p6=0  \r\n";
                selectSQL += " exec sp_GetID @RemoteId=N'00',@cAcc_Id=N'" + iLoginEx.AccID() + "',@cVouchType=N'mom_moallocate',@iAmount=" + mom_moallocateiAmount.ToString() + ",@iFatherId=@p5 output,@iChildId=@p6 output  \r\n";
                selectSQL += " select @p5 as 'mom_moallocate_iFatherId', @p6 as 'maxAllocateID'  \r\n";


                myCommand.CommandText = selectSQL;
                myReader = myCommand.ExecuteReader();
                if (myReader.Read())
                {
                    mom_moallocate_iFatherId = Convert.ToInt32(myReader["mom_moallocate_iFatherId"]);
                    maxAllocateID = Convert.ToInt32(myReader["maxAllocateID"]);
                    myReader.Close();
                }
                else
                {
                    myReader.Close();
                    MessageBox.Show(this, "取最大ID号失败！", "制造单导入", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                //selectSQL = "exec Usp_MO_InsCollectiveMo @v_moid=" + MoId + ",@v_maxmodid=" + MoDId + ",@v_maxallocateid=" + maxAllocateID.ToString() + ",@v_maxsortseq=" + wRow.ToString() + ",@v_pcount=0,@v_ccount=" + mom_moallocateiAmount.ToString() + ",@v_updallocateflag=0  \r\n";
                //selectSQL += "   \r\n";
                //myCommand.CommandText = selectSQL;//子件
                //myCommand.ExecuteNonQuery();

                selectSQL = "exec Usp_MO_InsAllocate @v_detailcount=" + maxAllocateID.ToString() + ",@v_pcount=0,@v_maxmodid=" + MoDId + "  \r\n";
                myCommand.CommandText = selectSQL;//子件
                myCommand.ExecuteNonQuery();


                SLbState.Text = "";
                System.Windows.Forms.Application.DoEvents();

                //***************************************生成制造单子件***    end      ******************************************************************

                myCommand.CommandText = APS21_DropTempTable;
                myCommand.ExecuteNonQuery();


                if (CopyComponent)
                {
                    selectSQL = "update  mom_moallocate  set AllocateId=m.AllocateId,IssQty=m.IssQty,TransQty=m.TransQty,Define29=m.Define29, Remark=m.Remark  \r\n";
                    selectSQL += "  from (select MoDId,ComponentId,AllocateId,IssQty,TransQty,Define29,Remark from #mom_moallocate" + iLoginEx.GetMacAddress().Replace(":", "_") + " (nolock) )m where m.ComponentId=mom_moallocate.ComponentId and m.MoDId=mom_moallocate.MoDId and mom_moallocate.MoDId=" + MoDId + " \r\n";
                    selectSQL += "  \r\n";
                    selectSQL += "insert into mom_moallocate(AllocateId," + mom_moallocateColumns + ") \r\n";
                    selectSQL += " select AllocateId," + mom_moallocateColumns + " from #mom_moallocate" + iLoginEx.GetMacAddress().Replace(":", "_") + " a (nolock) where  a.MoDId=" + MoDId + "  \r\n";
                    selectSQL += " and not exists (select 1 from mom_moallocate b (nolock) where a.MoDId=b.MoDId and a.ComponentId=b.ComponentId) \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();

                    selectSQL = " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'tempdb..#mom_moallocate" + iLoginEx.GetMacAddress().Replace(":", "_") + "') and type='U')  \r\n";
                    selectSQL += " drop table #mom_moallocate" + iLoginEx.GetMacAddress().Replace(":", "_") + ";  \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();
                }

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21_ReLoadBOM()");
                frmmsg.ShowDialog(this);
                return false;
            }

        }


        #region 排产管理--保存
        /// <summary>
        /// 排产管理--保存
        /// </summary>
        private void APS21Save()
        {
            try
            {
                if (tab6_dataGridView1.Columns.Count > 0)
                {
                    tab6_dataGridView1.Columns["排产数量"].ReadOnly = true;
                }

                if (!APS21Save_TempTable())
                {
                    return;
                }

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();
                string RowColors = "";
                string selectSQL = "";

                if (DoOrderSplit)
                {
                    APS21_OrderSplit();//拆单 
                    APS21Query(true, SQLSelect_Temp);
                }


                OleDbCommand myCommand = new OleDbCommand("", myConn);

                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["调库存"].Value))
                    {
                        APS21_ReProduce(true, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["产品编码"].Value));
                    }
                    else
                    {
                        APS21_ReProduce(false, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["产品编码"].Value));
                    }


                    if (Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["投产"].Value))
                    {
                        APS21_IntoOperation(true, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value));
                    }
                    else
                    {
                        APS21_IntoOperation(false, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value));
                    }

                    if (Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["结案"].Value))
                    {
                        APS21_Closed(true, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value));
                    }
                    else
                    {
                        APS21_Closed(false, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value));
                    }


                    RowColors = "";
                    for (int c = 0; c < tab6_dataGridView1.Columns.Count; c++)
                    {
                        if (ColorTranslator.ToHtml(tab6_dataGridView1.Rows[i].Cells[c].Style.BackColor) == "White")
                        {
                            RowColors += ";";
                        }
                        else
                        {
                            RowColors += ColorTranslator.ToHtml(tab6_dataGridView1.Rows[i].Cells[c].Style.BackColor) + ";";
                        }
                    }
                    RowColors += iLoginEx.Chr(8);
                    RowColors = RowColors.Replace(";" + iLoginEx.Chr(8), "");
                    selectSQL = " update zhrs_t_mom_orderdetail_userDefine set Priority=" + (Convert.ToString(tab6_dataGridView1.Rows[i].Cells["优先级"].Value).Length == 0 ? "0" : Convert.ToString(tab6_dataGridView1.Rows[i].Cells["优先级"].Value)) + ",ViewSort=" + i.ToString() + ",RowColor='" + RowColors + "' where MoId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value);
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();

                    ////未结案和未完工的制造单，才可以修改计划生产日期
                    selectSQL = "update  mom_morder set StartDate='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["计划生产日期"].Value) + "',DueDate='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["计划生产日期"].Value) + "' where \r\n";
                    selectSQL += " exists (select 1 from mom_orderdetail a(nolock) where a.MoId=mom_morder.MoId and a.MoDId=mom_morder.MoDId and  len(isnull(a.CloseUser,''))<=0 and (isnull(a.qty,0)-isnull(a.QualifiedInQty,0))>0) and mom_morder.moid=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " \r\n";
                    selectSQL += " and mom_morder.modid=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value) + " \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();


                    selectSQL = " update mom_orderdetail set Qty=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["排产数量"].Value) + ",MoClass=" + (Convert.ToString(tab6_dataGridView1.Rows[i].Cells["类别"].Value) == "非标" ? "2" : "1") + ", \r\n";
                    selectSQL += " MDeptCode=(select cDepCode from Department (nolock) where cDepName='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["部门"].Value) + "'),Define29='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["备注"].Value) + "',remark='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["备注"].Value) + "' \r\n";
                    selectSQL += " where  len(isnull(CloseUser,''))<=0 and (isnull(qty,0)-isnull(QualifiedInQty,0))>0 and moid=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " \r\n";
                    selectSQL += " and modid=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value) + " \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();

                }




                for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
                {
                    tab6_dataGridView1.Columns[i].ReadOnly = true;
                }


                myCommand.CommandText = APS21_DropTempTable;
                myCommand.ExecuteNonQuery();


                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }

                MessageBox.Show(this, "保存完成！", "排产管理", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21Save()");
                frmmsg.ShowDialog(this);
            }

        }

        private void SaveAPS21_1()
        {
            try
            {

                if (!APS21Save_TempTable())
                {
                    return;
                }

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();
                string RowColors = "";
                string selectSQL = "";

                if (DoOrderSplit)
                {
                    APS21_OrderSplit();
                    APS21Query(true, SQLSelect_Temp);
                }


                OleDbCommand myCommand = new OleDbCommand("", myConn);

                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["调库存"].Value))
                    {
                        APS21_ReProduce(true, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["产品编码"].Value));
                    }
                    else
                    {
                        APS21_ReProduce(false, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["产品编码"].Value));
                    }


                    if (Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["投产"].Value))
                    {
                        APS21_IntoOperation(true, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value));
                    }
                    else
                    {
                        APS21_IntoOperation(false, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["行号M"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value));
                    }

                    if (Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["结案"].Value))
                    {
                        APS21_Closed(true, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value));
                    }
                    else
                    {
                        APS21_Closed(false, Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value));
                    }


                    RowColors = "";
                    for (int c = 0; c < tab6_dataGridView1.Columns.Count; c++)
                    {
                        if (ColorTranslator.ToHtml(tab6_dataGridView1.Rows[i].Cells[c].Style.BackColor) == "White")
                        {
                            RowColors += ";";
                        }
                        else
                        {
                            RowColors += ColorTranslator.ToHtml(tab6_dataGridView1.Rows[i].Cells[c].Style.BackColor) + ";";
                        }
                    }
                    RowColors += iLoginEx.Chr(8);
                    RowColors = RowColors.Replace(";" + iLoginEx.Chr(8), "");
                    selectSQL = " update zhrs_t_mom_orderdetail_userDefine set Priority=" + (Convert.ToString(tab6_dataGridView1.Rows[i].Cells["优先级"].Value).Length == 0 ? "0" : Convert.ToString(tab6_dataGridView1.Rows[i].Cells["优先级"].Value)) + ",ViewSort=" + i.ToString() + ",RowColor='" + RowColors + "' where MoId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value);
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();


                    selectSQL = "update  mom_morder set StartDate='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["计划生产日期"].Value) + "',DueDate='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["计划生产日期"].Value) + "' where \r\n";
                    selectSQL += " exists (select 1 from mom_orderdetail a(nolock) where a.MoId=mom_morder.MoId and a.MoDId=mom_morder.MoDId and  len(isnull(a.CloseUser,''))<=0 and (isnull(a.qty,0)-isnull(a.QualifiedInQty,0))>0) and mom_morder.moid=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " \r\n";
                    selectSQL += " and mom_morder.modid=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value) + " \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();

                    selectSQL = " update mom_orderdetail set MDeptCode=(select cDepCode from Department (nolock) where cDepName='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["部门"].Value) + "'),Define29='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["备注"].Value) + "',remark='" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["备注"].Value) + "' \r\n";
                    selectSQL += " where  len(isnull(CloseUser,''))<=0 and (isnull(qty,0)-isnull(QualifiedInQty,0))>0 and moid=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " \r\n";
                    selectSQL += " and modid=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value) + " \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();

                }




                for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
                {
                    tab6_dataGridView1.Columns[i].ReadOnly = true;
                }


                myCommand.CommandText = APS21_DropTempTable;
                myCommand.ExecuteNonQuery();


                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "APS21Save()");
                frmmsg.ShowDialog(this);
            }

        }

        #endregion

        /// <summary>
        /// 制告单依据
        /// </summary>
        private void mom_orderBasis()
        {
            try
            {
                string cinvCodeChild = "";
                string[] paraCinvCode = null;
                this.Text = "排产管理   正在查询，请稍候...";
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

                this.Text = "排产管理   查询完成！共" + (tab4_dataGridView1.RowCount).ToString() + "行";
                System.Windows.Forms.Application.DoEvents();
            }

            catch (Exception ex)
            {
                this.Text = "排产管理";

                frmMessege frmmsg = new frmMessege(ex.ToString(), "mom_orderBasis()");
                frmmsg.ShowDialog(this);
            }
        }



        private void toolToExcel_Click(object sender, EventArgs e)
        {
            toolToExcel.Enabled = false;

            try
            {
                this.Text = "排产管理   正在导出Excel，请稍候...";
                switch (this.tabControl1.SelectedIndex)
                {
                    case 2:
                        {
                            iLoginEx.ExportExcel("排产管理_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "排产管理", tab6_dataGridView1, 11);
                            break;
                        }
                    case 3:
                        {
                            iLoginEx.ExportExcel("需求供应与进度_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "需求供应与进度", tab8_dataGridView1, 6);
                            break;
                        }
                    case 4:
                        {
                            iLoginEx.ExportExcel("库存综合查询_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "库存综合查询", tab2_dataGridView1, 4);
                            break;
                        }
                    case 5:
                        {
                            iLoginEx.ExportExcel("库存综合查询明细_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "库存综合查询明细", tab1_dataGridView1, 8);
                            break;
                        }
                    case 6:
                        {
                            iLoginEx.ExportExcel("请购来源_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "请购来源", tab3_dataGridView1, 7);
                            break;
                        }
                    case 7:
                        {
                            iLoginEx.ExportExcel("制造单来源_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "制造单来源", tab4_dataGridView1, 9);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "toolToExcel_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                this.Text = "排产管理";
                toolToExcel.Enabled = true;
            }
        }

        private void toolClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRSERP_APS21_Load(object sender, EventArgs e)
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


                tab4_dataGridView1Width = tab4_dataGridView1.Width;
                tab4_dataGridView1Height = tab4_dataGridView1.Height;
                tab6_dataGridView1Width = tab6_dataGridView1.Width;
                tab6_dataGridView1Height = tab6_dataGridView1.Height;
                tab8_dataGridView1Width = tab8_dataGridView1.Width;
                tab8_dataGridView1Height = tab8_dataGridView1.Height;

                tab2_DateL.Value = DateTime.Now;
                tab2_DateH.Value = DateTime.Now;
                tab2_DateL.Enabled = false;
                tab2_DateH.Enabled = false;
                tab3_dDateL.Enabled = false;
                tab3_dDateH.Enabled = false;
                toolSave.Enabled = false;

                tabControl1.SelectedIndex = 2;

                DateTime dt = DateTime.Now;
                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末//
                tab3_dDateL.Value = startMonth;
                tab3_dDateH.Value = endMonth;

                ini = new Ini(System.Windows.Forms.Application.StartupPath.ToString() + "\\rserpconfig.ini");
                if (ini.ReadValue("Production", "ShowProd").Trim().Length == 0)
                {
                    ini.Writue("Production", "ShowProd", "Y");
                }
                if (ini.ReadValue("Production", "ShowProd") == "Y")
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

                toolEdit.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Edit);
                toolSave.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Save);
                toolToExcel.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Export);
                toolQuery.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Query);
                toolRefresh.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Query);
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "frmRSERP_APS21_Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void frmRSERP_APS21_Resize(object sender, EventArgs e)
        {
            Ini ini = new Ini(System.Windows.Forms.Application.StartupPath.ToString() + "\\utconfig.ini");
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

                tab6_dataGridView1.Width = tab6_dataGridView1Width + (this.Width - FormWidth);
                tab6_dataGridView1.Height = tab6_dataGridView1Height + (this.Height - FormHeight);


                tab8_dataGridView1.Width = tab8_dataGridView1Width + (this.Width - FormWidth);
                tab8_dataGridView1.Height = tab8_dataGridView1Height + (this.Height - FormHeight);
            }

        }

        private void frmRSERP_APS21_Shown(object sender, EventArgs e)
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
                ini.Writue("Production", "ShowProd", "Y");
            }
            else
            {
                if (tab1_dataGridView1.Columns.Count > 0)
                {
                    tab1_dataGridView1.Columns[5].Visible = false;
                    tab1_dataGridView1.Columns[6].Visible = false;
                    tab1_dataGridView1.Columns[7].Visible = false;
                }
                ini.Writue("Production", "ShowProd", "N");
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

        private void tab2_cInvCodeL_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            tab2_cInvCodeL.Text = iLoginEx.OpenSelectWindow("物料", "select cInvCode as '料号',cInvName  as '品名',cInvStd  as '规格' from  inventory  (nolock)", tab2_cInvCodeL.Text, 430, 300, 1);
            string[] para = tab2_cInvCodeL.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab2_cInvCodeL.Text = para[0];
                //wName = para[0];  
            }
        }

        private void tab2_cInvCodeH_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tab2_cInvCodeH.Text = iLoginEx.OpenSelectWindow("物料", "select cInvCode as '料号',cInvName  as '品名',cInvStd  as '规格' from  inventory  (nolock)", tab2_cInvCodeH.Text, 430, 300, 1);
            string[] para = tab2_cInvCodeH.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                tab2_cInvCodeH.Text = para[0];
                //wName = para[0];  
            }
        }

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
            tab2_cInvCodeL.Text = tab1_dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            tab2_cInvCodeH.Text = tab2_cInvCodeL.Text;
            CompreStok();
            tabControl1.SelectedIndex = 0;
        }

        private void cmtab6SelectAll_Click(object sender, EventArgs e)
        {
            if (cmtab6SelectAll.Text == "全选")
            {
                cmtab6SelectAll.Text = "取消全选";
                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    tab6_dataGridView1.Rows[i].Selected = true;
                }
            }
            else
            {
                cmtab6SelectAll.Text = "全选";
                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    tab6_dataGridView1.Rows[i].Selected = false;
                }
            }
        }





        private void toolEdit_Click(object sender, EventArgs e)
        {
            SLBTotal.Text = "编辑";
            toolSave.Enabled = true;
            switch (this.tabControl1.SelectedIndex)
            {
                case 2://排产管理
                    {

                        cmtab6Paste.Enabled = true;
                        cmtab6RowCut.Enabled = true;
                        cmtab6RowPaste.Enabled = true;
                        cmtab6Priority.Enabled = true;
                        cmtab6Sure2.Enabled = true;
                        cmtab6NotSure.Enabled = true;
                        cmtab6Close.Enabled = true;
                        cmtab6NotColse.Enabled = true;
                        cmtab6ChangeDate.Enabled = true;
                        cmtab6OrderSplit.Enabled = true;
                        cmtab6OrderMerge.Enabled = true;
                        cmtab6Colors.Enabled = true;
                        tab6_StandardCut.Enabled = true;
                        if (tab6_dataGridView1.Columns.Count > 0)
                        {
                            tab6_dataGridView1.Columns["排产数量"].ReadOnly = false;
                        }

                        string EditColumnsNames = "部门,计划生产日期,投产,调库存,结案,备注,优先级";
                        for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
                        {
                            if (EditColumnsNames.IndexOf(tab6_dataGridView1.Columns[i].DataPropertyName) > -1)
                            {
                                tab6_dataGridView1.Columns[i].ReadOnly = false;
                            }
                        }
                        break;
                    }
            }
        }

        private void cmtab6Copy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.Clear();
                string CopyText = "", CulomnsText = "";
                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    CulomnsText = "";
                    for (int c = 0; c < tab6_dataGridView1.Columns.Count; c++)
                    {
                        if (tab6_dataGridView1.Rows[i].Cells[c].Selected)
                        {
                            if (tab6_dataGridView1.Columns[c].GetType().Name == "DataGridViewCheckBoxColumn")
                            {
                                CulomnsText += Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells[c].Value) ? "Y" : "N" + "\t";
                            }
                            else
                            {
                                CulomnsText += Convert.ToString(tab6_dataGridView1.Rows[i].Cells[c].Value) + "\t";
                            }
                        }
                    }
                    CulomnsText += iLoginEx.Chr(8);
                    CulomnsText = CulomnsText.Replace("\t" + iLoginEx.Chr(8), "");
                    CulomnsText = CulomnsText.Replace(iLoginEx.Chr(8), "");
                    if (CulomnsText.Length > 0)
                    {
                        CopyText += CulomnsText + "\n";
                    }
                }
                CopyText += iLoginEx.Chr(8);
                CopyText = CopyText.Replace("\n" + iLoginEx.Chr(8), "");
                CopyText = CopyText.Replace(iLoginEx.Chr(8), "");
                Clipboard.SetText(CopyText);
            }
            catch
            {
                // 不处理  
            }
        }

        private void cmtab6Paste_Click(object sender, EventArgs e)
        {
            try
            {
                int r = -1;
                int c = -1;

                int SelectedRows = 0, SelectedColumns = 0, SelectedColumns2 = 0;
                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    SelectedColumns = 0;

                    for (int y = 0; y < tab6_dataGridView1.Columns.Count; y++)
                    {
                        if (tab6_dataGridView1.Rows[i].Cells[y].Selected)
                        {
                            if (r < 0)
                            {
                                r = i;
                            }
                            if (c < 0)
                            {
                                c = y;
                            }

                            SelectedColumns++;
                        }
                    }
                    if (SelectedColumns > 0)
                    {
                        SelectedRows++;
                        SelectedColumns2 = SelectedColumns;
                    }
                }


                int c1 = c < 0 ? 0 : c;
                int r1 = r < 0 ? 0 : r;




                // 获取剪切板的内容，并按行分割  
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText))
                    return;
                string[] lines = pasteText.Split('\n');

                if (SelectedRows + SelectedColumns2 <= 2)
                {
                    r = tab6_dataGridView1.CurrentCell.RowIndex;
                    r = tab6_dataGridView1.CurrentCell.RowIndex;
                    c1 = c;
                    r1 = r;
                    //再按单元格填充行  
                    foreach (string line in lines)
                    {
                        if (r < tab6_dataGridView1.Rows.Count)
                        {
                            c1 = c;
                            if (string.IsNullOrEmpty(line.Trim()))
                                continue;
                            // 按 Tab 分割数据  
                            string[] vals = line.Split('\t');
                            foreach (string val in vals)
                            {
                                if (c1 < tab6_dataGridView1.Columns.Count)
                                {
                                    if (!tab6_dataGridView1.Columns[c1].ReadOnly)
                                    {
                                        tab6_dataGridView1.Rows[r].Cells[c1].Value = val;
                                    }

                                }
                                else
                                {
                                    break;
                                }
                                c1++;
                            }
                        }
                        else
                        {
                            break;
                        }
                        r++;
                    }
                }
                else
                {
                    while (r1 < r + SelectedRows)
                    {
                        //再按单元格填充行  
                        foreach (string line in lines)
                        {
                            if (r1 < r + SelectedRows)
                            {
                                c1 = c;
                                if (string.IsNullOrEmpty(line.Trim()))
                                    continue;
                                // 按 Tab 分割数据  
                                string[] vals = line.Split('\t');
                                while (c1 < c + SelectedColumns2)
                                {
                                    foreach (string val in vals)
                                    {
                                        if (c1 < c + SelectedColumns2)
                                        {
                                            if (!tab6_dataGridView1.Columns[c1].ReadOnly)
                                            {
                                                tab6_dataGridView1.Rows[r1].Cells[c1].Value = val;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                        c1++;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                            r1++;
                        }
                    }

                }
            }
            catch
            {
                // 不处理  
            }
        }

        private void toolRefresh_Click(object sender, EventArgs e)
        {
            if (tab6_dataGridView1.Columns.Count > 0)
            {
                tab6_dataGridView1.Columns["排产数量"].ReadOnly = true;
            }

            switch (this.tabControl1.SelectedIndex)
            {
                case 2://排产管理
                    {
                        APS21Query(true, SQLSelect_Temp);
                        toolSave.Enabled = false;
                        break;
                    }
            }
        }

        private void tab6_dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            string EditColumnsNames = "部门";
            int CrrColumnIndex = -100;
            for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
            {
                if (EditColumnsNames.IndexOf(tab6_dataGridView1.Columns[i].DataPropertyName) > -1)
                {
                    CrrColumnIndex = i;
                    break;
                }
            }
            if (CrrColumnIndex > -100)
            {
                if (e.ColumnIndex == CrrColumnIndex && !tab6_dataGridView1.Columns[CrrColumnIndex].ReadOnly)
                {
                    string wDeptName = iLoginEx.OpenSelectWindow("部门", "select cDepCode as '部门编码',cDepName  as '部门名称' from Department (nolock)", Convert.ToString(tab6_dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value), 430, 300, 2);
                    string[] para = wDeptName.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
                    if (para.Length > 1)
                    {
                        tab6_dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = para[1];

                    }
                }
            }
        }

        private void tab6_dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (Tab6_SaveCulomnsWidth && tab6_dataGridView1.Columns.Count > 0)
            {

                string ColumnsWidths = "";
                for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
                {
                    ColumnsWidths += tab6_dataGridView1.Columns[i].Width.ToString() + ";";
                }
                ColumnsWidths += iLoginEx.Chr(8);
                ColumnsWidths = ColumnsWidths.Replace(";" + iLoginEx.Chr(8), "");

                iLoginEx.WriteUserProfileValue("APS", "ColumnsWidths", ColumnsWidths);
                SLbState.Text = "列宽保存完成";
            }


        }

        private void cmtab6Colors_Click(object sender, EventArgs e)
        {
            if (tab6_dataGridView1.Columns.Count > 0)
            {

                frmColorPanel fColor = new frmColorPanel();
                fColor.ShowDialog(this);


                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    for (int c = 0; c < tab6_dataGridView1.Columns.Count; c++)
                    {
                        if (tab6_dataGridView1.Rows[i].Cells[c].Selected)
                        {
                            tab6_dataGridView1.Rows[i].Cells[c].Style.BackColor = ColorTranslator.FromHtml(fColor.GetColorByHTML());
                        }
                    }
                }
            }
        }

        private void cmtab6_oth_ResetColumnsWidth_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "ColumnsWidths", "");
            APS21Query(true, SQLSelect_Temp);
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            SLBTotal.Text = "";
            switch (this.tabControl1.SelectedIndex)
            {
                case 2://排产管理
                    {

                        cmtab6Paste.Enabled = false;
                        cmtab6RowCut.Enabled = false;
                        cmtab6RowPaste.Enabled = false;
                        cmtab6Priority.Enabled = false;
                        cmtab6Sure2.Enabled = false;
                        cmtab6NotSure.Enabled = false;
                        cmtab6Close.Enabled = false;
                        cmtab6NotColse.Enabled = false;
                        cmtab6ChangeDate.Enabled = false;
                        cmtab6OrderSplit.Enabled = false;
                        cmtab6OrderMerge.Enabled = false;
                        cmtab6Colors.Enabled = false;
                        tab6_StandardCut.Enabled = false;
                        APS21Save();

                        toolSave.Enabled = false;
                        APS21Query(false, SQLSelect);
                        break;
                    }
            }
        }

        private void cmtab6_Font8_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "FontSize", "8");
        }

        private void cmtab6_Font9_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "FontSize", "9");
        }

        private void cmtab6_Font10_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "FontSize", "10");
        }

        private void cmtab6_Font11_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "FontSize", "11");
        }

        private void cmtab6_Font12_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "FontSize", "12");
        }

        private void cmtab6_Font13_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "FontSize", "13");
        }

        private void cmtab6_Font14_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "FontSize", "14");
        }

        private void cmtab6_Font15_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "FontSize", "15");
        }

        private void cmtab6_Font16_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "FontSize", "16");
        }

        private void cmtab6Sure2_Click(object sender, EventArgs e)
        {
            if (tab6_dataGridView1.Columns.Count > 0)
            {

                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    if (tab6_dataGridView1.Rows[i].Selected || Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["投产"].Selected))
                    {
                        tab6_dataGridView1.Rows[i].Cells["投产"].Value = 1;
                    }
                }

            }
        }

        private void cmtab6NotSure_Click(object sender, EventArgs e)
        {
            if (tab6_dataGridView1.Columns.Count > 0)
            {

                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    if (tab6_dataGridView1.Rows[i].Selected || Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["投产"].Selected))
                    {
                        tab6_dataGridView1.Rows[i].Cells["投产"].Value = 0;
                    }
                }

            }
        }

        private void cmtab6Close_Click(object sender, EventArgs e)
        {
            if (tab6_dataGridView1.Columns.Count > 0)
            {

                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    if (tab6_dataGridView1.Rows[i].Selected || Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["结案"].Selected))
                    {
                        tab6_dataGridView1.Rows[i].Cells["结案"].Value = 1;
                    }
                }

            }
        }

        private void cmtab6NotColse_Click(object sender, EventArgs e)
        {
            if (tab6_dataGridView1.Columns.Count > 0)
            {

                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    if (tab6_dataGridView1.Rows[i].Selected || Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["结案"].Selected))
                    {
                        tab6_dataGridView1.Rows[i].Cells["结案"].Value = 0;
                    }
                }
            }
        }

        private void cmtab6ChangeDate_Click(object sender, EventArgs e)
        {
            bool wIsSelect = false;

            if (tab6_dataGridView1.Columns.Count > 0)
            {

                for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                {
                    if (tab6_dataGridView1.Rows[i].Selected || Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["计划生产日期"].Selected))
                    {
                        wIsSelect = true;
                        break;
                    }
                }
                if (wIsSelect)
                {
                    frmDate fDate = new frmDate();
                    fDate.ShowDialog(this);

                    for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                    {
                        if (tab6_dataGridView1.Rows[i].Selected || Convert.ToBoolean(tab6_dataGridView1.Rows[i].Cells["计划生产日期"].Selected))
                        {
                            tab6_dataGridView1.Rows[i].Cells["计划生产日期"].Value = Convert.ToDateTime(fDate.GetDate());
                        }
                    }

                }
            }

        }

        private void cmtab6ColumnFrozen_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
            {
                tab6_dataGridView1.Columns[i].Frozen = false;
            }
            tab6_dataGridView1.Columns[tab6_dataGridView1.CurrentCell.ColumnIndex].Frozen = true;
            iLoginEx.WriteUserProfileValue("APS", "ColumnFrozen", tab6_dataGridView1.CurrentCell.ColumnIndex.ToString());

        }

        private void cmtab6ColumnNotFrozen_Click(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APS", "ColumnFrozen", "");
            for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
            {
                tab6_dataGridView1.Columns[i].Frozen = false;
            }
        }

        private void cmtab6Priority_Click(object sender, EventArgs e)
        {
            try
            {
                if (tab6_dataGridView1.Columns.Count > 0)
                {
                    int ColumnNum = 0;
                    for (int i = 0; i < tab6_dataGridView1.Columns.Count; i++)
                    {
                        if (tab6_dataGridView1.Columns[i].DataPropertyName == "优先级")
                        {
                            ColumnNum = i;
                            break;
                        }
                    }
                    if (!tab6_dataGridView1.Columns[ColumnNum].ReadOnly)
                    {
                        int PriorityNum = Convert.ToInt32(Input.InputBox.ShowInputBox("优先级起始号", "1"));

                        for (int r = 0; r < tab6_dataGridView1.Rows.Count; r++)
                        {
                            if (tab6_dataGridView1.Rows[r].Cells[ColumnNum].Selected)
                            {
                                tab6_dataGridView1.Rows[r].Cells[ColumnNum].Value = PriorityNum;
                                PriorityNum++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "cmtab6Priority_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmtab6RowCut_Click(object sender, EventArgs e)
        {
            try
            {
                APSTab6RowCutList.Clear();

                if (tab6_dataGridView1.Columns.Count > 0)
                {

                    for (int r = 0; r < tab6_dataGridView1.Rows.Count; r++)
                    {
                        if (tab6_dataGridView1.Rows[r].Selected)
                        {
                            APSTab6RowCutList.Add(r);

                        }
                    }
                }




            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "cmtab6RowCut_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmtab6RowPaste_Click(object sender, EventArgs e)
        {
            try
            {
                if (APSTab6RowCutList.Count > 0)
                {
                    int CurrentRowIndex = tab6_dataGridView1.CurrentCell.RowIndex;
                    string selectSQL = "";

                    OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                    if (myConn.State == System.Data.ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                    myConn.Open();


                    OleDbCommand myCommand = new OleDbCommand("", myConn);

                    int i = 0;
                    int i_new = 0;
                    while (i < tab6_dataGridView1.Rows.Count)
                    {
                        if (i < CurrentRowIndex)
                        {
                            selectSQL = " update tempdb.dbo.zhrs_t_aps2101 set ViewSort=" + i.ToString() + " where cUser_ID=" + iLoginEx.UID().ToString() + " and MoId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value);
                            myCommand.CommandText = selectSQL;
                            myCommand.ExecuteNonQuery();
                        }
                        else if (CurrentRowIndex == i)
                        {
                            for (int s = 0; s < APSTab6RowCutList.Count; s++)
                            {
                                selectSQL = " update tempdb.dbo.zhrs_t_aps2101 set ViewSort=" + (i + s).ToString() + " where cUser_ID=" + iLoginEx.UID().ToString() + " and MoId=" + Convert.ToString(tab6_dataGridView1.Rows[APSTab6RowCutList[s]].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[APSTab6RowCutList[s]].Cells["MoDId"].Value);
                                myCommand.CommandText = selectSQL;
                                myCommand.ExecuteNonQuery();
                            }
                            i_new = APSTab6RowCutList.Count + i;
                            selectSQL = " update tempdb.dbo.zhrs_t_aps2101 set ViewSort=" + i_new.ToString() + " where cUser_ID=" + iLoginEx.UID().ToString() + " and MoId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value);
                            myCommand.CommandText = selectSQL;
                            myCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            if (APSTab6RowCutList.IndexOf(i) > -1)
                            {
                                i_new--;
                            }
                            else
                            {
                                selectSQL = " update tempdb.dbo.zhrs_t_aps2101 set ViewSort=" + i_new.ToString() + " where cUser_ID=" + iLoginEx.UID().ToString() + "and MoId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[i].Cells["MoDId"].Value);
                                myCommand.CommandText = selectSQL;
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        i++;
                        i_new++;
                    }


                    if (myConn.State == System.Data.ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                    APS21Save_TempTable();
                    APS21Query(true, SQLSelect_Temp);
                    tab6_dataGridView1.Rows[CurrentRowIndex].Selected = true;
                    tab6_dataGridView1.CurrentCell = tab6_dataGridView1.Rows[CurrentRowIndex].Cells[0];
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "cmtab6RowPaste_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmtab6FindSimilarity_Click(object sender, EventArgs e)
        {
            string CurrentVaue = Convert.ToString(tab6_dataGridView1.CurrentCell.Value);
            if (CurrentVaue.Length > 0)
            {
                frmColorPanel fColor = new frmColorPanel();
                fColor.ShowDialog(this);
                string[] CurrentVaueWords = CurrentVaue.Split(' ');
                if (CurrentVaueWords.Length > 0)
                {
                    for (int k = 0; k < CurrentVaueWords.Length; k++)
                    {
                        for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                        {
                            if (CurrentVaueWords[k].IndexOf(Convert.ToString(tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Value)) > -1 || Convert.ToString(tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Value).IndexOf(CurrentVaueWords[k]) > -1)
                            {
                                tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Style.BackColor = ColorTranslator.FromHtml(fColor.GetColorByHTML());
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
                    {
                        if (CurrentVaue.IndexOf(Convert.ToString(tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Value)) > -1 || Convert.ToString(tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Value).IndexOf(CurrentVaue) > -1)
                        {
                            tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Style.BackColor = ColorTranslator.FromHtml(fColor.GetColorByHTML());
                        }
                    }
                }
            }
        }

        private void cmtab6Findword_Click(object sender, EventArgs e)
        {
            string CurrentVaue = Input.InputBox.ShowInputBox("查找内容", string.Empty);
            for (int i = 0; i < tab6_dataGridView1.Rows.Count; i++)
            {
                if (CurrentVaue.Length > 0)
                {
                    if (CurrentVaue.IndexOf(Convert.ToString(tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Value)) > -1 || Convert.ToString(tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Value).IndexOf(CurrentVaue) > -1)
                    {
                        tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Style.ForeColor = Color.Red;
                    }
                }
                else
                {
                    tab6_dataGridView1.Rows[i].Cells[tab6_dataGridView1.CurrentCell.ColumnIndex].Style.ForeColor = Color.Black;
                }
            }
        }

        private void cmtab6ReMake_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "cmtab6ReMake_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmtab6Moallocate_Click(object sender, EventArgs e)
        {
            if (tab6_dataGridView1.Rows.Count > 0)
            {
                int CrrRowIndex = tab6_dataGridView1.CurrentCell.RowIndex;

                frmMOM_Moallocate fMoallocate = new frmMOM_Moallocate(iLoginEx, Convert.ToInt32(Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["MoDId"].Value)), Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["产品编码"].Value), Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["产品名称"].Value), Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["规格型号"].Value), Convert.ToDouble(Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["排产数量"].Value)));
                fMoallocate.ShowDialog(this);
            }
        }

        private void tab6_dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)e.Control;
                tb.ContextMenuStrip = contextMenu_Tab6;
            }

            else
            {
                ((DataGridViewTextBoxEditingControl)e.Control).ContextMenuStrip = null;
            }

        }

        private void cmtab6ReLoadBOM_Click(object sender, EventArgs e)
        {
            try
            {
                for (int CrrRowIndex = 0; CrrRowIndex < tab6_dataGridView1.Rows.Count; CrrRowIndex++)
                {
                    if (tab6_dataGridView1.Rows[CrrRowIndex].Selected)
                    {
                        APS21_ReLoadBOMByManual(Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["制造单号"].Value), Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["行号M"].Value), Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["MoId"].Value), Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["MoDId"].Value));
                    }
                }

                MessageBox.Show(this, "重载BOM完成！", "重载BOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "cmtab6ReLoadBOM_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 分批生产
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmtab6OrderSplit_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                int CrrRowIndex = tab6_dataGridView1.CurrentCell.RowIndex;
                string selectSQL = "";

                OleDbCommand myCommand = new OleDbCommand("", myConn);
                OleDbDataReader myReader = null;

                double Qty = 0, QualifiedInQty = 0, UsedQty = 0, OrderSplitQty = 0;
                int ViewSort = 0;//**************************当前的行号***************************
                string MoCode = "", NewMoCode = "";
                int MoCodeSplitNum = 1;



                selectSQL = "select ViewSort=isnull(ViewSort,0) ,制造单号 \r\n";
                selectSQL += "  from  tempdb.dbo.zhrs_t_aps2101 where cUser_ID=" + iLoginEx.UID() + " and  MoId=" + Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["MoId"].Value) + " and MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["MoDId"].Value) + "  \r\n";
                myCommand.CommandText = selectSQL;
                myReader = myCommand.ExecuteReader();
                if (myReader.Read())
                {
                    ViewSort = Convert.ToInt32(myReader["ViewSort"]);
                    MoCode = myReader["制造单号"].ToString();
                    string[] MoCodePara = MoCode.Split('-');
                    if (MoCodePara.Length > 1)
                    {
                        MoCodeSplitNum = StringToInt(MoCodePara[MoCodePara.Length - 1]);
                        MoCode = MoCodePara[0];
                    }

                }
                myReader.Close();

                NewMoCode = MoCode + "-" + MoCodeSplitNum.ToString();

                selectSQL = "select MoCode from  mom_order (nolock) where  MoCode='" + NewMoCode + "' \r\n";
                myCommand.CommandText = selectSQL;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    myReader.Close();
                    MoCodeSplitNum++;
                    NewMoCode = MoCode + "-" + MoCodeSplitNum.ToString();
                    selectSQL = "select MoCode from  mom_order (nolock) where  MoCode='" + NewMoCode + "' \r\n";
                    myCommand.CommandText = selectSQL;
                    myReader = myCommand.ExecuteReader();

                }
                myReader.Close();





                selectSQL = " select Qty=sum(isnull(m.Qty,0)),QualifiedInQty=sum(isnull(m.QualifiedInQty,0)),UsedQty=sum(isnull(m.UsedQty,0)) from (   \r\n";
                selectSQL += " select Qty=排产数量,QualifiedInQty=isnull(完工数量,0),UsedQty=0 from tempdb.dbo.zhrs_t_aps2101 (nolock) where  rowid=" + Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["rowid"].Value) + "  \r\n";
                selectSQL += " union  \r\n";
                selectSQL += " select Qty=0,QualifiedInQty=0,max(CEILING(isnull(IssQty,0)/(isnull(BaseQtyN,0)/case when isnull(BaseQtyD,1)=0 then 1 else isnull(BaseQtyD,1) end))) as 'UsedQty'   \r\n";
                selectSQL += "  from mom_moallocate (nolock) where MoDId=" + Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["MoDId"].Value) + "  \r\n";
                selectSQL += " ) m  \r\n";
                myCommand.CommandText = selectSQL;
                myReader = myCommand.ExecuteReader();
                if (myReader.Read())
                {
                    Qty = Convert.ToDouble(myReader["Qty"]);
                    QualifiedInQty = Convert.ToDouble(myReader["QualifiedInQty"]);
                    UsedQty = Convert.ToDouble(myReader["UsedQty"]);
                }
                myReader.Close();
                myReader.Dispose();
                OrderSplitQty = (Qty - QualifiedInQty - UsedQty) - 1;
                OrderSplitQty = OrderSplitQty < 0 ? 0 : OrderSplitQty;
                frmOrderSplitQty fOrderSplitQty = new frmOrderSplitQty("原排产数量：" + Qty.ToString() + "\r\n已完工数量：" + QualifiedInQty.ToString() + "\r\n最大已领料套数：" + UsedQty.ToString() + "\r\n可拆分数量：" + OrderSplitQty.ToString(), OrderSplitQty);
                fOrderSplitQty.ShowDialog(this);
                if (fOrderSplitQty.GetOrderSplitQty() > 0)
                {
                    OrderSplitQty = OrderSplitQty < fOrderSplitQty.GetOrderSplitQty() ? Qty - OrderSplitQty : Qty - fOrderSplitQty.GetOrderSplitQty();//如果拆出的数量比可拆分数量大，则原单只能保留减掉可拆分数量后的数量，即：拆后的排产数量=原排产量-可拆数量

                    selectSQL = "update tempdb.dbo.zhrs_t_aps2101 set ViewSort=ViewSort+1  where cUser_ID=" + iLoginEx.UID() + " and ViewSort>" + ViewSort.ToString() + "  \r\n";
                    selectSQL += "   \r\n";
                    selectSQL += " insert into tempdb.dbo.zhrs_t_aps2101(cUser_ID,ReScheduleType,ReScheduleSourceMoId,ReScheduleSourceMoDId,ViewSort,部门,优先级,制造单号,行号M,销售订单,业务员,行号S,产品编码,产品名称,规格型号,投产,计划生产日期,排产数量,完工数量,结案,LOGO,软件信息,备注,欠料提示,调库存,MoId,ModId)  \r\n";
                    selectSQL += " select cUser_ID,ReScheduleType=1,(case when ReScheduleType=1 and  MoId=0 then ReScheduleSourceMoId else MoId end ) as 'ReScheduleSourceMoId',(case when ReScheduleType=1 and  MoDId=0 then ReScheduleSourceMoDId else MoDId end ) as 'ReScheduleSourceMoDId',ViewSort=" + ViewSort.ToString() + "+1,部门,优先级,'" + NewMoCode + "' as '制造单号',行号M,销售订单,业务员,行号S,产品编码,产品名称,规格型号,投产,计划生产日期,排产数量=" + fOrderSplitQty.GetOrderSplitQty().ToString() + ",完工数量,结案,LOGO,软件信息,备注,欠料提示,调库存,MoId,ModId=0  \r\n";
                    selectSQL += "  from  tempdb.dbo.zhrs_t_aps2101 where cUser_ID=" + iLoginEx.UID() + " and  rowid=" + Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["rowid"].Value) + "  \r\n";
                    selectSQL += "   \r\n";
                    selectSQL += " update tempdb.dbo.zhrs_t_aps2101 set 排产数量=" + OrderSplitQty.ToString() + ",ReScheduleType=1  where cUser_ID=" + iLoginEx.UID() + " and rowid=" + Convert.ToString(tab6_dataGridView1.Rows[CrrRowIndex].Cells["rowid"].Value) + "  \r\n";
                    myCommand.CommandText = selectSQL;
                    myCommand.ExecuteNonQuery();
                    DoOrderSplit = true;
                }

                APS21Query(true, SQLSelect_Temp);
                tab6_dataGridView1.Rows[CrrRowIndex].Selected = true;
                tab6_dataGridView1.CurrentCell = tab6_dataGridView1.Rows[CrrRowIndex].Cells[0];
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "cmtab6ReLoadBOM_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int StringToInt(string iStringValue)
        {
            int resultValue = 1;

            try
            {
                resultValue = Convert.ToInt32(iStringValue) + 1;
            }
            catch// (Exception ex)
            {
                // MessageBox.Show(this, ex.ToString(), "cmtab6ReLoadBOM_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultValue;
        }

        private void cmtab6OrderMerge_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();


                string selectSQL = "";

                OleDbCommand myCommand = new OleDbCommand("", myConn);
                OleDbDataReader myReader = null;


            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "cmtab6OrderMerge_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tab6_dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            CellMouseDown = true;
            SLBTotal.Text = "";
        }

        private void tab6_dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //if (e.ColumnIndex >= 0 && e.ColumnIndex <= 11)
                //{
                double SelectTotal = 0.0;
                int selectedCellCount = tab6_dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0 && CellMouseDown)
                {
                    SelectTotal = 0.0;
                    for (int i = 0; i < selectedCellCount; i++)
                    {
                        SelectTotal += Convert.ToDouble(Convert.ToString(Convert.IsDBNull(tab6_dataGridView1.SelectedCells[i].Value) ? "" : tab6_dataGridView1.SelectedCells[i].Value) == "" ? "0" : tab6_dataGridView1.SelectedCells[i].Value.ToString());
                    }
                    SLBTotal.Text = string.Format("{0:N0}", SelectTotal);
                }
                //}

            }
            catch
            {
            }
        }

        private void tab6_dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            tab6_dataGridView1_CellMouseMove(sender, e);
            CellMouseDown = false;
        }

        private void tab6_dataGridView1_Click(object sender, EventArgs e)
        {
            SLbState.Text = "";
        }

        private void cmtab6MissingMaterials_Click(object sender, EventArgs e)
        {
            APS21MOMSupply_MoallocateDetailed();
        }

        private void tab6_toStandard_Click(object sender, EventArgs e)
        {
            if (tab6_dataGridView1.Columns.Count > 0)
            {

                for (int i = 0; i < tab6_dataGridView1.SelectedRows.Count; i++)
                {

                    tab6_dataGridView1.SelectedRows[i].Cells["类别"].Value = "标准";

                }

            }
        }

        private void tab6_toNotStandard_Click(object sender, EventArgs e)
        {
            if (tab6_dataGridView1.Columns.Count > 0)
            {

                for (int i = 0; i < tab6_dataGridView1.SelectedRows.Count; i++)
                {

                    tab6_dataGridView1.SelectedRows[i].Cells["类别"].Value = "非标";

                }

            }
        }

       
        
        private void tsmiStock_Click(object sender, EventArgs e)
        {
            OLEDBHelper.iLoginEx = iLoginEx;
            string selectSQL = "";
            if (tab6_dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                   // selectSQL = " if exists (select 1  from tempdb.dbo.sysobjects where id = object_id(N'zhrs_t_APS21MOMSMoalloDetail_zzc" + iLoginEx.GetMacAddress().Replace(":", "") + "') and type='U')   \r\n";
                    selectSQL = "if object_id('zhrs_t_APS21MOMSMoalloDetail_zzc" + iLoginEx.GetMacAddress().Replace(":", "") + "') is not null";
                    selectSQL += " drop table zhrs_t_APS21MOMSMoalloDetail_zzc" + iLoginEx.GetMacAddress().Replace(":", "") + " ;   \r\n";
                    OLEDBHelper.ExecuteNonQuery(selectSQL, CommandType.Text);
                    selectSQL = " CREATE TABLE zhrs_t_APS21MOMSMoalloDetail_zzc" + iLoginEx.GetMacAddress().Replace(":", "") + "( \r\n";
                    selectSQL += " 	modid int)  \r\n";
                    selectSQL += "   \r\n";
                    OLEDBHelper.ExecuteNonQuery(selectSQL, CommandType.Text);
                    StringBuilder str = new StringBuilder();
                    for (int r = 0; r < tab6_dataGridView1.SelectedRows.Count; r++)
                    {
                        selectSQL = " insert into zhrs_t_APS21MOMSMoalloDetail_zzc" + iLoginEx.GetMacAddress().Replace(":", "") + "(modid)values(" + Convert.ToString(tab6_dataGridView1.SelectedRows[r].Cells["MoDId"].Value) + ");";
                        str.Append(selectSQL);
                    }
                    OLEDBHelper.ExecuteNonQuery(str.ToString(), CommandType.Text);
                    StringBuilder str_InvCode = new StringBuilder();

                    selectSQL = "select o.MoCode as '制造单号', m.sortSeq as '行号',m.InvCode as '产品编码',m.Qty as '排产数量',a.SortSeq as '子件行号',a.OpSeq as '工序行号',rt.Description as '工序说明',a.InvCode , i.cInvName as '物料名称',i.cInvStd as '规格型号',    \r\n";
                    selectSQL += "   a.BaseQtyN/case when isnull(a.BaseQtyD,1)=0 then 1 else isnull(a.BaseQtyD,1) end  as '单位用量',a.Qty as '用量',a.IssQty as '已领数',a.TransQty as '已调拨数',    \r\n";
                    selectSQL += "   case  when a.WIPType=1 then '入库倒冲' else case when a.WIPType=2 then '工序倒冲' else case when a.WIPType=3 then '领用' else case when a.WIPType=4 then '直接供应'  end end end end as '供应类型',    \r\n";
                    selectSQL += "   isnull(a.Remark,a.Define29) as '备注'     \r\n";
                    selectSQL += "   from mom_moallocate a (nolock)    \r\n";
                    selectSQL += "   left join inventory i (nolock) on a.invcode=i.cinvcode   \r\n";
                    selectSQL += "   left join mom_orderdetail m (nolock)on a.modid=m.modid    \r\n";
                    selectSQL += "   left join mom_order o (nolock)on o.moid=m.moid   \r\n";
                    selectSQL += "   left join (    \r\n";
                    selectSQL += "  select OpSeq ,RoutingId=MoDId,Description from sfc_moroutingdetail     \r\n";
                    selectSQL += "   union     \r\n";
                    selectSQL += "   select OpSeq,RoutingId=PRoutingId,Description from sfc_proutingdetail     \r\n";
                    selectSQL += "  union     \r\n";
                    selectSQL += "   select OpSeq,RoutingId=EcnRoutingId,Description from ecn_proutingdetail     \r\n";
                    selectSQL += "  ) rt on a.OpSeq=rt.OpSeq and m.RoutingId=rt.RoutingId    \r\n";
                    selectSQL += "   where  exists (select 1 from zhrs_t_APS21MOMSMoalloDetail_zzc" + iLoginEx.GetMacAddress().Replace(":", "") + " tmp where a.modid=tmp.modid)     \r\n";
                    selectSQL += "   \r\n";
                    OleDbDataReader dr = OLEDBHelper.ExecuteReader(selectSQL, CommandType.Text);
                    while (dr.Read())
                    {
                        str_InvCode.Append(dr["InvCode"].ToString() + ";");
                    }
                    dr.Close();
                    OLEDBHelper.CloseCon();

                    this.Text = "排产管理   正在查询，请稍候...";
                    System.Windows.Forms.Application.DoEvents();

                    string mySelectQuery = "", cInvCode = "";
                    //合计

                    mySelectQuery += "   \r\n";
                    mySelectQuery += " select '合计' as  'DocType' ,'' as 'cCode',null as 'dDate','' as 'cDefine30',a.cinvcode,p.cInvName,replace(replace(p.cinvstd,'''',''),'\"','') as cInvStd,p.cInvDefine7 ,a.moQty,a.Now_PurArrQty,a.Now_PurQty,a.CurSotckQty, (isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)) as 'allSotckQty',a.useQty, a.toArrQty,a.AltmQty, \r\n";
                    //可用量=采购在途+到货在检+现存量-已分配量
                    mySelectQuery += "   (isnull(a.Now_PurQty,0)+isnull(a.Now_PurArrQty,0)+isnull(a.CurSotckQty,0)+isnull(a.AltmQty,0)-isnull(a.useQty,0)) as  'AvailableQty'   from   \r\n";
                    tab2_toArrQty.HeaderText = "即将到货";
                    tab2_Now_PurQty.HeaderText = "采购在途(A)";
                    mySelectQuery += " (select cinvcode,sum(isnull(moQty,0)) as 'moQty',sum(isnull(Now_PurArrQty,0)) as 'Now_PurArrQty',sum(isnull(Now_PurQty,0)) as 'Now_PurQty',  sum(isnull(CurSotckQty,0)) as 'CurSotckQty',  \r\n";
                    mySelectQuery += " sum(isnull(useQty,0)) as 'useQty',sum(isnull(toArrQty,0)) as 'toArrQty',sum(isnull(AltmQty,0)) as 'AltmQty'  \r\n";
                    mySelectQuery += "    from " + wComprehensiveStock.ComprehensiveStockInfo(iLoginEx, 0, cInvCode, "", "", iLoginEx.pubDB_UF(), "", "") + " vw    \r\n";
                    mySelectQuery += "     where  1=1 ";

                    str_InvCode = str_InvCode.Replace("；", ";");
                    tab2_cinvCodeAny.Text = str_InvCode.ToString();
                    if (str_InvCode.Length > 0)
                    {
                        string cinvCodeChild = "";
                        string[] paraCinvCode = str_InvCode.ToString().Split(';');
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
                            mySelectQuery += " and  cinvcode ='" + str_InvCode + "' \r\n";
                        }
                    }
                    mySelectQuery += "  group by cinvcode) a left join inventory p on a.cinvcode=p.cinvcode    \r\n";
                    mySelectQuery += "  order by a.cinvcode   \r\n";
                    DataTable dt = new DataTable();
                    dt = OLEDBHelper.GetDataTalbe(mySelectQuery, CommandType.Text);
                    this.tab2_dataGridView1.AutoGenerateColumns = false;//不自动生成列
                    //设置数据源    

                    this.tab2_dataGridView1.DataSource = dt;//数据源 

                    //标准居中
                    this.tab2_dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    //设置自动换行

                    this.tab2_dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    //设置自动调整高度

                    this.tab2_dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    for (int i = 0; i < tab2_dataGridView1.Rows.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            tab2_dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                        }
                    }

                    tabControl1.SelectedIndex = 4;

                    this.Text = "排产管理   查询完成！共" + (tab2_dataGridView1.RowCount).ToString() + "行";
                    System.Windows.Forms.Application.DoEvents();
                }

                catch (Exception ex)
                {
                    this.Text = "排产管理";
                    MessageBox.Show(this, ex.ToString(), "PDBOM.btnQuery_Click()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally { GC.Collect(); }
              

            }
        }
    }
}
