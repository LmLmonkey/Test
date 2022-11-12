using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoScrew
{
    public partial class ResetProcess : Form
    {
        public ResetProcess()
        {
            string str = "";
            //if (GlobalVar.Language == 1)
            //{
            //    GlobalVar.English = true;
            //    GlobalVar.chinese = false;
            //}
            //else
            //{
            //    GlobalVar.chinese = true;
            //    GlobalVar.English = false;
            //}
            //if (GlobalVar.English)
            //{
            //    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            //}
            //else
            //{
            //    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
            //}
            InitializeComponent();
        }
        #region 窗体圆角的实现
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                SetWindowRegion();
            }
            else
            {
                this.Region = null;
            }
        }

        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            FormPath = GetRoundedRectPath(rect, 50);
            this.Region = new Region(FormPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect">窗体大小</param>
        /// <param name="radius">圆角大小</param>
        /// <returns></returns>
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            path.AddArc(arcRect, 180, 90);//左上角

            arcRect.X = rect.Right - diameter;//右上角
            path.AddArc(arcRect, 270, 90);

            arcRect.Y = rect.Bottom - diameter;// 右下角
            path.AddArc(arcRect, 0, 90);

            arcRect.X = rect.Left;// 左下角
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }
        #endregion
        public void SetProcessText(string Chinese, string English)
        {
            //set
            {
                Invoke(new Action(() =>
                {
                    //if (GlobalVar.English)
                    //{
                    //    Processnotice.Text = English;
                    //}
                    //else
                    {
                        Processnotice.Text = Chinese;
                    }
                }));
            }
        }
        public int SetprogressBarValue
        {
            set
            {
                Invoke(new Action(() =>
                {
                    progressBar1.Value = value;
                    ProcessBarInt.Text = " " + value + " %";
                }));
            }
        }
        public void CloseResetProcessForm()
        {
            Invoke(new Action(() =>
            {
                this.Close();
            }));
        }
    }
}