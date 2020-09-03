using FIL.Api.Core.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Api.Core;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace FIL.Api.Utilities
{
    public class DataSettings : IDataSettings
    {
        private readonly ISettings _settings;

        private SqlDatabase _transactionalDatabase;
        private SqlDatabase _readOnlyDatabase;

        public DataSettings(ISettings settings)
        {
            _settings = settings;
            UnitOfWork = new UnitOfWork(this);
        }

        public const string DollarConversionAPIEndPoint = "http://free.currconv.com/api/v7/convert?q=USD_";
        public const string DollarConversionAPIEndPointKey = "&compact=ultra&apiKey=5784faebf7325a3d17b7";

        public SqlDatabase TransactionalDatabase => _transactionalDatabase ?? (_transactionalDatabase = new SqlDatabase(TransactionalDatabaseConnectionString));

        private string TransactionalDatabaseConnectionString => _settings.GetConfigSetting<string>(SettingKeys.Api.Database.TransactionalConnectionString);

        public SqlDatabase ReadOnlyDatabase => _readOnlyDatabase ?? (_readOnlyDatabase = new SqlDatabase(ReadOnlyDatabaseConnectionString));

        private string ReadOnlyDatabaseConnectionString => _settings.GetConfigSetting<string>(SettingKeys.Api.Database.ReadOnlyConnectionString);

        public IUnitOfWork UnitOfWork { get; private set; }

        public void Dispose()
        {
            UnitOfWork = null;
        }
    }
}