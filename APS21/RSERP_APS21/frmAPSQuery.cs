using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UTLoginEx;
using System.Data.OleDb;

namespace RSERP_APS21
{
    public partial class frmAPSQuery : Form
    {
        private UTLoginEx.LoginEx iLoginEx = new LoginEx();
        private string selectSQL = "";
        public frmAPSQuery(UTLoginEx.LoginEx iiLoginEx)
        {
            InitializeComponent();
            iLoginEx = iiLoginEx;
        }

        private void frmQuery_Load(object sender, EventArgs e)
        {
            try
            {
                selectSQL = "";
                dtDateH.Enabled = false;
                dtDateL.Enabled = false;
                chkDate.Checked = true;

                DateTime dt = DateTime.Now;
                DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末//

                dtDateL.Value = startMonth;
                dtDateH.Value = endMonth;
                dtDateL.Value = iLoginEx.ReadUserProfileValue("APS21Query", "dtDateL").Length == 0 ? startMonth : Convert.ToDateTime(iLoginEx.ReadUserProfileValue("APS21Query", "dtDateL"));
                dtDateH.Value = iLoginEx.ReadUserProfileValue("APS21Query", "dtDateH").Length == 0 ? startMonth : Convert.ToDateTime(iLoginEx.ReadUserProfileValue("APS21Query", "dtDateH"));

                chkDate.Checked = iLoginEx.ReadUserProfileValue("APS21Query", "chkDate") == "1" ? true : false;
                chkShowPreparedMO.Checked = iLoginEx.ReadUserProfileValue("APS21Query", "chkShowPreparedMO") == "1" ? true : false;
                chkShowFinishedMO.Checked = iLoginEx.ReadUserProfileValue("APS21Query", "chkShowFinishedMO") == "1" ? true : false;
                chkNoShowColor.Checked = iLoginEx.ReadUserProfileValue("APS21Query", "chkNoShowColor") == "1" ? true : false;
                chkSkipFinish.Checked = iLoginEx.ReadUserProfileValue("APS21Query", "chkSkipFinish") == "1" ? true : false;
                chkALLMaking.Checked = iLoginEx.ReadUserProfileValue("APS21Query", "chkALLMaking") == "1" ? true : false;
                txtcPersonCode.Text = iLoginEx.ReadUserProfileValue("APS21Query", "txtcPersonCode");
                txtcInvStd.Text = iLoginEx.ReadUserProfileValue("APS21Query", "txtcInvStd");
                txtcInvCode.Text = iLoginEx.ReadUserProfileValue("APS21Query", "txtcInvCode");
                txtMoCode.Text = iLoginEx.ReadUserProfileValue("APS21Query", "txtMoCode");
                txtcSOCode.Text = iLoginEx.ReadUserProfileValue("APS21Query", "txtcSOCode");


                if (chkALLMaking.Checked)
                {
                    chkSkipFinish.Enabled = false;
                    txtcInvCode.Enabled = false;
                    txtcInvStd.Enabled = false;
                    txtcSOCode.Enabled = false;
                    txtcPersonCode.Enabled = false;
                    txtMoCode.Enabled = false;
                    chkDate.Enabled = false;
                    dtDateH.Enabled = false;
                    dtDateL.Enabled = false;


                }
                else
                {
                    chkSkipFinish.Enabled = true;
                    txtcInvCode.Enabled = true;
                    txtcInvStd.Enabled = true;
                    txtcSOCode.Enabled = true;
                    txtcPersonCode.Enabled = true;
                    txtMoCode.Enabled = true;
                    chkDate.Enabled = true;
                    if (chkDate.Checked)
                    {
                        dtDateH.Enabled = true;
                        dtDateL.Enabled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "frmQuery_Load()");
                frmmsg.ShowDialog(this);
            }
        }
        public string GetSelectSQL()
        {
            return selectSQL;
        }

        private void btnQurey_Click(object sender, EventArgs e)
        {


            iLoginEx.WriteUserProfileValue("APS21Query", "dtDateL", dtDateL.Value.ToString("yyyy-MM-dd"));
            iLoginEx.WriteUserProfileValue("APS21Query", "dtDateH", dtDateH.Value.ToString("yyyy-MM-dd"));
            iLoginEx.WriteUserProfileValue("APS21Query", "chkDate", chkDate.Checked ? "1" : "0");
            iLoginEx.WriteUserProfileValue("APS21Query", "chkShowPreparedMO", chkShowPreparedMO.Checked ? "1" : "0");
            iLoginEx.WriteUserProfileValue("APS21Query", "chkShowFinishedMO", chkShowFinishedMO.Checked ? "1" : "0");
            iLoginEx.WriteUserProfileValue("APS21Query", "chkNoShowColor", chkNoShowColor.Checked ? "1" : "0");
            iLoginEx.WriteUserProfileValue("APS21Query", "chkSkipFinish", chkSkipFinish.Checked ? "1" : "0");
            iLoginEx.WriteUserProfileValue("APS21Query", "txtcPersonCode", txtcPersonCode.Text);
            iLoginEx.WriteUserProfileValue("APS21Query", "txtcInvStd", txtcInvStd.Text);
            iLoginEx.WriteUserProfileValue("APS21Query", "txtcInvCode", txtcInvCode.Text);
            iLoginEx.WriteUserProfileValue("APS21Query", "txtMoCode", txtMoCode.Text);
            iLoginEx.WriteUserProfileValue("APS21Query", "txtcSOCode", txtcSOCode.Text);
            iLoginEx.WriteUserProfileValue("APS21Query", "chkALLMaking", chkALLMaking.Checked ? "1" : "0");
            try
            {
                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();
                OleDbCommand myCommand = new OleDbCommand("", myConn);


                string cinvCodeChild = "";
                string[] paraCinvCode = null;
                selectSQL = " if exists (select 1  from tempdb.dbo.sysobjects (nolock) where upper(name) = upper('APS21_moQurey" + iLoginEx.GetMacAddress().Replace(":", "") + "') and type='U')   \r\n";
                selectSQL += " drop table tempdb..APS21_moQurey" + iLoginEx.GetMacAddress().Replace(":", "") + " ;   \r\n";
                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();

                selectSQL = " create table tempdb..APS21_moQurey" + iLoginEx.GetMacAddress().Replace(":", "") + "(  \r\n";
                selectSQL += " MoCode nvarchar(30)  null,MoDId int null,SortSeq int null  )   \r\n";
                selectSQL += "   \r\n";
                selectSQL += "   \r\n";
                myCommand.CommandText = selectSQL;
                myCommand.ExecuteNonQuery();


                selectSQL = "select UID=" + iLoginEx.UID().ToString() + ",isnull(ud.ViewSort,0) as 'ViewSort',a.MoClass,d.cDepName as '部门',ud.Priority as '优先级',c.MoCode as'制造单号',a.sortseq as'行号M',a.OrderCode as'销售订单',person.cpersonname as '业务员',  \r\n";
                selectSQL += " a.OrderSeq as'行号S',a.InvCode as '产品编码',i.cInvName as '产品名称',i.cInvStd as '规格型号',convert(bit,case when isnull(a.Status,0)=3 then 1 else 0 end) as'投产',  \r\n";
                selectSQL += " b.StartDate as'计划生产日期',isnull(a.qty,0) as '排产数量',isnull(a.QualifiedInQty,0) as '完工数量',  \r\n";
                selectSQL += "  convert(bit,case when len(isnull(a.CloseUser,''))>0  or Status=4 then 1 else 0 end) as'结案',soa.cDefine31 as 'LOGO' ,soa.cDefine33 as '软件信息',a.Define29 as '备注',调库存=isnull(ud.ReProduce,0),a.MoId,a.ModId  \r\n";
                selectSQL += "   from mom_orderdetail a   (nolock)   \r\n";
                selectSQL += "  left join  mom_morder b  (nolock)on a.MoId=b.MoId  and a.ModId=b.ModId    \r\n";
                selectSQL += "  left join  mom_order c  (nolock) on a.MoId=c.MoId    \r\n";
                selectSQL += "  left join  Department d  (nolock) on d.cDepCode=a.MDeptCode    \r\n";
                selectSQL += "  left join  SO_SODetails soa (nolock) on  a.OrderDId =soa.iSOsID  \r\n";
                selectSQL += "  left join  SO_SOMain sob (nolock) on soa.ID =sob.ID    \r\n";
                selectSQL += "  left join  person  (nolock) on sob.cPersonCode= person.cpersoncode   \r\n";
                selectSQL += "  left join Inventory i (nolock) on a.InvCode=i.cInvCode  \r\n";
                selectSQL += "  left join zhrs_t_mom_orderdetail_userDefine ud (nolock) on ud.MoID=a.MoID and ud.MoDId=a.MoDId  \r\n";
                selectSQL += "  where 1=1   \r\n";
                if (!(chkShowFinishedMO.Checked && chkShowPreparedMO.Checked))
                {
                    if (chkShowFinishedMO.Checked)
                    {
                        selectSQL += "  and  isnull(a.OrderCode,'')<>'' ";
                    }
                    if (chkShowPreparedMO.Checked)
                    {
                        selectSQL += "  and  isnull(a.OrderCode,'')='' ";
                    }
                }
                if (chkALLMaking.Checked)
                {
                    selectSQL += " and  isnull(a.CloseUser,'')='' and (isnull(a.qty,0)-isnull(a.QualifiedInQty,0))>=0   \r\n";
                }
                else
                {
                    if (chkSkipFinish.Checked)
                    {
                        selectSQL += " and  isnull(a.CloseUser,'')='' and (isnull(a.qty,0)-isnull(a.QualifiedInQty,0))>0   \r\n";
                    }

                    //销售订单号
                    txtcSOCode.Text = txtcSOCode.Text.Trim();
                    txtcSOCode.Text = txtcSOCode.Text.Replace("；", ";");

                    if (txtcSOCode.Text.Length > 0)
                    {
                        cinvCodeChild = "";
                        paraCinvCode = txtcSOCode.Text.Split(';');
                        if (paraCinvCode.Length > 0)
                        {
                            for (int i = 0; i < paraCinvCode.Length; i++)
                            {
                                cinvCodeChild += "'" + paraCinvCode[i].ToString() + "',";
                            }

                            selectSQL += " and  a.OrderCode in (" + cinvCodeChild + "'\r\n'" + ") \r\n";
                        }
                        else
                        {
                            selectSQL += " and  a.OrderCode ='" + txtcSOCode.Text + "' \r\n";
                        }
                    }


                    //制造单号
                    string[] paraSortSeq = null;

                    txtMoCode.Text = txtMoCode.Text.Trim();
                    txtMoCode.Text = txtMoCode.Text.Replace("；", ";");
                    txtMoCode.Text = txtMoCode.Text.Replace("，", ",");

                    if (txtMoCode.Text.Length > 0)
                    {
                        paraCinvCode = txtMoCode.Text.Split(';');
                        if (paraCinvCode.Length > 0)
                        {
                            for (int i = 0; i < paraCinvCode.Length; i++)
                            {
                                if (paraCinvCode[i].Length > 0)
                                {
                                    paraSortSeq = paraCinvCode[i].Split(',');
                                    if (paraSortSeq.Length > 1)
                                    {
                                        for (int k = 1; k < paraSortSeq.Length; k++)
                                        {
                                            if (paraSortSeq[k].Length > 0)
                                            {
                                                myCommand.CommandText = " insert into  tempdb..APS21_moQurey" + iLoginEx.GetMacAddress().Replace(":", "") + "(MoCode,SortSeq)values('" + paraSortSeq[0] + "'," + paraSortSeq[k] + ")   \r\n";
                                                myCommand.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        myCommand.CommandText = " insert into  tempdb..APS21_moQurey" + iLoginEx.GetMacAddress().Replace(":", "") + "(MoCode)values('" + paraCinvCode[i].ToString() + "')   \r\n";
                                        myCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            selectSQL += " and  exists (select 1 from tempdb..APS21_moQurey" + iLoginEx.GetMacAddress().Replace(":", "") + " q where q.MoCode=c.MoCode and (q.SortSeq=a.SortSeq or q.SortSeq is null))  \r\n";
                        }
                        else
                        {
                            selectSQL += " and  c.MoCode ='" + txtMoCode.Text + "' \r\n";
                        }
                    }


                    //物料编码
                    txtcInvCode.Text = txtcInvCode.Text.Trim();
                    txtcInvCode.Text = txtcInvCode.Text.Replace("；", ";");

                    if (txtcInvCode.Text.Length > 0)
                    {
                        cinvCodeChild = "";
                        paraCinvCode = txtcInvCode.Text.Split(';');
                        if (paraCinvCode.Length > 0)
                        {
                            for (int i = 0; i < paraCinvCode.Length; i++)
                            {
                                cinvCodeChild += "'" + paraCinvCode[i].ToString() + "',";
                            }

                            selectSQL += " and   a.InvCode in (" + cinvCodeChild + "'\r\n'" + ") \r\n";
                        }
                        else
                        {
                            selectSQL += " and   a.InvCode ='" + txtcInvCode.Text + "' \r\n";
                        }
                    }

                    //规格型号
                    txtcInvStd.Text = txtcInvStd.Text.Trim();
                    txtcInvStd.Text = txtcInvStd.Text.Replace(" ", ";");
                    txtcInvStd.Text = txtcInvStd.Text.Replace("\t", ";");
                    txtcInvStd.Text = txtcInvStd.Text.Replace("\r", ";");
                    txtcInvStd.Text = txtcInvStd.Text.Replace("\n", ";");
                    txtcInvStd.Text = txtcInvStd.Text.Replace("；", ";");
                    while (txtcInvStd.Text.IndexOf(";;") > -1)
                    {
                        txtcInvStd.Text = txtcInvStd.Text.Replace(";;", ";");
                    }

                    if (txtcInvStd.Text.Length > 0)
                    {
                        string mySelectQuery = "select cInvCode  from Inventory (nolock) where ";
                        cinvCodeChild = "";
                        paraCinvCode = txtcInvStd.Text.Split(';');
                        if (paraCinvCode.Length > 0)
                        {
                            for (int i = 0; i < paraCinvCode.Length; i++)
                            {
                                cinvCodeChild += "  cinvstd like '%" + paraCinvCode[i].ToString() + "%'  or ";
                            }

                            cinvCodeChild += iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r";
                            mySelectQuery += cinvCodeChild.Replace("or " + iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r", "");
                        }
                        else
                        {
                            mySelectQuery += " cinvstd like '%" + txtcInvStd.Text + "%'  ";
                        }

                        cinvCodeChild = "";


                        myCommand.CommandText = mySelectQuery;
                        OleDbDataReader myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            cinvCodeChild += "'" + Convert.ToString(myReader["cInvCode"]) + "',";
                        }
                        myReader.Close();
                        myReader.Dispose();
                        cinvCodeChild += iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r";
                        selectSQL += " and  a.InvCode in (" + cinvCodeChild.Replace("," + iLoginEx.Chr(8) + iLoginEx.Chr(8) + "\r\r", "") + ") \r\n";

                    }

                    if (chkDate.Checked)
                    {
                        selectSQL += " and b.StartDate >= N'" + dtDateL.Value.ToString("yyyy-MM-dd") + "' And b.StartDate<= N'" + dtDateH.Value.ToString("yyyy-MM-dd") + "'  \r\n";
                    }
                }





                this.Close();
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "btnQurey_Click()");
                frmmsg.ShowDialog(this);
            }
        }

        private void chkPU_AppVouch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
            {
                dtDateH.Enabled = true;
                dtDateL.Enabled = true;
            }
            else
            {
                dtDateH.Enabled = false;
                dtDateL.Enabled = false;
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectSQL = "";
            this.Close();
        }

        private void txtcInvCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtcInvCode.Text = iLoginEx.OpenSelectWindow("物料", "select cInvCode as '料号',cInvName  as '品名',cInvStd  as '规格' from  inventory  (nolock)", txtcInvCode.Text, 430, 300, 1);
            string[] para = txtcInvCode.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                txtcInvCode.Text = para[0];
                //wName = para[0];  
            }
        }

        private void txtcSOCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtcSOCode.Text = iLoginEx.OpenSelectWindow("销售订单号", "select cSOCode as '销售订单号',dDate as '订单日期' from  SO_SOMain (nolock) where  isnull(cCloser,'')=''", txtcSOCode.Text, 430, 300, 1);
            string[] para = txtcSOCode.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                txtcSOCode.Text = para[0];
                //wName = para[0];  
            }
        }

        private void txtcPersonCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtcPersonCode.Text = iLoginEx.OpenSelectWindow("业务员", "select cPersonCode as '工号',cpersonname as '业务员' from  person  (nolock)", txtcPersonCode.Text, 430, 300, 1);
            string[] para = txtcPersonCode.Text.Split(new string[] { "\r\n\r\n\r\n " }, StringSplitOptions.None);
            if (para.Length > 1)
            {
                txtcPersonCode.Text = para[0];
                //wName = para[0];  
            }
        }

