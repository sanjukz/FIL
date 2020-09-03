using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.ASI;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Foundation.Senders;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Web.Core.Providers
{
    public interface ITicketContentProvider
    {
        Task<List<string>> GetTicketReprintPahData(Guid transactionAltId, Guid ModifiedBy, string transactionIds = "", bool? isASI = false, bool? isASIQR = false);
        Task<List<string>> GetTicketData(long transactionId,bool isASI = false, string transactionIds = "", bool? isASIQR = false);
        Task<List<string>> GetCorporateTicketData(long transactionId, TicketPrintingOption ticketPrintingOptionId, bool isPrintEntityName, Guid ModifiedBy);
    }

    public class TicketContentProvider : ITicketContentProvider
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly IAmazonS3FileProvider _amazonS3FileProvider;
        private readonly FIL.Logging.ILogger _logger;

        public TicketContentProvider(ICommandSender commandSender, IQuerySender querySender, IAmazonS3FileProvider amazonS3FileProvider, FIL.Logging.ILogger logger)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _amazonS3FileProvider = amazonS3FileProvider;
            _logger = logger;
        }

        public async Task<List<string>> GetTicketReprintPahData(Guid transactionAltId, Guid ModifiedBy, string transactionIds = "", bool? isASI = false, bool? isASIQR = false)
        {
            List<string> ticketdata = new List<string>();
            try
            {
                if (!(bool)isASIQR)
                {
                    PahCommandResult ReprintPAHResult = await _commandSender.Send<PahCommand, PahCommandResult>(new PahCommand
                    {
                        TransactionAltId = transactionAltId,
                        isASI = (bool)isASI,
                        ModifiedBy = ModifiedBy
                    });

                    if (ReprintPAHResult.Success)
                    {
                        if ((bool)!isASI)
                        {
                            var QueryResult = await _querySender.Send(new GetPahInfoQuery
                            {
                                TransactionId = ReprintPAHResult.Id,
                                ModifiedBy = ModifiedBy
                            });
                            if (QueryResult.matchSeatTicketInfo != null)
                            {
                                ticketdata = BuildPrintAtHome(QueryResult.matchSeatTicketInfo.ToList(), QueryResult.matchTeamInfo.ToList(), ReprintPAHResult.Id, TicketPrintingOption.Price, true);
                            }
                            else
                            {
                                ticketdata = null;
                            }
                        }
                        else
                        {
                            ticketdata = await GetTicketData(ReprintPAHResult.Id, true, transactionIds, false);
                        }
                    }
                }
                else
                {
                    ticketdata = await GetTicketData(0, true, transactionIds, isASIQR);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return ticketdata;
        }

        public async Task<List<string>> GetTicketData(long transactionId, bool isASI = false, string transactionIds = "", bool? isASIQR = false)
        {
            List<string> ticketdata = new List<string>();
            try
            {
                if (!isASI)
                {
                    var QueryResult = await _querySender.Send(new GetTicketInfoQuery
                    {
                        TransactionId = transactionId,
                        ModifiedBy = new Guid("2C7B7359-6039-4074-8833-80E24750916C")
                    });
                    if (QueryResult.matchSeatTicketInfo != null)
                    {
                        ticketdata = BuildPrintAtHome(QueryResult.matchSeatTicketInfo.ToList(), QueryResult.matchTeamInfo.ToList(), transactionId,TicketPrintingOption.Price,true);
                    }
                    else
                    {
                        ticketdata = null;
                    }
                }
            }
            catch (Exception ex)
            {
                ticketdata = null;
            }
            return ticketdata;        }

        public async Task<List<string>> GetCorporateTicketData(long transactionId, TicketPrintingOption ticketPrintingOptionId, bool isPrintEntityName, Guid ModifiedBy)
        {
            List<string> ticketdata = new List<string>();
            try
            {
                var QueryResult = await _querySender.Send(new GetTicketInfoQuery
                {
                    TransactionId = transactionId,
                    ModifiedBy = ModifiedBy
                });
                if (QueryResult.matchSeatTicketInfo != null)
                {
                    ticketdata = BuildPrintAtHome(QueryResult.matchSeatTicketInfo.ToList(), QueryResult.matchTeamInfo.ToList(), transactionId, ticketPrintingOptionId, isPrintEntityName);
                }
                else
                {
                    ticketdata = null;
                }
            }
            catch (Exception ex)
            {
                ticketdata = null;
            }
            return ticketdata;
        }


        public List<string> BuildPrintAtHome(List<MatchSeatTicketInfo> ticketDetails, List<MatchTeamInfo> teamDetails, long transactionId, TicketPrintingOption ticketPrintingOptionId, bool isPrintEntityName)
        {
            List<string> ticketList = new List<string>();
            try
            {
                if (ticketDetails != null && ticketDetails.Count > 0)
                {
                    int ticketnumber = 1;

                    foreach (var item in ticketDetails)
                    {
                        if (item.EventId.ToString() == "7111" || item.EventId.ToString() == "7101")
                        {

                            if (item.EventDeatilId.ToString() == "554401" || item.EventDeatilId.ToString() == "554439") //Ride
                            {
                                ticketList.Add(GenerateRASVPrintAtHomeRides(item, ticketnumber, ticketDetails.Count).ToString());
                            }
                            else
                            {
                                ticketList.Add(GenerateRASVPrintAtHome(item, ticketnumber, ticketDetails.Count).ToString());
                            }
                        }
                        if (item.EventId.ToString() == "7169" || item.EventId.ToString() == "7174")
                        {
                            ticketList.Add(GeneratePrintAtHomeIndiavsWendies(item, ticketnumber, ticketDetails.Count, transactionId).ToString());

                        }
                        if (item.EventId.ToString() == "8277" || item.EventId.ToString() == "8060 ")
                        {
                            ticketList.Add(GeneratePrintAtHomeAustraliaWomenSeries(item, ticketnumber, ticketDetails.Count, transactionId).ToString());

                        }
                        if (item.EventId.ToString() == "8719" || item.EventId.ToString() == "8732")
                        {
                            ticketList.Add(GeneratePrintAtHomeSuper50Series(item, ticketnumber, ticketDetails.Count, transactionId).ToString());
                        }
                        if (item.EventId.ToString() == "8778" || item.EventId.ToString() == "8826" || item.EventId.ToString() == "9429")
                        {
                            ticketList.Add(GeneratePrintAtHomeIndiaWindiesWomen(item, ticketnumber, ticketDetails.Count, transactionId, ticketPrintingOptionId,isPrintEntityName).ToString());
                        }
                        if (item.EventId.ToString() == "14560")
                        {
                            ticketList.Add(GeneratePrintAtHomeIreLandWindies(item, ticketnumber, ticketDetails.Count, transactionId).ToString());
                        }
                        if (item.EventId.ToString() == "14910" )
                        {
                            ticketList.Add(GeneratePrintAtHomeTOM(item, ticketnumber, ticketDetails.Count, transactionId).ToString());
                        }
                        ticketnumber++;
                    }
                    return ticketList;
                }
                else
                {
                    return ticketList;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return ticketList;
            }
        }

        private StringBuilder GenerateRASVPrintAtHomeRides(MatchSeatTicketInfo item, int TicketNumber, int TotalTic)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                _amazonS3FileProvider.UploadQrCodeImage(item.BarcodeNumber, ImageType.QrCode);
                string qrcodeImage = _amazonS3FileProvider.GetImagePath(item.BarcodeNumber.ToString(), ImageType.QrCode);
                string shortMatchDay = item.EventStartTime.ToString("dddd").Substring(0, 3);
                string longMatchDay = item.EventStartTime.ToString("dddd");
                string matchDate = item.EventStartTime.ToString("MMMM dd, yyyy");
                string numMonth = item.EventStartTime.ToString("dd");
                string numDay = item.EventStartTime.ToString("MMMM").Substring(0, 3);
                string shortMatchDate = numDay + ' ' + numMonth;
                string matchtime = item.EventStartTime.ToString("HH:mm");
                string IsWheelChair = item.IsWheelChair.ToString();
                string IsCompanion = item.IsCompanion.ToString();
                StringBuilder sbTicket = new StringBuilder();
                string matchAdditionalInfo = item.MatchAdditionalInfo;
                string ticketType = string.Empty;
                string PriceWithType = string.Empty;

                if (item.TicketTypeId == 2)
                {
                    ticketType = "Child";
                    PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                }
                else if (item.TicketTypeId == 3)
                {
                    ticketType = "Senior Citizen";
                    PriceWithType = "Complimentary";
                }
                else
                {
                    if (Convert.ToInt32(IsCompanion) > 1)
                    {
                        ticketType = "Companion";
                        PriceWithType = "" + item.CurrencyName + " 0.00";
                    }
                    else if (Convert.ToInt32(IsWheelChair) > 1)
                    {
                        ticketType = "Wheelchair";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                    else
                    {
                        ticketType = "Adult";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                }
                sb.Append("<!DOCTYPE html><html lang='en'><head><title>Print At Home</title><meta charset='utf-8'></head><body>");
                sb.Append("<div style='height: 1037px'>");
                sb.Append("<table align='center' width=\"750px\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
                sb.Append(" <tr>");
                sb.Append(" <td style=\"width:187px; background:url(https://static2.zoonga.com/Images/rasv/eticket/voucher-bg-white-big.jpg) repeat-x; height:292px; vertical-align:top; text-align:center; padding:0 5px;\">");
                sb.Append(" <img src=\"https://static2.zoonga.com/Images/rasv/eticket/rasv-logo.png\" alt=\"RASV Logo\" style=\"width:90px; height:70px;\" />");
                sb.Append(" ");
                sb.Append(" <p style=\"margin:0; margin-top:24px;\">");
                sb.Append(" <img src=" + qrcodeImage + " alt=\"QR Code\" style=\"width:80px; height:80px;\" /> ");
                sb.Append(" </p>");
                sb.Append(" <span style=\"font-family:Helvetica, sans-serif; font-size:11px; font-weight:normalbold; border:1px solid #000000; margin:0; width:130px; display:inline-block; padding:2px 0; margin-top: 6px;\">");
                sb.Append(" " + item.BarcodeNumber + "");
                sb.Append(" </span>");
                sb.Append(" <p style=\"font-family:Helvetica, sans-serif; font-size:9px; font-weight:normal; margin:0; margin-top:14px;\">");
                sb.Append(" Royal Melbourne Show ride vouchers <br />");
                sb.Append(" should only be purchased through <br />");
                sb.Append(" royalshow.com.au or zoonga.com.au <br />  to ensure authenticity");
                sb.Append(" </p>");
                sb.Append(" ");
                sb.Append(" </td>");
                sb.Append(" <td style=\"width:596px; background:url(https://static2.zoonga.com/Images/rasv/eticket/voucher-bg-gray-big.jpg) repeat-x; vertical-align:top; text-align:center;\">");
                sb.Append(" <table width=\"458px\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" align=\"center\">");
                sb.Append(" <tr>");
                sb.Append(" <td style=\"height:30px;\"></td>");
                sb.Append(" </tr>");
                sb.Append(" <tr>");
                sb.Append(" <td style=\"font-family:Helvetica, sans-serif; font-size:14px; font-weight:bold; border:2px solid #9669ad; background:#ffffff; padding:4px 0; line-height:1.5;\">");

                if (item.TicketCategoryId.ToString() == "11708" || item.TicketCategoryId.ToString() == "11689")
                {
                    sb.Append("$35 Ride Voucher<br /> ");
                }
                else if (item.TicketCategoryId.ToString() == "11709" || item.TicketCategoryId.ToString() == "11688")
                {
                    sb.Append("$55 Ride Voucher<br /> ");
                }
                else if (item.TicketCategoryId.ToString() == "11710" || item.TicketCategoryId.ToString() == "11687")
                {
                    sb.Append("$70 Ride Voucher<br /> ");
                }
                else if (item.TicketCategoryId.ToString() == "11711" || item.TicketCategoryId.ToString() == "11686")
                {
                    sb.Append("$105 Ride Voucher<br /> ");
                }
                else if (item.TicketCategoryId.ToString() == "11712" || item.TicketCategoryId.ToString() == "11685")
                {
                    sb.Append("$140 Ride Voucher<br /> ");
                }
                else if (item.TicketCategoryId.ToString() == "11707" || item.TicketCategoryId.ToString() == "11684")
                {
                    sb.Append("$200 Ride Voucher<br /> ");
                }
                else
                {
                    sb.Append("" + item.TicketCategoryName + "<br /> ");
                }

                sb.Append("Sat 21 Sept 2019, 9.30am – Tue 01 Oct 2019, 9.00pm");

                sb.Append(" </td>");
                sb.Append(" </tr>");
                sb.Append(" <tr>");
                sb.Append(" <td style=\"font-family:Helvetica, sans-serif; font-size:22px; font-weight:bold; line-height:1.2; text-transform:uppercase; color:#9669ad; height:80px;\">");
                sb.Append(" How to Redeem <br /> ");
                sb.Append(" This Voucher");
                sb.Append(" </td>");
                sb.Append(" </tr>");
                sb.Append(" <tr>");
                sb.Append(" <td style=\"font-family:Helvetica, sans-serif; font-size:11px; font-weight:bold;\">");
                sb.Append(" Exchange this voucher for your rides pass at any information <br />");
                sb.Append(" booth at the 2019 Royal Melbourne Show.");
                sb.Append(" </td>");
                sb.Append(" </tr>");
                sb.Append(" <tr>");
                sb.Append(" <td style=\"font-family:Helvetica, sans-serif; font-size:8px; font-weight:normal; height:24px; border-bottom:1px solid #9669ad;\">");
                sb.Append(" Simply present either a printed copy of the ride voucher or voucher barcode on an electronic device to receive rides pass.");
                sb.Append(" </td>");
                sb.Append(" </tr>");
                sb.Append(" <tr>");
                sb.Append(" <td style=\"font-family:Helvetica, sans-serif; font-size:8px; font-weight:normal; text-align:justify; line-height:1.3; padding:4px 0;\">");
                sb.Append(" Terms and Conditions: Ride vouchers and ride passes are not redeemable for cash, cannot be resold, no change can be given for unused credit and cannot be replaced if lost or stolen. Ride vouchers and ride passes are valid Saturday 21 September  Tuesday 1 October 2019 and can be used on any day at the Royal Melbourne Show. This offer is not available in conjunction with any other advertised or promotional ride offer. Each voucher has a unique barcode which can be scanned for one use only.");
                sb.Append(" </td>");
                sb.Append(" </tr>");
                sb.Append(" </table>");
                sb.Append(" </td>");
                sb.Append(" </tr>");
                sb.Append("</table>");

                sb.Append("<div style='margin-top:10px; margin-bottom:20px; text-align:center'>");
                sb.Append("<p style='font-family:Helvetica, sans-serif; font-size:14px; color:#565656;line-height:16px;'>&nbsp;</p>");
                sb.Append("<img src='https://static2.zoonga.com/Images/rasv/eticket/zoonga-logo.png' alt='Zoonga Logo' style='width:120px; height:54px;' />");
                sb.Append("<p style='margin:10px 0;'><a href='https://www.zoonga.com.au/' style='font-family:Helvetica, sans-serif; font-size:12px; color:#157ad0;text-decoration:none;'>zoonga.com.au</a></p>");
                sb.Append("<p style='font-family:Helvetica, sans-serif; font-size:12px; color:#565656; margin:0;'>Proudly the Official Ticketing Partner of the Royal Melbourne Show.</p>");
                sb.Append("</div>");


                sb.Append("</div>");
                sb.Append("</body></html>");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return sb;
        }

        private StringBuilder GenerateRASVPrintAtHome(MatchSeatTicketInfo item, int TicketNumber, int TotalTic)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                _amazonS3FileProvider.UploadQrCodeImage(item.BarcodeNumber, ImageType.QrCode);
                string qrcodeImage = _amazonS3FileProvider.GetImagePath(item.BarcodeNumber.ToString(), ImageType.QrCode);
                string shortMatchDay = item.EventStartTime.ToString("dddd").Substring(0, 3);
                string longMatchDay = item.EventStartTime.ToString("dddd");
                string matchDate = item.EventStartTime.ToString("MMMM dd, yyyy");
                string numMonth = item.EventStartTime.ToString("dd");
                string numDay = item.EventStartTime.ToString("MMMM").Substring(0, 3);
                string shortMatchDate = numDay + ' ' + numMonth;
                string matchtime = item.EventStartTime.ToString("HH:mm");
                string IsWheelChair = item.IsWheelChair.ToString();
                string IsCompanion = item.IsCompanion.ToString();
                StringBuilder sbTicket = new StringBuilder();
                string matchAdditionalInfo = item.MatchAdditionalInfo;
                string ticketType = string.Empty;
                string PriceWithType = string.Empty;


                if (item.TicketTypeId == 2)
                {
                    ticketType = "Child";
                    PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                }
                else if (item.TicketTypeId == 3)
                {
                    ticketType = "Senior Citizen";
                    PriceWithType = "Complimentary";
                }
                else
                {
                    if (Convert.ToInt32(IsCompanion) > 1)
                    {
                        ticketType = "Companion";
                        PriceWithType = "" + item.CurrencyName + " 0.00";
                    }
                    else if (Convert.ToInt32(IsWheelChair) > 1)
                    {
                        ticketType = "Wheelchair";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                    else
                    {
                        ticketType = "Adult";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                }
                string TicketNotes = "";
                sb.Append("<!DOCTYPE html><html lang='en'><head><title>Print At Home</title><meta charset='utf-8'></head><body>");
                sb.Append("<div style='height: 1037px'>");
                sb.Append("<table align='center' width=\"743px\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
                sb.Append("<tr>");
                sb.Append("<td style=\"vertical-align:top; width:170px;\"></td>");
                sb.Append("<td style=\"vertical-align:top; width:13px;\"></td>");
                sb.Append("<td style=\"vertical-align:top; width:560px; font-family:Helvetica, sans-serif; font-weight:normal; font-size:13px; text-transform:uppercase; text-align:center; padding-bottom:2px;\">");
                sb.Append("Venue:");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"vertical-align:top;\">");
                sb.Append("<table width=\"170px\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
                sb.Append("<tr>");
                sb.Append("<td><img src=\"https://static2.zoonga.com/Images/rasv/eticket/rasv-logo.png\" alt=\"RASV Logo\" style=\"width:170px; height:133px;\" /></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"font-family:Helvetica, sans-serif; font-weight:normal; font-size:12px; font-weight:bold; text-align:center; padding:7px 0 6px 0; text-transform:uppercase;\">");
                sb.Append("Name");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append("<p style=\"font-family:Helvetica, sans-serif; font-weight:normal; font-size:12px; font-weight:bold; border:1px solid #000000; margin:0; padding:6px 0; text-align:center;\">");
                sb.Append("" + item.SponsorOrCustomerName + "");
                sb.Append("</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"font-family:Helvetica, sans-serif; font-weight:normal; font-size:12px; font-weight:bold; text-align:center; padding:6px 0; text-transform:uppercase;\">");
                sb.Append("Order Ref:");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append("<p style=\"font-family:Helvetica, sans-serif; font-weight:normal; font-size:12px; font-weight:bold; border:1px solid #000000; margin:0; padding:6px 0; text-align:center;\">");
                sb.Append("" + item.BarcodeNumber + "");
                sb.Append("</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("<td style=\"vertical-align:top;\"></td>");
                sb.Append("<td style=\"vertical-align:top;\">");
                sb.Append("<div style=\"border:1px solid #000000\">");
                sb.Append("<table width=\"558px\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
                sb.Append("<tr>");
                sb.Append("<td style=\"font-family:Helvetica, sans-serif; font-size:20px; font-weight:bold; border-bottom:1px solid #000000; text-align:center; padding:10px 0;\">");
                sb.Append("Melbourne Showgrounds <br />Epsom Road, Ascot Vale");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"font-family:Helvetica, sans-serif; font-size:20px; font-weight:bold; border-bottom:1px solid #000000; text-align:center; padding:8px 0 9px 0;\">");

                if (item.EventDeatilId.ToString() == "554440" || item.EventDeatilId.ToString() == "554402")
                {
                    sb.Append("General Admission");
                }
                else if (item.EventDeatilId.ToString() == "554441" || item.EventDeatilId.ToString() == "554403")
                {
                    sb.Append("RACV Member");
                }
                else if (item.EventDeatilId.ToString() == "554442" || item.EventDeatilId.ToString() == "554404")
                {
                    sb.Append("After 5pm Super Saver");
                }
                else if (item.EventDeatilId.ToString() == "554443" || item.EventDeatilId.ToString() == "554405")
                {
                    sb.Append("Group Offer");
                }


                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"font-family:Helvetica, sans-serif; font-size:16px; font-weight:bold; border-bottom:1px solid #000000; text-align:center; padding:10px 0;\">");
                if (item.EventDeatilId.ToString() == "554440" || item.EventDeatilId.ToString() == "554402")
                {
                    sb.Append("Sat 21 Sept 2019, 9.30am – Tue 01 Oct 2019, 9.00pm, *No entry after 8.30pm");
                }
                else
                {
                    sb.Append("Sat 21 Sept 2019, 9.30am – Tue 01 Oct 2019, 9.00pm");
                }
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"font-family:Helvetica, sans-serif; font-size:18px; font-weight:bold; text-align:center; padding:17px 0;\">");



                if (item.EventDeatilId.ToString() == "554440" || item.EventDeatilId.ToString() == "554402")
                {
                    if (item.TicketCategoryId.ToString() == "1334")
                    {
                        sb.Append("Adult<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1335")
                    {
                        sb.Append("Child<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "Valid for children 5 - 14 years of age";
                    }
                    else if (item.TicketCategoryId.ToString() == "1336")
                    {
                        sb.Append("Concession<br /> Standard Ticket <br />" + PriceWithType + "");
                        TicketNotes = "Must present valid concession card on entry";
                    }
                    else if (item.TicketCategoryId.ToString() == "1337")
                    {
                        sb.Append("Family A (2 Adult, 2 Child)<br /> " + ticketType + " Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1338")
                    {
                        sb.Append("Family B (1 Adult, 3 Child)<br /> " + ticketType + "  Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1339")
                    {
                        sb.Append("2 Day VIP Pass<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "This ticket is valid for any two days of Show";
                    }
                    else if (item.TicketCategoryId.ToString() == "11706" || item.TicketCategoryId.ToString() == "11681")
                    {
                        sb.Append("Child<br />Under 5 years<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                }
                else if (item.EventDeatilId.ToString() == "554441" || item.EventDeatilId.ToString() == "554403")
                {
                    if (item.TicketCategoryId.ToString() == "1334")
                    {
                        sb.Append("Adult<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1335")
                    {
                        sb.Append("Child<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "Valid for children 5 - 14 years of age";
                    }
                    else if (item.TicketCategoryId.ToString() == "1336")
                    {
                        sb.Append("Online Super Saver - Concession<br />&nbsp;<br />" + PriceWithType + "");
                        TicketNotes = "Must present valid concession card on entry";
                    }
                    else if (item.TicketCategoryId.ToString() == "1337")
                    {
                        sb.Append("Family A (2 Adult, 2 Child)<br /> " + ticketType + " Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1338")
                    {
                        sb.Append("Family B (1 Adult, 3 Child)<br />" + ticketType + " Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1339")
                    {
                        sb.Append("2 Day VIP Pass<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "This ticket is valid for any two days of Show";
                    }

                }
                else if (item.EventDeatilId.ToString() == "554442" || item.EventDeatilId.ToString() == "554404")
                {
                    if (item.TicketCategoryId.ToString() == "1334")
                    {
                        sb.Append("Adult<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1335")
                    {
                        sb.Append("Child<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "Valid for children 5 - 14 years of age";
                    }
                    else if (item.TicketCategoryId.ToString() == "1336")
                    {
                        sb.Append("Online Super Saver - Concession<br />&nbsp;<br />" + PriceWithType + "");
                        TicketNotes = "Must present valid concession card on entry";
                    }
                    else if (item.TicketCategoryId.ToString() == "1337")
                    {
                        sb.Append("Family A (2 Adult, 2 Child)<br />" + ticketType + " Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1338")
                    {
                        sb.Append("Family B (1 Adult, 3 Child)<br />" + ticketType + " Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1339")
                    {
                        sb.Append("2 Day VIP Pass<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "This ticket is valid for any two days of Show";
                    }

                }
                else if (item.EventDeatilId.ToString() == "554443" || item.EventDeatilId.ToString() == "554405")
                {
                    if (item.TicketCategoryId.ToString() == "1334")
                    {
                        sb.Append("Adult<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "1335")
                    {
                        sb.Append("Child<br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "Valid for children 5 - 14 years of age";
                    }
                    else if (item.TicketCategoryId.ToString() == "1336")
                    {
                        sb.Append("Online Super Saver - Concession<br />&nbsp;<br />" + PriceWithType + "");
                        TicketNotes = "Must present valid concession card on entry";
                    }
                    else if (item.TicketCategoryId.ToString() == "1063" || item.TicketCategoryId.ToString() == "11690")
                    {
                        sb.Append("Family (2 Adult, 2 Child)<br />" + ticketType + " Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                    else if (item.TicketCategoryId.ToString() == "11713" || item.TicketCategoryId.ToString() == "11682")
                    {
                        sb.Append("Dog Exhibitor <br />Standard Ticket<br />" + PriceWithType + "");
                        TicketNotes = "&nbsp;";
                    }
                }

                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"height:14px;\"></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"vertical-align:top; text-align:center;\">");
                sb.Append("<div style=\"font-family:Helvetica, sans-serif; font-size:12px; font-weight:bold; border:1px solid #000000; padding:4px; height:108px;\">");
                sb.Append("<span style=\"font-family:Helvetica, sans-serif; font-size:12px; font-weight:bold; text-transform:uppercase\">Notes:</span>");
                sb.Append("<br /><br />");
                sb.Append("" + TicketNotes + "");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("<td style=\"vertical-align:top; width:10px;\"></td>");
                sb.Append("<td style=\"vertical-align:top;\">");
                sb.Append("<div style=\"border:1px solid #000000; padding: 8px 10px;\">");
                sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
                sb.Append("<tr>");
                sb.Append("<td><img src=" + qrcodeImage + " alt=\"QR Code\" style=\"width:95px; height:95px;\" /></td>");
                sb.Append("<td style=\"padding-left:10px;\">");
                sb.Append("<p style=\"font-family:Helvetica, sans-serif; font-size:8px; font-weight:normal; margin-top:0; margin-bottom:5px;\">Each person attending the Royal Melbourne Show is required to present either a printed copy of their individual E-Ticket or ticket barcode on an electronic mobile device to gain entry. Each ticket has a unique barcode which can be scanned for one entry only. If there are multiple copies of a ticket, only the first ticket will be admitted. Thereafter, any patrons attempting entry with additional copies will be denied entry. Be cautious if someone tries to sell you this document. It has no official resale value and may not be authentic.</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"height:14px;\"></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"text-align:center\" colspan=\"3\">");
                sb.Append("<div style='margin-top:10px; margin-bottom:15px;'>");
                sb.Append("<img src='https://static2.zoonga.com/Images/rasv/eticket/zoonga-logo.png' alt='Zoonga Logo' style='width:120px; height:50px;' />");
                sb.Append("<p style='margin:5px 0;'><a href='https://www.zoonga.com.au/' style='font-family:Helvetica, sans-serif; font-size:12px; color:#157ad0;text-align:center; text-decoration:none;'>zoonga.com.au</a></p>");
                sb.Append("<p style='font-family:Helvetica, sans-serif; font-size:12px; color:#565656;text-align:center; margin:0;'>Proudly the Official Ticketing Partner of the Royal Melbourne Show.</p>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"text-align:center; width:743px;\" colspan=\"3\">");
                sb.Append("<img src=\"https://static2.zoonga.com/Images/rasv/eticket/rms-banner-new2.jpg\" alt=\"RMS banner\" style=\"width:743px;\" />");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</body></html>");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return sb;
        }

        private StringBuilder GeneratePrintAtHomeIndiavsWendies(MatchSeatTicketInfo item, int TicketNumber, int TotalTic, long transactionId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                _amazonS3FileProvider.UploadQrCodeImage(item.BarcodeNumber, ImageType.QrCode);
                string qrcodeImage = _amazonS3FileProvider.GetImagePath(item.BarcodeNumber.ToString(), ImageType.QrCode);
                string shortMatchDay = item.EventStartTime.ToString("dddd").Substring(0, 3);
                string longMatchDay = item.EventStartTime.ToString("dddd");
                string matchDate = item.EventStartTime.ToString("MMMM dd, yyyy");
                string numMonth = item.EventStartTime.ToString("dd");
                string numDay = item.EventStartTime.ToString("MMMM").Substring(0, 3);
                string shortMatchDate = numDay + ' ' + numMonth;
                string matchtime = item.EventStartTime.ToString("HH:mm");
                string IsWheelChair = item.IsWheelChair.ToString();
                string IsCompanion = item.IsCompanion.ToString();
                StringBuilder sbTicket = new StringBuilder();
                string matchAdditionalInfo = item.MatchAdditionalInfo;
                string ticketType = string.Empty;
                string PriceWithType = string.Empty;


                if (item.TicketTypeId == 2)
                {
                    ticketType = "Child";
                    PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                }
                else if (item.TicketTypeId == 3)
                {
                    ticketType = "Senior Citizen";
                    PriceWithType = "Complimentary";
                }
                else
                {
                    if (Convert.ToInt32(IsCompanion) > 1)
                    {
                        ticketType = "Companion";
                        PriceWithType = "" + item.CurrencyName + " 0.00";
                    }
                    else if (Convert.ToInt32(IsWheelChair) > 1)
                    {
                        ticketType = "Wheelchair";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                    else
                    {
                        ticketType = "Adult";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                }

                //string IsLayoutAvailable = dr["IsLayoutAvail"].ToString();

                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; border:1px solid #868686 ; background:url(https://static1.zoonga.com/Images/eTickets/arena-bg.png) no-repeat top left;padding:10px'>");
                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                //sb.Append("<td style='vertical-align:top; background:url(images/etkt-tkt-bg-1.gi) no-repeat bottom; padding:10px'>");
                sb.Append("<td style ='vertical-align:top;'>");
                sb.Append("<table width='718px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td>");
                sb.Append("<div style='width:350px; float:left;'>");
                sb.Append("<img src='http://boxoffice.kyazoonga.com/tickethtml/images/etkt-z-logo.png' style='width:214px; height:77px'>");
                sb.Append("</div>");
                sb.Append("<div style='width:350px; float:right; text-align:right; font-family:arial; font-size:44px; font-weight:bold; text-decoration:none; margin-top:30px;'><i>e</i>-ticket</div> ");
                sb.Append("");
                sb.Append("<div style='clear:both'></div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='height:4px; border-bottom:2px solid #dcdcdc'></td>");
                //sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:10px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top'>");
                sb.Append("<td style='border:1px solid #868686; background:url(https://static1.zoonga.com/Images/eTickets/wi-ind-bg.jpg) no-repeat; height:267px;'> ");
                sb.Append("<table width='650px' align='center' cellspacing='0' cellpadding='0' border='0' style='margin-top:3px;'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("");
                sb.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                sb.Append("<tbody>");
                sb.Append("<tr><td style='height:10px'></td></tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:17px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>");
                sb.Append("MyTeam11.com West Indies vs India <span style='font-family:arial; font-size:13px; font-weight:bold;'><br>co-powered by Skoda</span>");
                sb.Append("</td>");
                sb.Append("<td rowspan='5' style='text-align:right'>");
                sb.Append("<table cellpadding='0' width='100%' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='text-align:right; font-family:arial; font-weight:bold '>");
                sb.Append("" + PriceWithType + "<br>");
                sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                //sb.Append("<p style='margin:0; margin-top:2px; padding:0'>" + ticketType + "</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.EventDetailsName + " </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                //sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                //sb.Append("<td>&nbsp;</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr style='height:4px'>");
                //sb.Append("<td></td>");
                //sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td>" + shortMatchDay + " | " + matchDate + " | " + matchtime + " <span style='font-family:arial; font-weight:normal;'> (Local)</span></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.VenueName + ", " + item.CityName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td colspan='2'>");
                sb.Append("");
                sb.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Enter through: &nbsp;<strong>" + item.GateName + " Gate</strong> </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Gates Open At <strong>" + item.GateOpenTime + "</strong> (Local)</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");

                StringBuilder sbMatchDay = new StringBuilder();

                if (item.EventDeatilId.ToString() == "554529" || item.EventDeatilId.ToString() == "554531" || item.EventDeatilId.ToString() == "554532")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("ODI <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else if (item.EventDeatilId.ToString() == "554513" || item.EventDeatilId.ToString() == "554514" || item.EventDeatilId.ToString() == "554515")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:5px; font-weight:bold'>");
                    sbMatchDay.Append("T20I <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("Day <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchDay + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }

                if (item.VenueId.ToString() == "58" || item.VenueId.ToString() == "62")
                {
                    if (item.IsSeatSelection)
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td>Row: &nbsp;<strong>" + item.RowNumber + " </strong> &nbsp;&nbsp;Seat:  &nbsp;<strong>" + item.TicketNumber + " </strong></td>");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                    else
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                }
                else
                {
                    sb.Append("<tr style='vertical-align:top'>");
                    sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                    sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                    //sb.Append(sbMatchDay.ToString());
                    sb.Append("</tr>");
                    if (item.TicketTypeId == 2)
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                        sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                        sb.Append("</td></tr>");
                    }
                }

                sb.Append("<!--<tr style='height:15px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:normal;'>");
                sb.Append("<td> Only sponsor's products will be permitted into the stadium.<br>");
                sb.Append("See www.cplt20.com/T&amp;Cs. Ambush marketing not permitted</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<!--<tr style='height:3px'>");
                sb.Append("</tr> ");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td> CPL Partners: Hero * Digicel * Guardian * El Dorado</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial; font-weight:bold; line-height:1.4'>This is Your Ticket</span><br>");
                sb.Append("Keep this page with you at all times<br>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial;font-weight:bold; line-height:1.4'>Do Not Duplicate</span>");
                sb.Append("</td>");
                sb.Append("<td style='width:28%; text-align:center'>");
                sb.Append("<img style='width:80px; height:80px' src='" + qrcodeImage + "'> ");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial;'>");
                sb.Append("" + item.TicketCategoryName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br>");
                sb.Append("Ticket " + TicketNumber + "/" + TotalTic + "<br> ");
                //sb.Append("Regenerated Ticket 1");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border:1px solid #868686; font-family:arial; font-size:11px; text-decoration:none; padding:8px;line-height:1.1; background:#ffffff;text-align:justify;'>");
                sb.Append("<span style='font-size:13px; font-weight:bold; text-transform:uppercase;'>THIS IS YOUR TICKET. DO NOT DUPLICATE. ONE ENTRY PER BARCODE</span>&nbsp; 1. This Ticket is sold subject to the Terms and Conditions set out below and those on display at CWI ticket");
                sb.Append("offices. Any person who makes use of, purchases or holds this Ticket ('the Holder') shall be deemed to have");
                sb.Append("agreed to all of the Terms and Conditions to which this Ticket is subject. The Holder, in agreeing to these");
                sb.Append("conditions, also agrees to be bound by the CWI Refund Policy. ");
                sb.Append("2. Right of admission is reserved at the");
                sb.Append("reasonable discretion of CWI. CWI reserves the right to search the Holder. Should the Holder refuse to be");
                sb.Append("searched, CWI reserves the right to refuse admission or eject him/her from the Stadium without refund. ");
                sb.Append("3.");
                sb.Append("Ambush marketing is prohibited. Ambush marketing includes (a) the unauthorised use (by the Holder or any");
                sb.Append("other person) of the Ticket as a prize or in a lottery or competition or for any other promotional, advertising or");
                sb.Append("commercial purpose; and (b) an intentional unauthorised activity which (i) associates a person with the");
                sb.Append("Series, (ii) exploits the publicity or goodwill of the Series or (iii) has the effect of diminishing the status of");
                sb.Append("Series sponsors or conferring on other persons the status of a Series sponsor. ");
                sb.Append("4. The Holder shall not (a)");
                sb.Append("dispose of or transfer the Ticket for the purpose of commercial gain nor sell the Ticket at a higher price than");
                sb.Append("its face value; or (b) purchase or obtain the Ticket from a person/entity who is not an authorised agent. Any");
                sb.Append("Ticket obtained in breach hereof shall be void and the Holder shall have no right of entry to the Stadium. ");
                sb.Append("5.");
                sb.Append("No alcohol may be brought into or taken out of the Stadium. The Holder shall only be entitled to bring in 1");
                sb.Append("bag / carrier / rucksack which must be no larger than 400mm by 300mm by 300mm (15.7' by 11.8' by 11.8')");
                sb.Append("and must be able to fit under the seat. ");
                sb.Append("6. The Holder shall not seek entry to, or view the Match from stands");
                sb.Append("or seats for which the Holder does not hold a valid Ticket. ");
                sb.Append("7. CWI reserves the right to refuse admission or");
                sb.Append("eject from the Stadium without refund any Holder who, in the reasonable opinion of CWI is in breach of");
                sb.Append("these conditions. ");
                sb.Append("8. The Holder shall not, except for non-commercial purposes, make, use, broadcast,");
                sb.Append("transmit, publish, disseminate, reproduce or circulate by any means any recordings, audio, video, data,");
                sb.Append("images, results, commentary or any other information relating to the Match. ");
                sb.Append("9. CWI shall not be liable for");
                sb.Append("the theft, loss or damage of any property of the Holder. ");
                sb.Append("10. Management reserves the right to make alteration to the times, dates and venues of Events or to substitute the seat or area to which the ticket refers with another at its reasonable discretion. Management does not guarantee that the holder will have an uninterrupted and/or uninhibited view of the Event from the seat provided. ");
                sb.Append("11. Any comments or queries should be made by writing to us at <span style='text-decoration:underline'>customerservice@zoonga.com</span> ");
                sb.Append("12. Please refer to the restricted items list and full terms and conditions on zoonga.com");
                sb.Append("<br><br>");
                sb.Append("<b>CWI REFUND POLICY </b> 1. If a day's play is limited as a result of rain or other act beyond the control of the CWI, the CWI will refund the face value of tickets purchased as follows (i) Less than 10 overs bowled in any one day (ie up to and including 9 overs, 5 balls): 100% refund (ii) In excess of 10 overs but less than 20 overs bowled in any one day (i.e. 10 overs or more up to 19 overs, 5 balls): 50% refund (iii) In excess of twenty overs bowled in any one day (i.e. 20 overs or more): no refund. 2 For Twenty20 International matches: (i) 5 overs or less bowled: 100% refund (ii) in excess of 5 overs but less than 10 overs bowled: 50% refund (iii) in excess of 10 overs bowled (i.e. 10 overs or more): no refund.");
                sb.Append(". ");
                sb.Append("2. The above does not apply: (i) to a match that is completed early in the normal course of play; or (ii) where");
                sb.Append("a reserve day for play has been allocated or a match is rescheduled and this ticket is valid for such reserve");
                sb.Append("day or rescheduled game. ");
                sb.Append("3. To receive a refund, the retained portion of the ticket stub must be presented");
                sb.Append("and surrendered to a ticket office authorised by the CWI within ten (10) working days after the completion of");
                sb.Append("the game for which the refund is being sought. Refunds will be made only for tickets purchased from CWI");
                sb.Append("ticket offices. Under no other circumstances will refunds be given. ");
                sb.Append("4. Refunds shall only be made to the");
                sb.Append("person whose name appears on the ticket. ");
                sb.Append("5. Party Stand Refunds Only the Cricket Component of the");
                sb.Append("Party Stand ticket will be refunded in accordance with the CWI Refund Policy. Specific information on this");
                sb.Append("is available upon request through the local Ticket Office.");
                sb.Append("<!-- 13. <span style='font-weight:bold'>NO REFUND, NO EXCHANGE, GAMES DATE AND TIME SUBJECT TO CHANGE WITHOUT NOTICE</span>.-->");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>&nbsp;</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='text-align:center'>");
                sb.Append("<td style='border-top:1px dashed #000000;'>");
                sb.Append("<p style='text-align:center; margin:0; font-size:18px; position:relative; top:-11px;'>✂</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:31%; font-size:12px; font-family:arial'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial'>");
                sb.Append("" + item.SponsorOrCustomerName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br><br>");
                sb.Append("" + item.BarcodeNumber + "");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td style='width:38%; font-size:12px;' align='center'; font-family:arial>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>");
                sb.Append("" + shortMatchDay + " | " + shortMatchDate + " | " + matchtime + " (Local)<br>");
                sb.Append("" + item.VenueName + ", " + item.CityName + "</span><br><br>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>Ticket " + TicketNumber + "/" + TotalTic + " &nbsp;&nbsp;</span>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:31%; font-size:12px;'>");
                sb.Append("<img style='width:80px; height:80px' src=" + qrcodeImage + ">");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</body></html>");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return sb;
        }
        private StringBuilder GeneratePrintAtHomeAustraliaWomenSeries(MatchSeatTicketInfo item, int TicketNumber, int TotalTic, long transactionId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                _amazonS3FileProvider.UploadQrCodeImage(item.BarcodeNumber, ImageType.QrCode);
                string qrcodeImage = _amazonS3FileProvider.GetImagePath(item.BarcodeNumber.ToString(), ImageType.QrCode);
                string shortMatchDay = item.EventStartTime.ToString("dddd").Substring(0, 3);
                string longMatchDay = item.EventStartTime.ToString("dddd");
                string matchDate = item.EventStartTime.ToString("MMMM dd, yyyy");
                string numMonth = item.EventStartTime.ToString("dd");
                string numDay = item.EventStartTime.ToString("MMMM").Substring(0, 3);
                string shortMatchDate = numDay + ' ' + numMonth;
                string matchtime = item.EventStartTime.ToString("HH:mm");
                string IsWheelChair = item.IsWheelChair.ToString();
                string IsCompanion = item.IsCompanion.ToString();
                StringBuilder sbTicket = new StringBuilder();
                string matchAdditionalInfo = item.MatchAdditionalInfo;
                string ticketType = string.Empty;
                string PriceWithType = string.Empty;


                if (item.TicketTypeId == 2)
                {
                    ticketType = "Child";
                    PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                }
                else if (item.TicketTypeId == 3)
                {
                    ticketType = "Senior Citizen";
                    PriceWithType = "Complimentary";
                }
                else
                {
                    if (Convert.ToInt32(IsCompanion) > 1)
                    {
                        ticketType = "Companion";
                        PriceWithType = "" + item.CurrencyName + " 0.00";
                    }
                    else if (Convert.ToInt32(IsWheelChair) > 1)
                    {
                        ticketType = "Wheelchair";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                    else
                    {
                        ticketType = "Adult";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                }

                //string IsLayoutAvailable = dr["IsLayoutAvail"].ToString();

                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; border:1px solid #868686 ; background:url(https://static1.zoonga.com/Images/eTickets/arena-bg.png) no-repeat top left;padding:10px'>");
                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                //sb.Append("<td style='vertical-align:top; background:url(images/etkt-tkt-bg-1.gi) no-repeat bottom; padding:10px'>");
                sb.Append("<td style ='vertical-align:top;'>");
                sb.Append("<table width='718px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td>");
                sb.Append("<div style='width:350px; float:left;'>");
                sb.Append("<img src='http://boxoffice.kyazoonga.com/tickethtml/images/etkt-z-logo.png' style='width:214px; height:77px'>");
                sb.Append("</div>");
                sb.Append("<div style='width:350px; float:right; text-align:right; font-family:arial; font-size:44px; font-weight:bold; text-decoration:none; margin-top:30px;'><i>e</i>-ticket</div> ");
                sb.Append("");
                sb.Append("<div style='clear:both'></div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='height:4px; border-bottom:2px solid #dcdcdc'></td>");
                //sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:10px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top'>");
                sb.Append("<td style='border:1px solid #868686; background:url(https://static1.zoonga.com/Images/eTickets/wi-aus-bg.jpg) no-repeat; height:267px;'> ");
                sb.Append("<table width='650px' align='center' cellspacing='0' cellpadding='0' border='0' style='margin-top:3px;'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("");
                sb.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                sb.Append("<tbody>");
                sb.Append("<tr><td style='height:10px'></td></tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:17px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>");
                //sb.Append("Australia Women's Tour of Caribbean 2019 <span style='font-family:arial; font-size:13px; font-weight:bold;'><br></span>");
                if (item.VenueId.ToString() == "57")
                {
                    sb.Append("Australia Women's Tour of Caribbean 2019 <span style='font-family:arial; font-size:13px; font-weight:bold;'><br></span>");
                }
                else
                {
                    sb.Append("Colonial Medical Insurance ODI Series <span style='font-family:arial; font-size:13px; font-weight:bold;'><br></span>");
                }
                sb.Append("</td>");
                sb.Append("<td rowspan='5' style='text-align:right'>");
                sb.Append("<table cellpadding='0' width='100%' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='text-align:right; font-family:arial; font-weight:bold '>");
                sb.Append("" + PriceWithType + "<br>");
                sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                //sb.Append("<p style='margin:0; margin-top:2px; padding:0'>" + ticketType + "</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; text-decoration:none; font-weight:bold;'>");
                // sb.Append("<td>" + item.EventDetailsName + " </td>");
                if (item.EventDeatilId.ToString() == "555934" || item.EventDeatilId.ToString() == "555472")
                {
                    sb.Append("<td>West Indies Women vs Australia Women, 1st T20I </td>");
                }
                else if (item.EventDeatilId.ToString() == "555935" || item.EventDeatilId.ToString() == "555473")
                {
                    sb.Append("<td>West Indies Women vs Australia Women, 2nd T20I </td>");
                }
                else if (item.EventDeatilId.ToString() == "555936" || item.EventDeatilId.ToString() == "555474")
                {
                    sb.Append("<td>West Indies Women vs Australia Women, 3rd T20I </td>");
                }
                else if (item.EventDeatilId.ToString() == "555931" || item.EventDeatilId.ToString() == "555469")
                {
                    sb.Append("<td>West Indies Women vs Australia Women, 1st ODI </td>");
                }
                else if (item.EventDeatilId.ToString() == "555932" || item.EventDeatilId.ToString() == "555470")
                {
                    sb.Append("<td>West Indies Women vs Australia Women, 2nd ODI </td>");
                }
                else if (item.EventDeatilId.ToString() == "555933" || item.EventDeatilId.ToString() == "555471")
                {
                    sb.Append("<td>West Indies Women vs Australia Women, 3rd ODI </td>");
                }
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                //sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                //sb.Append("<td>&nbsp;</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr style='height:4px'>");
                //sb.Append("<td></td>");
                //sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td>" + shortMatchDay + " | " + matchDate + " | " + matchtime + " <span style='font-family:arial; font-weight:normal;'> (Local)</span></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.VenueName + ", " + item.CityName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td colspan='2'>");
                sb.Append("");
                sb.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Enter through: &nbsp;<strong>" + item.GateName + " Gate</strong> </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Gates Open At <strong>" + item.GateOpenTime + "</strong> (Local)</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");

                StringBuilder sbMatchDay = new StringBuilder();

                if (item.EventDeatilId.ToString() == "554529" || item.EventDeatilId.ToString() == "554531" || item.EventDeatilId.ToString() == "554532")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("ODI <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else if (item.EventDeatilId.ToString() == "554513" || item.EventDeatilId.ToString() == "554514" || item.EventDeatilId.ToString() == "554515")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:5px; font-weight:bold'>");
                    sbMatchDay.Append("T20I <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("Day <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchDay + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }

                if (item.VenueId.ToString() == "11691" || item.VenueId.ToString() == "9697")
                {
                    if (item.IsSeatSelection)
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td>Row: &nbsp;<strong>" + item.RowNumber + " </strong> &nbsp;&nbsp;Seat:  &nbsp;<strong>" + item.TicketNumber + " </strong></td>");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                    else
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                }
                else
                {
                    sb.Append("<tr style='vertical-align:top'>");
                    sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                    sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                    //sb.Append(sbMatchDay.ToString());
                    sb.Append("</tr>");
                    if (item.TicketTypeId == 2)
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                        sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                        sb.Append("</td></tr>");
                    }
                }

                sb.Append("<!--<tr style='height:15px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:normal;'>");
                sb.Append("<td> Only sponsor's products will be permitted into the stadium.<br>");
                sb.Append("See www.cplt20.com/T&amp;Cs. Ambush marketing not permitted</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<!--<tr style='height:3px'>");
                sb.Append("</tr> ");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td> CPL Partners: Hero * Digicel * Guardian * El Dorado</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial; font-weight:bold; line-height:1.4'>This is Your Ticket</span><br>");
                sb.Append("Keep this page with you at all times<br>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial;font-weight:bold; line-height:1.4'>Do Not Duplicate</span>");
                sb.Append("</td>");
                sb.Append("<td style='width:28%; text-align:center'>");
                sb.Append("<img style='width:80px; height:80px' src='" + qrcodeImage + "'> ");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial;'>");
                sb.Append("" + item.TicketCategoryName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br>");
                sb.Append("Ticket " + TicketNumber + "/" + TotalTic + "<br> ");
                //sb.Append("Regenerated Ticket 1");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border:1px solid #868686; font-family:arial; font-size:11px; text-decoration:none; padding:8px;line-height:1.1; background:#ffffff;text-align:justify;'>");
                sb.Append("<span style='font-size:13px; font-weight:bold; text-transform:uppercase;'>THIS IS YOUR TICKET. DO NOT DUPLICATE. ONE ENTRY PER BARCODE</span>&nbsp; 1. This Ticket is sold subject to the Terms and Conditions set out below and those on display at CWI ticket");
                sb.Append("offices. Any person who makes use of, purchases or holds this Ticket ('the Holder') shall be deemed to have");
                sb.Append("agreed to all of the Terms and Conditions to which this Ticket is subject. The Holder, in agreeing to these");
                sb.Append("conditions, also agrees to be bound by the CWI Refund Policy. ");
                sb.Append("2. Right of admission is reserved at the");
                sb.Append("reasonable discretion of CWI. CWI reserves the right to search the Holder. Should the Holder refuse to be");
                sb.Append("searched, CWI reserves the right to refuse admission or eject him/her from the Stadium without refund. ");
                sb.Append("3.");
                sb.Append("Ambush marketing is prohibited. Ambush marketing includes (a) the unauthorised use (by the Holder or any");
                sb.Append("other person) of the Ticket as a prize or in a lottery or competition or for any other promotional, advertising or");
                sb.Append("commercial purpose; and (b) an intentional unauthorised activity which (i) associates a person with the");
                sb.Append("Series, (ii) exploits the publicity or goodwill of the Series or (iii) has the effect of diminishing the status of");
                sb.Append("Series sponsors or conferring on other persons the status of a Series sponsor. ");
                sb.Append("4. The Holder shall not (a)");
                sb.Append("dispose of or transfer the Ticket for the purpose of commercial gain nor sell the Ticket at a higher price than");
                sb.Append("its face value; or (b) purchase or obtain the Ticket from a person/entity who is not an authorised agent. Any");
                sb.Append("Ticket obtained in breach hereof shall be void and the Holder shall have no right of entry to the Stadium. ");
                sb.Append("5.");
                sb.Append("No alcohol may be brought into or taken out of the Stadium. The Holder shall only be entitled to bring in 1");
                sb.Append("bag / carrier / rucksack which must be no larger than 400mm by 300mm by 300mm (15.7' by 11.8' by 11.8')");
                sb.Append("and must be able to fit under the seat. ");
                sb.Append("6. The Holder shall not seek entry to, or view the Match from stands");
                sb.Append("or seats for which the Holder does not hold a valid Ticket. ");
                sb.Append("7. CWI reserves the right to refuse admission or");
                sb.Append("eject from the Stadium without refund any Holder who, in the reasonable opinion of CWI is in breach of");
                sb.Append("these conditions. ");
                sb.Append("8. The Holder shall not, except for non-commercial purposes, make, use, broadcast,");
                sb.Append("transmit, publish, disseminate, reproduce or circulate by any means any recordings, audio, video, data,");
                sb.Append("images, results, commentary or any other information relating to the Match. ");
                sb.Append("9. CWI shall not be liable for");
                sb.Append("the theft, loss or damage of any property of the Holder. ");
                sb.Append("10. Management reserves the right to make alteration to the times, dates and venues of Events or to substitute the seat or area to which the ticket refers with another at its reasonable discretion. Management does not guarantee that the holder will have an uninterrupted and/or uninhibited view of the Event from the seat provided. ");
                sb.Append("11. Any comments or queries should be made by writing to us at <span style='text-decoration:underline'>customerservice@zoonga.com</span> ");
                sb.Append("12. Please refer to the restricted items list and full terms and conditions on zoonga.com");
                sb.Append("<br><br>");
                sb.Append("<b>CWI REFUND POLICY </b> 1. If a day's play is limited as a result of rain or other act beyond the control of the CWI, the CWI will refund the face value of tickets purchased as follows (i) Less than 10 overs bowled in any one day (ie up to and including 9 overs, 5 balls): 100% refund (ii) In excess of 10 overs but less than 20 overs bowled in any one day (i.e. 10 overs or more up to 19 overs, 5 balls): 50% refund (iii) In excess of twenty overs bowled in any one day (i.e. 20 overs or more): no refund. 2 For Twenty20 International matches: (i) 5 overs or less bowled: 100% refund (ii) in excess of 5 overs but less than 10 overs bowled: 50% refund (iii) in excess of 10 overs bowled (i.e. 10 overs or more): no refund.");
                sb.Append(". ");
                sb.Append("2. The above does not apply: (i) to a match that is completed early in the normal course of play; or (ii) where");
                sb.Append("a reserve day for play has been allocated or a match is rescheduled and this ticket is valid for such reserve");
                sb.Append("day or rescheduled game. ");
                sb.Append("3. To receive a refund, the retained portion of the ticket stub must be presented");
                sb.Append("and surrendered to a ticket office authorised by the CWI within ten (10) working days after the completion of");
                sb.Append("the game for which the refund is being sought. Refunds will be made only for tickets purchased from CWI");
                sb.Append("ticket offices. Under no other circumstances will refunds be given. ");
                sb.Append("4. Refunds shall only be made to the");
                sb.Append("person whose name appears on the ticket. ");
                sb.Append("5. Party Stand Refunds Only the Cricket Component of the");
                sb.Append("Party Stand ticket will be refunded in accordance with the CWI Refund Policy. Specific information on this");
                sb.Append("is available upon request through the local Ticket Office.");
                sb.Append("<!-- 13. <span style='font-weight:bold'>NO REFUND, NO EXCHANGE, GAMES DATE AND TIME SUBJECT TO CHANGE WITHOUT NOTICE</span>.-->");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>&nbsp;</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='text-align:center'>");
                sb.Append("<td style='border-top:1px dashed #000000;'>");
                sb.Append("<p style='text-align:center; margin:0; font-size:18px; position:relative; top:-11px;'>✂</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:31%; font-size:12px; font-family:arial'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial'>");
                sb.Append("" + item.SponsorOrCustomerName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br><br>");
                sb.Append("" + item.BarcodeNumber + "");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td style='width:38%; font-size:12px;' align='center'; font-family:arial>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>");
                sb.Append("" + shortMatchDay + " | " + shortMatchDate + " | " + matchtime + " (Local)<br>");
                sb.Append("" + item.VenueName + ", " + item.CityName + "</span><br><br>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>Ticket " + TicketNumber + "/" + TotalTic + " &nbsp;&nbsp;</span>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:31%; font-size:12px;'>");
                sb.Append("<img style='width:80px; height:80px' src=" + qrcodeImage + ">");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</body></html>");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return sb;
        }
        private List<string> GenerateASI(List<FIL.Contracts.Models.ASI.ASITicketInfo> asiTicketInfo)
        {

            List<string> ticketList = new List<string>();
            try
            {
                var index = 0;
                foreach (FIL.Contracts.Models.ASI.ASITicketInfo item in asiTicketInfo)
                {
                    StringBuilder sb = new StringBuilder();
                    _amazonS3FileProvider.UploadQrCodeImage(item.QRCode, ImageType.QrCode);
                    string qrcodeImage = _amazonS3FileProvider.GetImagePath(item.QRCode.ToString(), ImageType.QrCode);
                    string shortMatchDay = item.VisitDate.ToString("dddd").Substring(0, 3);
                    string longMatchDay = item.VisitDate.ToString("dddd");
                    string matchDate = item.VisitDate.ToString("MMMM dd, yyyy");
                    string numMonth = item.VisitDate.ToString("dd");
                    string numDay = item.VisitDate.ToString("MMMM").Substring(0, 3);
                    string shortMatchDate = numDay + ' ' + numMonth;
                    string matchtime = item.VisitTime;
                    index = index + 1;
                    string ticketType = string.Empty;
                    string PriceWithType = string.Empty;
                    ticketType = item.TicketType;

                    PriceWithType = "" + "₹" + " " + String.Format("{0:0.00}", item.Price) + "";
                    sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                    sb.Append("<tbody><tr>");
                    sb.Append("<td style='vertical-align:top; border:1px solid #868686 ; background:url(https://static1.zoonga.com/Images/eTickets/arena-bg.png) no-repeat top left;padding:10px'>");
                    sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0'>");
                    sb.Append("<tbody><tr>");
                    sb.Append("<td style ='vertical-align:top;'>");
                    sb.Append("<table width='718px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                    sb.Append("<tbody><tr>");
                    sb.Append("<td>");
                    sb.Append("<div style='width:350px; float:left;'>");
                    sb.Append("<img src='http://boxoffice.kyazoonga.com/tickethtml/images/etkt-z-logo.png' style='width:214px; height:77px'>");
                    sb.Append("</div>");
                    //  sb.Append("<div style='width:350px; float:right; text-align:right; font-family:arial; font-size:44px; font-weight:bold; text-decoration:none; margin-top:30px;'><i>e</i>-ticket</div> ");
                    sb.Append("<div style='width:350px; float:right; text-align:right; margin-top:25px '><img src='https://static2.zoonga.com/Images/asi-monuments/asilogo.jpg' width='300'> </div> ");
                    sb.Append("");
                    sb.Append("<div style='clear:both'></div>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='height:20px;'></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td align='center' ><div style='font-size:22px; font-weight:bold;'>E-Ticket for " + item.EventName + "</div> <div style='font-size:18px;'>Ticket is valid for one person and one time use only</ div> </td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='height:20px;'></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='vertical-align:top'>");
                    sb.Append("<td style='border:1px solid #868686; background:url(https://static1.zoonga.com/Images/eTickets/monument.jpg) no-repeat; height:267px;'> ");
                    sb.Append("<table width='650px' align='center' cellspacing='0' cellpadding='0' border='0' style='margin-top:3px;'>");
                    sb.Append("<tbody><tr>");
                    sb.Append("<td style='vertical-align:top; font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                    sb.Append("");
                    sb.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                    sb.Append("<tbody>");
                    sb.Append("<tr>");
                    sb.Append("<td style='height:8px;'></td>");
                    sb.Append("<td style='height:8px;'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:13px; text-decoration:none; font-weight:bold;'>");
                    sb.Append("<td>" + item.EventName + " </td>");

                    sb.Append("<td style='text-align:right; font-family:arial; font-weight:bold; width:150px'>");
                    sb.Append("" + PriceWithType + "<br>");
                    sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");

                    sb.Append("</td>");

                    sb.Append("</tr>");
                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                    sb.Append("<td>" + shortMatchDay + " | " + matchDate + " | " + matchtime + " <span style='font-family:arial; font-weight:normal;'> (Local)</span></td>");
                    sb.Append("<td align='right'><img src='https://static2.zoonga.com/Images/asi-monuments/swach-bharat.png' style='width:200px;'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                    sb.Append("<td>" + item.VenueName + ", " + item.CityName + "</td>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                    sb.Append("<td colspan='2'>");
                    sb.Append("");
                    sb.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                    sb.Append("<tbody>");
                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='vertical-align:top'>");
                    sb.Append("<td style='padding-top: 10px;><strong>" + item.TicketCategory + "</strong></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                    sb.Append("<td><img style='vertical-align:middle; margin-right:10px;' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'>" + item.VenueName + ", " + item.CityName + " </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                    sb.Append("</tr>");
                    sb.Append("<tr style='height:10px'><td></td></tr>");
                    sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                    sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                    sb.Append("</td></tr>");
                    sb.Append("<!--<tr style='height:15px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:normal;'>");
                    sb.Append("<td> Only sponsor's products will be permitted into the stadium.<br>");
                    sb.Append("See www.cplt20.com/T&amp;Cs. Ambush marketing not permitted</td>");
                    sb.Append("</tr>-->");
                    sb.Append("</tbody></table>");
                    sb.Append("");
                    sb.Append("</td>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<!--<tr style='height:3px'>");
                    sb.Append("</tr> ");
                    sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:bold;'>");
                    sb.Append("<td> CPL Partners: Hero * Digicel * Guardian * El Dorado</td>");
                    sb.Append("</tr>-->");
                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</tbody></table>");
                    sb.Append("");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='height:8px;'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                    sb.Append("");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                    sb.Append("<tbody>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:36%; font-size:12px; font-family:arial;'>");
                    sb.Append("<span style='text-transform:uppercase; font-family:arial; font-weight:bold; line-height:1.4'>This is Your Ticket</span><br>");
                    sb.Append("Keep this page with you at all times<br>");
                    sb.Append("<span style='text-transform:uppercase; font-family:arial;font-weight:bold; line-height:1.4'>Do Not Duplicate</span>");
                    sb.Append("</td>");
                    sb.Append("<td style='width:28%; text-align:center'>");
                    sb.Append("<img style='width:120px; height:120px' src='" + qrcodeImage + "'> ");
                    sb.Append("</td>");
                    sb.Append("<td align='right' style='width:36%; font-size:12px; font-family:arial;'>");
                    sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial;'>");
                    sb.Append("" + item.TicketCategory + "</span><br>");
                    sb.Append("" + item.TicketNo + "<br>");
                    sb.Append("Ticket " + index + "/" + asiTicketInfo.Count() + "<br> ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='height:8px;'></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='border:1px solid #868686; font-family:arial; font-size:11px; text-decoration:none; padding:8px;line-height:1.1; background:#ffffff;text-align:justify;'>");
                    sb.Append("<span style='font-size:13px; font-weight:bold; text-transform:uppercase;'>THIS IS YOUR TICKET. DO NOT DUPLICATE. ONE ENTRY PER BARCODE.</span>&nbsp;");
                    sb.Append("<br ><br ><span style='font-size:13px;'>1. The e-ticket is not transferable.</span>&nbsp;");
                    sb.Append("<br ><span style='font-size:13px;'>2. Entry Fee is not refundable.</span>&nbsp;");
                    sb.Append("<br ><span style='font-size:13px;'>3. E-ticket cancellations are not permitted.</ span>&nbsp;");
                    sb.Append("<br ><span style='font-size:13px;'>4. Visitor shall be required to show photo identity proof in original at the entry to the monument.</ span>&nbsp;");
                    sb.Append("<br ><span style='font-size:13px;'>5. Inflammable/dangerous/explosive articles are not allowed.</ span>&nbsp;");
                    sb.Append("<br ><span style='font-size:13px;'>6. This monument is a Non Smoking zone.</ span>&nbsp;");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td>&nbsp;</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='text-align:center'>");
                    sb.Append("<td style='border-top:1px dashed #000000;'>");
                    sb.Append("<p style='text-align:center; margin:0; font-size:18px; position:relative; top:-11px;'>✂</p>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("");
                    sb.Append("<tr>");
                    sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                    sb.Append("");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                    sb.Append("<tbody>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width:31%; font-size:12px; font-family:arial'>");
                    sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial'>");
                    sb.Append("" + item.TicketNo + "<br><br>");
                    sb.Append("");
                    sb.Append("</td>");
                    sb.Append("<td style='width:38%; font-size:12px;' align='right'; font-family:arial>");
                    sb.Append("<span style='font-weight:bold; font-family:arial'>");
                    sb.Append("" + shortMatchDay + " | " + shortMatchDate + " | " + matchtime + " (Local)<br>");
                    sb.Append("" + item.VenueName + ", " + item.CityName + "</span><br><br>");
                    sb.Append("<span style='font-weight:bold; font-family:arial'>Ticket " + index + "/" + asiTicketInfo.Count() + " &nbsp;&nbsp;</span>");
                    sb.Append("");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</tbody></table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</tbody></table> ");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</tbody></table>");
                    sb.Append("</body></html>");

                    ticketList.Add(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return ticketList;
        }
        private StringBuilder GeneratePrintAtHomeSuper50Series(MatchSeatTicketInfo item, int TicketNumber, int TotalTic, long transactionId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                _amazonS3FileProvider.UploadQrCodeImage(item.BarcodeNumber, ImageType.QrCode);
                string qrcodeImage = _amazonS3FileProvider.GetImagePath(item.BarcodeNumber.ToString(), ImageType.QrCode);
                string shortMatchDay = item.EventStartTime.ToString("dddd").Substring(0, 3);
                string longMatchDay = item.EventStartTime.ToString("dddd");
                string matchDate = item.EventStartTime.ToString("MMMM dd, yyyy");
                string numMonth = item.EventStartTime.ToString("dd");
                string numDay = item.EventStartTime.ToString("MMMM").Substring(0, 3);
                string shortMatchDate = numDay + ' ' + numMonth;
                string matchtime = item.EventStartTime.ToString("HH:mm");
                string IsWheelChair = item.IsWheelChair.ToString();
                string IsCompanion = item.IsCompanion.ToString();
                StringBuilder sbTicket = new StringBuilder();
                string matchAdditionalInfo = item.MatchAdditionalInfo;
                string ticketType = string.Empty;
                string PriceWithType = string.Empty;


                if (item.TicketTypeId == 2)
                {
                    ticketType = "Child";
                    PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                }
                else if (item.TicketTypeId == 3)
                {
                    ticketType = "Senior Citizen";
                    PriceWithType = "Complimentary";
                }
                else
                {
                    if (Convert.ToInt32(IsCompanion) > 1)
                    {
                        ticketType = "Companion";
                        PriceWithType = "" + item.CurrencyName + " 0.00";
                    }
                    else if (Convert.ToInt32(IsWheelChair) > 1)
                    {
                        ticketType = "Wheelchair";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                    else
                    {
                        ticketType = "Adult";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                }

                //string IsLayoutAvailable = dr["IsLayoutAvail"].ToString();

                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; border:1px solid #868686 ; background:url(https://static1.zoonga.com/Images/eTickets/arena-bg.png) no-repeat top left;padding:10px'>");
                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                //sb.Append("<td style='vertical-align:top; background:url(images/etkt-tkt-bg-1.gi) no-repeat bottom; padding:10px'>");
                sb.Append("<td style ='vertical-align:top;'>");
                sb.Append("<table width='718px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td>");
                sb.Append("<div style='width:350px; float:left;'>");
                sb.Append("<img src='http://boxoffice.kyazoonga.com/tickethtml/images/etkt-z-logo.png' style='width:214px; height:77px'>");
                sb.Append("</div>");
                sb.Append("<div style='width:350px; float:right; text-align:right; font-family:arial; font-size:44px; font-weight:bold; text-decoration:none; margin-top:30px;'><i>e</i>-ticket</div> ");
                sb.Append("");
                sb.Append("<div style='clear:both'></div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='height:4px; border-bottom:2px solid #dcdcdc'></td>");
                //sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:10px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top'>");
                sb.Append("<td style='border:1px solid #868686; background:url(https://static1.zoonga.com/Images/eTickets/ticket-at-home-bg.jpg) no-repeat; height:267px;'> ");
                sb.Append("<table width='650px' align='center' cellspacing='0' cellpadding='0' border='0' style='margin-top:3px;'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("");
                sb.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                sb.Append("<tbody>");
                sb.Append("<tr><td style='height:10px'></td></tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:17px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>");
                sb.Append("Regional Super50 CUP <span style='font-family:arial; font-size:13px; font-weight:bold;'><br></span>");
                //if (item.VenueId.ToString() == "57")
                //{
                //    sb.Append("Australia Women's Tour of Caribbean 2019 <span style='font-family:arial; font-size:13px; font-weight:bold;'><br></span>");
                //}
                //else
                //{
                //    sb.Append("Colonial Medical Insurance ODI Series <span style='font-family:arial; font-size:13px; font-weight:bold;'><br></span>");
                //}
                sb.Append("</td>");
                sb.Append("<td rowspan='5' style='text-align:right'>");
                sb.Append("<table cellpadding='0' width='100%' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='text-align:right; font-family:arial; font-weight:bold '>");
                //sb.Append("" + PriceWithType + "<br>");
                //sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                if (item.TransactingOptionId == 2)
                {
                    sb.Append("Complimentary<br>");
                }
                else
                {
                    sb.Append("" + PriceWithType + "<br>");
                    sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                }
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.EventDetailsName + " </td>");
                //if (item.EventDeatilId.ToString() == "555934" || item.EventDeatilId.ToString() == "555472")
                //{
                //    sb.Append("<td>West Indies Women vs Australia Women, 1st T20I </td>");
                //}
                //else if (item.EventDeatilId.ToString() == "555935" || item.EventDeatilId.ToString() == "555473")
                //{
                //    sb.Append("<td>West Indies Women vs Australia Women, 2nd T20I </td>");
                //}
                //else if (item.EventDeatilId.ToString() == "555936" || item.EventDeatilId.ToString() == "555474")
                //{
                //    sb.Append("<td>West Indies Women vs Australia Women, 3rd T20I </td>");
                //}
                //else if (item.EventDeatilId.ToString() == "555931" || item.EventDeatilId.ToString() == "555469")
                //{
                //    sb.Append("<td>West Indies Women vs Australia Women, 1st ODI </td>");
                //}
                //else if (item.EventDeatilId.ToString() == "555932" || item.EventDeatilId.ToString() == "555470")
                //{
                //    sb.Append("<td>West Indies Women vs Australia Women, 2nd ODI </td>");
                //}
                //else if (item.EventDeatilId.ToString() == "555933" || item.EventDeatilId.ToString() == "555471")
                //{
                //    sb.Append("<td>West Indies Women vs Australia Women, 3rd ODI </td>");
                //}
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                //sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                //sb.Append("<td>&nbsp;</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr style='height:4px'>");
                //sb.Append("<td></td>");
                //sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td>" + shortMatchDay + " | " + matchDate + " | " + matchtime + " <span style='font-family:arial; font-weight:normal;'> (Local)</span></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.VenueName + ", " + item.CityName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td colspan='2'>");
                sb.Append("");
                sb.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Enter through: &nbsp;<strong>" + item.GateName + " Gate</strong> </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Gates Open At <strong>" + item.GateOpenTime + "</strong> (Local)</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");

                StringBuilder sbMatchDay = new StringBuilder();

                if (item.EventDeatilId.ToString() == "554529" || item.EventDeatilId.ToString() == "554531" || item.EventDeatilId.ToString() == "554532")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("ODI <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else if (item.EventDeatilId.ToString() == "554513" || item.EventDeatilId.ToString() == "554514" || item.EventDeatilId.ToString() == "554515")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:5px; font-weight:bold'>");
                    sbMatchDay.Append("T20I <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("Day <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchDay + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }

                if (item.VenueId.ToString() == "11691" || item.VenueId.ToString() == "9697")
                {
                    if (item.IsSeatSelection)
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td>Row: &nbsp;<strong>" + item.RowNumber + " </strong> &nbsp;&nbsp;Seat:  &nbsp;<strong>" + item.TicketNumber + " </strong></td>");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                    else
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                }
                else
                {
                    sb.Append("<tr style='vertical-align:top'>");
                    sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                    sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                    //sb.Append(sbMatchDay.ToString());
                    sb.Append("</tr>");
                    if (item.TicketTypeId == 2)
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                        sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                        sb.Append("</td></tr>");
                    }
                }

                sb.Append("<!--<tr style='height:15px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:normal;'>");
                sb.Append("<td> Only sponsor's products will be permitted into the stadium.<br>");
                sb.Append("See www.cplt20.com/T&amp;Cs. Ambush marketing not permitted</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<!--<tr style='height:3px'>");
                sb.Append("</tr> ");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td> CPL Partners: Hero * Digicel * Guardian * El Dorado</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial; font-weight:bold; line-height:1.4'>This is Your Ticket</span><br>");
                sb.Append("Keep this page with you at all times<br>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial;font-weight:bold; line-height:1.4'>Do Not Duplicate</span>");
                sb.Append("</td>");
                sb.Append("<td style='width:28%; text-align:center'>");
                sb.Append("<img style='width:80px; height:80px' src='" + qrcodeImage + "'> ");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial;'>");
                sb.Append("" + item.TicketCategoryName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br>");
                sb.Append("Ticket " + TicketNumber + "/" + TotalTic + "<br> ");
                //sb.Append("Regenerated Ticket 1");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border:1px solid #868686; font-family:arial; font-size:11px; text-decoration:none; padding:8px;line-height:1.1; background:#ffffff;text-align:justify;'>");
                sb.Append("<span style='font-size:13px; font-weight:bold; text-transform:uppercase;'>THIS IS YOUR TICKET. DO NOT DUPLICATE. ONE ENTRY PER BARCODE</span>&nbsp; 1. This Ticket is sold subject to the Terms and Conditions set out below and those on display at CWI ticket");
                sb.Append("offices. Any person who makes use of, purchases or holds this Ticket ('the Holder') shall be deemed to have");
                sb.Append("agreed to all of the Terms and Conditions to which this Ticket is subject. The Holder, in agreeing to these");
                sb.Append("conditions, also agrees to be bound by the CWI Refund Policy. ");
                sb.Append("2. Right of admission is reserved at the");
                sb.Append("reasonable discretion of CWI. CWI reserves the right to search the Holder. Should the Holder refuse to be");
                sb.Append("searched, CWI reserves the right to refuse admission or eject him/her from the Stadium without refund. ");
                sb.Append("3.");
                sb.Append("Ambush marketing is prohibited. Ambush marketing includes (a) the unauthorised use (by the Holder or any");
                sb.Append("other person) of the Ticket as a prize or in a lottery or competition or for any other promotional, advertising or");
                sb.Append("commercial purpose; and (b) an intentional unauthorised activity which (i) associates a person with the");
                sb.Append("Series, (ii) exploits the publicity or goodwill of the Series or (iii) has the effect of diminishing the status of");
                sb.Append("Series sponsors or conferring on other persons the status of a Series sponsor. ");
                sb.Append("4. The Holder shall not (a)");
                sb.Append("dispose of or transfer the Ticket for the purpose of commercial gain nor sell the Ticket at a higher price than");
                sb.Append("its face value; or (b) purchase or obtain the Ticket from a person/entity who is not an authorised agent. Any");
                sb.Append("Ticket obtained in breach hereof shall be void and the Holder shall have no right of entry to the Stadium. ");
                sb.Append("5.");
                sb.Append("No alcohol may be brought into or taken out of the Stadium. The Holder shall only be entitled to bring in 1");
                sb.Append("bag / carrier / rucksack which must be no larger than 400mm by 300mm by 300mm (15.7' by 11.8' by 11.8')");
                sb.Append("and must be able to fit under the seat. ");
                sb.Append("6. The Holder shall not seek entry to, or view the Match from stands");
                sb.Append("or seats for which the Holder does not hold a valid Ticket. ");
                sb.Append("7. CWI reserves the right to refuse admission or");
                sb.Append("eject from the Stadium without refund any Holder who, in the reasonable opinion of CWI is in breach of");
                sb.Append("these conditions. ");
                sb.Append("8. The Holder shall not, except for non-commercial purposes, make, use, broadcast,");
                sb.Append("transmit, publish, disseminate, reproduce or circulate by any means any recordings, audio, video, data,");
                sb.Append("images, results, commentary or any other information relating to the Match. ");
                sb.Append("9. CWI shall not be liable for");
                sb.Append("the theft, loss or damage of any property of the Holder. ");
                sb.Append("10. Management reserves the right to make alteration to the times, dates and venues of Events or to substitute the seat or area to which the ticket refers with another at its reasonable discretion. Management does not guarantee that the holder will have an uninterrupted and/or uninhibited view of the Event from the seat provided. ");
                sb.Append("11. Any comments or queries should be made by writing to us at <span style='text-decoration:underline'>customerservice@zoonga.com</span> ");
                sb.Append("12. Please refer to the restricted items list and full terms and conditions on zoonga.com");
                sb.Append("<br><br>");
                sb.Append("<b>CWI REFUND POLICY </b> 1. If a day's play is limited as a result of rain or other act beyond the control of the CWI, the CWI will refund the face value of tickets purchased as follows (i) Less than 10 overs bowled in any one day (ie up to and including 9 overs, 5 balls): 100% refund (ii) In excess of 10 overs but less than 20 overs bowled in any one day (i.e. 10 overs or more up to 19 overs, 5 balls): 50% refund (iii) In excess of twenty overs bowled in any one day (i.e. 20 overs or more): no refund. 2 For Twenty20 International matches: (i) 5 overs or less bowled: 100% refund (ii) in excess of 5 overs but less than 10 overs bowled: 50% refund (iii) in excess of 10 overs bowled (i.e. 10 overs or more): no refund.");
                sb.Append(". ");
                sb.Append("2. The above does not apply: (i) to a match that is completed early in the normal course of play; or (ii) where");
                sb.Append("a reserve day for play has been allocated or a match is rescheduled and this ticket is valid for such reserve");
                sb.Append("day or rescheduled game. ");
                sb.Append("3. To receive a refund, the retained portion of the ticket stub must be presented");
                sb.Append("and surrendered to a ticket office authorised by the CWI within ten (10) working days after the completion of");
                sb.Append("the game for which the refund is being sought. Refunds will be made only for tickets purchased from CWI");
                sb.Append("ticket offices. Under no other circumstances will refunds be given. ");
                sb.Append("4. Refunds shall only be made to the");
                sb.Append("person whose name appears on the ticket. ");
                sb.Append("5. Party Stand Refunds Only the Cricket Component of the");
                sb.Append("Party Stand ticket will be refunded in accordance with the CWI Refund Policy. Specific information on this");
                sb.Append("is available upon request through the local Ticket Office.");
                sb.Append("<!-- 13. <span style='font-weight:bold'>NO REFUND, NO EXCHANGE, GAMES DATE AND TIME SUBJECT TO CHANGE WITHOUT NOTICE</span>.-->");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>&nbsp;</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='text-align:center'>");
                sb.Append("<td style='border-top:1px dashed #000000;'>");
                sb.Append("<p style='text-align:center; margin:0; font-size:18px; position:relative; top:-11px;'>✂</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:31%; font-size:12px; font-family:arial'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial'>");
                sb.Append("" + item.SponsorOrCustomerName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br><br>");
                sb.Append("" + item.BarcodeNumber + "");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td style='width:38%; font-size:12px;' align='center'; font-family:arial>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>");
                sb.Append("" + shortMatchDay + " | " + shortMatchDate + " | " + matchtime + " (Local)<br>");
                sb.Append("" + item.VenueName + ", " + item.CityName + "</span><br><br>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>Ticket " + TicketNumber + "/" + TotalTic + " &nbsp;&nbsp;</span>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:31%; font-size:12px;'>");
                sb.Append("<img style='width:80px; height:80px' src=" + qrcodeImage + ">");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</body></html>");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return sb;
        }

        private StringBuilder GeneratePrintAtHomeIndiaWindiesWomen(MatchSeatTicketInfo item, int TicketNumber, int TotalTic, long transactionId, TicketPrintingOption ticketPrintingOptionId, bool isPrintEntityName)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                _amazonS3FileProvider.UploadQrCodeImage(item.BarcodeNumber, ImageType.QrCode);
                string qrcodeImage = _amazonS3FileProvider.GetImagePath(item.BarcodeNumber.ToString(), ImageType.QrCode);
                string shortMatchDay = item.EventStartTime.ToString("dddd").Substring(0, 3);
                string longMatchDay = item.EventStartTime.ToString("dddd");
                string matchDate = item.EventStartTime.ToString("MMMM dd, yyyy");
                string numMonth = item.EventStartTime.ToString("dd");
                string numDay = item.EventStartTime.ToString("MMMM").Substring(0, 3);
                string shortMatchDate = numDay + ' ' + numMonth;
                string matchtime = item.EventStartTime.ToString("HH:mm");
                string IsWheelChair = item.IsWheelChair.ToString();
                string IsCompanion = item.IsCompanion.ToString();
                StringBuilder sbTicket = new StringBuilder();
                string matchAdditionalInfo = item.MatchAdditionalInfo;
                string ticketType = string.Empty;
                string PriceWithType = string.Empty;

                //string IsLayoutAvailable = dr["IsLayoutAvail"].ToString();

                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; border:1px solid #868686 ; background:url(https://static1.zoonga.com/Images/eTickets/arena-bg.png) no-repeat top left;padding:10px'>");
                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                //sb.Append("<td style='vertical-align:top; background:url(images/etkt-tkt-bg-1.gi) no-repeat bottom; padding:10px'>");
                sb.Append("<td style ='vertical-align:top;'>");
                sb.Append("<table width='718px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td>");
                sb.Append("<div style='width:350px; float:left;'>");
                sb.Append("<img src='http://boxoffice.kyazoonga.com/tickethtml/images/etkt-z-logo.png' style='width:214px; height:77px'>");
                sb.Append("</div>");
                sb.Append("<div style='width:350px; float:right; text-align:right; font-family:arial; font-size:44px; font-weight:bold; text-decoration:none; margin-top:30px;'><i>e</i>-ticket</div> ");
                sb.Append("");
                sb.Append("<div style='clear:both'></div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='height:4px; border-bottom:2px solid #dcdcdc'></td>");
                //sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:10px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top'>");
                sb.Append("<td style='border:1px solid #868686; background:url(https://static1.zoonga.com/Images/eTickets/india-women-bg.jpg) no-repeat; height:267px;'> ");
                sb.Append("<table width='650px' align='center' cellspacing='0' cellpadding='0' border='0' style='margin-top:3px;'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("");
                sb.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                sb.Append("<tbody>");
                sb.Append("<tr><td style='height:10px'></td></tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:17px; text-decoration:none; font-weight:bold;'>");
                //sb.Append("<td>");
                //sb.Append("MyTeam11.com West Indies vs India <span style='font-family:arial; font-size:13px; font-weight:bold;'><br>co-powered by Skoda</span>");
                //sb.Append("</td>");
                sb.Append("<td>");
                sb.Append("India Women`s Tour of Caribbean 2019");
                sb.Append("</td>");
                sb.Append("<td rowspan='5' style='text-align:right'>");
                sb.Append("<table cellpadding='0' width='100%' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='text-align:right; font-family:arial; font-weight:bold '>");
         
                if (ticketPrintingOptionId== TicketPrintingOption.Price)
                {
                    if (item.TicketTypeId == 2)
                    {
                        ticketType = "Child";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                    else if (item.TicketTypeId == 3)
                    {
                        ticketType = "Senior Citizen";
                        PriceWithType = "Complimentary";
                    }
                    else
                    {
                        if (Convert.ToInt32(IsCompanion) > 1)
                        {
                            ticketType = "Companion";
                            PriceWithType = "" + item.CurrencyName + " 0.00";
                        }
                        else if (Convert.ToInt32(IsWheelChair) > 1)
                        {
                            ticketType = "Wheelchair";
                            PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                        }
                        else
                        {
                            ticketType = "Adult";
                            PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                        }
                    }
                    sb.Append("" + PriceWithType + "<br>");
                    sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                }
                else if (ticketPrintingOptionId==TicketPrintingOption.Complimentary)
                {
                    ticketType = "Adult";                  
                    sb.Append("Complimentary<br>");
                }
                else if (ticketPrintingOptionId == TicketPrintingOption.ChildComplimentary)
                {
                    ticketType = "Child";
                    sb.Append("Child<br>Complimentary<br>");
                }
                else if (ticketPrintingOptionId == TicketPrintingOption.GovernmentComplimentary)
                {
                    ticketType = "Government";
                    sb.Append("Government<br>Complimentary<br>");
                }
                else if (ticketPrintingOptionId == TicketPrintingOption.Hospitality)
                {
                    ticketType = "Adult";
                    sb.Append("Hospitality<br>");
                }
                else if (ticketPrintingOptionId == TicketPrintingOption.ChildPrice)
                {
                    ticketType = "Child";
                    PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price/2) + "";
                    sb.Append("" + PriceWithType + "<br>");
                    sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                }
                else if (ticketPrintingOptionId == TicketPrintingOption.ZeroPrice)
                {

                    ticketType = "Adult";
                    PriceWithType = "" + item.CurrencyName + " 0.00";
                    sb.Append("" + PriceWithType + "<br>");
                    sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                }
                else if (ticketPrintingOptionId == TicketPrintingOption.NoPrice)
                {
                    sb.Append("&nbsp;<br>");                
                }
                else if (ticketPrintingOptionId == TicketPrintingOption.ComplimentaryZeroPrice)
                {
                    PriceWithType = "" + item.CurrencyName + " 0.00";
                    sb.Append("Complimentary" + PriceWithType + "<br>");                 
                }
                else
                {
                    sb.Append("" + PriceWithType + "<br>");
                    sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                }
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.EventDetailsName + " </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td>" + shortMatchDay + " | " + matchDate + " | " + matchtime + " <span style='font-family:arial; font-weight:normal;'> (Local)</span></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.VenueName + ", " + item.CityName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td colspan='2'>");
                sb.Append("");
                sb.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Enter through: &nbsp;<strong>" + item.GateName + " Gate</strong> </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Gates Open At <strong>" + item.GateOpenTime + "</strong> (Local)</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");

                StringBuilder sbMatchDay = new StringBuilder();

                if (item.EventDeatilId.ToString() == "554529" || item.EventDeatilId.ToString() == "554531" || item.EventDeatilId.ToString() == "554532")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("ODI <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else if (item.EventDeatilId.ToString() == "554513" || item.EventDeatilId.ToString() == "554514" || item.EventDeatilId.ToString() == "554515")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:5px; font-weight:bold'>");
                    sbMatchDay.Append("T20I <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("Day <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchDay + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }

                if (item.VenueId.ToString() == "58" || item.VenueId.ToString() == "62")
                {
                    if (item.IsSeatSelection)
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td>Row: &nbsp;<strong>" + item.RowNumber + " </strong> &nbsp;&nbsp;Seat:  &nbsp;<strong>" + item.TicketNumber + " </strong></td>");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                    else
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                }
                else
                {
                    sb.Append("<tr style='vertical-align:top'>");
                    sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                    sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                    //sb.Append(sbMatchDay.ToString());
                    sb.Append("</tr>");
                    if (item.TicketTypeId == 2)
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                        sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                        sb.Append("</td></tr>");
                    }
                }

                sb.Append("<!--<tr style='height:15px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:normal;'>");
                sb.Append("<td> Only sponsor's products will be permitted into the stadium.<br>");
                sb.Append("See www.cplt20.com/T&amp;Cs. Ambush marketing not permitted</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<!--<tr style='height:3px'>");
                sb.Append("</tr> ");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td> CPL Partners: Hero * Digicel * Guardian * El Dorado</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial; font-weight:bold; line-height:1.4'>This is Your Ticket</span><br>");
                sb.Append("Keep this page with you at all times<br>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial;font-weight:bold; line-height:1.4'>Do Not Duplicate</span>");
                sb.Append("</td>");
                sb.Append("<td style='width:28%; text-align:center'>");
                sb.Append("<img style='width:80px; height:80px' src='" + qrcodeImage + "'> ");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial;'>");
                sb.Append("" + item.TicketCategoryName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br>");
                sb.Append("Ticket " + TicketNumber + "/" + TotalTic + "<br> ");
                //sb.Append("Regenerated Ticket 1");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border:1px solid #868686; font-family:arial; font-size:11px; text-decoration:none; padding:8px;line-height:1.1; background:#ffffff;text-align:justify;'>");
                sb.Append("<span style='font-size:13px; font-weight:bold; text-transform:uppercase;'>THIS IS YOUR TICKET. DO NOT DUPLICATE. ONE ENTRY PER BARCODE</span>&nbsp; 1. This Ticket is sold subject to the Terms and Conditions set out below and those on display at CWI ticket");
                sb.Append("offices. Any person who makes use of, purchases or holds this Ticket ('the Holder') shall be deemed to have");
                sb.Append("agreed to all of the Terms and Conditions to which this Ticket is subject. The Holder, in agreeing to these");
                sb.Append("conditions, also agrees to be bound by the CWI Refund Policy. ");
                sb.Append("2. Right of admission is reserved at the");
                sb.Append("reasonable discretion of CWI. CWI reserves the right to search the Holder. Should the Holder refuse to be");
                sb.Append("searched, CWI reserves the right to refuse admission or eject him/her from the Stadium without refund. ");
                sb.Append("3.");
                sb.Append("Ambush marketing is prohibited. Ambush marketing includes (a) the unauthorised use (by the Holder or any");
                sb.Append("other person) of the Ticket as a prize or in a lottery or competition or for any other promotional, advertising or");
                sb.Append("commercial purpose; and (b) an intentional unauthorised activity which (i) associates a person with the");
                sb.Append("Series, (ii) exploits the publicity or goodwill of the Series or (iii) has the effect of diminishing the status of");
                sb.Append("Series sponsors or conferring on other persons the status of a Series sponsor. ");
                sb.Append("4. The Holder shall not (a)");
                sb.Append("dispose of or transfer the Ticket for the purpose of commercial gain nor sell the Ticket at a higher price than");
                sb.Append("its face value; or (b) purchase or obtain the Ticket from a person/entity who is not an authorised agent. Any");
                sb.Append("Ticket obtained in breach hereof shall be void and the Holder shall have no right of entry to the Stadium. ");
                sb.Append("5.");
                sb.Append("No alcohol may be brought into or taken out of the Stadium. The Holder shall only be entitled to bring in 1");
                sb.Append("bag / carrier / rucksack which must be no larger than 400mm by 300mm by 300mm (15.7' by 11.8' by 11.8')");
                sb.Append("and must be able to fit under the seat. ");
                sb.Append("6. The Holder shall not seek entry to, or view the Match from stands");
                sb.Append("or seats for which the Holder does not hold a valid Ticket. ");
                sb.Append("7. CWI reserves the right to refuse admission or");
                sb.Append("eject from the Stadium without refund any Holder who, in the reasonable opinion of CWI is in breach of");
                sb.Append("these conditions. ");
                sb.Append("8. The Holder shall not, except for non-commercial purposes, make, use, broadcast,");
                sb.Append("transmit, publish, disseminate, reproduce or circulate by any means any recordings, audio, video, data,");
                sb.Append("images, results, commentary or any other information relating to the Match. ");
                sb.Append("9. CWI shall not be liable for");
                sb.Append("the theft, loss or damage of any property of the Holder. ");
                sb.Append("10. Management reserves the right to make alteration to the times, dates and venues of Events or to substitute the seat or area to which the ticket refers with another at its reasonable discretion. Management does not guarantee that the holder will have an uninterrupted and/or uninhibited view of the Event from the seat provided. ");
                sb.Append("11. Any comments or queries should be made by writing to us at <span style='text-decoration:underline'>customerservice@zoonga.com</span> ");
                sb.Append("12. Please refer to the restricted items list and full terms and conditions on zoonga.com");
                sb.Append("<br><br>");
                sb.Append("<b>CWI REFUND POLICY </b> 1. If a day's play is limited as a result of rain or other act beyond the control of the CWI, the CWI will refund the face value of tickets purchased as follows (i) Less than 10 overs bowled in any one day (ie up to and including 9 overs, 5 balls): 100% refund (ii) In excess of 10 overs but less than 20 overs bowled in any one day (i.e. 10 overs or more up to 19 overs, 5 balls): 50% refund (iii) In excess of twenty overs bowled in any one day (i.e. 20 overs or more): no refund. 2 For Twenty20 International matches: (i) 5 overs or less bowled: 100% refund (ii) in excess of 5 overs but less than 10 overs bowled: 50% refund (iii) in excess of 10 overs bowled (i.e. 10 overs or more): no refund.");
                sb.Append(". ");
                sb.Append("2. The above does not apply: (i) to a match that is completed early in the normal course of play; or (ii) where");
                sb.Append("a reserve day for play has been allocated or a match is rescheduled and this ticket is valid for such reserve");
                sb.Append("day or rescheduled game. ");
                sb.Append("3. To receive a refund, the retained portion of the ticket stub must be presented");
                sb.Append("and surrendered to a ticket office authorised by the CWI within ten (10) working days after the completion of");
                sb.Append("the game for which the refund is being sought. Refunds will be made only for tickets purchased from CWI");
                sb.Append("ticket offices. Under no other circumstances will refunds be given. ");
                sb.Append("4. Refunds shall only be made to the");
                sb.Append("person whose name appears on the ticket. ");
                sb.Append("5. Party Stand Refunds Only the Cricket Component of the");
                sb.Append("Party Stand ticket will be refunded in accordance with the CWI Refund Policy. Specific information on this");
                sb.Append("is available upon request through the local Ticket Office.");
                sb.Append("<!-- 13. <span style='font-weight:bold'>NO REFUND, NO EXCHANGE, GAMES DATE AND TIME SUBJECT TO CHANGE WITHOUT NOTICE</span>.-->");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>&nbsp;</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='text-align:center'>");
                sb.Append("<td style='border-top:1px dashed #000000;'>");
                sb.Append("<p style='text-align:center; margin:0; font-size:18px; position:relative; top:-11px;'>✂</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:31%; font-size:12px; font-family:arial'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial'>");
                if (isPrintEntityName)
                {
                    sb.Append("" + item.SponsorOrCustomerName + "</span><br>");
                }
                else {
                    sb.Append("&nbsp;</span><br>");
                }
             
                sb.Append("Z-" + transactionId + "<br><br>");
                sb.Append("" + item.BarcodeNumber + "");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td style='width:38%; font-size:12px;' align='center'; font-family:arial>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>");
                sb.Append("" + shortMatchDay + " | " + shortMatchDate + " | " + matchtime + " (Local)<br>");
                sb.Append("" + item.VenueName + ", " + item.CityName + "</span><br><br>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>Ticket " + TicketNumber + "/" + TotalTic + " &nbsp;&nbsp;</span>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:31%; font-size:12px;'>");
                sb.Append("<img style='width:80px; height:80px' src=" + qrcodeImage + ">");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</body></html>");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return sb;
        }

        private StringBuilder GeneratePrintAtHomeIreLandWindies(MatchSeatTicketInfo item, int TicketNumber, int TotalTic, long transactionId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                _amazonS3FileProvider.UploadQrCodeImage(item.BarcodeNumber, ImageType.QrCode);
                string qrcodeImage = _amazonS3FileProvider.GetImagePath(item.BarcodeNumber.ToString(), ImageType.QrCode);
                string shortMatchDay = item.EventStartTime.ToString("dddd").Substring(0, 3);
                string longMatchDay = item.EventStartTime.ToString("dddd");
                string matchDate = item.EventStartTime.ToString("MMMM dd, yyyy");
                string numMonth = item.EventStartTime.ToString("dd");
                string numDay = item.EventStartTime.ToString("MMMM").Substring(0, 3);
                string shortMatchDate = numDay + ' ' + numMonth;
                string matchtime = item.EventStartTime.ToString("HH:mm");
                string IsWheelChair = item.IsWheelChair.ToString();
                string IsCompanion = item.IsCompanion.ToString();
                StringBuilder sbTicket = new StringBuilder();
                string matchAdditionalInfo = item.MatchAdditionalInfo;
                string ticketType = string.Empty;
                string PriceWithType = string.Empty;


                if (item.TicketTypeId == 2)
                {
                    ticketType = "Child";
                    PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                }
                else if (item.TicketTypeId == 3)
                {
                    ticketType = "Senior Citizen";
                    PriceWithType = "Complimentary";
                }
                else
                {
                    if (Convert.ToInt32(IsCompanion) > 1)
                    {
                        ticketType = "Companion";
                        PriceWithType = "" + item.CurrencyName + " 0.00";
                    }
                    else if (Convert.ToInt32(IsWheelChair) > 1)
                    {
                        ticketType = "Wheelchair";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                    else
                    {
                        ticketType = "Adult";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                }

                //string IsLayoutAvailable = dr["IsLayoutAvail"].ToString();

                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; border:1px solid #868686 ; background:url(https://static1.zoonga.com/Images/eTickets/arena-bg.png) no-repeat top left;padding:10px'>");
                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                //sb.Append("<td style='vertical-align:top; background:url(images/etkt-tkt-bg-1.gi) no-repeat bottom; padding:10px'>");
                sb.Append("<td style ='vertical-align:top;'>");
                sb.Append("<table width='718px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td>");
                sb.Append("<div style='width:350px; float:left;'>");
                sb.Append("<img src='http://boxoffice.kyazoonga.com/tickethtml/images/etkt-z-logo.png' style='width:214px; height:77px'>");
                sb.Append("</div>");
                sb.Append("<div style='width:350px; float:right; text-align:right; font-family:arial; font-size:44px; font-weight:bold; text-decoration:none; margin-top:30px;'><i>e</i>-ticket</div> ");
                sb.Append("");
                sb.Append("<div style='clear:both'></div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='height:4px; border-bottom:2px solid #dcdcdc'></td>");
                //sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:10px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top'>");
                sb.Append("<td style='border:1px solid #868686; background:url(https://static1.zoonga.com/Images/eTickets/ireland-bg.jpg) no-repeat; height:267px;'> ");
                sb.Append("<table width='650px' align='center' cellspacing='0' cellpadding='0' border='0' style='margin-top:3px;'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("");
                sb.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                sb.Append("<tbody>");
                sb.Append("<tr><td style='height:10px'></td></tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:17px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>");
                sb.Append("Ireland Tour of West Indies");
                sb.Append("</td>");
                sb.Append("<td rowspan='5' style='text-align:right'>");
                sb.Append("<table cellpadding='0' width='100%' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='text-align:right; font-family:arial; font-weight:bold '>");

                if (item.TransactingOptionId == 2)
                {
                    sb.Append("Complimentary<br>");
                }
                else
                {
                    sb.Append("" + PriceWithType + "<br>");
                    sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                }
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.EventDetailsName + " </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");

                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td>" + shortMatchDay + " | " + matchDate + " | " + matchtime + " <span style='font-family:arial; font-weight:normal;'> (Local)</span></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.VenueName + ", " + item.CityName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td colspan='2'>");
                sb.Append("");
                sb.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Enter through: &nbsp;<strong>" + item.GateName + " Gate</strong> </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Gates Open At <strong>" + item.GateOpenTime + "</strong> (Local)</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");

                StringBuilder sbMatchDay = new StringBuilder();

                if (item.EventDeatilId.ToString() == "554529" || item.EventDeatilId.ToString() == "554531" || item.EventDeatilId.ToString() == "554532")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("ODI <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else if (item.EventDeatilId.ToString() == "554513" || item.EventDeatilId.ToString() == "554514" || item.EventDeatilId.ToString() == "554515")
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:5px; font-weight:bold'>");
                    sbMatchDay.Append("T20I <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchNo + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }
                else
                {
                    sbMatchDay.Append("<td rowspan='7' align='center' style='padding-top:25px; font-weight:bold'>");
                    sbMatchDay.Append("Day <br>");
                    sbMatchDay.Append("<p style='font-family:arial; font-size:42px; text-decoration:none; color:#ffffff; background-color:#000000; text-align:center; margin:0; margin-top:2px; padding:2px; width:50px'><strong>" + item.MatchDay + "</strong></p>");
                    sbMatchDay.Append("</td>");
                }

                if (item.VenueId.ToString() == "58" || item.VenueId.ToString() == "62")
                {
                    if (item.IsSeatSelection)
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td>Row: &nbsp;<strong>" + item.RowNumber + " </strong> &nbsp;&nbsp;Seat:  &nbsp;<strong>" + item.TicketNumber + " </strong></td>");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                    else
                    {
                        sb.Append("<tr style='vertical-align:top'>");
                        sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr style='height:4px'>");
                        sb.Append("<td></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                        //sb.Append(sbMatchDay.ToString());
                        sb.Append("</tr>");
                        if (item.TicketTypeId == 2)
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                            sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr style='height:10px'><td></td></tr>");
                            sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                            sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                            sb.Append("</td></tr>");
                        }
                    }
                }
                else
                {
                    sb.Append("<tr style='vertical-align:top'>");
                    sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                    sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                    //sb.Append(sbMatchDay.ToString());
                    sb.Append("</tr>");
                    if (item.TicketTypeId == 2)
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                        sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style=' font-size: 17px; padding: 5px;font-weight: bold;display: inline-block;'>" + ticketType + "</span>");
                        sb.Append("</td></tr>");
                    }
                }

                sb.Append("<!--<tr style='height:15px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:normal;'>");
                sb.Append("<td> Only sponsor's products will be permitted into the stadium.<br>");
                sb.Append("See www.cplt20.com/T&amp;Cs. Ambush marketing not permitted</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<!--<tr style='height:3px'>");
                sb.Append("</tr> ");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td> CPL Partners: Hero * Digicel * Guardian * El Dorado</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial; font-weight:bold; line-height:1.4'>This is Your Ticket</span><br>");
                sb.Append("Keep this page with you at all times<br>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial;font-weight:bold; line-height:1.4'>Do Not Duplicate</span>");
                sb.Append("</td>");
                sb.Append("<td style='width:28%; text-align:center'>");
                sb.Append("<img style='width:80px; height:80px' src='" + qrcodeImage + "'> ");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial;'>");
                sb.Append("" + item.TicketCategoryName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br>");
                sb.Append("Ticket " + TicketNumber + "/" + TotalTic + "<br> ");
                //sb.Append("Regenerated Ticket 1");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border:1px solid #868686; font-family:arial; font-size:11px; text-decoration:none; padding:8px;line-height:1.1; background:#ffffff;text-align:justify;'>");
                sb.Append("<span style='font-size:13px; font-weight:bold; text-transform:uppercase;'>THIS IS YOUR TICKET. DO NOT DUPLICATE. ONE ENTRY PER BARCODE</span>&nbsp; 1. This Ticket is sold subject to the Terms and Conditions set out below and those on display at CWI ticket");
                sb.Append("offices. Any person who makes use of, purchases or holds this Ticket ('the Holder') shall be deemed to have");
                sb.Append("agreed to all of the Terms and Conditions to which this Ticket is subject. The Holder, in agreeing to these");
                sb.Append("conditions, also agrees to be bound by the CWI Refund Policy. ");
                sb.Append("2. Right of admission is reserved at the");
                sb.Append("reasonable discretion of CWI. CWI reserves the right to search the Holder. Should the Holder refuse to be");
                sb.Append("searched, CWI reserves the right to refuse admission or eject him/her from the Stadium without refund. ");
                sb.Append("3.");
                sb.Append("Ambush marketing is prohibited. Ambush marketing includes (a) the unauthorised use (by the Holder or any");
                sb.Append("other person) of the Ticket as a prize or in a lottery or competition or for any other promotional, advertising or");
                sb.Append("commercial purpose; and (b) an intentional unauthorised activity which (i) associates a person with the");
                sb.Append("Series, (ii) exploits the publicity or goodwill of the Series or (iii) has the effect of diminishing the status of");
                sb.Append("Series sponsors or conferring on other persons the status of a Series sponsor. ");
                sb.Append("4. The Holder shall not (a)");
                sb.Append("dispose of or transfer the Ticket for the purpose of commercial gain nor sell the Ticket at a higher price than");
                sb.Append("its face value; or (b) purchase or obtain the Ticket from a person/entity who is not an authorised agent. Any");
                sb.Append("Ticket obtained in breach hereof shall be void and the Holder shall have no right of entry to the Stadium. ");
                sb.Append("5.");
                sb.Append("No alcohol may be brought into or taken out of the Stadium. The Holder shall only be entitled to bring in 1");
                sb.Append("bag / carrier / rucksack which must be no larger than 400mm by 300mm by 300mm (15.7' by 11.8' by 11.8')");
                sb.Append("and must be able to fit under the seat. ");
                sb.Append("6. The Holder shall not seek entry to, or view the Match from stands");
                sb.Append("or seats for which the Holder does not hold a valid Ticket. ");
                sb.Append("7. CWI reserves the right to refuse admission or");
                sb.Append("eject from the Stadium without refund any Holder who, in the reasonable opinion of CWI is in breach of");
                sb.Append("these conditions. ");
                sb.Append("8. The Holder shall not, except for non-commercial purposes, make, use, broadcast,");
                sb.Append("transmit, publish, disseminate, reproduce or circulate by any means any recordings, audio, video, data,");
                sb.Append("images, results, commentary or any other information relating to the Match. ");
                sb.Append("9. CWI shall not be liable for");
                sb.Append("the theft, loss or damage of any property of the Holder. ");
                sb.Append("10. Management reserves the right to make alteration to the times, dates and venues of Events or to substitute the seat or area to which the ticket refers with another at its reasonable discretion. Management does not guarantee that the holder will have an uninterrupted and/or uninhibited view of the Event from the seat provided. ");
                sb.Append("11. Any comments or queries should be made by writing to us at <span style='text-decoration:underline'>customerservice@zoonga.com</span> ");
                sb.Append("12. Please refer to the restricted items list and full terms and conditions on zoonga.com");
                sb.Append("<br><br>");
                sb.Append("<b>CWI REFUND POLICY </b> 1. If a day's play is limited as a result of rain or other act beyond the control of the CWI, the CWI will refund the face value of tickets purchased as follows (i) Less than 10 overs bowled in any one day (ie up to and including 9 overs, 5 balls): 100% refund (ii) In excess of 10 overs but less than 20 overs bowled in any one day (i.e. 10 overs or more up to 19 overs, 5 balls): 50% refund (iii) In excess of twenty overs bowled in any one day (i.e. 20 overs or more): no refund. 2 For Twenty20 International matches: (i) 5 overs or less bowled: 100% refund (ii) in excess of 5 overs but less than 10 overs bowled: 50% refund (iii) in excess of 10 overs bowled (i.e. 10 overs or more): no refund.");
                sb.Append(". ");
                sb.Append("2. The above does not apply: (i) to a match that is completed early in the normal course of play; or (ii) where");
                sb.Append("a reserve day for play has been allocated or a match is rescheduled and this ticket is valid for such reserve");
                sb.Append("day or rescheduled game. ");
                sb.Append("3. To receive a refund, the retained portion of the ticket stub must be presented");
                sb.Append("and surrendered to a ticket office authorised by the CWI within ten (10) working days after the completion of");
                sb.Append("the game for which the refund is being sought. Refunds will be made only for tickets purchased from CWI");
                sb.Append("ticket offices. Under no other circumstances will refunds be given. ");
                sb.Append("4. Refunds shall only be made to the");
                sb.Append("person whose name appears on the ticket. ");
                sb.Append("5. Party Stand Refunds Only the Cricket Component of the");
                sb.Append("Party Stand ticket will be refunded in accordance with the CWI Refund Policy. Specific information on this");
                sb.Append("is available upon request through the local Ticket Office.");
                sb.Append("<!-- 13. <span style='font-weight:bold'>NO REFUND, NO EXCHANGE, GAMES DATE AND TIME SUBJECT TO CHANGE WITHOUT NOTICE</span>.-->");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>&nbsp;</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='text-align:center'>");
                sb.Append("<td style='border-top:1px dashed #000000;'>");
                sb.Append("<p style='text-align:center; margin:0; font-size:18px; position:relative; top:-11px;'>✂</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:31%; font-size:12px; font-family:arial'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial'>");
                sb.Append("" + item.SponsorOrCustomerName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br><br>");
                sb.Append("" + item.BarcodeNumber + "");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td style='width:38%; font-size:12px;' align='center'; font-family:arial>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>");
                sb.Append("" + shortMatchDay + " | " + shortMatchDate + " | " + matchtime + " (Local)<br>");
                sb.Append("" + item.VenueName + ", " + item.CityName + "</span><br><br>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>Ticket " + TicketNumber + "/" + TotalTic + " &nbsp;&nbsp;</span>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:31%; font-size:12px;'>");
                sb.Append("<img style='width:80px; height:80px' src=" + qrcodeImage + ">");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</body></html>");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return sb;
        }

        private StringBuilder GeneratePrintAtHomeTOM(MatchSeatTicketInfo item, int TicketNumber, int TotalTic, long transactionId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                _amazonS3FileProvider.UploadQrCodeImage(item.BarcodeNumber, ImageType.QrCode);
                string qrcodeImage = _amazonS3FileProvider.GetImagePath(item.BarcodeNumber.ToString(), ImageType.QrCode);
                string shortMatchDay = item.EventStartTime.ToString("dddd").Substring(0, 3);
                string longMatchDay = item.EventStartTime.ToString("dddd");
                string matchDate = item.EventStartTime.ToString("MMMM dd, yyyy");
                string numMonth = item.EventStartTime.ToString("dd");
                string numDay = item.EventStartTime.ToString("MMMM").Substring(0, 3);
                string shortMatchDate = numDay + ' ' + numMonth;
                string matchtime = item.EventStartTime.ToString("HH:mm");
                string IsWheelChair = item.IsWheelChair.ToString();
                string IsCompanion = item.IsCompanion.ToString();
                StringBuilder sbTicket = new StringBuilder();
                string matchAdditionalInfo = item.MatchAdditionalInfo;
                string ticketType = string.Empty;
                string PriceWithType = string.Empty;
                string PriceWithGST = string.Empty;
                decimal TicketPrice = 0;
                decimal GST = 0;
                decimal TotalPrice = 0;
                decimal BasePrice = 0;

                #region GTS Calculation
                BasePrice = Convert.ToDecimal(item.Price) / 128 * 100;
                if (Convert.ToDecimal(BasePrice) > 500)
                {
                    GST = (Convert.ToDecimal(BasePrice) * 28 / 100);
                    TicketPrice = BasePrice;
                    TotalPrice = TicketPrice + GST;

                }
                else
                {
                    GST = 0;
                    TicketPrice = Convert.ToDecimal(item.Price);
                    TotalPrice = Convert.ToDecimal(item.Price);
                }

                #endregion


                if (item.TicketTypeId == 2)
                {
                    ticketType = "Child";
                    PriceWithGST = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                }
                else if (item.TicketTypeId == 3)
                {
                    ticketType = "Senior Citizen";
                    PriceWithType = "Complimentary";
                }
                else
                {
                    if (Convert.ToInt32(IsCompanion) > 1)
                    {
                        ticketType = "Companion";
                        PriceWithType = "" + item.CurrencyName + " 0.00";
                    }
                    else if (Convert.ToInt32(IsWheelChair) > 1)
                    {
                        ticketType = "Wheelchair";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                    else
                    {
                        ticketType = "Adult";
                        PriceWithType = "" + item.CurrencyName + " " + String.Format("{0:0.00}", item.Price) + "";
                    }
                }

                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; border:1px solid #868686 ; background:url(https://static1.zoonga.com/Images/eTickets/arena-bg.png) no-repeat top left;padding:10px'>");
                sb.Append("<table width='756px' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style ='vertical-align:top;'>");
                sb.Append("<table width='718px' cellpadding='0' cellspacing='0' border='0' align='center'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td>");
                sb.Append("<div style='width:350px; float:left;'>");
                sb.Append("<img src='https://static1.zoonga.com/Images/eTickets/etkt-z-logo.png' style='width:214px; height:77px'>");
                sb.Append("</div>");
                sb.Append("<div style='width:350px; float:right; text-align:right; font-family:arial; font-size:44px; font-weight:bold; text-decoration:none; margin-top:30px;'><i>e</i>-ticket</div> ");
                sb.Append("");
                sb.Append("<div style='clear:both'></div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:10px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top'>");
                sb.Append("<td style='border:1px solid #868686; background:url(https://static1.zoonga.com/Images/eTickets/tom-ticket-bg.jpg) no-repeat;'> ");
                sb.Append("<table width='680px' align='center' cellspacing='0' cellpadding='0' border='0' style='margin-top:3px;'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='vertical-align:top; font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                sb.Append("");
                sb.Append("<table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                sb.Append("<tbody>");
                sb.Append("<tr><td style='height:10px'></td></tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:17px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td style='width: 60%;'>");
                sb.Append(""+item.EventName+"");
                sb.Append("</td>");
                sb.Append("<td rowspan='5' style='text-align:right; width:40%;padding-right: 95px;'>");
                sb.Append("<table cellpadding='0' width='100%' cellspacing='0' border='0'>");
                sb.Append("<tbody><tr>");
                sb.Append("<td style='text-align:right; font-family:arial; font-weight:bold '>");

                if (item.TransactingOptionId == 2)
                {
                    sb.Append("Complimentary<br>");
                }
                else
                {
                    
                    sb.Append("<span style='font-size:12px; font-weight: normal; '>Ticket Price: </span>" + item.CurrencyName + " " + String.Format("{0:0.00}", TicketPrice) + "<br>");
                    sb.Append("<span style='font-size:12px; font-weight: normal; '>GST: </span>" + item.CurrencyName + " " + String.Format("{0:0.00}", GST) + "<br>");
                    sb.Append("<span style='font-size:12px; font-weight: normal; '>Total Amount: </span>" + item.CurrencyName + " " + String.Format("{0:0.00}", TotalPrice) + "<br>");
                  
                    //sb.Append("<span style='vertical-align:top; font-family:arial; font-size:11px; text-decoration:none; font-weight:normal'>(Incl. of all taxes)</span><br> ");
                }
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td>" + item.EventDetailsName + " </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");

                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td>" + shortMatchDay + " | " + matchDate + " | " + matchtime + " <span style='font-family:arial; font-weight:normal;'> (Local)</span></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:12px; text-decoration:none; font-weight:bold;'>");
                //sb.Append("<td>" + item.VenueName + ", " + item.CityName + "</td>");
                sb.Append("<td>Shree Chhatrapati Shivaji Sports Complex<br />Balewadi, Mhalunge, Pune</td>");
                
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
                sb.Append("<tr style='font-family:arial; font-size:13px; font-weight:bold; text-decoration:none;'>");
                sb.Append("<td colspan='2'>");
                sb.Append("");
                sb.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Enter through: &nbsp;<strong>" + item.GateName + "</strong> </td>");
                sb.Append("</tr>");
                sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                sb.Append("<td>Gates Open At <strong>" + item.GateOpenTime + "</strong> (Local)</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='height:4px'>");
                sb.Append("<td></td>");
                sb.Append("</tr>");


                if (item.IsSeatSelection)
                {
                    sb.Append("<tr style='vertical-align:top'>");
                    sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='vertical-align:top; font-family:arial; font-size:13px; font-weight:normal; text-decoration:none;'>");
                    sb.Append("<td>Row:&nbsp;<strong>" + item.RowNumber + " </strong> &nbsp;&nbsp;Seat:&nbsp;<strong>" + item.TicketNumber + " </strong></td>");
                    //sb.Append(sbMatchDay.ToString());
                    sb.Append("</tr>");
                    if (item.TicketTypeId == 2)
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                        sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style=' font-size: 17px; font-weight: bold;display: inline-block;'>&nbsp;</span>");
                        sb.Append("</td></tr>");
                    }
                }
                else
                {
                    sb.Append("<tr style='vertical-align:top'>");
                    sb.Append("<td style='padding-top: 10px;'><strong>" + item.TicketCategoryName + "</strong></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='height:4px'>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='font-family:arial; font-size:11px; font-weight:normal; text-decoration:none;'>");
                    sb.Append("<td><img style='vertical-align:middle' src='https://static1.zoonga.com/Images/eTickets/images/drop.png'> Unreserved seating within block </td><!--<td style='font-size:13px; font-weight:bold'>Row: A &nbsp;&nbsp;Seat: 1</td>-->");
                    //sb.Append(sbMatchDay.ToString());
                    sb.Append("</tr>");
                    if (item.TicketTypeId == 2)
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style='margin-right:5px; font-size: 32px;background: #000000;color: #ffffff;padding: 5px 20px;font-weight: bold;display: inline-block;'> " + ticketType + "<div> </div></span>");
                        sb.Append("<span style='display: inline-block;position: relative;bottom: 4px;font-weight: bold;'> 12 years and under </ span >");
                        sb.Append("</td></tr>");
                    }
                    else
                    {
                        sb.Append("<tr style='height:10px'><td></td></tr>");
                        sb.Append("<tr style='font-family:arial; font-weight:normal; text-decoration:none;'>");
                        sb.Append("<td> <span style=' font-size: 17px; font-weight: bold;display: inline-block;'>&nbsp;</span>");
                        sb.Append("</td></tr>");
                    }
                }

                sb.Append("<tr style='height:10px'><td></td></tr>");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<!--<tr style='height:3px'>");
                sb.Append("</tr> ");
                sb.Append("<tr style='font-family:arial; font-size:10px; text-decoration:none; font-weight:bold;'>");
                sb.Append("<td> CPL Partners: Hero * Digicel * Guardian * El Dorado</td>");
                sb.Append("</tr>-->");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial; font-weight:bold; line-height:1.4'>This is Your Ticket</span><br>");
                sb.Append("Keep this page with you at all times<br>");
                sb.Append("<span style='text-transform:uppercase; font-family:arial;font-weight:bold; line-height:1.4'>Do Not Duplicate</span>");
                sb.Append("</td>");
                sb.Append("<td style='width:28%; text-align:center'>");
                sb.Append("<img style='width:80px; height:80px' src='" + qrcodeImage + "'> ");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:36%; font-size:12px; font-family:arial;'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial;'>");
                sb.Append("" + item.TicketCategoryName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br>");
                sb.Append("Ticket " + TicketNumber + "/" + TotalTic + "<br> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='height:8px;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border:1px solid #868686; font-family:arial; font-size:11px; text-decoration:none; padding:8px;line-height:1.1; background:#ffffff;text-align:justify;'>");
                sb.Append("<span style='font-size:13px; font-weight:bold; text-transform:uppercase;'>THIS IS YOUR TICKET. DO NOT DUPLICATE. ONE ENTRY PER BARCODE</span>&nbsp;    1. No ticket holder or spectator may : (a) continually collect, disseminate, transmit, publish or release from the grounds of the Tournament any match scores or related statistical data during match play (from the commencement of a match through its conclusion for any commercial, betting or gambling purpose); and (b) film, photograph, broadcast, stream, publish, transmit and/or otherwise offer to the public (or assist any third party in offering to the public), on a live or on a delayed basis, in whole or in part, and whether on a free basis or subject to payment, any sound recording, photograph, video footage, motion picture, film and/or other audio-visual content captured by any means whatsoever inside the Tournament site (except as is allowed in the Tournament Accreditation Policy). The continual use of laptop computers or other handheld electronic devices within the confines (spectator area) of the tournament match courts is prohibited. The exception to this provision is properly credentialed media, tournament vendors and tournament staff when used in the performance of their duties. 2. Entry on this ticket is valid only for one person unless specified. 3. Tickets once purchased are non-transferable. 4. The consumption and sale of illegal substances is strictly prohibited. 5. The rights of admission are reserved with the organizers. The organizers reserve the right to frisk the ticket holders at the entry point for security reasons. Your cooperation is solicited. 6. No drugs, alcohol, cigarettes or lighters shall be allowed inside the venue. The venue is a no-smoking area. 7. No food or beverage from outside is allowed. 8. Cameras, handbags, backpacks, bottles, cans or tins are not allowed inside the venue. 9. The organizers are not liable for any type of injury suffered by the ticket holder while attending the event. 10. The organizers, the venue and ticketing company shall not be held liable for any difficulties caused by unauthorized copy or reproduction of this ticket. 11. All pagers and cell phones from outside must be switched off. 12. Entry will not be allowed for those holding a tampered ticket. 13. The ticket is issued in accordance to the rules and regulations of the event. 14. The starting time of the event may change without prior intimation. 15. The organizers, the venue or ticketing company shall not be held liable on cancellation of the event due to calamities/or as directed by the Government Authorities. Subject to force Majeure. 16. Tournament reserves the right to restrict entry.");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>&nbsp;</td>");
                sb.Append("</tr>");
                sb.Append("<tr style='text-align:center'>");
                sb.Append("<td style='border-top:1px dashed #000000;'>");
                sb.Append("<p style='text-align:center; margin:0; font-size:18px; position:relative; top:-11px;'>✂</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("");
                sb.Append("<tr>");
                sb.Append("<td style='background:#ffffff; vertical-align:top'>");
                sb.Append("");
                sb.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='width:31%; font-size:12px; font-family:arial'>");
                sb.Append("<span style='font-weight:bold; line-height:2; font-family:arial'>");
                sb.Append("" + item.SponsorOrCustomerName + "</span><br>");
                sb.Append("Z-" + transactionId + "<br><br>");
                sb.Append("" + item.BarcodeNumber + "");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td style='width:38%; font-size:12px;' align='center'; font-family:arial>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>");
                sb.Append("" + shortMatchDay + " | " + shortMatchDate + " | " + matchtime + " (Local)<br>");
                sb.Append("" + item.VenueName + ", " + item.CityName + "</span><br><br>");
                sb.Append("<span style='font-weight:bold; font-family:arial'>Ticket " + TicketNumber + "/" + TotalTic + " &nbsp;&nbsp;</span>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("<td align='right' style='width:31%; font-size:12px;'>");
                sb.Append("<img style='width:80px; height:80px' src=" + qrcodeImage + ">");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody></table>");
                sb.Append("</body></html>");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            return sb;
        }
    }
}
