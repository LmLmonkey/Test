using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoScrew
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool bExist;
            Mutex MyMutex = new Mutex(true, "OnlyRunOncetime", out bExist);
            if (bExist)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
                MyMutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("程序已打开！", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
