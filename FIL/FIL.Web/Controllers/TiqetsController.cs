using FIL.Configuration;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.Tiqets;
using FIL.Foundation.Senders;
using FIL.Web.Core;
using FIL.Web.Feel.ViewModels.Tiqets;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using FIL.Logging;
using FIL.Logging.Enums;
using FIL.Contracts.Queries;

namespace FIL.Web.Feel.Controllers
{
    public class TiqetsController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly IAmazonS3FileProvider _amazonS3FileProvider;
        private readonly ILogger _logger;

        public TiqetsController(ICommandSender commandSender, IQuerySender querySender, IAmazonS3FileProvider amazonS3FileProvider, ILogger logger)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _amazonS3FileProvider = amazonS3FileProvider;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/sync-tiqets/products/{skipIndex}/{takeIndex}/{getProducts}/{pageNumber}")]
        public async Task<SyncProductResponseModel> SyncProducts(int skipIndex, int takeIndex, bool getProducts, int pageNumber)
        {
            try
            {
                SyncTiqetProductsCommandResult products = await _commandSender.Send<SyncTiqetProductsCommand, SyncTiqetProductsCommandResult>(new SyncTiqetProductsCommand { SkipIndex = skipIndex, TakeIndex = takeIndex, PageNumber = pageNumber, GetProducts = getProducts, ModifiedBy = new Guid("D21A85EE-351C-4349-9953-FE1492740976") }, new TimeSpan(2, 0, 0));
                return new SyncProductResponseModel
                {
                    success = true,
                    productResponse = AutoMapper.Mapper.Map<SyncProductResponse>(products),
                    remainingProducts = products.RemainingProducts
                };
            }
            catch (Exception ex)
            {
                return new SyncProductResponseModel
                {
                    success = false,
                    productResponse = null
                };
            }
        }

        [HttpGet]
        [Route("api/update-product-details/{productId}/{uploadImage}")]
        public async Task<UpdateDetailResponseModel> SaveAndUpdateDetails(string productId, bool uploadImage = false)
        {
            try
            {
                UpdateProductCommandResult updateProducts = await _commandSender.Send<UpdateProductCommand, UpdateProductCommandResult>(new UpdateProductCommand { productId = productId, IsImageUpload = uploadImage, ModifiedBy = new Guid("D21A85EE-351C-4349-9953-FE1492740976") }, new TimeSpan(2, 0, 0));
                if ((updateProducts.success && updateProducts.IsImageUpload) || uploadImage)
                {
                    await UploadImages(updateProducts.ImageLinks, updateProducts.EventAltId);
                }
                return new UpdateDetailResponseModel
                {
                    success = updateProducts.success
                };
            }
            catch (Exception ex)
            {
                return new UpdateDetailResponseModel
                {
                    success = false
                };
            }
        }

        [HttpGet]
        [Route("api/get-timeslots/{productId}/{day}")]
        public async Task<ViewModels.Tiqets.TimeSlotResponseModel> GetAvailabeTimeSlots(string productId, string day)
        {
            try
            {
                GetTimeSlotsCommandResult timeSlots = await _commandSender.Send<GetTimeSlotsCommand, GetTimeSlotsCommandResult>(new GetTimeSlotsCommand { ProductId = productId, Day = day });

                return new ViewModels.Tiqets.TimeSlotResponseModel
                {
                    TimeSlots = timeSlots.TimeSlots
                };
            }
            catch (Exception ex)
            {
                return new ViewModels.Tiqets.TimeSlotResponseModel
                {
                    TimeSlots = null
                };
            }
        }

