using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICorporateTicketAllocationDetailRepository : IOrmRepository<CorporateTicketAllocationDetail, CorporateTicketAllocationDetail>
    {
        CorporateTicketAllocationDetail Get(long id);

        IEnumerable<CorporateTicketAllocationDetail> GetByEventTicketAttributeId(long eventTicketAttributeId);

        IEnumerable<CorporateTicketAllocationDetail> GetByEventTicketAttributeIds(IEnumerable<long> eventTicketAttributeIds);

        CorporateTicketAllocationDetail GetByEventTicketAttributeIdandSponsorId(long eventTicketAttributeId, long sponsorId);

        CorporateTicketAllocationDetail GetByCorporateTicketAllocationDetailId(long corporateTicketAllocationDetailId);

        CorporateTicketAllocationDetail GetByEventTicketAttributeIdandSponsor(long eventTicketAttributeId, long sponsorId);

        IEnumerable<FIL.Contracts.Models.TMS.CorporateTicketAllocationDetail> GetCorporateDetails(long corporateTicketAllocationDetailId);

        IEnumerable<CorporateTicketAllocationDetail> GetByIds(IEnumerable<long> ids);

        IEnumerable<CorporateTicketAllocationDetail> GetByEventTicketAttributeIdsandSponsorId(IEnumerable<long> eventTicketAttributeIds, long sponsorId);

        IEnumerable<FIL.Contracts.Models.TMS.CorporateTicketAllocationDetail> GetCorporateDetails(long sponsorId, long ticketCategoryId, IEnumerable<long> eventDeatilIds);
    }

    public class CorporateTicketAllocationDetailRepository : BaseLongOrmRepository<CorporateTicketAllocationDetail>, ICorporateTicketAllocationDetailRepository
    {
        public CorporateTicketAllocationDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateTicketAllocationDetail Get(long id)
        {
            return (new CorporateTicketAllocationDetail { Id = id });
        }

        public IEnumerable<CorporateTicketAllocationDetail> GetByEventTicketAttributeId(long eventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTicketAllocationDetail.EventTicketAttributeId):c}=@EventTicketAttributeId")
            .WithParameters(new { EventTicketAttributeId = eventTicketAttributeId }));
        }

        public IEnumerable<CorporateTicketAllocationDetail> GetByEventTicketAttributeIds(IEnumerable<long> eventTicketAttributeIds)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTicketAllocationDetail.EventTicketAttributeId):c} IN @EventTicketAttributeIds")
            .WithParameters(new { EventTicketAttributeIds = eventTicketAttributeIds }));
        }

        public CorporateTicketAllocationDetail GetByEventTicketAttributeIdandSponsorId(long eventTicketAttributeId, long sponsorId)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTicketAllocationDetail.EventTicketAttributeId):c} = @EventTicketAttributeId AND {nameof(CorporateTicketAllocationDetail.SponsorId):c} = @SponsorId")
            .WithParameters(new { EventTicketAttributeId = eventTicketAttributeId, SponsorId = sponsorId })).FirstOrDefault();
        }

        public CorporateTicketAllocationDetail GetByCorporateTicketAllocationDetailId(long corporateTicketAllocationDetailId)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTicketAllocationDetail.Id):c} = @CorporateTicketAllocationDetailId ")
             .WithParameters(new { CorporateTicketAllocationDetailId = corporateTicketAllocationDetailId }))
             .FirstOrDefault();
        }

        public CorporateTicketAllocationDetail GetByEventTicketAttributeIdandSponsor(long eventTicketAttributeId, long sponsorId)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTicketAllocationDetail.EventTicketAttributeId):c} = @EventTicketAttributeId AND {nameof(CorporateTicketAllocationDetail.SponsorId):c} = @SponsorId")
            .WithParameters(new { EventTicketAttributeId = eventTicketAttributeId, SponsorId = sponsorId })).FirstOrDefault();
        }

        public IEnumerable<FIL.Contracts.Models.TMS.CorporateTicketAllocationDetail> GetCorporateDetails(long corporateTicketAllocationDetailId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.CorporateTicketAllocationDetail>("SELECT ISNULL(S.SponsorName,'None') as SponsorName, " +
                                "ISNULL(S.FirstName, 'None') as FirstName, " +
                                "ISNULL(S.LastName, 'None') as LastName, " +
                                "ISNULL(S.Email, 'None') as Email, " +
                                "ISNULL(S.PhoneCode, 'None') as PhoneCode, " +
                                "ISNULL(S.PhoneNumber, 'None') as PhoneNumber, " +
                                "ISNULL(S.IdType, 'None') as IdType, " +
                                "ISNULL(S.IdNumber, 'None') as IdNumber, " +
                                "CTAD.SponsorId, " +
                                "ED.Id AS EventDetailId, " +
                                "E.Name AS EventName, " +
                                "CONVERT(VARCHAR(12),ED.StartDateTime,113) AS EventStartDate," +
                                " CONVERT(VARCHAR(5), CAST(StartDateTime AS TIME)) AS EventStartTime, " +
                                "E.Id AS EventId, " +
                                "ED.Name AS EventDetailName, " +
                                "V.Name As VenueName, C.Name AS CityName, " +
                                "TC.Name AS TicketCategory, " +
                                "ETA.LocalPrice AS PricePerTicket,  " +
                                "ETA.Id AS EventTicketAttributeId, " +
                                "ISNULL([dbo].[fn_GetTicketFeeDetails](ETA.Id, 1), 0) AS ConvenceCharge, " +
                                "ISNULL([dbo].[fn_GetTicketFeeDetails] (ETA.Id,2),0) AS ServiceTax,  " +
                                "CT.Code AS CurrencyName, CT.Id AS CurrencyId    " +
                                "FROM Sponsors S WITH(NOLOCK)  " +
                                "INNER JOIN CorporateTicketAllocationDetails CTAD WITH(NOLOCK) ON S.Id=CTAD.SponsorId  " +
                                "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON CTAD.EventTicketAttributeId= ETA.Id  " +
                                "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON ETA.LocalCurrencyId= CT.Id  " +
                                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId  = ETD.Id " +
                                "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id " +
                                "INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id " +
                                "INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id " +
                                "INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id " +
                                "INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id " +
                                "WHERE CTAD.Id= @Id", new
                                {
                                    Id = corporateTicketAllocationDetailId
                                });
        }

        public IEnumerable<CorporateTicketAllocationDetail> GetByIds(IEnumerable<long> ids)
        {
            var CorporateTicketAllocationDetailList = GetAll(statement => statement
                                 .Where($"{nameof(CorporateTicketAllocationDetail.Id):C} IN @Ids")
                                 .WithParameters(new { Ids = ids }));
            return CorporateTicketAllocationDetailList;
        }

        public IEnumerable<CorporateTicketAllocationDetail> GetByEventTicketAttributeIdsandSponsorId(IEnumerable<long> eventTicketAttributeIds, long sponsorId)
        {
            return GetAll(s => s.Where($"{nameof(CorporateTicketAllocationDetail.EventTicketAttributeId):c} In @EventTicketAttributeIds AND {nameof(CorporateTicketAllocationDetail.SponsorId):c} = @SponsorId")
            .WithParameters(new { EventTicketAttributeIds = eventTicketAttributeIds, SponsorId = sponsorId }));
        }

        public IEnumerable<FIL.Contracts.Models.TMS.CorporateTicketAllocationDetail> GetCorporateDetails(long sponsorId, long ticketCategoryId, IEnumerable<long> eventDeatilIds)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.CorporateTicketAllocationDetail>("SELECT ISNULL(S.SponsorName,'None') as SponsorName, " +
                                "ISNULL(S.FirstName, 'None') as FirstName, ISNULL(S.LastName, 'None') as LastName, " +
                                "ISNULL(S.Email, 'None') as Email, ISNULL(S.PhoneCode, 'None') as PhoneCode, " +
                                "ISNULL(S.PhoneNumber, 'None') as PhoneNumber, ISNULL(S.IdType, 'None') as IdType, " +
                                "ISNULL(S.IdNumber, 'None') as IdNumber, CTAD.SponsorId, ED.Id AS EventDetailId, " +
                                "E.Name AS EventName,CONVERT(VARCHAR(12), ED.StartDateTime, 113) AS EventStartDate, " +
                                "CONVERT(VARCHAR(5), CAST(StartDateTime AS TIME)) AS EventStartTime," +
                                "E.Id AS EventId, ED.Name AS EventDetailName,V.Name As VenueName, C.Name AS CityName, " +
                                "TC.Name AS TicketCategory, ETA.LocalPrice AS PricePerTicket,ETA.Id AS EventTicketAttributeId, ISNULL(ETD.InventoryTypeId,0)As InventoryTypeId, " +
                                "ISNULL([dbo].[fn_GetTicketFeeDetails](ETA.Id, 1), 0) AS ConvenceCharge, " +
                                "ISNULL([dbo].[fn_GetTicketFeeDetails] (ETA.Id,2),0) AS ServiceTax,  " +
                                "CT.Code AS CurrencyName, CT.Id AS CurrencyId " +
                                "FROM Sponsors S WITH(NOLOCK)  " +
                                "INNER JOIN CorporateTicketAllocationDetails CTAD WITH(NOLOCK) ON S.Id=CTAD.SponsorId  " +
                                "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON CTAD.EventTicketAttributeId= ETA.Id  " +
                                "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON ETA.LocalCurrencyId= CT.Id  " +
                                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId  = ETD.Id " +
                                "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id " +
                                "INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id " +
                                "INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id " +
                                "INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id " +
                                "INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id " +
                                "WHERE S.Id=@SponsorId And TC.Id=@TicketCategoryId And  ED.Id IN @EventDeatilIds ", new
                                {
                                    SponsorId = sponsorId,
                                    TicketCategoryId = ticketCategoryId,
                                    EventDeatilIds = eventDeatilIds
                                });
        }
    }
}