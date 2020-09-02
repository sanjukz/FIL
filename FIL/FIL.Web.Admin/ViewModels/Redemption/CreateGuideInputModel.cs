using System;
using System.Collections.Generic;
using FIL.Contracts.Models;

namespace FIL.Web.Kitms.Feel.ViewModels.Redemption
{
    public class CreateGuideInputModel
    {
        public List<int> EventIDs { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<int> LanguageId { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public int CurrencyId { get; set; }
        public string FinanceCountryAltId { get; set; }
        public int FinanceStateId { get; set; }
        public FIL.Contracts.Enums.BankAccountType BankAccountType { get; set; }
        public FIL.Contracts.Enums.AccountType AccountType { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string BranchCode { get; set; }
        public string TaxId { get; set; }
        public string RoutingNumber { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Services> Services { get; set; }
        public string ServiceNotes { get; set; }
        public int Document { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string Zip { get; set; }
        public string ResidentCountryAltId { get; set; }
        public int ResidentStateId { get; set; }
        public int ResidentCityId { get; set; }
    }
}
