namespace RSERP_ST531
{
    partial class frmQuery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuery));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOutMonths = new System.Windows.Forms.TextBox();
            this.txtPercent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInMonths = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtOutMonths2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkInMonths = new System.Windows.Forms.CheckBox();
            this.chkInMonths2 = new System.Windows.Forms.CheckBox();
            this.txtInMonths2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkComprehensiveStock = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkOutMonths2 = new System.Windows.Forms.RadioButton();
            this.chkOutMonths = new System.Windows.Forms.RadioButton();
            this.txtWhCode = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(127, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "月内，出库数量低于库存的";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(54, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "最近";
            // 
            // txtOutMonths
            // 
            this.txtOutMonths.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOutMonths.Location = new System.Drawing.Point(95, 72);
            this.txtOutMonths.Name = "txtOutMonths";
            this.txtOutMonths.Size = new System.Drawing.Size(31, 25);
            this.txtOutMonths.TabIndex = 2;
            // 
            // txtPercent
            // 
            this.txtPercent.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPercent.Location = new System.Drawing.Point(322, 72);
            this.txtPercent.Name = "txtPercent";
            this.txtPercent.Size = new System.Drawing.Size(31, 25);
            this.txtPercent.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(359, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "%";
            // 
            // txtInMonths
            // 
            this.txtInMonths.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInMonths.Location = new System.Drawing.Point(95, 102);
            this.txtInMonths.Name = "txtInMonths";
            this.txtInMonths.Size = new System.Drawing.Size(31, 25);
            this.txtInMonths.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(54, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "最近";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(127, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(202, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "月内，无采购入库记录的物料";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(110, 264);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 39);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtOutMonths2
            // 
            this.txtOutMonths2.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOutMonths2.Location = new System.Drawing.Point(95, 43);
            this.txtOutMonths2.Name = "txtOutMonths2";
            this.txtOutMonths2.Size = new System.Drawing.Size(31, 25);
            this.txtOutMonths2.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(54, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 15);
            this.label7.TabIndex = 10;
            this.label7.Text = "最近";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(127, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(142, 15);
            this.label8.TabIndex = 9;
            this.label8.Text = "月内，不使用的物料";
            // 
            // chkInMonths
            // 
            this.chkInMonths.AutoSize = true;
            this.chkInMonths.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkInMonths.Location = new System.Drawing.Point(33, 105);
            this.chkInMonths.Name = "chkInMonths";
            this.chkInMonths.Size = new System.Drawing.Size(15, 14);
            this.chkInMonths.TabIndex = 14;
            this.chkInMonths.UseVisualStyleBackColor = true;
            this.chkInMonths.CheckedChanged += new System.EventHandler(this.chkInMonths_CheckedChanged);
            // 
            // chkInMonths2
            // 
            this.chkInMonths2.AutoSize = true;
            this.chkInMonths2.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkInMonths2.Location = new System.Drawing.Point(33, 135);
            this.chkInMonths2.Name = "chkInMonths2";
            this.chkInMonths2.Size = new System.Drawing.Size(15, 14);
            this.chkInMonths2.TabIndex = 18;
            this.chkInMonths2.UseVisualStyleBackColor = true;
            this.chkInMonths2.CheckedChanged += new System.EventHandler(this.chkInMonths2_CheckedChanged);
            // 
            // txtInMonths2
            // 
            this.txtInMonths2.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInMonths2.Location = new System.Drawing.Point(95, 132);
            this.txtInMonths2.Name = "txtInMonths2";
            this.txtInMonths2.Size = new System.Drawing.Size(31, 25);
            this.txtInMonths2.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(54, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 16;
            this.label4.Text = "最近";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(127, 135);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(202, 15);
            this.label9.TabIndex = 15;
            this.label9.Text = "月内，有采购入库记录的物料";
            // 
            // chkComprehensiveStock
            // 
            this.chkComprehensiveStock.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkComprehensiveStock.Location = new System.Drawing.Point(33, 163);
            this.chkComprehensiveStock.Name = "chkComprehensiveStock";
            this.chkComprehensiveStock.Size = new System.Drawing.Size(359, 64);
            this.chkComprehensiveStock.TabIndex = 19;
            this.chkComprehensiveStock.Text = "显示已分配量、采购在途、在检、已分配量等信息（该选项，由于数据运算量大，需几十秒钟的等待时间）";
            this.chkComprehensiveStock.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(31, 230);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(257, 12);
            this.label10.TabIndex = 20;
            this.label10.Text = "已分配量：指的是某种物料有哪些单正在使用中";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(223, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 39);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkOutMonths2
            // 
            this.chkOutMonths2.AutoSize = true;
            this.chkOutMonths2.Location = new System.Drawing.Point(33, 46);
            this.chkOutMonths2.Name = "chkOutMonths2";
            this.chkOutMonths2.Size = new System.Drawing.Size(14, 13);
            this.chkOutMonths2.TabIndex = 22;
            this.chkOutMonths2.TabStop = true;
            this.chkOutMonths2.UseVisualStyleBackColor = true;
            this.chkOutMonths2.CheckedChanged += new System.EventHandler(this.chkOutMonths2_CheckedChanged);
            // 
            // chkOutMonths
            // 
            this.chkOutMonths.AutoSize = true;
            this.chkOutMonths.Location = new System.Drawing.Point(33, 72);
            this.chkOutMonths.Name = "chkOutMonths";
            this.chkOutMonths.Size = new System.Drawing.Size(14, 13);
            this.chkOutMonths.TabIndex = 23;
            this.chkOutMonths.TabStop = true;
            this.chkOutMonths.UseVisualStyleBackColor = true;
            this.chkOutMonths.CheckedChanged += new System.EventHandler(this.chkOutMonths_CheckedChanged);
            // 
            // txtWhCode
            // 
            this.txtWhCode.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWhCode.Location = new System.Drawing.Point(101, 7);
            this.txtWhCode.Name = "txtWhCode";
            this.txtWhCode.Size = new System.Drawing.Size(273, 25);
            this.txtWhCode.TabIndex = 24;
            this.txtWhCode.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtWhCode_MouseDoubleClick);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(28, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 15);
            this.label11.TabIndex = 25;
            this.label11.Text = "仓库代码";
            // 
            // frmQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 324);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtWhCode);
            this.Controls.Add(this.chkOutMonths);
            this.Controls.Add(this.chkOutMonths2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.chkComprehensiveStock);
            this.Controls.Add(this.chkInMonths2);
            this.Controls.Add(this.txtInMonths2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkInMonths);
            this.Controls.Add(this.txtOutMonths2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtInMonths);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPercent);
            this.Controls.Add(this.txtOutMonths);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查询";
            this.Load += new System.EventHandler(this.frmQuery_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmQuery_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOutMonths;
        private System.Windows.Forms.TextBox txtPercent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInMonths;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtOutMonths2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkInMonths;
        private System.Windows.Forms.CheckBox chkInMonths2;
        private System.Windows.Forms.TextBox txtInMonths2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkComprehensiveStock;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton chkOutMonths2;
        private System.Windows.Forms.RadioButton chkOutMonths;
        private System.Windows.Forms.TextBox txtWhCode;
        private System.Windows.Forms.Label label11;
    }
}