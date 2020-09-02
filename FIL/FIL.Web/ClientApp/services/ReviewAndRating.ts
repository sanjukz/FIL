import axios from "axios";
import ReviewsRatingDataViewModel from "../models/ReviewsRating/ReviewsRatingViewModel";
import ReviewsRatingResponseViewModel from "../models/ReviewsRating/ReviewsRatingResponseViewModel";
import { EventLearnPageViewModel } from "../models/EventLearnPageViewModel";


function saveReviewAndRatings(reviewAndRatings) {
    return axios.post("api/reviews_rating/save", reviewAndRatings, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<ReviewsRatingResponseViewModel>)
        .catch((error) => {
            alert(error);
        });
}

function getEventLearnPageData(eventdata) {
    return axios.post("api/event/slug", eventdata, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<EventLearnPageViewModel>)
        .catch((error) => {
            alert(error);
        });
}


export const ReviewRatingservice = {
    saveReviewAndRatings,
    getEventLearnPageData
};
