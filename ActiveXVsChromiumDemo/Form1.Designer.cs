using System.Windows.Forms;

namespace ActiveXVsChromiumDemo
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxBrowserType;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Panel panelBrowser;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxBrowserType = new System.Windows.Forms.ComboBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.panelBrowser = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // comboBoxBrowserType
            // 
            this.comboBoxBrowserType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBrowserType.Location = new System.Drawing.Point(12, 12);
            this.comboBoxBrowserType.Name = "comboBoxBrowserType";
            this.comboBoxBrowserType.Size = new System.Drawing.Size(120, 20);
            this.comboBoxBrowserType.TabIndex = 3;
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(138, 12);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(400, 22);
            this.txtUrl.TabIndex = 2;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(544, 10);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "載入網址";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // panelBrowser
            // 
            this.panelBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBrowser.Location = new System.Drawing.Point(12, 40);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(760, 500);
            this.panelBrowser.TabIndex = 0;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panelBrowser);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.comboBoxBrowserType);
            this.Name = "Form1";
            this.Text = "ActiveX vs Chromium Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
