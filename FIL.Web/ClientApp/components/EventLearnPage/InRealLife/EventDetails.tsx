import * as React from "React";

export default class EventDetails extends React.Component<any, any> {
    public render() {
        var ticketCategory = this.props.ticketCategory;
        var that = this;
        var ticketInfo = this.props.eventTicketDetail.map((item) => {
            let categoryName = that.props.ticketCategory.filter(function (val) {
                return val.id == item.ticketCategoryId
            });
            let price = that.props.eventTicketAttribute.filter(function (val) {
                return val.eventTicketDetailId == item.id
            });
            return <div><span className="font-weight-bold">{categoryName.length && categoryName[0].name}: </span> Rs. {price.length && price[0].price}  per ticket  <br /></div>
        });

        return <div className="col-sm-4 d-none">
            <h5 className="mb-4">Ticket Information</h5>
            <p>
                {ticketInfo}
            </p>
        </div>
    }
}
