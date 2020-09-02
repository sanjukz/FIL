using Autofac;
using FIL.Api.QueryHandlers;
using FIL.Contracts.Interfaces.Queries;
using FIL.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.Controllers
{
    public class QueryController : Controller
    {
        private readonly IComponentContext _componentContext;
        private readonly ILogger _logger;
        private static readonly IDictionary<Type, Type> _queryToHandlerDictionary = new ConcurrentDictionary<Type, Type>();

        public QueryController(ILogger logger,
            IComponentContext componentContext)
        {
            _logger = logger;
            _componentContext = componentContext;
        }

        [Route("api/query/{queryName}")]
        [HttpPost]
        public Task<object> RouteQuery([FromBody] object query)
        {
            if (query == null)
            {
                return null;
            }
            var queryType = query.GetType();
            Type handlerType;
            if (_queryToHandlerDictionary.ContainsKey(queryType))
            {
                handlerType = _queryToHandlerDictionary[queryType];
            }
            else
            {
                var queryInterfaceType = queryType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));
                var queryResultType = queryInterfaceType.GetGenericArguments().First();
                handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, queryResultType);
                _queryToHandlerDictionary[queryType] = handlerType;
            }
            var handler = _componentContext.Resolve(handlerType);
            var result = handlerType.GetMethod("Handle").Invoke(handler, new[] { query });

            // TODO Caching

            return Task.FromResult(result);
        }
    }
}