using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IDynamicStadiumTicketCategoriesDetailsRepository : IOrmRepository<DynamicStadiumTicketCategoriesDetails, DynamicStadiumTicketCategoriesDetails>
    {
        DynamicStadiumTicketCategoriesDetails Get(int id);

        IEnumerable<DynamicStadiumTicketCategoriesDetails> GetByDynamicStadiumCoordinateId(IEnumerable<int> dynamicStadiumCoordinateId);
    }

    public class DynamicStadiumTicketCategoriesDetailsRepository : BaseOrmRepository<DynamicStadiumTicketCategoriesDetails>, IDynamicStadiumTicketCategoriesDetailsRepository
    {
        public DynamicStadiumTicketCategoriesDetailsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public DynamicStadiumTicketCategoriesDetails Get(int id)
        {
            return Get(new DynamicStadiumTicketCategoriesDetails { Id = id });
        }

        public IEnumerable<DynamicStadiumTicketCategoriesDetails> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteDynamicStadiumTicketCategoriesDetails(DynamicStadiumTicketCategoriesDetails dynamicStadiumTicketCategoriesDetails)
        {
            Delete(dynamicStadiumTicketCategoriesDetails);
        }

        public DynamicStadiumTicketCategoriesDetails SaveDynamicStadiumTicketCategoriesDetails(DynamicStadiumTicketCategoriesDetails dynamicStadiumTicketCategoriesDetails)
        {
            return Save(dynamicStadiumTicketCategoriesDetails);
        }

        public IEnumerable<DynamicStadiumTicketCategoriesDetails> GetByDynamicStadiumCoordinateId(IEnumerable<int> dynamicStadiumCoordinateId)
        {
            return (GetAll(s => s.Where($"{nameof(DynamicStadiumTicketCategoriesDetails.DynamicStadiumCoordinateId):C} IN @DynamicStadiumCoordinateId AND {nameof(DynamicStadiumTicketCategoriesDetails.IsEnabled):C}=@isEnabled")
                           .WithParameters(new { DynamicStadiumCoordinateId = dynamicStadiumCoordinateId, isEnabled = true })));
        }
    }
}