using FIL.Api.Core.Utilities;
using FIL.Contracts.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Data;
using System.Data.Common;

namespace KZ.Api.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void BeginReadUncommittedTransaction();

        void Commit();

        void Rollback();

        DbConnection GetDbConnection();

        DbTransaction GetDbTransaction();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataSettings _dataSettings;

        protected DbConnection Connection;
        protected DbTransaction Transaction;

        public UnitOfWork(IDataSettings dataSettings)
        {
            _dataSettings = dataSettings;
        }

        public void BeginTransaction()
        {
            if (Transaction != null)
            {
                throw new CustomException("Transaction already open.");
            }

            if (Connection == null)
            {
                Connection = CreateConnection(_dataSettings.TransactionalDatabase);
            }
            CheckConnectionStatus(Connection);
            Transaction = Connection.BeginTransaction();
        }

        public void BeginReadUncommittedTransaction()
        {
            if (Transaction != null)
            {
                throw new CustomException("Transaction already open.");
            }

            if (Connection == null)
            {
                Connection = CreateConnection(_dataSettings.ReadOnlyDatabase);
            }
            CheckConnectionStatus(Connection);
            Transaction = Connection.BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        public void Commit()
        {
            Transaction?.Commit();
        }

        public void Rollback()
        {
            Transaction?.Rollback();
        }

        public DbConnection GetDbConnection()
        {
            Connection = Connection ?? CreateConnection(_dataSettings.ReadOnlyDatabase);
            CheckConnectionStatus(Connection);
            return Connection;
        }

        private void CheckConnectionStatus(DbConnection conn)
        {
            if (conn.State != ConnectionState.Open)
            {
                 conn.Open();
            }
        }

        private DbConnection CreateConnection(SqlDatabase database)
        {
            return database.CreateConnection();
        }

        public DbTransaction GetDbTransaction()
        {
            return Transaction;
        }

        public void Dispose()
        {
            Connection?.Close();
            Transaction = null;
            Connection = null;
        }
    }
}