using FIL.Contracts.Models;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Redemption
{
    public class GuideDetailsResult
    {
        public GuideMaster GuideMaster { get; set; }
        public GuideMasterDetail GuideMasterDetail { get; set; }

        public List<MasterFinanceDetails> MasterFinanceDetails { get; set; }
        public List<GuideServices> GuideServices { get; set; }
        public List<GuidePlaceMappings> GuidePlaceMappings { get; set; }
        public List<GuideFinanceMappings> GuideFinanceMappings { get; set; }
        public List<GuideDocumentMappings> GuideDocumentMappings { get; set; }
    }

    public class GuideEditQueryResult
    {
        public FIL.Contracts.DataModels.User User { get; set; }
        public FIL.Contracts.DataModels.UserAddressDetail UserAddressDetail { get; set; }
        public UserAddressDetailMapping UserAddressDetailMapping { get; set; }
        public FIL.Contracts.DataModels.Redemption.Redemption_GuideDetails GuideDetail { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Redemption_GuidePlaceMappings> GuidePlaceMappings { get; set; }
        public List<Models.Event> GuidePlaces { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Redemption_GuideServices> GuideServices { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Services> Services { get; set; }
        public FIL.Contracts.DataModels.Redemption.Redemption_GuideFinanceMappings GuideFinanceMap { get; set; }
        public FIL.Contracts.DataModels.Redemption.MasterFinanceDetails MasterFinanceDetails { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Redemption_GuideDocumentMappings> GuideDocumentMappings { get; set; }
    }

    public class UserAddressDetailMapping
    {
        public City ResidentialCity { get; set; }
        public State ResidentialState { get; set; }
        public FIL.Contracts.Models.Country ResidentialCountry { get; set; }
        public City FinancialCity { get; set; }
        public State FinancialState { get; set; }
        public FIL.Contracts.Models.Country FinancialCountry { get; set; }
    }

    public class OrderDetails
    {
        public long Id { get; set; }
        public Guid AltId { get; set; }
        public long TransactionId { get; set; }
        public int OrderStatusId { get; set; }

        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedUtc { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class MasterFinanceDetails
    {
        public int Id { get; set; }
        public int CurrenyId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }

        public int AccountTypeId { get; set; }
        public int BankAccountTypeId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string BranchCode { get; set; }
        public string TaxId { get; set; }
        public string RoutingNumber { get; set; }
    }

    public class GuideServices
    {
        public long Id { get; set; }
        public long GuideId { get; set; }
        public int ServiceId { get; set; }
        public string Notes { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class GuidePlaceMappings
    {
        public long Id { get; set; }
        public long GuideId { get; set; }
        public long EventId { get; set; }
        public int ApproveStatusId { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedUtc { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class GuideMaster
    {
        public long Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailId { get; set; }
        public string phoneCode { get; set; }
        public string phoneNumber { get; set; }
        public string role { get; set; }
        public string password { get; set; }
    }

    public class GuideFinanceMappings
    {
        public long Id { get; set; }
        public long GuideId { get; set; }
        public int MasterFinanceDetailId { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class GuideDocumentMappings
    {
        public long Id { get; set; }
        public long GuideId { get; set; }
        public long DocumentID { get; set; }
        public string DocumentSouceURL { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class GuideMasterDetail
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int VenueId { get; set; }
        public string LanguageId { get; set; }
        public FIL.Contracts.Enums.ApproveStatus ApproveStatusId { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedUtc { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class CountryTaxTypeMappings
    {
        public long Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}