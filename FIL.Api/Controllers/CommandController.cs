using Autofac;
using FIL.Api.CommandHandlers;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Exceptions;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.Controllers
{
    public class CommandController : Controller
    {
        private readonly IComponentContext _componentContext;
        private readonly ILogger _logger;
        private readonly IDataSettings _dataSettings;

        public CommandController(ILogger logger,
            IComponentContext componentContext,
            IDataSettings dataSettings)
        {
            _logger = logger;
            _componentContext = componentContext;
            _dataSettings = dataSettings;
        }

        [Route("api/command/{commandName}")]
        [HttpPost]
        public async Task RouteCommand([FromBody] Contracts.Interfaces.Commands.ICommand command)
        {
            var commandType = command.GetType();

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
            dynamic handler = _componentContext.Resolve(handlerType);

            _dataSettings.UnitOfWork.BeginTransaction();
            try
            {
                await handler.Handle(command);
                _dataSettings.UnitOfWork.Commit();
            }
            catch (Exception)
            {
                _dataSettings.UnitOfWork.Rollback();
                throw;
            }
        }

        [Route("api/command/{commandName}/result")]
        [HttpPost]
        public async Task<ICommandResult> RouteCommandWithResult([FromBody] Contracts.Interfaces.Commands.ICommand command)
        {
            var commandType = command.GetType();

            var handlerType =
                typeof(ICommandHandlerWithResult<,>).MakeGenericType(commandType, GetResultType(commandType));
            dynamic handler = _componentContext.Resolve(handlerType);

            ICommandResult result;
            _dataSettings.UnitOfWork.BeginTransaction();
            try
            {
                result = await handler.Handle(command);
                _dataSettings.UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _dataSettings.UnitOfWork.Rollback();
                throw;
            }

            return result;
        }

        private Type GetResultType(Type commandType)
        {
            var resultInterface = commandType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandWithResult<>));
            if (resultInterface == null)
            {
                throw new CustomException("Attempted to send a void command to the endpoint for commands with return values",
                    new Dictionary<string, object>
                    {
                        {"CommandType", commandType}
                    });
            }
            return resultInterface.GetGenericArguments().Single();
        }
    }
}