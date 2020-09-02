import * as React from "React";
import { EventFrequencyType } from "../../../Enum/EventFrequencyType";
import { Convert24hrTo12hrTimeFormat, formatDate } from "../../../utils/currencyFormatter";

export class Banner extends React.Component<any, any> {
    state = {
        height: null,
        fitst_card_width: null,
        second_card_width: null
    }

    componentDidMount() {
        var element = document.getElementById('calculate');
        var fitst_card = document.getElementById('firstCard');
        var second_card = document.getElementById('secondCard');
        if (element != null && fitst_card != null && second_card != null) {
            var positionInfo = element.getBoundingClientRect();
            var firstCard = fitst_card.getBoundingClientRect();
            var secondCard = second_card.getBoundingClientRect();
            var height = positionInfo.height;
            this.setState({
                height: height,
                fitst_card_width: firstCard.width,
                second_card_width: secondCard.width
            })
        }
    }

    getHostJSX = (hosts: any, props: any) => {
        if (hosts.length == 1) {
            return <span className="select-tkt-subtitle">Hosted by {hosts[0].firstName} {hosts[0].lastName}</span>
        } else {
            return <>
                <span className="select-tkt-subtitle">Main host: {hosts[0].firstName} {hosts[0].lastName}</span>
                <span className="select-tkt-subtitle">Other host(s): {hosts.slice(1, hosts.length).map(item => {
                     return `${item.firstName} ${item.lastName}`
                }).toString().replace(/,/g, ", ")}</span>
            </>
        }
    }

    render() {
        let props = this.props;
        const splitDateString = props.ticketCategoryData.formattedDateString.split(',');
        if (!props.scheduleDetail && props.ticketCategoryData.eventDetail[0].eventFrequencyType == EventFrequencyType.Recurring) {
            return <div></div>
        }
        return <section className="py-4 fil-inner-banner mt-3 fil-select-ticket-banner bg-white">
            <div className="container position-relative">
                <div className="card-deck overflow-hidden bg-light fil-inner-banner-images" id="calculate">
                    <div className="card">
                        <h2 className="m-0">
                            {props.ticketCategoryData.event.name}
                            {this.getHostJSX(props.ticketCategoryData.eventHostMapping, props)}
                        </h2>
                        <hr />
                        <div className="mb-2">
                            {props.ticketCategoryData.eventDetail[0].eventFrequencyType != EventFrequencyType.OnDemand && <img src="https://static7.feelitlive.com/images/fil-images/fil-exp-schedule.svg" alt="" width="16" className="mr-2" />}
                            {props.ticketCategoryData.eventDetail[0].eventFrequencyType == EventFrequencyType.Recurring ?
                                props.scheduleDetail.localStartDateString :
                                props.ticketCategoryData.eventDetail[0].eventFrequencyType == EventFrequencyType.OnDemand ?
                                    <span className="ml-2 red-tag-ticket-cat rounded px-3 py-1">On Demand</span>
                                    :
                                    `${splitDateString[0]}, ${splitDateString[1]}, ${splitDateString[2]}`}

                            {props.ticketCategoryData.eventDetail[0].eventFrequencyType == EventFrequencyType.Recurring && <a href="javascript:Void(0)" onClick={() => { props.onChangeDate() }}><img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-edit.svg" className="ml-2" /></a>}
                        </div>
                        {props.ticketCategoryData.eventDetail[0].eventFrequencyType != EventFrequencyType.OnDemand && <div>
                            <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/clock.svg" className="mr-2" width="16" alt="fil opening hours" />
                            {props.ticketCategoryData.eventDetail[0].eventFrequencyType == EventFrequencyType.Recurring ?
                                `${Convert24hrTo12hrTimeFormat(props.scheduleDetail.localStartTime)} - ${Convert24hrTo12hrTimeFormat(props.scheduleDetail.localEndTime)} ${props.ticketCategoryData.eventAttributes[0].timeZoneAbbreviation}` :
                                `${splitDateString[3]} ${props.ticketCategoryData.eventAttributes[0].timeZoneAbbreviation}`}
                        </div>}
                    </div>
                    <div className="card" id="firstCard">
                        {this.state.height != null && this.state.fitst_card_width != null && <img src={`https://feelitlive.imgix.net/images/places/tiles/${props.ticketCategoryData.event.altId.toUpperCase()}-ht-c1.jpg?auto=format&fit=crop&h=${this.state.height}&w=${this.state.fitst_card_width}&crop=entropy&q=55`} alt="" />}
                    </div>
                    <div className="card" id="secondCard">
                        {this.state.height != null && this.state.second_card_width != null && <img src={`https://feelitlive.imgix.net/images/places/about/${props.ticketCategoryData.event.altId.toUpperCase()}-about.jpg?auto=format&fit=crop&h=${this.state.height}&w=${this.state.second_card_width}&crop=entropy&q=55`} alt="" />}
                    </div>
                </div>
            </div>
        </section>
    }
}

