using FIL.Api.Repositories;
using FIL.Api.Repositories.Tiqets;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.DataModels.Tiqets;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Tiqets;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static FIL.Contracts.Utils.Constant;

namespace FIL.Api.CommandHandlers.Tiqets
{
    public class SyncTiqetProductsCommandHandler : BaseCommandHandlerWithResult<SyncTiqetProductsCommand, SyncTiqetProductsCommandResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ITiqetProductRepository _tiqetProductRepository;
        private readonly ITiqetProductImageRepository _tiqetProductImageRepository;
        private readonly ITiqetProductTagMappingRepository _tiqetProductTagMappingRepository;
        private readonly ITiqetTagTypeRepository _tiqetTagTypeRepository;
        private readonly ITiqetTagRepository _tiqetTagRepository;
        private readonly ITiqetEventDetailMappingRepository _tiqetEventDetailMappingRepository;
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public SyncTiqetProductsCommandHandler(IEventRepository eventRepository, ISettings settings, ILogger logger, ITiqetProductRepository tiqetProductRepository, ITiqetProductImageRepository tiqetProductImageRepository, ITiqetProductTagMappingRepository tiqetProductTagMappingRepository, ITiqetTagTypeRepository tiqetTagTypeRepository, ITiqetTagRepository tiqetTagRepository, ITiqetEventDetailMappingRepository tiqetEventDetailMappingRepository,
        IMediator mediator) : base(mediator)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _settings = settings;
            _tiqetProductRepository = tiqetProductRepository;
            _tiqetProductImageRepository = tiqetProductImageRepository;
            _tiqetProductTagMappingRepository = tiqetProductTagMappingRepository;
            _tiqetTagTypeRepository = tiqetTagTypeRepository;
            _tiqetTagRepository = tiqetTagRepository;
            _tiqetEventDetailMappingRepository = tiqetEventDetailMappingRepository;
        }

        protected override async Task<ICommandResult> Handle(SyncTiqetProductsCommand command)
        {
            SyncTiqetProductsCommandResult result = new SyncTiqetProductsCommandResult();
            try
            {
                TagTypeResponseModel tagTypes = new TagTypeResponseModel();
                tagTypes = Mapper<TagTypeResponseModel>.MapFromJson((await SyncTagTypes()));
                //here skipIndex check is to avoid hitting the DB
                if (tagTypes.success && tagTypes.tag_types.Count() > 0 && command.SkipIndex == 0)
                {
                    var tiqetTagTypes = _tiqetTagTypeRepository.GetByNames(tagTypes.tag_types.Select(s => s.name)).ToList();
                    var tiqetTagTypesList = AutoMapper.Mapper.Map<List<TiqetTagType>>(tiqetTagTypes);
                    foreach (TagType tagType in tagTypes.tag_types)
                    {
                        var currentTagType = tiqetTagTypesList.Where(s => s.Name == tagType.name).FirstOrDefault();
                        if (currentTagType == null)
                        {
                            currentTagType = _tiqetTagTypeRepository.Save(new TiqetTagType
                            {
                                Name = tagType.name,
                                TypeId = tagType.id,
                                IsEnabled = true,
                                CreatedUtc = DateTime.UtcNow,
                                CreatedBy = command.ModifiedBy
                            });
                        }
                    }
                    //Intially Disable all event Detail Mappings
                    _tiqetEventDetailMappingRepository.DisableAll();
                }

                TagResponseModel tags = new TagResponseModel();
                tags = Mapper<TagResponseModel>.MapFromJson((await SyncTags()));
                if (tags.success && tags.tags.Count() > 0 && command.SkipIndex == 0)
                {
                    var tiqetTags = _tiqetTagRepository.GetByTagIds(tags.tags.Select(s => s.id));
                    var tiqetTagsList = AutoMapper.Mapper.Map<List<TiqetTag>>(tiqetTags);

                    foreach (Tag tag in tags.tags)
                    {
                        var currentTag = tiqetTagsList.Where(s => s.TagId == tag.id).FirstOrDefault();
                        if (currentTag == null)
                        {
                            currentTag = _tiqetTagRepository.Save(new TiqetTag
                            {
                                Name = tag.name,
                                TagId = tag.id,
                                TagTypeId = tag.type_id,
                                IsEnabled = true,
                                CreatedUtc = DateTime.UtcNow,
                                CreatedBy = command.ModifiedBy
                            });
                        }
                    }
                }

                AllProductResponseModel allProductsList = new AllProductResponseModel();
                allProductsList = Mapper<AllProductResponseModel>.MapFromJson((await GetProductList(command.PageNumber)));
                if (allProductsList.success && allProductsList.products.Count > 0)
                {
                    foreach (Product productData in allProductsList.products.Skip(command.SkipIndex).Take(command.TakeIndex))    // this is done to sync the products in batch wise
                    {
                        var tiqetsProducts = _tiqetProductRepository.GetByProductId(productData.id);
                        if (tiqetsProducts == null)
                        {
                            tiqetsProducts = _tiqetProductRepository.Save(new Contracts.DataModels.Tiqets.TiqetProduct
                            {
                                ProductId = productData.id,
                                Tittle = productData.title,
                                SaleStatus = productData.sale_status,
                                Inclusions = productData.whats_included,
                                Language = productData.language,
                                CountryName = productData.country_name,
                                CityName = productData.city_name,
                                ProductSlug = productData.product_slug,
                                Price = Convert.ToDecimal(productData.price),
                                SaleStatuReason = productData.sale_status_reason,
                                VenueName = productData.venue.name,
                                VenueAddress = productData.venue.address,
                                Summary = productData.summary,
                                TagLine = productData.tagline,
                                PromoLabel = productData.promo_label,
                                RatingAverage = productData.ratings.average.ToString(),
                                GeoLocationLatitude = productData.geolocation.lat.ToString(),
                                GeoLocationLongitude = productData.geolocation.lng.ToString(),
                                IsEnabled = true,
                                CreatedUtc = DateTime.UtcNow,
                                CreatedBy = command.ModifiedBy
                            });
                        }

                        var tiqetsImages = _tiqetProductImageRepository.GetByProductId(tiqetsProducts.ProductId);
                        if (tiqetsImages.Count() == 0 && productData.images != null && productData.images.Count() > 0)
                        {
                            foreach (Image images in productData.images)
                            {
                                var image = _tiqetProductImageRepository.Save(new TiqetProductImage
                                {
                                    ProductId = tiqetsProducts.ProductId,
                                    Small = images.small,
                                    Medium = images.medium,
                                    Large = images.large,
                                    IsEnabled = true,
                                    CreatedUtc = DateTime.UtcNow,
                                    CreatedBy = command.ModifiedBy
                                });
                            }
                        }

                        var tiqetProductMappings = _tiqetProductTagMappingRepository.GetByProductId(tiqetsProducts.ProductId);
                        if (tiqetProductMappings.Count() == 0)
                        {
                            var tiqetProductTags = _tiqetTagRepository.GetByTagIds(productData.tag_ids);
                            var tiqetProductTagsList = AutoMapper.Mapper.Map<List<TiqetTag>>(tiqetProductTags);
                            foreach (var tiqetIds in productData.tag_ids)
                            {
                                bool isCategory = false;
                                var isCategoryType = tiqetProductTagsList.Where(s => s.TagId == tiqetIds).FirstOrDefault();
                                if (isCategoryType != null && isCategoryType.TagTypeId == TiqetsConstant.CategoryId)
                                {
                                    isCategory = true;
                                }
                                var tiqetProductTagMapping = _tiqetProductTagMappingRepository.Save(new TiqetProductTagMapping
                                {
                                    ProductId = tiqetsProducts.ProductId,
                                    TagId = tiqetIds,
                                    IsEnabled = true,
                                    IsCategoryType = isCategory,
                                    CreatedUtc = DateTime.UtcNow,
                                    CreatedBy = command.ModifiedBy
                                });
                            }
                        }
                    }
                }

                result.RemainingProducts = allProductsList.pagination.total - (command.SkipIndex + command.TakeIndex);

                if (command.GetProducts)
                {
                    var getEnabledProducts = _tiqetProductRepository.GetAll().Where(s => s.IsEnabled);
                    result.tiqetProducts = AutoMapper.Mapper.Map<List<Contracts.Models.Tiqets.TiqetProduct>>(getEnabledProducts);
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to Sync Products", e));
                return null;
            }
        }

        public async Task<string> SyncTagTypes()
        {
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    using (var response = await httpClient.GetAsync("tag_types?lang=en"))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
                return responseData;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get Tag Types", e));
                return null;
            }
        }

        public async Task<string> GetProductList(int pageNumber)
        {
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData, response_Data = null;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    using (var response = await httpClient.GetAsync("products?lang=en"))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                    var data = Mapper<AllProductResponseModel>.MapFromJson(responseData);
                    if (data.success && data.products.Count > 0)
                    {
                        using (var response = await httpClient.GetAsync("products?lang=en&page_size=500&page=" + pageNumber))
                        {
                            response_Data = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                return response_Data;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get product list", e));
                return null;
            }
        }

        public async Task<string> SyncTags()
        {
            try
            {
                TagResponseModel tags = new TagResponseModel();
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData, response_data = null;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    using (var response = await httpClient.GetAsync("tags?lang=en"))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                    tags = Mapper<TagResponseModel>.MapFromJson(responseData);
                    if (tags.success && tags.tags.Count > 0)
                    {
                        using (var response = await httpClient.GetAsync("tags?lang=en&page_size=" + tags.pagination.total))
                        {
                            response_data = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                return response_data;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get Tags", e));
                return null;
            }
        }
    }
}