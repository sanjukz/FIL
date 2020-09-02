using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.QueryResults.Redemption;

namespace FIL.Web.Kitms.Feel.ViewModels.Redemption
{
    public class GuideEditResponseModel
    {
        public FIL.Contracts.DataModels.User User { get; set; }
        public FIL.Contracts.DataModels.UserAddressDetail UserAddressDetail { get; set; }
        public UserAddressDetailMapping UserAddressDetailMapping { get; set; }
        public FIL.Contracts.DataModels.Redemption.Redemption_GuideDetails GuideDetail { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Redemption_GuidePlaceMappings> GuidePlaceMappings { get; set; }
        public List<FIL.Contracts.Models.Event> GuidePlaces { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Redemption_GuideServices> GuideServices { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Services> Services { get; set; }
        public FIL.Contracts.DataModels.Redemption.Redemption_GuideFinanceMappings GuideFinanceMapping { get; set; }
        public FIL.Contracts.DataModels.Redemption.MasterFinanceDetails MasterFinanceDetails { get; set; }
        public List<FIL.Contracts.DataModels.Redemption.Redemption_GuideDocumentMappings> GuideDocumentMappings { get; set; }
    }
}
