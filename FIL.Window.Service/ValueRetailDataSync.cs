using FIL.Foundation.Senders;
using Scheduler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIL.Logging;
using System.Diagnostics;
using System.Net.Http;

namespace FIL.Window.Service
{
    public class ValueRetailDataSync : IValueRetailDataSync
    {


        public ValueRetailDataSync()
        {

        }

        public async Task Synchronize()
        {
            //MyScheduler.IntervalInDays(13, 30, 30, async () =>    //It will run at 5:30 AM every 15 days 
            //{
                List<string> VREndpoints = new List<string>
                {
                    "api/get-vr-villages",
                    "api/get-vr-express-route",
                    "api/get-vr-package-route",
                    "api/get-vr-express",
                    "api/get-vr-package",
                    "api/get-vr-chauffeur-service"
                };

                
                Console.WriteLine($"Value Retail Data Sync Started at {DateTime.Now}");

                //Value Retail Command Senders
                foreach (var endpoint in VREndpoints)
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.feelaplace.com/"), Timeout = new TimeSpan(0, 15, 0) })
                    {
                        var response = await client.GetAsync(endpoint);
                        if(response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"====== {await response.Content.ReadAsStringAsync()}");
                        }
                        else
                        {
                            throw new TaskCanceledException($"Value Retaial Data Sync Failed: {endpoint} at {DateTime.Now}");
                        }
                    }
                }
              
                Console.WriteLine($"Value Retail Data Sync finished at {DateTime.Now}. Total time taken is");
            //});
            return;
        }

        // VALUE RETAIL CONSOLE APP COMMAND SENDERS
        /*public async Task Synchronize()
        {
            MyScheduler.IntervalInDays(5, 30, 7, async () =>    //It will run at 5:30 AM everyday 
            {
                stopwatch.Reset();
                stopwatch.Start();
                Console.WriteLine($"Value Retail Data Sync Started at {DateTime.Now}");

                //Value Retail Command Handlers
                _logger.Log(Logging.Enums.LogCategory.Info, "Value Retail Villages Sync Started..");
                await _commandSender.Send(new VillageCommand { ModifiedBy = Guid.NewGuid() }, new TimeSpan(1, 0, 0));
                _logger.Log(Logging.Enums.LogCategory.Info, "Value Retail Village Sync Finished..");

                _logger.Log(Logging.Enums.LogCategory.Info, "Value Retail Route Sync Started");
                await _commandSender.Send(new ValueRetailRouteCommand { ModifiedBy = Guid.NewGuid() }, new TimeSpan(1, 0, 0));
                _logger.Log(Logging.Enums.LogCategory.Info, "Value Retail Route Finished..");

                _logger.Log(Logging.Enums.LogCategory.Info, "Shopping Package Route and Returns started...");
                await _commandSender.Send(new ValueRetailPackageRouteCommand { ModifiedBy = Guid.NewGuid() }, new TimeSpan(1, 0, 0));
                _logger.Log(Logging.Enums.LogCategory.Info, "Shopping Package Route and Returns finished.");

                _logger.Log(Logging.Enums.LogCategory.Info, "Shopping Express Sync Started..");
                await _commandSender.Send(new ShoppingExpressCommand { ModifiedBy = Guid.NewGuid() }, new TimeSpan(1, 0, 0));
                _logger.Log(Logging.Enums.LogCategory.Info, "Shopping Express Sync Finished..");

                _logger.Log(Logging.Enums.LogCategory.Info, "Shopping Package Sync Started..");
                await _commandSender.Send(new ShoppingPackageCommand { ModifiedBy = Guid.NewGuid() }, new TimeSpan(1, 0, 0));
                _logger.Log(Logging.Enums.LogCategory.Info, "Shopping Package Sync Finished..");

                _logger.Log(Logging.Enums.LogCategory.Info, "Chauffeur Drive Sync Started..");
                await _commandSender.Send(new ChauffeurDrivenCommand { ModifiedBy = Guid.NewGuid() }, new TimeSpan(1, 0, 0));
                _logger.Log(Logging.Enums.LogCategory.Info, "Chauffeur Drive Sync Finished..");

                stopwatch.Stop();
                Console.WriteLine($"Value Retail Data Sync finished at {DateTime.Now}. Total time taken is {stopwatch.Elapsed}");
            });
            return;
        }*/
    }
}