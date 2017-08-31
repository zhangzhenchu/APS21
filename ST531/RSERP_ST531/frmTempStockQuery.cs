using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RSERP_ST531
{
    public partial class frmTempStockQuery : Form
    {
        private string selectSQL = "";
        public frmTempStockQuery()
        {
            InitializeComponent();
        }

        public string GetSQL()
        {
            return selectSQL;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            selectSQL = "select a.cWhCode as '仓库代码',w.cWhName  as '仓库名称',a.cinvcode as '物料编码',i.cInvName as'物料名称',i.cInvStd as '规格型号',a.Qty as '现存量',a.cmtInQty as '本期累计入库数',a.cmtOutQty as '本期累计出库数' from zhrs_t_MRP_CurrentStock a  \r\n";
            selectSQL += "  left join Inventory i (nolock) on i.cInvCode=a.cInvCode   \r\n";
            selectSQL += "  left join Warehouse  w (nolock) on a.cWhCode=w.cWhCode  where 1=1  \r\n";
            if (txtcWhCodeL.Text.Length > 0)
            {
                selectSQL += "  and a.cWhCode>='" + txtcWhCodeL.Text + "'";
            }

            if (txtcWhCodeH.Text.Length > 0)
            {
                selectSQL += "  and  a.cWhCode<='" + txtcWhCodeH.Text + "'";
            }


            if (txtcInvCodeL.Text.Length > 0)
            {
                selectSQL += "  and  a.cInvCode>='" + txtcInvCodeL.Text + "'";
            }

            if (txtcInvCodeH.Text.Length > 0)
            {
                selectSQL += "  and  a.cInvCode<='" + txtcInvCodeH.Text + "'";
            }
            this.Close();  
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectSQL = "";
            this.Close();
        }

        private void txtcWhCodeL_Leave(object sender, EventArgs e)
        {

            if (txtcWhCodeH.Text.Length == 0 && txtcWhCodeL.Text.Length > 0)
            {
                txtcWhCodeH.Text = txtcWhCodeL.Text;
            }
        }

        private void txtcInvCodeL_Leave(object sender, EventArgs e)
        {
            if (txtcInvCodeH.Text.Length == 0 && txtcInvCodeL.Text.Length > 0)
            {
                txtcInvCodeH.Text = txtcInvCodeL.Text;
            }
        }
    }
}
