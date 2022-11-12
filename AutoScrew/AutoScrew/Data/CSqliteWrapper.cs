using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SQLite;
using System.Data;

namespace AutoScrew
{
    class CSqliteWrapper
    {
        private SQLiteConnection m_Connection;
        private SQLiteCommand m_command;
        private string m_connectionStr;

        public CSqliteWrapper()
        {
        }

        public int OpenConn(string p_ssConnect)
        {
            int nRet = 0;
            m_connectionStr = p_ssConnect;
            m_Connection = new SQLiteConnection(m_connectionStr);

            if (this.m_Connection.State != ConnectionState.Open)
            {
                this.m_Connection.Open();
            }

            return nRet;
        }
        public void CloseConn()
        {
            if (this.m_Connection.State == ConnectionState.Open)
            {
                this.m_Connection.Close();
                if (this.m_Connection != null)
                this.m_Connection.Dispose();
            }
        }
        //执行SQL语句，返回数据到DataSet中
        public DataSet ReturnDataSet(string sql, string DataSetName)
        {
            DataSet dataSet = new DataSet();

            SQLiteDataAdapter SQLiteDA = new SQLiteDataAdapter(sql, m_Connection);
            SQLiteDA.Fill(dataSet, DataSetName);

            return dataSet;
        }
        //执行Sql语句,返回带分页功能的dataset
        public DataSet ReturnDataSet(string sql, int PageSize, int CurrPageIndex, string DataSetName)
        {
            DataSet dataSet = new DataSet();

            SQLiteDataAdapter SQLiteDA = new SQLiteDataAdapter(sql, m_Connection);
            SQLiteDA.Fill(dataSet, PageSize * (CurrPageIndex - 1), PageSize, DataSetName);

            return dataSet;
        }
        //返回 DataReader,用之前一定要先.read()打开,然后才能读到数据
        public SQLiteDataReader ReturnDataReader(String sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, m_Connection);

            SQLiteDataReader dataReader = command.ExecuteReader();

