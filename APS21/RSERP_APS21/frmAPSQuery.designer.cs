namespace RSERP_APS21
{
    partial class frmAPSQuery
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAPSQuery));
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dtDateH = new System.Windows.Forms.DateTimePicker();
            this.dtDateL = new System.Windows.Forms.DateTimePicker();
            this.txtcPersonCode = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtcSOCode = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtcInvCode = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.btnQurey = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtcInvStd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkSkipFinish = new System.Windows.Forms.CheckBox();
            this.chkALLMaking = new System.Windows.Forms.CheckBox();
            this.chkShowFinishedMO = new System.Windows.Forms.CheckBox();
            this.chkShowPreparedMO = new System.Windows.Forms.CheckBox();
            this.chkNoShowColor = new System.Windows.Forms.CheckBox();
            this.txtMoCode = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmTransFomat = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkDate.Location = new System.Drawing.Point(20, 355);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(116, 19);
            this.chkDate.TabIndex = 6;
            this.chkDate.Text = "计划生产日期";
            this.chkDate.UseVisualStyleBackColor = true;
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkPU_AppVouch_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(276, 357);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 15);
            this.label10.TabIndex = 139;
            this.label10.Text = "至";
            // 
            // dtDateH
            // 
            this.dtDateH.CustomFormat = "yyyy-MM-dd";
            this.dtDateH.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtDateH.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDateH.Location = new System.Drawing.Point(315, 352);
            this.dtDateH.Name = "dtDateH";
            this.dtDateH.Size = new System.Drawing.Size(134, 25);
            this.dtDateH.TabIndex = 8;
            this.dtDateH.Value = new System.DateTime(2013, 10, 11, 0, 0, 0, 0);
            // 
            // dtDateL
            // 
            this.dtDateL.CustomFormat = "yyyy-MM-dd";
            this.dtDateL.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtDateL.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDateL.Location = new System.Drawing.Point(142, 352);
            this.dtDateL.Name = "dtDateL";
            this.dtDateL.Size = new System.Drawing.Size(128, 25);
            this.dtDateL.TabIndex = 7;
            this.dtDateL.Value = new System.DateTime(2013, 10, 11, 0, 0, 0, 0);
            // 
            // txtcPersonCode
            // 
            this.txtcPersonCode.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtcPersonCode.Location = new System.Drawing.Point(90, 252);
            this.txtcPersonCode.Name = "txtcPersonCode";
            this.txtcPersonCode.Size = new System.Drawing.Size(359, 25);
            this.txtcPersonCode.TabIndex = 5;
            this.txtcPersonCode.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtcPersonCode_MouseDoubleClick);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(19, 255);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 15);
            this.label15.TabIndex = 131;
            this.label15.Text = "业务员";
            // 
            // txtcSOCode
            // 
            this.txtcSOCode.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtcSOCode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtcSOCode.Location = new System.Drawing.Point(90, 50);
            this.txtcSOCode.Name = "txtcSOCode";
            this.txtcSOCode.Size = new System.Drawing.Size(359, 25);
            this.txtcSOCode.TabIndex = 1;
            this.txtcSOCode.TextChanged += new System.EventHandler(this.txtcSOCode_TextChanged);
            this.txtcSOCode.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtcSOCode_MouseDoubleClick);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(17, 55);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(67, 15);
            this.label17.TabIndex = 129;
            this.label17.Text = "销售订单";
            // 
            // txtcInvCode
            // 
            this.txtcInvCode.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtcInvCode.Location = new System.Drawing.Point(90, 189);
            this.txtcInvCode.Name = "txtcInvCode";
            this.txtcInvCode.Size = new System.Drawing.Size(359, 25);
            this.txtcInvCode.TabIndex = 4;
            this.txtcInvCode.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtcInvCode_MouseDoubleClick);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(17, 194);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(67, 15);
            this.label19.TabIndex = 127;
            this.label19.Text = "物料编码";
            // 
            // btnQurey
            // 
            this.btnQurey.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQurey.Location = new System.Drawing.Point(111, 395);
            this.btnQurey.Name = "btnQurey";
            this.btnQurey.Size = new System.Drawing.Size(75, 41);
            this.btnQurey.TabIndex = 15;
            this.btnQurey.Text = "查询";
            this.btnQurey.UseVisualStyleBackColor = true;
            this.btnQurey.Click += new System.EventHandler(this.btnQurey_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(272, 395);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 41);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtcInvStd
            // 
            this.txtcInvStd.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtcInvStd.Location = new System.Drawing.Point(90, 220);
            this.txtcInvStd.Name = "txtcInvStd";
            this.txtcInvStd.Size = new System.Drawing.Size(359, 25);
            this.txtcInvStd.TabIndex = 140;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(17, 225);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 141;
            this.label1.Text = "规格型号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(17, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 143;
            this.label2.Text = "制造单";
            // 
            // chkSkipFinish
            // 
            this.chkSkipFinish.AutoSize = true;
            this.chkSkipFinish.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkSkipFinish.Location = new System.Drawing.Point(22, 291);
            this.chkSkipFinish.Name = "chkSkipFinish";
            this.chkSkipFinish.Size = new System.Drawing.Size(176, 19);
            this.chkSkipFinish.TabIndex = 144;
            this.chkSkipFinish.Text = "不显示已结案之制造单";
            this.chkSkipFinish.UseVisualStyleBackColor = true;
            this.chkSkipFinish.CheckedChanged += new System.EventHandler(this.chkSkipFinish_CheckedChanged);
            // 
            // chkALLMaking
            // 
            this.chkALLMaking.AutoSize = true;
            this.chkALLMaking.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkALLMaking.Location = new System.Drawing.Point(22, 12);
            this.chkALLMaking.Name = "chkALLMaking";
            this.chkALLMaking.Size = new System.Drawing.Size(251, 19);
            this.chkALLMaking.TabIndex = 145;
            this.chkALLMaking.Text = "显示所有已投产但未结案的制造单";
            this.chkALLMaking.UseVisualStyleBackColor = true;
            this.chkALLMaking.CheckedChanged += new System.EventHandler(this.chkALLMaking_CheckedChanged);
            // 
            // chkShowFinishedMO
            // 
            this.chkShowFinishedMO.AutoSize = true;
            this.chkShowFinishedMO.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkShowFinishedMO.Location = new System.Drawing.Point(22, 320);
            this.chkShowFinishedMO.Name = "chkShowFinishedMO";
            this.chkShowFinishedMO.Size = new System.Drawing.Size(56, 19);
            this.chkShowFinishedMO.TabIndex = 146;
            this.chkShowFinishedMO.Text = "成品";
            this.chkShowFinishedMO.UseVisualStyleBackColor = true;
            this.chkShowFinishedMO.CheckedChanged += new System.EventHandler(this.chkShowFinishedMO_CheckedChanged);
            // 
            // chkShowPreparedMO
            // 
            this.chkShowPreparedMO.AutoSize = true;
            this.chkShowPreparedMO.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkShowPreparedMO.Location = new System.Drawing.Point(115, 320);
            this.chkShowPreparedMO.Name = "chkShowPreparedMO";
            this.chkShowPreparedMO.Size = new System.Drawing.Size(71, 19);
            this.chkShowPreparedMO.TabIndex = 147;
            this.chkShowPreparedMO.Text = "半成品";
            this.chkShowPreparedMO.UseVisualStyleBackColor = true;
            this.chkShowPreparedMO.CheckedChanged += new System.EventHandler(this.chkShowPreparedMO_CheckedChanged);
            // 
            // chkNoShowColor
            // 
            this.chkNoShowColor.AutoSize = true;
            this.chkNoShowColor.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkNoShowColor.Location = new System.Drawing.Point(224, 291);
            this.chkNoShowColor.Name = "chkNoShowColor";
            this.chkNoShowColor.Size = new System.Drawing.Size(101, 19);
            this.chkNoShowColor.TabIndex = 148;
            this.chkNoShowColor.Text = "不显示颜色";
            this.chkNoShowColor.UseVisualStyleBackColor = true;
            this.chkNoShowColor.CheckedChanged += new System.EventHandler(this.chkNoShowColor_CheckedChanged);
            // 
            // txtMoCode
            // 
            this.txtMoCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMoCode.ContextMenuStrip = this.contextMenuStrip1;
            this.txtMoCode.Location = new System.Drawing.Point(90, 82);
            this.txtMoCode.Name = "txtMoCode";
            this.txtMoCode.Size = new System.Drawing.Size(359, 101);
            this.txtMoCode.TabIndex = 149;
            this.txtMoCode.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmTransFomat});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // cmTransFomat
            // 
            this.cmTransFomat.Name = "cmTransFomat";
            this.cmTransFomat.Size = new System.Drawing.Size(124, 22);
            this.cmTransFomat.Text = "格式转换";
            this.cmTransFomat.Click += new System.EventHandler(this.cmTransFomat_Click);
            // 
            // frmAPSQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 449);
            this.Controls.Add(this.txtMoCode);
            this.Controls.Add(this.chkNoShowColor);
            this.Controls.Add(this.chkShowPreparedMO);
            this.Controls.Add(this.chkShowFinishedMO);
            this.Controls.Add(this.chkALLMaking);
            this.Controls.Add(this.chkSkipFinish);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtcInvStd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnQurey);
            this.Controls.Add(this.chkDate);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dtDateH);
            this.Controls.Add(this.dtDateL);
            this.Controls.Add(this.txtcPersonCode);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtcSOCode);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtcInvCode);
            this.Controls.Add(this.label19);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAPSQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查询";
            this.Load += new System.EventHandler(this.frmQuery_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtDateH;
        private System.Windows.Forms.DateTimePicker dtDateL;
        private System.Windows.Forms.TextBox txtcPersonCode;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtcSOCode;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtcInvCode;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnQurey;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtcInvStd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkSkipFinish;
        private System.Windows.Forms.CheckBox chkALLMaking;
        private System.Windows.Forms.CheckBox chkShowFinishedMO;
        private System.Windows.Forms.CheckBox chkShowPreparedMO;
        private System.Windows.Forms.CheckBox chkNoShowColor;
        private System.Windows.Forms.RichTextBox txtMoCode;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cmTransFomat;
    }
}