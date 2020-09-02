using FIL.Contracts.Commands.Feel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.PrintPAH
{
	public class AssignBarcodeResponseViewModel
	{
		public long TransactionId { get; set; }
		public bool Success { get; set; }
		public List<PAHDetail> PahDetail { get; set; }
	}
}
