using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetPaymentOptionsQueryHandler : IQueryHandler<GetPaymentOptionsQuery, GetPaymentOptionQueryResult>
    {
        private readonly IZsuitePaymentOptionRepository _zsuitePaymentOptionRepository;

        public GetPaymentOptionsQueryHandler(IZsuitePaymentOptionRepository zsuitePaymentOptionRepository)
        {
            _zsuitePaymentOptionRepository = zsuitePaymentOptionRepository;
        }

        public GetPaymentOptionQueryResult Handle(GetPaymentOptionsQuery query)
        {
            var paymentOptions = _zsuitePaymentOptionRepository.GetAll();
            return new GetPaymentOptionQueryResult
            {
                PaymentOptions = AutoMapper.Mapper.Map<List<ZsuitePaymentOption>>(paymentOptions)
            };
        }
    }
}