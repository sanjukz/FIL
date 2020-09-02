using System;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using System.Threading.Tasks;
using FIL.Contracts.Commands.ValueRetail;
using FIL.Logging;
using System.Diagnostics;

namespace FIL.Web.Feel.Controllers
{
    public class ValueRetailController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ILogger _logger;
        private readonly Guid ValueRetailUid = Guid.NewGuid();
        public Stopwatch stopWatch = new Stopwatch();

        public ValueRetailController(ICommandSender commandSender, IQuerySender querySender, ILogger logger)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/get-vr-villages")]
        public async Task<ValueRetailControllerResult> GetValueRetailVillages()
        {
            try
            {
                stopWatch.Reset();
                stopWatch.Start();
                await _commandSender.Send(new VillageCommand { ModifiedBy = ValueRetailUid }, new TimeSpan(1, 0, 0));
                stopWatch.Stop();
                return new ValueRetailControllerResult
                {
                    Success = true,
                    CompletionTime = stopWatch.Elapsed
                };
            }

            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, $"Value Retail Villages update failed. {ex}");
                return new ValueRetailControllerResult
                {
                    Success = false,
                    CompletionTime = stopWatch.Elapsed
                };
            }
        }

        [HttpGet]
        [Route("api/get-vr-express-route")]
        public async Task<ValueRetailControllerResult> GetValueRetailExpressRoute()
        {
            try
            {
                stopWatch.Reset();
                stopWatch.Start();
                await _commandSender.Send(new ValueRetailRouteCommand { ModifiedBy = ValueRetailUid }, new TimeSpan(1, 0, 0));
                stopWatch.Stop();

                return new ValueRetailControllerResult
                {
                    Success = true,
                    CompletionTime = stopWatch.Elapsed
                };
            }

            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, $"Value Retail Express Route update failed. {ex}");
                return new ValueRetailControllerResult
                {
                    Success = false,
                    CompletionTime = stopWatch.Elapsed
                };
            }
        }

        [HttpGet]
        [Route("api/get-vr-package-route")]
        public async Task<object> GetValueRetailPackageRoute()
        {
            try
            {
                stopWatch.Reset();
                stopWatch.Start();
                await _commandSender.Send(new ValueRetailPackageRouteCommand { ModifiedBy = ValueRetailUid }, new TimeSpan(1, 0, 0));
                stopWatch.Stop();

                return new ValueRetailControllerResult
                {
                    Success = true,
                    CompletionTime = stopWatch.Elapsed
                };
            }

            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, $"Value Retail Package Route update failed. {ex}");
                return new ValueRetailControllerResult
                {
                    Success = false,
                    CompletionTime = stopWatch.Elapsed
                };
            }
        }

        [HttpGet]
        [Route("api/get-vr-express")]
        public async Task<ValueRetailControllerResult> GetValueRetailExpress()
        {
            try
            {
                stopWatch.Reset();
                stopWatch.Start();
                await _commandSender.Send(new ShoppingExpressCommand { ModifiedBy = ValueRetailUid }, new TimeSpan(1, 0, 0));
                stopWatch.Stop();

                return new ValueRetailControllerResult
                {
                    Success = true,
                    CompletionTime = stopWatch.Elapsed
                };
            }

            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, $"Value Retail Express update failed. {ex}");
                return new ValueRetailControllerResult
                {
                    Success = false,
                    CompletionTime = stopWatch.Elapsed
                };
            }
        }

        [HttpGet]
        [Route("api/get-vr-package")]
        public async Task<ValueRetailControllerResult> GetValueRetailPackage()
        {
            try
            {
                stopWatch.Reset();
                stopWatch.Start();
                await _commandSender.Send(new ShoppingPackageCommand { ModifiedBy = ValueRetailUid }, new TimeSpan(1, 0, 0));
                stopWatch.Stop();

                return new ValueRetailControllerResult
                {
                    Success = true,
                    CompletionTime = stopWatch.Elapsed
                };
            }

            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, $"Value Retail Package update failed. {ex}");
                return new ValueRetailControllerResult
                {
                    Success = false,
                    CompletionTime = stopWatch.Elapsed
                };
            }
        }

        [HttpGet]
        [Route("api/get-vr-chauffeur-service")]
        public async Task<ValueRetailControllerResult> GetValueRetailChauffeur()
        {
            try
            {
                stopWatch.Reset();
                stopWatch.Start();
                await _commandSender.Send(new ChauffeurDrivenCommand { ModifiedBy = ValueRetailUid }, new TimeSpan(1, 0, 0));
                stopWatch.Stop();

                return new ValueRetailControllerResult
                {
                    Success = true,
                    CompletionTime = stopWatch.Elapsed
                };
            }

            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, $"Value Retail Chauffeur update failed. {ex}");
                return new ValueRetailControllerResult
                {
                    Success = false,
                    CompletionTime = stopWatch.Elapsed
                };
            }
        }

        public class ValueRetailControllerResult
        {
            public bool Success { get; set; }
            public TimeSpan CompletionTime { get; set; }
        }
    }
}
