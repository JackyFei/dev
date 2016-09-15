using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Common.Data.SQL
{
    public interface IDataAccess
    {
        #region Connections
        string ConnectionString { get; }
        string ConnectionStringWithoutCredentials { get; }
        DbConnection CreateConnection();
        #endregion

        #region Parameters

        void AddInParameter(DbCommand command, string name, DbType dbType);
        void AddInParameter(DbCommand command, string name, DbType dbType, object value);
        void AddOutParameter(DbCommand command, string name, DbType dbType, int size);
        object GetParameterValue(DbCommand command, string name);
        void SetParameterValue(DbCommand command, string parameterName, object value);

        #endregion

        #region DbCommands

        DbCommand GetSqlStringCommand(string query);
        DbCommand GetStoredProcCommand(string storedProcedureName);
        DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues);

        #endregion

        #region ExecuteDataSet

        DataSet ExecuteDataSet(DbCommand command);
        DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues);
        DataSet ExecuteDataSet(CommandType commandType, string commandText);

        #endregion

        #region ExecuteDataTable

        DataTable ExecuteDataTable(DbCommand command);
        DataTable ExecuteDataTable(string storedProcedureName, params object[] parameterValues);
        DataTable ExecuteDataTable(CommandType commandType, string commandText);

        #endregion

        #region ExecuteNonQuery

        int ExecuteNonQuery(DbCommand command);
        int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues);
        int ExecuteNonQuery(CommandType commandType, string commandText);

        #endregion

        #region ExecuteReader

        IDataReader ExecuteReader(DbCommand command);
        IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues);
        IDataReader ExecuteReader(CommandType commandType, string commandText);

        #endregion

        #region ExecuteScalar

        object ExecuteScalar(DbCommand command);
        object ExecuteScalar(string storedProcedureName, params object[] parameterValues);
        object ExecuteScalar(CommandType commandType, string commandText);

        #endregion

        #region Transactions

        void ExecuteTransaction(Action action, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required, TransactionOptions transactionOptions = new TransactionOptions());

        T ExecuteTransaction<T>(Func<T> function, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required, TransactionOptions transactionOptions = new TransactionOptions());

        #endregion
    }
}
