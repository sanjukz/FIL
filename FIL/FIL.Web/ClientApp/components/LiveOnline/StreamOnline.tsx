import * as React from 'react';
import * as parse from "url-parse";
import { getOnlyDonation, getOnlyBSP } from "../../utils/TicketCategory/TiqetsCategoryValidationProvider";
import { GetCartData } from "../../utils/TicketCategory/CartDataStateProvider";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { IApplicationState } from "../../stores";
import { ReactHTMLConverter } from "react-html-converter/browser";
import { RouteComponentProps } from "react-router-dom";
import * as LiveOnlineStore from "../../stores/LiveOnline";
import KzLoader from "../Loader/KzLoader";
import { gets3BaseUrl } from "../../utils/imageCdn";
import { GetUserDetailResponseModel } from "../../models/LiveOnline/GetUserDetailResponseModel";
import * as TicketCategoryPageStore from "../../stores/TicketCategory";
import { TicketCategoryResponseViewModel } from "../../models/TicketCategoryResponseViewModel";
import 'antd/dist/antd.css';
import { Alert } from 'antd';
import Donate from './Donate';
import EventDetail from './EventDetail';
import BSPUpgrade from './BSPUpgrade';

type StreamOnlineComponentProps = LiveOnlineStore.ILiveOnlineProps
    & TicketCategoryPageStore.ITicketCategoryPageProps
    & typeof TicketCategoryPageStore.actionCreators
    & typeof LiveOnlineStore.actionCreators
    & RouteComponentProps<{}>;

let urlData;

export class StreamOnline extends React.Component<StreamOnlineComponentProps, any>{
    state = {
        s3BaseUrl: gets3BaseUrl(),
        videoId: null,
        isEventStarted: false,
        isInteractivityStarted: false,
        token: null,
        donationCart: undefined,
        bspCart: undefined,
        isPlayEnd: false,
        btnText: 'Join Meeting',
        startTime: '0m0s',
        attendeeInteractivityMsg: 'The Backstage Pass interaction will begin soon. Please join by clicking on the button above.',
        hostInteractivityMsg: ' Host your backstage pass by clicking on the button above.',
        eventEndVideoId: 428415405,
        meetingId: ''
    }

    componentDidMount = () => {
        urlData = parse(location.search, location, true);
        
        let token = urlData.query.token;
        this.props.requestUserData(
            token,
            'ad2a92a9-78c2-4c90-84d1-73307c3a7ff2',
            false,
            (response: GetUserDetailResponseModel) => {
                if (response.success) {
                    if (response.isDonate || (response.isUpgradeToBSP)) {
                        this.props.getTicketCategory(response.event.altId, (responseData: TicketCategoryResponseViewModel) => {
                            let data = {
                                ticketCategoryData: responseData,
                                eventAltId: responseData.event.altId
                            }
                            if (response.isDonate) {
                                let ticketCategoryDataModel = GetCartData(data);
                                ticketCategoryDataModel.cartItems = getOnlyDonation(ticketCategoryDataModel.cartItems);
                                this.setState({ donationCart: ticketCategoryDataModel.cartItems });
                            }
                            if (response.isUpgradeToBSP) {
                                let ticketCategoryDataModel = GetCartData(data);
                                ticketCategoryDataModel.cartItems = getOnlyBSP(ticketCategoryDataModel.cartItems);
                                this.setState({ bspCart: ticketCategoryDataModel.cartItems });
                            }
                        });
                    }
                    if (!this.props.LiveOnline.userDetails.eventDetail.isEnabled) {
                        this.setState({ hostInteractivityMsg: 'Event has ended now, Thank you for hosting!' })
                    }
                    let btnText = this.getButtonText(response);
                    this.setState({ btnText: btnText, token: token, meetingId: response.meetingNumber }, () => {
                        if (response.liveEventDetail && response.liveEventDetail != null && (response.liveEventDetail.onlineEventTypeId % 2 == 1)) {
                            this.checkEventTimeAndSetEventStartTimer(response, token);
                        }
                        this.checkVideoEnded(response);
                        this.checkInterActivityStartTimer(response, token);
                    })
                }
            }
        );
    }

    getButtonText = (response: GetUserDetailResponseModel) => {
        let btnText = "Your General Admission Pass";
        if (response.roleId == "1") { // if it's host
            btnText = "Host your Backstage pass";
        } else if (response.roleId == "0" && response.isChatAvailable && response.isAudioAvailable && response.isVideoAvailable) {
            btnText = "Your Backstage Pass";
        } else if (response.roleId == "0" && response.isChatAvailable && response.isAudioAvailable) {
            btnText = "Your Exclusive Access Pass";
        } else if (response.roleId == "0" && response.isChatAvailable) {
            btnText = "Your VIP Pass";
        }
        return btnText;
    }

