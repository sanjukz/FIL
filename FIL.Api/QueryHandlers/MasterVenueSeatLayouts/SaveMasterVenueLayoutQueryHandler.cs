using FIL.Api.Repositories;
using FIL.Contracts.Queries.MasterVenueLayout;
using FIL.Contracts.QueryResults.MasterVenueLayout;
using System;

namespace FIL.Api.QueryHandlers.MasterVenueSeatLayouts
{
    public class SaveMasterVenueLayoutQueryHandler : IQueryHandler<SaveMasterVenueLayoutQuery, SaveMasterVenueLayoutQueryResult>
    {
        private readonly ISaveMasterLayoutRepository _saveMasterLayoutRepository;
        private readonly IMasterVenueLayoutSectionSeatRepository _masterVenueLayoutSectionSeatRepository;

        public SaveMasterVenueLayoutQueryHandler(IMasterVenueLayoutSectionSeatRepository masterVenueLayoutSectionSeatRepository, ISaveMasterLayoutRepository saveMasterLayoutRepository)
        {
            _saveMasterLayoutRepository = saveMasterLayoutRepository;
            _masterVenueLayoutSectionSeatRepository = masterVenueLayoutSectionSeatRepository;
        }

        public SaveMasterVenueLayoutQueryResult Handle(SaveMasterVenueLayoutQuery query)
        {
            try
            {
                var seatCount = _masterVenueLayoutSectionSeatRepository.GetSeatCount(query.MasterVenueLayoutSectionId);
                if (seatCount != 0 && query.ShouldSeatInsert)
                {
                    return new SaveMasterVenueLayoutQueryResult
                    {
                        Success = false,
                        IsExisting = true
                    };
                }
                else
                {
                    var result = _saveMasterLayoutRepository.SaveSeatLayoutData(query.xmlData, query.MasterVenueLayoutSectionId, query.ShouldSeatInsert);
                    if (result >= 0)
                    {
                        return new SaveMasterVenueLayoutQueryResult
                        {
                            Success = true,
                            IsExisting = false
                        };
                    }
                    else
                    {
                        return new SaveMasterVenueLayoutQueryResult
                        {
                            Success = false,
                            IsExisting = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new SaveMasterVenueLayoutQueryResult
                {
                    Success = false,
                    IsExisting = false
                };
            }
        }
    }
}