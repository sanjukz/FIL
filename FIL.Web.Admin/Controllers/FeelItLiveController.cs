using System.Threading.Tasks;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;
using FIL.Contracts.Queries;
using FIL.Web.Kitms.Feel.ViewModels;

namespace FIL.Web.Kitms.Feel.Controllers
{
    public class FeelItLiveController : Controller
    {
        private readonly IQuerySender _querySender;

        public FeelItLiveController(IQuerySender querySender)
        {
            _querySender = querySender;
        }

        [HttpGet]
        [Route("/api/host-detail/{emailId}")]
        public async Task<FeelItLiveHostDetailViewModel> GetHostDetail(string emailId)
        {
            try
            {
                var queryResult = await _querySender.Send(new FeelItLiveHostQuery
                {
                    Email = emailId
                });

                return AutoMapper.Mapper.Map<FeelItLiveHostDetailViewModel>(queryResult);
            }
            catch
            {   
                return new FeelItLiveHostDetailViewModel();
            }
        }
    }
}