using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Data;

namespace AutoScrew
{
    public class FileOp
    {
        private static string getDefaultConfigPath(string fileName)
        {
            string filePath = Directory.GetCurrentDirectory() + @"\Config\";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath += "\\" + fileName;
            return filePath;
        }
       
        //删除绝对路径下的文件      
        private static string deleteOldFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            return fileName;
        }

        // 创建默认路径下"COCOB.XML"文件
        public static void CreateXMLFile()
        {
            if (!File.Exists(getDefaultConfigPath("FSI.xml")))
            {
                XmlDocument xml = new XmlDocument();
                XmlDeclaration xmldel;
                XmlNode root;
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
        }
        // 创建相对路径下的文件      
        public static void CreateXMLFileRP(string fileName)
        {
            string filePath = Directory.GetCurrentDirectory() + @"\Config\";
            if (!File.Exists(getDefaultConfigPath(fileName + ".xml")))
            {
                XmlDocument xml = new XmlDocument();
                XmlDeclaration xmldel;
                XmlNode root;
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
                xml.Save(filePath + fileName + ".xml");
            }
        }
        // 创建绝对路径下的文件    
        public static void CreateXMLFileAP(string fileName)
        {
            if (!File.Exists(fileName + ".xml"))
            {
                XmlDocument xml = new XmlDocument();
                XmlDeclaration xmldel;
                XmlNode root;
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
        }
        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public static void CopyFolder()
        {
            try
            {
                string sourceFolder = Directory.GetCurrentDirectory() + @"\Config\";
                string destFolder = Directory.GetCurrentDirectory() + @"\ConfigBackUp\" + DateTime.Now.ToString("yyyyMMdd") + @"\";
                //如果目标路径不存在,则创建目标路径
                if (!Directory.Exists(destFolder))//若目标文件夹不存在
                {
                    string newPath;
                    FileInfo fileInfo;
                    Directory.CreateDirectory(destFolder);//创建目标文件夹
                    //遍历文件
                    string[] strs = Directory.GetFiles(sourceFolder);//获取源文件夹中的所有文件完整路径
                    foreach (string path in strs)
                    {
                        fileInfo = new FileInfo(path);
                        //如果需要筛选：
                        //此处可使用fileInfo.Extension（.扩展名）
                        //fileInfo.Name (文件名.扩展名)等获取文件扩展名做筛选
                        newPath = destFolder + fileInfo.Name;
                        File.Copy(path, newPath, true);
                    }
                }
            }
            catch (Exception e)
            {
                
            }
        }

        public static bool SaveDataToXMLDPDP(string fnodename, string cnodename, string cnodetext)
        {
            bool booladddata = true;
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmldel;
            XmlNode root;
            XmlNode node1;
            XmlNode node2;
            try
            {

                if (!File.Exists(getDefaultConfigPath("FSI.xml")))
                {
                    xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                    xml.AppendChild(xmldel);
                    root = xml.CreateElement("Parameter");
                    xml.AppendChild(root);
                }
                else
                {
                    xml.Load(getDefaultConfigPath("FSI.xml"));
                    root = xml.SelectSingleNode("Parameter");
                    if (root == null)
                    {
                        deleteOldFile(getDefaultConfigPath("FSI.xml"));
                        booladddata = false;
                        return booladddata;
                    }
                }
                if (root.SelectSingleNode(fnodename) != null)
                {
                    node1 = root.SelectSingleNode(fnodename);
                    if (node1.SelectSingleNode(cnodename) != null)
                    {
                        node2 = node1.SelectSingleNode(cnodename);
                        node2.InnerText = cnodetext;
                    }
                    else
                    {
                        node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                        node2.InnerText = cnodetext;
                        node1.AppendChild(node2);
                    }
                }
                else
                {
                    node1 = xml.CreateNode(XmlNodeType.Element, fnodename, null);
                    root.AppendChild(node1);
                    node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                    node2.InnerText = cnodetext;
                    node1.AppendChild(node2);
                }
                xml.Save(getDefaultConfigPath("FSI.xml"));
            }
            catch (Exception)
            {
                booladddata = false;
            }
            return booladddata;
        }
        public static void WriteLaserResultToCSV(string disX, string disU, string disD)//数据统计
        {
          
                StreamWriter sw;
                string strdata = "";
                string InputdataTocsv = "";

                string filePath = @"d:\Report\Size\";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string DataFileName = filePath + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

                string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

                strdata = time + "," + disX + "," + disU + "," + disU ;
                if (!File.Exists(DataFileName))
                {
                    InputdataTocsv = "Time,DisX,DisU,DisD";
                    sw = File.CreateText(DataFileName);
                    sw.WriteLine(InputdataTocsv);
                }
                else
                {
                    sw = File.AppendText(DataFileName);
                }
                sw.WriteLine(strdata);
                sw.Flush();
                sw.Close();            
        }
        public static void WriteResultToCSV(string BN, string Result)//数据统计
        {

            StreamWriter sw;
            string strdata = "";
            string InputdataTocsv = "";

            string filePath = @"d:\Result\";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string DataFileName = filePath + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

            string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            strdata = time + "," + BN + "," + Result;
            if (!File.Exists(DataFileName))
            {
                InputdataTocsv = "ID,Batch NO.,Result";
                sw = File.CreateText(DataFileName);
                sw.WriteLine(InputdataTocsv);
            }
            else
            {
                sw = File.AppendText(DataFileName);
            }
            sw.WriteLine(strdata);
            sw.Flush();
            sw.Close();
        }
        public static bool ReadDataFromXMLDP(string fnodename, string cnodename, ref string cnodetext)
        {
            bool boolreaddata = true;
            XmlDocument xml = new XmlDocument();
            xml.Load(getDefaultConfigPath("FSI.xml"));
            XmlNode node1;
            XmlNode node2;
            XmlNode root = xml.SelectSingleNode("Parameter");
            try
            {



                if (root == null)
                {
                    boolreaddata = false;
                    return boolreaddata;
                }
                if (root.SelectSingleNode(fnodename) != null)
                {
                    node1 = root.SelectSingleNode(fnodename);
                    if (node1.SelectSingleNode(cnodename) != null)
                    {
                        node2 = node1.SelectSingleNode(cnodename);
                        cnodetext = node2.InnerText;
                    }
                    else
                    {
                        cnodetext = "0";
                        boolreaddata = false;
                    }
                }
                else
                {
                    cnodetext = "0";
                    boolreaddata = false;
                }
            }

            catch (Exception)
            {

            }
            return boolreaddata;

        }
        public static bool SaveDataToXMLDPRP(string filename,string fnodename, string cnodename, string cnodetext)
        {
            bool booladddata = true;
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmldel;
            XmlNode root;
            XmlNode node1;
            XmlNode node2;
            if (!File.Exists(getDefaultConfigPath(filename + ".xml")))
            {
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
            else
            {
                xml.Load(getDefaultConfigPath(filename + ".xml"));
                root = xml.SelectSingleNode("Parameter");
                if (root == null)
                {
                    deleteOldFile(getDefaultConfigPath(filename + ".xml"));
                    booladddata = false;
                    return booladddata;
                }
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    node2.InnerText = cnodetext;
                }
                else
                {
                    node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                    node2.InnerText = cnodetext;
                    node1.AppendChild(node2);
                }
            }
            else
            {
                node1 = xml.CreateNode(XmlNodeType.Element, fnodename, null);
                root.AppendChild(node1);
                node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                node2.InnerText = cnodetext;
                node1.AppendChild(node2);
            }
            xml.Save(getDefaultConfigPath(filename + ".xml"));
            return booladddata;
        }


        public static int WriteLines(string p_szFileName_abs, ref List<string> p_lisRows)
        {
            int nRet = 0;

            string szFileAllname = "";
            if (!p_szFileName_abs.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                szFileAllname = p_szFileName_abs + ".csv";
            }
            else
            {
                szFileAllname = p_szFileName_abs;
            }

            try
            {
                FileStream fs;
                StreamWriter sw;

                fs = new FileStream(szFileAllname, FileMode.Create, FileAccess.ReadWrite);
                sw = new StreamWriter(fs, Encoding.GetEncoding("utf-8")); //GB2312

                foreach (var item in p_lisRows)
                {
                    sw.WriteLine(item);
                }
                sw.Flush();
                fs.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                nRet = 1;
            }
            return nRet;
        }

        public static bool ReadDataFromXMLRP(string filename, string fnodename, string cnodename, ref string cnodetext)   //读取相对路径
        {
            bool boolreaddata = true;
            XmlDocument xml = new XmlDocument();
            xml.Load(getDefaultConfigPath(filename + ".xml"));
            XmlNode node1;
            XmlNode node2;
            XmlNode root = xml.SelectSingleNode("Parameter");
            if (root == null)
            {
                boolreaddata = false;
                return boolreaddata;
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    if (node2.InnerText != "")
                    {
                        cnodetext = node2.InnerText;
                    }
                    else
                    {
                        cnodetext = "0";
                    }
                }
                else
                {
                    cnodetext = "0";
                    boolreaddata = false;
                }
            }
            else
            {
                cnodetext = "0";
                boolreaddata = false;
            }
            return boolreaddata;
        }

        /// <summary>
        /// 保存在绝对路径下的文件中
        /// </summary>
        /// <param name="filename">绝对路径下的文件名</param>
        /// <param name="fnodename">父节点</param>
        /// <param name="cnodename">子节点</param>
        /// <param name="cnodetext">子节点内容</param>
        /// <returns>是否成功</returns>
        public static bool SaveDataToXMLDPAP(string filename, string fnodename, string cnodename, string cnodetext)       //保存绝对路径
        {
            bool booladddata = true;
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xmldel;
            XmlNode root;
            XmlNode node1;
            XmlNode node2;
            if (!File.Exists(filename + ".xml"))
            {
                xmldel = xml.CreateXmlDeclaration("1.0", null, null);
                xml.AppendChild(xmldel);
                root = xml.CreateElement("Parameter");
                xml.AppendChild(root);
            }
            else
            {
                xml.Load(filename + ".xml");
                root = xml.SelectSingleNode("Parameter");
                if (root == null)
                {
                    deleteOldFile(filename + ".xml");
                    booladddata = false;
                    return booladddata;
                }
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    node2.InnerText = cnodetext;
                }
                else
                {
                    node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                    node2.InnerText = cnodetext;
                    node1.AppendChild(node2);
                }
            }
            else
            {
                node1 = xml.CreateNode(XmlNodeType.Element, fnodename, null);
                root.AppendChild(node1);
                node2 = xml.CreateNode(XmlNodeType.Element, cnodename, null);
                node2.InnerText = cnodetext;
                node1.AppendChild(node2);
            }
            xml.Save(filename + ".xml");

            return booladddata;
        }

