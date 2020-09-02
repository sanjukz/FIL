using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Logging;
using System;

namespace FIL.Api.Providers.Transaction
{
    public interface ISaveGuestUserProvider
    {
        bool SaveGuestUsers(GuestUserDetail guestUserDetail, TransactionDetail transactionDetail);
    }

    public class SaveGuestUserProvider : ISaveGuestUserProvider
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IGuestDetailRepository _guestDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public SaveGuestUserProvider(ILogger logger, ISettings settings,
            IGuestDetailRepository guestDetailRepository,
                 ICountryRepository countryRepository
            )
        {
            _countryRepository = countryRepository;
            _guestDetailRepository = guestDetailRepository;
            _logger = logger;
        }

        public bool SaveGuestUsers(GuestUserDetail guestUserDetail, TransactionDetail transactionDetail)
        {
            try
            {
                var countryData = _countryRepository.GetByAltId(guestUserDetail.PhoneCode);
                var newGuest = _guestDetailRepository.Save(new FIL.Contracts.DataModels.GuestDetail
                {
                    TransactionDetailId = transactionDetail.Id,
                    Age = guestUserDetail.Age.ToString(),
                    DocumentNumber = guestUserDetail.IdentityNumber,
                    Email = guestUserDetail.Email,
                    FirstName = guestUserDetail.FirstName,
                    LastName = guestUserDetail.LastName,
                    CustomerDocumentTypeId = guestUserDetail.IdentityType == 0 ? 1 : guestUserDetail.IdentityType,
                    PhoneNumber = guestUserDetail.PhoneNumber,
                    PhoneCode = countryData == null ? "NA" : countryData.Phonecode.ToString(),
                    GenderId = guestUserDetail.Gender == 0 ? Gender.Male : (Gender)guestUserDetail.Gender,
                    IsEnabled = true,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                });
                return true;
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return false;
            }
        }
    }
}