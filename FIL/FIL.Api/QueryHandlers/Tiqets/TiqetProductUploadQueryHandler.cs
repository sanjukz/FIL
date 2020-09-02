using FIL.Api.Repositories;
using FIL.Api.Repositories.Tiqets;
using FIL.Contracts.Models.Tiqets;
using FIL.Contracts.Queries;
using FIL.Contracts.QueryResults.Tiqets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TicketLookup
{
    public class TiqetProductUploadQueryHandler : IQueryHandler<TiqetProductUploadQuery, TiqetProductUploadQueryResult>
    {
        private readonly ITiqetProductRepository _tiqetProductRepository;
        private readonly ITiqetProductImageRepository _tiqetProductImageRepository;
        private readonly ITiqetEventDetailMappingRepository _tiqetEventDetailMappingRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;

        public TiqetProductUploadQueryHandler(ITiqetProductRepository tiqetProductRepository, ITiqetProductImageRepository tiqetProductImageRepository, ITiqetEventDetailMappingRepository tiqetEventDetailMappingRepository, IEventDetailRepository eventDetailRepository, IEventRepository eventRepository)
        {
            _tiqetProductRepository = tiqetProductRepository;
            _tiqetProductImageRepository = tiqetProductImageRepository;
            _eventRepository = eventRepository;
            _tiqetEventDetailMappingRepository = tiqetEventDetailMappingRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
        }

        public TiqetProductUploadQueryResult Handle(TiqetProductUploadQuery query)
        {
            var tiqetProducts = _tiqetProductRepository.GetAll().Skip(query.SkipIndex).Take(query.TakeIndex).ToList();
            List<TiqetImageModel> tiqetImageModelList = new List<TiqetImageModel>();

            foreach (var currentProduct in tiqetProducts)
            {
                try
                {
                    TiqetImageModel tiqetImageModel = new TiqetImageModel();
                    var tiqetImage = _tiqetProductImageRepository.GetByProductId(currentProduct.ProductId).FirstOrDefault();
                    var tiqetEventDetail = _tiqetEventDetailMappingRepository.GetByProductId(currentProduct.ProductId);
                    var eventDetail = _eventDetailRepository.Get(tiqetEventDetail.EventDetailId);
                    var eventId = _eventRepository.Get(eventDetail.EventId);
                    tiqetImageModel.EventAltId = eventId.AltId;
                    tiqetImageModel.ImageLink = tiqetImage.Large;
                    tiqetImageModelList.Add(tiqetImageModel);
                }
                catch (Exception e)
                {
                    continue;
                }
            }

            return new TiqetProductUploadQueryResult
            {
                TiqetImagesList = tiqetImageModelList
            };
        }
    }
}