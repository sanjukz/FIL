using CurrencyConverter;
using FIL.Contracts.Models;
using FIL.Foundation.Senders;
using FIL.Logging.Enums;
using FIL.Web.Feel.ViewModels.DeliveryOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.Modules.SiteExtensions
{
    public interface IGeoCurrency
    {
        List<ViewModels.CategoryHomePage.CategoryEventContainer> UpdateCategoriesCurrency(List<ViewModels.CategoryHomePage.CategoryEventContainer> categories,
            HttpContext _context);
        List<Contracts.Models.CategoryEventContainer> UpdateCategoriesCurrency_v2(List<Contracts.Models.CategoryEventContainer> categories,
            HttpContext _context);

        Contracts.QueryResults.FeelEventLearnPage.FeelEventLearnPageQueryResult UpdateCategoriesCurrency_v2_2(HttpContext _context, Contracts.QueryResults.FeelEventLearnPage.FeelEventLearnPageQueryResult queryResult);
        Contracts.QueryResults.TicketCategories.TicketCategoryQueryResult UpdateCategoriesCurrency_v3(Contracts.QueryResults.TicketCategories.TicketCategoryQueryResult categories,
            HttpContext _context);
        CurrencyType GetCurrencyCode(int currencyID);
        CurrencyType GetCurrencyID(string currencyCode);

        string GetSessionCurrency(HttpContext context);

        UpdateTransactionResponseViewModel DeliveryOptionsUpdate(UpdateTransactionResponseViewModel updateTransactionResponseViewModel,
            HttpContext context);

        Contracts.DataModels.SearchVenue SearchVenueUpdate(Contracts.DataModels.SearchVenue searchVenue, HttpContext context);
        EventTicketAttribute eventTicketAttributeUpdate(EventTicketAttribute eventTicketAttribute,
            HttpContext _context, string CurrencyCode = "");
        List<PlaceDetail> UpdatePlaceDetails(List<PlaceDetail> placeDetails,
            HttpContext _context);
        List<FIL.Contracts.DataModels.MasterBudgetRange> UpdateBudgetRange(List<FIL.Contracts.DataModels.MasterBudgetRange> masterBudgetRanges,
           HttpContext _context);
        Decimal GetConvertedDiscountAmount(Decimal discountAmount, int currentCurrencyId, HttpContext _context);
        List<FIL.Contracts.Models.Transaction> UpdateTransactions(List<FIL.Contracts.Models.Transaction> transactions,
           HttpContext _context);
        decimal Exchange(decimal amount, string sourceCurrencyCode);
        TransactionDetail updateTransactionDetail(TransactionDetail transactionDetail,
           HttpContext _context, int currentCurrencyId, string CurrencyCode = "");
    }
    public class GeoCurrency : IGeoCurrency
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IQuerySender _querySender;
        private readonly bool _IsGeoCurrencySelectionEnabled = true;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly FIL.Logging.ILogger _logger;

        public string GetSessionCurrency(HttpContext context)
        {
            string TargetCurrencyCode = string.Empty; //"USD"; //default or not loaded yet from home controller.
            try
            {
                if (context.Request.Cookies["user_currency"] == null)
                {
                    if (context.Request.Host.Host.ToLower().EndsWith(".uk"))
                        TargetCurrencyCode = "GBP";
                    else if (context.Request.Host.Host.ToLower().EndsWith(".in"))
                        TargetCurrencyCode = "INR";
                    else if (context.Request.Host.Host.ToLower().EndsWith(".au"))
                        TargetCurrencyCode = "AUD";
                    else if (context.Request.Host.Host.ToLower().EndsWith(".de"))
                        TargetCurrencyCode = "EUR";
                    else if (context.Request.Host.Host.ToLower().EndsWith(".fr"))
                        TargetCurrencyCode = "EUR";
                    else if (context.Request.Host.Host.ToLower().EndsWith(".es"))
                        TargetCurrencyCode = "EUR";
                    else if (context.Request.Host.Host.ToLower().EndsWith(".nz"))
                        TargetCurrencyCode = "NZD";
                    else TargetCurrencyCode = "USD";

                    //create cookie so that frontend can show selected value.
                    context.Response.Cookies.Append("user_currency",
                   TargetCurrencyCode,
                   new Microsoft.AspNetCore.Http.CookieOptions
                   {
                       Expires = DateTimeOffset.Now.AddYears(1)
                   });
                }
                else
                {
                    TargetCurrencyCode = context.Request.Cookies["user_currency"];
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                TargetCurrencyCode = "USD";
            }

            return TargetCurrencyCode;
        }

        public GeoCurrency(IQuerySender querySender, IMemoryCache memoryCache, ICurrencyConverter currencyConverter, FIL.Logging.ILogger logger)
        {
            _querySender = querySender;
            _memoryCache = memoryCache;
            _currencyConverter = currencyConverter;
            _logger = logger;
        }

        public CurrencyType GetCurrencyCode(int currencyID)
        {
            var queryResult = GetCurrencyTypesQueryResult();
            Contracts.Models.CurrencyType _ct = queryResult.Result.currencyTypes.Where(x => x.Id == currencyID).FirstOrDefault();
            return _ct;
        }

        public CurrencyType GetCurrencyID(string currencyCode)
        {
            var queryResult = GetCurrencyTypesQueryResult();
            CurrencyType _ct;
            if (queryResult.Result.currencyTypes != null)
            {
                _ct = queryResult.Result.currencyTypes.Where(x => x.Code == currencyCode).FirstOrDefault();
                if (_ct == null)
                {
                    _ct = queryResult.Result.currencyTypes.Where(x => x.Code == "USD").FirstOrDefault();
                }
                return _ct;
            }
            return new CurrencyType();
        }

        private Task<Contracts.QueryResults.CurrencyTypesQueryResult> GetCurrencyTypesQueryResult()
        {
            if (!_memoryCache.TryGetValue($"__GetCurrencyTypesQueryResult", out Task<Contracts.QueryResults.CurrencyTypesQueryResult> queryResult))
            {
                queryResult = _querySender.Send(new Contracts.Queries.CurrencyTypes.CurrencyTypesQuery { });
                _memoryCache.Set($"__GetCurrencyTypesQueryResult", queryResult, DateTime.Now.AddMinutes(60));
            }
            else
            {
                return (Task<Contracts.QueryResults.CurrencyTypesQueryResult>)_memoryCache.Get("__GetCurrencyTypesQueryResult");
            }
            return queryResult;
        }

        public List<ViewModels.CategoryHomePage.CategoryEventContainer> UpdateCategoriesCurrency(List<ViewModels.CategoryHomePage.CategoryEventContainer> categories,
            HttpContext _context)
        {
            var allCategories = new List<ViewModels.CategoryHomePage.CategoryEventContainer>(); //new categories list is to hold the filtered categories items
            try
            {
                if (_IsGeoCurrencySelectionEnabled && categories != null)
                {
                    string TargetCurrencyCode = GetSessionCurrency(_context);
                    int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;

                    foreach (ViewModels.CategoryHomePage.CategoryEventContainer _cvc in categories)
                    {
                        if (_cvc.EventTicketAttribute.Any())
                        {
                            try
                            {
                                foreach (Contracts.Models.EventTicketAttribute _eta in _cvc.EventTicketAttribute)
                                {
                                    if (_eta.CurrencyId != 0)
                                    {
                                        _eta.Price = _currencyConverter.Exchange(_eta.Price, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                        _eta.SeasonPackagePrice = _currencyConverter.Exchange(_eta.SeasonPackagePrice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                        _eta.Specialprice = _currencyConverter.Exchange(_eta.Specialprice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                        _eta.SpecialSeasonPrice = _currencyConverter.Exchange(_eta.SpecialSeasonPrice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                        _eta.CurrencyId = TargetCurrencyID;
                                    }
                                }
                                if (_cvc.CurrencyType != null)
                                {
                                    _cvc.CurrencyType.Code = TargetCurrencyCode;
                                    _cvc.CurrencyType.Id = TargetCurrencyID;
                                    _cvc.CurrencyType.CountryId = GetCurrencyID(TargetCurrencyCode).CountryId;
                                    _cvc.CurrencyType.Name = GetCurrencyID(TargetCurrencyCode).Name;
                                    _cvc.CurrencyType.ExchangeRate = 0; //fix this?
                                    allCategories.Add(_cvc);
                                }
                            }
                            catch (Exception e)
                            {
                                _logger.Log(LogCategory.Error, new Exception("", e));
                            }
                        }
                    }
                }
                return allCategories;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return allCategories;
            }
        }

        public Contracts.QueryResults.FeelEventLearnPage.FeelEventLearnPageQueryResult UpdateCategoriesCurrency_v2_2(HttpContext _context, Contracts.QueryResults.FeelEventLearnPage.FeelEventLearnPageQueryResult queryResult)
        {
            try
            {
                if (_IsGeoCurrencySelectionEnabled && queryResult != null)
                {
                    string TargetCurrencyCode = GetSessionCurrency(_context);
                    int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;

                    foreach (Contracts.Models.EventTicketAttribute _eta in queryResult.EventTicketAttribute)
                    {
                        try
                        {
                            if (_eta.CurrencyId != 0)
                            {
                                _eta.Price = _currencyConverter.Exchange(_eta.Price, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                _eta.SeasonPackagePrice = _currencyConverter.Exchange(_eta.SeasonPackagePrice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                _eta.Specialprice = _currencyConverter.Exchange(_eta.Specialprice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                _eta.SpecialSeasonPrice = _currencyConverter.Exchange(_eta.SpecialSeasonPrice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                _eta.CurrencyId = TargetCurrencyID;
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.Log(LogCategory.Error, new Exception("", e));
                        }
                    }
                    queryResult.CurrencyType.Code = TargetCurrencyCode;
                    queryResult.CurrencyType.Id = TargetCurrencyID;
                    queryResult.CurrencyType.CountryId = GetCurrencyID(TargetCurrencyCode).CountryId;
                    queryResult.CurrencyType.Name = GetCurrencyID(TargetCurrencyCode).Name;
                    queryResult.CurrencyType.ExchangeRate = 0; //fix this?
                }
                return queryResult;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return queryResult;
            }

        }

        public List<Contracts.Models.CategoryEventContainer> UpdateCategoriesCurrency_v2(List<Contracts.Models.CategoryEventContainer> categoryEventContainers,
            HttpContext _context)
        {
            List<CategoryEventContainer> categories = new List<CategoryEventContainer>();

            foreach (var item in categoryEventContainers)
            {
                if (item.Event != null)
                {
                    categories.Add(item);
                }
            }
            try
            {
                if (_IsGeoCurrencySelectionEnabled && categories != null && categories.Count > 0)
                {
                    string TargetCurrencyCode = GetSessionCurrency(_context);
                    int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                    foreach (Contracts.Models.CategoryEventContainer _cvc in categories)
                    {
                        foreach (Contracts.Models.EventTicketAttribute _eta in _cvc.EventTicketAttribute)
                        {
                            try
                            {
                                if (_eta.CurrencyId != 0)
                                {
                                    _eta.Price = _currencyConverter.Exchange(_eta.Price, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                    _eta.SeasonPackagePrice = _currencyConverter.Exchange(_eta.SeasonPackagePrice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                    _eta.Specialprice = _currencyConverter.Exchange(_eta.Specialprice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                    _eta.SpecialSeasonPrice = _currencyConverter.Exchange(_eta.SpecialSeasonPrice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                                    _eta.CurrencyId = TargetCurrencyID;
                                }
                            }
                            catch (Exception e)
                            {
                                _logger.Log(LogCategory.Error, new Exception("", e));
                            }
                        }
                        _cvc.CurrencyType.Code = TargetCurrencyCode;
                        _cvc.CurrencyType.Id = TargetCurrencyID;
                        _cvc.CurrencyType.CountryId = GetCurrencyID(TargetCurrencyCode).CountryId;
                        _cvc.CurrencyType.Name = GetCurrencyID(TargetCurrencyCode).Name;
                        _cvc.CurrencyType.ExchangeRate = 0; //fix this?
                    }
                }
                return categories;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return categories;
            }
        }

        public Contracts.QueryResults.TicketCategories.TicketCategoryQueryResult UpdateCategoriesCurrency_v3(Contracts.QueryResults.TicketCategories.TicketCategoryQueryResult categories,
            HttpContext _context)
        {
            try
            {
                if (_IsGeoCurrencySelectionEnabled && categories != null)
                {
                    string TargetCurrencyCode = GetSessionCurrency(_context);
                    int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;

                    foreach (Contracts.Models.EventTicketAttribute _eta in categories.EventTicketAttribute)
                    {
                        try
                        {
                            _eta.Price = _currencyConverter.Exchange(_eta.Price, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                            _eta.SeasonPackagePrice = _currencyConverter.Exchange(_eta.SeasonPackagePrice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                            _eta.Specialprice = _currencyConverter.Exchange(_eta.Specialprice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                            _eta.SpecialSeasonPrice = _currencyConverter.Exchange(_eta.SpecialSeasonPrice, GetCurrencyCode(_eta.CurrencyId).Code, TargetCurrencyCode);
                            _eta.CurrencyId = TargetCurrencyID;
                        }
                        catch (Exception e)
                        {
                            _logger.Log(LogCategory.Error, new Exception("", e));
                        }
                    }
                    categories.CurrencyType.Code = TargetCurrencyCode;
                    categories.CurrencyType.Id = TargetCurrencyID;
                    categories.CurrencyType.CountryId = GetCurrencyID(TargetCurrencyCode).CountryId;
                    categories.CurrencyType.Name = GetCurrencyID(TargetCurrencyCode).Name;
                    categories.CurrencyType.ExchangeRate = 0; //fix this?
                }
                return categories;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return categories;
            }
        }

        public UpdateTransactionResponseViewModel DeliveryOptionsUpdate(UpdateTransactionResponseViewModel updateTransactionResponseViewModel,
            HttpContext context)
        {
            if (_IsGeoCurrencySelectionEnabled && updateTransactionResponseViewModel != null)
            {
                string TargetCurrencyCode = GetSessionCurrency(context); //default or not loaded yet from home controller.
                int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                if (updateTransactionResponseViewModel.GrossTicketAmount.HasValue)
                    updateTransactionResponseViewModel.GrossTicketAmount = _currencyConverter.Exchange(updateTransactionResponseViewModel.GrossTicketAmount.Value, GetCurrencyCode(updateTransactionResponseViewModel.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionResponseViewModel.DeliveryCharges.HasValue)
                    updateTransactionResponseViewModel.DeliveryCharges = _currencyConverter.Exchange(updateTransactionResponseViewModel.DeliveryCharges.Value, GetCurrencyCode(updateTransactionResponseViewModel.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionResponseViewModel.ConvenienceCharges.HasValue)
                    updateTransactionResponseViewModel.ConvenienceCharges = _currencyConverter.Exchange(updateTransactionResponseViewModel.ConvenienceCharges.Value, GetCurrencyCode(updateTransactionResponseViewModel.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionResponseViewModel.ServiceCharge.HasValue)
                    updateTransactionResponseViewModel.ServiceCharge = _currencyConverter.Exchange(updateTransactionResponseViewModel.ServiceCharge.Value, GetCurrencyCode(updateTransactionResponseViewModel.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionResponseViewModel.DiscountAmount.HasValue)
                    updateTransactionResponseViewModel.DiscountAmount = _currencyConverter.Exchange(updateTransactionResponseViewModel.DiscountAmount.Value, GetCurrencyCode(updateTransactionResponseViewModel.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionResponseViewModel.NetTicketAmount.HasValue)
                    updateTransactionResponseViewModel.NetTicketAmount = _currencyConverter.Exchange(updateTransactionResponseViewModel.NetTicketAmount.Value, GetCurrencyCode(updateTransactionResponseViewModel.CurrencyId).Code, TargetCurrencyCode);

                updateTransactionResponseViewModel.CurrencyId = TargetCurrencyID;
            }

            return updateTransactionResponseViewModel;
        }

        public Contracts.DataModels.SearchVenue SearchVenueUpdate(Contracts.DataModels.SearchVenue searchVenue, HttpContext context)
        {
            if (_IsGeoCurrencySelectionEnabled && searchVenue != null)
            {
                string TargetCurrencyCode = GetSessionCurrency(context); //default or not loaded yet from home controller.
                searchVenue.Price = _currencyConverter.Exchange(searchVenue.Price, searchVenue.Currency, TargetCurrencyCode);
                searchVenue.AdultPrice = _currencyConverter.Exchange(searchVenue.AdultPrice, searchVenue.Currency, TargetCurrencyCode);
                searchVenue.ChildPrice = _currencyConverter.Exchange(searchVenue.ChildPrice, searchVenue.Currency, TargetCurrencyCode);
                searchVenue.Currency = TargetCurrencyCode;
            }
            return searchVenue;
        }

        public EventTicketAttribute eventTicketAttributeUpdate(EventTicketAttribute eventTicketAttribute,
            HttpContext _context, string CurrencyCode = "")
        {
            try
            {
                if (_IsGeoCurrencySelectionEnabled && eventTicketAttribute != null)
                {
                    string TargetCurrencyCode = CurrencyCode != "" ? CurrencyCode : GetSessionCurrency(_context);
                    int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;

                    eventTicketAttribute.Price = _currencyConverter.Exchange(eventTicketAttribute.Price, GetCurrencyCode(eventTicketAttribute.CurrencyId).Code, TargetCurrencyCode);
                    eventTicketAttribute.SeasonPackagePrice = _currencyConverter.Exchange(eventTicketAttribute.SeasonPackagePrice, GetCurrencyCode(eventTicketAttribute.CurrencyId).Code, TargetCurrencyCode);
                    eventTicketAttribute.Specialprice = _currencyConverter.Exchange(eventTicketAttribute.Specialprice, GetCurrencyCode(eventTicketAttribute.CurrencyId).Code, TargetCurrencyCode);
                    eventTicketAttribute.SpecialSeasonPrice = _currencyConverter.Exchange(eventTicketAttribute.SpecialSeasonPrice, GetCurrencyCode(eventTicketAttribute.CurrencyId).Code, TargetCurrencyCode);
                    eventTicketAttribute.CurrencyId = TargetCurrencyID;
                }

                return eventTicketAttribute;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return eventTicketAttribute;
            }
        }

        public TransactionDetail updateTransactionDetail(TransactionDetail transactionDetail,
            HttpContext _context, int currentCurrencyId, string CurrencyCode = "")
        {
            try
            {
                if (_IsGeoCurrencySelectionEnabled && transactionDetail != null)
                {
                    string TargetCurrencyCode = CurrencyCode != "" ? CurrencyCode : GetSessionCurrency(_context);
                    int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                    transactionDetail.PricePerTicket = _currencyConverter.Exchange(transactionDetail.PricePerTicket, GetCurrencyCode(currentCurrencyId).Code, TargetCurrencyCode);
                    transactionDetail.ConvenienceCharges = _currencyConverter.Exchange((decimal)transactionDetail.ConvenienceCharges, GetCurrencyCode(currentCurrencyId).Code, TargetCurrencyCode);
                    transactionDetail.ServiceCharge = _currencyConverter.Exchange((decimal)transactionDetail.ServiceCharge, GetCurrencyCode(currentCurrencyId).Code, TargetCurrencyCode);
                }

                return transactionDetail;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return transactionDetail;
            }
        }

        public List<PlaceDetail> UpdatePlaceDetails(List<PlaceDetail> placeDetails,
            HttpContext _context)
        {
            try
            {
                if (_IsGeoCurrencySelectionEnabled && placeDetails.Any())
                {
                    string TargetCurrencyCode = GetSessionCurrency(_context);
                    int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                    foreach (PlaceDetail placeDetail in placeDetails)
                    {
                        if (placeDetail.CurrencyId != 0)
                        {
                            placeDetail.MaxPrice = _currencyConverter.Exchange(placeDetail.MaxPrice, GetCurrencyCode(placeDetail.CurrencyId).Code, TargetCurrencyCode);
                            placeDetail.MinPrice = _currencyConverter.Exchange(placeDetail.MinPrice, GetCurrencyCode(placeDetail.CurrencyId).Code, TargetCurrencyCode);
                            placeDetail.CurrencyId = TargetCurrencyID;
                            placeDetail.Currency = TargetCurrencyCode;
                        }
                    }
                }
                return placeDetails;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return placeDetails;
            }
        }

        public List<FIL.Contracts.DataModels.MasterBudgetRange> UpdateBudgetRange(List<FIL.Contracts.DataModels.MasterBudgetRange> masterBudgetRanges,
           HttpContext _context)
        {
            try
            {
                if (_IsGeoCurrencySelectionEnabled && masterBudgetRanges.Any())
                {
                    string TargetCurrencyCode = GetSessionCurrency(_context);
                    int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                    foreach (FIL.Contracts.DataModels.MasterBudgetRange masterBudgetRange in masterBudgetRanges)
                    {
                        if (masterBudgetRange.CurrencyId != 0)
                        {
                            masterBudgetRange.MaxPrice = _currencyConverter.Exchange(masterBudgetRange.MaxPrice, GetCurrencyCode(masterBudgetRange.CurrencyId).Code, TargetCurrencyCode);
                            masterBudgetRange.MinPrice = _currencyConverter.Exchange(masterBudgetRange.MinPrice, GetCurrencyCode(masterBudgetRange.CurrencyId).Code, TargetCurrencyCode);
                            masterBudgetRange.CurrencyId = TargetCurrencyID;
                        }
                    }
                }
                return masterBudgetRanges;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return masterBudgetRanges;
            }
        }

        public List<FIL.Contracts.Models.Transaction> UpdateTransactions(List<FIL.Contracts.Models.Transaction> transactions,
           HttpContext _context)
        {
            try
            {
                if (_IsGeoCurrencySelectionEnabled && transactions.Any())
                {
                    string TargetCurrencyCode = GetSessionCurrency(_context);
                    int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                    foreach (FIL.Contracts.Models.Transaction masterBudgetRange in transactions)
                    {
                        masterBudgetRange.NetTicketAmount = _currencyConverter.Exchange((decimal)masterBudgetRange.NetTicketAmount, GetCurrencyCode(masterBudgetRange.CurrencyId).Code, TargetCurrencyCode);
                        masterBudgetRange.CurrencyId = TargetCurrencyID;

                    }
                }
                return transactions;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return transactions;
            }
        }

        public Decimal GetConvertedDiscountAmount(Decimal discountAmount, int currentCurrencyId, HttpContext _context)
        {
            try
            {

                string TargetCurrencyCode = GetSessionCurrency(_context);
                discountAmount = _currencyConverter.Exchange(discountAmount, GetCurrencyCode(currentCurrencyId).Code, TargetCurrencyCode);
                return discountAmount;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return discountAmount;
            }
        }

        public decimal Exchange(decimal amount, string sourceCurrencyCode)
        {
            try
            {
                string targetCurrencyCode = "USD";
                amount = _currencyConverter.Exchange(amount, sourceCurrencyCode, targetCurrencyCode);
                return amount;
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return amount;
            }
        }
    }
}
