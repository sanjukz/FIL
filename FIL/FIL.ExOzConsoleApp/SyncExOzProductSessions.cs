using FIL.Contracts.Commands.ExOz;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations.ExOz;
using FIL.Foundation.Senders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzProductOptionResponse;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzSessionResponse;

namespace FIL.ExOzConsoleApp
{
    public class SyncExOzProductSessions : ISynchronizer<ProductOptionList, SessionList>
    {
        private readonly ICommandSender _commandSender;
        private readonly IConsoleLogger _logger;
        private readonly ISynchronizer<List<ExOzProductOption>, ProductOptionList> _productOptionSynchronizer;

        public SyncExOzProductSessions(ICommandSender commandSender, IConsoleLogger logger,
            ISynchronizer<List<ExOzProductOption>, ProductOptionList> productOptionSynchronizer)
        {
            _commandSender = commandSender;
            _logger = logger;
            _productOptionSynchronizer = productOptionSynchronizer;
        }

        public async Task<ProductOptionList> Synchronize(SessionList sessionList)
        {
            string line = "";

            int i = 0;
            foreach (var sess in sessionList.ProductSessions)
            {
                SessionList sessionDetails = new SessionList()
                {
                    ProductSessions = new List<ExOzSessionResponse>()
                };

                sessionDetails.ProductSessions.Add(sess);

                SaveExOzSessionCommandResult retSessions = new SaveExOzSessionCommandResult()
                {
                    SessionList = new List<ExOzProductSession>()
                };

                retSessions = await _commandSender.Send<SaveExOzSessionCommand, SaveExOzSessionCommandResult>(new SaveExOzSessionCommand
                {
                    SessionList = sessionDetails.ProductSessions,
                    ModifiedBy = new Guid("C043DDEE-D0B1-48D8-9C3F-309A77F44793")
                });

                if (sess.ProductOptions != null)
                {
                    ProductOptionList optionList = new ProductOptionList()
                    {
                        ProductOptions = new List<ExOzProductOptionResponse>()
                    };

                    foreach (var opt in sess.ProductOptions)
                    {
                        opt.SessionId = sess.Id;
                        opt.SessionName = sess.SessionName;
                        optionList.ProductOptions.Add(opt);
                    }

                    _logger.StartMsg("Options");
                    await _productOptionSynchronizer.Synchronize(optionList);
                }
                
                i++;
                line = _logger.Update(i, sessionList.ProductSessions.Count, line);
            }

            //Insert Sessions Here
            //try
            //{

            //    _logger.FinishMsg(retSessions.SessionList.Count, "Sessions");
            //    return optionList;
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
