using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICorporateOrderRequestRepository : IOrmRepository<CorporateOrderRequest, CorporateOrderRequest>
    {
        CorporateOrderRequest Get(long id);

        IEnumerable<CorporateOrderRequest> GetAll();

        IEnumerable<FIL.Contracts.Models.TMS.SponsorOrderDetail> GetSponsorOrderDetails(IEnumerable<long> eventDetailIds);

        IEnumerable<FIL.Contracts.Models.TMS.Invoice.SponsorOrderDetailModel> GetSponsorOrderData(long venueId, long eventId, long sponsorId);

        IEnumerable<FIL.Contracts.Models.TMS.Invoice.InvoiceDetailModel> GetInvoiceData(long invoiceId);

        CorporateOrderRequest GetByEventTicketAttributeIdAndSponsorId(long EventTicketAttributeId, long SponsorId);
    }

    public class CorporateOrderRequestRepository : BaseLongOrmRepository<CorporateOrderRequest>, ICorporateOrderRequestRepository
    {
        public CorporateOrderRequestRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CorporateOrderRequest Get(long id)
        {
            return Get(new CorporateOrderRequest { Id = id });
        }

        public IEnumerable<CorporateOrderRequest> GetAll()
        {
            return GetAll(null);
        }

        public CorporateOrderRequest GetByEventTicketAttributeIdAndSponsorId(long eventTicketAttributeId, long sponsorId)
        {
            return GetAll(s => s.Where($"{nameof(CorporateOrderRequest.EventTicketAttributeId):C} = @EventTicketAttributeId AND  {nameof(CorporateOrderRequest.SponsorId):C} = @SponsorId ")
                    .WithParameters(new { EventTicketAttributeId = eventTicketAttributeId, SponsorId = sponsorId })
                    ).FirstOrDefault();
        }

        public IEnumerable<FIL.Contracts.Models.TMS.SponsorOrderDetail> GetSponsorOrderDetails(IEnumerable<long> eventDetailIds)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.SponsorOrderDetail>("SELECT COR.Id, AccountTypeId, OrderTypeId, ISNULL(S.SponsorName, '') AS SponsorName, " +
                "COR.FirstName + ' ' + COR.LastName AS[Name], COR.PhoneCode, COR.PhoneNumber, COR.Email, " +
                "E.Name As EventName, ED.Name AS EventDetailName, V.Name As VenueName, C.Name As CityName, " +
                "CONVERT(NVARCHAR(17), ED.StartDateTime, 113) AS EventStartDate, TC.Name AS TicketCategoryName, " +
                "COR.RequestedTickets, ISNULL(AllocatedTickets, 0) AS AllocatedTickets, ETA.LocalPrice,CT.Code AS CurrencyName,ETA.RemainingTicketForSale, " +
                "COR.EventTicketAttributeId, OrderStatusId, U.FirstName + ' ' + U.LastName as RequestedBy " +
                "FROM CorporateOrderRequests COR  WITH(NOLOCK) " +
                "INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON COR.EventTicketAttributeId = ETA.Id " +
                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id " +
                "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id " +
                "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON ETA.LocalCurrencyId = CT.Id " +
                "INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id " +
                "INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id " +
                "INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id " +
                "INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id " +
                "INNER JOIN Sponsors S WITH(NOLOCK) ON S.Id = COR.SponsorId " +
                "INNER JOIN Users U WITH(NOLOCK) ON U.AltId = COR.CreatedBy " +
                "WHERE ED.Id IN @EventDetailIds " +
                "ORDER BY OrderStatusId Desc ", new
                {
                    EventDetailIds = eventDetailIds,
                });
        }

        public IEnumerable<FIL.Contracts.Models.TMS.Invoice.SponsorOrderDetailModel> GetSponsorOrderData(long venueId, long eventId, long sponsorId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.Invoice.SponsorOrderDetailModel>("SELECT COR.Id,ED.Name As EventDetailName, TC.Name As TicketCategoryName,AllocatedTickets As Quantity, " +
                "ETA.LocalPrice As Price, CT.Code as CurrencyName,ISNULL([dbo].[fn_GetTicketFeeDetails](ETA.Id, 1), 0) AS ConvenienceCharge, " +
                "ISNULL([dbo].[fn_GetTicketFeeDetails] (ETA.Id,2),0) AS ServiceCharge,OrderStatusId " +
                "FROM CorporateOrderRequests COR WITH(NOLOCK) " +
                "INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON COR.EventTicketAttributeId = ETA.Id " +
                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id " +
                "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id " +
                "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON ETA.LocalCurrencyId = CT.Id " +
                "INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id " +
                "INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id " +
                "INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id " +
                "INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id " +
                "INNER JOIN Sponsors S WITH(NOLOCK) ON S.Id = COR.SponsorId " +
                "WHERE ED.Eventid= @Eventid AND ED.VenueId = @VenueId AND SponsorId = @SponsorId " +
                "AND COR.AllocatedTickets<>0 " +
                "ORDER BY ED.StartDateTime Desc", new
                {
                    Eventid = eventId,
                    VenueId = venueId,
                    SponsorId = sponsorId
                });
        }

        public IEnumerable<FIL.Contracts.Models.TMS.Invoice.InvoiceDetailModel> GetInvoiceData(long invoiceId)
        {
              return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.Invoice.InvoiceDetailModel>("SELECT CID.Id,CD.Name AS CompanyName, CD.Prefix AS InvoicePrefix, " +
                 "CONVERT(VARCHAR(12), CID.InvoiceDate, 113) AS InvoiceDate,S.SponsorName, COR.Email,  " +
                 "COR.PhoneCode, COR.PhoneNumber, ED.Name As EventDetailName,  " +
                 "CONVERT(VARCHAR(12), ED.StartDateTime, 113) AS EventDate, "+
                 "CONVERT(VARCHAR(5), CAST(StartDateTime AS TIME)) AS EventTime,TC.Name As TicketCategoryName,  " +
                 "COR.AllocatedTickets As Quantity, ETA.LocalPrice, CT.Name As CurrencyName, COIM.ConvenienceCharge, COIM.ServiceCharge  " +
                 "FROM CorporateInvoiceDetails CID WITH(NOLOCK)  " +
                 "INNER JOIN CompanyDetails CD WITH(NOLOCK) ON CID.CompanyDetailId = CD.Id  " +
                 "INNER JOIN CorporateOrderInvoiceMappings COIM WITH(NOLOCK) ON COIM.CorporateInvoiceDetailId = CID.Id  " +
                 "INNER JOIN CorporateOrderRequests COR WITH(NOLOCK) ON COIM.CorporateOrderRequestId = COR.Id  " +
                 "INNER JOIN  EventTicketAttributes ETA WITH(NOLOCK)  ON COR.EventTicketAttributeId = ETA.Id " +
                 "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id  " +
                 "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id  " +
                 "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON ETA.LocalCurrencyId = CT.Id  " +
                 "INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id  " +
                 "INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id  " +
                 "INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id  " +
                 "INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id  " +
                 "INNER JOIN Sponsors S WITH(NOLOCK) ON S.Id = COR.SponsorId  " +
                 "WHERE CID.Id = @InvoiceId " +
                 "ORDER BY ED.StartDateTime Desc", new
                  {
                   InvoiceId = invoiceId
                  });
          }
    }
}