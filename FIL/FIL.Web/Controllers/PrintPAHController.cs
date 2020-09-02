using System;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Feel;
using FIL.Foundation.Senders;
using FIL.Web.Feel.ViewModels.PrintPAH;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Core.Providers;
using Microsoft.Extensions.Caching.Memory;
using FIL.Logging;

namespace FIL.Web.Feel.Controllers
{
    public class PrintPAHController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;
        private readonly IClientIpProvider _clientIpProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;

        public PrintPAHController(ICommandSender commandSender, IQuerySender querySender, ISessionProvider sessionProvider, IClientIpProvider clientIpProvider, IMemoryCache memoryCache, ILogger logger)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _sessionProvider = sessionProvider;
            _clientIpProvider = clientIpProvider;
            _memoryCache = memoryCache;
            _logger = logger;
        }
					          
		[HttpGet]
        [Route("api/feelaplace/assignbarcode/{transactionId}")]
        public async Task<AssignBarcodeResponseViewModel> AssignBarcode(long transactionId)
        {
            try
            {
				SaveFeelBarcodeCommandResult barcodeResult = await _commandSender.Send<SaveFeelBarcodeCommand, SaveFeelBarcodeCommandResult>(new SaveFeelBarcodeCommand
                {
                    TransactionId = transactionId,
                    ModifiedBy = new Guid("22BE7B9F-A62C-4E3C-8A9F-84449B1C9BEE"),
                });
                return new AssignBarcodeResponseViewModel { Success = true, TransactionId = barcodeResult.Id, PahDetail = barcodeResult.PahDetail };
			}
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new AssignBarcodeResponseViewModel { Success = false };
            }
        }

		




	}
}