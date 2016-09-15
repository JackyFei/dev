using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Common.Data.SQL
{
    public class EnterpriseLibraryDataAccess : IDataAccess
    {
        #region Fields & Properties

        private readonly DatabaseProviderFactory _dbProviderFactory = new DatabaseProviderFactory();
        private Database DatabaseInstance { get; set; }

        #endregion

        #region Constructors

        public EnterpriseLibraryDataAccess(string name, bool testConnection = false)
        {
            try
            {
                DatabaseInstance = _dbProviderFactory.Create(name);

                if (testConnection)
                {
                    TestConnection(DatabaseInstance);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ISqlDataAccess

        #region Connections
        public string ConnectionString
        {
            get { return DatabaseInstance.ConnectionString; }
        }

        public string ConnectionStringWithoutCredentials
        {
            get { return DatabaseInstance.ConnectionStringWithoutCredentials; }
        }

        public DbConnection CreateConnection()
        {
            return DatabaseInstance.CreateConnection();
        }
        #endregion

        #region Parameters

        public void AddInParameter(DbCommand command, string name, DbType dbType)
        {
            DatabaseInstance.AddInParameter(command, name, dbType);
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            DatabaseInstance.AddInParameter(command, name, dbType, value);
        }

        public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
        {
            DatabaseInstance.AddOutParameter(command, name, dbType, size);
        }

        public object GetParameterValue(DbCommand command, string name)
        {
            return DatabaseInstance.GetParameterValue(command, name);
        }

        public void SetParameterValue(DbCommand command, string parameterName, object value)
        {
            DatabaseInstance.SetParameterValue(command, parameterName, value);
        }

        #endregion

        #region DbCommands
        public DbCommand GetSqlStringCommand(string query)
        {
            return DatabaseInstance.GetSqlStringCommand(query);
        }

        public DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            return DatabaseInstance.GetStoredProcCommand(storedProcedureName);
        }

        public DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
        {
            return DatabaseInstance.GetStoredProcCommand(storedProcedureName, parameterValues);
        }

        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataSet(DbCommand command)
        {
            return Invoke(() => DatabaseInstance.ExecuteDataSet(command));
        }

        public DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
        {
            return Invoke(() => DatabaseInstance.ExecuteDataSet(storedProcedureName, parameterValues));
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            return Invoke(() => DatabaseInstance.ExecuteDataSet(commandType, commandText));
        }
        #endregion

        #region ExecuteDataTable

        public DataTable ExecuteDataTable(DbCommand command)
        {
            return Invoke(() =>
            {
                var dataSet = DatabaseInstance.ExecuteDataSet(command);
                return GetFirstDataTableFromDataSet(dataSet);
            });
        }

        public DataTable ExecuteDataTable(string storedProcedureName, params object[] parameterValues)
        {
            return Invoke(() =>
            {
                var dataSet = DatabaseInstance.ExecuteDataSet(storedProcedureName, parameterValues);
                return GetFirstDataTableFromDataSet(dataSet);
            });
        }

        public DataTable ExecuteDataTable(CommandType commandType, string commandText)
        {
            return Invoke(() =>
            {
                var dataSet = DatabaseInstance.ExecuteDataSet(commandType, commandText);
                return GetFirstDataTableFromDataSet(dataSet);
            });
        }

        #endregion

        #region ExecuteNonQuery
        public int ExecuteNonQuery(DbCommand command)
        {
            return Invoke(() => DatabaseInstance.ExecuteNonQuery(command));
        }

        public int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
        {
            return Invoke(() => DatabaseInstance.ExecuteNonQuery(storedProcedureName, parameterValues));
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return Invoke(() => DatabaseInstance.ExecuteNonQuery(commandType, commandText));
        }

        #endregion

        #region ExecuteReader
        public IDataReader ExecuteReader(DbCommand command)
        {
            return Invoke(() => DatabaseInstance.ExecuteReader(command));
        }

        public IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues)
        {
            return Invoke(() => DatabaseInstance.ExecuteReader(storedProcedureName, parameterValues));
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return Invoke(() => DatabaseInstance.ExecuteReader(commandType, commandText));
        }
        #endregion

        #region ExecuteScalar
        public object ExecuteScalar(DbCommand command)
        {
            return Invoke(() => DatabaseInstance.ExecuteScalar(command));
        }

        public object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
        {
            return Invoke(() => DatabaseInstance.ExecuteScalar(storedProcedureName, parameterValues));
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            return Invoke(() => DatabaseInstance.ExecuteScalar(commandType, commandText));
        }
        #endregion

        #region Transactions
        public void ExecuteTransaction(Action action, TransactionScopeOption transactionScopeOption, TransactionOptions transactionOptions)
        {
            using (var transactionScope = new TransactionScope(transactionScopeOption, transactionOptions))
            {
                Execute(() =>
                {
                    action();
                    // ReSharper disable AccessToDisposedClosure
                    transactionScope.Complete();
                    // ReSharper restore AccessToDisposedClosure
                });
            }
        }

        public T ExecuteTransaction<T>(Func<T> function, TransactionScopeOption transactionScopeOption, TransactionOptions transactionOptions)
        {
            using (var transactionScope = new TransactionScope(transactionScopeOption, transactionOptions))
            {
                return Invoke(() =>
                {
                    var result = function.Invoke();
                    // ReSharper disable AccessToDisposedClosure
                    transactionScope.Complete();
                    // ReSharper restore AccessToDisposedClosure
                    return result;
                });
            }
        }
        #endregion

        #endregion

        #region Private Helper Methods

        private static void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (DbException dbException)
            {
                throw new SjkException(ErrorCodes.DataAccessError, Resources.ErrorCodes.GeneralDataAccessError, dbException);
            }
        }

        private static T Invoke<T>(Func<T> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (DbException dbException)
            {
                throw new SjkException(ErrorCodes.DataAccessError, Resources.ErrorCodes.GeneralDataAccessError,
                    dbException);
            }
        }

        private static void TestConnection(Database database)
        {
            using (var con = database.CreateConnection())
            {
                con.Open();
            }
        }

        private static DataTable GetFirstDataTableFromDataSet(DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count < 1) return null;
            return dataSet.Tables[0];
        }
        #endregion
    }


}

