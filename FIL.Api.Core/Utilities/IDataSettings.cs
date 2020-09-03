using FIL.Api.Core;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;

namespace FIL.Api.Core.Utilities
{
    public interface IDataSettings : IDisposable
    {
        SqlDatabase TransactionalDatabase { get; }
        SqlDatabase ReadOnlyDatabase { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}