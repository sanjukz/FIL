using FIL.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace FIL.Foundation.Senders
{
    public interface ICommandSender
    {
        Task Send<T>(T command) where T : Contracts.Interfaces.Commands.ICommand;

        Task<TR> Send<T, TR>(T command) where T : Contracts.Interfaces.Commands.ICommandWithResult<TR>
            where TR : Contracts.Interfaces.Commands.ICommandResult;

        Task Send<T>(T command, TimeSpan clientTimeout) where T : Contracts.Interfaces.Commands.ICommand;

        Task<TR> Send<T, TR>(T command, TimeSpan clientTimeout) where T : Contracts.Interfaces.Commands.ICommandWithResult<TR>
            where TR : Contracts.Interfaces.Commands.ICommandResult;
    }

    public class CommandSender : ICommandSender
    {
        private readonly MediaTypeFormatter _formatter;
        private readonly IRestHelper _defaultRestHelper;
        private readonly ConcurrentDictionary<long, IRestHelper> _restHelpers;

        public CommandSender(IRestHelper restHelper)
        {
            _defaultRestHelper = restHelper;
            _formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }
            };

            _restHelpers = new ConcurrentDictionary<long, IRestHelper>
            {
                [restHelper.ClientTimeout.Ticks] = restHelper
            };
        }

        public Task Send<T>(T command) where T : Contracts.Interfaces.Commands.ICommand
        {
            return _defaultRestHelper.Post($"api/command/{typeof(T).Name}", command, _formatter);
        }

        public Task<TR> Send<T, TR>(T command) where T : Contracts.Interfaces.Commands.ICommandWithResult<TR>
            where TR : Contracts.Interfaces.Commands.ICommandResult
        {
            return _defaultRestHelper.Post<TR>($"api/command/{typeof(T).Name}/result", command, _formatter);
        }

        public Task Send<T>(T command, TimeSpan clientTimeout) where T : Contracts.Interfaces.Commands.ICommand
        {
            return GetRestHelper(clientTimeout).Post($"api/command/{typeof(T).Name}", command, _formatter);
        }

        public Task<TR> Send<T, TR>(T command, TimeSpan clientTimeout) where T : Contracts.Interfaces.Commands.ICommandWithResult<TR>
            where TR : Contracts.Interfaces.Commands.ICommandResult
        {
            return GetRestHelper(clientTimeout).Post<TR>($"api/command/{typeof(T).Name}/result", command, _formatter);
        }

        private IRestHelper GetRestHelper(TimeSpan timeout)
        {
            if (!_restHelpers.ContainsKey(timeout.Ticks))
            {
                _restHelpers[timeout.Ticks] = CreateRestHelper(timeout);
            }

            return _restHelpers[timeout.Ticks];
        }

        private IRestHelper CreateRestHelper(TimeSpan timeout)
        {
            return new RestHelper(_defaultRestHelper.BaseAddress, timeout);
        }
    }
}