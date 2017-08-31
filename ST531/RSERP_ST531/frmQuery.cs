using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UTLoginEx;
using RSERP;


namespace RSERP_ST531
{
    public partial class frmQuery : Form
    {
        private UTLoginEx.LoginEx iLoginEx = new LoginEx();
        private string selectSQL = "";
        private RSERP.ComprehensiveStock wComprehensiveStock = new ComprehensiveStock();
        private int mOutMonth = 0;
        private bool mchkComprehensiveStock = false;
        private string mWhCode = "";
        public frmQuery(UTLoginEx.LoginEx iiLoginEx)
        {
            InitializeComponent();
            iLoginEx = iiLoginEx;
        }
        public string GetSQL()
        {
            return selectSQL;
        }
        public bool GetchkComprehensiveStock()
        {
            return mchkComprehensiveStock;
        }
        public int OutMonth()
        {
            return mOutMonth;
        }
        public string WhCode()
        {
            return mWhCode;
        }

        private void frmQuery_Load(object sender, EventArgs e)
        {
            chkInMonths.Checked = iLoginEx.ReadUserProfileValue("Query", "chkInMonths") == "0" ? false : true;
            txtInMonths.Text = iLoginEx.ReadUserProfileValue("Query", "InMonths") == "" ? "2" : iLoginEx.ReadUserProfileValue("Query", "InMonths");
            chkOutMonths.Checked = iLoginEx.ReadUserProfileValue("Query", "chkOutMonths") == "0" ? false : true;
            txtOutMonths.Text = iLoginEx.ReadUserProfileValue("Query", "OutMonths") == "" ? "1" : iLoginEx.ReadUserProfileValue("Query", "OutMonths");
            txtPercent.Text = iLoginEx.ReadUserProfileValue("Query", "Percent") == "" ? "33" : iLoginEx.ReadUserProfileValue("Query", "Percent");
            chkInMonths2.Checked = iLoginEx.ReadUserProfileValue("Query", "chkInMonths2") == "1" ? true : false;
            txtInMonths2.Text = iLoginEx.ReadUserProfileValue("Query", "txtInMonths2") == "" ? "1" : iLoginEx.ReadUserProfileValue("Query", "txtInMonths2");
            chkOutMonths2.Checked = iLoginEx.ReadUserProfileValue("Query", "chkOutMonths2") == "1" ? true : false;
            txtOutMonths2.Text = iLoginEx.ReadUserProfileValue("Query", "txtOutMonths2") == "" ? "1" : iLoginEx.ReadUserProfileValue("Query", "txtOutMonths2");
            chkComprehensiveStock.Checked = iLoginEx.ReadUserProfileValue("Query", "chkComprehensiveStock") == "1" ? true : false;

            chkOut_Month();
            chkInMonths_CheckedChanged(null, null);
            chkInMonths2_CheckedChanged(null, null);

        }

