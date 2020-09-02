using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Reporting
{
    public class FeelUserQueryHandler : IQueryHandler<FeelUserQuery, FeelUserQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIPDetailRepository _iPDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IFeelUserAdditionalDetailRepository _feelUserAdditionalDetailRepository;
        private readonly IEventRepository _eventRepository;

        public FeelUserQueryHandler(
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            IEventRepository eventRepository,
            IFeelUserAdditionalDetailRepository feelUserAdditionalDetailRepository,
            IIPDetailRepository iPDetailRepository)
        {
            _userRepository = userRepository;
            _iPDetailRepository = iPDetailRepository;
            _transactionRepository = transactionRepository;
            _feelUserAdditionalDetailRepository = feelUserAdditionalDetailRepository;
            _eventRepository = eventRepository;
        }

        public FeelUserQueryResult Handle(FeelUserQuery query)
        {
            List<FIL.Contracts.Models.Report.FeelUserReport> feelUserReports = new List<Contracts.Models.Report.FeelUserReport>();
            try
            {
                var users = _userRepository.GetAllByChannel(Channels.Feel).OrderByDescending(s => s.CreatedUtc);
                var ipIds = users.Select(s => s.IPDetailId).ToList();
                var userIpDetails = _iPDetailRepository.GetAllByIds(ipIds.Where(x => x != null).Take(2000).Select(x => x.Value).ToList());
                List<FIL.Contracts.DataModels.Transaction> transactions1 = new List<Contracts.DataModels.Transaction>();
                List<FIL.Contracts.DataModels.FeelUserAdditionalDetail> feelUserAdditionalDetails1 = new List<Contracts.DataModels.FeelUserAdditionalDetail>();
                List<FIL.Contracts.DataModels.Event> Events = new List<Contracts.DataModels.Event>();
                for (var i = 0; i < users.Count();)
                {
                    var user = users.Skip(i).Take(2000);
                    var transactions = _transactionRepository.GetSuccessFullTransactionByEmailIds(user.Select(s => s.Email).ToList());
                    var feelUserAdditionalDetails = _feelUserAdditionalDetailRepository.GetByUserIds(user.Select(s => s.Id).ToList());
                    var events = _eventRepository.GetAllByCreatorAltIds(user.Select(s => s.AltId).ToList());
                    transactions1.AddRange(transactions);
                    feelUserAdditionalDetails1.AddRange(feelUserAdditionalDetails);
                    Events.AddRange(events);
                    i = i + 2000;
                }

                foreach (var currentUser in users)
                {
                    FIL.Contracts.Models.Report.FeelUserReport feelUserReport = new FIL.Contracts.Models.Report.FeelUserReport();
                    var userIpDetail = userIpDetails.ToList().Where(s => s.Id == currentUser.IPDetailId).FirstOrDefault();
                    feelUserReport.CreatedDate = currentUser.CreatedUtc;
                    feelUserReport.FirstName = currentUser.FirstName;
                    feelUserReport.LastName = currentUser.LastName;
                    feelUserReport.Email = currentUser.Email;
                    feelUserReport.PhoneCode = currentUser.PhoneCode != null ? currentUser.PhoneCode : "";
                    feelUserReport.PhoneNumber = currentUser.PhoneNumber != null ? currentUser.PhoneNumber : "";
                    feelUserReport.SignUpMethod = currentUser.SignUpMethodId != null ? currentUser.SignUpMethodId.ToString() : "Regular";
                    feelUserReport.IsTransacted = transactions1.Where(s => s.EmailId == currentUser.Email).Any() ? true : false;
                    feelUserReport.IsCreator = Events.Where(s => s.CreatedBy == currentUser.AltId).Any() ? "Yes" : "No";
                    feelUserReport.IsOptIn = feelUserAdditionalDetails1.Where(s => s.UserId == currentUser.Id).Any() ? feelUserAdditionalDetails1.Where(s => s.UserId == currentUser.Id).FirstOrDefault().OptedForMailer ? "Yes" : "No" : "No";
                    if (userIpDetail != null)
                    {
                        feelUserReport.IPAddress = userIpDetail.IPAddress;
                        feelUserReport.IPCity = userIpDetail.City;
                        feelUserReport.IPCountry = userIpDetail.CountryName;
                        feelUserReport.IPState = userIpDetail.RegionName;
                    }
                    else
                    {
                        feelUserReport.IPAddress = "";
                        feelUserReport.IPCity = "";
                        feelUserReport.IPCountry = "";
                        feelUserReport.IPState = "";
                    }
                    feelUserReports.Add(feelUserReport);
                }
                return new FeelUserQueryResult
                {
                    FeelUsers = feelUserReports
                };
            }
            catch (System.Exception ex)
            {
                return new FeelUserQueryResult
                {
                    FeelUsers = feelUserReports
                };
            }
        }
    }
}