using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.Models.Integrations.ExOz;
using FIL.Foundation.Senders;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzSessionResponse;

namespace FIL.ExOzConsoleApp
{
    public class SyncExOzProducts : ISynchronizer<SessionList, ProductList>
    {
        private readonly ICommandSender _commandSender;
        private readonly IConsoleLogger _logger;
        private readonly ISynchronizer<ProductOptionList, SessionList> _productSessionSynchronizer;

        public SyncExOzProducts(ICommandSender commandSender, IConsoleLogger logger,
            ISynchronizer<ProductOptionList, SessionList> productSessionSynchronizer)
        {
            _commandSender = commandSender;
            _logger = logger;
            _productSessionSynchronizer = productSessionSynchronizer;
        }

        public async Task<SessionList> Synchronize(ProductList productList)
        {
            string line = "";
            
            
            int i = 0;
            foreach (var prod in productList.Products)
            {

                ProductList productDetails = new ProductList()
                {
                    Products = new List<ExOzProductResponse>()
                };

                string input = prod.CanonicalRegionUrlSegment + "/" + prod.OperatorUrlSegment + "/" + prod.UrlSegment;
                ExOzProductResponse objProduct = new ExOzProductResponse();

                string postResponse = HttpWebRequestHelper.ExOz_WebRequestGet(input);
                if (postResponse != "0")
                {
                    objProduct = Mapper<ExOzProductResponse>.MapFromJson(postResponse);
                }

                objProduct.Name = CommonFunctions.replaceSingleQuotes(objProduct.Name);
                objProduct.Summary = CommonFunctions.replaceSingleQuotes(objProduct.Summary);
                objProduct.OperatorPublicName = CommonFunctions.replaceSingleQuotes(objProduct.OperatorPublicName);
                objProduct.Title = CommonFunctions.replaceSingleQuotes(objProduct.Title);
                objProduct.Description = CommonFunctions.replaceSingleQuotes(objProduct.Description);
                objProduct.MoreInfo = CommonFunctions.replaceSingleQuotes(objProduct.MoreInfo);
                objProduct.Tips = CommonFunctions.replaceSingleQuotes(objProduct.Tips);

                productDetails.Products.Add(objProduct);
                SaveExOzProductCommandResult retProducts = new SaveExOzProductCommandResult()
                {
                    ProductList = new List<Contracts.DataModels.ExOzProduct>()
                };

                retProducts = await _commandSender.Send<SaveExOzProductCommand, SaveExOzProductCommandResult>(new SaveExOzProductCommand
                {
                    ProductList = productDetails.Products,
                    ModifiedBy = new Guid("C043DDEE-D0B1-48D8-9C3F-309A77F44793")
                });

                if (objProduct.ProductSessions != null)
                {
                    SessionList sessionList = new SessionList()
                    {
                        ProductSessions = new List<ExOzSessionResponse>()
                    };

                    foreach (var sess in objProduct.ProductSessions)
                    {
                        sess.ProductId = objProduct.Id;
                        sessionList.ProductSessions.Add(sess);
                    }

                    _logger.StartMsg("Sessions");
                    ProductOptionList productOptionResponse = await _productSessionSynchronizer.Synchronize(sessionList);
                }
                
                i++;
                line = _logger.Update(i, productList.Products.Count, line);
            }

            //Insert Products Here
            //try
            //{

            //    _logger.FinishMsg(retProducts.ProductList.Count, "Products");
            //    return sessionList;
            //}
            //catch (Exception e)
            //{
            //    _logger.Log($"Exception: {e.Message}");
            //    throw;
            //}
            return null;
        }
    }
}
