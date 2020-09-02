using Algolia.Search.Clients;
using Kz.Contracts.Commands.CitySightSeeing;
using Kz.Contracts.Commands.HubSpot;
using Kz.Contracts.Models;
using Kz.Contracts.Models.Algolia;
using Kz.Foundation.Senders;
using Kz.Messaging.Models.Emails;
using Kz.Messaging.Senders;
using Kz.Window.Service.Models;
using Kz.Window.Service.Services;
using Scheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kz.Window.Service
{
    public class AlgoliaDataSync : IAlgoliaDataSync
    {


        public AlgoliaDataSync()
        {

        }

        public async Task Synchronize()
        {
            try
            {
                Task.WaitAll(IntegrateWithSlackAsync("start", true));
                Console.WriteLine("Algolia Data Sync Started at " + DateTime.Now);
                Task.WaitAll(SynchronizeData());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Occured on " + DateTime.Now);
            }
            return;
        }


        public static async Task SynchronizeData()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            bool isSuccess = false; bool isCities = false;
            HttpClient webClient = new HttpClient();
            webClient.Timeout = new TimeSpan(0, 15, 0);
            //Initially disbabling Algolia index Local
            Uri uriDisable = new Uri("https://www.feelitlive.com/api/algolia/disable-index");
            HttpResponseMessage responseDisable = await webClient.GetAsync(uriDisable);
            var jsonStringResponseDisable = await responseDisable.Content.ReadAsStringAsync();
            var responseDisableData = Kz.Api.Utilities.Mapper<PlacesSyncReponseModel>.MapFromJson(jsonStringResponseDisable);
            if (responseDisableData.IsSuccess)
            {
                int skipIndex = 0;
                for (int i = 0; i < 20; i++)
                {
                    Uri uri = new Uri("https://www.feelitlive.com/api/algolia/sync-places/" + skipIndex + "/500/false");
                    HttpResponseMessage response = await webClient.GetAsync(uri);
                    var jsonStringResponses = await response.Content.ReadAsStringAsync();
                    var responseData = Kz.Api.Utilities.Mapper<PlacesSyncReponseModel>.MapFromJson(jsonStringResponses);
                    skipIndex = i * 500;

                }

                // update Algolia Index Directly
                Uri uriUpdate = new Uri("https://www.feelitlive.com/api/algolia/delete-from-index");
                HttpResponseMessage responseUpdate = await webClient.GetAsync(uriDisable);
                var jsonStringResponseUpdate = await responseDisable.Content.ReadAsStringAsync();
                var responseUpdateData = Kz.Api.Utilities.Mapper<PlacesSyncReponseModel>.MapFromJson(jsonStringResponseUpdate);


                if (responseUpdateData.IsSuccess)
                {
                    isSuccess = true;
                }

            }

            stopwatch.Stop();
            Task.WaitAll(IntegrateWithSlackAsync("end", isSuccess));
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

        }
        public static async Task IntegrateWithSlackAsync(string message, bool success)
        {
            string message_to_send = "";
            if (message == "end")
            {
                if (success)
                {
                    message_to_send = "Algolia Data Sync Completed Successfully at " + DateTime.Now;
                }
                else
                {
                    message_to_send = "Algolia Data Sync Failed at " + DateTime.Now;
                }
            }
            else
            {
                message_to_send = "Algolia Sync Started at " + DateTime.Now;
            }
            var webhookUrl = new Uri("https://hooks.slack.com/services/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");
            var slackClient = new SlackClient(webhookUrl);
            var response = await slackClient.SendMessageAsync(message_to_send);
            var isValid = response.IsSuccessStatusCode ? "valid" : "invalid";
        }
    }
}