        private void WriteUserProfileValue()
        {
            iLoginEx.WriteUserProfileValue("Query", "InMonths", txtInMonths.Text);
            iLoginEx.WriteUserProfileValue("Query", "OutMonths", txtOutMonths.Text);
            iLoginEx.WriteUserProfileValue("Query", "Percent", txtPercent.Text);

            iLoginEx.WriteUserProfileValue("Query", "chkInMonths", chkInMonths.Checked ? "1" : "0");
            iLoginEx.WriteUserProfileValue("Query", "chkOutMonths", chkOutMonths.Checked ? "1" : "0");
            iLoginEx.WriteUserProfileValue("Query", "chkInMonths2", chkInMonths2.Checked ? "1" : "0");
            iLoginEx.WriteUserProfileValue("Query", "txtInMonths2", txtInMonths2.Text);
            iLoginEx.WriteUserProfileValue("Query", "chkOutMonths2", chkOutMonths2.Checked ? "1" : "0");
            iLoginEx.WriteUserProfileValue("Query", "txtOutMonths2", txtOutMonths2.Text);
            iLoginEx.WriteUserProfileValue("Query", "chkComprehensiveStock", chkComprehensiveStock.Checked ? "1" : "0");

        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            //if (chkOutMonths.Checked && chkOutMonths2.Checked)
            //{
            //    txtOutMonths.Text = txtOutMonths2.Text;
            //}

            if (chkOutMonths2.Checked)
            {
                mOutMonth = Convert.ToInt32(txtOutMonths2.Text);
            }
            else
            {
                mOutMonth = Convert.ToInt32(txtOutMonths.Text);
            }

            mWhCode = txtWhCode.Text.Trim();


            selectSQL = "  \r\n";
            selectSQL += " select out.cinvcode as '物料编码',i.cInvName as'物料名称',i.cInvStd as '规格型号',out.stQty as'库存数量', isnull(prc.unitcost*prc.exchangerate,0)*out.stQty as '库存金额' ,\r\n";
            selectSQL += " out.OutQty as '出库数量',out.OutQty*isnull(prc.unitcost*prc.exchangerate,0) as '出库金额'  \r\n";
            if (chkComprehensiveStock.Checked)
            {
                selectSQL += ",out.useQty as '已分配量',out.Now_PurQty as '采购在途',out.Now_PurArrQty as '到货在检', out.moQty as '在制'  \r\n";
            }
            selectSQL += " from (  \r\n";
            if (chkComprehensiveStock.Checked)
            {
                selectSQL += " select cinvcode, sum(isnull(stQty,0)) as  'stQty',sum(isnull(OutQty,0)) as  'OutQty',sum(useQty) as 'useQty',sum(Now_PurQty) as 'Now_PurQty',sum(Now_PurArrQty) as 'Now_PurArrQty',sum(moQty) as 'moQty' from   \r\n";
            }
            else
            {
                selectSQL += " select cinvcode, sum(isnull(stQty,0)) as  'stQty',sum(isnull(OutQty,0)) as  'OutQty' from   \r\n";
            }
            selectSQL += " (  \r\n";
            if (chkComprehensiveStock.Checked)
            {
                selectSQL += " select  vw.cinvcode,vw.moQty,vw.Now_PurArrQty,vw.Now_PurQty,vw.CurSotckQty as 'stQty',vw.useQty,OutQty=0 from  \r\n";
                selectSQL += wComprehensiveStock.ComprehensiveStockInfo(iLoginEx,0, "", "", "", iLoginEx.pubDB_UF(), mWhCode,"") + " \r\n";
                selectSQL += "  vw \r\n";
            }
            else
            {
                selectSQL += " select cinvcode, iquantity as 'stQty',0 as 'OutQty' from CurrentStock a (nolock) where a.iquantity>0 and exists(select 1 from Warehouse w (nolock) where a.cWhCode=w.cWhCode  and w.bMRP=1)  \r\n";

                mWhCode = mWhCode.Replace("\r", "");
                mWhCode = mWhCode.Replace("\n", "");
                mWhCode = mWhCode.Replace("；", ";");

                if (mWhCode.Length > 0)
                {
                    string mWhCodeChild = "";
                    string[] paramWhCode = mWhCode.Split(';');
                    if (paramWhCode.Length > 0)
                    {
                        for (int i = 0; i < paramWhCode.Length; i++)
                        {
                            mWhCodeChild += "'" + paramWhCode[i].ToString() + "',";
                        }

                        selectSQL += " and  a.cWhCode in (" + mWhCodeChild + "'\r\n'" + ") \r\n";
                    }
                    else
                    {
                        selectSQL += " and  a.cWhCode ='" + mWhCode + "' \r\n";
                    }
                }
            }
            selectSQL += " union all  \r\n";
            if (chkComprehensiveStock.Checked)
            {
                selectSQL += " select  a.cinvcode,moQty=0,Now_PurArrQty=0,Now_PurQty=0,stQty=0,useQty=0, a.iquantity as 'OutQty' ";
            }
            else
            {
                selectSQL += " select  a.cinvcode,stQty=0, a.iquantity as 'OutQty' ";
            }

            selectSQL += " from RdRecords a (nolock) left join RdRecord b (nolock)on a.id=b.id  where b.cVouchType='11' and b.ddate between left(convert(varchar,dateadd(mm,-";
            if (chkOutMonths.Checked)
            {
                selectSQL += txtOutMonths.Text;
            }
            else if (chkOutMonths2.Checked)
            {
                selectSQL += txtOutMonths2.Text;
            }
            selectSQL += ",getdate()),20),10) and left(convert(varchar,getdate(),20),10)     \r\n";
            selectSQL += " ) m group by m.cinvcode  \r\n";
            selectSQL += " ) out   \r\n";
            selectSQL += " left join Inventory i (nolock) on i.cInvCode=out.cInvCode  \r\n";
            selectSQL += " left join zhrs_t_LastPoPrice prc(nolock) on  prc.cInvCode=out.cInvCode where prc.ponotype=1  and out.stQty>0 \r\n";


            if (chkOutMonths2.Checked)
            {
                selectSQL += " and out.OutQty=0  \r\n";
            }
            else if (chkOutMonths.Checked)
            {
                selectSQL += " and out.OutQty<(out.stQty*" + txtPercent.Text + "/100) \r\n";
            }


            if (!(chkInMonths.Checked && chkInMonths2.Checked))//二选1,当两个都选的时候，则表示，根本不考虑有没有采购入库这回事
            {
                if (chkInMonths.Checked)
                {
                    //n月内无采购
                    selectSQL += " and not out.cinvcode in   \r\n";
                    selectSQL += " (select a.cinvcode from RdRecords a (nolock) left join RdRecord b (nolock)on a.id=b.id  where b.cVouchType='01' and b.ddate between left(convert(varchar,dateadd(mm,-" + txtInMonths.Text + ",getdate()),20),10) and left(convert(varchar,getdate(),20),10) group by a.cinvcode)   \r\n";
                }
                if (chkInMonths2.Checked)
                {
                    //n月内有采购
                    selectSQL += " and  out.cinvcode in   \r\n";
                    selectSQL += " (select a.cinvcode from RdRecords a (nolock) left join RdRecord b (nolock)on a.id=b.id  where b.cVouchType='01' and b.ddate between left(convert(varchar,dateadd(mm,-" + txtInMonths.Text + ",getdate()),20),10) and left(convert(varchar,getdate(),20),10) group by a.cinvcode)   \r\n";
                }
            }
            selectSQL += "  order by  isnull(prc.unitcost*prc.exchangerate,0)*out.stQty desc \r\n";
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectSQL = "";
            this.Close();
        }

