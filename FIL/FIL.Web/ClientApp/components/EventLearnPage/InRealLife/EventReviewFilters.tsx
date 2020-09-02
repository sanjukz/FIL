import * as React from "React";
import ReactStars from 'react-stars';

export default class EventReviewFilters extends React.Component<any, any> {
    constructor(props) {
        super(props);
    }

    getRatingFilterElements = () => {
        let arr = [...Array(5)];
        let ratingFilterElements = arr.map((item, index) => {
            return (<div>
                <input
                    id={"checkbox" + (arr.length - index).toString()}
                    type="checkbox"
                    name="ratingFilterValue"
                    value={arr.length - index}
                    className="d-inline-block"
                    checked={this.props.stateProps.ratingFilterValue.includes((arr.length - index).toString())}
                    onChange={this.props.handleRatingFilter} />
                <ReactStars
                    key={index}
                    color2={"#572483"}
                    className="d-inline-block ml-2"
                    edit={false}
                    value={arr.length - index}
                /></div>);
        });
        return ratingFilterElements;
    };

    public render() {
        return (
            <div className="col-sm-4 p-0 mt-3">
                <div className="p-3 bg-light rounded shadow-sm">
                    <h4>Filter Reviews & Rating</h4>
                    <div>
                        <select name="review-filter" onChange={this.props.handleReviewFilter} className="form-control my-2">
                            <option selected value="newest">Newest First</option>
                            <option value="oldest">Oldest First</option>
                        </select>
                    </div>
                    <div>
                        {this.getRatingFilterElements()}
                    </div>
                </div>
            </div>);
    }
}