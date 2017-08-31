using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UTLoginEx;

namespace RSERP_ST531
{
    public partial class StockQuery : Form
    {
        public bool IsQuery = false;
        private UTLoginEx.LoginEx iLoginEx = new LoginEx();
        private int AuthID = 0;
        public StockQuery(LoginEx iiLoginEx, int iAuthID)
        {
            InitializeComponent();
            iLoginEx = iiLoginEx;
            AuthID = iAuthID;
        }

        private void StockQuery_Load(object sender, EventArgs e)
        {
            try
            {
                txtcInvCCode.Text = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCCode");//物料分类
                txtcWhCode.Text = iLoginEx.ReadUserProfileValue("StockQuery", "cWhCode");//仓库代码
                txtcInvCode.Text = iLoginEx.ReadUserProfileValue("StockQuery", "cInvCode");//物料编码
                txtQtyL.Text = iLoginEx.ReadUserProfileValue("StockQuery", "QtyL");//数量L 
                txtQtyH.Text = iLoginEx.ReadUserProfileValue("StockQuery", "QtyH");//数量H
                txtPriceL.Text = iLoginEx.ReadUserProfileValue("StockQuery", "PriceL");//单价L
                txtPriceH.Text = iLoginEx.ReadUserProfileValue("StockQuery", "PriceH");//单价H
                txtAmtL.Text = iLoginEx.ReadUserProfileValue("StockQuery", "AmtL");//库存金额L
                txtAmtH.Text = iLoginEx.ReadUserProfileValue("StockQuery", "AmtH");//库存金额H
                cmbQureyType.SelectedIndex = iLoginEx.ReadUserProfileValue("StockQuery", "QureyType").Length == 0 ? 0 : Convert.ToInt32(iLoginEx.ReadUserProfileValue("StockQuery", "QureyType"));//0=现存量；1=综合查询
                cmbABC.SelectedIndex = iLoginEx.ReadUserProfileValue("StockQuery", "InvABC").Length == 0 ? 0 : Convert.ToInt32(iLoginEx.ReadUserProfileValue("StockQuery", "InvABC"));//ABC分类

                txtPriceL.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Price);
                txtPriceH.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Price);
                txtAmtL.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Price);
                txtAmtH.Enabled = iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Price);
                if (!iLoginEx.CheckAuthorityDetail(iLoginEx.UserId(), AuthID, LoginEx.AuthorityDetailType.Price))
                {
                    txtPriceL.Text = "";
                    txtPriceH.Text = "";
                    txtAmtL.Text = "";
                    txtAmtH.Text = "";
                }
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "StockQuery_Load()");
                frmmsg.ShowDialog(this);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsQuery = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                iLoginEx.WriteUserProfileValue("StockQuery", "cInvCCode", txtcInvCCode.Text);
                iLoginEx.WriteUserProfileValue("StockQuery", "cWhCode", txtcWhCode.Text);
                iLoginEx.WriteUserProfileValue("StockQuery", "cInvCode", txtcInvCode.Text);
                iLoginEx.WriteUserProfileValue("StockQuery", "QtyL", txtQtyL.Text);
                iLoginEx.WriteUserProfileValue("StockQuery", "QtyH", txtQtyH.Text);
                iLoginEx.WriteUserProfileValue("StockQuery", "PriceL", txtPriceL.Text);
                iLoginEx.WriteUserProfileValue("StockQuery", "PriceH", txtPriceH.Text);
                iLoginEx.WriteUserProfileValue("StockQuery", "AmtL", txtAmtL.Text);
                iLoginEx.WriteUserProfileValue("StockQuery", "AmtH", txtAmtH.Text);

                iLoginEx.WriteUserProfileValue("StockQuery", "QureyType", cmbQureyType.SelectedIndex.ToString());
                iLoginEx.WriteUserProfileValue("StockQuery", "InvABC", cmbABC.SelectedIndex.ToString());
                IsQuery = true;
                this.Close();
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "btnOK_Click()");
                frmmsg.ShowDialog(this);
            }
        }

        private void txtcInvCCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                frmcInvCCodeTree ftree = new frmcInvCCodeTree(iLoginEx);
                ftree.ShowDialog(this);
                txtcInvCCode.Text = ftree.cInvCCode;
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "txtcInvCCode_MouseDoubleClick()");
                frmmsg.ShowDialog(this);
            }
        }

        private void txtcWhCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtcWhCode.Text = iLoginEx.OpenSelectWindow("仓库", "select cWhCode as '仓库代码',cWhName as '仓库名称' from Warehouse (nolock) where cWhCode in (select cWhCode from " + iLoginEx.pubDB_UT() + "..StockClose (nolock) where cDisable=0 and cAccID='" + iLoginEx.AccID() + "')", txtcWhCode.Text, 430, 300, 1, true);
        }

        private void txtcInvCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtcInvCode.Text = iLoginEx.OpenSelectWindow("存货档案", " select cInvCode as '物料编码',cInvName as '物料名称',cInvStd as '规格' from Inventory (nolock) ", txtcInvCode.Text, 430, 300, 1, true);
        }
    }
}
