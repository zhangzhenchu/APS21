using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RSERP_APS21
{
    public partial class frmOrderSplitQty : Form
    {
        private double OrderSplitQty = 0;
        private string mMSG = "";

        public frmOrderSplitQty(string iMSG, double iOrderSplitQty)
        {
            InitializeComponent();
            
            mMSG = iMSG;
            OrderSplitQty = iOrderSplitQty;
        }

        private void txtOrderSplitQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            //检测是否已经输入了小数点 
            bool IsContainsDot = this.txtOrderSplitQty.Text.Contains(".");
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
            else if (IsContainsDot && (e.KeyChar == 46)) //如果输入了小数点，并且再次输入 
            {
                e.Handled = true;
            }
        }
        public double GetOrderSplitQty()
        {
            return OrderSplitQty;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OrderSplitQty = Convert.ToDouble(txtOrderSplitQty.Text.Trim());
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OrderSplitQty = 0;
            this.Close();
        }

        private void frmOrderSplitQty_Load(object sender, EventArgs e)
        {
            lbMsg.Text = mMSG;
            txtOrderSplitQty.Text = OrderSplitQty.ToString();   
        }
    }
}
