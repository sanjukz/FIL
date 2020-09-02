using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations.ExOz;
using FIL.Foundation.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzProductResponse;

namespace FIL.ExOzConsoleApp
{
    public class SyncExOzOperators : ISynchronizer<ProductList, OperatorList>
    {
        private readonly ICommandSender _commandSender;
        private readonly IConsoleLogger _logger;
        private readonly ISynchronizer<SessionList, ProductList> _productSynchronizer;

        public SyncExOzOperators(ICommandSender commandSender, IConsoleLogger logger,
            ISynchronizer<SessionList, ProductList> productSynchronizer)
        {
            _commandSender = commandSender;
            _logger = logger;
            _productSynchronizer = productSynchronizer;
        }

        public async Task<ProductList> Synchronize(OperatorList operatorList)
        {
            string line = "";
            _logger.StartMsg("Operators");
                        
            int i = 0;

            foreach (var op in operatorList.operators)
            {
                try
                {
                    string input = op.CanonicalRegionUrlSegment + "/" + op.UrlSegment;
                    ExOzOperatorResponse opDetails = new ExOzOperatorResponse();

                    string postResponse = HttpWebRequestHelper.ExOz_WebRequestGet(input);
                    if (postResponse != "0")
                    {
                        opDetails = Mapper<ExOzOperatorResponse>.MapFromJson(postResponse);
                    }
                    if (opDetails.Id != -1 && opDetails.Geolocations != null && opDetails.Geolocations[0].Address != null)
                    {
                        opDetails.RegionId = op.RegionId;
                        opDetails.RegionUrlSegment = op.RegionUrlSegment;
                        CommonFunctions.replaceSingleQuotes(opDetails.Name);
                        CommonFunctions.replaceSingleQuotes(opDetails.PublicName);
                        CommonFunctions.replaceSingleQuotes(opDetails.Summary);
                        CommonFunctions.replaceSingleQuotes(opDetails.Tips);
                        CommonFunctions.replaceSingleQuotes(opDetails.Description);
                        CommonFunctions.replaceSingleQuotes(opDetails.Address);

                        //operatorList.operators = new List<ExOzOperatorResponse>();
                        OperatorList operatorDetails = new OperatorList()
                        {
                            operators = new List<ExOzOperatorResponse>()
                        };

                        operatorDetails.operators.Add(opDetails);
                        SaveExOzOperatorCommandResult retOperators = new SaveExOzOperatorCommandResult()
                        {
                            OperatorList = new List<ExOzOperator>()
                        };

                        retOperators = await _commandSender.Send<SaveExOzOperatorCommand, SaveExOzOperatorCommandResult>(new SaveExOzOperatorCommand
                        {
                            OperatorList = operatorDetails.operators,
                            ModifiedBy = new Guid("C043DDEE-D0B1-48D8-9C3F-309A77F44795")

                        });

                        if (opDetails.Products != null)
                        {
                            ProductList productList = new ProductList
                            {
                                Products = new List<ExOzProductResponse>()
                            };

                            foreach (var prod in opDetails.Products)
                            {
                                prod.CanonicalRegionUrlSegment = opDetails.CanonicalRegionUrlSegment;
                                prod.OperatorUrlSegment = opDetails.UrlSegment;
                                productList.Products.Add(prod);
                            }

                            _logger.StartMsg("Product");
                            SessionList sessionResponse = await _productSynchronizer.Synchronize(productList);
                        }

                    }
                    i++;
                    line = _logger.Update(i, operatorList.operators.Count, line);


                }
                catch (Exception e)
                {
                    _logger.Log($"Exception: {e.Message}");
                    continue;
                }
            }
            ////Insert Operators here
            //try
            //{


            //    _logger.FinishMsg(retOperators.OperatorList.Count, "Operators");
            //    return productList;
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
