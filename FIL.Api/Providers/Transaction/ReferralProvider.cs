using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.DataModels;
using FIL.Logging;
using System;

namespace FIL.Api.Providers.Transaction
{
    public interface IReferralProvider
    {
        FIL.Contracts.DataModels.Referral GetReferral(string referralId,
            long eventId,
            Guid modifiedBy);
    }

    public class ReferralProvider : IReferralProvider
    {
        private readonly IReferralRepository _referralRepository;
        private readonly FIL.Logging.ILogger _logger;

        public ReferralProvider(ILogger logger, ISettings settings,
                 IReferralRepository referralRepository
            )
        {
            _referralRepository = referralRepository;
            _logger = logger;
        }

        public FIL.Contracts.DataModels.Referral GetReferral(
            string referralId,
            long eventId,
            Guid modifiedBy
            )
        {
            try
            {
                var Referral = _referralRepository.GetBySlug(referralId);
                if (Referral != null)
                {
                    return Referral;
                }
                else
                {
                    var referral = new Referral
                    {
                        Name = referralId,
                        Code = referralId,
                        Description = "",
                        AltId = Guid.NewGuid(),
                        EventId = eventId,
                        IsEnabled = true,
                        Slug = referralId,
                        CreatedBy = modifiedBy,
                        CreatedUtc = DateTime.UtcNow,
                        ModifiedBy = modifiedBy,
                        UpdatedBy = modifiedBy,
                        UpdatedUtc = DateTime.UtcNow
                    };
                    return _referralRepository.Save(referral);
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return new Referral
                {
                };
            }
        }
    }
}