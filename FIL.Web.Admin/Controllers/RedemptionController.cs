using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.FeelRedemption;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.FeelRedemption;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.Queries.User;
using FIL.Foundation.Senders;
using FIL.Logging;
using FIL.Logging.Enums;
using FIL.Web.Core.Helpers;
using FIL.Web.Core.Providers;
using FIL.Web.Admin.ViewModels.Redemption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Admin.Controllers
{
    public class RedemptionController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;
        private readonly ILogger _logger;

        public RedemptionController(ICommandSender commandSender,
            ILogger logger,
            ISessionProvider sessionProvider,
             IQuerySender querySender
            )
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _logger = logger;
            _sessionProvider = sessionProvider;
        }

        [HttpGet]
        [Route("api/redemption/{transactionId}/{userAltId}")]
        public async Task<GetDetailResponseModel> GetDetails(long transactionId, Guid userAltId)
        {
            var queryResult = await _querySender.Send(new GetDetailQuery
            {
                TransactionId = transactionId,
                UserAltId = userAltId
            });
            return new GetDetailResponseModel
            {
                TransactionDetail = queryResult.TransactionDetail,
                EventDetail = queryResult.EventDetail,
                EventTicketAttribute = queryResult.EventTicketAttribute,
                EventTicketDetail = queryResult.EventTicketDetail,
                TicketCategory = queryResult.TicketCategory,
            };
        }

        [HttpPost]
        [Route("api/redeem/ticket")]
        public async Task<RedemptionResponseModel> Redeem([FromBody]RedemptionFormDataModel model)
        {
            if (ModelState.IsValid)
            {
                await _commandSender.Send(new FeelRedemptionCommand { TransactionDetailIds = model.TransactionDetailIds });
                return new RedemptionResponseModel
                {
                    IsSuccess = true
                };
            }
            else
            {
                return new RedemptionResponseModel
                {
                    IsSuccess = false
                };

            }
        }

        [Route("api/Guide/Add")]
        public async Task<CreateGuideResponseViewModel> GuideAdd([FromBody]CreateGuideInputModel model)
        {
            try
            {
                var data = await _sessionProvider.Get();
                //sample API call
                var createGuideCommandResult = await _commandSender.Send<Contracts.Commands.Redemption.GuideDetailsCommand, Contracts.Commands.Redemption.GuideDetailsCommandResult>(new Contracts.Commands.Redemption.GuideDetailsCommand
                {
                    AccountNumber = model.AccountNumber,
                    AddressLineOne = model.AddressLineOne,
                    AddressLineTwo = model.AddressLineTwo,
                    CityId = model.ResidentCityId,
                    ZipCode = model.Zip,
                    AddressProofType = "AddressProofType",
                    BankName = model.BankName,
                    BranchName = model.BranchCode,
                    CountryCode = model.PhoneCode,
                    CurrencyId = model.CurrencyId,
                    EmailId = model.Email,
                    EventIDs = model.EventIDs,
                    FirstName = model.FirstName,
                    IPAddress = "IPAddress",
                    LanguageIds = model.LanguageId,
                    LastName = model.LastName,
                    MobileNo = model.PhoneNumber,
                    Notes = model.ServiceNotes,
                    ModifiedBy = data.IsAuthenticated ? data.User.AltId : new Guid("A101F398-2FFE-47A2-82AE-11A9220C06F4"),
                    Services = model.Services,
                    State = model.FinanceStateId,
                    SwiftCode = model.BranchCode,
                    TaxCountry = model.FinanceCountryAltId,
                    TaxNumber = model.TaxId,
                    Url = "Url",
                    VenueID = 1,
                    RoutingNumber = model.RoutingNumber,
                    BankAccountType = model.BankAccountType,
                    AccountType = model.AccountType,
                    Document = model.Document
                });
                if (createGuideCommandResult.Success)
                {
                    return new CreateGuideResponseViewModel
                    {
                        Success = true,
                        IsSaving = true,
                    };
                }
                else
                {
                    return new CreateGuideResponseViewModel
                    {
                        Success = false,
                        IsSaving = false
                    };
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
                return new CreateGuideResponseViewModel { };
            }
        }

        [Route("api/Guide/Confirm/{guideId}/{approveStatusId}")]
        public async Task<ConfirmGuideResponseModel> GuideConfirm(int guideId, int approveStatusId)
        {
            var data = await _sessionProvider.Get();
            if (data.IsAuthenticated)
            {
                //sample API call
                await _commandSender.Send(new Contracts.Commands.Redemption.GuideConfirmCommand
                {
                    Id = guideId,
                    IsEnabled = (FIL.Contracts.Enums.ApproveStatus)approveStatusId == FIL.Contracts.Enums.ApproveStatus.Success ? true : false,
                    ApproveStatus = (FIL.Contracts.Enums.ApproveStatus)approveStatusId,
                    ModifiedBy = data.User.AltId
                });
                return new ConfirmGuideResponseModel
                {
                    Success = true
                };
            }
            else
            {
                return new ConfirmGuideResponseModel { };
            }
        }

        [Route("api/Order/Confirm/{approveStatusId}/{transactionId}")]
        public async Task<ConfirmGuideResponseModel> OrderConfirm(int approveStatusId, int transactionId)
        {
            var data = await _sessionProvider.Get();
            if (data.IsAuthenticated)
            {
                //sample API call
                await _commandSender.Send(new Contracts.Commands.Redemption.GuideOrderDetailsCommand
                {
                    OrderStatusId = (FIL.Contracts.Enums.ApproveStatus)approveStatusId,
                    TransactionId = transactionId,
                    ModifiedBy = data.User.AltId
                });
                return new ConfirmGuideResponseModel
                {
                    Success = true
                };
            }
            else
            {
                return new ConfirmGuideResponseModel { };
            }
        }

        [Route("api/Get/Orders/{orderStatusId}")]
        public async Task<GuideOrderResponseModel> GuideConfirm(int orderStatusId)
        {
            var data = await _sessionProvider.Get();
            if (data.IsAuthenticated)
            {
                try
                {
                    //sample API call
                    var queryResult = await _querySender.Send(new GuideOrderDetailsGetAllQuery
                    {
                        UserId = data.User.Id,
                        UserAltId = data.User.AltId,
                        RolesId = (int)data.User.RolesId,
                        OrderStatusId = orderStatusId
                    });
                    return new GuideOrderResponseModel
                    {
                        Success = true,
                        OrderDetails = queryResult.OrderDetails,
                        ApprovedByUsers = queryResult.ApprovedByUsers
                    };
                }
                catch (Exception e)
                {
                    return new GuideOrderResponseModel { };
                }
            }
            else
            {
                return new GuideOrderResponseModel { };
            }
        }

        [HttpGet]
        [Route("api/Guide/GetAll/{orderStatusId}")]
        public async Task<GuideDetailsGetAllResponseViewModal> GuideGetAll(int orderStatusId)
        {
            var queryResult = await _querySender.Send(new Contracts.Queries.Redemption.GuideDetailsGetAllQuery()
            {
                OrderStatusId = orderStatusId
            });
            return new GuideDetailsGetAllResponseViewModal
            {
                GuideDetails = queryResult.GuideDetails,
                ApprovedByUsers = queryResult.ApprovedByUsers
            };
        }

        [HttpGet]
        [Route("api/Guide/Get")]
        public async Task GuideGet()
        {
            var queryResult = await _querySender.Send(new Contracts.Queries.Redemption.GuideDetailsQuery
            {
                GuideId = 5
            });
        }

        [HttpGet]
        [Route("api/Guide/Services")]
        public async Task<ServiceResponseViewModel> GuideServices()
        {
            var queryResult = await _querySender.Send(new Contracts.Queries.Redemption.ServicesQuery());
            return new ServiceResponseViewModel
            {
                Services = queryResult.Services
            };
        }

        [HttpGet]
        [Route("api/Guide/Languages")]
        public async Task<LanguageResponseViewModel> GuideLanguages()
        {
            var queryResult = await _querySender.Send(new Contracts.Queries.Redemption.LanguagesQuery());
            return new LanguageResponseViewModel
            {
                Languages = queryResult.Languages
            };
        }

        [HttpGet]
        [Route("api/guide/edit/{userAltId}")]
        public async Task<GuideEditResponseModel> GetGuideEditDetails(Guid userAltId)
        {
            var queryResult = await _querySender.Send(new Contracts.Queries.Redemption.GuideEditQuery
            {
                UserId = userAltId
            });

            return new GuideEditResponseModel
            {
                UserAddressDetail = queryResult.UserAddressDetail,
                UserAddressDetailMapping = queryResult.UserAddressDetailMapping,
                User = queryResult.User,
                GuideDetail = queryResult.GuideDetail,
                GuideDocumentMappings = queryResult.GuideDocumentMappings,
                GuideFinanceMapping = queryResult.GuideFinanceMap,
                GuidePlaceMappings = queryResult.GuidePlaceMappings,
                //GuidePlaces = queryResult.GuidePlaces,
                GuideServices = queryResult.GuideServices,
                Services = queryResult.Services,
                MasterFinanceDetails = queryResult.MasterFinanceDetails
            };
        }
    }
}