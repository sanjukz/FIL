using System;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using FIL.Web.Feel.ViewModels.ReviewsRating;
using Microsoft.AspNetCore.Mvc;
using FIL.Contracts.Commands.ReviewsRating;
using FIL.Contracts.Queries.ReviewsAndRating;
using FIL.Contracts.Queries.UserOrders;
using FIL.Logging;

namespace FIL.Web.Feel.Controllers
{
    public class ReviewsRatingController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ILogger _logger;

        public ReviewsRatingController(ICommandSender commandSender, IQuerySender querySender, ILogger logger)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/reviews_rating/save")]
        public async Task<ReviewsRatingResponseViewModel> SaveReviewRating([FromBody]ReviewsRatingDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _commandSender.Send(new ReviewsRatingCommand
                    {
                        UserAltId = new Guid(model.UserAltId.ToString()),
                        EventAltId = new Guid(model.EventAltId.ToString()),
                        Points = model.Points,
                        Comment = model.Comment,
                        IsEnabled = true,
                    });
                    return new ReviewsRatingResponseViewModel { Success = true };
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new ReviewsRatingResponseViewModel { Success = false };
                }
            }
            else
            {
                return new ReviewsRatingResponseViewModel { Success = false };
            }
        }

        [HttpGet]
        [Route("api/review/activate/{altid}")]
        public async Task<ReviewsRatingResponseViewModel> ActiavteUser(Guid AltId)
        {
            try
            {
                await _commandSender.Send(new ActiveReviewCommand
                {
                    AltId = AltId
                });
                return new ReviewsRatingResponseViewModel { Success = true };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new ReviewsRatingResponseViewModel
                {
                    Success = false,
                };
            }
        }

        [HttpGet]
        [Route("api/reviewRatingValidator/{UserAltId}")]
        public async Task<ReviewRatingValidationDataViewModel> Get(Guid userAltId)
        {
            var queryResult = await _querySender.Send(new ReviewsAndRatingQuery
            {
                UserAltId = userAltId
            });
            return new ReviewRatingValidationDataViewModel
            {
                IsPurchase = queryResult.IsPurchase
            };
        }

        [HttpGet]
        [Route("api/placeReviewRatingValidator/{UserAltId}")]

        public async Task<UserOrderRespnseViewModel> GetUserData(Guid userAltId)
        {
            var queryResult = await _querySender.Send(new UserOrdersQuery
            {
                UserAltId = userAltId
            });
            return new UserOrderRespnseViewModel
            {
                Event = queryResult.Event,
                Transaction = queryResult.Transaction,
                transactionDetail = queryResult.transactionDetail,
                EventTicketAttribute = queryResult.EventTicketAttribute,
                EventTicketDetail = queryResult.EventTicketDetail,
                TicketCategory = queryResult.TicketCategory,
                TransactionPaymentDetail = queryResult.TransactionPaymentDetail,
                CurrencyType = queryResult.CurrencyType
            };
        }
    }
}
