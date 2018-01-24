using EntLibContrib.Data.OdpNet;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace WindowsFormsApp1
{
    public class DL
    {
        public string ConnectionString => ConfigurationManager.AppSettings["DBConnection"];
        private string procName = "MD_FIELD_GETALL";
        
        public Database ODbBase => m_dbBase;
        static protected Database m_dbBase;
        public DL()
        {
                m_dbBase = new OracleDatabase(ConnectionString);
        }


        public DataSet QueryList( string procName, Dictionary<string, object> dicParams)
        {
            DataSet dsAllData = new DataSet();
            DbCommand dbCommand;
            using ( DbConnection dbConnection = m_dbBase.CreateConnection() )
            {
                dbConnection.Open();
                dbCommand = TypedParameter(dicParams, procName);
                dbCommand.Connection = dbConnection;
            }

            dsAllData = m_dbBase.ExecuteDataSet(dbCommand);

            return dsAllData;
        }


        public DbCommand TypedParameter( Dictionary<string, object> paramsValue, string procName)
        {
            DbCommand dbCommand;
            dbCommand = m_dbBase.GetStoredProcCommand(procName);
            m_dbBase.DiscoverParameters(dbCommand);

            foreach ( DbParameter parameter in dbCommand.Parameters )
            {
                var parameterName = parameter.ParameterName.Remove(0, 1);

                if ( (parameter.Direction == ParameterDirection.Input) && (paramsValue.ContainsKey(parameterName)
                    || paramsValue.ContainsKey(parameterName.ToUpper()) || paramsValue.ContainsKey(parameterName.ToLower())) )
                {
                    parameter.Value = paramsValue[parameterName];
                }
            }


            return dbCommand;
        }

        public IEnumerable<T> GetData<T>(string procName, params object[] paramsValue) where T : new()
        {
            IEnumerable<T> allData;
            try
            {
                allData = QueryList<T>(procName, paramsValue);
            }
            catch ( Exception ex )
            {

                throw ex;
            }


            return allData;
        }

        public IEnumerable<T> QueryList<T>(string procName, params object[] paramsValue) where T : new()
        {
            IEnumerable<T> allData;
            OracleDatabase oracleDataBase;

            oracleDataBase = (OracleDatabase)m_dbBase;

            IResultSetMapper<T> resultSetMapper = new EntityMapper<T>();
            var accessor = new OracleSprocAccessor<T>(oracleDataBase, procName, resultSetMapper);

            allData = accessor.Execute(paramsValue);

            return allData;
        }
    }
}
