using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterVenueLayout;
using FIL.Contracts.QueryResults.MasterVenueLayout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.MasterVenueLayout
{
    public class MasterVenueLayoutQueryHandler : IQueryHandler<MasterVenueLayoutQuery, MasterVenueLayoutQueryResult>
    {
        private readonly IMasterVenueLayoutRepository _masterVenueLayoutRepository;
        private readonly IVenueRepository _venueRepository;

        public MasterVenueLayoutQueryHandler(IMasterVenueLayoutRepository masterVenueLayoutRepository,
            IVenueRepository venueRepository)
        {
            _masterVenueLayoutRepository = masterVenueLayoutRepository;
            _venueRepository = venueRepository;
        }

        public MasterVenueLayoutQueryResult Handle(MasterVenueLayoutQuery query)
        {
            var venueDataModel = _venueRepository.GetByAltId(query.AltId);
            var venueModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.Venue>(venueDataModel);
            var masterVenueLayoutDataModel = _masterVenueLayoutRepository.GetAllByVenueId(venueModel.Id);
            if (masterVenueLayoutDataModel.Any())
            {
                var MasterVenueLayoutModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.MasterVenueLayout>>(masterVenueLayoutDataModel);
                return new MasterVenueLayoutQueryResult
                {
                    masterVenueLayouts = MasterVenueLayoutModel
                };
            }
            else
            {
                FIL.Contracts.DataModels.MasterVenueLayout masterVenueLayout = new FIL.Contracts.DataModels.MasterVenueLayout();
                masterVenueLayout = _masterVenueLayoutRepository.Save(new FIL.Contracts.DataModels.MasterVenueLayout()
                {
                    AltId = Guid.NewGuid(),
                    LayoutName = venueModel.Name,
                    VenueId = venueModel.Id,
                    IsEnabled = true,
                    CreatedBy = new Guid(),
                    CreatedUtc = DateTime.Now,
                    UpdatedBy = new Guid(),
                    ModifiedBy = new Guid(),
                    UpdatedUtc = DateTime.Now
                });
                List<FIL.Contracts.DataModels.MasterVenueLayout> temp = new List<FIL.Contracts.DataModels.MasterVenueLayout>();
                temp.Add(masterVenueLayout);
                return new MasterVenueLayoutQueryResult
                {
                    masterVenueLayouts = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.MasterVenueLayout>>(temp)
                };
            }
        }
    }
}