        /// <summary>
        /// 从绝对路径下的文件中读取
        /// </summary>
        /// <param name="filename">绝对路径下的文件名</param>
        /// <param name="fnodename">父节点</param>
        /// <param name="cnodename">子节点</param>
        /// <param name="cnodetext">子节点内容</param>
        /// <returns>是否成功</returns>
        public static bool ReadDataFromXMLAP(string filename, string fnodename, string cnodename, ref string cnodetext)
        {
            bool boolreaddata = true;
            XmlDocument xml = new XmlDocument();
            xml.Load(filename + ".xml");
            XmlNode node1;
            XmlNode node2;
            XmlNode root = xml.SelectSingleNode("Parameter");
            if (root == null)
            {
                boolreaddata = false;
                return boolreaddata;
            }
            if (root.SelectSingleNode(fnodename) != null)
            {
                node1 = root.SelectSingleNode(fnodename);
                if (node1.SelectSingleNode(cnodename) != null)
                {
                    node2 = node1.SelectSingleNode(cnodename);
                    if (node2.InnerText != "")
                    {
                        cnodetext = node2.InnerText;
                    }
                    else
                    {
                        cnodetext = "0";
                    }
                }
                else
                {
                    boolreaddata = false;
                }
            }
            else
            {
                boolreaddata = false;
            }

            return boolreaddata;
        }

