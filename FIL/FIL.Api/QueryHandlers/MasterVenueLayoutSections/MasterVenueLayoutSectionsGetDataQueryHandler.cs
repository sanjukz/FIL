using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterVenueLayoutSections;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsGetDataQueryHandler : IQueryHandler<MasterVenueLayoutSectionsGetDataQuery, MasterVenueLayoutSectionsGetDataQueryResult>
    {
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IMasterVenueLayoutRepository _masterVenueLayout;

        public MasterVenueLayoutSectionsGetDataQueryHandler(IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository,
            IMasterVenueLayoutRepository masterVenueLayout)
        {
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _masterVenueLayout = masterVenueLayout;
        }

        public MasterVenueLayoutSectionsGetDataQueryResult Handle(MasterVenueLayoutSectionsGetDataQuery query)
        {
            if (query.isGetByMasterVenueLayoutSectionId)
            {
                var MasterVenueLayoutSectionDataModels = _masterVenueLayoutSectionRepository.GetByAltId(query.AltId);
                var MasterVenueLayoutSectionDataModel = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutSectionIdandVenueLayoutAreaId(MasterVenueLayoutSectionDataModels.Id, query.VenueLayoutAreaId);
                var MasterVenueLayoutModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.MasterVenueLayoutSection>>(MasterVenueLayoutSectionDataModel);
                return new MasterVenueLayoutSectionsGetDataQueryResult
                {
                    MasterVenueLayoutSections = MasterVenueLayoutModel
                };
            }
            else
            {
                var MasterVenueLayoutDataModel = _masterVenueLayout.GetByAltId(query.AltId);
                var MasterVenueLayoutSectionDataModel = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutIdandVenueLayoutAreaId(MasterVenueLayoutDataModel.Id, query.VenueLayoutAreaId);
                var MasterVenueLayoutModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.MasterVenueLayoutSection>>(MasterVenueLayoutSectionDataModel);
                return new MasterVenueLayoutSectionsGetDataQueryResult
                {
                    MasterVenueLayoutSections = MasterVenueLayoutModel
                };
            }
        }
    }
}