    getVideoId = (response: GetUserDetailResponseModel, isEventStarted: boolean,) => {
        let videoId = this.state.videoId;
        
        { /* if Event is pre-recorded or Pre-recorded Interactivity */ }
        if (response.liveEventDetail != null && (
            response.liveEventDetail.onlineEventTypeId == 1 || response.liveEventDetail.onlineEventTypeId == 3)
        ) {
            if (isEventStarted) {
                return response.liveEventDetail.videoId.split(",")[0];
            } else {
                return response.liveEventDetail.videoId.split(",").length > 1 ? response.liveEventDetail.videoId.split(",")[1] : response.liveEventDetail.videoId.split(",")[0];
            }
        } else {
            return videoId;
        }
    }

    checkMillisecondLimit = (offsetmilliseconds) => {
        /* Max limit of the setTimout is 2147483647 else it break as it can store 32-bit number only*/
        if (offsetmilliseconds > 2147483647) {
            return 2147483647;
        }
        return offsetmilliseconds;
    }

    checkEventTimeAndSetEventStartTimer = (response: GetUserDetailResponseModel, token: any) => {
        let timetarget = this.getEventUtc().getTime();
        let timenow = this.getCurrentUTC().getTime();
        let offsetmilliseconds = this.checkMillisecondLimit(timetarget - timenow);
        if (this.getEventUtc() <= this.getCurrentUTC() && (this.getCurrentUTC() <= this.getActualInterActivityStartUtc())) {
            let milliSecondDiff = Math.abs(offsetmilliseconds);
            let startTime = (Math.ceil(milliSecondDiff / 1000) / 60).toFixed(2).replace(".", "m").concat('s');
            console.log('start time=  ' + startTime);
            this.setState({ startTime: startTime, videoId: this.getVideoId(response, true), isEventStarted: true }, () => {
                console.log(`Id ${this.state.videoId}`)
            });
        } else if (this.getCurrentUTC() >= this.getActualInterActivityStartUtc()) {
            this.setState({ videoId: this.getVideoId(response, true), isEventStarted: true });
        }
        else {
            this.setState({ videoId: this.getVideoId(response, false) }, () => {
                console.log(`Id ${this.state.videoId}`)
                setTimeout(() => { this.enableEvent(response); }, offsetmilliseconds);
            });
        }
        console.log("Event UTC=" + this.getEventUtc() + " Current UTC= " + this.getCurrentUTC());
    }

    checkInterActivityStartTimer = (response: GetUserDetailResponseModel, token: any) => {
        let timetarget = this.getInterActivityStartUtc().getTime();
        let timenow = this.getCurrentUTC().getTime();
        let offsetmilliseconds = this.checkMillisecondLimit(timetarget - timenow);
        if (this.getInterActivityStartUtc() <= this.getCurrentUTC()) {
            this.setState({ isInteractivityStarted: true });
        } else {
            setTimeout(() => {
                this.setState({ isInteractivityStarted: true })
            }, offsetmilliseconds);
        }
    }

    checkVideoEnded = (response: GetUserDetailResponseModel) => {
        let timetarget = this.getActualInterActivityStartUtc().getTime();
        let timenow = this.getCurrentUTC().getTime();
        let offsetmilliseconds = this.checkMillisecondLimit(timetarget - timenow);
        let attendeeInteractivityMsg;
        if (this.props.LiveOnline.userDetails.eventDetail.isEnabled && this.props.LiveOnline.userDetails.event.isEnabled) {
            attendeeInteractivityMsg = "The Backstage Pass interaction has already begun. Please join by clicking on the button above";
        } else if (this.props.LiveOnline.userDetails.eventDetail.isEnabled) {
            attendeeInteractivityMsg = "This experience has now ended. Thanks for watching! If you want to watch it again, please click on the lower left hand corner of the player window on the rewind arrow or the play button in the middle of the player window (visible when you take your cursor on the player window).";
        } else {
            attendeeInteractivityMsg = "This experience has now ended. Thanks!";
        }
        if (this.getInterActivityStartUtc() <= this.getCurrentUTC()) {
            this.setState({ attendeeInteractivityMsg: attendeeInteractivityMsg });
        } else {
            setTimeout(() => { this.setState({ attendeeInteractivityMsg: attendeeInteractivityMsg }) }, offsetmilliseconds);
        }
    }

