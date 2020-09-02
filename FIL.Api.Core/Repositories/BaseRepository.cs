using Dapper;
using FIL.Api.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#pragma warning disable 693

namespace FIL.Api.Core.Repositories
{
    public class BaseRepository<T> where T : class
    {
        protected readonly IDataSettings _dataSettings;

        public BaseRepository(IDataSettings dataSettings)
        {
            _dataSettings = dataSettings;
        }

        protected DbConnection GetCurrentConnection()
        {
            return _dataSettings.UnitOfWork.GetDbConnection();
        }

        protected DbTransaction GetCurrentTransaction()
        {
            return _dataSettings.UnitOfWork.GetDbTransaction();
        }

        protected IEnumerable<T> ExecuteStoredProcedure<T>(string storedProcedureName, object param)
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.Query<T>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: GetCurrentTransaction());
            }
        }

        protected int ExecuteStoredProcedure(string storedProcedureName, object param)
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.Execute(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: GetCurrentTransaction());
            }
        }

        protected int ExecuteStoredProcedure(string storedProcedureName, DynamicParameters p)
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.Execute(storedProcedureName, p, commandType: CommandType.StoredProcedure, transaction: GetCurrentTransaction());
            }
        }

        protected object ExecuteScalar(string storedProcedureName, object param)
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.ExecuteScalar(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: GetCurrentTransaction());
            }
        }

        protected T ExecuteScalar<T>(string storedProcedureName, object param)
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.ExecuteScalar<T>(storedProcedureName, param, commandType: CommandType.StoredProcedure, transaction: GetCurrentTransaction());
            }
        }

        protected IEnumerable<T> ExecuteStoredProcedure<T1, T2, T>(string storedProcedureName, Func<T1, T2, T> map, object param, string splitOn = "Id")
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.Query(
                    storedProcedureName,
                    map,
                    param,
                    splitOn: splitOn,
                    commandType: CommandType.StoredProcedure,
                    transaction: GetCurrentTransaction());
            }
        }

        // I couldn't come up with a good way to not repeat myself; these are just variations of ExecuteStoredProcedure with more types.

        #region Extra Type Parameters

        protected IEnumerable<T> ExecuteStoredProcedure<T1, T2, T3, T>(string storedProcedureName, Func<T1, T2, T3, T> map, object param, string splitOn = "Id")
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.Query(
                    storedProcedureName,
                    map,
                    param,
                    splitOn: splitOn,
                    commandType: CommandType.StoredProcedure,
                    transaction: GetCurrentTransaction());
            }
        }

        protected IEnumerable<T> ExecuteStoredProcedure<T1, T2, T3, T4, T>(string storedProcedureName, Func<T1, T2, T3, T4, T> map, object param, string splitOn = "Id")
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.Query(
                    storedProcedureName,
                    map,
                    param,
                    splitOn: splitOn,
                    commandType: CommandType.StoredProcedure,
                    transaction: GetCurrentTransaction());
            }
        }

        protected IEnumerable<T> ExecuteStoredProcedure<T1, T2, T3, T4, T5, T>(string storedProcedureName, Func<T1, T2, T3, T4, T5, T> map, object param, string splitOn = "Id")
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.Query(
                    storedProcedureName,
                    map,
                    param,
                    splitOn: splitOn,
                    commandType: CommandType.StoredProcedure,
                    transaction: GetCurrentTransaction());
            }
        }

        protected IEnumerable<T> ExecuteStoredProcedure<T1, T2, T3, T4, T5, T6, T>(string storedProcedureName, Func<T1, T2, T3, T4, T5, T6, T> map, object param, string splitOn = "Id")
        {
            using (var conn = GetCurrentConnection())
            {
                return conn.Query(
                    storedProcedureName,
                    map,
                    param,
                    splitOn: splitOn,
                    commandType: CommandType.StoredProcedure,
                    transaction: GetCurrentTransaction());
            }
        }

        #endregion Extra Type Parameters
    }
}