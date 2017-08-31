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
    public partial class frmDate : Form
    {
        public frmDate()
        {
            InitializeComponent();
        }
        public string  GetDate()
        {
            return dtDateL.Value.ToString("yyyy-MM-dd") ;
        }
        private void frmDate_Load(object sender, EventArgs e)
        {
            dtDateL.Value = DateTime.Now; 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
