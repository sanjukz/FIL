using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterVenueLayoutSections;
using FIL.Contracts.QueryResults.MasterVenueLayoutSections;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsQueryHandler : IQueryHandler<MasterVenueLayoutSectionsQuery, MasterVenueLayoutSectionsQueryResult>
    {
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IMasterVenueLayoutSectionSeatRepository _masterVenueLayoutSectionSeatRepository;
        public MasterVenueLayoutSectionsQueryHandler(IMasterVenueLayoutSectionSeatRepository masterVenueLayoutSectionSeatRepository, IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository)
        {
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _masterVenueLayoutSectionSeatRepository = masterVenueLayoutSectionSeatRepository;
        }

        public MasterVenueLayoutSectionsQueryResult Handle(MasterVenueLayoutSectionsQuery query)
        {
            var response = new MasterVenueLayoutSectionsQueryResult();
            var currentCapacity = query.SelectedSectionId != 0 ?_masterVenueLayoutSectionRepository.Get(query.SelectedSectionId).Capacity : 0;
            var existingChildCapacity = _masterVenueLayoutSectionRepository.GetExistingChildCapacity(query.MasterVenueLayoutSectionId) - currentCapacity;
            var parentCapacity = query.MasterVenueLayoutSectionId != 0 ? _masterVenueLayoutSectionRepository.Get(query.MasterVenueLayoutSectionId).Capacity : existingChildCapacity;
            var availableCapacity = query.MasterVenueLayoutSectionId != 0 ? parentCapacity - existingChildCapacity : existingChildCapacity;


            var seatCount = _masterVenueLayoutSectionSeatRepository.GetSeatCount(query.MasterVenueLayoutSectionId);
            if (seatCount != 0)
            {
                response.IsSeatExists = true;
                response.IsExisting = false;
                response.AvailableCapacity = availableCapacity;
            }
            else
            {
                var section = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutIdandMasterVenueLayoutSectionIdandVenueLayoutAreaId(query.MasterVenueLayoutId, query.MasterVenueLayoutSectionId, query.VenueLayoutAreaId);
                if (section.Select(s => s.SectionName).Contains(query.SectionName))
                {
                    response.IsExisting = true;
                    response.IsSeatExists = false;
                    response.AvailableCapacity = availableCapacity;
                }
                else
                {                    
                    response.IsExisting = false;
                    response.IsSeatExists = false;
                    response.AvailableCapacity = availableCapacity;
                }
            }

            return response;
        }
    }
}