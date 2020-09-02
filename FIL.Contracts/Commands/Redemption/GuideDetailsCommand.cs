using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Redemption
{
    public class GuideDetailsCommand : Contracts.Interfaces.Commands.ICommandWithResult<GuideDetailsCommandResult> //Contracts.Interfaces.Commands.ICommandWithResult<GuideDetailsCommandResult>
    {
        public int Id { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        public int Country { get; set; }
        public int Place { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string CountryCode { get; set; }
        public string MobileNo { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public int CityId { get; set; }
        public string ZipCode { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string SwiftCode { get; set; }
        public string RoutingNumber { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public int SellerType { get; set; }
        public string TaxNumber { get; set; }
        public int CurrencyId { get; set; }
        public string TaxCountry { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Services> Services { get; set; }
        public int[] ServiceIDs { get; set; }
        public string Notes { get; set; }
        public string AddressProofType { get; set; }
        public string Url { get; set; }
        public string IPAddress { get; set; }
        public List<int> LanguageIds { get; set; }
        public int VenueID { get; set; }
        public List<int> EventIDs { get; set; }
        public FIL.Contracts.Enums.BankAccountType BankAccountType { get; set; }
        public FIL.Contracts.Enums.AccountType AccountType { get; set; }
        public int Document { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class Service
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
    }

    public class GuideDetailsCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public long UserId { get; set; }
    }

    public class GuideConfirmCommand : BaseCommand //Contracts.Interfaces.Commands.ICommandWithResult<GuideDetailsCommandResult>
    {
        public long Id { get; set; }
        public bool IsEnabled { get; set; }
        public Contracts.Enums.ApproveStatus ApproveStatus { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class GuideOrderDetailsCommand : BaseCommand
    {
        public long TransactionId { get; set; }
        public FIL.Contracts.Enums.ApproveStatus OrderStatusId { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}