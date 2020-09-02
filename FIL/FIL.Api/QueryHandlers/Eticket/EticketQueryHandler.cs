using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.Eticket;
using FIL.Contracts.QueryResults.Eticket;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Eticket
{
    public class EticketQueryHandler : IQueryHandler<EticketQuery, EticketQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionDeliveryDetailRepository _transactionDeliveryDetailRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;

        public EticketQueryHandler(ITransactionRepository transactionRepository, IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, ITransactionDeliveryDetailRepository transactionDeliveryDetailRepository, ITransactionDetailRepository transactionDetailRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _transactionDeliveryDetailRepository = transactionDeliveryDetailRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
        }

        public EticketQueryResult Handle(EticketQuery query)
        {
            FIL.Contracts.DataModels.Transaction transactionDataModel = _transactionRepository.Get(query.Id);

            if (transactionDataModel != null)
            {
                List<TransactionDetail> transactionDetailDataModel = _transactionDetailRepository.GetByTransactionId(query.Id).ToList();
                TransactionDeliveryDetail transactionDeliveryDetailDataModel = _transactionDeliveryDetailRepository.GetByTransactionDetailId(transactionDetailDataModel[0].Id);
                List<MatchSeatTicketDetail> matchSeatTicketDetailDataModel = _matchSeatTicketDetailRepository.GetbyTransactionId(query.Id).ToList();

                if (matchSeatTicketDetailDataModel.Any())
                {
                    int printCount = matchSeatTicketDetailDataModel.Select(p => p.PrintCount).Max().Value;
                    EticketQueryResult response = verifyTransaction(transactionDataModel, transactionDeliveryDetailDataModel, printCount, query);
                    return response;
                }
            }
            return new EticketQueryResult
            {
                Success = false
            };
        }

        public EticketQueryResult verifyTransaction(Contracts.DataModels.Transaction transactionDataModel, TransactionDeliveryDetail transactionDeliveryDetailDataModel, int printCount, EticketQuery query)
        {
            EticketQueryResult response = new EticketQueryResult();
            response.MaxPrintCount = printCount;
            response.IsVerified = false;
            response.Success = true;
            response.Transaction = transactionDataModel;
            response.DeliveryType = transactionDeliveryDetailDataModel.DeliveryTypeId;

            if (transactionDataModel.TransactionStatusId == Contracts.Enums.TransactionStatus.Success)
                response.TransactionStatus = Contracts.Enums.TransactionStatus.Success;
            else
                response.TransactionStatus = Contracts.Enums.TransactionStatus.None;

            if (transactionDeliveryDetailDataModel != null)
            {
                if (transactionDeliveryDetailDataModel.DeliveryTypeId == Contracts.Enums.DeliveryTypes.PrintAtHome)
                    response.IsPAH = true;
                else
                    response.IsPAH = false;
            }

            if (query.Email == transactionDataModel.EmailId)
                response.EmailVerified = true;
            else
                response.EmailVerified = false;

            if (response.IsPAH && response.EmailVerified &&
                response.MaxPrintCount < 5 && response.TransactionStatus == Contracts.Enums.TransactionStatus.Success)
            {
                response.IsVerified = true;
            }
            return response;
        }
    }
}