using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Drawing.Imaging;

namespace AutoScrew
{
    class MyChart
    {
        Series torq_line = new Series("Torq");    //扭力线
        Series torq_lineF = new Series("");       //上限线
        Series torq_line1 = new Series("Torq1");       //上限线
        Series torq_lineSSL = new Series("");     //SSL着座点
        public MyChart()
        {
            torq_line.ChartType = SeriesChartType.Line;   //定义线型
            torq_line.Color = System.Drawing.Color.Blue;  //定义颜色

            torq_line1.ChartType = SeriesChartType.Line;   //定义线型
            torq_line1.Color = System.Drawing.Color.Red;  //定义颜色
            torq_line1.XAxisType = AxisType.Secondary;

            torq_lineF.ChartType = SeriesChartType.Line;
            torq_lineF.Color = System.Drawing.Color.Yellow;
            torq_lineSSL.ChartType = SeriesChartType.Line;
            torq_lineSSL.Color = System.Drawing.Color.LightGreen;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>          //图形控件
        /// <param name="labels"></param>         //标签控件数组
        /// <param name="interval"></param>       //采样时间间隔
        /// <param name="type"></param>           //0:扭力vs时间，1:扭力vs角度
        /// <param name="savePath"></param>       //保存路径
        /// <param name="picName"></param>        //图片名称
        /// <param name="arrResultTK"></param>    //过程数据
        /// <param name="Max_Torque"></param>     //最大扭力
        /// <param name="Max_Angle"></param>      //最大角度
        /// <param name="torq_double"></param>    //扭力数据
        /// <param name="Angle_double"></param>   //角度数据
        /// <param name="titleAxisY"></param>     //Y轴标题
        public void Draw(Chart chart, Label[] labels, int interval, int type, string[] arrResultTK, double Max_Torque, double Max_Angle, double[] torq_double, double[] Angle_double, string titleAxisY, int indexSSL)
        {
            string EC = arrResultTK[3];
            double torqDrawF = Convert.ToDouble(arrResultTK[20]);      //Drop_torque上限值
            double torqDrawSSL = torq_double[indexSSL];   //SSL着座值
            string[] items = new string[12];
            items[0] = "EC=" + arrResultTK[3];   //EC
            items[1] = "T1=" + arrResultTK[7];   //T1
            items[2] = "T2=" + arrResultTK[8];   //T2
            items[3] = "T3=" + arrResultTK[9];   //T3
            items[4] = "Tt=" + arrResultTK[11];  //Tt
            items[5] = "A1=" + arrResultTK[12];  //A1
            items[6] = "A2=" + arrResultTK[13];  //A2
            items[7] = "A3=" + arrResultTK[14];  //A3
            items[8] = "Max.angle=" + Max_Angle;
            items[9] = "Max.torque=" + Max_Torque;
            items[10] = "SSL=" + torqDrawSSL;    //SSL
            items[11] = "MC=" + arrResultTK[2];  //MC
            for (int i = 0; i < 12; i++)
            {
                labels[i].Invoke(new EventHandler(delegate
                {
                    labels[i].Text = items[i];
                }));
            }

            chart.Invoke(new EventHandler(delegate
            {
                chart.Series.Clear();
                torq_line.Points.Clear();
                torq_line1.Points.Clear();
                torq_lineF.Points.Clear();
                torq_lineSSL.Points.Clear();

                int length = torq_double.Length;
                if (EC == "OK")
                {
                    for (int i = 0; i < length; i++)   //对着上限值和扭力线进行描点
                    {
                        if (type == 0)  //0:扭力vs时间
                        {
                            torq_line.Points.AddXY(interval * i, torq_double[i]);   //对线进行描点
                            torq_line1.Points.AddXY(Math.Abs(Angle_double[i]), torq_double[i]);  //反转时角度在图上显示正值
                            torq_lineF.Points.AddXY(interval * i, torqDrawF);       //对上限线进行描点
                        }
                        else      //否则1:扭力vs角度
                        {
                            torq_line.Points.AddXY(Math.Abs(Angle_double[i]), torq_double[i]);  //反转时角度在图上显示正值
                            torq_lineF.Points.AddXY(Math.Abs(Angle_double[i]), torqDrawF);
                        }
                    }

                    for (int i = 0; i < indexSSL + 1; i++)   //对着座值SSL进行描点
                    {
                        if (type == 0)
                        {
                            torq_lineSSL.Points.AddXY(interval * i, torq_double[indexSSL]);
                        }
                        else
                        {
                            torq_lineSSL.Points.AddXY(Math.Abs(Angle_double[i]), torq_double[indexSSL]);
                        }
                    }
                }
                else   //否则锁NG
                {
                    for (int i = 0; i < length; i++)
                    {
                        if (type == 0)
                        {
                            torq_line.Points.AddXY(interval * i, torq_double[i]);
                            torq_line1.Points.AddXY(Math.Abs(Angle_double[i]), torq_double[i]);
                        }
                        else
                        {
                            torq_line.Points.AddXY(Math.Abs(Angle_double[i]), torq_double[i]);
                        }
                    }
                }

                double avgAngle = 0;    //用多個值累加避免只用最後一個值正負都有可能
                for (int i = 0; i < length; i++)
                {
                    avgAngle += Angle_double[i];
                }
                if (avgAngle < 0)  //反转时，上限线torqDrawF重新描成负值
                {
                    torq_lineF.Points.Clear();        //把原来的点清理掉
                    for (int i = 0; i < length; i++)   //对着上限值进行描点
                    {
                        if (type == 0)  //0:扭力vs时间
                        {
                            torq_lineF.Points.AddXY(interval * i, -torqDrawF);       //对上限线进行描点
                           
                        }
                        else      //否则1:扭力vs角度
                        {
                            torq_lineF.Points.AddXY(Math.Abs(Angle_double[i]), -torqDrawF);
                        }
                    }
                }

                chart.Series.Add(torq_lineF);    //把线添加到图纸
                chart.Series.Add(torq_line1);    //把线添加到图纸
                chart.Series.Add(torq_lineSSL);
                chart.Series.Add(torq_line);
                chart.ChartAreas[0].AxisY.Title = titleAxisY;   //Y轴标题

            }));
        }

        public void ShotPicture(Panel panel, string savePath, string picName)   //截图
        {

            if (!Directory.Exists(savePath))        //创建保存路径
                Directory.CreateDirectory(savePath);

            string path = savePath + "\\" + picName + ".bmp";
           
                Bitmap bmp = new Bitmap(panel.Width, panel.Height);
                panel.DrawToBitmap(bmp, new Rectangle(0, 0, panel.Width, panel.Height));
                bmp.Save(path, ImageFormat.Bmp);
           
        }
    }
}
