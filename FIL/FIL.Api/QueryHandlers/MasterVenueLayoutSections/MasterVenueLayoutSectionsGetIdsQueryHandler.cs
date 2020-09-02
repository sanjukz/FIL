using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterVenueLayoutSections;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;
using System;

namespace FIL.Api.QueryHandlers.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsGetIdsQueryHandler : IQueryHandler<MasterVenueLayoutSectionsGetIdsQuery, MasterVenueLayoutSectionsGetIdsQueryResult>
    {
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IMasterVenueLayoutRepository _masterVenueLayoutRepository;
        private readonly IEntryGateRepository _entryGateRepository;

        public MasterVenueLayoutSectionsGetIdsQueryHandler(IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository,
            IMasterVenueLayoutRepository masterVenueLayoutRepository,
            IEntryGateRepository entryGateRepository)
        {
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _masterVenueLayoutRepository = masterVenueLayoutRepository;
            _entryGateRepository = entryGateRepository;
        }

        public MasterVenueLayoutSectionsGetIdsQueryResult Handle(MasterVenueLayoutSectionsGetIdsQuery query)
        {
            var masterVenueLayoutDataModel = _masterVenueLayoutRepository.GetByAltId(new Guid(query.MasterVenueLayoutAltId));
            var entryGateId = 0;
            if (query.EntryGateAltId != null)
            {
                var entryGateDataModel = _entryGateRepository.GetByAltId(new Guid(query.EntryGateAltId));
                entryGateId = entryGateDataModel.Id;
            }

            int MasterVenueLayoutSectionId;
            int Id;
            if (query.AltId != "" && query.AltId != null)
            {
                var MasterVenueLayoutSectionDataModel = _masterVenueLayoutSectionRepository.GetByAltId(new Guid(query.AltId));
                MasterVenueLayoutSectionId = MasterVenueLayoutSectionDataModel.Id;
                Id = MasterVenueLayoutSectionDataModel.Id;
            }
            else
            {
                MasterVenueLayoutSectionId = 0;
                Id = -1;
            }
            return new MasterVenueLayoutSectionsGetIdsQueryResult
            {
                Id = Id,
                EntryGateId = entryGateId,
                MasterVenueLayoutId = masterVenueLayoutDataModel.Id,
                MasterVenueLayoutSectionId = MasterVenueLayoutSectionId
            };
        }
    }
}