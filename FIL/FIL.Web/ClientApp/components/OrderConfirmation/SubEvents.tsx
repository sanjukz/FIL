import * as React from "react";
import TicketCategories from "./TicketCategories";
import TotalPrice from "./TotalPrice";
import { gets3BaseUrl } from "../../utils/imageCdn";

export default class SubEvents extends React.Component<any, any> {

    public render() {
        var subEventcontainer = this.props.subEventContainer;
        var i = -1;
        var currency = this.props.currency;
        var subEventDetail = subEventcontainer.map((item) => {
            i = i + 1;
            return <div className="media">
                <img className="mr-3 d-none d-sm-block" src={`${gets3BaseUrl()}/places/tiles/` + item.event.altId.toUpperCase() + `-ht-c2.jpg`} alt="Generic placeholder image" width="140" />
                <div className="media-body">
                    <p className="mb-0 pb-2 border-bottom">{item.eventDetail.name} <TotalPrice currency={currency} TransactionDetails={item.transactionDetail} /> </p>
                    <div className="text-muted pb-3">
                        <p className="m-0"><small><i className="fa fa-map-marker-alt pink-color"></i> {item.venue.name}, {item.city.name} </small></p>
                        <TicketCategories ticketCategories={item.ticketCategory} transactionDetails={item.transactionDetail} currency={currency} />
                    </div>
                </div>
            </div>
        })
        return <>{subEventDetail}</>
    }
}