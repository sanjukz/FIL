import * as React from "react";
import ReactStars from 'react-stars';
import { Link } from "react-router-dom";
import { Rate } from 'antd';

export default class EventRatingsV1 extends React.PureComponent<any, any> {
    public render() {
        var rating = this.props.rating;
        var reviewAvg = 0;
        var ratingLength = 0;
        if (!this.props.isInnerPage && rating !== undefined) {
            var reviewLength = 1;
            if (rating.length !== 0) {
                reviewLength = rating.filter(function (item) { return item.points > 0 }).length;
            }
            reviewAvg = rating.length == 0 ? 0 : rating.map((item) => { return item.points }).reduce((a, b) => { return a + b }) / reviewLength;
            ratingLength = rating.length;
        } else if (this.props.isInnerPage) {
            reviewAvg = this.props.rating;
            ratingLength = this.props.rating;
        }
        else {
            ratingLength = 0;
        }

        return <div className="tils-text">
            Ratings:<span className="star-rating">
                <Rate value={reviewAvg} disabled={true} style={{ color: "#572483" }} />
            </span>| {ratingLength}+ Reviews
           </div>
    }
}
