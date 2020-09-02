using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IActivityRepository : IOrmRepository<Activity, Activity>
    {
        Activity Get(int id);
    }

    public class ActivityRepository : BaseOrmRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Activity Get(int id)
        {
            return Get(new Activity { Id = id });
        }

        public IEnumerable<Activity> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteActivity(Activity activity)
        {
            Delete(activity);
        }

        public Activity SaveActivity(Activity activity)
        {
            return Save(activity);
        }
    }
}