        /// <summary>
        /// 获取相对路径下的生产数据文件
        /// </summary>
        /// <returns>生产数据文件的相对路径下</returns>
        public static string getDefaultDataPath()
        {
            string filePath = Directory.GetCurrentDirectory() + @"\生产数据\";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return filePath;
        }        
        public static void WriteRunTxt(string writedata)
        {
            StreamWriter sw;
            string strdata = "";
            strdata = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + "," + writedata;
            string MDataFileName = @"d:\生产数据\日志文件\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            try
            {
                if (!File.Exists(MDataFileName))
                {
                    sw = File.CreateText(MDataFileName);
                }
                else
                {
                    sw = File.AppendText(MDataFileName);
                }
                sw.WriteLine(strdata);//开始写入值 
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
        public static void WriteTxtAlarm(string writedata)
        {
            StreamWriter sw;
            string strdata = "";
            strdata = writedata;
            string MDataFileName = @"d:\生产数据\报警文件\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            if (!File.Exists(MDataFileName))
            {
                sw = File.CreateText(MDataFileName);
            }
            else
            {
                sw = File.AppendText(MDataFileName);
            }
            sw.WriteLine(strdata);//开始写入值 
            sw.Flush();
            sw.Close();
        }
        
        public static void WriteRecordValueCSV(string savePath, string fileName, string title, string value)
        {
            StreamWriter sw;
            string strdata = "";
            string InputdataTocsv = "";
            string MDataFileName = "";
            string DataFileName = @"e:\生产数据\" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

            strdata = value;
            string filePath = @"e:\生产数据\";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (!File.Exists(DataFileName))
            {
                InputdataTocsv = title;
                sw = File.CreateText(DataFileName);
                sw.WriteLine(InputdataTocsv);
            }
            else
            {
                sw = File.AppendText(DataFileName);
            }
            sw.WriteLine(strdata);  //开始写入值  
            sw.Flush();
            sw.Close();
        }


        public static void ReadDataFromCSV(string savePath)
        {
            //实例化一个datatable用来存储数据
            DataTable dt = new DataTable();

            //文件流读取
            FileStream fs = new FileStream("d:\\1.csv", FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312"));

            string tempText = "";
            bool isFirst = true;
            while ((tempText = sr.ReadLine()) != null)
            {
                string[] arr = tempText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //一般第一行为标题，所以取出来作为标头
                if (isFirst)
                {
                    foreach (string str in arr)
                    {
                        dt.Columns.Add(str);
                    }
                    isFirst = false;
                }
                else
                {
                    //从第二行开始添加到datatable数据行
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dr[i] = i < arr.Length ? arr[i] : "";
                    }
                    dt.Rows.Add(dr);
                }
            }
            //展示到页面
            // dataGridView1.DataSource = dt;
            //关闭流
            sr.Close();
            fs.Close();
        }


        public static void WriteDataToCSV(string time, string x1, string y1, string x2, string y2, string x3, string y3, string x4, string y4)
        {
            StreamWriter sw;
            string strdata = "";
            string InputdataTocsv = "";
            string MDataFileName = "";
            string DataFileName = @"e:\生产数据\" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

            strdata = time + "," + x1 + "," + y1 + "," + x2 + "," + y2 + "," + x3 + "," + y3 + "," + x4 + "," + y4;
            if (!File.Exists(DataFileName))
            {
                InputdataTocsv = "Time, x1, y1, x2, y2, x3, y3, x4, y4";
                sw = File.CreateText(DataFileName);
                sw.WriteLine(InputdataTocsv);
            }
            else
            {
                sw = File.AppendText(DataFileName);
            }
            sw.WriteLine(strdata);  //开始写入值  
            sw.Flush();
            sw.Close();
            if (!File.Exists(MDataFileName))
            {
                InputdataTocsv = "Time, x1, y1, x2, y2, x3, y3, x4, y4";
                sw = File.CreateText(MDataFileName);
                sw.WriteLine(InputdataTocsv);
            }
            else
            {
                sw = File.AppendText(MDataFileName);
            }
            sw.WriteLine(strdata);  //开始写入值  
            sw.Flush();
            sw.Close();
        }       
        public static string[] barcodeSPK = new string[64];
        public static void ReadCPKbarcode()//自动生成CPK文件
        {
            string MAutoCPKFileName = Directory.GetCurrentDirectory() + @"\AutoCPK\AUTOCPK&GRRSN.csv";
            // string MAutoCPKFileName = @"D:\生产数据\AUTOCPK&GRRSN.csv";
            if (File.Exists(MAutoCPKFileName))
            {

                FileStream fs = new FileStream(MAutoCPKFileName, FileMode.Open);

                StreamReader m_streamReader = new StreamReader(fs);

                m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

                string strLine = m_streamReader.ReadLine();

                int i = 0;
                do
                {

                    strLine = m_streamReader.ReadLine();

                    if (strLine == null)
                    {
                        break;
                    }
                    string[] singlecmddata = strLine.Split(new char[] { ',' });

                    barcodeSPK[i] = singlecmddata[1];
                    i++;

                    if (i > 49)
                    {
                        break;
                    }
                    else
                    {

                    }

                } while (strLine != null);

                m_streamReader.Close();
                m_streamReader.Dispose();
                fs.Close();
            }
            else
            {
                MessageBox.Show("AutoCPK模板数据不存在，请先把模板数据考进D：生产数据目录下面：名称和CSV格式为：AUTOCPK&GRRSN.csv，");
            }
        }      
        public static void WriteDataAsCSV(string ptime, string CameraUTCode, string CameraNVCode, string HSGCode,
                                       string disUTx, string disUTy, string disNVx,
                                       string disNVy, string disUTcc, string disNVcc,
                                      string Spring1_X, string Spring2_X, string Spring3_Y,
                                       string Spring4_Y, string Foam_X, string Foam_Angle,
                                         string mpressure, string Zpos, string CameraTossingNum,
                                       string Passorfail, string mode, string Test, int Pro, string status)
        {
            #region
            StreamWriter sw;
            string LinerSta = "";
            string strdata = "";
            string InputdataTocsv = "";
            string MDataFileName = "";
            string DataFileName = "";
            if (HSGCode == " ")
            {
                HSGCode = "123456789";
            }
            if (double.Parse(disNVcc) <= 0.25 && double.Parse(Foam_X) <= 0.2 && Math.Abs(double.Parse(Foam_Angle)) <= 1)
            {
                status = "OK";  // wu
            }
            else
            {
                status = "NG";  //you
            }
                DataFileName = @"d:\生产统计\Data\" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";
 

            #endregion
            strdata = ptime + "," + CameraUTCode + "," + CameraNVCode + "," + HSGCode + "," + disUTx + "," + disUTy + "," + disNVx + ","
                + disNVy + "," + disUTcc + "," + disNVcc + "," + Spring1_X + "," + Spring2_X + "," + Spring3_Y + "," + Spring4_Y
                + "," + Foam_X + "," + Foam_Angle + "," + mpressure + "," + Zpos + "," + CameraTossingNum + "," + Passorfail + "," + mode + "," + Test + "," + Pro + "," + status;   
           
                try
                {
                    if (!File.Exists(DataFileName))
                    {
                        InputdataTocsv = "ptime,CameraUTCode,CameraNVCode,HSGCode,disUTx,disUTy,disNVx,disNVy,disUTcc,disNVcc,Spring1_X,Spring2_X,Spring3_Y,Spring4_Y,Foam_cc,Foam_Angle,mpressure,Zpos,CameraTossingNum,Passorfail,mode,Test,Pro,status";
                        sw = File.CreateText(DataFileName);
                        sw.WriteLine(InputdataTocsv);//开始写入值  
                    }
                    else
                    {
                        sw = File.AppendText(DataFileName);
                    }
                    sw.WriteLine(strdata);//开始写入值  
                    sw.Flush();
                    sw.Close();
                }
                catch
                { }          
        }    
    }


}
