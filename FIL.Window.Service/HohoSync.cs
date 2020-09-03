using Algolia.Search.Clients;
using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.Commands.HubSpot;
using FIL.Contracts.Models;
using FIL.Contracts.Models.Algolia;
using FIL.Foundation.Senders;
using FIL.Messaging.Models.Emails;
using FIL.Messaging.Senders;
using FIL.Window.Service.Models;
using FIL.Window.Service.Services;
using Scheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Window.Service
{
    public class HohoSync : IHohoSync
    {


        public HohoSync()
        {

        }

        public async Task Synchronize()
        {
            try
            {
                Task.WaitAll(IntegrateWithSlackAsync("start", true));
                Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
                Console.WriteLine("Location Sync Started at " + DateTime.Now);
                HttpClient webClient = new HttpClient();
                Uri locationsuri = new Uri("https://www.feelitlive.com/api/get-hoho-locations");
                HttpResponseMessage response = await webClient.GetAsync(locationsuri);
                var jsonStringLocations = await response.Content.ReadAsStringAsync();
                var locationsData = FIL.Api.Utilities.Mapper<LocationsResponseModel>.MapFromJson(jsonStringLocations);
                if (locationsData.LocationResponse.Locations != null && locationsData.LocationResponse.Locations.Count > 0)
                {
                    Console.WriteLine("Locations Synced Successfully " + locationsData.LocationResponse.Locations.Count + "Locations Found");
                    int i = 0;
                    foreach (var location in locationsData.LocationResponse.Locations)
                    {
                        string uriFormat = "https://www.feelitlive.com/api/sync-data/{0}/{1}";
                        string uri = string.Format(uriFormat, location.Name, location.CountryName);
                        HttpResponseMessage location_response = await webClient.GetAsync(uri);
                        var jsonStringOfLocation = await location_response.Content.ReadAsStringAsync();
                        var locationData = FIL.Api.Utilities.Mapper<SucessResponseModel>.MapFromJson(jsonStringOfLocation);
                        if (locationData.isSuccess)
                        {
                            Console.WriteLine("Done ..." + location.Name + " " + location.CountryName);
                        }
                        else
                        {
                            Console.WriteLine("Error ..." + location.Name + " " + location.CountryName);
                        }
                    }
                    Console.WriteLine("Events Disabling started");
                    Uri disablinguri = new Uri("https://www.feelitlive.com/api/disable-events");
                    HttpResponseMessage responseOfDisabled = await webClient.GetAsync(disablinguri);
                    var jsonStringOfDisabled = await responseOfDisabled.Content.ReadAsStringAsync();
                    var locationssData = FIL.Api.Utilities.Mapper<SucessResponseModel>.MapFromJson(jsonStringOfDisabled);
                    if (locationssData.isSuccess)
                    {
                        Console.WriteLine("Events Disabling Completed");
                    }
                    else
                    {
                        Console.WriteLine("Error in Events Disabling");
                    }

                    stopwatch.Stop();
                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                    Task.WaitAll(IntegrateWithSlackAsync("end", locationssData.isSuccess));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Occured on " + DateTime.Now);
            }
            return;
        }
        public static async Task IntegrateWithSlackAsync(string message, bool success)
        {
            string message_to_send = "";
            if (message == "end")
            {
                if (success)
                {
                    message_to_send = "Hoho Sync Completed Successfully at " + DateTime.Now;
                }
                else
                {
                    message_to_send = "Hoho Sync Failed at " + DateTime.Now;
                }
            }
            else
            {
                message_to_send = "Hoho Sync Started at " + DateTime.Now;
            }
            var webhookUrl = new Uri("https://hooks.slack.com/services/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");
            var slackClient = new SlackClient(webhookUrl);
            var response = await slackClient.SendMessageAsync(message_to_send);
            var isValid = response.IsSuccessStatusCode ? "valid" : "invalid";
        }
    }
}