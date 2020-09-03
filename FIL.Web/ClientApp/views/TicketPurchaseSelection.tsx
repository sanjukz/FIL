import * as React from "react";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as parse from "url-parse";
import { RouteComponentProps } from "react-router-dom";
import { checkIsLiveOnlineEvents } from "../../ClientApp/utils/TicketCategory/ItineraryProvider";
import { IApplicationState } from "../stores";
import * as TicketCategoryPageStore from "../stores/TicketCategory";
import * as CategoryPageStore from "../stores/AllCategories";
import FilLoader from "../components/Loader/FilLoader";
import Metatag from "../components/Metatags/Metatag";
import TicketCategorySelection from "../components/TicketCategory/TicketCategorySelection";
import TicketCategorySelectionMoveAround from "../components/TicketCategory/TicketCategorySelectionMoveAround";
import "../scss/_ticket-details.scss";
import "../scss/_custom.scss";
import "../scss/recurring-event.scss";
import { gets3BaseUrl } from "../utils/imageCdn";
import CheckoutComponent from "../../ClientApp/components/CheckOut/CheckoutComponent";
import { actionCreators as sessionActionCreators, ISessionProps } from "shared/stores/Session";
import { EventFrequencyType } from '../Enum/EventFrequencyType';
import { MasterEventTypes } from '../Enum/MasterEventTypes';
import { Banner as OnlineBanner } from '../components/TicketCategory/Online/Banner';
import { Banner as IRLBanner } from '../components/TicketCategory/InRealLife/Banner';
import { Schedule } from '../components/TicketCategory/Online/Schedule';
import { TicketCategoryResponseViewModel } from "../models/TicketCategoryResponseViewModel";

enum View {
    TicketCategory,
    Schedule
}

type TicketCategoryComponentProps =
    TicketCategoryPageStore.ITicketCategoryPageProps
    & CategoryPageStore.IAllCategoriesProps
    & ISessionProps & typeof sessionActionCreators
    & typeof TicketCategoryPageStore.actionCreators
    & typeof CategoryPageStore.actionCreators
    & RouteComponentProps<{ eventId: string; }>;

class TicketPurchaseSelection extends React.Component<TicketCategoryComponentProps, any> {
    constructor(props) {
        super(props);
        this.state = {
            currentValue: 0,
            categoryState: 0,
            defaultPlace: true,
            catClass: 0,
            sabCatClass: 0,
            isAddOns: false,
            addOnId: 0,
            className: "",
            selectedDate: null,
            checkoutUser: false,
            scheduleDetailId: null,
            s3BaseUrl: gets3BaseUrl(),
            view: View.TicketCategory
        }
    }

    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }
        this.props.getTicketCategory(this.props.match.params.eventId, (response: TicketCategoryResponseViewModel) => {
            if (response.eventDetail[0].eventFrequencyType == EventFrequencyType.Recurring) {
                this.setState({ view: View.Schedule })
            }
        });
        this.props.requestCountryData();
        let urlData = parse(location.search, location, true);
        if (urlData.query.type && urlData.query.type == 'donation') {
            this.setState({ isDonate: true });
        }
    }

    public goToItinerary() {
        let isLiveOnlineEvents = checkIsLiveOnlineEvents();
        if (isLiveOnlineEvents) {
            this.setState({ checkoutUser: true })
        } else {
            this.props.history.push({ pathname: "/itinerary" });
        }
    }

    onChangeDate(date) {
        this.setState({ selectedDate: date })
    }

    onCloseSignInSignUp = () => {
        this.setState({ checkoutUser: false })
    }

    getView = (ticketCategoryData) => {
        if (this.state.view == View.Schedule) {
            return <Schedule
                ticketCategoryData={ticketCategoryData}
                isClosable={this.state.scheduleDetail}
                onSelectSchedule={(scheduleDetail) => {
                    this.setState({ scheduleDetail: scheduleDetail, view: View.TicketCategory });
                }}
                onCloseDrawer={() => {
                    this.setState({ view: View.TicketCategory });
                }}
            />
        } else {
            return <>
                {ticketCategoryData.event.masterEventTypeId != MasterEventTypes.Online ?
                    <IRLBanner ticketCategoryData={ticketCategoryData} eventId={this.props.match.params.eventId} /> : <OnlineBanner
                        ticketCategoryData={ticketCategoryData}
                        scheduleDetail={this.state.scheduleDetail}
                        onChangeDate={() => {
                            this.setState({ view: View.Schedule })
                        }}
                    />}
                {this.props.ticketCategory.fetchEventSuccess
                    && this.props.ticketCategory.fetchCountriesSuccess ? <div>{+this.props.ticketCategory.ticketCategories.event.eventCategoryId == 34 ? <TicketCategorySelectionMoveAround ticketCategoryData={ticketCategoryData} eventName={ticketCategoryData.eventDetail[0].name} eventAltId={this.props.match.params.eventId} goToItinerary={this.goToItinerary.bind(this)} countries={this.props.ticketCategory.countryList.countries} /> :
                        <TicketCategorySelection scheduleDetail={this.state.scheduleDetail} ticketCategoryData={ticketCategoryData} eventName={ticketCategoryData.eventDetail[0].name}
                            eventAltId={this.props.match.params.eventId} goToItinerary={this.goToItinerary.bind(this)}
                            countries={this.props.ticketCategory.countryList.countries}
                            getTiqetsTimeSlots={this.props.getTiqetsTimeSlots} fetchTiqetsTimeSlots={this.props.ticketCategory.fetchTiqetsTimeSlots}
                            tiqetsTimeSlots={this.props.ticketCategory.tiqetsTimeSlots} onChangeDate={this.onChangeDate.bind(this)} selectedDate={this.state.selectedDate}
                            getHohoTimeSlots={this.props.getHoHoTimeSlots} fetchHohoTimeSlots={this.props.ticketCategory.fetchHohoTimeSlots} hohoTimeSlots={this.props.ticketCategory.hohoTimeSlots} isDonate={this.state.isDonate} />} </div> : null}
            </>
        }
    }

    public render() {
        if (this.props.ticketCategory.fetchEventSuccess && this.props.ticketCategory.fetchCountriesSuccess) {
            const ticketCategoryData = this.props.ticketCategory.ticketCategories;
            return <div className="fil-site fil-exp-landing-page">
                <Metatag url={this.props.location.pathname} metaContent={ticketCategoryData} title={ticketCategoryData.event.name} eventAltId={this.props.match.params.eventId} />
                {this.getView(ticketCategoryData)}
                {
                    this.state.checkoutUser &&
                    <CheckoutComponent session={this.props.session} history={this.props.history}
                        onCloseSignInSignUp={() => this.onCloseSignInSignUp()}
                    />
                }
            </div>
        }
        else {
            return <div><FilLoader /></div>
        }
    }
}

export default connect(
    (state: IApplicationState) => ({ ticketCategory: state.ticketCategory, allCategories: state.allCategories, session: state.session }),
    (dispatch) => bindActionCreators({ ...TicketCategoryPageStore.actionCreators, ...CategoryPageStore.actionCreators, ...sessionActionCreators }, dispatch)
)(TicketPurchaseSelection);

