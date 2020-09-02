using FIL.Api.Repositories;
using FIL.Contracts.Models.MasterLayout;
using FIL.Contracts.Queries.ManageLayout;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.ManageLayouts
{
    public class ManageLayoutQueryHandler : IQueryHandler<ManageLayoutQuery, ManageLayoutQueryResult>
    {
        private readonly IMasterVenueLayoutRepository _masterVenueLayoutsRepository;
        private readonly IEntryGateRepository _entryGateRepository;
        private readonly IMasterVenueLayoutSectionRepository _masterVenueLayoutSectionRepository;
        private readonly IMasterVenueLayoutSectionSeatRepository _masterVenueLayoutSectionSeatRepository;

        public ManageLayoutQueryHandler(IMasterVenueLayoutSectionSeatRepository masterVenueLayoutSectionSeatRepository, IMasterVenueLayoutRepository masterVenueLayoutRepository, IMasterVenueLayoutSectionRepository masterVenueLayoutSectionRepository, IEntryGateRepository entryGateRepository)
        {
            _masterVenueLayoutsRepository = masterVenueLayoutRepository;
            _masterVenueLayoutSectionRepository = masterVenueLayoutSectionRepository;
            _entryGateRepository = entryGateRepository;
            _masterVenueLayoutSectionSeatRepository = masterVenueLayoutSectionSeatRepository;
        }

        public ManageLayoutQueryResult Handle(ManageLayoutQuery query)
        {
            var MasterVenueLayout = _masterVenueLayoutsRepository.GetByAltId(query.AltId);

            var masterlayoutSection = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutId(MasterVenueLayout.Id);
            //  var masterlayoutSectionModel = masterlayoutSection.ToDictionary(mls => mls);

            var stands = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutIdAndVenueAreaId(MasterVenueLayout.Id, 1);

            var standContainer = stands.Select(eid =>
            {
                var stand = _masterVenueLayoutSectionRepository.Get(eid.Id);
                var standmodel = AutoMapper.Mapper.Map<MasterLayout>(stand);

                var Levels = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutSectionIdAndVenueAreaId(eid.Id, 2).ToList();
                var LevelModel = AutoMapper.Mapper.Map<List<MasterLayout>>(Levels);

                var Blocks = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutSectionIdAndVenueAreaId(eid.Id, 3).ToList();
                var BlockModel = AutoMapper.Mapper.Map<List<MasterLayout>>(Blocks);

                var Sections = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutSectionIdAndVenueAreaId(eid.Id, 4).ToList();
                var SectionModel = AutoMapper.Mapper.Map<List<MasterLayout>>(Sections);

                var LevelContainer = Levels.Select(lvl =>
                {
                    var blockUnderLevelDataModel = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutSectionIdAndVenueAreaId(lvl.Id, 3).ToList();
                    var blockUnderLevelModel = AutoMapper.Mapper.Map<List<MasterLayout>>(blockUnderLevelDataModel);

                    var SectionUnderLevelDataModel = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutSectionIdAndVenueAreaId(lvl.Id, 4).ToList();
                    var SectionUnderLevelModel = AutoMapper.Mapper.Map<List<MasterLayout>>(SectionUnderLevelDataModel);

                    var BlockConatiner = blockUnderLevelModel.Select(blc =>
                    {
                        var SectionUnderBlockDataModel = _masterVenueLayoutSectionRepository.GetByMasterVenueLayoutSectionIdAndVenueAreaId(blc.Id, 4).ToList();
                        var SectionUnderBlockModel = AutoMapper.Mapper.Map<List<MasterLayout>>(SectionUnderBlockDataModel);

                        return new MasterLayoutBlockContainer
                        {
                            SectionUnderBlock = SectionUnderBlockModel
                        };
                    });

                    return new MasterLayoutLevelContainer
                    {
                        MasterLayoutBlockContainers = BlockConatiner.ToList(),
                        BlockUnderLevel = blockUnderLevelModel,
                        SectionUnderLevel = SectionUnderLevelModel
                    };
                });
                return new MasterLayoutStandContainer
                {
                    stands = standmodel,
                    levels = LevelModel,
                    blocks = BlockModel,
                    sections = SectionModel,
                    MasterLayoutLevelContainer = LevelContainer.ToList(),
                };
            });

            return new ManageLayoutQueryResult
            {
                MasterLayoutStandContainer = standContainer.ToList()
            };
        }
    }
}