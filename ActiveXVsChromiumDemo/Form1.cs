using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace ActiveXVsChromiumDemo
{
    public partial class Form1 : Form
    {
        private ChromiumWebBrowser chromiumBrowser;
        private WebBrowser activeXBrowser;
        private bool chromiumInitialized = false;
        private Label lblStatus;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // ComboBox 設定
            comboBoxBrowserType.Items.AddRange(new string[] { "ActiveX IE", "Chromium" });
            comboBoxBrowserType.SelectedIndex = 0;

            // WebBrowser (IE)
            activeXBrowser = new WebBrowser();
            activeXBrowser.Dock = DockStyle.Fill;
            panelBrowser.Controls.Add(activeXBrowser);

            lblStatus = new Label();
            lblStatus.Text = "準備就緒";
            lblStatus.AutoSize = true;
            lblStatus.Location = new System.Drawing.Point(12, 550); // 依實際 Form 調整位置
            this.Controls.Add(lblStatus);
        }

        private async Task InitializeChromiumIfNeeded()
        {
            if (chromiumInitialized) return;

            await Task.Run(() =>
            {
                var settings = new CefSettings();

                // ✅ 1. 設定 Cache 路徑
                settings.CachePath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "ActiveXVsChromiumDemo", "CefCache");

                // ✅ 2. 初始化
                Cef.Initialize(settings);
            });

            // ✅ 3. 預載 example.com 暖機
            chromiumBrowser = new ChromiumWebBrowser("https://example.com");
            chromiumBrowser.Dock = DockStyle.Fill;

            // ✅ 4. 加入 LoadingStateChanged 事件
            chromiumBrowser.LoadingStateChanged += (s, args) =>
            {
                this.Invoke((Action)(() =>
                {
                    lblStatus.Text = args.IsLoading ? "載入中..." : "完成";
                }));
            };

            panelBrowser.Invoke((MethodInvoker)(() =>
            {
                panelBrowser.Controls.Add(chromiumBrowser);
                chromiumBrowser.Visible = false;
            }));

            chromiumInitialized = true;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("請輸入網址");
                return;
            }

            string selected = comboBoxBrowserType.SelectedItem.ToString();

            if (selected == "ActiveX IE")
            {
                activeXBrowser.Visible = true;
                if (chromiumBrowser != null)
                    chromiumBrowser.Visible = false;

                activeXBrowser.Navigate(url);
                lblStatus.Text = "載入中..."; // 顯示狀態
            }
            else if (selected == "Chromium")
            {
                await InitializeChromiumIfNeeded();

                activeXBrowser.Visible = false;
                chromiumBrowser.Visible = true;
                chromiumBrowser.Load(url);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (chromiumInitialized)
            {
                Cef.Shutdown(); // 很重要，避免 zombie process！
            }
            base.OnFormClosing(e);
        }
    }
}