        private void frmQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteUserProfileValue();
        }

        private void chkOut_Month()
        {
            if (chkOutMonths2.Checked)
            {
                txtOutMonths2.Enabled = true;
                txtOutMonths.Enabled = false;
                txtPercent.Enabled = false;
            }
            else
            {
                txtOutMonths2.Enabled = false;
                txtOutMonths.Enabled = true;
                txtPercent.Enabled = true;
            }
        }

        private void chkOutMonths2_CheckedChanged(object sender, EventArgs e)
        {
            chkOut_Month();
        }

        private void chkOutMonths_CheckedChanged(object sender, EventArgs e)
        {
            chkOut_Month();
        }

        private void chkInMonths_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInMonths.Checked)
            {
                txtInMonths.Enabled = true;
            }
            else
            {
                txtInMonths.Enabled = false;
            }
        }

        private void chkInMonths2_CheckedChanged(object sender, EventArgs e)
        {

            if (chkInMonths2.Checked)
            {
                txtInMonths2.Enabled = true;
            }
            else
            {
                txtInMonths2.Enabled = false;
            }
        }

        private void txtWhCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtWhCode.Text = iLoginEx.OpenSelectWindow("仓库", "select cWhCode as '仓库代码',cWhName as '仓库名称' from Warehouse (nolock) where cWhCode in (select cWhCode from " + iLoginEx.pubDB_UT() + "..StockClose (nolock) where cDisable=0 and cAccID='" + iLoginEx.AccID() + "')", txtWhCode.Text, 430, 300, 1, true);
            //string[] para = txtWhCode.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            //if (para.Length > 1)
            //{
            //    txtWhCode.Text = para[0];
            //}

        }
    }
}
