using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITransactionDetailRepository : IOrmRepository<TransactionDetail, TransactionDetail>
    {
        TransactionDetail Get(long id);

        TransactionDetail GetByAltId(Guid altId);

        IEnumerable<TransactionDetail> GetByTransactionId(long transactionId);

        IEnumerable<TransactionDetail> GetByEventTicketAttributeAndTransactionId(IEnumerable<long> EventAttributeId, long transactionId);

        IEnumerable<TransactionDetail> GetByEventTicketAttributeIds(IEnumerable<long> EventTicketAttributeIds);

        IEnumerable<TransactionDetail> GetByTransactionIdsAndEventTicketAttributeIds(IEnumerable<long> transactionIds, IEnumerable<long> EventTicketAttributeIds);

        IEnumerable<TransactionDetail> GetByEventTicketAttributeandTransactionId(IEnumerable<long> eventTicketAttribute, long transactionId);

        IEnumerable<TransactionDetail> GetByTransactionIds(IEnumerable<long> transactionIds);

        TransactionDetail GetByTicketAttributeIdAndTransactionId(long transactionId, long eventTicketAttributeId);

        TransactionDetail GetByEventTicketAttributeIdAndTransactionIdAndTicketTypeId(long transactionId, long eventTicketAttributeId, short ticketTypeId);

        IEnumerable<TransactionDetail> GetByTransactionIdsAndEventTicketAttributeIdsAndTicketTypeId(IEnumerable<long> transactionIds, IEnumerable<long> EventTicketAttributeIds, IEnumerable<short> ticketTypeId);

        IEnumerable<TransactionDetail> GetAllByIds(IEnumerable<long> ids);

        IEnumerable<TransactionDetail> GetByEventTicketAttributeId(long EventTicketAttributeId);
    }

    public class TransactionDetailRepository : BaseLongOrmRepository<TransactionDetail>, ITransactionDetailRepository
    {
        public TransactionDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionDetail Get(long id)
        {
            return Get(new TransactionDetail { Id = id });
        }

        public TransactionDetail GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.AltId):C} = @AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public IEnumerable<TransactionDetail> GetAllByIds(IEnumerable<long> ids)
        {
            var transactionDetailsLists = GetAll(statement => statement
                                    .Where($"{nameof(TransactionDetail.Id):C} IN @Ids")
                                    .WithParameters(new { Ids = ids }));
            return transactionDetailsLists;
        }

        public IEnumerable<TransactionDetail> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<TransactionDetail> GetByTransactionId(long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.TransactionId):C} = @TransactionId AND TotalTickets > 0")
               .WithParameters(new { TransactionId = transactionId })
           );
        }

        public void DeleteTransactionDetail(TransactionDetail transactionDetail)
        {
            Delete(transactionDetail);
        }

        public TransactionDetail SaveTransactionDetail(TransactionDetail transactionDetail)
        {
            return Save(transactionDetail);
        }

        public IEnumerable<TransactionDetail> GetByEventTicketAttributeIds(IEnumerable<long> EventTicketAttributeIds)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.EventTicketAttributeId):C} IN @TicketAttributeIds ")
                .WithParameters(new { TicketAttributeIds = EventTicketAttributeIds })
            );
        }

        public IEnumerable<TransactionDetail> GetByEventTicketAttributeId(long EventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.EventTicketAttributeId):C} = @TicketAttributeId ")
                .WithParameters(new { TicketAttributeId = EventTicketAttributeId })
            );
        }

        public IEnumerable<TransactionDetail> GetByTransactionIdsAndEventTicketAttributeIds(IEnumerable<long> transactionIds, IEnumerable<long> eventTicketAttributeIds)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.TransactionId):C} IN @TransactionIds AND {nameof(TransactionDetail.EventTicketAttributeId):C} IN @TicketAttributeIds ")
                .WithParameters(new { TransactionIds = transactionIds, TicketAttributeIds = eventTicketAttributeIds })
            );
        }

        public IEnumerable<TransactionDetail> GetByEventTicketAttributeandTransactionId(IEnumerable<long> eventTicketAttribute, long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.EventTicketAttributeId):C} IN @TicketAttributeIds AND {nameof(TransactionDetail.TransactionId):C} = @TransactionId ")
           .WithParameters(new { TicketAttributeIds = eventTicketAttribute, TransactionId = transactionId })
           );
        }

        public IEnumerable<TransactionDetail> GetByTransactionIds(IEnumerable<long> transactionIds)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.TransactionId):C}IN @TransactionIds")
                .WithParameters(new { TransactionIds = transactionIds })
            );
        }

        public IEnumerable<TransactionDetail> GetByEventTicketAttributeAndTransactionId(IEnumerable<long> eventTicketAttribute, long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.TransactionId):C}=@TransactionId AND {nameof(TransactionDetail.EventTicketAttributeId):C} IN @EventTicketAttributeId")
           .WithParameters(new { EventTicketAttributeId = eventTicketAttribute, TransactionId = transactionId })
           );
        }

        public TransactionDetail GetByTicketAttributeIdAndTransactionId(long transactionId, long eventTicketAttributeId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.TransactionId):C}=@TransactionId AND {nameof(TransactionDetail.EventTicketAttributeId):C} = @EventTicketAttributeId")
           .WithParameters(new { TransactionId = transactionId, EventTicketAttributeId = eventTicketAttributeId })
           ).FirstOrDefault();
        }

        public TransactionDetail GetByEventTicketAttributeIdAndTransactionIdAndTicketTypeId(long transactionId, long eventTicketAttributeId, short ticketTypeId)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.TransactionId):C}=@TransactionId AND {nameof(TransactionDetail.EventTicketAttributeId):C} = @EventTicketAttributeId AND {nameof(TransactionDetail.TicketTypeId):C} = @TicketTypeId")
           .WithParameters(new { TransactionId = transactionId, EventTicketAttributeId = eventTicketAttributeId, TicketTypeId = ticketTypeId })
           ).FirstOrDefault();
        }

        public IEnumerable<TransactionDetail> GetByTransactionIdsAndEventTicketAttributeIdsAndTicketTypeId(IEnumerable<long> transactionIds, IEnumerable<long> eventTicketAttributeIds, IEnumerable<short> ticketTypeIds)
        {
            return GetAll(s => s.Where($"{nameof(TransactionDetail.TransactionId):C} IN @TransactionIds AND {nameof(TransactionDetail.EventTicketAttributeId):C} IN @TicketAttributeIds AND {nameof(TransactionDetail.TicketTypeId):C} IN @TicketTypeIdIds ")
                .WithParameters(new { TransactionIds = transactionIds, TicketAttributeIds = eventTicketAttributeIds, TicketTypeIdIds = ticketTypeIds })
            );
        }
    }
}