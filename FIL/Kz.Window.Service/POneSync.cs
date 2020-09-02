using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kz.Window.Service
{
    public class POneSync : IPOneSync
    {


        public POneSync()
        {
        }

        public async Task Synchronize()
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            Console.WriteLine("Starting P1 events sync at" + DateTime.Now + "...\n");

            try
            {
                List<int> offsets = new List<int>
                {
                    0,
                    500,
                    1000,
                    1500,
                    2000,
                    2500,
                };
                foreach (var item in offsets)
                {
                    var status = await POneApiAccessor(item);
                    if (status)
                    {
                        Console.WriteLine($"\t Fetching events from indices {item} to {item + 500} completed!");
                    }
                    else
                    {
                        Console.WriteLine($"\t Fetching events from indices {item} to {item + 500} failed!");
                    }
                }

                HttpResponseMessage message; 
                using (HttpClient client = new HttpClient())
                {
                    Console.WriteLine("\t Updating table rows...");
                    message = await client.GetAsync(new Uri("https://www.feelaplace.com/api/pOne/save"));
                    if(message.IsSuccessStatusCode)
                    {
                        Console.WriteLine("\t Finished updating table rows...");
                    } else
                    {
                        Console.WriteLine("\t Failed updating table rows...");
                    }
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Occured on " + DateTime.Now);
            }
            finally
            {
                Console.WriteLine("Finished P1 events sync at" + DateTime.Now + "...\n");
                stopwatch.Stop();
                Console.WriteLine($"Total time taken {stopwatch.ElapsedMilliseconds}");
            }
            return;
        }

        private async Task<bool> POneApiAccessor(int offset)
        {
            HttpResponseMessage httpResponseMessage;
            using (HttpClient client = new HttpClient())
            {
                var locationsuri = new Uri($"https://www.feelaplace.com/api/corporate/pOne/{offset}");
                httpResponseMessage = await client.GetAsync(locationsuri);
            }

            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}