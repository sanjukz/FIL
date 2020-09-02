using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IMatchSeatTicketDetailRepository : IOrmRepository<MatchSeatTicketDetail, MatchSeatTicketDetail>
    {
        MatchSeatTicketDetail Get(long id);

        IEnumerable<MatchSeatTicketDetail> GetByIds(IEnumerable<long> ids);

        IEnumerable<MatchSeatTicketDetail> GetbyMatchSeatTicketDetailId(IEnumerable<long> matchSeatTicketDetailIds);

        MatchSeatTicketDetail GetByEventTicketDetailsId(long eventTicketDetailId);

        MatchSeatTicketDetail GetByTransactionId(long transactionId);

        IEnumerable<MatchSeatTicketDetail> GetByEventTicketDetailId(long eventTicketDetailId);

        IEnumerable<MatchSeatTicketDetail> GetAllTopByEventTicketDetailId(long eventTicketDetailId, int index);

        IEnumerable<MatchSeatTicketDetail> GetbyTransactionId(long transactionId);

        MatchSeatTicketDetail GetByBarcodeNumber(string barcodenumber);

        MatchSeatTicketDetail GetByAltId(Guid altId);

        IEnumerable<MatchSeatTicketDetail> GetBySponserId(long id);

        IEnumerable<MatchSeatTicketDetail> GetBySponserIds(List<long> id);

        MatchSeatTicketDetail GetByBarcodeNumberAndEventTicketDetailIds(string barcodenumber, IEnumerable<long> eventTicketDetailIds);

        IEnumerable<MatchSeatTicketDetail> GetByTransactionIdAndTicketDetailIds(long transactionId, IEnumerable<long> eventTicketDetailIds);

        IEnumerable<MatchSeatTicketDetail> GetbyTranscationSeatTicketDetailId(IEnumerable<long> transactionSeatTicketDetailIds);

        MatchSeatTicketDetail GetByTransactionIdAndBarcodeId(long transactionId, string barcodeId);

        IEnumerable<MatchSeatTicketDetail> GetByTransactionIdAndTicketDetailId(long transactionId, long eventTicketDetailId);

        IEnumerable<MatchSeatTicketDetail> GetNotPrintedTicketByTransactionId(long transactionId);

        MatchSeatTicketDetail GetByMatchLayoutSectionSeatId(long matchLayoutSectionSeatId);

        IEnumerable<MatchSeatTicketDetail> GetBySponsorIdAndEventTicketDetailIds(long sponsorId, IEnumerable<long> eventTicketDetailIds);

        IEnumerable<MatchSeatTicketDetail> GetByTransactionIdAndMatchSeatTicketDetailId(long transactionId, long matchSeatTicketDetailId);

        IEnumerable<MatchSeatTicketDetail> GetByBarcodeList(IEnumerable<string> barcodenumbers);

        MatchSeatTicketDetail GetByMatchLayoutSectionSeatIdAndEventTicketDetailId(long matchLayoutSectionSeatId, long eventTicketDetailId);

        IEnumerable<MatchSeatTicketDetail> GetByMatchSeatTicketDetailIds(IEnumerable<long> MatchSeatTicketDetailIds);
    }

    public class MatchSeatTicketDetailRepository : BaseLongOrmRepository<MatchSeatTicketDetail>, IMatchSeatTicketDetailRepository
    {
        public MatchSeatTicketDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public MatchSeatTicketDetail Get(long id)
        {
            return Get(new MatchSeatTicketDetail { Id = id });
        }

        public IEnumerable<MatchSeatTicketDetail> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<MatchSeatTicketDetail> GetByIds(IEnumerable<long> ids)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.Id):C}IN @Ids")
                 .WithParameters(new { Ids = ids })
             );
        }

        public void DeleteMatchSeatTicketDetail(MatchSeatTicketDetail matchSeatTicketDetail)
        {
            Delete(matchSeatTicketDetail);
        }

        public MatchSeatTicketDetail SaveMatchSeatTicketDetail(MatchSeatTicketDetail matchSeatTicketDetail)
        {
            return Save(matchSeatTicketDetail);
        }

        public IEnumerable<MatchSeatTicketDetail> GetbyMatchSeatTicketDetailId(IEnumerable<long> matchSeatTicketDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.Id):C}IN @MatchSeatTicketDetailIds")
                .WithParameters(new { MatchSeatTicketDetailIds = matchSeatTicketDetailIds })
            );
        }

        public MatchSeatTicketDetail GetByEventTicketDetailsId(long eventTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.EventTicketDetailId):C}= @EventTicketDetailId and { nameof(MatchSeatTicketDetail.SeatStatusId):C}=@SeatStatusId ")
            .WithParameters(new { EventTicketDetailId = eventTicketDetailId, SeatStatusId = 1 })).FirstOrDefault();
        }

        public IEnumerable<MatchSeatTicketDetail> GetAllTopByEventTicketDetailId(long eventTicketDetailId, int index)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.EventTicketDetailId):C} = @EventTicketDetailId and { nameof(MatchSeatTicketDetail.SeatStatusId):C}=@SeatStatusId ")
            .WithParameters(new { EventTicketDetailId = eventTicketDetailId, SeatStatusId = 1 })).Take(index);
        }

        public MatchSeatTicketDetail GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.TransactionId):C}= @TransactionId ")
                        .WithParameters(new { TransactionId = transactionId })).FirstOrDefault();
        }

        public MatchSeatTicketDetail GetByTransactionIdAndBarcodeId(long transactionId, string barcodeId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.TransactionId):C}= @TransactionId AND {nameof(MatchSeatTicketDetail.BarcodeNumber):C} = @BarcodeNumber")
                        .WithParameters(new { TransactionId = transactionId, BarcodeNumber = barcodeId })).FirstOrDefault();
        }

        public IEnumerable<MatchSeatTicketDetail> GetByEventTicketDetailId(long eventTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.EventTicketDetailId):C} = @EventTicketDetailId")
                .WithParameters(new { EventTicketDetailId = eventTicketDetailId })
            );
        }

        public IEnumerable<MatchSeatTicketDetail> GetbyTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.TransactionId):C} = @Id")
                .WithParameters(new { Id = transactionId })
            );
        }

        public MatchSeatTicketDetail GetByBarcodeNumber(string barcodenumber)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.BarcodeNumber):C} = @BarcodeNumber")
                .WithParameters(new { BarcodeNumber = barcodenumber })
            ).FirstOrDefault();
        }

        public MatchSeatTicketDetail GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.AltId):C} = @AltId")
                    .WithParameters(new { AltId = altId })
           ).FirstOrDefault();
        }

        public IEnumerable<MatchSeatTicketDetail> GetBySponserId(long id)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.SponsorId):C} = @sponserId")
                    .WithParameters(new { sponserId = id })
           );
        }

        public IEnumerable<MatchSeatTicketDetail> GetBySponserIds(List<long> ids)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.SponsorId):C} IN @sponserId")
                    .WithParameters(new { sponserId = ids })
           );
        }

        public MatchSeatTicketDetail GetByMatchLayoutSectionSeatId(long matchLayoutSectionSeatId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.MatchLayoutSectionSeatId):C} = @MatchLayoutSectionSeatId")
                    .WithParameters(new { MatchLayoutSectionSeatId = matchLayoutSectionSeatId })
           ).FirstOrDefault();
        }

        public IEnumerable<MatchSeatTicketDetail> GetBySponsorIdAndEventTicketDetailIds(long sponsorId, IEnumerable<long> eventTicketDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.SponsorId):C} = @SponsorId AND {nameof(MatchSeatTicketDetail.EventTicketDetailId):C} IN @EventTicketDetailIds ")
                .WithParameters(new { SponsorId = sponsorId, EventTicketDetailIds = eventTicketDetailIds })
            );
        }

        public MatchSeatTicketDetail GetByBarcodeNumberAndEventTicketDetailIds(string barcodenumber, IEnumerable<long> eventTicketDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.BarcodeNumber):C} = @BarcodeNumber AND {nameof(MatchSeatTicketDetail.EventTicketDetailId):C} IN @EventTicketDetailIds ")
                .WithParameters(new { BarcodeNumber = barcodenumber, EventTicketDetailIds = eventTicketDetailIds })
            ).FirstOrDefault();
        }

        public IEnumerable<MatchSeatTicketDetail> GetByTransactionIdAndTicketDetailIds(long transactionId, IEnumerable<long> eventTicketDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.TransactionId):C} = @TransactionId AND {nameof(MatchSeatTicketDetail.EventTicketDetailId):C} IN @EventTicketDetailIds ")
                .WithParameters(new { TransactionId = transactionId, EventTicketDetailIds = eventTicketDetailIds })
            );
        }

        public IEnumerable<MatchSeatTicketDetail> GetbyTranscationSeatTicketDetailId(IEnumerable<long> transactionSeatTicketDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.Id):C} = @Id ")
                .WithParameters(new { Id = transactionSeatTicketDetailIds, })
            );
        }

        public IEnumerable<MatchSeatTicketDetail> GetByTransactionIdAndTicketDetailId(long transactionId, long eventTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.TransactionId):C} = @TransactionId AND {nameof(MatchSeatTicketDetail.EventTicketDetailId):C} = @EventTicketDetailId")
               .WithParameters(new { TransactionId = transactionId, EventTicketDetailId = eventTicketDetailId })
           );
        }

        public IEnumerable<MatchSeatTicketDetail> GetNotPrintedTicketByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.TransactionId):C} = @Id AND PrintStatusId<>2 ")
                .WithParameters(new { Id = transactionId })
            );
        }

        public IEnumerable<MatchSeatTicketDetail> GetByTransactionIdAndMatchSeatTicketDetailId(long transactionId, long matchSeatTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.TransactionId):C} = @TransactionId AND {nameof(MatchSeatTicketDetail.Id):C} = @MatchSeatTicketDetailId ")
                .WithParameters(new { TransactionId = transactionId, MatchSeatTicketDetailId = matchSeatTicketDetailId })
            );
        }

        public IEnumerable<MatchSeatTicketDetail> GetByBarcodeList(IEnumerable<string> barcodenumbers)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.BarcodeNumber):C} In @BarcodeNumber")
               .WithParameters(new { BarcodeNumber = barcodenumbers })
           );
        }

        public MatchSeatTicketDetail GetByMatchLayoutSectionSeatIdAndEventTicketDetailId(long matchLayoutSectionSeatId, long eventTicketDetailId)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.MatchLayoutSectionSeatId):C}= @MatchLayoutSectionSeatId and{nameof(MatchSeatTicketDetail.EventTicketDetailId):C}= @EventTicketDetailId")
            .WithParameters(new { MatchLayoutSectionSeatId = matchLayoutSectionSeatId, EventTicketDetailId = eventTicketDetailId })).FirstOrDefault();
        }

        public IEnumerable<MatchSeatTicketDetail> GetByMatchSeatTicketDetailIds(IEnumerable<long> matchSeatTicketDetailIds)
        {
            return GetAll(s => s.Where($"{nameof(MatchSeatTicketDetail.Id):C} In @MatchSeatTicketDetailIds and SeatStatusId=2")
            .WithParameters(new { MatchSeatTicketDetailIds = matchSeatTicketDetailIds }));
        }
    }
}