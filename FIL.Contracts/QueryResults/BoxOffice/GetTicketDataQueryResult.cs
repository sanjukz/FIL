using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class GetTicketDataQueryResult
    {
        public FIL.Contracts.Models.Transaction Transaction { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public MatchSeatTicketDetail MatchSeatTicketDetail { get; set; }
        public EventDetail EventDetail { get; set; }
        public TicketCategory TicketCategory { get; set; }
        public List<CurrencyType> CurrencyType { get; set; }
        public bool IsValid { get; set; }
        public string errorMessage { get; set; }
        public List<FIL.Contracts.DataModels.BoCustomerDetail> BoCustomerDetails { get; set; }
    }
}