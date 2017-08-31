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
    public partial class frmMessege : Form
    {
        string mmsg = "", mTitle="";
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="imsg">内容</param>
        /// <param name="iTitle">标题</param>
        public frmMessege(string imsg,string iTitle)
        {
            InitializeComponent();
            mmsg = imsg;
            mTitle = iTitle;
        }

        private void frmMessege_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            richTextBox1.Text = mmsg;
            this.Text = mTitle;
        }
    }
}
