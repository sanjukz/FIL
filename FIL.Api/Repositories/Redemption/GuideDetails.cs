using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories.Redemption
{
    public interface IGuideDetailsRepository : IOrmRepository<Redemption_GuideDetails, Redemption_GuideDetails>
    {
        Redemption_GuideDetails Get(long Id);

        IEnumerable<Redemption_GuideDetails> GetAllTop1000();

        Redemption_GuideDetails GetByUserId(long UserId);

        List<GuideDetailsCustom> GetRecent100Guides();

        List<GuideDetailsCustom> GetRecent100GuidesByStatus(int OrderStatusId);

        IEnumerable<GuideOrderDetailsCustomModel> GetRecent100GuideOrders(long userId, int OrderStatusId, int roleId);

        IEnumerable<GuideOrderDetailsCustomModel> GetRecent100PendingGuideOrders(long userId, int OrderStatusId, int roleId);
    }

    public class GuideDetailsRepository : BaseLongOrmRepository<Redemption_GuideDetails>, IGuideDetailsRepository
    {
        public GuideDetailsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Redemption_GuideDetails Get(long Id)
        {
            return Get(new Redemption_GuideDetails { Id = Id });
        }

        public Redemption_GuideDetails GetByUserId(long userId)
        {
            return GetAll(s => s.Where($"{nameof(Redemption_GuideDetails.UserId):C}=@UserId")
                .WithParameters(new { UserId = userId })
            ).FirstOrDefault();
        }

        public IEnumerable<Redemption_GuideDetails> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<Redemption_GuideDetails> GetAllTop1000()
        {
            return GetAll(null).OrderByDescending(p => p.Id).Take(100);
        }

        public string getGuideScript()
        {
            return "SELECT TOP (500) rgd.Id, rgd.UserId as UserId, " +
                "uad.AddressLine1 as VenueName, ct.Name as CityName, " +
                "st.Name as StateName, cnt.CountryName as CountryName," +
                "rgd.LanguageId as LanguageId, appst.ApproveStatus as ApproveStatusId , rgd.ApprovedBy," +
                "rgd.ApprovedUtc, rgd.IsEnabled, rgd.CreatedUtc, rgd.UpdatedUtc, rgd.CreatedBy, rgd.UpdatedBy," +
                "users.FirstName, users.LastName, users.PhoneCode, users.AltId, users.PhoneNumber, users.email, e.Name as PlaceName, " +
                "mfd.AccountNumber, mfd.AccountTypeId as AccountType, mfd.BankAccountTypeId as BankAccountType," +
                "mfd.BankName, mfd.BranchCode, " +
                "crt.Code as CurrencyName, mfd.RoutingNumber, mfd.TaxId as TaxId FROM Redemption_GuideDetails rgd " +
                "INNER JOIN Redemption_GuideFinanceMappings gfm With(NOLOCK) on rgd.id = gfm.GuideId " +
                "INNER JOIN ApproveStatuses appst with (NOLOCK) on rgd.ApproveStatusId = appst.Id  " +
                "INNER JOIN Redemption_GuidePlaceMappings gpm With(NOLOCK) on gpm.GuideId = rgd.Id  " +
                "INNER JOIN events e With(NOLOCK) on e.id = gpm.eventid " +
                "Inner JOIN Users users With(NOLOCK) on rgd.UserId = users.Id  " +
                "Inner JOIN MasterFinanceDetails mfd With(NOLOCK) on mfd.Id = gfm.MasterFinanceDetailId " +
                "Inner JOIN AccountTypes acctyp With(NOLOCK) on acctyp.Id = mfd.AccountTypeId " +
                "Inner JOIN BankAccountTypes batype With(NOLOCK) on batype.Id = mfd.BankAccountTypeId " +
                "Inner JOIN CurrencyTypes crt With(NOLOCK) on crt.Id = mfd.CurrenyId " +
                "Inner JOIN useraddressdetails uad With(NOLOCK) on uad.Id = rgd.UserAddressDetailId  " +
                "Inner JOIN Cities ct With(NOLOCK) on ct.Id = uad.CityId " +
                "Inner JOIN States st With(NOLOCK) on st.Id = ct.StateId " +
                "Inner JOIN Countries cnt With(NOLOCK) on cnt.Id = st.CountryId ";
        }

        public List<GuideDetailsCustom> GetRecent100Guides()
        {
            return GetCurrentConnection().Query<GuideDetailsCustom>(getGuideScript() + " order by 1 desc").ToList();
        }

        public List<GuideDetailsCustom> GetRecent100GuidesByStatus(int OrderStatusId)
        {
            return GetCurrentConnection().Query<GuideDetailsCustom>(getGuideScript() +
               " where rgd.approveStatusId= " + OrderStatusId + " order by rgd.id desc").ToList();
        }

        public string getScript()
        {
            var script = "SELECT DISTINCT TOP 500 t.id AS TransactionId, " +
                         " ee.NAME AS PlaceName, " +
                        "  c.NAME AS PlaceCity, " +
                       "   s.NAME AS PlaceState, " +
                        "  cc.NAME AS PlaceCountry, " +
                        "  td.visitdate AS VisitDate, " +
                        "  tc.NAME AS TicketCategory, " +
                        "  tc.id AS TicketCategoryId, " +
                       "   eta.id AS EventTicketAttribueId, " +
                       "   appst.approvestatus AS OrderStatus, " +
                       "   rod.ApprovedBy AS OrderApprovedBy, " +
                        "  t.firstname AS CustomerFirstName, " +
                        "  t.lastname AS CustomerLastName, " +
                        "  rod.ApprovedUtc AS OrderApprovedDate, " +
                       "   rod.OrderCompletedDate AS OrderCompletedDate, " +
                       "   ct.code AS Currency, " +
                        "  td.pricePerTicket AS TicketPrice, " +
                       "   usr.firstName as GuideFirstName," +
                       "   usr.LastName as GuideLastName," +
                       "   t.createdutc" +
  " FROM   transactiondetails td WITH(nolock) " +
         " INNER JOIN transactions t WITH(nolock)" +
             "    ON t.id = td.transactionid" +
         " INNER JOIN CurrencyTypes ct WITH(nolock)" +
        "         ON ct.id = t.currencyId" +
        " INNER JOIN eventticketattributes eta WITH(nolock)" +
           "      ON eta.id = td.eventticketattributeid" +
        " INNER JOIN eventticketdetails etd WITH(nolock)" +
        "         ON etd.id = eta.eventticketdetailid" +
       "  INNER JOIN ticketcategories tc WITH(nolock)" +
         "        ON tc.id = etd.ticketcategoryid" +
         " INNER JOIN eventdetails ed WITH(nolock)" +
         "        ON ed.id = etd.eventdetailid" +
        " INNER JOIN events ee WITH(nolock)" +
            "     ON ee.id = ed.eventid" +
        " INNER JOIN redemption_guideplacemappings rgpm" +
            "     ON rgpm.eventid = ee.id" +
       "  INNER JOIN redemption_orderdetails(nolock) rod" +
            "    ON rod.transactionid = t.id" +
        " INNER JOIN approvestatuses appst WITH(nolock)" +
            "     ON rod.orderstatusid = appst.id" +
        " INNER JOIN venues v WITH(nolock)" +
             "    ON v.id = ed.venueid" +
        " INNER JOIN cities c WITH(nolock)" +
           "      ON c.id = v.cityid" +
        " INNER JOIN states s WITH(nolock)" +
           "      ON s.id = c.stateid" +
       "  INNER JOIN countries cc WITH(nolock)" +
            "     ON cc.id = s.countryid";
            return script;
        }

        public IEnumerable<GuideOrderDetailsCustomModel> GetRecent100GuideOrders(long userId,
            int OrderStatusId,
            int roleId
            )
        {
            return GetCurrentConnection().Query<GuideOrderDetailsCustomModel>(getScript() +
                "  INNER JOIN users usr" +
                " ON usr.altid = rod.approvedby " +
                " where " + "rod.OrderStatusId= " +
                OrderStatusId.ToString() + (roleId == 10 ? "" : "and usr.Id =" + userId.ToString()) + " order by t.id desc");
        }

        public IEnumerable<GuideOrderDetailsCustomModel> GetRecent100PendingGuideOrders(long userId,
            int OrderStatusId,
            int roleId)
        {
            return GetCurrentConnection().Query<GuideOrderDetailsCustomModel>(getScript() +
                " INNER JOIN Redemption_GuideDetails RGD WITH(nolock)" +
                " ON RGD.Id = rgpm.guideId" +
                "  INNER JOIN users usr" +
                " ON usr.Id = RGD.userId " +
                " where rod.OrderStatusId = " +
                OrderStatusId.ToString() + (roleId == 10 ? "" : " AND usr.Id =" + userId.ToString()) + " order by t.id desc");
        }
    }
}