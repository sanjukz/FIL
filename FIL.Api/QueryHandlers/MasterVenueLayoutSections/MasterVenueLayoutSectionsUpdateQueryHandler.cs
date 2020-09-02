using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterVenueLayoutSections;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;

namespace FIL.Api.QueryHandlers.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsUpdateQueryHandler : IQueryHandler<MasterVenueLayoutSectionsUpdateQuery, MasterVenueLayoutSectionsUpdateQueryResult>
    {
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IEntryGateRepository _entryGateRepository;

        public MasterVenueLayoutSectionsUpdateQueryHandler(IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository,
            IEntryGateRepository entryGateRepository)
        {
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _entryGateRepository = entryGateRepository;
        }

        public MasterVenueLayoutSectionsUpdateQueryResult Handle(MasterVenueLayoutSectionsUpdateQuery query)
        {
            var masterVenueLayoutSections = _masterVenueLayoutSectionRepository.GetByAltId(query.AltId);
            var entryGate = _entryGateRepository.Get(masterVenueLayoutSections.EntryGateId);
            string Stand = "", Level = "", Block = "", Section = "";
            if (masterVenueLayoutSections.VenueLayoutAreaId == 1)
            {
                Stand = masterVenueLayoutSections.SectionName;
            }
            if (masterVenueLayoutSections.VenueLayoutAreaId == 2)
            {
                Level = masterVenueLayoutSections.SectionName;
            }
            else if (masterVenueLayoutSections.VenueLayoutAreaId == 3)
            {
                Block = masterVenueLayoutSections.SectionName;
            }
            else if (masterVenueLayoutSections.VenueLayoutAreaId == 4)
            {
                Section = masterVenueLayoutSections.SectionName;
            }

            if (masterVenueLayoutSections.MasterVenueLayoutSectionId == 0)
            {
                Stand = masterVenueLayoutSections.SectionName;
            }
            else
            {
                while (masterVenueLayoutSections.MasterVenueLayoutSectionId != 0)
                {
                    masterVenueLayoutSections = _masterVenueLayoutSectionRepository.Get(masterVenueLayoutSections.MasterVenueLayoutSectionId);
                    if (masterVenueLayoutSections.VenueLayoutAreaId == 1)
                    {
                        Stand = masterVenueLayoutSections.SectionName;
                    }
                    if (masterVenueLayoutSections.VenueLayoutAreaId == 2)
                    {
                        Level = masterVenueLayoutSections.SectionName;
                    }
                    else if (masterVenueLayoutSections.VenueLayoutAreaId == 3)
                    {
                        Block = masterVenueLayoutSections.SectionName;
                    }
                    else if (masterVenueLayoutSections.VenueLayoutAreaId == 4)
                    {
                        Section = masterVenueLayoutSections.SectionName;
                    }
                }
            }
            return new MasterVenueLayoutSectionsUpdateQueryResult
            {
                Stand = Stand,
                Level = Level,
                Block = Block,
                Section = Section,
                Capacity = masterVenueLayoutSections.Capacity,
                EntryGate = entryGate.Name
            };
        }
    }
}