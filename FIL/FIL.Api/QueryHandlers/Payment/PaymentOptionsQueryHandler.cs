using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Payment;
using FIL.Contracts.QueryResults.Payment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Payment
{
    public class PaymentOptionsQueryHandler : IQueryHandler<PaymentOptionsQuery, PaymentOptionsQueryResult>
    {
        private readonly INetBankingBankDetailRepository _netBankingBankDetailRepository;
        private readonly ICashCardDetailRepository _cashCardDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public PaymentOptionsQueryHandler(INetBankingBankDetailRepository netBankingBankDetailRepository, ICashCardDetailRepository cashCardDetailRepository, FIL.Logging.ILogger logger)
        {
            _netBankingBankDetailRepository = netBankingBankDetailRepository;
            _cashCardDetailRepository = cashCardDetailRepository;
            _logger = logger;
        }

        public PaymentOptionsQueryResult Handle(PaymentOptionsQuery query)
        {
            try
            {
                var bankDetail = _netBankingBankDetailRepository.GetAll(null).Where(w => w.IsEnabled);
                var bankDetailModel = AutoMapper.Mapper.Map<List<NetBankingBankDetail>>(bankDetail);
                var cashcardDetail = _cashCardDetailRepository.GetAll(null).Where(w => w.IsEnabled);
                var cashcardModel = AutoMapper.Mapper.Map<List<CashCardDetail>>(cashcardDetail);
                return new PaymentOptionsQueryResult
                {
                    BankDetails = bankDetailModel,
                    CashCardDetails = cashcardModel,
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new PaymentOptionsQueryResult
                {
                    BankDetails = null,
                    CashCardDetails = null,
                };
            }
        }
    }
}