            return dataReader;
        }
        //执行SQL语句，返回记录总数数
        public int GetRecordCount(string sql)
        {
            int recordCount = 0;

            m_command = new SQLiteCommand(sql, m_Connection);
            SQLiteDataReader dataReader = m_command.ExecuteReader();
            while (dataReader.Read())
            {
                recordCount++;
            }
            dataReader.Close();

            return recordCount;
        }
        //取当前序列,条件为seq.nextval或seq.currval
        public decimal GetSeq(string seqstr)
        {
            decimal seqnum = 0;
            string sql = "select " + seqstr + " from dual";

            m_command = new SQLiteCommand(sql, m_Connection);
            SQLiteDataReader dataReader = m_command.ExecuteReader();
            if (dataReader.Read())
            {
                seqnum = decimal.Parse(dataReader[0].ToString());
            }
            dataReader.Close();

            return seqnum;
        }
        //执行SQL语句,返回所影响的行数
        public int ExecuteSQL(string sql)
        {
            int Cmd = 0;
            m_command = new SQLiteCommand(sql, m_Connection);
            try
            {
                Cmd = m_command.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                //    CloseConn();
            }
            return Cmd;
        }
        //对于 UPDATE、INSERT 和 DELETE 语句，返回值为该命令所影响的行数。对于其他所有类型的语句，返回值为 -1
        public int ExecuteNonQuery(string sql)
        {

            m_command = new SQLiteCommand(sql, m_Connection);
            int rv = -1;
            SQLiteTransaction sqliteTransaction = null;
            try
            {
                sqliteTransaction = m_Connection.BeginTransaction();
                m_command.Transaction = sqliteTransaction;
                rv = m_command.ExecuteNonQuery();
                sqliteTransaction.Commit();
            }
            catch (Exception ex)
            {
                sqliteTransaction.Rollback();
                rv = -1;
                throw ex;
            }
            return rv;
        }
        //执行查询，并将查询返回的结果集中第一行的第一列作为.NET数据类型返回。忽略额外的列或行。
        public object ExecuteScalar(string sql)
        {
            try
            {
                m_command = new SQLiteCommand(sql, m_Connection);
                return m_command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //执行单Sql语句查询，并将查询返回的结果作为一个数据集返回
        public DataSet RetriveDataSet(string sql)
        {
            if (sql == null || sql == string.Empty)
            {
                throw new Exception("参数为空...");
            }

            using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, m_Connection))
            {
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return ds;
            }
        }
        //执行Sql数组语句查询，并将查询返回的结果作为一个数据集返回
        public DataSet RetriveDataSet(string[] sqls, params string[] tableNames)
        {
            if (sqls == null || sqls.Length == 0)
            {
                throw new Exception("参数为空...");
            }
            int sqlLength;
            sqlLength = sqls.Length;

            DataSet ds = new DataSet();
            int tableNameLength = tableNames.Length;
            for (int i = 0; i < sqlLength; i++)
            {
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sqls[i], m_Connection))
                {
                    try
                    {
                        if (i < tableNameLength)
                            da.Fill(ds, tableNames[i]);
                        else
                            da.Fill(ds, "table" + i);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return ds;
        }
        /// 更新数据集. 
        /// 过程:客户层(dataSet.GetChanges()) -- 修改 --> 数据服务层(hasChangesDataSet.update()) -- 更新--> 数据层(hasChangesDataSet) ...
        ///  数据层(hasChangesDataSet) -- 新数据 --> 数据服务层 (hasChangesDataSet) -- 合并 -- > 客户层(dataSet.Merge(hasChangesDataSet))
        public DataSet UpdateDataSet(string sql, DataSet hasChangesDataSet)
        {
            if (sql == null || sql == string.Empty)
            {
                throw new Exception("参数为空...");
            }

            using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, m_Connection))
            {
                try
                {
                    SQLiteCommandBuilder cb = new SQLiteCommandBuilder(da);
                    da.Update(hasChangesDataSet);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return hasChangesDataSet;
            }
        }
        //将一组 UPDATE、INSERT 和 DELETE 语句以事务执行
        public bool ExecuteTransaction(string[] sqls)
        {
            if (sqls == null || sqls.Length == 0)
            {
                throw new Exception("参数为空...");
            }
            SQLiteTransaction sqliteTransaction = null;
            //OracleCommand command = new OracleCommand(sql, Connection);
            //OracleCommand command = null;
            try
            {

                m_command = m_Connection.CreateCommand();
                sqliteTransaction = m_Connection.BeginTransaction();
                m_command.Connection = m_Connection;
                m_command.Transaction = sqliteTransaction;

                for (int i = 0; i < sqls.Length; i++)
                {
                    m_command.CommandText = sqls[i];
                    m_command.ExecuteNonQuery();
                }
                sqliteTransaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (sqliteTransaction != null)
                {
                    sqliteTransaction.Rollback();
                }
                throw ex;
            }
        }
        //执行Sql数组语句查询，并将查询返回的结果作为一个数据读取器返回
        public SQLiteDataReader RetriveDataReader(string sql)
        {
            if (sql == null || sql == string.Empty)
            {
                throw new Exception("参数为空...");
            }
            //OpenConn();
            using (m_command = new SQLiteCommand(sql, m_Connection))
            {
                try
                {
                    SQLiteDataReader sqliteDataReader = m_command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    return sqliteDataReader;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //执行一个查询式的存贮过程,返回得到的数据集
        public DataSet ExecStoredProcedure(string proceName, object[] myParams)
        {
            if (proceName == null || proceName == string.Empty)
            {
                throw new Exception("参数为空...");
            }
            DataSet ds = new DataSet();
            try
            {
                //OpenConn();
                //OracleCommand oracleCommand = new OracleCommand(sql, Connection);
                //OracleCommand oracleCommand = null;
                m_command = m_Connection.CreateCommand();
                m_command.CommandType = CommandType.StoredProcedure;
                m_command.CommandText = proceName;
                if (myParams != null)
                {
                    for (int i = 0; i < myParams.Length; i++)
                        m_command.Parameters.Add((SQLiteParameter)myParams[i]);
                }
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(m_command))
                {
                    int returnValue = da.Fill(ds);
                    if (returnValue < 0)
                    {
                        ;
                        //                         throw new Exception("存储过程执行错误:" + (returnValue >= -14 ?
                        //                             ((StoreProcReturn)returnValue).ToString() : "ErrCode:" + returnValue));
                    }
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //执行一个非查询式的存贮过程
        public int ExecNonQueryStoredProcedure(string proceName, ref object[] myParams)
        {
            if (proceName == null || proceName == string.Empty)
            {
                throw new Exception("参数为空...");
            }
            try
            {
                //OpenConn();
                //OracleCommand oracleCommand = new OracleCommand(sql, Connection);
                //OracleCommand oracleCommand = null;
                m_command = m_Connection.CreateCommand();
                m_command.CommandType = CommandType.StoredProcedure;
                m_command.CommandText = proceName;
                if (myParams != null)
                {
                    for (int i = 0; i < myParams.Length; i++)
                    {
                        m_command.Parameters.Add((SQLiteParameter)myParams[i]);
                    }
                }
                int returnValue = m_command.ExecuteNonQuery();
                if (returnValue < 0)
                {
                    //throw new Exception("存储过程执行错误:" + (returnValue >= -14 ?
                    //((StoreProcReturn)returnValue).ToString() : "ErrCode:" + returnValue));
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        //--
        //--added on 2017-04-21
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        static string ms_ssConnect = "";

        static SQLiteConnection ms_objConnection = null;
        static ReaderWriterLockSlim ms_lockWR = new ReaderWriterLockSlim();

        public static void _S_Init(string p_ssConn)
        {
            ms_ssConnect = p_ssConn;
            _S_OpenConn();
        }
        public static void _S_Uninit()
        {
            _S_CloseConn();
        }

        private static int _S_OpenConn()
        {
            int nRet = 0;
            
            ms_objConnection = new SQLiteConnection(ms_ssConnect);

            if (ms_objConnection.State != ConnectionState.Open)
            {
                ms_objConnection.Open();
            }

            return nRet;
        }
        private static void _S_CloseConn()
        {
            if (ms_objConnection != null && ms_objConnection.State != ConnectionState.Closed)
            {
                ms_objConnection.Close();
                ms_objConnection.Dispose();
            }
        }

        public static void _S_Get_ConnObj(bool p_bWritable)
        {
            try
            {
                //加锁，直到释放前，其它线程无法得到conn 
                if (p_bWritable)
                    ms_lockWR.EnterWriteLock();
                else
                    ms_lockWR.EnterReadLock();
            }
            catch (Exception exp)
            {
                
            }
        }
        public static void _S_Release_ConnObj(bool p_bWritable)
        {
            try
            {
                //释放 
                if (p_bWritable)
                    ms_lockWR.ExitWriteLock();
                else
                    ms_lockWR.ExitReadLock();
            }
            catch (Exception exp)
            {
               
            }
        }

        #region 公用方法

        public static int _S_GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = _S_GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public static bool _S_Exists(string strSql)
        {
            object obj = _S_GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult == 0 ? false : true;
        }
        public static bool _S_Exists(string strSql, params SQLiteParameter[] cmdParms)
        {
            object obj = _S_GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult == 0 ? false : true;
        }

        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int _S_ExecuteSql(string SQLString)
        {
            int nRows = 0;
            SQLiteConnection objConn = ms_objConnection;//_S_Get_ConnObj(true);

            SQLiteCommand cmd = new SQLiteCommand(SQLString, objConn);

            try
            {
                nRows = cmd.ExecuteNonQuery();
                return nRows;
            }
            catch (System.Data.SQLite.SQLiteException E)
            {
                throw new Exception(E.Message);
            }
            finally
            {
                cmd.Dispose();
                //-_S_Release_ConnObj(true);
                //ATSLog.Log_Db("[_S_ExecuteSql]sqlstring....finally");
            }
            
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>    
        public static void _S_ExecuteSqlTran(List<string> SQLStringList)
        {
            SQLiteConnection objConn = ms_objConnection;//_S_Get_ConnObj(true);

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = objConn;
            SQLiteTransaction tx = objConn.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch (System.Data.SQLite.SQLiteException E)
            {
                tx.Rollback();
                throw new Exception(E.Message);
            }
            finally
            {
                cmd.Dispose();
                //-_S_Release_ConnObj(true);
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int _S_ExecuteSql(string SQLString, string content)
        {
            SQLiteConnection objConn = ms_objConnection;//_S_Get_ConnObj(true);

            SQLiteCommand cmd = new SQLiteCommand(SQLString, objConn);
            SQLiteParameter myParameter = new SQLiteParameter("@content", DbType.String);
            myParameter.Value = content;
            cmd.Parameters.Add(myParameter);
            try
            {
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (System.Data.SQLite.SQLiteException E)
            {
                throw new Exception(E.Message);
            }
            finally
            {
                cmd.Dispose();
                //-_S_Release_ConnObj(true);
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int _S_ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            SQLiteConnection objConn = ms_objConnection;//_S_Get_ConnObj(true);

            SQLiteCommand cmd = new SQLiteCommand(strSQL, objConn);
            SQLiteParameter myParameter = new SQLiteParameter("@fs", DbType.Binary);
            myParameter.Value = fs;
            cmd.Parameters.Add(myParameter);
            try
            {
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (System.Data.SQLite.SQLiteException E)
            {
                throw new Exception(E.Message);
            }
            finally
            {
                cmd.Dispose();
                //-_S_Release_ConnObj(true);
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object _S_GetSingle(string strSQL)
        {
            SQLiteConnection objConn = ms_objConnection;//_S_Get_ConnObj(false);

            SQLiteCommand cmd = new SQLiteCommand(strSQL, objConn);

            try
            {
                object obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                //-_S_Release_ConnObj(false);
            }
        }
        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public static SQLiteDataReader _S_ExecuteReader(string strSQL)
        {
            SQLiteConnection objConn = ms_objConnection;//_S_Get_ConnObj(false);

            SQLiteCommand cmd = new SQLiteCommand(strSQL, objConn);
            try
            {
                SQLiteDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                //-_S_Release_ConnObj(false);
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet _S_Query(string SQLString)
        {
            SQLiteConnection objConn = ms_objConnection;//_S_Get_ConnObj(false);
            try
            {
                SQLiteDataAdapter command = new SQLiteDataAdapter(SQLString, objConn);
                DataSet ds = new DataSet();
                command.Fill(ds, "ds");
                return ds;
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //-_S_Release_ConnObj(false);
            }
        }

        #endregion

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int _S_ExecuteSql(string SQLString, params SQLiteParameter[] cmdParms)
        {
            int nRows = 0;
            SQLiteConnection objConn = GlobalVar.ms_objConnection;//_S_Get_ConnObj(true);

            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                _S_PrepareCommand(cmd, objConn, null, SQLString, cmdParms);
                nRows = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return nRows;
            }
            catch (System.Data.SQLite.SQLiteException E)
            {
                throw new Exception(E.Message);
            }
            finally
            {
                //-_S_Release_ConnObj(true);
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SQLiteParameter[]）</param>
        public static void _S_ExecuteSqlTran(Dictionary<string, SQLiteParameter[]> SQLStringList)
        {
            SQLiteConnection objConn = GlobalVar.ms_objConnection;//_S_Get_ConnObj(true);

            using (SQLiteTransaction trans = objConn.BeginTransaction())
            {
                SQLiteCommand cmd = new SQLiteCommand();
                try
                {
                    //循环
                    foreach (var myDE in SQLStringList)
                    {
                        string cmdText = myDE.Key.ToString();
                        SQLiteParameter[] cmdParms = (SQLiteParameter[])myDE.Value;
                        _S_PrepareCommand(cmd, objConn, trans, cmdText, cmdParms);
                        int val = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    //-_S_Release_ConnObj(true);
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="p_ssSqlCmd">SQL语句</param>
        /// <param name="p_listCmdParms">SQL语句的参数向量表</param>
        public static void _S_ExecuteSqlTran(string p_ssSqlCmd, List<SQLiteParameter[]> p_listCmdParms)
        {
            SQLiteConnection objConn = GlobalVar.ms_objConnection;//_S_Get_ConnObj(true);

            SQLiteTransaction trans = objConn.BeginTransaction();
            try
            {
                SQLiteCommand cmd = new SQLiteCommand();

                //循环
                foreach (var myDE in p_listCmdParms)
                {
                    string cmdText = p_ssSqlCmd;
                    SQLiteParameter[] cmdParms = myDE;
                    _S_PrepareCommand(cmd, objConn, trans, cmdText, cmdParms);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                //-_S_Release_ConnObj(true);
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object _S_GetSingle(string SQLString, params SQLiteParameter[] cmdParms)
        {
            //SQLiteConnection connection = new SQLiteConnection(ms_ssConnect);
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                try
                {
                    SQLiteConnection objConn = GlobalVar.ms_objConnection;//_S_Get_ConnObj(false);

                    _S_PrepareCommand(cmd, objConn, null, SQLString, cmdParms);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SQLite.SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    //-_S_Release_ConnObj(false);
                    //ATSLog.Log_Db("[_S_GetSingle]...finally...");
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public static SQLiteDataReader _S_ExecuteReader(string SQLString, params SQLiteParameter[] cmdParms)
        {
            //SQLiteConnection connection = new SQLiteConnection(ms_ssConnect);
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                SQLiteConnection objConn = GlobalVar.ms_objConnection;//_S_Get_ConnObj(false);

                _S_PrepareCommand(cmd, objConn, null, SQLString, cmdParms);
                SQLiteDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                //-_S_Release_ConnObj(false);
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet _S_Query(string SQLString, params SQLiteParameter[] cmdParms)
        {

            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                SQLiteConnection objConn = GlobalVar.ms_objConnection;//_S_Get_ConnObj(false);

                _S_PrepareCommand(cmd, objConn, null, SQLString, cmdParms);

                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "ds");
                cmd.Parameters.Clear();
                return ds;
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //-_S_Release_ConnObj(false);
            }
        }


        private static void _S_PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion
        
    }//end class
}
