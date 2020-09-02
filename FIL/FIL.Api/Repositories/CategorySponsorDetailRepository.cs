using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Models.TMS;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICategorySponsorDetailRepository : IOrmRepository<CategorySponsorDataModel, CategorySponsorDataModel>
    {
        CategorySponsorDataModel GetSponsorCategoryData(long eventTicketAttributeId);

        CategorySponsorDataModel GetVenueCategorySponsorDetail(long eventId, int venueId, long ticketCategoryId);
    }

    public class CategorySponsorDetailRepository : BaseOrmRepository<CategorySponsorDataModel>, ICategorySponsorDetailRepository
    {
        public CategorySponsorDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CategorySponsorDataModel GetSponsorCategoryData(long eventTicketAttributeId)
        {
            CategorySponsorDataModel categorySponsorDataModel = new CategorySponsorDataModel();
            var sponsorCategoryData = GetCurrentConnection().QueryMultiple("spGetCategorySponsorDetails", new { EventTicketAttributeId = eventTicketAttributeId }, commandType: CommandType.StoredProcedure);
            categorySponsorDataModel.SponsorModels = sponsorCategoryData.Read<SponsorModel>().ToList();
            categorySponsorDataModel.CategorySponsorDetailModel = sponsorCategoryData.Read<CategorySponsorDetailModel>().FirstOrDefault();
            return categorySponsorDataModel;
        }

        public CategorySponsorDataModel GetVenueCategorySponsorDetail(long eventId, int venueId, long ticketCategoryId)
        {
            CategorySponsorDataModel categorySponsorDataModel = new CategorySponsorDataModel();
            var sponsorCategoryData = GetCurrentConnection().QueryMultiple("spGetVenueCategorySponsorDetails",
                new
                {
                    EventId = eventId,
                    VenueId = venueId,
                    TicketCategoryId = ticketCategoryId
                }, commandType: CommandType.StoredProcedure);
            categorySponsorDataModel.SponsorModels = sponsorCategoryData.Read<SponsorModel>().ToList();
            categorySponsorDataModel.SponsorEventDetailModels = sponsorCategoryData.Read<SponsorEventDetailModel>().ToList();
            categorySponsorDataModel.CategorySponsorDetailModel = sponsorCategoryData.Read<CategorySponsorDetailModel>().FirstOrDefault();
            return categorySponsorDataModel;
        }
    }
}