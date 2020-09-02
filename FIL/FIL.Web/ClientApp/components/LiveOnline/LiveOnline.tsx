import * as React from "react";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { IApplicationState } from "../../stores";
import FeelLoader from "../../components/Loader/FeelLoader";
import * as LiveOnlineStore from "../../stores/LiveOnline";
import * as parse from "url-parse";
import * as cryptoRandomString from "crypto-random-string";
import { GetUserDetailResponseModel } from "../../models/LiveOnline/GetUserDetailResponseModel";

let ZoomMtg,
    token,
    urlSafe,
    isMobile = false;
class LiveOnline extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            meetingLaunched: false,
            isLoading: true,
            eventName: ""
        };
    }
    launchMeeting = (userDetails, token: any) => {
        let endurl = "";
        if (typeof window != "undefined" && window.screen.width <= 640) {
            isMobile = true;
        }
        endurl = window.location.href + "&end=true";
        var that = this;
        this.setState({ meetingLaunched: !this.state.meetingLaunched });
        ZoomMtg.ZoomMtg.init({
            leaveUrl: endurl,
            disableJoinAudio: !userDetails.isAudioAvailable,
            disableInvite: true,
            isSupportChat: userDetails.isChatAvailable,
            success() {
                ZoomMtg.ZoomMtg.join({
                    meetingNumber: userDetails.meetingNumber,
                    userName: userDetails.userName,
                    signature: userDetails.signature,
                    apiKey: userDetails.apikey,
                    passWord: "",
                    success() {
                        let titleTab = document.getElementsByClassName("title tab");
                        if (titleTab && titleTab.length > 0) {
                            titleTab[0].remove();
                        }
                        if (userDetails.roleId != "1") {
                            if (!userDetails.isVideoAvailable) {
                                var index = userDetails.isAudioAvailable ? 1 : 0;
                                var x = document.getElementsByClassName(
                                    "join-audio ax-outline"
                                );
                                if (x && x.length > 0) {
                                    x[index].remove();
                                }
                            }
                            if (!isMobile) {
                                let moreButton = document.getElementsByClassName(
                                    "dropup btn-group"
                                );
                                if (moreButton && moreButton.length > 0) {
                                    moreButton[0].remove();
                                }
                                if (moreButton && moreButton.length > 0) {
                                    moreButton[0].remove();
                                }
                                let manageElements = document.getElementsByClassName(
                                    "undefined footer-button__button ax-outline"
                                );
                                if (manageElements && manageElements.length > 0) {
                                    manageElements[1].remove();
                                }
                            } else {
                                let moreElements = document.getElementsByClassName(
                                    "more-button__pop-menu dropdown-menu"
                                );
                                if (moreElements && moreElements.length > 0) {
                                    if (
                                        moreElements[0].children &&
                                        moreElements[0].children.length > 0
                                    )
                                        moreElements[0].children[0].remove();
                                }
                            }
                        }
                        that.setState({ isLoading: false });
                    },
                    error(res) {
                        alert("Failed to join please try again.");
                        window.location.assign(`/stream-online?token=${token}&play=end`);
                    }
                });
            },
            error(res) {
                console.log(res);
            }
        });
    };

    componentDidMount() {
        ZoomMtg = require("@zoomus/websdk");
        ZoomMtg.ZoomMtg.setZoomJSLib("https://source.zoom.us/1.8.0/lib", "/av");
        ZoomMtg.preLoadWasm();
        ZoomMtg.prepareJssdk();
        const data: any = parse(location.search, location, true);
        if (data.query.end) {
            let status = data.query.end;
            if (status) {
                let sessionToken = localStorage.getItem(token);
                this.props.deactiveUser(token, sessionToken);
                localStorage.removeItem(token);
                window.location.assign(`/stream-online?token=${data.query.token}&play=end`);
            }
        } else {
            token = data.query.token;
            urlSafe = cryptoRandomString({ length: 10, type: "url-safe" });
            let sessionToken = localStorage.getItem(token);
            let alertResponse = true;
            if (sessionToken == undefined) {
                localStorage.setItem(token, urlSafe);
                sessionToken = urlSafe;
            } else {
                alertResponse = confirm(
                    "Your session was closed abruptly, do you want to continue?"
                );
            }
            if (alertResponse) {
                this.props.requestUserData(
                    token,
                    sessionToken,
                    true,
                    (response: GetUserDetailResponseModel) => {
                        if (response.success) {
                            this.setState({
                                eventName: response.eventName && response.eventName
                            });
                            this.launchMeeting(response, token);
                        } else {
                            alert(response.message);
                            window.location.assign(`/stream-online?token=${data.query.token}&play=end`);
                        }
                    }
                );
            } else {
                this.props.deactiveUser(token, sessionToken);
                localStorage.removeItem(token);
                window.location.assign(`/stream-online?token=${data.query.token}&play=end`);
            }
        }
    }
    render() {
        if (this.props.isLoading) {
            return <FeelLoader />;
        } else {
            return (
                <>
                    <div className="zoom-video-head">
                        <img
                            src="https://static5.feelitlive.com/images/logos/fap-live-stream.png"
                            alt="feel Guide logo"
                            width="100"
                        />
                        <b className="heat-title">{this.state.eventName}</b>

                    </div>
                </>
            );
        }
    }
}
export default connect(
    (state: IApplicationState) => ({
        liveOnline: state.liveOnline
    }),
    dispatch =>
        bindActionCreators({ ...LiveOnlineStore.actionCreators }, dispatch)
)(LiveOnline);
