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
            this.listBoxControl1 = new DevExpress.XtraEditors.ListBoxControl();
            this.label1 = new System.Windows.Forms.Label();
            this.gridConfig = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.CmdAddColumn = new DevExpress.XtraEditors.SimpleButton();
            this.txtPageNumber = new System.Windows.Forms.TextBox();
            this.txtSearchTerms = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxControl1
            // 
            this.listBoxControl1.Location = new System.Drawing.Point(12, 29);
            this.listBoxControl1.Name = "listBoxControl1";
            this.listBoxControl1.Size = new System.Drawing.Size(120, 407);
            this.listBoxControl1.TabIndex = 0;
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
            this.gridConfig.Size = new System.Drawing.Size(502, 327);
            this.gridConfig.TabIndex = 2;
            this.gridConfig.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridConfig;
            this.gridView1.Name = "gridView1";
            // 
            // CmdAddColumn
            // 
            this.CmdAddColumn.Location = new System.Drawing.Point(363, 362);
            this.CmdAddColumn.Name = "CmdAddColumn";
            this.CmdAddColumn.Size = new System.Drawing.Size(75, 23);
            this.CmdAddColumn.TabIndex = 3;
            this.CmdAddColumn.Text = "simpleButton1";
            this.CmdAddColumn.Click += new System.EventHandler(this.CmdAddColumn_Click);
            // 
            // txtPageNumber
            // 
            this.txtPageNumber.Location = new System.Drawing.Point(230, 393);
            this.txtPageNumber.Name = "txtPageNumber";
            this.txtPageNumber.Size = new System.Drawing.Size(100, 20);
            this.txtPageNumber.TabIndex = 4;
            // 
            // txtSearchTerms
            // 
            this.txtSearchTerms.Location = new System.Drawing.Point(230, 362);
            this.txtSearchTerms.Name = "txtSearchTerms";
            this.txtSearchTerms.Size = new System.Drawing.Size(100, 20);
            this.txtSearchTerms.TabIndex = 5;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 448);
            this.Controls.Add(this.txtSearchTerms);
            this.Controls.Add(this.txtPageNumber);
            this.Controls.Add(this.CmdAddColumn);
            this.Controls.Add(this.gridConfig);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxControl1);
            this.Name = "ConfigForm";
            this.Text = "ConfigForm";
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl listBoxControl1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gridConfig;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton CmdAddColumn;
        private System.Windows.Forms.TextBox txtPageNumber;
        private System.Windows.Forms.TextBox txtSearchTerms;
    }
}