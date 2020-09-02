using FIL.Api.Repositories;
using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.QueryResults.Redemption;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Redemption
{
    public class GuideDetailsQueryHandler : IQueryHandler<GuideDetailsQuery, GuideDetailsResult>
    {
        private readonly IGuideDetailsRepository _GuideDetailsRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IOrderDetailsRepository _OrderDetailsRepository;
        private readonly IGuidePlaceMappingsRepository _GuidePlaceMappingsRepository;
        private readonly IGuideServicesRepository _GuideServicesRepository;
        private readonly IMasterFinanceDetailsRepository _MasterFinanceDetailsRepository;
        private readonly IGuideDocumentMappingsRepository _GuideDocumentMappingsRepository;
        private readonly IGuideFinanceMappingsRepository _GuideFinanceMappingsRepository;
        private readonly ICountryTaxTypeMappingsRepository _CountryTaxTypeMappingsRepository;
        private readonly FIL.Logging.ILogger _logger;

        public GuideDetailsQueryHandler(IGuideDetailsRepository GuideDetailsRepository,
        IUserRepository UserRepository,
        IOrderDetailsRepository OrderDetailsRepository,
        IGuidePlaceMappingsRepository GuidePlaceMappingsRepository,
        IGuideServicesRepository GuideServicesRepository,
        IMasterFinanceDetailsRepository MasterFinanceDetailsRepository,
        IGuideDocumentMappingsRepository GuideDocumentMappingsRepository,
        IGuideFinanceMappingsRepository GuideFinanceMappingsRepository,
        ICountryTaxTypeMappingsRepository CountryTaxTypeMappingsRepository,
        FIL.Logging.ILogger logger)
        {
            _GuideDetailsRepository = GuideDetailsRepository;
            _UserRepository = UserRepository;
            _OrderDetailsRepository = OrderDetailsRepository;
            _GuidePlaceMappingsRepository = GuidePlaceMappingsRepository;
            _GuideServicesRepository = GuideServicesRepository;
            _MasterFinanceDetailsRepository = MasterFinanceDetailsRepository;
            _GuideDocumentMappingsRepository = GuideDocumentMappingsRepository;
            _GuideFinanceMappingsRepository = GuideFinanceMappingsRepository;
            _CountryTaxTypeMappingsRepository = CountryTaxTypeMappingsRepository;
            _logger = logger;
        }

        public GuideDetailsResult Handle(GuideDetailsQuery query)
        {
            GuideDetailsResult guideDetailsResult = new GuideDetailsResult();
            try
            {
                Contracts.DataModels.Redemption.Redemption_GuideDetails GuideDetails = _GuideDetailsRepository.Get(query.GuideId);
                Contracts.DataModels.User GuideMaster = _UserRepository.Get(GuideDetails.UserId);
                var GuideFinanceMappings = _GuideFinanceMappingsRepository.GetAllByGuideId(GuideDetails.Id);

                var GuideServices = _GuideServicesRepository.GetAllByGuideId(GuideDetails.Id);

                var GuidePlaceMappings = _GuidePlaceMappingsRepository.GetAllByGuideId(GuideDetails.Id);

                var GuideDocumentMappings = _GuideDocumentMappingsRepository.GetAllByGuideId(GuideDetails.Id);

                guideDetailsResult.GuideMaster = new GuideMaster();
                guideDetailsResult.GuideMaster.emailId = GuideMaster.Email;
                guideDetailsResult.GuideMaster.firstName = GuideMaster.FirstName;
                guideDetailsResult.GuideMaster.Id = GuideMaster.Id;
                guideDetailsResult.GuideMaster.lastName = GuideMaster.LastName;
                guideDetailsResult.GuideMaster.phoneCode = GuideMaster.PhoneCode;
                guideDetailsResult.GuideMaster.phoneNumber = GuideMaster.PhoneNumber;

                guideDetailsResult.GuideMasterDetail = new GuideMasterDetail();
                guideDetailsResult.GuideMasterDetail.ApprovedBy = GuideDetails.ApprovedBy;
                guideDetailsResult.GuideMasterDetail.ApprovedUtc = GuideDetails.ApprovedUtc;
                guideDetailsResult.GuideMasterDetail.ApproveStatusId = GuideDetails.ApproveStatusId;
                guideDetailsResult.GuideMasterDetail.Id = GuideDetails.Id;
                guideDetailsResult.GuideMasterDetail.IsEnabled = GuideDetails.IsEnabled;
                guideDetailsResult.GuideMasterDetail.LanguageId = GuideDetails.LanguageId;

                guideDetailsResult.MasterFinanceDetails = new List<MasterFinanceDetails>();
                foreach (Contracts.DataModels.Redemption.Redemption_GuideFinanceMappings GuideFinanceMapping in GuideFinanceMappings)
                {
                    Contracts.DataModels.Redemption.MasterFinanceDetails masterFinanceDetails = _MasterFinanceDetailsRepository.Get(GuideFinanceMappings.ToList()[0].MasterFinanceDetailId);
                    FIL.Contracts.QueryResults.Redemption.MasterFinanceDetails masterFinanceDetail = new MasterFinanceDetails();

                    masterFinanceDetail.AccountNumber = masterFinanceDetails.AccountNumber;
                    masterFinanceDetail.AccountTypeId = (int)masterFinanceDetails.AccountTypeId;
                    masterFinanceDetail.BankAccountTypeId = (int)masterFinanceDetails.BankAccountTypeId;
                    masterFinanceDetail.BankName = masterFinanceDetails.BankName;
                    masterFinanceDetail.BranchCode = masterFinanceDetails.BranchCode;
                    masterFinanceDetail.CountryId = masterFinanceDetails.CountryId;
                    masterFinanceDetail.CurrenyId = masterFinanceDetails.CurrenyId;
                    masterFinanceDetail.Id = masterFinanceDetails.Id;
                    masterFinanceDetail.RoutingNumber = masterFinanceDetails.RoutingNumber;
                    masterFinanceDetail.StateId = masterFinanceDetails.StateId;
                    masterFinanceDetail.TaxId = masterFinanceDetails.TaxId;

                    guideDetailsResult.MasterFinanceDetails.Add(masterFinanceDetail);
                }

                guideDetailsResult.GuideServices = new List<GuideServices>();
                foreach (Contracts.DataModels.Redemption.Redemption_GuideServices GuideService in GuideServices)
                {
                    FIL.Contracts.QueryResults.Redemption.GuideServices obj = new GuideServices();

                    obj.GuideId = GuideService.GuideId;
                    obj.Id = GuideService.Id;
                    obj.IsEnabled = GuideService.IsEnabled;
                    obj.Notes = GuideService.Notes;
                    obj.ServiceId = GuideService.ServiceId;

                    guideDetailsResult.GuideServices.Add(obj);
                }

                guideDetailsResult.GuidePlaceMappings = new List<GuidePlaceMappings>();
                foreach (Contracts.DataModels.Redemption.Redemption_GuidePlaceMappings GuidePlaceMapping in GuidePlaceMappings)
                {
                    FIL.Contracts.QueryResults.Redemption.GuidePlaceMappings obj = new GuidePlaceMappings();

                    obj.ApprovedBy = GuidePlaceMapping.ApprovedBy;
                    obj.ApprovedUtc = GuidePlaceMapping.ApprovedUtc;
                    obj.ApproveStatusId = GuidePlaceMapping.ApproveStatusId;
                    obj.EventId = GuidePlaceMapping.EventId;
                    obj.GuideId = GuidePlaceMapping.GuideId;
                    obj.Id = GuidePlaceMapping.Id;
                    obj.IsEnabled = GuidePlaceMapping.IsEnabled;

                    guideDetailsResult.GuidePlaceMappings.Add(obj);
                }

                guideDetailsResult.GuideDocumentMappings = new List<GuideDocumentMappings>();
                foreach (Contracts.DataModels.Redemption.Redemption_GuideDocumentMappings GuideDocumentMapping in GuideDocumentMappings)
                {
                    FIL.Contracts.QueryResults.Redemption.GuideDocumentMappings obj = new GuideDocumentMappings();

                    obj.DocumentID = GuideDocumentMapping.DocumentID;
                    obj.DocumentSouceURL = GuideDocumentMapping.DocumentSouceURL;
                    obj.GuideId = GuideDocumentMapping.GuideId;
                    obj.Id = GuideDocumentMapping.Id;
                    obj.IsEnabled = GuideDocumentMapping.IsEnabled;

                    guideDetailsResult.GuideDocumentMappings.Add(obj);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new GuideDetailsResult();
            }
            return guideDetailsResult;
        }
    }
}