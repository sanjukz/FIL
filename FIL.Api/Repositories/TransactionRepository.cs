using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.Zoom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITransactionRepository : IOrmRepository<Transaction, Transaction>
    {
        Transaction Get(long id);

        Transaction GetByTransactionId(long transactionId);

        IEnumerable<Transaction> GetAllByTransactionId(long transactionId);

        Transaction GetSuccessfulTransactionDetails(long transactionId);

        Transaction GetSuccessfulBOTransactionDetails(long transactionId);

        IEnumerable<Transaction> GetSuccessfullTransactionByPromoCode(string email);

        Transaction GetByEmail(string email);

        Transaction GetUnderPaymentTransactionDetails(long transactionId);

        Transaction GetByTransactionAltId(Guid transactionAltId);

        IEnumerable<Transaction> GetByTransactionIds(IEnumerable<long> transactionIds);

        IEnumerable<Transaction> GetReportTransactionsByIds(IEnumerable<long> transactionIds, DateTime fromDate, DateTime toDate);

        IEnumerable<Transaction> GetNonSuccessfullTransactionsByIds(IEnumerable<long> transactionIds, DateTime fromDate, DateTime toDate, int channelId);

        IEnumerable<Transaction> GetByAllTransactionIds(IEnumerable<long> transactionIds);

        IEnumerable<Transaction> GetByUserAltId(Guid useraltId);

        IEnumerable<Transaction> GetByEmailId(string email);

        IEnumerable<Transaction> GetByName(string name);

        IEnumerable<Transaction> GetByPhone(string phone);

        IEnumerable<Transaction> GetByAllAbandonedTransactions();

        IEnumerable<Transaction> GetSuccessFullTransactionByEmail(string email);

        IEnumerable<Transaction> GetSuccessFullTransactionByPhone(string phone);

        IEnumerable<Transaction> GetSuccessFullTransactionByEmailOrPhoneNumber(string email, string phone);

        IEnumerable<Transaction> GetReportTransactionsByDates(DateTime fromDate, DateTime toDate);

        IEnumerable<Transaction> GetTransactionsByUserAndDates(Guid userAltid, DateTime fromDate, DateTime toDate);

        Transaction GetByAltId(Guid altId);

        IEnumerable<Transaction> GetTransactionsByUserDatesAndStatus(Guid userAltid, DateTime fromDate, DateTime toDate, ReportExportStatus exportStatus);

        IEnumerable<Transaction> GetByTransactionIds(IEnumerable<long?> transactionIds);

        IEnumerable<FIL.Contracts.Models.TMS.CorporateOrderDetails> GetCorporateOrderDetails(long transactionId);

        IEnumerable<FIL.Contracts.Models.TMS.CorporateOrderDetails> GetCorporateConfirmationDetails(long transactionId);

        IEnumerable<FIL.Contracts.Models.TMS.TicketHandoverDetail> GetTicketHandoverDetails(long transactionId);

        IEnumerable<FIL.Contracts.Models.TMS.TicketDetailModel> GetBarcodeDetails(long transactionId);

        IEnumerable<Transaction> GetTransactionByPhoneNumber(string phoneNumber);

        IEnumerable<LiveOnlineTransactionDetailResponseModel> GetFeelOnlineDetails(long transactionId);

        long GetTransactionCountByEvent(long eventId);

        IEnumerable<FIL.Contracts.DataModels.Transaction> GetAllSuccessfulTransactionByReferralId(string referralId);
        IEnumerable<Transaction> GetSuccessFullTransactionByEmailIds(List<string> emails);
    }

    public class transactionRepository : BaseLongOrmRepository<Transaction>, ITransactionRepository
    {
        public transactionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Transaction Get(long id)
        {
            return Get(new Transaction { Id = id });
        }

        public IEnumerable<Transaction> GetAll()
        {
            return GetAll(null);
        }

        public Transaction SaveTransaction(Transaction transaction)
        {
            return Save(transaction);
        }

        public Transaction GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C} = @TransactionId AND {nameof(Transaction.TransactionStatusId):C} = @TransactionStatusId ")
            .WithParameters(new { TransactionId = transactionId, TransactionStatusId = 0 })
            ).FirstOrDefault();
        }

        public IEnumerable<Transaction> GetAllByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C} = @TransactionId AND {nameof(Transaction.TransactionStatusId):C} = @TransactionStatusId ")
            .WithParameters(new { TransactionId = transactionId, TransactionStatusId = 8 })
            );
        }

        public Transaction GetByTransactionAltId(Guid transactionAltId)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.AltId):C} = @TransactionAltId")
            .WithParameters(new { TransactionAltId = transactionAltId })
            ).FirstOrDefault();
        }

        public Transaction GetSuccessfulTransactionDetails(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C} = @TransactionId AND {nameof(Transaction.TransactionStatusId):C} = @TransactionStatusId ")
            .WithParameters(new { TransactionId = transactionId, TransactionStatusId = TransactionStatus.Success })
            ).FirstOrDefault();
        }

        public Transaction GetSuccessfulBOTransactionDetails(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C} = @TransactionId AND {nameof(Transaction.TransactionStatusId):C} = @TransactionStatusId AND {nameof(Transaction.ChannelId):C}=@ChannelId ")
            .WithParameters(new { TransactionId = transactionId, TransactionStatusId = TransactionStatus.Success, ChannelId = Channels.Boxoffice })
            ).FirstOrDefault();
        }

        public Transaction GetByEmail(string email)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.EmailId):C} = @Email")
                .WithParameters(new { Email = email })
                .OrderBy($"{nameof(Transaction.Id):C} DESC")
            ).FirstOrDefault();
        }

        public Transaction GetUnderPaymentTransactionDetails(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C}=@TransactionId AND {nameof(Transaction.TransactionStatusId):C} = @TransactionStatusId ")
            .WithParameters(new { TransactionId = transactionId, TransactionStatusId = TransactionStatus.UnderPayment })
            ).FirstOrDefault();
        }

        public IEnumerable<Transaction> GetByTransactionIds(IEnumerable<long> transactionIds)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C} IN @TransactionIds AND  {nameof(Transaction.TransactionStatusId):C} = 8")
                .WithParameters(new { TransactionIds = transactionIds })
            );
        }

        public IEnumerable<Transaction> GetSuccessFullTransactionByEmail(string email)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.EmailId):C} = @emailId AND  {nameof(Transaction.TransactionStatusId):C} = 8")
                .WithParameters(new { emailId = email })
            );
        }

        public IEnumerable<Transaction> GetSuccessFullTransactionByEmailIds(List<string> emails)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.EmailId):C} IN @emailId AND  {nameof(Transaction.TransactionStatusId):C} = 8")
                .WithParameters(new { emailId = emails })
            );
        }

        public IEnumerable<Transaction> GetSuccessfullTransactionByPromoCode(string promocode)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.DiscountCode):C} = @promo AND  {nameof(Transaction.TransactionStatusId):C} = 8")
                .WithParameters(new { promo = promocode })
            );
        }

        public IEnumerable<Transaction> GetSuccessFullTransactionByPhone(string phoneNumber)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.PhoneNumber):C} = @phoneNumber AND  {nameof(Transaction.TransactionStatusId):C} = 8")
                .WithParameters(new { phoneNumber = phoneNumber })
            );
        }

        public IEnumerable<Transaction> GetSuccessFullTransactionByEmailOrPhoneNumber(string email, string phoneNumber)
        {
            if (email != "" && phoneNumber != "")
            {
                return GetAll(s => s.Where($"({nameof(Transaction.EmailId):C} = @emailId OR {nameof(Transaction.PhoneNumber):C} = @phoneNumber) AND  {nameof(Transaction.TransactionStatusId):C} = 8")
                    .WithParameters(new { emailId = email, phoneNumber = phoneNumber })
                );
            }
            else if (email == "" && phoneNumber != "")
            {
                return GetAll(s => s.Where($" {nameof(Transaction.PhoneNumber):C} = @phone AND  {nameof(Transaction.TransactionStatusId):C} = 8")
                    .WithParameters(new { phone = phoneNumber })
                );
            }
            else
            {
                return GetAll(s => s.Where($"{nameof(Transaction.EmailId):C} = @emailId AND  {nameof(Transaction.TransactionStatusId):C} = 8")
                .WithParameters(new { emailId = email }));
            }
        }

        public IEnumerable<Transaction> GetTransactionByPhoneNumber(string phoneNumber)
        {
            return GetAll(s => s.Where($"({nameof(Transaction.PhoneNumber):C} = @phoneNumber)")
                     .WithParameters(new { phoneNumber = phoneNumber })
                 );
        }

        public IEnumerable<Transaction> GetReportTransactionsByIds(IEnumerable<long> transactionIds, DateTime fromDate, DateTime toDate)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C} IN @TransactionIds AND  {nameof(Transaction.TransactionStatusId):C} = 8 AND  {nameof(Transaction.CreatedUtc):C} >= @FromDate AND  {nameof(Transaction.CreatedUtc):C} <= @ToDate")
                .WithParameters(new { TransactionIds = transactionIds, FromDate = fromDate, ToDate = toDate })
            );
        }

        public IEnumerable<Transaction> GetNonSuccessfullTransactionsByIds(IEnumerable<long> transactionIds, DateTime fromDate, DateTime toDate, int channelId)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C} IN @TransactionIds AND  {nameof(Transaction.TransactionStatusId):C} NOT IN (8) AND  {nameof(Transaction.CreatedUtc):C} >= @FromDate AND  {nameof(Transaction.CreatedUtc):C} <= @ToDate AND {nameof(Transaction.ChannelId):C} = @ChannelId")
                .WithParameters(new { TransactionIds = transactionIds, FromDate = fromDate, ToDate = toDate, ChannelId = channelId })
            );
        }

        public IEnumerable<Transaction> GetByAllTransactionIds(IEnumerable<long> transactionIds)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C} IN @TransactionIds")
                .WithParameters(new { TransactionIds = transactionIds })
            );
        }

        public IEnumerable<Transaction> GetByUserAltId(Guid useraltId)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.CreatedBy):C} = @CreatedById AND  {nameof(Transaction.TransactionStatusId):C} = 8")
            .WithParameters(new { CreatedById = useraltId }));
        }

        public IEnumerable<Transaction> GetByEmailId(string email)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.EmailId):C} like '%'+@EmailId+'%'")
            .WithParameters(new { EmailId = email }));
        }

        public IEnumerable<Transaction> GetByName(string name)
        {
            name = name.Trim();
            var firstName = name.Split(" ").Length > 1 ? name.Split(" ")[0] : "";
            var lastname = name.Split(" ").Length > 1 ? name.Split(" ")[1] : "";
            if (firstName == "" && lastname == "")
            {
                return GetAll(s => s.Where($"{nameof(Transaction.FirstName):C} like '%'+@FirstName+'%' OR {nameof(Transaction.LastName):C} like '%'+@LastName+'%' ")
            .WithParameters(new { FirstName = name, LastName = name })).Distinct();
            }
            else
            {
                return GetAll(s => s.Where($"{nameof(Transaction.FirstName):C} like '%'+@ByFirstName+'%' OR {nameof(Transaction.LastName):C} like '%'+@ByLastName+'%' ")
            .WithParameters(new { ByFirstName = firstName, ByLastName = lastname })).Distinct();
            }
        }

        public IEnumerable<Transaction> GetByPhone(string phone)
        {
            phone = phone.Trim();
            return GetAll(s => s.Where($"{nameof(Transaction.PhoneNumber):C} like '%'+@Phone+'%' ")
            .WithParameters(new { Phone = phone, })).Distinct();
        }

        public IEnumerable<Transaction> GetByAllAbandonedTransactions()
        {
            return GetAll(s => s.Where($"{nameof(Transaction.TransactionStatusId):C} <> 8 AND {nameof(Transaction.CreatedUtc):C} >= @FromDate AND {nameof(Transaction.CreatedUtc):C} <= @ToDate")
            .WithParameters(new { FromDate = DateTime.UtcNow.AddMinutes(-60), ToDate = DateTime.UtcNow.AddMinutes(-20) }));
        }

        public IEnumerable<Transaction> GetReportTransactionsByDates(DateTime fromDate, DateTime toDate)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.TransactionStatusId):C} = 8 AND  {nameof(Transaction.CreatedUtc):C} >= @FromDate AND  {nameof(Transaction.CreatedUtc):C} <= @ToDate")
                .WithParameters(new { FromDate = fromDate, ToDate = toDate })
            );
        }

        public IEnumerable<Transaction> GetTransactionsByUserAndDates(Guid createdby, DateTime fromDate, DateTime toDate)
        {
            return GetAll(s => s.Where($" {nameof(Transaction.TransactionStatusId):C} = 8 AND  {nameof(Transaction.CreatedBy):C} = @Createdby  AND  {nameof(Transaction.CreatedUtc):C} >= @FromDate AND  {nameof(Transaction.CreatedUtc):C} <= @ToDate")
                .WithParameters(new { CreatedBy = createdby, FromDate = fromDate, ToDate = toDate })
            );
        }

        public Transaction GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.AltId):C} = @AltId")
           .WithParameters(new { AltId = altId })).FirstOrDefault();
        }

        public IEnumerable<Transaction> GetTransactionsByUserDatesAndStatus(Guid createdby, DateTime fromDate, DateTime toDate, ReportExportStatus exportStatus)
        {
            if (exportStatus != ReportExportStatus.None)
            {
                return GetAll(s => s.Where($" {nameof(Transaction.TransactionStatusId):C} = 8 AND  {nameof(Transaction.CreatedBy):C} = @Createdby  AND  {nameof(Transaction.CreatedUtc):C} >= @FromDate AND  {nameof(Transaction.CreatedUtc):C} <= @ToDate AND  {nameof(Transaction.ReportExportStatus):C} = @ExportStatus")
                    .WithParameters(new { CreatedBy = createdby, FromDate = fromDate, ToDate = toDate, ExportStatus = exportStatus })
                );
            }
            else
            {
                return GetAll(s => s.Where($" {nameof(Transaction.TransactionStatusId):C} = 8 AND  {nameof(Transaction.CreatedBy):C} = @Createdby  AND  {nameof(Transaction.CreatedUtc):C} >= @FromDate AND  {nameof(Transaction.CreatedUtc):C} <= @ToDate")
                    .WithParameters(new { CreatedBy = createdby, FromDate = fromDate, ToDate = toDate, ExportStatus = exportStatus })
                );
            }
        }

        public IEnumerable<Transaction> GetByTransactionIds(IEnumerable<long?> transactionIds)
        {
            return GetAll(s => s.Where($"{nameof(Transaction.Id):C} IN @TransactionIds")
                .WithParameters(new { TransactionIds = transactionIds })
            );
        }

        public IEnumerable<FIL.Contracts.Models.TMS.CorporateOrderDetails> GetCorporateOrderDetails(long transactionId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.CorporateOrderDetails>("SELECT DISTINCT T.Id As TransactionId ,E.Name AS EventName, " +
                                                 "ED.Name AS EventDetailName,ETA.Id AS EventTicketAttributeId,V.Name As VenueName,C.Name AS CityName, " +
                                                 "CONVERT(VARCHAR(17), ED.StartDateTime, 113) AS EventStartDate, " +
                                                 "CASE WHEN TransactingOptionId = 4 THEN 'Paid' ELSE 'Complimentary' END AS TransactionType,  " +
                                                 "V.Id AS VenueId, CONVERT(VARCHAR(5), CAST(StartDateTime AS TIME)) AS EventStartTime,  " +
                                                 "TD.ServiceCharge, TD.ConvenienceCharges, S.SponsorName AS CompanyName, PaymentDetail AS PaymentType,  " +
                                                 "S.PhoneCode, S.PhoneNumber, T.CreatedUtc, TD.TotalTickets, S.FirstName,S.LastName,S.IdType,S.IdNumber,  " +
                                                 "S.Email,TC.Name AS TicketCategoryName,CT.Code AS CurrencyName,TD.DiscountAmount,  " +
                                                 "T.NetTicketAmount, T.CurrencyId,TD.PricePerTicket  " +
                                                 "FROM Transactions T WITH(NOLOCK) " +
                                                 "INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId  " +
                                                 "INNER JOIN TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id = TPD.TransactionId  " +
                                                 "INNER JOIN CorporateTransactionDetails CTD WITH(NOLOCK) On T.Id = CTD.TransactionId  " +
                                                 "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON TD.EventTicketAttributeId = ETA.Id  " +
                                                 "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON T.CurrencyId = CT.Id  " +
                                                 "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id   " +
                                                 "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id  " +
                                                 "INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id  " +
                                                 "INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id  " +
                                                 "INNER JOIN Sponsors S WITH(NOLOCK) On CTD.SponsorId = S.Id  " +
                                                 "INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id  " +
                                                 "INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id  " +
                                                 "Where T.Id = @TransactionId", new
                                                 {
                                                     TransactionId = transactionId
                                                 });
        }

        public IEnumerable<FIL.Contracts.Models.TMS.CorporateOrderDetails> GetCorporateConfirmationDetails(long transactionId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.CorporateOrderDetails>("SELECT DISTINCT T.Id As TransactionId, E.Name AS EventName, " +
                                                 "ED.Name AS EventDetailName,ETA.Id AS EventTicketAttributeId,V.Name As VenueName,C.Name AS CityName, " +
                                                 "CONVERT(VARCHAR(17), ED.StartDateTime, 113) AS EventStartDate, " +
                                                 "CASE WHEN TransactingOptionId = 4 THEN 'Paid' ELSE 'Complimentary' END AS TransactionType,  " +
                                                 "V.Id AS VenueId, CONVERT(VARCHAR(5), CAST(StartDateTime AS TIME)) AS EventStartTime,  " +
                                                 "T.ServiceCharge, T.ConvenienceCharges, S.SponsorName AS CompanyName, PaymentDetail AS PaymentType,  " +
                                                 "S.PhoneCode, S.PhoneNumber, T.CreatedUtc, TD.TotalTickets, S.FirstName,S.LastName,S.IdType,S.IdNumber,  " +
                                                 "S.Email,TC.Name AS TicketCategoryName,CT.Code AS CurrencyName,T.DiscountAmount,  " +
                                                 "NetTicketAmount, T.CurrencyId,TD.PricePerTicket  " +
                                                 "FROM Transactions T WITH(NOLOCK) " +
                                                 "INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId  " +
                                                 "INNER JOIN TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id = TPD.TransactionId  " +
                                                 "INNER JOIN CorporateTransactionDetails CTD WITH(NOLOCK) On T.Id = CTD.TransactionId  " +
                                                 "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON TD.EventTicketAttributeId = ETA.Id  " +
                                                 "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON T.CurrencyId = CT.Id  " +
                                                 "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id   " +
                                                 "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id  " +
                                                 "INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id  " +
                                                 "INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id  " +
                                                 "INNER JOIN Sponsors S WITH(NOLOCK) On CTD.SponsorId = S.Id  " +
                                                 "INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id  " +
                                                 "INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id  " +
                                                 "Where T.TransactionStatusId = 8 AND  T.Id = @TransactionId", new
                                                 {
                                                     TransactionId = transactionId
                                                 });
        }

        public IEnumerable<FIL.Contracts.Models.TMS.TicketHandoverDetail> GetTicketHandoverDetails(long transactionId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.TicketHandoverDetail>("SELECT DISTINCT E.Name AS EventName,ED.Name AS EventDetailName,V.Name AS VenueName,C.Name AS CityName, " +
                                                "CONVERT(VARCHAR(12),ED.StartDateTime,113) AS EventStartDate," +
                                                " CONVERT(VARCHAR(5), CAST(StartDateTime AS TIME)) AS EventStartTime, " +
                                                "TD.TotalTickets,TC.Name AS TicketCategory,CT.Code AS CurrencyName,TD.PricePerTicket,TD.DiscountAmount,T.TotalTickets AS Quantity, " +
                                                "CASE WHEN TransactingOptionId =1 THEN 'Paid' ELSE 'Complimentary' END AS TransactionType, S.SponsorName, " +
                                                "ISNULL(S.FirstName,'')+' '+ISNULL(S.LastName,'') AS CustomerName, " +
                                                "ISNULL(S.Email, 'None') As Email, ISNULL(S.PhoneCode, '00') As PhoneCode, ISNULL(S.PhoneNumber, 0000000000) As PhoneNumber,T.CreatedUtc " +
                                                "FROM Transactions T WITH(NOLOCK) " +
                                                "INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id=TD.TransactionId " +
                                                "INNER JOIN TransactionPaymentDetails TPD WITH(NOLOCK) ON T.Id= TPD.TransactionId " +
                                                "INNER JOIN CorporateTransactionDetails CTD WITH(NOLOCK) On T.Id = CTD.TransactionId " +
                                                "INNER JOIN EventTicketAttributes EA WITH(NOLOCK) ON TD.EventTicketAttributeId= EA.Id " +
                                                "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON T.CurrencyId= CT.Id " +
                                                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON EA.EventTicketDetailId  = ETD.Id " +
                                                "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id " +
                                                "INNER JOIN EventDetails ED WITH(NOLOCK) On ETD.EventDetailId = ED.Id " +
                                                "INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id " +
                                                "INNER JOIN Sponsors S WITH(NOLOCK) On CTD.SponsorId = S.Id " +
                                                "INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id " +
                                                "INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id " +
                                                "AND T.Id = @TransactionId", new
                                                {
                                                    TransactionId = transactionId
                                                });
        }

        public IEnumerable<FIL.Contracts.Models.TMS.TicketDetailModel> GetBarcodeDetails(long transactionId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.TicketDetailModel>(" SELECT MSTD.Id, T.Id AS TransactionId, BarcodeNumber, " +
                "ED.name AS EventDetailName,TC.Name AS CategoryName,CONVERT(VARCHAR(20), StartDateTime, 107)  AS EventStartDate, " +
                "CONVERT(VARCHAR(5), CAST(StartDateTime AS TIME)) AS EventStartTime,V.Name AS VenueName, C.NAme As CityName, MLSS.SeatTag, " +
                "CONVERT(VARCHAR(20), EntryDateTime, 100) AS EntryDate " +
                "FROM Transactions T WITH(NOLOCK) " +
                "INNER JOIN TransactionDetails TD WITH(NOLOCK) ON T.Id = TD.TransactionId " +
                "INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON TD.EventTicketAttributeId = ETA.Id " +
                "INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id " +
                "INNER JOIN TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id " +
                "INNER JOIN EventDetails ED WITH(NOLOCK) ON ETD.EventDetailId = ED.Id " +
                "INNER JOIN EventAttributes EA WITH(NOLOCK) ON ED.Id = EA.EventDetailId " +
                "INNER JOIN Events E WITH(NOLOCK) ON ED.EventId = E.Id " +
                "INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id " +
                "INNER JOIN Cities C WITH(NOLOCK) ON V.CityId = C.Id " +
                "INNER JOIN CurrencyTypes CT WITH(NOLOCK) ON T.CurrencyId = CT.Id " +
                "INNER JOIN MatchSeatTicketDetails MSTD WITH(NOLOCK) ON T.Id = MSTD.TransactionId AND ETD.Id = MSTD.EventTicketDetailId " +
                "INNER JOIN MatchLayoutSectionSeats MLSS WITH(NOLOCK) ON MSTD.MatchLayoutSectionSeatId = MLSS.Id " +
                "INNER JOIN MatchLayoutSections MLS WITH(NOLOCK) ON MLSS.MatchLayoutSectionId = MLS.Id " +
                "INNER JOIN EntryGates EG WITH(NOLOCK) ON MLS.EntryGateId = EG.Id " +
                "WHERE T.Id =  @TransactionId " +
                "AND T.TransactionStatusId = 8 ORDER BY ED.StartDateTime, TC.Name ", new
                {
                    TransactionId = transactionId
                });
        }

        public IEnumerable<LiveOnlineTransactionDetailResponseModel> GetFeelOnlineDetails(long transactionId)
        {
            var LiveOnlineTransactionDetail = GetCurrentConnection().Query<LiveOnlineTransactionDetailResponseModel>("select distinct e.Id as EventId, ecm.EventcategoryId, e.Name, e.Description, ed.StartDateTime,ed.EndDateTime, td.VisitDate, td.VisitEndDate, td.TransactionType, dtm.StartTime, dtm.EndTime, tc.id as TicketCategoryId, t.createdBy as UserTransactionAltId, e.createdBy as CreatorAltId,t.channelId as Channel from transactions t with (NOLOCK) inner join TransactionDetails td with (NOLOCK) on td.TransactionId=t.id inner join EventTicketAttributes eta with (NOLOCK) on td.EventTicketAttributeId = eta.Id inner join EventTicketDetails etd with (NOLOCK) on eta.EventTicketDetailId = etd.Id inner join EventDetails ed with (NOLOCK) on etd.EventDetailId = ed.Id inner join events e with (NOLOCK) on e.Id = ed.eventid inner join TicketCategories tc with (NOLOCK) on tc.Id = etd.TicketCategoryId left outer join eventcategorymappings ecm with (NOLOCK) on e.Id = ecm.eventid left outer join placeweekopendays pwod with (NOLOCK) on e.Id = pwod.eventid left outer join daytimemappings dtm with (NOLOCK) on pwod.id = dtm.placeweekopendayId where t.id=" + transactionId.ToString(), null, GetCurrentTransaction());
            return LiveOnlineTransactionDetail;
        }

        public long GetTransactionCountByEvent(long eventId)
        {
            return GetCurrentConnection().Query<long>("select count(*) from transactions t with (NOLOCK) inner join TransactionDetails td with (NOLOCK) on td.transactionid=t.id inner join eventticketattributes eta with (NOLOCK) on eta.id=td.eventticketattributeid inner join eventticketdetails etd with (NOLOCK) on etd.Id=eta.eventticketdetailid inner join eventdetails ed with (NOLOCK) on ed.id=etd.eventdetailid inner join events e with (NOLOCK) on e.id=ed.eventid where t.transactionstatusid=8 and e.id=" + eventId.ToString(), null, GetCurrentTransaction()).FirstOrDefault();
        }

        public IEnumerable<FIL.Contracts.DataModels.Transaction> GetAllSuccessfulTransactionByReferralId(string referralId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.DataModels.Transaction>("select t.Id from transactions t WITH(NOLOCK) " +
                "Inner join transactiondetails td WITH(NOLOCK) on td.transactionId = t.id " +
                "Inner join referrals rf WITH(NOLOCK) on rf.id = td.referralId " +
                "WHERE t.transactionstatusid = 8 AND rf.slug = '" + referralId + "'", null, GetCurrentTransaction());
        }
    }
}