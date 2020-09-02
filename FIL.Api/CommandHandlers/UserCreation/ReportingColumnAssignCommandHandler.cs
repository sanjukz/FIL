using FIL.Api.Repositories;
using FIL.Contracts.Commands.UserCreation;
using MediatR;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.UserCreation
{
    public class ReportingColumnAssignCommandHandler : BaseCommandHandler<ReportingColumnAssignCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IReportingColumnRepository _reportingColumnRepository;
        private readonly IReportingColumnsUserMappingRepository _reportingColumnsUserMappingRepository;
        private readonly IReportingColumnsMenuMappingRepository _reportingColumnsMenuMappingRepository;
        private readonly IMediator _mediator;

        public ReportingColumnAssignCommandHandler(
            IUserRepository userRepository,
            IReportingColumnRepository reportingColumnRepository,
            IReportingColumnsUserMappingRepository reportingColumnsUserMappingRepository,
            IReportingColumnsMenuMappingRepository reportingColumnsMenuMappingRepository,
            IMediator mediator)
            : base(mediator)
        {
            _userRepository = userRepository;
            _reportingColumnRepository = reportingColumnRepository;
            _reportingColumnsUserMappingRepository = reportingColumnsUserMappingRepository;
            _reportingColumnsMenuMappingRepository = reportingColumnsMenuMappingRepository;
            _mediator = mediator;
        }

        protected override async Task Handle(ReportingColumnAssignCommand command)
        {
            var user = _userRepository.GetByAltId(command.UserAltId);
            if (user != null)
            {
                int Counter = 0;
                foreach (var item in command.reportingColumns)
                {
                    Counter = Counter + 1;
                    var reportingColumnsMenuMappings = _reportingColumnsMenuMappingRepository.GetByColumnIdAndMenuId(item.Id, 1);
                    if (reportingColumnsMenuMappings == null)
                    {
                        var ReportingColumnsMenuMapping = new FIL.Contracts.DataModels.ReportingColumnsMenuMapping
                        {
                            MenuId = 1,
                            ColumnId = item.Id,
                            CreatedBy = command.ModifiedBy,
                            IsEnabled = true
                        };
                        var reportingColumnsMenuMappingResult = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.ReportingColumnsMenuMapping>(_reportingColumnsMenuMappingRepository.Save(ReportingColumnsMenuMapping));

                        if (reportingColumnsMenuMappingResult.Id != 0)
                        {
                            var reportingColumnsUserMapping = _reportingColumnsUserMappingRepository.GetByUserIdandColumnsMenuMappingId(user.Id, reportingColumnsMenuMappingResult.Id);
                            if (reportingColumnsUserMapping == null)
                            {
                                var ReportingColumnsUserMapping = new FIL.Contracts.DataModels.ReportingColumnsUserMapping
                                {
                                    ColumnsMenuMappingId = reportingColumnsMenuMappingResult.Id,
                                    UserId = user.Id,
                                    SortOrder = Counter,
                                    IsShow = true,
                                    IsEnabled = true,
                                    CreatedBy = command.ModifiedBy
                                };
                                var reportingColumnsUserMappingResult = _reportingColumnsUserMappingRepository.Save(ReportingColumnsUserMapping);
                            }
                        }
                    }
                    else
                    {
                        var reportingColumnsUserMapping = _reportingColumnsUserMappingRepository.GetByUserIdandColumnsMenuMappingId(user.Id, reportingColumnsMenuMappings.Id);
                        if (reportingColumnsUserMapping == null)
                        {
                            var ReportingColumnsUserMapping = new FIL.Contracts.DataModels.ReportingColumnsUserMapping
                            {
                                ColumnsMenuMappingId = reportingColumnsMenuMappings.Id,
                                UserId = user.Id,
                                SortOrder = Counter,
                                IsShow = true,
                                IsEnabled = true,
                                CreatedBy = command.ModifiedBy
                            };
                            var reportingColumnsUserMappingResult = _reportingColumnsUserMappingRepository.Save(ReportingColumnsUserMapping);
                        }
                    }
                }
            }
        }
    }
}