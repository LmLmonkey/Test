using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoScrew
{
    class CDBhelper
    {
        private static CDBhelper ms_me = null;
        private static readonly object ms_lockme = new object();
        // private static SQLiteConnection m_DBSqlConn = null;
        public static double[] mCT = new double[10];
        public CDBhelper()
        {
        }
        public static CDBhelper _Get_Instance()
        {
            lock (ms_lockme)
            {
                if (ms_me == null)
                {
                    ms_me = new CDBhelper();
                }
            }
            return ms_me;
        }

        public void _Init(string p_dbAbsPath)
        {
            try
            {
                if (GlobalVar.ms_objConnection == null)
                {
                    if (!File.Exists(p_dbAbsPath))
                    {
                        SQLiteConnection.CreateFile(p_dbAbsPath);
                        GlobalVar.ms_objConnection = new SQLiteConnection("data source=" + p_dbAbsPath);
                        GlobalVar.ms_objConnection.Open();
                        string name3 = "用户管理";
                        string ssSql_Error4 = "CREATE TABLE IF NOT EXISTS " + name3 + "(aid varchar(36) primary key," + "UserName string,PassWord String,Level String);";      //只保存协定药房的名称
                        SQLiteCommand commandError4 = new SQLiteCommand(ssSql_Error4, GlobalVar.ms_objConnection);
                        commandError4.ExecuteNonQuery();
                        for (int i = 0; i < 3; i++)
                        {
                            string name = "工位"+(i+1).ToString()+"组合轴";
                            string ssSql_Error = "CREATE TABLE IF NOT EXISTS " + name + "(aid varchar(36) primary key," + "PointName string,X string,Y String,Z String);";      //只保存协定药房的名称
                            SQLiteCommand commandError = new SQLiteCommand(ssSql_Error, GlobalVar.ms_objConnection);
                            commandError.ExecuteNonQuery();
                        }
                        string ssTableName = "T_EquError";
                        string ssSql_t = "CREATE TABLE IF NOT EXISTS " + ssTableName + "(aid varchar(36) primary key,"
                        + "errId integer,Message string,Class string,createTime DATETIME,endTime DATETIME);";
                        SQLiteCommand commandError1 = new SQLiteCommand(ssSql_t, GlobalVar.ms_objConnection);
                        commandError1.ExecuteNonQuery();
                        string ssTableName1 = "ProduceData";
                        string ssSql_t1 = "CREATE TABLE IF NOT EXISTS " + ssTableName + "(aid varchar(36) primary key,"
                        + "SN string,CC_3 string,CC_4 string,CC_5 string,CC_6 string,ScrewCC_3 string,ScrewCC_4 string,ScrewCC_5 string,ScrewCC_6 string,Result string,createTime DATETIME);";
                        SQLiteCommand commandError11 = new SQLiteCommand(ssSql_t1, GlobalVar.ms_objConnection);
                        commandError11.ExecuteNonQuery();
                    }
                    else
                    {                   
                        GlobalVar.ms_objConnection = new SQLiteConnection("data source = " + p_dbAbsPath);
                        GlobalVar.ms_objConnection.Open();
                    }
                }
            }
           catch (Exception e)
            { }
        }

        public void CreateTable(string name)              //用于创建协定处方表
        {
            string ssSql_Error = "CREATE TABLE IF NOT EXISTS " + "用户管理" + "(aid varchar(36) primary key," + "UserName string,PassWord String,Level String);";      //只保存中药颗粒的名称、计量，密度当量信息从中药颗粒的数据库获取；
            SQLiteCommand commandError = new SQLiteCommand(ssSql_Error, GlobalVar.ms_objConnection);
            commandError.ExecuteNonQueryAsync();
        }
        public void DeleteTable(string name)              //用于删除协定处方表     有问题
        {
            //string ssSql_Error = "DROP TABLE IF EXISTS " + name;      //只保存中药颗粒的名称、计量，密度当量信息从中药颗粒的数据库获取；
            //SQLiteCommand commandError3 = new SQLiteCommand(ssSql_Error, GlobalVar.ms_objConnection);
            //commandError3.ExecuteNonQuery();          
            SQLiteCommand cmd = GlobalVar.ms_objConnection.CreateCommand();
            cmd.CommandText = "DROP TABLE IF EXISTS " + name;
            cmd.ExecuteNonQuery();
        }

        public void Table_Remove(string name)    //删除行
        {
            int nRet = 0;
            string ssTableName = "协定处方";
            //--DateTime earlyTim = DateTime.Now.AddDays(-60);
            string ssSql_t = "DELETE FROM " + ssTableName + " WHERE CF_name= @p1;";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",name)};
            try
            {
                CSqliteWrapper._S_Get_ConnObj(true);
                nRet = CSqliteWrapper._S_ExecuteSql(ssSql_t, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;              
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }           
        }

        public void Table_Remove_name(string name)    //删除行
        {
            int nRet = 0;
            string ssTableName = "协定名称";
            //--DateTime earlyTim = DateTime.Now.AddDays(-60);
            string ssSql_t = "DELETE FROM " + ssTableName + " WHERE Name= @p1;";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",name)};
            try
            {
                CSqliteWrapper._S_Get_ConnObj(true);
                nRet = CSqliteWrapper._S_ExecuteSql(ssSql_t, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
        }
        public void User_Remove(string username)    //删除行
        {
            int nRet = 0;
            string ssTableName = "User";
            //--DateTime earlyTim = DateTime.Now.AddDays(-60);
            string ssSql_t = "DELETE FROM " + ssTableName + " WHERE UserName= @p1;";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",username)};
            try
            {
                CSqliteWrapper._S_Get_ConnObj(true);
                nRet = CSqliteWrapper._S_ExecuteSql(ssSql_t, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }


        }     
        public bool Isexists(string name)     //判断表是否存在
        {
            bool net = false;
            SQLiteCommand cmd = GlobalVar.ms_objConnection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + name + "'";
            //DataSet ds = new DataSet();
            //SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            //da.Fill(ds);
            if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
            {
                net = false;
            }
            else
            {
                net = true;
            }
            return net;
        }

        public int _Message_Add(string number, string name, string weight, string equivalent, string density, string x, string y, string people, string time)   //添加中药颗粒
        {
            int nRet = 0;
            string ssTableName = "T_Message";
            string sql = "insert into " + ssTableName + " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Guid.NewGuid().ToString()),
                new SQLiteParameter("@p2",number),
                new SQLiteParameter("@p3",name),
                new SQLiteParameter("@p4",weight),
                new SQLiteParameter("@p5",equivalent),
                new SQLiteParameter("@p6",density),
                new SQLiteParameter("@p7",x),
                new SQLiteParameter("@p8",y),
                new SQLiteParameter("@p9",people),
                new SQLiteParameter("@p10",time)};
            try
            {
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }
        public int _Add(string CF_name, string name, string weight)    //添加协定处方
        {
            int nRet = 0;
            string name1 = "协定处方";
            string sql = "insert into " + name1 + " VALUES(@p1,@p2,@p3,@p4)";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Guid.NewGuid().ToString()),
                new SQLiteParameter("@p2",CF_name),
                new SQLiteParameter("@p3",name),
                new SQLiteParameter("@p4",weight)};
            try
            {
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }

        public int User_Add(string username, string password, string level)    //添加用户信息
        {
            int nRet = 0;
            string name1 = "用户管理";
            string sql = "insert into " + name1 + " VALUES(@p1,@p2,@p3,@p4)";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Guid.NewGuid().ToString()),
                new SQLiteParameter("@p2",username),
                new SQLiteParameter("@p3",password),
                new SQLiteParameter("@p4",level)};
            try
            {
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }


        public int PointAdd(string Tablename, string Point, string X, string Y, string Z)    //添加用户信息
        {
            int nRet = 0;
            string sql = "insert into " + Tablename + " VALUES(@p1,@p2,@p3,@p4,@p5)";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Guid.NewGuid().ToString()),
                new SQLiteParameter("@p2",Point),
                new SQLiteParameter("@p3",X),
                new SQLiteParameter("@p4",Y),
                new SQLiteParameter("@p5",Z)};
            try
            {
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }
        public void PointRemove(string Tablename, string Pointe)    //删除行
        {
            int nRet = 0;
            string ssSql_t = "DELETE FROM " + Tablename + " WHERE PointName= @p1;";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Pointe)};
            try
            {
                CSqliteWrapper._S_Get_ConnObj(true);
                nRet = CSqliteWrapper._S_ExecuteSql(ssSql_t, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
        }
        public SQLiteDataReader PointMessage(string Tablename, string Pointe)              //协定处方包含的颗粒种类
        {
            SQLiteDataReader reader = null;
            try
            {
                string cmdstr = "select * from " + Tablename + " where PointName=@p1";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Pointe)
                };
                CSqliteWrapper._S_Get_ConnObj(false);
                reader = CSqliteWrapper._S_ExecuteReader(cmdstr, objParam);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }

        public int _ProductData_Add(string SN,string CC_3, string CC_4, string CC_5, string CC_6,DateTime datatime)
        {
            int nRet = 0;

            try
            {
                string ssTableName = "ProduceData";
                string sql = "insert into " + ssTableName + " VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12)";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Guid.NewGuid().ToString()),
                new SQLiteParameter("@p2",SN),
                new SQLiteParameter("@p3",CC_3),
                new SQLiteParameter("@p4",CC_4),
                new SQLiteParameter("@p5",CC_5),
                new SQLiteParameter("@p6",CC_6),
                new SQLiteParameter("@p7","0"),
                new SQLiteParameter("@p8","0"),
                new SQLiteParameter("@p9","0"),
                new SQLiteParameter("@p10","0"),
                new SQLiteParameter("@p11","NG"),
                new SQLiteParameter("@p12",datatime)};
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }

        public int _Up_Product_CCD2_CC_By_SN(string SN,string Screw_CC3, string Screw_CC4)
        {
            int nRet = 0;

            try
            {
                string ssTableName = "ProduceData";
                string sql = "update " + ssTableName + " set ScrewCC_3=@p1,ScrewCC_4=@p2 where SN=@p3";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Screw_CC3),
                new SQLiteParameter("@p2",Screw_CC4),
                new SQLiteParameter("@p3", SN)};
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }
        public int _Up_Product_CCD3_CC_By_SN(string SN, string Screw_CC5, string Screw_CC6)
        {
            int nRet = 0;

            try
            {
                string ssTableName = "ProduceData";
                string sql = "update " + ssTableName + " set ScrewCC_5=@p1,ScrewCC_6=@p2 where SN=@p3";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Screw_CC5),
                new SQLiteParameter("@p2",Screw_CC6),
                new SQLiteParameter("@p3", SN)};
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }
        //设备故障信息

        public int _EquError_Add(ST_EquError p_stIn)
        {
            int nRet = 0;
           
            try
            {
                string ssTableName = "T_EquError";
                string sql = "insert into " + ssTableName + " VALUES(@p1,@p2,@p3,@p4,@p5,@p6)";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Guid.NewGuid().ToString()),
                new SQLiteParameter("@p2",(UInt32)p_stIn.emID),
                new SQLiteParameter("@p3",p_stIn.ssDesc_en),
                new SQLiteParameter("@p4",p_stIn.Category),
                new SQLiteParameter("@p5",p_stIn.stTime_beg),
                new SQLiteParameter("@p6",p_stIn.stTime_beg)};

                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }
        public int _EquError_Alter_ByErrEnd(ST_EquError p_stIn)
        {
            int nRet = 0;
           
            try
            {
                string ssTableName = "T_EquError";
                string sql = "update " + ssTableName + " set endTime=@p1 where errId=@p2 and createTime=@p3";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",p_stIn.stTime_end),
                new SQLiteParameter("@p2",p_stIn.emID),
                new SQLiteParameter("@p3",p_stIn.stTime_beg)};
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }
        public int _EquError_Get(DateTime p_begin, DateTime p_end, out List<ST_EquError> p_lisOUT)
        {
            p_lisOUT = new List<ST_EquError>();

            int nRet = 0;
            string ssTableName = "T_EquError";
            string sql = "select * from " + ssTableName + " where createTime>=@p1 and createTime<@p2";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",p_begin),
                new SQLiteParameter("@p2",p_end)
                };
            try
            {
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(false);


            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return nRet;
        }


        public SQLiteDataReader Get_Resistance1Message_Time(DateTime BeginTime, DateTime EndTime)              
        {
            SQLiteDataReader reader = null;
            try
            {
                string ssTableName = "T_EquError";
                string sql = "select * from " + ssTableName + " where createTime>=@p1 and createTime<@p2";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",BeginTime),
                new SQLiteParameter("@p2",EndTime)
                };
                CSqliteWrapper._S_Get_ConnObj(false);
                reader = CSqliteWrapper._S_ExecuteReader(sql, objParam);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }
        public SQLiteDataReader Get_ProductMessage_Time(DateTime BeginTime, DateTime EndTime)
        {
            SQLiteDataReader reader = null;
            try
            {
                string ssTableName = "ProduceData";
                string sql = "select * from " + ssTableName + " where createTime>=@p1 and createTime<@p2";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",BeginTime),
                new SQLiteParameter("@p2",EndTime)
                };
                CSqliteWrapper._S_Get_ConnObj(false);
                reader = CSqliteWrapper._S_ExecuteReader(sql, objParam);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }
        public SQLiteDataReader Get_ProductMessage_BySN(string SN)
        {
            SQLiteDataReader reader = null;
            try
            {
                string ssTableName = "ProduceData";
                string sql = "select * from " + ssTableName + " where SN=@p1";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",SN)};
                CSqliteWrapper._S_Get_ConnObj(false);
                reader = CSqliteWrapper._S_ExecuteReader(sql, objParam);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }


        public void _DeleteHistoryData()
        {
            //删除过早的数据''''''''''''''''''''''''''''''''''''''
            DateTime dtEarly = DateTime.Now.AddDays(GlobalVar.DataSaveDays);
            Table_RemoveRecordsByCreateTime("T_EquError", dtEarly);
            Table_RemoveRecordsByCreateTime("ProduceData", dtEarly);
        }
        private int Table_RemoveRecordsByCreateTime(string p_ssTable, DateTime p_dtStopOnMe)
        {
            int nRet = 0;           
            try
            {
                string ssTableName = p_ssTable;
                string ssSql_t = "DELETE FROM " + ssTableName + " WHERE createTime < @p1;";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",p_dtStopOnMe)};

                CSqliteWrapper._S_Get_ConnObj(true);
                nRet = CSqliteWrapper._S_ExecuteSql(ssSql_t, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }

            return 0;
        }




        public int Name_Add(string name)    //添加协定药方名称 
        {
            int nRet = 0;
            string name1 = "协定名称";
            string sql = "insert into " + name1 + " VALUES(@p1,@p2)";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Guid.NewGuid().ToString()),
                new SQLiteParameter("@p2",name)};
            try
            {
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }

        public int _Message_UpDate(string number, string name, string weight, string equivalent, string density, string x, string y, string people, string time)
        {
            int nRet = 0;
            string ssTableName = "T_Message";
            string sql = "update " + ssTableName + " set number=@p2 ,weight=@p4, equivalent=@p5 , density=@p6, x=@p7 , y=@p8, people=@p9 , time=@p10 where name=@p3";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Guid.NewGuid().ToString()),
                new SQLiteParameter("@p2",number),
                new SQLiteParameter("@p3",name),
                new SQLiteParameter("@p4",weight),
                new SQLiteParameter("@p5",equivalent),
                new SQLiteParameter("@p6",density),
                new SQLiteParameter("@p7",x),
                new SQLiteParameter("@p8",y),
                new SQLiteParameter("@p9",people),
                new SQLiteParameter("@p10",time)};
            try
            {
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }

        public int _Message_Change(string name, string x, string y)
        {
            int nRet = 0;
            string ssTableName = "T_Message";
            string sql = "update " + ssTableName + " set  x=@p7 , y=@p8 where name=@p3";
            SQLiteParameter[] objParam = new SQLiteParameter[]{
                new SQLiteParameter("@p1",Guid.NewGuid().ToString()),
                new SQLiteParameter("@p3",name),
                new SQLiteParameter("@p7",x),
                new SQLiteParameter("@p8",y)};
            try
            {
                //LogHelper.Debug("_Flyer_Read_byTime.beg");
                CSqliteWrapper._S_Get_ConnObj(true);
                CSqliteWrapper._S_ExecuteSql(sql, objParam);
            }
            catch (Exception e)
            {
                nRet = -1;
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(true);
            }
            return nRet;
        }       
        public SQLiteDataReader YGMessage()      //获取中药颗粒信息
        {
            SQLiteDataReader reader = null;
            try
            {
                string ssTableName = "T_Message";
                string cmdstr = "select * from " + ssTableName;
                SQLiteParameter[] objParam = new SQLiteParameter[] { };
                CSqliteWrapper._S_Get_ConnObj(false);
                objParam = null;
                reader = CSqliteWrapper._S_ExecuteReader(cmdstr, objParam);

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }
        public SQLiteDataReader UserYGMessage()      //获取中药颗粒信息
        {
            SQLiteDataReader reader = null;
            try
            {
                string ssTableName = "User";
                string cmdstr = "select * from " + ssTableName;
                SQLiteParameter[] objParam = new SQLiteParameter[] { };
                CSqliteWrapper._S_Get_ConnObj(false);
                objParam = null;
                reader = CSqliteWrapper._S_ExecuteReader(cmdstr, objParam);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }
        public SQLiteDataReader CF_Message(string name)              //协定处方包含的颗粒种类
        {
            SQLiteDataReader reader = null;
            try
            {
                string ssTableName = "协定处方";
                string cmdstr = "select * from " + ssTableName + " where CF_name=@p1";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
               // new SQLiteParameter("@p1",nShiftVal),
                new SQLiteParameter("@p1",name)
                };
                CSqliteWrapper._S_Get_ConnObj(false);
                reader = CSqliteWrapper._S_ExecuteReader(cmdstr, objParam);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }
        public SQLiteDataReader User_Message(string username)              //获取用户信息
        {
            SQLiteDataReader reader = null;
            try
            {
                string ssTableName = "用户管理";
                string cmdstr = "select * from " + ssTableName + " where UserName=@p1";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
               // new SQLiteParameter("@p1",nShiftVal),
                new SQLiteParameter("@p1",username)
                };
                CSqliteWrapper._S_Get_ConnObj(false);
                reader = CSqliteWrapper._S_ExecuteReader(cmdstr, objParam);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }

        public SQLiteDataReader Medicine_Message(string name)              //用户信息获取
        {
            SQLiteDataReader reader = null;
            try
            {
                string ssTableName = "T_Message";
                string cmdstr = "select * from " + ssTableName + " where Name=@p1";
                SQLiteParameter[] objParam = new SQLiteParameter[]{
               // new SQLiteParameter("@p1",nShiftVal),
                new SQLiteParameter("@p1",name)
                };
                CSqliteWrapper._S_Get_ConnObj(false);
                reader = CSqliteWrapper._S_ExecuteReader(cmdstr, objParam);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }

        public SQLiteDataReader Name_Message(string name)      //协定药方的名称获取
        {
            SQLiteDataReader reader = null;
            try
            {
                string ssTableName = "协定名称";
                string cmdstr = "select * from " + ssTableName;
                SQLiteParameter[] objParam = new SQLiteParameter[]{};
                CSqliteWrapper._S_Get_ConnObj(false);
                objParam = null;
                reader = CSqliteWrapper._S_ExecuteReader(cmdstr, objParam);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("queryDataInTable()" + ex.Message);
            }
            finally
            {
                CSqliteWrapper._S_Release_ConnObj(false);
            }
            return reader;
        }
    }
}
