namespace PDFScanAndSort
{
    partial class ConfigForm
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
            this.lstBoxApplications = new DevExpress.XtraEditors.ListBoxControl();
            this.label1 = new System.Windows.Forms.Label();
            this.gridConfig = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtPageNumber = new System.Windows.Forms.TextBox();
            this.txtSearchTerms = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtApplication = new System.Windows.Forms.TextBox();
            this.CmdAddColumn = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.lstBoxApplications)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lstBoxApplications
            // 
            this.lstBoxApplications.Location = new System.Drawing.Point(12, 29);
            this.lstBoxApplications.Name = "lstBoxApplications";
            this.lstBoxApplications.Size = new System.Drawing.Size(120, 407);
            this.lstBoxApplications.TabIndex = 0;
            this.lstBoxApplications.SelectedValueChanged += new System.EventHandler(this.lstBoxApplications_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Applications";
            // 
            // gridConfig
            // 
            this.gridConfig.Location = new System.Drawing.Point(138, 29);
            this.gridConfig.MainView = this.gridView1;
            this.gridConfig.Name = "gridConfig";
            this.gridConfig.Size = new System.Drawing.Size(502, 352);
            this.gridConfig.TabIndex = 2;
            this.gridConfig.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridConfig;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            // 
            // txtPageNumber
            // 
            this.txtPageNumber.Location = new System.Drawing.Point(571, 387);
            this.txtPageNumber.Name = "txtPageNumber";
            this.txtPageNumber.Size = new System.Drawing.Size(69, 20);
            this.txtPageNumber.TabIndex = 4;
            // 
            // txtSearchTerms
            // 
            this.txtSearchTerms.Location = new System.Drawing.Point(387, 387);
            this.txtSearchTerms.Name = "txtSearchTerms";
            this.txtSearchTerms.Size = new System.Drawing.Size(100, 20);
            this.txtSearchTerms.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(493, 390);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Page Number";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(308, 392);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Search Terms";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 392);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Application";
            // 
            // txtApplication
            // 
            this.txtApplication.Location = new System.Drawing.Point(203, 389);
            this.txtApplication.Name = "txtApplication";
            this.txtApplication.Size = new System.Drawing.Size(100, 20);
            this.txtApplication.TabIndex = 8;
            // 
            // CmdAddColumn
            // 
            this.CmdAddColumn.Location = new System.Drawing.Point(571, 413);
            this.CmdAddColumn.Name = "CmdAddColumn";
            this.CmdAddColumn.Size = new System.Drawing.Size(69, 23);
            this.CmdAddColumn.TabIndex = 3;
            this.CmdAddColumn.Text = "Add Record";
            this.CmdAddColumn.Click += new System.EventHandler(this.CmdAddColumn_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 448);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtApplication);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSearchTerms);
            this.Controls.Add(this.txtPageNumber);
            this.Controls.Add(this.CmdAddColumn);
            this.Controls.Add(this.gridConfig);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstBoxApplications);
            this.Name = "ConfigForm";
            this.Text = "ConfigForm";
            ((System.ComponentModel.ISupportInitialize)(this.lstBoxApplications)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl lstBoxApplications;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gridConfig;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.TextBox txtPageNumber;
        private System.Windows.Forms.TextBox txtSearchTerms;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtApplication;
        private DevExpress.XtraEditors.SimpleButton CmdAddColumn;
    }
}