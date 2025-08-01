using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace ActiveXVsChromiumDemo
{
    public partial class Form1 : Form
    {
        private WebView2 webView2Browser;
        private WebBrowser activeXBrowser;
        private bool webView2Ready = false;
        private Label lblStatus;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            comboBoxBrowserType.Items.Clear();
            comboBoxBrowserType.Items.AddRange(new string[] { "ActiveX IE", "WebView2" });
            comboBoxBrowserType.SelectedIndex = 0;

            activeXBrowser = new WebBrowser { Dock = DockStyle.Fill };
            panelBrowser.Controls.Add(activeXBrowser);

            lblStatus = new Label
            {
                Text = "準備就緒",
                AutoSize = true,
                Location = new System.Drawing.Point(12, 550)
            };
            this.Controls.Add(lblStatus);
        }

        // 1. 改為回傳 Task
        private async Task InitWebView2()
        {
            if (webView2Ready) return;

            var sw = Stopwatch.StartNew();
            lblStatus.Text = "WebView2 初始化中...";
            Application.DoEvents();

            webView2Browser = new WebView2
            {
                Dock = DockStyle.Fill,
                Visible = false
            };
            panelBrowser.Controls.Add(webView2Browser);

            await webView2Browser.EnsureCoreWebView2Async();

            webView2Ready = true;
            sw.Stop();
            lblStatus.Text = $"WebView2 完成！({sw.ElapsedMilliseconds}ms)";
        }

        // 2. 直接 await InitWebView2()
        private async void btnLoad_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url)) url = "https://www.google.com";
            if (!url.StartsWith("http")) url = "https://" + url;

            if (comboBoxBrowserType.SelectedItem.ToString() == "ActiveX IE")
            {
                activeXBrowser.Visible = true;
                activeXBrowser.Navigate(url);
            }
            else
            {
                if (!webView2Ready)
                    await InitWebView2();
                activeXBrowser.Visible = false;
                webView2Browser.Visible = true;
                webView2Browser.CoreWebView2.Navigate(url);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            webView2Browser?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
