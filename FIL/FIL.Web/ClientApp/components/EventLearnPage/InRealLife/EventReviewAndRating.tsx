import * as React from "React";
import ReactStars from 'react-stars';
import { gets3BaseUrl } from "../../../utils/imageCdn";
import EventReviewFilters from './EventReviewFilters';
import EventReviews from "./EventReviews";
import { sortByDate, filterByRatings } from "../../../utils/ShuffleArray";

export default class EventReviewAndRating extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            ratingFilterValue: [],
            reviewFilterValue: "newest",
            isModalVisible: false,
            newReview: "",
            newRating: "",
            search: ""
        };
    }

    // for filtering stars
    handleRatingFilter = (event) => {
        let ratings = [...this.state.ratingFilterValue];
        if (ratings.includes(event.target.value)) {
            var index = this.state.ratingFilterValue.indexOf(event.target.value);
            ratings.splice(index, 1);
            this.setState({
                ratingFilterValue: ratings
            });
        } else {
            this.setState({
                ratingFilterValue: [...ratings, event.target.value]
            });
        }
    };

    handleReviewFilter = (event) => {
        this.setState({
            reviewFilterValue: event.target.value
        });
    };

    searchReviews = () => {
        let ratings = [...this.props.rating];
        return ratings.filter((item, index) => {
            return item.comment.toLowerCase().indexOf(this.state.search.toLowerCase()) >= 0
        });
    };

    sortReviews = () => {
        let ratings = [...this.props.rating];
        let sortedRatings = sortByDate(ratings, this.state.reviewFilterValue);
        let filteredRatings = filterByRatings(sortedRatings, this.state.ratingFilterValue);
        return filteredRatings;
    };

    handleInputChange = (event) => {
        this.setState({
            ...this.state,
            [event.target.name]: event.target.value
        });
    };

    ratingChanged = (newRating) => {
        this.setState({
            newRating: newRating,
        });
    };

    getMedianRating = () => {
        let medianRating = 0;
        this.props.rating.forEach(item => {
            medianRating + item.points;
        });
        return medianRating / this.props.rating.length;
    };

    onReviewSubmit = () => {
        if (this.state.newRating == "") {
            alert("Please give a rating to this place");
            return;
        }
        this.props.onSubmit(this.state.newReview, this.state.newRating);
    };

    render() {
        let ratings = this.sortReviews();
        return (
            <div className="pt-3 row review-sec w-100">
                <div className="col-sm-8 review-sec-head">
                    <h4 className="m-0">{this.props.event.name}</h4>
                    <div className="rating">
                        <span>
                            <ReactStars
                                className="d-inline-block"
                                color2={"#572483"}
                                style={{ lineHeight: '10px' }}
                                size="18"
                                half={false}
                                value={this.state.newRating}
                                onChange={this.ratingChanged} />
                        </span>
                    </div>
                </div>
                <div className="col-sm-4">
                    <input
                        className="form-control "
                        type="search"
                        name="search"
                        onChange={this.handleInputChange}
                        placeholder="Search reviews"
                        aria-label="Search"
                    />
                </div>
                <div className="col-sm-12 pt-3 mt-3 border-top user-reviews">
                    <img
                        src={`${gets3BaseUrl()}/user-profile/fee-review-user-icon.png`}
                        className="rounded-circle" alt="fee review user icon" />
                    <input
                        type="textarea"
                        name="newReview"
                        className="form-control comment-box "
                        onChange={this.handleInputChange}
                        placeholder="Write a review.."
                        aria-label="Search"
                        required />
                    <button
                        value="submit"
                        className="float-right btn btn-outline-primary mt-2"
                        onClick={this.onReviewSubmit}
                        type="button">
                        Submit
                        </button>
                </div>
                {/* <div className="col-sm-12 pt-3 mt-3 border-top user-reviews">
                    <img
                        src={`${gets3BaseUrl()}/user-profile/fee-review-user-icon.png`}
                        className="rounded-circle"
                        alt="feel review user icon" />
                    
                    </div> */}
                {this.props.rating.length > 0 ? <div className="row pt-3 m-0">
                    <EventReviewFilters
                        handleReviewFilter={this.handleReviewFilter}
                        handleRatingFilter={this.handleRatingFilter}
                        stateProps={this.state}
                    />
                    <EventReviews
                        {...this.props}
                        search={this.state.search}
                        rating={ratings}
                    />
                </div> : null}
            </div>
        );
    }
}