    getEventUtc = () => {
        let streamUTCTime = this.props.LiveOnline.userDetails.eventDetail.startDateTime;
        let splitStreamTime = streamUTCTime.split(/[^0-9]/);
        let eventUtcDate = new Date(+splitStreamTime[0], +(splitStreamTime[1]) - 1, +(splitStreamTime[2]), +(splitStreamTime[3]), +(splitStreamTime[4]), +(splitStreamTime[5]));
        return eventUtcDate;
    }

    getInterActivityStartUtc = () => {
        let streamUTCTime = (this.props.LiveOnline.userDetails.liveEventDetail != null && this.props.LiveOnline.userDetails.liveEventDetail.eventStartDateTime != null) ? this.props.LiveOnline.userDetails.liveEventDetail.eventStartDateTime : this.props.LiveOnline.userDetails.eventDetail.startDateTime;
        let splitStreamTime = streamUTCTime.split(/[^0-9]/);
        let eventUtcDate = new Date(+splitStreamTime[0], +(splitStreamTime[1]) - 1, +(splitStreamTime[2]), +(splitStreamTime[3]), +(splitStreamTime[4]), +(splitStreamTime[5]));
        return eventUtcDate;
    }

    getActualInterActivityStartUtc = () => {
        let streamUTCTime = (this.props.LiveOnline.userDetails != null && this.props.LiveOnline.userDetails.videoEndDateTime != null) ? this.props.LiveOnline.userDetails.videoEndDateTime : this.props.LiveOnline.userDetails.eventDetail.startDateTime;
        let splitStreamTime = streamUTCTime.split(/[^0-9]/);
        let eventUtcDate = new Date(+splitStreamTime[0], +(splitStreamTime[1]) - 1, +(splitStreamTime[2]), +(splitStreamTime[3]), +(splitStreamTime[4]), +(splitStreamTime[5]));
        return eventUtcDate;
    }

    getCurrentUTC = () => {
        let splitStreamTime = this.props.LiveOnline.userDetails.utcTimeNow.split(/[^0-9]/);
        let currentUtcDate = new Date(+splitStreamTime[0], +(splitStreamTime[1]) - 1, +(splitStreamTime[2]), +(splitStreamTime[3]), +(splitStreamTime[4]), +(splitStreamTime[5]));
        return currentUtcDate;
    }

    enableEvent = (response: GetUserDetailResponseModel) => {
        let currentVideoId = this.getVideoId(response, true);
        this.setState({
            videoId: currentVideoId,
            isEventStarted: true
        }, () => {
            console.log(`Id ${this.state.videoId}, Event is started`)
        });
    }

