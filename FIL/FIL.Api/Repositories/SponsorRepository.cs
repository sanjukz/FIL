using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ISponsorRepository : IOrmRepository<Sponsor, Sponsor>
    {
        Sponsor Get(long id);

        IEnumerable<Sponsor> GetByIds(IEnumerable<long> ids);

        IEnumerable<Sponsor> GetByUserName(string username);

        Sponsor GetBySponsorName(string sponsorName);

        IEnumerable<Sponsor> GetByEventDetailId(long eventDetailId);

        IEnumerable<Sponsor> GetFilteredSponsor(long eventDetailId);

        IEnumerable<FIL.Contracts.Models.TMS.SponsorTicketDetail> GetSponsorTicketDetails(long transactionId);

        IEnumerable<Sponsor> GetAllByDateSponsors(DateTime fromDate);

        IEnumerable<Sponsor> GetSponsorByEventDetailIds(IEnumerable<long> eventDetailId);

        IEnumerable<Sponsor> GetFilteredSponsorByEventDetailIds(IEnumerable<long> eventDetailId);

        IEnumerable<Sponsor> GetSponsorByEventDetailId(long eventDetailId);

        IEnumerable<Sponsor> GetAll();
    }

    public class SponsorsRepository : BaseLongOrmRepository<Sponsor>, ISponsorRepository
    {
        public SponsorsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Sponsor Get(long id)
        {
            return Get(new Sponsor { Id = id });
        }

        public IEnumerable<Sponsor> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<Sponsor> GetByIds(IEnumerable<long> ids)
        {
            return GetAll(s => s.Where($"{nameof(Sponsor.Id):c} IN @Ids")
            .WithParameters(new { Ids = ids }));
        }

        public IEnumerable<Sponsor> GetByUserName(string username)
        {
            return GetAll(s => s.Where($"{nameof(Sponsor.Username):C} = @EmailId AND Password != '0'")
                    .WithParameters(new { EmailId = username })
           );
        }

        public Sponsor GetBySponsorName(string sponsorName)
        {
            return GetAll(s => s.Where($"{nameof(Sponsor.SponsorName):C} = @SponsorName")
                    .WithParameters(new { SponsorName = sponsorName })
           ).FirstOrDefault();
        }

        public IEnumerable<Sponsor> GetAllByDateSponsors(DateTime fromDate)
        {
            var companyList = GetAll(s => s.Where($"{nameof(Sponsor.CreatedUtc):C} > @CreatedUtc")
                           .WithParameters(new { CreatedUtc = fromDate }));
            return companyList;
        }

        public IEnumerable<Sponsor> GetSponsorByEventDetailIds(IEnumerable<long> eventDetailId)
        {
            return GetCurrentConnection().Query<Sponsor>("SELECT S.* FROM Sponsors S  WITH(NOLOCK) " +
                                                       "WHERE S.Id IN ( SELECT DISTINCT ESM.SponsorId from EventSponsorMappings ESM WITH(NOLOCK) " +
                                                       "WHERE ESM.EventDetailId IN @EventDetailId ) " +
                                                       "AND S.IsEnabled=1 AND S.SponsorName <> '' ORDER BY S.SponsorName ASC ", new
                                                       {
                                                           EventDetailId = eventDetailId,
                                                       });
        }

        public IEnumerable<Sponsor> GetFilteredSponsorByEventDetailIds(IEnumerable<long> eventDetailId)
        {
            return GetCurrentConnection().Query<Sponsor>("SELECT S.* FROM Sponsors S  WITH(NOLOCK) " +
                                                       "WHERE S.Id NOT IN ( SELECT DISTINCT ESM.SponsorId from EventSponsorMappings ESM WITH(NOLOCK) " +
                                                       "WHERE ESM.EventDetailId IN @EventDetailId ) " +
                                                       "AND S.IsEnabled=1 AND S.SponsorName <> '' ORDER BY S.SponsorName ASC ", new
                                                       {
                                                           EventDetailId = eventDetailId,
                                                       });
        }

        public IEnumerable<Sponsor> GetSponsorByEventDetailId(long eventDetailId)
        {
            return GetCurrentConnection().Query<Sponsor>("SELECT S.* FROM Sponsors S  WITH(NOLOCK) " +
                                                       "WHERE S.Id NOT IN ( SELECT DISTINCT ESM.SponsorId from EventSponsorMappings ESM WITH(NOLOCK) " +
                                                       "WHERE ESM.EventDetailId = @EventDetailId ) " +
                                                       "AND S.IsEnabled=1 AND S.SponsorName <> '' ORDER BY S.SponsorName ASC ", new
                                                       {
                                                           EventDetailId = eventDetailId,
                                                       });
        }

        public IEnumerable<Sponsor> GetByEventDetailId(long eventDetailId)
        {
            return GetCurrentConnection().Query<Sponsor>("SELECT S.* FROM Sponsors S  WITH(NOLOCK) " +
                                                       "WHERE S.Id NOT IN ( SELECT DISTINCT ESM.SponsorId from EventSponsorMappings ESM WITH(NOLOCK) " + 
                                                       "WHERE ESM.EventDetailId = @EventDetailId ) " +
                                                       "AND S.IsEnabled=1 AND S.SponsorName <> '' ORDER BY S.SponsorName ASC ", new
                                                       {
                                                           EventDetailId = eventDetailId,
                                                       });
        }

        public IEnumerable<Sponsor> GetFilteredSponsor(long eventDetailId)
        {
            return GetCurrentConnection().Query<Sponsor>("SELECT S.* FROM Sponsors S  WITH(NOLOCK) " +
                                                        "WHERE S.Id NOT IN ( SELECT DISTINCT ESM.SponsorId from EventSponsorMappings ESM WITH(NOLOCK) " +
                                                        "WHERE ESM.EventDetailId = @EventDetailId ) " +
                                                        "AND S.IsEnabled=1 AND S.SponsorName <> '' ORDER BY S.SponsorName ASC ", new
                                                        {
                                                            EventDetailId = eventDetailId,
                                                        });
        }

        public IEnumerable<FIL.Contracts.Models.TMS.SponsorTicketDetail> GetSponsorTicketDetails(long transactionId)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TMS.SponsorTicketDetail>("SELECT A.TransactionId,A.Quantity,B.TotalTickets, " +
                                                        "SerialStart,SerialEnd, TicketHandedBy,TicketHandedTo FROM HandoverSheets A WITH(NOLOCK)  " +
                                                        "INNER JOIN Transactions B WITH(NOLOCK) ON A.TransactionId =B.Id " +
                                                        "Where A.TransactionId =@TransactionId  ", new
                                                        {
                                                            TransactionId = transactionId,
                                                        });
        }
    }
}