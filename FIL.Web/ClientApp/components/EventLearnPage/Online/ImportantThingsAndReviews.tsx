import * as React from "react";
import ReactStars from "react-stars";
import { gets3BaseUrl } from "../../../utils/imageCdn";
import { EventLearnPageViewModel } from "../../../models/EventLearnPageViewModel";
import EventReviews from "../InRealLife/EventReviews";
import { sortByDate, filterByRatings } from "../../../utils/ShuffleArray";

interface Iprops {
  eventData: EventLearnPageViewModel;
}
class ImportantThingsAndReviews extends React.PureComponent<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      newReview: "",
      newRating: "",
      s3BaseUrl: gets3BaseUrl(),
      ratingFilterValue: [],
      search: "",
    };
  }
  ratingChanged = (newRating) => {
    this.setState({
      newRating: newRating,
    });
  };
  handleInputChange = (event) => {
    this.setState({
      ...this.state,
      [event.target.name]: event.target.value,
    });
  };

  onReviewSubmit = () => {
    if (this.state.newRating == "") {
      alert("Please give a rating to this place");
      return;
    }
    this.props.onSubmit(this.state.newReview, this.state.newRating);
  };
  sortReviews = () => {
    let ratings = [...this.props.rating];
    let sortedRatings = sortByDate(ratings, this.state.reviewFilterValue);
    let filteredRatings = filterByRatings(
      sortedRatings,
      this.state.ratingFilterValue
    );
    return filteredRatings;
  };
  render() {
    let ratings = this.sortReviews();
    return (
      <>
        {this.props.isLiveStream ? (
          <section className="exp-sec-pad">
            <div className="container">
              <div className="card-deck">
                <div className="card">
                  <h3 className="text-purple">
                    Important things to keep in mind
                  </h3>
                </div>
                <div className="card fil-exp-list">
                  <h4>Technical requirements</h4>
                  <p>
                    You’ll need an internet connection and the ability to stream
                    audio and video to enjoy your experience. We recommend an
                    internet speed of 50MBPS, and viewing the experience or
                    event on a computer, for an optimal experience. For
                    Backstage Pass tickets, we recommend having access to a
                    keyboard, for chatting. Your unique link to attend will be
                    included in your booking confirmation email. Please check
                    your spam or other folders and tabs for the email in case
                    you don't see it in your inbox.
                  </p>
                  <h4 className="mt-sm-5">Cancellation policy</h4>
                  <p>
                    Experiences and events, once purchased are not cancellable
                    or refundable. For more details refer to our{" "}
                    <a href="/terms">terms and conditions</a> .
                  </p>
                </div>
              </div>
            </div>
          </section>
        ) : null}

        <section className="exp-sec-pad fil-exp-page-review-sec">
          <div className="container">
            <div className="card-deck">
              <div className="card">
                <h3 className="text-purple">Reviews</h3>
              </div>
              <div className="card fil-exp-list">
                <p>
                  {this.props.rating.length > 0
                    ? "Write a review."
                    : "Be the first to write a review."}
                  <div className="float-right">
                    <ReactStars
                      className="d-inline-block"
                      color2={"#572483"}
                      style={{ lineHeight: "10px" }}
                      size="18"
                      half={false}
                      value={this.state.newRating}
                      onChange={this.ratingChanged}
                    />
                  </div>
                </p>
                <div className="media">
                  <img
                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/fil-review-avatar.svg"
                    className="align-self-start mr-4 rounded-circle"
                    alt="fil Review"
                    width="50"
                    height="50"
                  />
                  <div className="media-body">
                    <div className="form-group">
                      <textarea
                        name="newReview"
                        className="form-control"
                        onChange={this.handleInputChange}
                        id="exampleFormControlTextarea1"
                        rows={4}
                      />
                    </div>
                    <div className="text-right">
                      <button
                        type="submit"
                        onClick={this.onReviewSubmit}
                        className="btn btn-primary btn-sm"
                      >
                        Submit{" "}
                        <img
                          className="ml-1 mt-0"
                          src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/arrow-right.svg"
                          width="3"
                        />
                      </button>
                    </div>
                  </div>
                </div>

                {this.props.rating.length > 0 && (
                  <EventReviews
                    {...this.props}
                    search={this.state.search}
                    rating={ratings}
                    isOnlineExperience={true}
                  />
                )}
              </div>
            </div>
          </div>
        </section>
      </>
    );
  }
}
export default ImportantThingsAndReviews;
