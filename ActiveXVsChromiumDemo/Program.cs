using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ActiveXVsChromiumDemo
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            //Stopwatch sw = Stopwatch.StartNew();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
