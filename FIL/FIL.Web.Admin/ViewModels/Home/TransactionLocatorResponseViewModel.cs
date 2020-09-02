using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.Home
{
    public class TransactionLocatorResponseViewModel
    {
        public IEnumerable<TransactionInfo> transactionInfos { get; set;}
    }
}