    render() {
        const converter = ReactHTMLConverter();
                
        converter.registerComponent('StreamOnline', StreamOnline);
        let iFrameSrc = `https://player.vimeo.com/video/` + this.state.videoId + `?autoplay=1$#t=${this.state.startTime}`;
        if (this.props.LiveOnline.userDetails != null && this.props.LiveOnline.userDetails.eventDetail != null && !this.props.LiveOnline.userDetails.eventDetail.isEnabled
            && !this.props.LiveOnline.userDetails.event.isEnabled) {
            iFrameSrc = `https://player.vimeo.com/video/` + this.state.eventEndVideoId + `?autoplay=1$#`;
        }
        let streamOpenLink = `https://us02web.zoom.us/j/${this.state.meetingId}`;
        if (this.props.LiveOnline.userDetails != null && this.props.LiveOnline.userDetails.roleId == "1") {
            streamOpenLink = `/live-online?token=${this.state.token}`;
        }
        return (<>
            {(!this.props.LiveOnline.isLoading && this.props.LiveOnline.userDetails != null) &&
                <div className="main-site">
                    <nav className="navbar sticky-top navbar-light bg-light">
                        <a className="navbar-brand" href="#">
                            <img
                                src={`${this.state.s3BaseUrl}/logos/fap-live-stream.png`}
                                width="120"
                                className="d-inline-block align-top"
                                alt=""
                            />
                        </a>
                    </nav>
                    <section className="fil-banner bg-dark position-relative">
                        <h1>{this.props.LiveOnline.userDetails.event.name}</h1>
                        <img src={`https://feelitlive.imgix.net/images/places/about/${this.props.LiveOnline.userDetails.event.altId.toUpperCase()}-about.jpg?auto=format&fit=crop&h=435&w=1920&crop=entropy&q=55`} className="w-100" alt="" />
                    </section>
                    <section className="text-center py-4 video-vimio">
                        {/* If Event is Pre-recorded or Pre-recorded with interactivity  */}
                        {(this.state.videoId != null) && < iframe
                            src={`${iFrameSrc}`}
                            width="750"
                            height="422"
                            className="mb-3 shadow rounded border"
                            frameBorder="0"
                            allow="autoplay; fullscreen"
                            allowFullScreen
                        ></iframe>}
                        {/* If there is no entry in liveEventDetails or liveEventDetails onlineEventTypeId is Interactivity or Prerecorded Interactivity */}
                        <div className="btn-group m-auto d-block zoom-video-head p-0">
                            <div className={"justify-content-center row"}>
                                <div className="col-sm-7">
                                    {(this.props.LiveOnline.userDetails.liveEventDetail == null ||
                                        (this.props.LiveOnline.userDetails.liveEventDetail != null &&
                                            this.props.LiveOnline.userDetails.liveEventDetail.onlineEventTypeId >= 2 &&
                                            (this.props.LiveOnline.userDetails.isVideoAvailable
                                                && this.props.LiveOnline.userDetails.isAudioAvailable
                                                && this.props.LiveOnline.userDetails.isChatAvailable
                                            ))) && this.props.LiveOnline.userDetails.event.isEnabled && <button style={!this.state.isInteractivityStarted ? { cursor: "not-allowed" } : {}}
                                                className="btn btn-outline-primary"
                                                disabled={!this.state.isInteractivityStarted}
                                                onClick={() => {
                                                    if (this.state.token != null && this.state.isInteractivityStarted) {
                                                        window.open(streamOpenLink)
                                                    }
                                                }}
                                            >
                                            {this.state.btnText}
                                        </button>}
                                    {(this.props.LiveOnline.userDetails
                                        && this.state.donationCart
                                        && this.props.LiveOnline.userDetails.isDonate
                                        && this.props.ticketCategory.fetchEventSuccess
                                        && this.props.ticketCategory.ticketCategories
                                        && this.props.LiveOnline.userDetails.roleId == "0"
                                        && this.props.ticketCategory.ticketCategories.event)
                                        && <Donate props={this.props} cartData={this.state.donationCart} />}
                                    {(this.props.LiveOnline.userDetails
                                        && this.state.bspCart
                                        && this.props.ticketCategory.fetchEventSuccess
                                        && this.props.ticketCategory.ticketCategories
                                        && this.props.LiveOnline.userDetails.roleId == "0"
                                        && this.props.ticketCategory.ticketCategories.event)
                                        && <BSPUpgrade props={this.props} cartData={this.state.bspCart} />}
                                </div>
                            </div>
                        </div>
                    </section>
                    {(this.state.isInteractivityStarted && (this.props.LiveOnline.userDetails.isVideoAvailable
                        && this.props.LiveOnline.userDetails.isAudioAvailable
                        && this.props.LiveOnline.userDetails.isChatAvailable
                    )) && <div className="text-center justify-content-center container" style={{ maxWidth: "780px" }}>
                            <Alert
                                message={this.props.LiveOnline.userDetails.roleId == "0" ? this.state.attendeeInteractivityMsg : this.state.hostInteractivityMsg}
                                type="info"
                                showIcon
                            />
                        </div>}
                    {(this.props.LiveOnline.userDetails.importantInformation && this.props.LiveOnline.userDetails.importantInformation != null && this.props.LiveOnline.userDetails.importantInformation != "" && (this.props.LiveOnline.userDetails.isChatAvailable && this.props.LiveOnline.userDetails.isAudioAvailable && this.props.LiveOnline.userDetails.isVideoAvailable)) && < section className="container my-4">
                        <h4 className="alert-heading">{this.props.LiveOnline.userDetails.roleId == "0" ? "Important information for your Backstage Pass access:" : "Important information:"}</h4>
                        <ul style={{ fontSize: "14px" }}>
                            <>{converter.convert(this.props.LiveOnline.userDetails.importantInformation)}</>
                        </ul>
                    </section>}
                    <EventDetail
                        props={this.props}
                    />
                </div>
            }
            {
                (this.props.LiveOnline.isLoading || this.props.LiveOnline.userDetails == null) &&
                <>
                    <KzLoader />
                </>
            }
        </>);
    }
}
export default connect(
    (state: IApplicationState) => ({ LiveOnline: state.liveOnline, ticketCategory: state.ticketCategory }),
    (dispatch) => bindActionCreators({ ...LiveOnlineStore.actionCreators, ...TicketCategoryPageStore.actionCreators }, dispatch)
)(StreamOnline);
