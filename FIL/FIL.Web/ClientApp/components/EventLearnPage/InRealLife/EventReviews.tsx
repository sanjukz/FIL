import * as React from "React";
import ReactStars from 'react-stars';
import { gets3BaseUrl } from "../../../utils/imageCdn";

export default class EventReviews extends React.Component<any, any> {
    constructor(props) {
        super(props);
    }

    render() {
        var months = ["January", "Feburary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        let userReview = this.props.rating.filter(rate => rate.comment.toLowerCase().indexOf(this.props.search.toLowerCase()) > -1).map((item, index) => {
            let user = this.props.user.filter((user) => { return user.id === item.userId });
            // let userImage = this.props.UserImageMap.filter((user) => { return user.id === item.userId });
            return (
                <div
                    key={index}
                    className="col-sm-12 pt-3 mt-3 border-top user-reviews">
                    <img
                        src={`${gets3BaseUrl()}/user-profile/fee-review-user-icon.png`}
                        className="rounded-circle"
                        alt="Profile Picture" />
                    <h6 className="text-capitalize">{user[0].firstName[0].toUpperCase() + "" + user[0].firstName.substring(1, user[0].firstName.length) + " " + (user[0].lastName ? user[0].lastName[0].toUpperCase() + "" + user[0].lastName.substring(1, user[0].lastName.length) : "")}</h6>
                    <p>
                        <div className="rating">
                            <ReactStars className="rating d-inline-block" color2={"#572483"} edit={false} value={item.points} /> <br />
                            <span className="review-time"> {months[new Date(item.createdUtc).getMonth()]} {new Date(item.createdUtc).getFullYear()} </span> <br />
                            <div className="feel-comment">{item.comment}</div>
                        </div>
                    </p>
                </div>);
        });

        return (<div className={`${this.props.isOnlineExperience ? 'review-sec' : 'col-sm-8'}`}>
            {
                userReview.length > 0 ? userReview : (
                    <div className="col-sm-12 pt-3 mt-3 border-top user-reviews pl-0">
                        No Reviews found for this filter. Please try changing the filters.
                    </div>
                )
            }
        </div >);
    }
}