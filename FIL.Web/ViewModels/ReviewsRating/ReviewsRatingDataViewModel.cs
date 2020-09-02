using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Web.Feel.ViewModels.ReviewsRating
{
    public class ReviewsRatingDataViewModel
    {
        public string UserAltId { get; set; }
        public string EventAltId { get; set; }
        public short Points { get; set; }
        public string Comment { get; set; }
        public bool IsEnabled { get; set; }
    }
}