        [HttpGet]
        [Route("api/tiqet/disable-events")]
        public async Task<UpdateDetailResponseModel> DisabledEvents()
        {
            try
            {
                await _commandSender.Send(new DisableTiqetEventCommand { ModifiedBy = new Guid("D21A85EE-351C-4349-9953-FE1492740976") });
                return new UpdateDetailResponseModel
                {
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new UpdateDetailResponseModel
                {
                    success = false
                };
            }
        }
        [HttpGet]
        [Route("api/upload-image/{skipIndex}/{takeIndex}")]
        public async Task<UpdateDetailResponseModel> UploadTiqetImages(int skipIndex, int takeIndex)
        {

            try
            {
                var queryResult = await _querySender.Send(new TiqetProductUploadQuery { SkipIndex = skipIndex, TakeIndex = takeIndex });
                //foreach (var item in queryResult.TiqetImagesList)
                //{
                //    try
                //    {
                //        string altId = item.EventAltId.ToString().ToUpper();
                //        System.Net.WebClient wc = new System.Net.WebClient();
                //        var trimImageLink = item.ImageLink.Split("?");
                //        ImageType imageType = ImageType.FeelHotTicketSlider;
                //        string imageAltId = "";
                //        byte[] bytes = wc.DownloadData(trimImageLink[0]);
                //        MemoryStream ms = new MemoryStream(bytes);
                //        System.Drawing.Image original = System.Drawing.Image.FromStream(ms);
                //        System.Drawing.Image resizedPlaceImage = ResizeImage(original, new Size(1920, 570));
                //        imageAltId = altId.ToString() + "-about";
                //        imageType = ImageType.FeelDescriptionPage;
                //        _amazonS3FileProvider.UploadFeelImage(resizedPlaceImage, imageAltId, imageType);

                //        System.Drawing.Image resizedInventoryImage = ResizeImage(original, new Size(1920, 350));
                //        imageAltId = altId.ToString() + "";
                //        imageType = ImageType.FeelInventoryPage;
                //        _amazonS3FileProvider.UploadFeelImage(resizedInventoryImage, imageAltId, imageType);
                //    }
                //    catch (Exception e)
                //    {
                //        continue;
                //    }
                //}


                return new UpdateDetailResponseModel
                {
                    success = true,
                    TiqetImagesList = queryResult.TiqetImagesList
                };

            }
            catch (Exception ex)
            {
                return new UpdateDetailResponseModel
                {
                    success = false
                };
            }
        }
        // for uploading images
        public async Task UploadImages(List<string> imageLinks, Guid guid)
        {
            string altId = guid.ToString().ToUpper();
            System.Net.WebClient wc = new System.Net.WebClient();
            int count = 1;
            foreach (var currentImage in imageLinks.Take(1))
            {
                try
                {
                    if (count <= 3)
                    {
                        var trimImageLink = currentImage.Split("?");
                        ImageType imageType = ImageType.FeelHotTicketSlider;
                        string imageAltId = "";
                        byte[] bytes = wc.DownloadData(trimImageLink[0]);
                        MemoryStream ms = new MemoryStream(bytes);
                        System.Drawing.Image original = System.Drawing.Image.FromStream(ms);

                        // Place page & Ticket Catgeory Image
                        if (count == 1)
                        {
                            System.Drawing.Image resizedPlaceImage = ResizeImage(original, new Size(1920, 570));
                            imageAltId = altId.ToString() + "-about";
                            imageType = ImageType.FeelDescriptionPage;
                            _amazonS3FileProvider.UploadFeelImage(resizedPlaceImage, imageAltId, imageType);

                            System.Drawing.Image resizedInventoryImage = ResizeImage(original, new Size(1920, 350));
                            imageAltId = altId.ToString() + "";
                            imageType = ImageType.FeelInventoryPage;
                            _amazonS3FileProvider.UploadFeelImage(resizedInventoryImage, imageAltId, imageType);
                        }

                        // Hot Ticket Upload
                        if (count <= 3)
                        {
                            System.Drawing.Image resizedHotTicketImage = ResizeImage(original, new Size(600, 381));
                            imageAltId = altId.ToString() + "-ht-c" + count.ToString() + "";
                            _amazonS3FileProvider.UploadFeelImage(resizedHotTicketImage, imageAltId, imageType);
                            // Gallery Images
                            System.Drawing.Image resizedGalleryImage = ResizeImage(original, new Size(1920, original.Width));
                            imageAltId = altId.ToString() + "-glr-" + count.ToString() + "";
                            imageType = ImageType.FeelGallery;
                            _amazonS3FileProvider.UploadFeelImage(resizedGalleryImage, imageAltId, imageType);
                            count++;
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Log(LogCategory.Error, new Exception("Failed to Upload Tiqet Images", e));
                    continue;
                }
            }

        }

        // Resize Image 
        public static System.Drawing.Image ResizeImage(System.Drawing.Image image, Size size)
        {
            int newWidth;
            int newHeight;
            newWidth = size.Width;
            newHeight = size.Height;
            System.Drawing.Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
    }
}