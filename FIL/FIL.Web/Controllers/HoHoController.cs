using System;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using System.Threading.Tasks;
using FIL.Web.Feel.ViewModels.CitySightSeeing;
using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.Models.CitySightSeeing;
using FIL.Contracts.Queries.CitySightSeeingLocation;
using System.Drawing;
using System.Drawing.Drawing2D;
using FIL.Contracts.Enums;
using System.IO;
using FIL.Web.Core;
using FIL.Logging;
using FIL.Contracts.QueryResults.CitySightSeeingLocation;
using FIL.Logging.Enums;

namespace FIL.Web.Feel.Controllers
{
    public class HoHoController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly IAmazonS3FileProvider _amazonS3FileProvider;
        private readonly ILogger _logger;
        public HoHoController(ICommandSender commandSender, IQuerySender querySender, IAmazonS3FileProvider amazonS3FileProvider, ILogger logger)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _amazonS3FileProvider = amazonS3FileProvider;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/get-hoho-locations")]
        public async Task<LocationsResponseModel> GethohoLocations()
        {
            try
            {
                var d = Guid.NewGuid();
                GetAllLocationCommandResult locations = await _commandSender.Send<GetAllLocationCommand, GetAllLocationCommandResult>(new GetAllLocationCommand { ModifiedBy = new Guid("D21A85EE-351C-4349-9953-FE1492740976") }, new TimeSpan(2, 0, 0));
                return new LocationsResponseModel
                {
                    LocationResponse = AutoMapper.Mapper.Map<LocationResponse>(locations),
                };
            }

            catch (Exception ex)
            {
                return new LocationsResponseModel
                {
                    LocationResponse = null
                };
            }
        }
        [HttpGet]
        [Route("api/sync-data/{cityName}/{countryName}")]
        public async Task<LocationSyncDataResponseModel> SyncData(string cityName, string countryName)
        {
            try
            {

                FIL.Contracts.Models.CitySightSeeing.Location LocationData = new FIL.Contracts.Models.CitySightSeeing.Location();
                LocationData.country_name = countryName;
                LocationData.location_name = cityName;
                CitySightSeeingCommandResult updateResponse = await _commandSender.Send<CitySightSeeingCommand, CitySightSeeingCommandResult>(new CitySightSeeingCommand { Location = LocationData, ModifiedBy = new Guid("D21A85EE-351C-4349-9953-FE1492740976") }, new TimeSpan(2, 0, 0));

                return new LocationSyncDataResponseModel
                {
                    isSuccess = updateResponse.Success
                };
            }
            catch (Exception ex)
            {
                return new LocationSyncDataResponseModel
                {
                    isSuccess = false
                };
            }
        }
        [HttpGet]
        [Route("api/disable-events")]
        public async Task<LocationSyncDataResponseModel> DisabledEvents()
        {
            try
            {
                await _commandSender.Send(new DisabledCitySightSeeingEventCommand { ModifiedBy = new Guid("6260467A-D5E0-4C80-8DD4-189129054AB2") });
                return new LocationSyncDataResponseModel
                {
                    isSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new LocationSyncDataResponseModel
                {
                    isSuccess = false
                };
            }
        }
        [HttpGet]
        [Route("api/availability/{date}/{ticketId}")]
        public async Task<AvailabilityResponseModel> GetAvailability(string date, string ticketId)
        {
            try
            {
                GetAvailabilityCommandResult availabilities = await _commandSender.Send<GetAvailabilityCommand, GetAvailabilityCommandResult>(new GetAvailabilityCommand { Date = date, TicketId = ticketId, ModifiedBy = new Guid("D21A85EE-351C-4349-9953-FE1492740976") }, new TimeSpan(2, 0, 0));
                return new AvailabilityResponseModel
                {
                    TimeSlots = availabilities.AvailableSlots
                };
            }

            catch (Exception ex)
            {
                return new AvailabilityResponseModel
                {
                    TimeSlots = null
                };
            }
        }
        [HttpGet]
        [Route("api/upload/hotticket/{skipIndex}/{takeIndex}")]
        public async Task<LocationSyncDataResponseModel> UploadImages(int skipIndex, int takeIndex)
        {
            try
            {
                var queryResult = await _querySender.Send(new GetExOzImagesQuery { SkipIndex = skipIndex, TakeIndex = takeIndex });
                System.Net.WebClient wc = new System.Net.WebClient();
                ImageType imageType = ImageType.FeelHotTicketSlider;
                foreach (var currentItem in queryResult.ExOzImageUploadModels)
                {
                    int count = 1;
                    foreach (var item in currentItem.ImageLinks)
                    {
                        if (count <= 3)
                        {
                            try
                            {
                                string imageAltId = "";
                                byte[] bytes = wc.DownloadData(item);
                                MemoryStream ms = new MemoryStream(bytes);
                                System.Drawing.Image original = System.Drawing.Image.FromStream(ms);
                                System.Drawing.Image resizedHotTicketImage = ResizeImage(original, new Size(600, 381));
                                imageAltId = currentItem.EventAltIds.ToString() + "-ht-c" + count.ToString() + "";
                                _amazonS3FileProvider.UploadFeelImage(resizedHotTicketImage, imageAltId, imageType);
                                count++;
                            }
                            catch (Exception e)
                            {
                                _logger.Log(LogCategory.Error, new Exception("Failed to Upload Tiqet Images", e));
                                continue;
                            }
                        }
                    }
                }
                return new LocationSyncDataResponseModel
                {
                    isSuccess = true
                };
            }
            catch (Exception e)
            {
                return new LocationSyncDataResponseModel { isSuccess = false };
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
