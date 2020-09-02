using Dapper;
using Dapper.FastCrud;
using Dapper.FastCrud.Configuration.StatementOptions.Builders;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Z.Dapper.Plus;

namespace FIL.Api.Core.Repositories
{
    public interface IOrmRepository<T, TV>
    {
        void Delete(T obj);

        T Save(T obj);

        IEnumerable<T> SaveAll(IEnumerable<T> obj);

        IEnumerable<T> DeleteAll(IEnumerable<T> obj);

        IEnumerable<T> ExecuteCommand(string sql, IEnumerable<T> obj);

        IEnumerable<T> GetAll(Func<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<TV>, IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<TV>> statementOptions = null);
    }

    public abstract class BaseOrmMappingRepository<T, TV> : BaseRepository<T>, IOrmRepository<T, TV>
        where T : class
        where TV : class

    {
        private readonly HashSet<string> _implementedInterfaces = new HashSet<string>(typeof(T).GetInterfaces().Select(i => i.Name));
        protected IDataSettings DataSettings { get; }

        protected BaseOrmMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
            DataSettings = dataSettings;
        }

        protected abstract TV ToDto(T obj);

        protected abstract IEnumerable<TV> ToDto(IEnumerable<T> obj);

        protected abstract T FromDto(TV obj);

        protected abstract IEnumerable<T> FromDto(IEnumerable<TV> obj);

        public virtual void Delete(T obj)
        {
            GetCurrentConnection().Delete(obj, s => s.AttachToTransaction(GetCurrentTransaction()));
        }

        public virtual T Save(T obj)
        {
            bool shouldInsert = CheckForInsert(obj);

            TV dto;
            var conn = GetCurrentConnection();
            if (shouldInsert)
            {
                if (_implementedInterfaces.Contains(nameof(IAuditable)))
                {
                    IAuditable auditableObj = obj as IAuditable;
                    auditableObj.CreatedUtc = DateTime.UtcNow;
                    auditableObj.CreatedBy = auditableObj.ModifiedBy;
                }
                else if (_implementedInterfaces.Contains(nameof(IAuditableDates)))
                {
                    IAuditableDates auditableObj = obj as IAuditableDates;
                    auditableObj.CreatedUtc = DateTime.UtcNow;
                }
                dto = ToDto(obj);
                conn.Insert(dto, s => s.AttachToTransaction(GetCurrentTransaction()));
            }
            else
            {
                if (_implementedInterfaces.Contains(nameof(IAuditable)))
                {
                    IAuditable auditableObj = obj as IAuditable;
                    auditableObj.UpdatedUtc = DateTime.UtcNow;
                    auditableObj.UpdatedBy = auditableObj.ModifiedBy;
                }
                else if (_implementedInterfaces.Contains(nameof(IAuditableDates)))
                {
                    IAuditableDates auditableObj = obj as IAuditableDates;
                    auditableObj.UpdatedUtc = DateTime.UtcNow;
                }
                dto = ToDto(obj);
                conn.Update(dto, s => s.AttachToTransaction(GetCurrentTransaction()));
            }
            return FromDto(dto);
        }

        public virtual IEnumerable<T> SaveAll(IEnumerable<T> obj)
        {
            var conn = GetCurrentConnection();
            bool shouldInsert = CheckForInsert(obj.FirstOrDefault());
            var dto = ToDto(obj);
            // TODO: Need better solution to handle the logic
            DapperPlusManager.Entity<T>().Table(obj.FirstOrDefault().GetType().Name + "s")
                                             .Identity(x => ((IId<int>)x).Id);
            if (shouldInsert)
            {
                conn.UseBulkOptions(options => options.InsertIfNotExists = true).Include(s => s.Transaction = GetCurrentTransaction()).BulkInsert(dto);
            }
            else
            {
                conn.UseBulkOptions(options => options.Transaction = GetCurrentTransaction()).BulkUpdate<TV>(dto);
            }
            return FromDto(dto);
        }

        public virtual IEnumerable<T> DeleteAll(IEnumerable<T> obj)
        {
            var conn = GetCurrentConnection();
            bool shouldInsert = CheckForInsert(obj.FirstOrDefault());
            var dto = ToDto(obj);
            // TODO: Need better solution to handle the logic
            DapperPlusManager.Entity<T>().Table(obj.FirstOrDefault().GetType().Name + "s")
                                             .Identity(x => ((IId<int>)x).Id);
            conn.UseBulkOptions(options => options.Transaction = GetCurrentTransaction()).BulkDelete(dto);
            return FromDto(dto);
        }

        public virtual IEnumerable<T> ExecuteCommand(string sqlScript, IEnumerable<T> obj)
        {
            var conn = GetCurrentConnection();
            var dto = ToDto(obj);
            conn.Execute(sqlScript, obj, GetCurrentTransaction());
            return FromDto(dto);
        }

        protected virtual bool CheckForInsert(T obj)
        {
            return ((IId<int>)obj).Id == 0;
        }

        protected T Get(T obj)
        {
            return FromDto(GetCurrentConnection().Get(ToDto(obj), s => s.AttachToTransaction(GetCurrentTransaction())));
        }

        public IEnumerable<T> GetAll(Func<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<TV>, IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<TV>> statementOptions = null)
        {
            if (statementOptions == null)
            {
                statementOptions = s => s;
            }

            var conn = GetCurrentConnection();
            var transaction = GetCurrentTransaction();
            return conn.Find<TV>(s => statementOptions(transaction != null ? s.AttachToTransaction(transaction) : s)).Select(FromDto);
        }
    }
}