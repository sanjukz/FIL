using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FIL.Foundation.Senders;
using FIL.Contracts.Commands.Users;
using Microsoft.AspNetCore.Identity;
using System;
using FIL.Web.Feel.ViewModels.Login;
using FIL.Web.Feel.ViewModels.Account;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.Queries.UserAddress;
using FIL.Contracts.Enums;
using FIL.Web.Core.Providers;

namespace FIL.Web.Feel.Controllers
{
    public class UserAddressController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;

        public UserAddressController(ICommandSender commandSender, IQuerySender querySender, ISessionProvider sessionProvider)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _sessionProvider = sessionProvider;
        }

        [HttpPost]
        [Route("api/useraddress")]
        public async Task<SaveAddressResponseViewModel> SaveAsync([FromBody]SaveAddressFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var session = await _sessionProvider.Get();
                var result = new { Success = true };
                try
                {
                    await _commandSender.Send(new SaveAddressCommand
                    {
                        UserAltId = session.User.AltId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneCode = model.PhoneCode,
                        PhoneNumber = model.PhoneNumber,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        Zipcode = model.Zipcode,
                        AddressTypeId = model.AddressTypeId ?? 0,
                    });
                    return new SaveAddressResponseViewModel
                    {
                        Success = result.Success,
                    };
                }
                catch (Exception ex)
                {
                    return new SaveAddressResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new SaveAddressResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpPost]
        [Route("api/useraddress/setdefault")]
        public async Task<SetDefaultAddressResponseViewModel> SetDefaultAddressAsync([FromBody]SetDefaultAddressDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new { Success = true };
                try
                {
                    await _commandSender.Send(new SetDefaultAddressCommand
                    {
                        AltId = model.AltId,
                        MakeDefault = model.MakeDefault,
                    });
                    return new SetDefaultAddressResponseViewModel
                    {
                        Success = result.Success,
                    };
                }
                catch (Exception ex)
                {
                    return new SetDefaultAddressResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new SetDefaultAddressResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpPost]
        [Route("api/useraddress/delete")]
        public async Task<DeleteAddressResponseViewModel> DeleteAsync([FromBody]DeleteAddressDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new { Success = true };
                try
                {
                    await _commandSender.Send(new DeleteAddressCommand
                    {
                        AltId = model.AltId,
                    });
                    return new DeleteAddressResponseViewModel
                    {
                        Success = result.Success,
                    };
                }
                catch (Exception ex)
                {
                    return new DeleteAddressResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new DeleteAddressResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpPost]
        [Route("api/useraddress/all")]
        public async Task<AddressResponseViewModel> GetAddressList([FromBody]GetAddressesDataViewModel model)
        {
            var session = await _sessionProvider.Get();
            if (session == null && session.User == null)
            {
                return new AddressResponseViewModel { };
            }
            var queryResult = await _querySender.Send(new UserAddressQuery
            {
                UserAltId = session.User.AltId,
                AddressTypeId = model.addressType,
            });
            return queryResult.UserAddresses != null ? new AddressResponseViewModel { UserAddresses = queryResult.UserAddresses } : null;
        }
    }
}
