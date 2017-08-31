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
    public partial class frmColorPanel : Form
    {
        private string mColor="White";
        public frmColorPanel()
        {
            InitializeComponent();
        }
        public string GetColorByHTML()
        {
            return mColor;
        }

        private void cSilver_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cSilver.BackColor);
            this.Close();  
        }

        private void cLightGray_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cLightGray.BackColor);
            this.Close();
        }

        private void cRed_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cRed.BackColor);
            this.Close();
        }

        private void cLightCoral_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cLightCoral.BackColor);
            this.Close();
        }

        private void cLightSalmon_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cLightSalmon.BackColor);
            this.Close();
        }

        private void cSandyBrown_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cSandyBrown.BackColor);
            this.Close();
        }

        private void cDarkOrange_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cDarkOrange.BackColor);
            this.Close();
        }

        private void cBurlyWood_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cBurlyWood.BackColor);
            this.Close();
        }

        private void cWhite_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cWhite.BackColor);
            this.Close();
        }

        private void cPaleGoldenrod_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cPaleGoldenrod.BackColor);
            this.Close();
        }

        private void cDarkKhaki_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cDarkKhaki.BackColor);
            this.Close();
        }

        private void cYellowGreen_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cYellowGreen.BackColor);
            this.Close();
        }

        private void cDarkSeaGreen_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cDarkSeaGreen.BackColor);
            this.Close();
        }

        private void cLightGreen_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cLightGreen.BackColor);
            this.Close();
        }

        private void cMediumSeaGreen_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cMediumSeaGreen.BackColor);
            this.Close();
        }

        private void cMediumSpringGreen_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cMediumSpringGreen.BackColor);
            this.Close();
        }

        private void cMediumAquamarine_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cMediumAquamarine.BackColor);
            this.Close();
        }

        private void cAquamarine_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cAquamarine.BackColor);
            this.Close();
        }

        private void cLightSeaGreen_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cLightSeaGreen.BackColor);
            this.Close();
        }

        private void cPaleTurquoise_Click(object sender, EventArgs e)
        {
            mColor = ColorTranslator.ToHtml(cPaleTurquoise.BackColor);
            this.Close();
        }

        
    }
}
