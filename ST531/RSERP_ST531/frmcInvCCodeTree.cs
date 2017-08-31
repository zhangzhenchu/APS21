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
namespace RSERP_ST531
{
    public partial class frmcInvCCodeTree : Form
    {
        private TreeNode theLastNode = null;//最后选择的节点（用于还原节点状态）
        private UTLoginEx.LoginEx iLoginEx = new LoginEx();
        public string cInvCCode = "";
        public frmcInvCCodeTree(LoginEx iiLoginEx)
        {
            InitializeComponent();
            iLoginEx = iiLoginEx;
        }

        private void frmcInvCCodeTree_Load(object sender, EventArgs e)
        {
            LoadcInvCCodeList();
        }

        /// <summary>
        /// 存货分类
        /// </summary>
        private void LoadcInvCCodeList()
        {
            try
            {
                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());
                string mySelectQuery = "";
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                mySelectQuery = "select cInvCCode ,cInvCName,iInvCGrade from InventoryClass (nolock) where iInvCGrade=1 order by cInvCCode";
                OleDbCommand myCommand = new OleDbCommand(mySelectQuery, myConn);
                OleDbDataReader myReader = myCommand.ExecuteReader();
                treeView1.Nodes.Clear();
                while (myReader.Read())
                {
                    treeView1.Nodes.Add(myReader["cInvCCode"].ToString(), myReader["cInvCCode"].ToString() + " " + myReader["cInvCName"].ToString());
                    foreach (TreeNode node1 in treeView1.Nodes)
                    {
                        NodeUpdate(node1);
                        //  ExpandAllChildNode(node1);
                    }
                    // treeView1.ExpandAll();
                }
                myReader.Close();
                myReader.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "LoadcInvCCodeList()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GC.Collect();
                GC.Collect(1);
            }
        }

        //更新结点(列出当前目录下的子目录)
        private void NodeUpdate(TreeNode node)
        {
            try
            {
                node.Nodes.Clear();
                string NewPath = string.Empty;
                //string NewcBOMID = "-1";
                //string PandSpaceChar = " ";

                NewPath = node.FullPath;

                int adr = 0;
                adr = NewPath.LastIndexOf("\\");
                if (adr > 1)
                {
                    NewPath = NewPath.Substring(adr + 1);
                }

                OleDbConnection myConn = new OleDbConnection(iLoginEx.ConnString());
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn.Open();

                string mySelectQuery = "select cInvCCode ,cInvCName,iInvCGrade from InventoryClass (nolock) where iInvCGrade>1 and left(cInvCCode," + node.Name.Length.ToString() + ") ='" + node.Name + "' and cInvCCode<>'" + node.Name + "' order by cInvCCode ";
                OleDbCommand myCommand = new OleDbCommand(mySelectQuery, myConn);
                OleDbDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    node.Nodes.Add(myReader["cInvCCode"].ToString(), myReader["cInvCCode"].ToString() + " " + myReader["cInvCName"].ToString());
                }
                myReader.Close();
                myReader.Dispose();
                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    myConn.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "NodeUpdate()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GC.Collect();
                GC.Collect(1);
            }
        }

        /// <summary>
        /// 递归遍历查找一个树节点的方法
        /// </summary>
        /// <param name="imageIndex"></param>
        /// <param name="treeView"></param>
        /// <returns></returns>
        private TreeNode CallFindNode(string icurrentPath, TreeView treeView, ref bool rErr)
        {
            try
            {
                TreeNodeCollection nodes = treeView.Nodes;
                foreach (TreeNode n in nodes)
                {
                    TreeNode temp = FindTreeNode(icurrentPath, n, ref rErr);
                    if (temp != null)
                        // treeView.SelectedNode = temp;
                        return temp;
                }
                return null;
            }
            catch
            {
                rErr = true;
                return null;
            }
            finally
            {
                GC.Collect();
                GC.Collect(1);
            }
        }
        private TreeNode FindTreeNode(string icurrentPath, TreeNode tnParent, ref bool rErr)
        {
            try
            {
                string newPath = string.Empty;
                if (tnParent == null)
                    return null;

                newPath = tnParent.FullPath;

                if (newPath == icurrentPath)
                    return tnParent;
                TreeNode tnRet = null;
                foreach (TreeNode tn in tnParent.Nodes)
                {
                    tnRet = FindTreeNode(icurrentPath, tn, ref rErr);
                    if (tnRet != null)
                        break;
                }
                return tnRet;
            }
            catch
            {
                rErr = true;
                return null;
            }
            finally
            {
                GC.Collect();
                GC.Collect(1);
            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {

                if (this.treeView1.SelectedNode != null)
                {
                    theLastNode = treeView1.SelectedNode;
                }

                if (e.Node == null)
                    return;

                e.Node.Expand();

                AfterSelectAct(e.Node);

            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "treeView1_AfterSelect()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AfterSelectAct(TreeNode tn)
        {
            try
            {
                if (tn == null)
                    return;


                string newPath = tn.FullPath;
                int adr = 0;
                adr = newPath.LastIndexOf("\\");
                if (adr > 1)
                {
                    newPath = newPath.Substring(adr + 1);
                }
                //CurrentAuthID = Convert.ToInt32(tn.Name);
                //CurrentMenuText = newPath;
                cInvCCode = newPath;
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "AfterSelectAct()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (e.Node == null)
                    return;

                NodeUpdate(e.Node); //更新当前结点
                foreach (TreeNode node in e.Node.Nodes) //更新所有子结点
                {
                    NodeUpdate(node);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.ToString(), "treeView1_BeforeExpand()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            AfterSelectAct(e.Node);
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.Close();
        }
    }
}
