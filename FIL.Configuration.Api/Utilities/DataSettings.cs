using FIL.Api.Core.Utilities;
using FIL.Api.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace FIL.Configuration.Api.Utilities
{
    public class DataSettings : IDataSettings
    {
        private readonly IConfiguration _configurationRoot;
        private SqlDatabase _database;

        public DataSettings(IConfiguration configurationRoot)
        {
            _configurationRoot = configurationRoot;
            UnitOfWork = new UnitOfWork(this);
        }

        private string DatabaseConnectionString => _configurationRoot[Constants.DatabaseConnectionStringEnvironmentVariable]
                                                    ?? _configurationRoot[Constants.DatabaseConnectionStringProperty];

        public SqlDatabase TransactionalDatabase => _database ?? (_database = new SqlDatabase(DatabaseConnectionString));

        public SqlDatabase ReadOnlyDatabase => _database ?? (_database = new SqlDatabase(DatabaseConnectionString));
        public IUnitOfWork UnitOfWork { get; private set; }

        public void Dispose()
        {
            UnitOfWork = null;
        }
    }
}