using System;
using System.Diagnostics;
using System.Drawing;
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

        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblFocusInfo;
        private ToolStripStatusLabel lblStatusInfo;
        private ToolStripStatusLabel lblTimeInfo;
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {

            // 時間更新 Timer
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) =>
            {
                lblTimeInfo.Text = $"時間：{DateTime.Now:HH:mm:ss}";
            };
            timer.Start();

            // 初始化瀏覽器選擇
            comboBoxBrowserType.Items.Clear();
            comboBoxBrowserType.Items.AddRange(new string[] { "ActiveX IE", "WebView2" });
            comboBoxBrowserType.SelectedIndex = 0;

            // 網址列事件
            txtUrl.KeyDown += TxtUrl_KeyDown;
            txtUrl.Enter += (s, e) => lblFocusInfo.Text = "txtUrl";

            // 載入按鈕事件
            btnLoad.Click += btnLoad_Click;
            btnLoad.Enter += (s, e) => lblFocusInfo.Text = "btnLoad";

            // 狀態列初始化（先建立，避免引用錯誤）
            statusStrip = new StatusStrip();
            lblFocusInfo = new ToolStripStatusLabel("");
            lblStatusInfo = new ToolStripStatusLabel("準備就緒");
            lblTimeInfo = new ToolStripStatusLabel();

            statusStrip.Items.Add(lblFocusInfo);
            statusStrip.Items.Add(new ToolStripStatusLabel { Spring = true });
            statusStrip.Items.Add(lblStatusInfo);
            statusStrip.Items.Add(new ToolStripStatusLabel { Spring = true });
            statusStrip.Items.Add(lblTimeInfo);
            this.Controls.Add(statusStrip);

            // ActiveX 初始化（放在狀態列之後）
            activeXBrowser = new WebBrowser { Dock = DockStyle.Fill };
            //activeXBrowser.Enter += (s, e) => lblFocusInfo.Text = "焦點：ActiveX";
            activeXBrowser.GotFocus += (s, e) => lblFocusInfo.Text = "ActiveX";
            panelBrowser.Controls.Add(activeXBrowser);
        }

        private void TxtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLoad.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private async Task InitWebView2()
        {
            if (webView2Ready) return;

            lblStatusInfo.Text = "WebView2 初始化中...";
            Application.DoEvents();

            webView2Browser = new WebView2
            {
                Dock = DockStyle.Fill,
                Visible = false
            };

            webView2Browser.CoreWebView2InitializationCompleted += WebView2Browser_CoreWebView2InitializationCompleted;
            panelBrowser.Controls.Add(webView2Browser);

            await webView2Browser.EnsureCoreWebView2Async();
        }

        private void WebView2Browser_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                webView2Ready = true;
                lblStatusInfo.Text = "WebView2 初始化完成";
            }
            else
            {
                lblStatusInfo.Text = $"WebView2 初始化失敗：{e.InitializationException.Message}";
            }
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url)) url = "https://www.google.com";
            if (!url.StartsWith("http")) url = "https://" + url;

            try
            {
                if (comboBoxBrowserType.SelectedItem.ToString() == "ActiveX IE")
                {
                    activeXBrowser.Visible = true;
                    activeXBrowser.Navigate(url);
                    activeXBrowser.Focus();
                    lblStatusInfo.Text = $"已載入 {url}";
                }
                else
                {
                    if (!webView2Ready)
                        await InitWebView2();

                    activeXBrowser.Visible = false;
                    webView2Browser.Visible = true;
                    if (!webView2Ready || webView2Browser.CoreWebView2 == null)
                    {
                        lblStatusInfo.Text = "WebView2 尚未初始化完成";
                        return;
                    }
                    webView2Browser.CoreWebView2.Navigate(url);
                    webView2Browser.Focus();
                    lblStatusInfo.Text = $"已載入 {url}";
                }
            }
            catch (Exception ex)
            {
                lblStatusInfo.Text = $"錯誤：{ex.Message}";
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            webView2Browser?.Dispose();
            base.OnFormClosing(e);
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            // 可選：更新狀態列提示
        }
    }
}
