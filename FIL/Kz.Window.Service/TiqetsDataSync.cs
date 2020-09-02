using Kz.Window.Service.Models;
using Kz.Window.Service.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kz.Window.Service
{
    public class TiqetsDataSync : ITiqetsDataSync
    {
        public TiqetsDataSync()
        {

        }

        public async Task Synchronize()
        {
            try
            {
                Task.WaitAll(IntegrateWithSlackAsync("start", true));
                HttpClient webClient = new HttpClient();
                webClient.Timeout = new TimeSpan(1, 0, 0);
                int index = 0, pageNumber = 1, skipIndex = 0;
                for (int i = 1; i <= 40; i++)
                {
                    index = index + 1;
                    Uri products_Uri = new Uri("https://www.feelitlive.com/api/sync-tiqets/products/" + skipIndex + "/100/false/" + pageNumber);
                    HttpResponseMessage response_data = await webClient.GetAsync(products_Uri);
                    var product_JsonString = await response_data.Content.ReadAsStringAsync();
                    var products_Data = Kz.Api.Utilities.Mapper<ProductResponseModel>.MapFromJson(product_JsonString);
                    skipIndex = skipIndex + 100;
                    if (i % 5 == 0)
                    {
                        index = 0; skipIndex = 0;
                        pageNumber = pageNumber + 1;
                    }

                }
                Uri productsUri = new Uri("https://www.feelitlive.com/api/sync-tiqets/products/0/10/true/1");
                HttpResponseMessage response = await webClient.GetAsync(productsUri);
                var productJsonString = await response.Content.ReadAsStringAsync();
                var productsData = Kz.Api.Utilities.Mapper<ProductResponseModel>.MapFromJson(productJsonString);
                int indx = 1; bool successFlag = true;
                foreach (var currentProduct in productsData.productResponse.tiqetProducts)
                {
                    try
                    {
                        string uriFormat = "https://www.feelitlive.com/api/update-product-details/{0}/false";
                        string uri = string.Format(uriFormat, currentProduct.productId);
                        HttpResponseMessage update_product_response = await webClient.GetAsync(uri);
                        var jsonStringOfProduct = await update_product_response.Content.ReadAsStringAsync();
                        var productData = Kz.Api.Utilities.Mapper<ProductUpdateResponseModel>.MapFromJson(jsonStringOfProduct);
                        if (productData.success)
                        {
                            //Console.WriteLine(indx + "Done ..." + currentProduct.productId);
                        }
                        else
                        {
                            Console.WriteLine("Error ..." + currentProduct.productId);
                        }
                        Console.WriteLine(productsData.productResponse.tiqetProducts.Count() - indx + " Remain " + " Index at" + indx);
                        indx++;
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
                Uri disable_productsUri = new Uri("https://www.feelitlive.com/api/tiqet/disable-events");
                HttpResponseMessage disable_response = await webClient.GetAsync(productsUri);
                var disable_responseString = await disable_response.Content.ReadAsStringAsync();
                var disableData = Kz.Api.Utilities.Mapper<ProductResponseModel>.MapFromJson(disable_responseString);
                Console.Write("Completed -------------------");
                Task.WaitAll(IntegrateWithSlackAsync("end", successFlag));
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
                    message_to_send = "Tiqets Sync Completed Successfully at " + DateTime.Now;
                }
                else
                {
                    message_to_send = "Tiqet Sync Failed at " + DateTime.Now;
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