using System;
using System.Threading.Tasks;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Foundation.Senders;
using FIL.Web.Kitms.Feel.ViewModels.Home;
using FIL.Web.Kitms.Feel.ViewModels.Fulfilment;
using Microsoft.AspNetCore.Mvc;
using FIL.Messaging.Senders;
using FIL.Messaging.Models.TextMessages;
using FIL.Web.Core.Providers;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.Queries.FulFilment;

namespace FIL.Web.Kitms.Feel.Controllers
{

    public class FulfillmentController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ICommandSender _commandSender;
        private readonly ITwilioTextMessageSender _twilioTextMessageSender;
        private readonly IGupShupTextMessageSender _gupShupTextMessageSender;
        private readonly ISessionProvider _sessionProvider;

        public FulfillmentController(IQuerySender querySender, ITwilioTextMessageSender twilioTextMessageSender, IGupShupTextMessageSender gupShupTextMessageSender, ICommandSender commandSender, ISessionProvider sessionProvider)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _sessionProvider = sessionProvider;
            _twilioTextMessageSender = twilioTextMessageSender;
            _gupShupTextMessageSender = gupShupTextMessageSender;
        }

        [HttpPost]
        [Route("api/fulfillment/getfulfilmemtData")]
        public async Task<TransactionLocatorResponseViewModel> GetFulfilmentDetails([FromBody]TransactionLocatorFormDataViewModel model)
        {
            TransactionLocatorResponseViewModel transactionLocatorResponseViewModel = new TransactionLocatorResponseViewModel();
            try
            {
                var QueryResult = await _querySender.Send(new TransactionLocatorQuery
                {
                    TransactionId = model.ConfirmationNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailId = model.EmailId,
                    UserMobileNo = model.UserMobileNo,
                    BarcodeNumber = model.BarcodeNumber,
                    IsFulfilment = true
                });
                if (QueryResult.TransactionInfos != null)
                {
                    transactionLocatorResponseViewModel.transactionInfos = QueryResult.TransactionInfos;
                }
                else
                {
                    transactionLocatorResponseViewModel.transactionInfos = null;
                }
            }
            catch (Exception ex)
            {
                transactionLocatorResponseViewModel.transactionInfos = null;
            }
            return transactionLocatorResponseViewModel;
        }
        [HttpGet]
        [Route("api/fulfillment/generate-otp/{phoneCode}/{phoneNumber}/{transDetailAltId}")]
        public async Task<GenerateOTPResponseViewModel> GenerateOTP(string phoneCode, string phoneNumber, Guid transDetailAltId)
        {
            try
            {
                Random rd = new Random();
                var session = await _sessionProvider.Get();
                string OTP = await SendOTP(rd, phoneCode.ToString(), phoneNumber);
                await _commandSender.Send(new SaveOTPCommand
                {
                    PickupOTP = Convert.ToInt32(OTP),
                    TransDetailAltId = transDetailAltId,
                });
                return new GenerateOTPResponseViewModel
                {
                    Success = true,
                };
            }

            catch (Exception e)
            {
                return new GenerateOTPResponseViewModel
                {
                    Success = false,
                };
            }
        }
        [HttpPost]
        [Route("api/fulfillment/submitFulFilmentDetails")]
        public async Task<SaveFulFilmentResponseModel> SubmitFulFilmentDetails([FromBody]SaveFulFilmentFormDataModel model)
        {
            {
                try
                {
                    var queryResult = await _querySender.Send(new ValidotpQuery
                    {
                        TransactionDetailId = model.transactionDetailId,
                        PickupOTP = model.pickupOTP
                    });
                    if (!queryResult.IsValid)
                    {
                        return new SaveFulFilmentResponseModel
                        {
                            IsValid = false,
                            Message = "Invalid OTP"
                        };
                    }
                    else
                    {
                        var session = await _sessionProvider.Get();
                        await _commandSender.Send(new SaveFulFilmentDetailCommand
                        {
                            TransactionDetailId = model.transactionDetailId,
                            TicketNumber = model.ticketNumber,
                        });
                        return new SaveFulFilmentResponseModel
                        {
                            IsValid = true,
                            Message = "Successfully Submitted"
                        };
                    }
                }
                catch (Exception e)
                {
                    return new SaveFulFilmentResponseModel
                    {
                        IsValid = false,
                        Message = "Something went wrong"
                    };
                }
            }
        }
        public async Task<string> SendOTP(Random rd, string PhoneCode, string PhoneNumber)
        {
            int otpLength = 6;
            string otpChars = "1234567890";
            char[] otpChar = new char[otpLength];
            for (int i = 0; i < otpLength; i++)
            {
                otpChar[i] = otpChars[rd.Next(0, otpChars.Length)];
            }
            string OTP = new string(otpChar);
            try
            {
                TextMessage message = new TextMessage();
                //message.To = "+" + model.UserDetail.PhoneCode + "" + model.UserDetail.PhoneNumber;
                message.To = "+" + PhoneCode + "" + PhoneNumber;
                message.From = "feelitLIVE";
                message.Body = "Your Redemption OTP is " + OTP + ".";

                //if (PhoneCode == "91")
                //{
                //    await _gupShupTextMessageSender.Send(message);
                //}
                //else
                //{
                await _twilioTextMessageSender.Send(message);
                //}

            }
            catch (Exception e)
            {
            }
            return OTP;
        }
    }
}
