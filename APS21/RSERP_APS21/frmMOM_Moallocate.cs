using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using UTLoginEx;

namespace RSERP_APS21
{
    public partial class frmMOM_Moallocate : Form
    {
        private UTLoginEx.LoginEx iLoginEx = new LoginEx();
        private int MoDid = 0;
        private string MoCode = "", cInvCode = "", cInName = "", cInStd = "";
        private double Qty = 0;
        private int FormWidth = 0, FormHeight = 0, dataGridView1Width = 0, dataGridView1Height = 0;

        public frmMOM_Moallocate(UTLoginEx.LoginEx iiLoginEx, int iMoDid, string iMoCode, string icInvCode, string icInName, string icInStd, double iQty)
        {
            InitializeComponent();
            iLoginEx = iiLoginEx;
            MoDid = iMoDid;
            MoCode = iMoCode;
            cInvCode = icInvCode;
            cInName = icInName;
            cInStd = icInStd;
            Qty = iQty;
        }


        private void LoadData()
        {
            try
            {
                txtcInvCode.Text = cInvCode; 
                txtcInvName.Text = cInName;
                txtcInvStd.Text = cInStd;
                txtQty.Text = Qty.ToString();

                this.Text = "制造单子件 " + MoCode;
 
                string selectSQL = null;

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                selectSQL = "select a.SortSeq as '行号',a.OpSeq as '工序行号',rt.Description as '工序说明',a.InvCode as '物料编码', i.cInvName as '物料名称',i.cInvStd as '规格型号',  \r\n";
                selectSQL += "  a.BaseQtyN/case when isnull(a.BaseQtyD,1)=0 then 1 else isnull(a.BaseQtyD,1) end  as '单位用量',a.Qty as '用量',a.IssQty as '已领数',a.TransQty as '已调拨数',  \r\n";
                selectSQL += "  case  when a.WIPType=1 then '入库倒冲' else case when a.WIPType=2 then '工序倒冲' else case when a.WIPType=3 then '领用' else case when a.WIPType=4 then '直接供应'  end end end end as '供应类型',  \r\n";
                selectSQL += "  isnull(a.WhCode,'')+'-'+isnull(w.cWhName,'') as '仓库', isnull(a.Remark,a.Define29) as '备注'   \r\n";
                selectSQL += "  from mom_moallocate a (nolock)  \r\n";
                selectSQL += "  left join inventory i (nolock) on a.invcode=i.cinvcode  \r\n";
                selectSQL += "  left join Warehouse w (nolock) on a.WhCode=w.cWhCode   \r\n";
                selectSQL += "  left join mom_orderdetail m (nolock)on a.modid=m.modid  \r\n";
                selectSQL += "  left join (  \r\n";
                selectSQL += " select OpSeq ,RoutingId=MoDId,Description from sfc_moroutingdetail   \r\n";
                selectSQL += "  union   \r\n";
                selectSQL += "  select OpSeq,RoutingId=PRoutingId,Description from sfc_proutingdetail   \r\n";
                selectSQL += " union   \r\n";
                selectSQL += "  select OpSeq,RoutingId=EcnRoutingId,Description from ecn_proutingdetail   \r\n";
                selectSQL += " ) rt on a.OpSeq=rt.OpSeq and m.RoutingId=rt.RoutingId  \r\n";
                selectSQL += "  where a.modid=" + MoDid.ToString() + "  \r\n";

                OleDbCommand myCommand = new OleDbCommand(selectSQL, myConn);
                this.dataGridView1.AutoGenerateColumns = true;
                //设置数据源    


                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(myCommand);
                da.Fill(ds);
                this.dataGridView1.DataSource = ds.Tables[0];//数据源 


                //标准居中
                this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //设置自动换行

                this.dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //设置自动调整高度

                this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].ReadOnly = true;
                    if (i >= 7 && i <= 9)
                    {
                        dataGridView1.Columns[i].DefaultCellStyle.Format = "#,###0.00";
                        dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                dataGridView1.Columns[6].DefaultCellStyle.Format = "#,###0.0000";
                dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }

            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "LoadData()");
                frmmsg.ShowDialog(this);
            }
        }

        private void toolRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void toolClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolToExcel_Click(object sender, EventArgs e)
        {
            iLoginEx.ExportExcel("制造单子件_" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "_").Replace(".", "_").Replace(":", "_").Replace("/", "_").Replace(" ", "_"), "制造单子件", dataGridView1, 6);
        }

        private void frmMOM_Moallocate_Load(object sender, EventArgs e)
        {
            FormWidth = this.Width;
            FormHeight = this.Height;
            dataGridView1Width = dataGridView1.Width;
            dataGridView1Height = dataGridView1.Height;
            LoadData();
        }

        private void frmMOM_Moallocate_Resize(object sender, EventArgs e)
        {
            Ini ini = new Ini(System.Windows.Forms.Application.StartupPath.ToString() + "\\utconfig.ini");
            ini.Writue("Window", "AutoAdaptive2", "");
            if (ini.ReadValue("Window", "AutoAdaptive") != "N")
            {
                dataGridView1.Width = dataGridView1Width + (this.Width - FormWidth);
                dataGridView1.Height = dataGridView1Height + (this.Height - FormHeight);
            }
        }
    }
}
