using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterVenueLayoutSections;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;
using System;

namespace FIL.Api.QueryHandlers.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsGetUpdateDataQueryHandler : IQueryHandler<MasterVenueLayoutSectionsGetUpdateDataQuery, MasterVenueLayoutSectionsGetUpdateDataQueryResult>
    {
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IMasterVenueLayoutRepository _masterVenueLayoutRepository;
        private readonly IEntryGateRepository _entryGateRepository;

        public MasterVenueLayoutSectionsGetUpdateDataQueryHandler(IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository,
            IMasterVenueLayoutRepository masterVenueLayoutRepository,
            IEntryGateRepository entryGateRepository)
        {
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _masterVenueLayoutRepository = masterVenueLayoutRepository;
            _entryGateRepository = entryGateRepository;
        }

        public MasterVenueLayoutSectionsGetUpdateDataQueryResult Handle(MasterVenueLayoutSectionsGetUpdateDataQuery query)
        {
            int MasterVenueLayoutSectionId;

            var masterVenueLayoutSections = _masterVenueLayoutSectionRepository.GetByAltId(new Guid(query.AltId));
            var entryGateId = 0;
            if (query.EntryGateAltId != null)
            {
                var entryGateDataModel = _entryGateRepository.GetByAltId(new Guid(query.EntryGateAltId));
                entryGateId = entryGateDataModel.Id;
            }
            if (masterVenueLayoutSections.VenueLayoutAreaId == 1)
            {
                MasterVenueLayoutSectionId = 0;
            }
            else
            {
                if (query.Section != null)
                {
                    var masterVenueLayoutSection = _masterVenueLayoutSectionRepository.GetByAltId(new Guid(query.Section));
                    MasterVenueLayoutSectionId = masterVenueLayoutSection.Id;
                }
                else if (query.Block != null)
                {
                    var masterVenueLayoutSection = _masterVenueLayoutSectionRepository.GetByAltId(new Guid(query.Block));
                    MasterVenueLayoutSectionId = masterVenueLayoutSection.Id;
                }
                else if (query.Level != null)
                {
                    var masterVenueLayoutSection = _masterVenueLayoutSectionRepository.GetByAltId(new Guid(query.Level));
                    MasterVenueLayoutSectionId = masterVenueLayoutSection.Id;
                }
                else if (query.Stand != null)
                {
                    var masterVenueLayoutSection = _masterVenueLayoutSectionRepository.GetByAltId(new Guid(query.Stand));
                    MasterVenueLayoutSectionId = masterVenueLayoutSection.Id;
                }
                else
                {
                    MasterVenueLayoutSectionId = masterVenueLayoutSections.MasterVenueLayoutSectionId;
                }
            }
            return new MasterVenueLayoutSectionsGetUpdateDataQueryResult
            {
                Id = masterVenueLayoutSections.Id,
                MasterVenueLayoutId = masterVenueLayoutSections.MasterVenueLayoutId,
                MasterVenueLayoutSectionId = MasterVenueLayoutSectionId,
                ParentId = masterVenueLayoutSections.MasterVenueLayoutSectionId,
                EntryGateId = entryGateId,
                VenueLayoutAreaId = masterVenueLayoutSections.VenueLayoutAreaId
            };
        }
    }
}