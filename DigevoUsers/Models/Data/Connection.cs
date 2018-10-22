using DigevoUsers.Controllers.Libraries;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace DigevoUsers.Models.Data
{

    public class Connection {

        private CommonTasks common = new CommonTasks();
        private string server;
        private string database;
        private string user;
        private string pass;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Connection() { }


        /// <summary>
        /// Receive any SQL sentence and returns set of rows as a List(ObjectType)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL sentence</param>
        /// <returns>List<T></returns>
        protected List<T> Select<T>(string sql)
        {

            List<T> output = new List<T>();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand cmd = null;

            try
            {
                
                cmd = database.GetSqlStringCommand(sql);
  
                using(IDataReader dataReader = database.ExecuteReader(cmd))
                {
                    output = this._DataReaderMapToList<T>(dataReader);
                }

            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                if(cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }


            return output;

        }


        /// <summary>
        /// Execute call to store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spName">Store Procedure's name</param>
        /// <param name="attr">Set of parameters to be passed to store procedure</param>
        /// <returns>List<T></returns>
        protected List<T> SelectSP<T>(string spName, Dictionary<string, object> attr = null)
        {

            List<T> output = new List<T>();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand cmd = null;

            try
            {
                cmd = CreateDbCommand(database, spName, attr);

                using(IDataReader dataReader = database.ExecuteReader(cmd))
                {
                    output = this._DataReaderMapToList<T>(dataReader);
                }

            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                if(cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }


            return output;

        }


        protected int InsertSP(string spName, Dictionary<string, object> attr = null)
        {
            return _NonSelectSP("insert", spName, attr);
        }

        protected int UpdateSP(string spName, Dictionary<string, object> attr = null)
        {
            return _NonSelectSP("update", spName, attr);
        }


        /// <summary>
        /// Any action instead of SELECT
        /// </summary>
        /// <param name="action">inserte, delete or update</param>
        /// <param name="spName">Store procedure's name</param>
        /// <param name="attr">Fields (key/value) to be updated</param>
        /// <returns></returns>
        private int _NonSelectSP(string action, string spName, Dictionary<string, object> attr = null)
        {

            int output = 0;
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand cmd = null;

            try
            {
 
                cmd = CreateDbCommand(database, spName, attr);

                // Rows affected
                if(action == "insert")
                {
                    // Will be catch SCOPE_IDENTITY() into SP called
                    output = Convert.ToInt32(database.ExecuteScalar(cmd));
                }
                else
                {
                    // Result as row affected
                    output = Convert.ToInt32(database.ExecuteNonQuery(cmd));
                }
                    
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
            finally
            {
                if(cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }


            return output;

        }


        /// <summary>
        /// Create the database object, using the default database service.
        /// The default database service is determined through configuration.
        /// It will be used for transaction
        /// </summary>
        protected Database CreateDatabase()
        {
            try
            {
                return DatabaseFactory.CreateDatabase();
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Create Connection from Database object already created
        /// </summary>
        /// <param name="database">Database object</param>
        /// <returns>DbConnection object</returns>
        protected DbConnection CreateConnection(Database database)
        {
            try
            {
                return database.CreateConnection();
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Create Transaction in function of Connection already created
        /// </summary>
        /// <param name="conn">DbConnection object</param>
        /// <returns>DbTransaction object</returns>
        protected DbTransaction CreateTransaction(DbConnection conn)
        {
            try
            {
                conn.Open();
                return conn.BeginTransaction();
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Close Connection received as parameter
        /// </summary>
        /// <param name="conn">DbConnection which will be closed</param>
        /// <returns>DbTransaction object</returns>
        protected void CloseConnection(DbConnection conn)
        {
            if(_isConnOpened(conn))
            {
                conn.Close();
            }
        }


        /// <summary>
        /// Will create a DbCommand to be used in transaction. It won't be executed, only created!.
        /// </summary>
        /// <param name="database">Database object related to transaction</param>
        /// <param name="spName">Store procedure name</param>
        /// <param name="attr">Attributes for store procedure</param>
        /// <returns></returns>
        protected DbCommand CreateDbCommand(Database database, string spName, Dictionary<string, object> attr = null)
        {

            try
            {
                DbCommand cmd = null;

                cmd = database.GetStoredProcCommand(spName);

                // If attr has elements to pass as parameters to store procedure
                if(attr != null)
                {
                    if(attr.Count > 0)
                    {
                        foreach(KeyValuePair<string, object> entry in attr)
                        {
                            string key = entry.Key;
                            var val = entry.Value;
                            if(val != null)
                            {
                                database.AddInParameter(cmd, entry.Key, common.Type2DbType(val.GetType()), val);
                            }
                            else
                            {
                                // If value is null, pass DBNull.Value instead
                                database.AddInParameter(cmd, entry.Key, DbType.String, DBNull.Value);
                            }
                            
                        }
                    }
                }

                return cmd;

            }
            catch(Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// Detect if connection is open or not
        /// </summary>
        /// <returns>True or false if connection is open</returns>
        private bool _isConnOpened(DbConnection conn)
        {
            if(conn != null)
            {
                if(conn.State.ToString().ToLower() == "open")
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Permite convertir cualquier SqlDataReader en una Lista genérica mediante Reflection
        /// </summary>
        /// <typeparam name="T">Tipo de objeto del que será la lista retornada</typeparam>
        /// <param name="dr">Objeto SqlDataReader (resultado de la consulta)</param>
        /// <returns></returns>
        private List<T> _DataReaderMapToList<T>(IDataReader dr) {
            List<T> list = new List<T>();
            T obj = default(T);
            while(dr.Read()) {
                obj = Activator.CreateInstance<T>();
                foreach(PropertyInfo prop in obj.GetType().GetProperties()) {
                    if(!object.Equals(dr[prop.Name], DBNull.Value)) {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                    
                }
                list.Add(obj);
            }
            return list;
        }

      
    }
}