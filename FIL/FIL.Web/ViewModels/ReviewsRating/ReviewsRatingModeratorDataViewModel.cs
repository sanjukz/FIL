using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.ReviewsRating
{
    public class ReviewsRatingModeratorDataViewModel
    {
        [Required]
        public string AltId { get; set; }
    }
}