        private void txtcSOCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkSkipFinish_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSkipFinish.Checked)
            {
                iLoginEx.WriteUserProfileValue("APSQuery", "chkSkipFinish", "1");
            }
            else
            {
                iLoginEx.WriteUserProfileValue("APSQuery", "chkSkipFinish", "0");
            }
        }

        private void chkALLMaking_CheckedChanged(object sender, EventArgs e)
        {
            if (chkALLMaking.Checked)
            {
                iLoginEx.WriteUserProfileValue("APSQuery", "chkALLMaking", "1");
                chkSkipFinish.Enabled = false;
                txtcInvCode.Enabled = false;
                txtcInvStd.Enabled = false;
                txtcSOCode.Enabled = false;
                txtcPersonCode.Enabled = false;
                txtMoCode.Enabled = false;
                chkDate.Enabled = false;
                dtDateH.Enabled = false;
                dtDateL.Enabled = false;


            }
            else
            {
                iLoginEx.WriteUserProfileValue("APSQuery", "chkALLMaking", "0");

                chkSkipFinish.Enabled = true;
                txtcInvCode.Enabled = true;
                txtcInvStd.Enabled = true;
                txtcSOCode.Enabled = true;
                txtcPersonCode.Enabled = true;
                txtMoCode.Enabled = true;
                chkDate.Enabled = true;
                dtDateH.Enabled = true;
                dtDateL.Enabled = true;
            }
        }

        private void chkShowFinishedMO_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowFinishedMO.Checked)
            {
                iLoginEx.WriteUserProfileValue("APSQuery", "chkShowFinishedMO", "1");
            }
            else
            {
                iLoginEx.WriteUserProfileValue("APSQuery", "chkShowFinishedMO", "0");
            }
        }

        private void chkShowPreparedMO_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPreparedMO.Checked)
            {
                iLoginEx.WriteUserProfileValue("APSQuery", "chkShowPreparedMO", "1");
            }
            else
            {
                iLoginEx.WriteUserProfileValue("APSQuery", "chkShowPreparedMO", "0");
            }
        }

        private void chkNoShowColor_CheckedChanged(object sender, EventArgs e)
        {
            iLoginEx.WriteUserProfileValue("APSQuery", "chkNoShowColor", chkNoShowColor.Checked ? "1" : "0");
        }

        private void cmTransFomat_Click(object sender, EventArgs e)
        {
            try
            {
                txtMoCode.Text = txtMoCode.Text.Replace("\r", "").Replace("\n", ";").Replace("\t", ",");
            }
            catch (Exception ex)
            {
                frmMessege frmmsg = new frmMessege(ex.ToString(), "cmTransFomat_Click()");
                frmmsg.ShowDialog(this);
            }
        }
    }
}
