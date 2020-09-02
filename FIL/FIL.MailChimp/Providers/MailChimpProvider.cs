using Gma.QrCodeNet.Encoding.DataEncodation;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models;
using FIL.Contracts.QueryResults.CurrentOrderData;
using FIL.MailChimp.Models;
using FIL.Web.Core.Providers;
using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.MailChimp
{
    public interface IMailChimpProvider
    {
        Task AddBuyerCart(CurrentOrderDataQueryResult queryResult);
        Task AddFILMember(MCUserModel model, string country);
        Task AddBuyerOrder(Contracts.QueryResults.FeelOrderConfirmation.FeelOrderConfirmationQueryResult queryResult);
        MailChimpManager GetMailChimp();
        Task AddFILMemberAdditionalDetails(MCUserAdditionalDetailModel model);
        Task AddFILMemberLastDetails(MCUserAdditionalDetailModel model);
    }

    public class MailChimpProvider : IMailChimpProvider
    {
        private readonly ISessionProvider _sessionProvider;
        private readonly IClientIpProvider _clientIpProvider;
        private readonly ISettings _settings;
        public MailChimpProvider(
            ISessionProvider sessionProvider,
            IClientIpProvider clientIpProvider,
            ISettings settings
            )
        {
            _sessionProvider = sessionProvider;
            _clientIpProvider = clientIpProvider;
            _settings = settings;
        }
        public MailChimpManager GetMailChimp()
        {
            var apiKey = _settings.GetConfigSetting<string>(SettingKeys.Integration.MailChimp.ApiKey);
            return new MailChimpManager(apiKey);
        }

        private Product ConvertEventSubContainerToProduct(List<OrderConfirmationSubContainer> confirmationContainer, string currencyCode)
        {
            var variants = new List<Variant>();
            foreach (var container in confirmationContainer)
            {
                foreach (var subEvent in container.subEventContainer)
                {
                    foreach (var item in subEvent.EventTicketDetail)
                    {
                        var tc = subEvent.TicketCategory.ToList().Find(t => t.Id == item.TicketCategoryId);
                        var totalPrice = subEvent.TransactionDetail.Sum(t => t.PricePerTicket);
                        var eta = subEvent.EventTicketAttribute.Where(s => s.EventTicketDetailId == item.Id).FirstOrDefault();
                        variants.Add(new Variant
                        {
                            Id = item.Id.ToString(),
                            Title = tc.Name,
                            Price = eta.Price,
                            InventoryQuantity = eta.RemainingTicketForSale,
                            CreatedAt = subEvent.Event.CreatedUtc,
                        });
                    }
                }
            }
            return new Product
            {
                Id = confirmationContainer.FirstOrDefault().subEventContainer.FirstOrDefault().Event.Id.ToString(),
                Title = confirmationContainer.FirstOrDefault().subEventContainer.FirstOrDefault().Event.Name,
                Description = confirmationContainer.FirstOrDefault().subEventContainer.FirstOrDefault().Event.Description,
                Type = ((Contracts.Enums.EventType)confirmationContainer.FirstOrDefault().subEventContainer.FirstOrDefault().Event.EventTypeId).ToString(),
                Variants = variants,
                ImageUrl = $"https://static7.feelaplace.com/images/places/tiles/{confirmationContainer.FirstOrDefault().subEventContainer.FirstOrDefault().Event.AltId.ToString().ToUpper()}-ht-c1.jpg"
            };
        }

        private Collection<Line> ConvertEventSubContainerToLine(List<OrderConfirmationSubContainer> confirmationContainer, string currencyCode)
        {
            var lineItems = new Collection<Line>();
            foreach (var container in confirmationContainer)
            {
                foreach (var subEvent in container.subEventContainer)
                {
                    foreach (var item in subEvent.EventTicketDetail)
                    {
                        var tc = subEvent.TicketCategory.ToList().Find(t => t.Id == item.TicketCategoryId);
                        var eta = subEvent.EventTicketAttribute.Where(s => s.EventTicketDetailId == item.Id).FirstOrDefault();
                        var totalPrice = subEvent.TransactionDetail.Where(s => s.EventTicketAttributeId == eta.Id).FirstOrDefault().PricePerTicket;
                        lineItems.Add(new Line
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = subEvent.Event.Id.ToString(),
                            ProductTitle = subEvent.Event.Name,
                            ProductVariantId = item.Id.ToString(),
                            ProductVariantTitle = tc.Name,
                            Quantity = subEvent.TransactionDetail.Sum(t => t.TotalTickets),
                            Price = totalPrice
                        });
                    }
                }
            }
            return lineItems;
        }

        public async Task AddBuyerCart(CurrentOrderDataQueryResult queryResult)
        {
            var storeId = _settings.GetConfigSetting<string>(SettingKeys.Integration.MailChimp.StoreId);
            var mailChimpApi = GetMailChimp();
            var user = await _sessionProvider.Get();
            var customer = await mailChimpApi.GetCustomerByEmail(storeId, user.IsAuthenticated ? user.User.Email : "");
            if ((customer == null || customer.Id == null) && user.User != null)
            {
                customer = await mailChimpApi.ECommerceStores.Customers(storeId).AddAsync(new Customer
                {
                    Id = user.User.AltId.ToString(),
                    EmailAddress = user.User.Email,
                    OptInStatus = true,
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    TotalSpent = queryResult.Transaction.NetTicketAmount
                });
            }
            try
            {
                //var product = await mailChimpApi.ECommerceStores.Products(storeId).GetAsync(eventContainer.Event.Id.ToString());
                var product = ConvertEventSubContainerToProduct(queryResult.orderConfirmationSubContainer, queryResult.CurrencyType.Code);
                await mailChimpApi.ECommerceStores.Products(storeId).UpdateAsync(product.Id, product);
            }
            catch
            {
                var product = ConvertEventSubContainerToProduct(queryResult.orderConfirmationSubContainer, queryResult.CurrencyType.Code);
                await mailChimpApi.ECommerceStores.Products(storeId).AddAsync(product);
            }

            await mailChimpApi.ECommerceStores.Carts(storeId).AddAsync(new Cart
            {
                Id = queryResult.Transaction.AltId.ToString(),
                Customer = customer,
                CurrencyCode = CurrencyCode.USD,
                OrderTotal = queryResult.Transaction.NetTicketAmount,
                Lines = ConvertEventSubContainerToLine(queryResult.orderConfirmationSubContainer, queryResult.CurrencyType.Code)
            });
        }

        public async Task AddBuyerOrder(Contracts.QueryResults.FeelOrderConfirmation.FeelOrderConfirmationQueryResult queryResult)
        {
            var storeId = _settings.GetConfigSetting<string>(SettingKeys.Integration.MailChimp.StoreId);
            var mailChimpApi = GetMailChimp();
            var user = await _sessionProvider.Get();
            var customer = await mailChimpApi.GetCustomerByEmail(storeId, user.IsAuthenticated ? user.User.Email : "");
            if (customer.Id == null && user.User != null)
            {
                customer = await mailChimpApi.ECommerceStores.Customers(storeId).AddAsync(new Customer
                {
                    Id = user.User.AltId.ToString(),
                    EmailAddress = user.User.Email,
                    OptInStatus = true,
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    TotalSpent = queryResult.Transaction.NetTicketAmount
                });
            }

            await mailChimpApi.ECommerceStores.Orders(storeId).AddAsync(new Order
            {
                Id = queryResult.Transaction.AltId.ToString(),
                StoreId = storeId,
                Customer = customer,
                CurrencyCode = CurrencyCode.USD,
                OrderTotal = (decimal)queryResult.Transaction.NetTicketAmount,
                Lines = ConvertEventSubContainerToLine(queryResult.orderConfirmationSubContainer, queryResult.CurrencyType.Code)
            });
        }

        public async Task AddFILMember(MCUserModel model, string country)
        {
            var listId = string.Empty;
            if (model.IsCreator)
            {
                listId = _settings.GetConfigSetting<string>(SettingKeys.Integration.MailChimp.CreatorListId);
            }
            else
            {
                listId = _settings.GetConfigSetting<string>(SettingKeys.Integration.MailChimp.BuyerListId);
            }

            var currentIp = _clientIpProvider.Get();
            var mailChimpApi = GetMailChimp();

            var location = ListExtension.GetLocationByIp(currentIp);

            var member = new Member
            {
                EmailAddress = model.Email,
                Status = Status.Subscribed,
                StatusIfNew = Status.Subscribed,
                TimestampSignup = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"),
                IpSignup = currentIp,
                IpOpt = currentIp
            };
            var subscriberDetails = new Dictionary<string, object>
                {
                    {"FNAME", model.FirstName},
                    {"LNAME", model.LastName},
                    {"PHONE", "+"+model.PhoneCode.Split("~")[0]+""+model.PhoneNumber},
                    {"COUNTRY", country!= null?country:""},
                    {"COUNTRYIP", location.country_name!=null?location.country_name:""},
                    {"CITYIP", location.city!=null?location.city:""},
                    {"STATEIP", location.state_prov!=null?location.state_prov:""},
                    {"TZIP", location.time_zone!=null?location.time_zone.name:""},
                    {"SUPMETD", model.SignUpType}
                };
            member.MergeFields = subscriberDetails;
            await mailChimpApi.Members.AddOrUpdateAsync(listId, member);
        }

        public async Task AddFILMemberAdditionalDetails(MCUserAdditionalDetailModel model)
        {
            var listId = string.Empty;
            if (model.IsCreator)
            {
                listId = _settings.GetConfigSetting<string>(SettingKeys.Integration.MailChimp.CreatorListId);
            }
            else
            {
                listId = _settings.GetConfigSetting<string>(SettingKeys.Integration.MailChimp.BuyerListId);
            }
            var mailChimpApi = GetMailChimp();
            var member = await mailChimpApi.Members.GetAsync(listId, model.Email);
            var memberDetails = new Dictionary<string, object>
                {
                    {"FNAME", model.FirstName},
                    {"LNAME", model.LastName},
                    {"PHONE", "+"+model.PhoneCode.Split("~")[0]+""+model.PhoneNumber},
                    {"GENDER", model.Gender == "" ? "Male" : model.Gender },
                    {"DOB", model.DOB == ""? DateTime.UtcNow.ToString() : model.DOB },
                };
            member.MergeFields = memberDetails;
            await mailChimpApi.Members.AddOrUpdateAsync(listId, member);
        }

        public async Task AddFILMemberLastDetails(MCUserAdditionalDetailModel model)
        {
            var listId = string.Empty;
            if (model.IsCreator)
            {
                listId = _settings.GetConfigSetting<string>(SettingKeys.Integration.MailChimp.CreatorListId);
            }
            else
            {
                listId = _settings.GetConfigSetting<string>(SettingKeys.Integration.MailChimp.BuyerListId);
            }
            var mailChimpApi = GetMailChimp();
            var member = await mailChimpApi.Members.GetAsync(listId, model.Email);
            var lastItems = new Dictionary<string, object>
                {
                    {"LEVENTNAME", model.LastEventName},
                    {"LEVENTTC", model.LastEventTicketCategory},
                    {"LPURCH", model.LastPurchaseChannel},
                    {"LPURAMT", model.LastPurchaseAmount },
                    {"LPURDATE", model.LastPurchaseDate },
                    {"LEVENTCAT", model.LastEventCategory },
                };
            member.MergeFields = lastItems;
            await mailChimpApi.Members.AddOrUpdateAsync(listId, member);
        }
    }
}