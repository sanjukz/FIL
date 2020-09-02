using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.FeelOrderConfirmation;
using FIL.Contracts.QueryResults.FeelOrderConfirmation;
using FIL.Foundation.Senders;
using FIL.MailChimp;
using FIL.Messaging.Senders;
using FIL.Web.Core;
using FIL.Web.Feel.Modules.SiteExtensions;
using FIL.Web.Feel.Providers;
using FIL.Web.Feel.ViewModels.OrderConformation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Web.Feel.Controllers
{
    public class OrderConformationController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ICommandSender _commandSender;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IConfirmationEmailSender _confirmationEmailSender;
        private readonly IAmazonS3FileProvider _amazonS3FileProvider;
        private readonly ILocalDateTimeProvider _localDateTimeProvider;
        private readonly IAccountEmailSender _accountEmailSender;
        private readonly IMailChimpProvider _mailChimpProvider;
        private readonly IGeoCurrency _geoCurrency;

        public OrderConformationController(IQuerySender querySender,
            IConfirmationEmailSender confirmationEmailSender,
            IAmazonS3FileProvider amazonS3FileProvider,
            FIL.Logging.ILogger logger,
            ILocalDateTimeProvider localDateTimeProvider,
            ICommandSender commandSender,
            IAccountEmailSender accountEmailSender,
            IGeoCurrency geoCurrency,
            IMailChimpProvider mailChimpProvider)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _confirmationEmailSender = confirmationEmailSender;
            _amazonS3FileProvider = amazonS3FileProvider;
            _localDateTimeProvider = localDateTimeProvider;
            _logger = logger;
            _accountEmailSender = accountEmailSender;
            _mailChimpProvider = mailChimpProvider;
            _geoCurrency = geoCurrency;
        }

        [HttpPost]
        [Route("api/orderconfirmations")]
        public async Task<OrderConfirmationResponseDataViewModel> GetOrderConfirmation([FromBody] GetOrderConfirmationDataViewModel model)
        {
            var utcOffset = "";
            var queryResult = await _querySender.Send(new FeelOrderConfirmationQuery
            {
                TransactionAltId = model.transactionId,
                confirmationFrmMyOrders = model.confirmationFromMyOrders,
                Channel = FIL.Contracts.Enums.Channels.Feel
            });

            try
            {
                var serialized = JsonConvert.SerializeObject(queryResult);
                var query = JsonConvert.DeserializeObject<FeelOrderConfirmationQueryResult>(serialized);
                query.Transaction.NetTicketAmount = _geoCurrency.Exchange((decimal)query.Transaction.NetTicketAmount, query.CurrencyType.Code);
                query.Transaction.GrossTicketAmount = _geoCurrency.Exchange((decimal)query.Transaction.GrossTicketAmount, query.CurrencyType.Code);
                query.Transaction.ServiceCharge = _geoCurrency.Exchange(query.Transaction.ServiceCharge != null ? (decimal)query.Transaction.ServiceCharge : 0, query.CurrencyType.Code);
                query.Transaction.ConvenienceCharges = _geoCurrency.Exchange(query.Transaction.ConvenienceCharges != null ? (decimal)query.Transaction.ConvenienceCharges : 0, query.CurrencyType.Code);
                foreach (var item in query.orderConfirmationSubContainer)
                {
                    foreach (var eventContainer in item.subEventContainer)
                    {
                        foreach (var eta in eventContainer.EventTicketAttribute)
                        {
                            _geoCurrency.eventTicketAttributeUpdate(eta, HttpContext, "USD");
                        }
                        foreach (var td in eventContainer.TransactionDetail)
                        {
                            _geoCurrency.updateTransactionDetail(td, HttpContext, query.CurrencyType.Id, "USD");
                        }
                    }
                }
                await _mailChimpProvider.AddBuyerOrder(query);
                var lastEvent = query.orderConfirmationSubContainer.LastOrDefault().subEventContainer.LastOrDefault();
                await _mailChimpProvider.AddFILMemberLastDetails(new MailChimp.Models.MCUserAdditionalDetailModel
                {
                    LastEventName = lastEvent.Event.Name,
                    LastEventTicketCategory = lastEvent.TicketCategory.LastOrDefault().Name,

                    LastPurchaseChannel = queryResult.Transaction.ChannelId,
                    LastPurchaseAmount = queryResult.Transaction.NetTicketAmount.ToString(),
                    LastPurchaseDate = queryResult.Transaction.CreatedUtc.ToString(),
                    LastEventCategory = lastEvent.EventCategory.DisplayName
                });
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
            }

            if (Request.Cookies["utcTimeOffset"] != null)
            {
                utcOffset = Request.Cookies["utcTimeOffset"];
            }
            try
            {
                var onlineStreamLink = "";
                if (queryResult.Transaction != null)
                {

                    var transactionDate = queryResult.Transaction.CreatedUtc;
                    if (utcOffset.Contains("+"))
                    {
                        var hours = Convert.ToInt32(utcOffset.Split(":")[0].Split("+")[1]);
                        var mins = Convert.ToInt32(utcOffset.Split(":")[1]);
                        transactionDate = transactionDate.AddHours(hours).AddMinutes(mins);
                    }
                    else if (utcOffset.Contains("-"))
                    {
                        var hours = Convert.ToInt32(utcOffset.Split(":")[0].Split("-")[1]);
                        var mins = Convert.ToInt32(utcOffset.Split(":")[1]);
                        transactionDate = transactionDate.AddHours(-hours).AddMinutes(-mins);
                    }
                    queryResult.Transaction.CreatedUtc = transactionDate;

                    //Check if order for Hoho
                    if (queryResult.IsHoho)
                    {
                        try
                        {
                            CreateBookingCommandResult createbooking = await _commandSender.Send<CreateBookingCommand, CreateBookingCommandResult>(new CreateBookingCommand { TransactionId = queryResult.Transaction.Id });
                        }
                        catch (Exception ex)
                        {
                            _logger.Log(FIL.Logging.Enums.LogCategory.Error, ex);
                        }
                    }

                    ConfirmOrderCommandResult confirmOrder = new ConfirmOrderCommandResult();
                    // for tiqets, Confirm the Order Here
                    if (queryResult.IsTiqets)
                    {
                        try
                        {
                            confirmOrder = await _commandSender.Send<ConfirmOrderCommand, ConfirmOrderCommandResult>(new ConfirmOrderCommand { TransactionId = queryResult.Transaction.Id });
                        }
                        catch (Exception ex)
                        {
                            _logger.Log(FIL.Logging.Enums.LogCategory.Error, ex);
                        }

                    }

                    if (!queryResult.IsLiveOnline && ((queryResult.IsTiqets && confirmOrder.Success) || !queryResult.IsTiqets))
                    {
                        _amazonS3FileProvider.UploadImage(queryResult.Transaction.Id.ToString(), ImageType.QrCode);

                        var transactionid = "FAP" + (Convert.ToInt64(queryResult.Transaction.Id) * 6) + "0019" + queryResult.Transaction.Id;

                        StringBuilder strnotes = new StringBuilder();
                        StringBuilder sbticketdetails = new StringBuilder();
                        StringBuilder tiqetsPdfDetails = new StringBuilder();

                        foreach (var orderconfiramtionContainer in queryResult.orderConfirmationSubContainer)
                        {
                            foreach (var subEventContainer in orderconfiramtionContainer.subEventContainer)
                            {

                                foreach (var eventdeliverytypeDetail in subEventContainer.EventDeliveryTypeDetail)
                                {
                                    var notes = eventdeliverytypeDetail.Notes;
                                    strnotes.Append("<td style = 'font-family:Verdana, Geneva, sans-serif; font-size:11px; line-height:20px; color:#323232; align = 'left'; ' > " + notes + " </td>");
                                    strnotes.Append("<br/>");
                                }
                            }
                        }
                        string streventname = string.Empty;
                        var transactionDetail = new FIL.Contracts.Models.TransactionDetail();
                        var transactions = new FIL.Contracts.Models.Transaction();
                        List<FIL.Contracts.Models.EventAteendeeDetail> eventAttendees = new List<Contracts.Models.EventAteendeeDetail>();

                        sbticketdetails.Append("<tr>");
                        sbticketdetails.Append("<td valign=\'top\' align=\'left\'>");
                        sbticketdetails.Append("<table width=\'100%\' cellspacing=\'0\' cellpadding=\'0\' border=\'0\' align=\'left\'>");
                        sbticketdetails.Append("<tbody><tr>");
                        sbticketdetails.Append(" <td style=\'line-height:10px;height:10px;\' valign=\'top\' align=\'left\'>&nbsp;</td>");
                        sbticketdetails.Append(" </tr>");
                        sbticketdetails.Append(" <tr>");
                        sbticketdetails.Append(" <td style=\'font-family:Verdana, Geneva, sans-serif; font-size:12px; line-height:18px; color:#000000;\'>");
                        sbticketdetails.Append(" <table width=\'100%\' cellspacing=\'0\' cellpadding=\'0\' border=\'0\' align=\'right\'>");
                        sbticketdetails.Append(" <tbody><tr>");
                        sbticketdetails.Append(" <td style=\'font-family:Verdana, Geneva, sans-serif; font-size:13px; line-height:18px; color:#000000; font-weight:bold;\' valign=\'top\' align=\'left\'>Your Ticket Details</td>");
                        sbticketdetails.Append(" </tr>");

                        foreach (var orderconfiramtionContainer in queryResult.orderConfirmationSubContainer)
                        {
                            string venuename = "", startdatetime = "";
                            decimal netTicketamount = 0;

                            foreach (var subEventContainer in orderconfiramtionContainer.subEventContainer)
                            {
                                venuename = subEventContainer.Venue.Name.ToString();
                                foreach (var currentTransactionDetail in subEventContainer.TransactionDetail)
                                {
                                    startdatetime = transactionDetail.VisitDate.ToString("MMM dd, yyyy HH:mm").ToUpper();
                                    if (transactionDetail.TransactionType == TransactionType.Itinerary)
                                        startdatetime = transactionDetail.VisitDate.ToString("MMM dd, yyyy").ToUpper();
                                    netTicketamount = netTicketamount + ((currentTransactionDetail.TotalTickets) * (currentTransactionDetail.PricePerTicket));
                                    transactionDetail = currentTransactionDetail;
                                }
                            }

                            sbticketdetails.Append(" <tr>");
                            sbticketdetails.Append(" <td style=\'line-height:10px;border-bottom:1px solid #ffffff; height:10px;\' valign=\'top\' align=\'left\'>&nbsp;</td>");
                            sbticketdetails.Append(" </tr>");
                            sbticketdetails.Append(" <tr>");
                            sbticketdetails.Append(" <td style=\'line-height:5px;height:5px;\' valign=\'top\' align=\'left\'>&nbsp;</td>");
                            sbticketdetails.Append(" </tr>");
                            sbticketdetails.Append(" <tr>");
                            sbticketdetails.Append(" <td valign=\'top\' align=\'left\'>");
                            sbticketdetails.Append(" <table width=\'100%\' cellspacing=\'0\' cellpadding=\'0\' border=\'0\' align=\'right\'>");
                            sbticketdetails.Append(" <tbody><tr>");
                            sbticketdetails.Append(" <td style=\'font-family:Verdana, Geneva, sans-serif; font-size:11px; line-height:26px; color:#000000; border-bottom:1px solid #ffffff; font-weight:bold\' width=\'57%\'>" + orderconfiramtionContainer.Event.Name + "</td>");
                            sbticketdetails.Append(" <td style=\'font-family:Verdana, Geneva, sans-serif; font-size:11px; line-height:26px; color:#000000; border-bottom:1px solid #ffffff; font-weight:bold\' width=\'43%\' align=\'right\'>" + String.Format("{0:0.00}", netTicketamount) + "</td>");
                            sbticketdetails.Append(" </tr>");
                            sbticketdetails.Append(" </tbody></table>");
                            sbticketdetails.Append(" </td>");
                            sbticketdetails.Append(" </tr>");
                            sbticketdetails.Append(" <tr>");
                            sbticketdetails.Append(" <td style=\'font-family:Verdana, Geneva, sans-serif; font-size:11px; line-height:18px; color:#6a6a6a;\'>" + (transactionDetail.TransactionType == TransactionType.Itinerary ? (transactionDetail.VisitDate.Hour + ":" + transactionDetail.VisitDate.Minute + " - " + transactionDetail.VisitEndDate.Hour + ":" + transactionDetail.VisitEndDate.Minute) : transactionDetail.VisitDate.ToString()) + "</td>");
                            sbticketdetails.Append(" </tr>");
                            sbticketdetails.Append(" <tr>");
                            sbticketdetails.Append(" <td style=\'font-family:Verdana, Geneva, sans-serif; font-size:11px; line-height:18px; color:#6a6a6a;\'>" + venuename + "</td>");
                            sbticketdetails.Append(" </tr>");

                            foreach (var subEventContainer in orderconfiramtionContainer.subEventContainer)
                            {
                                foreach (var currentTransactionDetail in subEventContainer.TransactionDetail)
                                {
                                    var currentEventAttributes = subEventContainer.EventTicketAttribute.ToList().Where(s => s.Id == currentTransactionDetail.EventTicketAttributeId).FirstOrDefault();
                                    var currentEventTicketDetails = subEventContainer.EventTicketDetail.ToList().Where(s => s.Id == currentEventAttributes.EventTicketDetailId).FirstOrDefault();
                                    var currentTicketCategory = subEventContainer.TicketCategory.ToList().Where(s => s.Id == currentEventTicketDetails.TicketCategoryId).FirstOrDefault();
                                    var Category = currentTicketCategory.Name.ToString();
                                    sbticketdetails.Append(" <tr>");
                                    sbticketdetails.Append(" <td style=\'line-height:5px;height:5px;\' valign=\'top\' align=\'left\'>&nbsp;</td>");
                                    sbticketdetails.Append(" </tr>");
                                    sbticketdetails.Append(" <tr>");
                                    sbticketdetails.Append(" <td style=\'line-height:6px;border-bottom:1px solid #ffffff; height:6px;\' valign=\'top\' align=\'left\'>&nbsp;</td>");
                                    sbticketdetails.Append(" </tr>");
                                    sbticketdetails.Append(" <tr>");
                                    sbticketdetails.Append(" <td style=\'line-height:5px;height:5px;\' valign=\'top\' align=\'left\'>&nbsp;</td>");
                                    sbticketdetails.Append(" </tr>");
                                    sbticketdetails.Append(" <tr>");
                                    sbticketdetails.Append(" <td valign=\'top\' align=\'left\'>");
                                    sbticketdetails.Append(" <table width=\'100%\' cellspacing=\'0\' cellpadding=\'0\' border=\'0\' align=\'right\'>");
                                    sbticketdetails.Append(" <tbody><tr>");
                                    sbticketdetails.Append(" <td style=\'font-family:Verdana, Geneva, sans-serif; font-size:11px; line-height:18px; color:#6a6a6a;\'><span style=\'color:#000000\'>" + (currentTransactionDetail.TransactionType == TransactionType.Itinerary ? (currentTransactionDetail.TicketTypeId == 10 ? "Adult" : "Child") : Category) + "</span>"
                                    + "(" + currentTransactionDetail.TotalTickets + "x" + queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", currentTransactionDetail.PricePerTicket) + ")</td>");
                                    sbticketdetails.Append(" <td style=\'font-family:Verdana, Geneva, sans-serif; font-size:11px; line-height:18px; color:#6a6a6a;\' align=\'right\'>" + queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", (currentTransactionDetail.TotalTickets * currentTransactionDetail.PricePerTicket)) + "</td>");
                                    sbticketdetails.Append(" </tr>");
                                    sbticketdetails.Append(" </tbody></table>");
                                    sbticketdetails.Append(" </td>");
                                    sbticketdetails.Append(" </tr>");
                                    sbticketdetails.Append(" <tr>");
                                    sbticketdetails.Append(" <td style=\'line-height:5px;height:5px;\' valign=\'top\' align=\'left\'>&nbsp;</td>");
                                    sbticketdetails.Append(" </tr>");
                                    sbticketdetails.Append(" </tbody></table>");
                                    sbticketdetails.Append(" </td>");
                                    sbticketdetails.Append(" </tr>");
                                    sbticketdetails.Append(" </tbody></table>");
                                    sbticketdetails.Append(" </td>");
                                    sbticketdetails.Append(" </tr>");
                                }
                            }
                            if (queryResult.IsTiqets)
                            {
                                tiqetsPdfDetails.Append("<a href=" + confirmOrder.TicketPdfLink + " style=\'font-size:12px;text-decoration: none;padding:10px 10px;display:inline-block;background: #572483;color: #fff;border-radius:5px\';> Get Your Ticket</a>");
                            }
                        }

                        FIL.Messaging.Models.Emails.Email email = new FIL.Messaging.Models.Emails.Email();
                        email.To = queryResult.Transaction.EmailId;
                        email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>";
                        email.TemplateName = "FeelConfirmationEmail01";
                        email.ConfigurationSetName = "FIL-Confirmation";
                        email.Variables = new Dictionary<string, object>
                        {
                            ["sitelogo"] = "<img src='https://static6.feelitlive.com/images/logos/feel-aplace.png'>",
                            ["sitename"] = "feelitlive.com",
                            ["transactionid"] = transactionid,
                            ["datetimestamp"] = _localDateTimeProvider.GetLocalDateTime(queryResult.Transaction.CreatedUtc).ToString("MMM dd, yyyy HH:mm").ToUpper(),
                            ["visitdatetimestamp"] = _localDateTimeProvider.GetLocalDateTime(transactionDetail.VisitDate).ToString("MMM dd, yyyy HH:mm").ToUpper(),
                            ["name"] = queryResult.Transaction.FirstName + " " + queryResult.Transaction.LastName,
                            ["nameoncard"] = queryResult.Transaction.FirstName + " " + queryResult.Transaction.LastName,
                            ["email"] = queryResult.Transaction.EmailId,
                            ["phonenumber"] = queryResult.Transaction.PhoneNumber == "" ? "" : "+" + queryResult.Transaction.PhoneCode + " - " + queryResult.Transaction.PhoneNumber,
                            ["paymentmode"] = queryResult.TransactionPaymentDetail.PaymentOptionId + " - " + queryResult.UserCardDetail.CardTypeId,
                            ["grossticketamount"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", queryResult.Transaction.GrossTicketAmount),
                            ["conveniencecharges"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", (queryResult.Transaction.ConvenienceCharges + queryResult.Transaction.ServiceCharge)),
                            ["netamount"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", queryResult.Transaction.NetTicketAmount),
                            ["notes"] = strnotes.ToString(),
                            ["ticketdetails"] = sbticketdetails.ToString(),
                            ["qrcode"] = "",
                            ["eticketlink"] = "<a href='https://www.feelitlive.com/pah/" + queryResult.Transaction.Id + "'></a>",
                            ["tiqetPdfDetails"] = tiqetsPdfDetails.ToString()

                        };
                        if (!model.confirmationFromMyOrders)
                        {
                            await _confirmationEmailSender.Send(email);
                        }
                    }
                    //sending email for live online events users
                    else if (queryResult.IsLiveOnline)
                    {
                        var streamDateTimeString = GetStreamDate(queryResult);
                        var transactionid = "FAP" + (Convert.ToInt64(queryResult.Transaction.Id) * 6) + "0019" + queryResult.Transaction.Id;
                        var hostName = HttpContext.Request.Host.ToString();
                        string host = "https://feelitlive.com/";
                        if (hostName.Contains("dev"))
                        {
                            host = "https://dev.feelitlive.com/";
                        }
                        else if (hostName.Contains("demo"))
                        {
                            host = "https://demo.feelitlive.com/";
                        }
                        string streamLink = (queryResult.ZoomUser != null && queryResult.liveOnlineDetailModel.TicketCategoryId != 19452 && queryResult.liveOnlineDetailModel.TicketCategoryId != 12259) ? host + "stream-online?token=" + queryResult.ZoomUser.AltId.ToString() : "";
                        var onlinestreaminglink = "<a href='" + streamLink + "'><img src='https://static6.feelitlive.com/images/feel-mail/online-stream-link.png' width='231' height='36px' style='border:0' alt='Online Stream Link' /></a>";
                        if (queryResult.orderConfirmationSubContainer.FirstOrDefault().Event.Id == 15645)
                        {
                            onlinestreaminglink = onlinestreaminglink + "<div>" + "<div style='font-family:Verdana,Geneva,Tahoma,sans-serif;font-size:14px;line-height:20px;color:#333;font-weight: bold;'>YAKOV SOCIAL MEDIA</div>" +
                                "<a style='margin-right:10px;' href='https://twitter.com/Yakov_Smirnoff' target='_blank'> <img src='https://s3-us-west-2.amazonaws.com/feelaplace-cdn/images/fil-images/social/twitter.png' alt='FIL Twitter' width='20' /></a>" +
                                "<a style='margin-right:10px;text-decoration:none' href='https://m.facebook.com/Yakov.Smirnoff.Comedian/' target='_blank'> <img src='https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/social/facebook.png' alt='FIL Facebook' width='20' /></a>" +
                                "<a style='margin-right:10px;text-decoration:none' href='https://www.instagram.com/yakov_smirnoff/' target='_blank' > <img src='https://s3-us-west-2.amazonaws.com/feelaplace-cdn/images/fil-images/social/instagram.png' alt='FIL Instagram' width='20' /></a>" +
                                "</div>";
                        }
                        FIL.Messaging.Models.Emails.Email email = new FIL.Messaging.Models.Emails.Email();
                        email.To = queryResult.Transaction.EmailId;
                        email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>";
                        // If Ticket Category is not Donate
                        if (queryResult.ZoomUser != null && queryResult.liveOnlineDetailModel.TicketCategoryId != 19452 && queryResult.liveOnlineDetailModel.TicketCategoryId != 12259)
                        {
                            email.TemplateName = "attendeeorderconfirmation";
                            email.Variables = new Dictionary<string, object>
                            {
                                ["eventimage"] = "<img style='width:100%;' width='100%' alt='Event banner' src='https://s3-us-west-2.amazonaws.com/feelaplace-cdn/images/places/about/" + queryResult.orderConfirmationSubContainer[0].Event.AltId.ToString().ToUpper() + "-about.jpg' />",
                                ["confirmationnumber"] = transactionid,
                                ["fullname"] = queryResult.Transaction.FirstName + " " + queryResult.Transaction.LastName,
                                ["modeofpayment"] = queryResult.TransactionPaymentDetail.PaymentOptionId,
                                ["email"] = queryResult.Transaction.EmailId,
                                ["phonenumber"] = queryResult.Transaction.PhoneNumber == "" ? "" : "+" + queryResult.Transaction.PhoneCode + " - " + queryResult.Transaction.PhoneNumber,
                                ["subtotal"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", queryResult.Transaction.GrossTicketAmount - (queryResult.Transaction.DonationAmount == null ? 0 : queryResult.Transaction.DonationAmount)),
                                ["bookingfee"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", (queryResult.Transaction.ConvenienceCharges + queryResult.Transaction.ServiceCharge)),
                                ["discountamount"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", (queryResult.Transaction.DiscountAmount == null ? 0 : queryResult.Transaction.DiscountAmount)),
                                ["donationamount"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", (queryResult.Transaction.DonationAmount == null ? 0 : queryResult.Transaction.DonationAmount)),
                                ["totalamountpaid"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", queryResult.Transaction.NetTicketAmount),
                                ["bookdatetime"] = transactionDate.ToString("MMM dd, yyyy HH:mm").ToUpper(),
                                ["eventname"] = queryResult.liveOnlineDetailModel.Name,
                                ["onlinestreaminglink"] = onlinestreaminglink,
                                ["streamdatetime"] = streamDateTimeString
                            };
                        }
                        else if (queryResult.liveOnlineDetailModel.TicketCategoryId == 19452 || queryResult.liveOnlineDetailModel.TicketCategoryId == 12259)
                        {
                            email.TemplateName = "donationconfirmation";
                            email.Variables = new Dictionary<string, object>
                            {
                                ["eventimage"] = "<img style='width:100%;' width='100%' alt='Event banner' src='https://s3-us-west-2.amazonaws.com/feelaplace-cdn/images/places/about/" + queryResult.orderConfirmationSubContainer[0].Event.AltId.ToString().ToUpper() + "-about.jpg' />",
                                ["confirmationnumber"] = transactionid,
                                ["fullname"] = queryResult.Transaction.FirstName + " " + queryResult.Transaction.LastName,
                                ["modeofpayment"] = queryResult.TransactionPaymentDetail.PaymentOptionId,
                                ["email"] = queryResult.Transaction.EmailId,
                                ["phonenumber"] = queryResult.Transaction.PhoneNumber == "" ? "" : "+" + queryResult.Transaction.PhoneCode + " - " + queryResult.Transaction.PhoneNumber,
                                ["donationamount"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", (queryResult.Transaction.DonationAmount == null ? 0 : queryResult.Transaction.DonationAmount)),
                                ["totalamountpaid"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", queryResult.Transaction.NetTicketAmount),
                                ["bookdatetime"] = transactionDate.ToString("MMM dd, yyyy HH:mm").ToUpper(),
                                ["eventname"] = queryResult.liveOnlineDetailModel.Name,
                                ["donate"] = queryResult.CurrencyType.Code + " " + String.Format("{0:0.00}", queryResult.Transaction.NetTicketAmount),
                            };
                        }
                        await _accountEmailSender.Send(email);
                        onlineStreamLink = streamLink;
                        //Sending email to all hosts
                        foreach (var currentHostUser in queryResult.hostUsersModel)
                        {
                            email.TemplateName = "hostorderconfirmation";
                            email.To = currentHostUser.email;
                            streamLink = host + "stream-online?token=" + currentHostUser.altId.ToString();
                            email.Variables = new Dictionary<string, object>
                            {
                                ["eventname"] = queryResult.liveOnlineDetailModel.Name,
                                ["onlinestreaminglink"] = "<a href='" + streamLink + "'><img src='https://static6.feelitlive.com/images/feel-mail/online-stream-link.png' width='231' height='36px' style='border:0' alt='Online Stream Link' /></a>",
                                ["datetime"] = streamDateTimeString
                            };
                            await _accountEmailSender.Send(email);
                        }
                    }
                    else
                    {
                        return new OrderConfirmationResponseDataViewModel
                        {
                        };
                    }
                }
                return new OrderConfirmationResponseDataViewModel
                {
                    TransactionQrcode = _amazonS3FileProvider.GetImagePath(queryResult.Transaction.Id.ToString(), ImageType.QrCode),
                    Transaction = queryResult.Transaction,
                    TransactionPaymentDetail = queryResult.TransactionPaymentDetail,
                    cardTypes = queryResult.cardTypes,
                    CurrencyType = queryResult.CurrencyType,
                    orderConfirmationSubContainer = queryResult.orderConfirmationSubContainer,
                    PaymentOption = queryResult.PaymentOption,
                    UserCardDetail = queryResult.UserCardDetail,
                    StreamLink = onlineStreamLink
                };

            }
            catch (Exception ex)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, ex);
                return new OrderConfirmationResponseDataViewModel
                {
                };
            }
        }


        DateTime ConvertDateTimeToLocal(string timeZoneOffset, DateTime dateTime)
        {
            if (timeZoneOffset.Contains('+') || !timeZoneOffset.Contains('-'))
            {
                var minutes = Convert.ToDouble(timeZoneOffset.Replace("+", ""));
                dateTime = dateTime.AddMinutes(minutes);
                return dateTime;
            }
            else
            {
                var minutes = Convert.ToDouble(timeZoneOffset.Replace("-", ""));
                dateTime = dateTime.AddMinutes(-minutes);
                return dateTime;
            }
        }

        string GetStreamDate(FeelOrderConfirmationQueryResult queryResult)
        {
            var orderConfSubContainer = queryResult.orderConfirmationSubContainer.FirstOrDefault().subEventContainer.FirstOrDefault();
            if (orderConfSubContainer.EventDetail.EventFrequencyType == EventFrequencyType.OnDemand)
            {
                return "On Demand";
            }
            else if (orderConfSubContainer.EventDetail.EventFrequencyType == EventFrequencyType.Recurring)
            {
                var startDateTime = queryResult.liveOnlineDetailModel.StartDateTime;
                var endDateTime = queryResult.liveOnlineDetailModel.EndDateTime;
                queryResult.orderConfirmationSubContainer.FirstOrDefault().subEventContainer.ForEach(item =>
                {
                    if (item.TransactionDetail.Where(s => s.TransactionType == TransactionType.LiveOnline).FirstOrDefault() != null)
                    {
                        if (item.ScheduleDetails.FirstOrDefault() != null)
                        {
                            startDateTime = item.ScheduleDetails.FirstOrDefault().StartDateTime;
                            endDateTime = item.ScheduleDetails.FirstOrDefault().EndDateTime;
                        }
                    }
                });
                queryResult.liveOnlineDetailModel.StartDateTime = ConvertDateTimeToLocal(orderConfSubContainer.EventAttribute.TimeZone, startDateTime);
                queryResult.liveOnlineDetailModel.EndDateTime = ConvertDateTimeToLocal(orderConfSubContainer.EventAttribute.TimeZone, endDateTime);
                var streamDateTime = queryResult.liveOnlineDetailModel.StartDateTime.ToLongDateString() + ", " + queryResult.liveOnlineDetailModel.StartDateTime.ToString("hh:mm tt") + " - " + queryResult.liveOnlineDetailModel.EndDateTime.ToString("hh:mm tt") + " " + orderConfSubContainer.EventAttribute.TimeZoneAbbreviation;
                return streamDateTime;
            }
            else
            {
                queryResult.liveOnlineDetailModel.StartDateTime = ConvertDateTimeToLocal(orderConfSubContainer.EventAttribute.TimeZone, queryResult.liveOnlineDetailModel.StartDateTime);
                var streamDateTime = queryResult.liveOnlineDetailModel.StartDateTime.ToLongDateString() + ", " + queryResult.liveOnlineDetailModel.StartTime + " - " + queryResult.liveOnlineDetailModel.EndTime + " " + orderConfSubContainer.EventAttribute.TimeZoneAbbreviation;
                return streamDateTime;
            }
        }
    }
}

