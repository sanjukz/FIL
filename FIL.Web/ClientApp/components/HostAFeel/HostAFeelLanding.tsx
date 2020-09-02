import * as React from "react";
import "../../scss/_hostafeel.scss";
const Typed = require('typed.js');

let delayTimer, typed;
export default class HostAFeelLanding extends React.Component<any, any> {
    state = {
        svgSource: "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Fitness-class.svg"
    };

    redirectToAdmin = (view = "") => {
        let feelAdminUrl = `https://host.feelitlive.com/verifyauth/`;
        if (window.origin.indexOf("dev") > -1) {
            feelAdminUrl = `https://devadmin.feelitlive.com/verifyauth/`;
        }

        if (this.props.session.isAuthenticated && view) {
            window.open(
                `${feelAdminUrl}${this.props.session.user.altId}?view=${view}`,
                '_blank' // <- This makes it open in a new window.
            );
        } else if (this.props.session.isAuthenticated) {
            window.open(
                feelAdminUrl + this.props.session.user.altId,
                '_blank' // <- This makes it open in a new window.
            );
        } else {
            let url = "";
            if (view) {
                url = `${feelAdminUrl}userAltId?view=${view}`;
            } else {
                url = `${feelAdminUrl}userAltId?view=${view}`;
            }
            this.props.showSignInSignUp(true, url);
        }
    };

    componentDidMount = () => {
        const options = {
            strings: ['<span className="purple-txt font-weight-bold d-block d-sm-inline"><img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Fitness-class.svg" width="14"/>Fitness Class</span>', '<span className="purple-txt font-weight-bold d-block d-sm-inline"><img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Concert.svg" width="14"/>Concert</span>', '<span className="purple-txt font-weight-bold d-block d-sm-inline"><img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Seminar.svg" width="14"/>Seminar</span>', '<span className="purple-txt font-weight-bold d-block d-sm-inline"><img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Arts-Performance.svg" width="14" />Arts Performance</span>', '<span className="purple-txt font-weight-bold d-block d-sm-inline"><img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Tour.svg" width="14" />Tour</span>', '<span className="purple-txt font-weight-bold d-block d-sm-inline"><img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Experiences.svg" width="14" />Experience</span>', '<span className="purple-txt font-weight-bold d-block d-sm-inline"><img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/museum.svg" width="14" />Museum</span>', '<span className="purple-txt font-weight-bold d-block d-sm-inline"><img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Galleries.svg" width="14" />Gallery</span>'],
            typeSpeed: 40,
            backSpeed: 50,
            loop: true,
            showCursor: false,
            smartBackspace: true,
        };

        typed = new Typed('#ScrollText', options);
        typed.start();

    }
    render() {
        return (
            <div className="hostAfeel-page" >
                <section className="head-sec container-fluid">
                    <div className="mx-auto text-center">
                        {this.props.isLiveStream &&
                            <div className="mb-4 pb-4"><img
                                width="170"
                                src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/logos/fap-live-stream.png`}
                                alt={"Host Live Stream Experiences and Events | Get Paid | FeelitLIVE"}
                                title={"Host Live Stream Experiences and Events | Get Paid | FeelitLIVE"}
                            /></div>
                        }
                        <h1 title="Host Live Stream Experiences and Events | Get Paid | FeelitLIVE">
                            Enabling hosts to create and earn <br />
                            from {this.props.isLiveStream ? "digital" : "real world"}  experiences
                        </h1>
                        <div style={{ fontSize: "18px" }}>
                            Attract an audience for your
                            <div id="ScrollText" className="d-inline-block pl-2">

                            </div>
                        </div>
                        <div className="btnGroup">
                            {this.props.isLiveStream ?
                                <button type="button" className="btn btn-primary m-1" onClick={() => this.redirectToAdmin("3")}>
                                    <span className="NewLBL">NEW</span>
                                    Get Started (it's free)
                                <small>Live stream your event to a paid (or free) audience</small>
                                </button> :
                                <button type="button" className="btn btn-outline-primary m-1" onClick={() => this.redirectToAdmin()}>
                                    In Real Life (IRL)
                                <small>
                                        Host and market your real-world travel/event destination
                                </small>
                                </button>}
                        </div>
                    </div>
                </section>
                <section className="text-center container-fluid">
                    <h2>You host, your way</h2>
                    <p>
                        A simple interface that helps you design your experience in the manner
                        you want
                    </p>
                    <div className="host-box-dtl">
                        <div className="host-boxes">
                            <div className="p-4 mb-2 d-inline-block border rounded bg-white">
                                <img
                                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/create.svg"
                                    alt=""
                                    width="55"
                                />
                            </div>
                            <p>Create</p>
                        </div>

                        <div className="host-boxes">
                            <div className="p-4 mb-2 d-inline-block border rounded bg-white">
                                <img
                                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/shedule.svg"
                                    alt=""
                                    width="60"
                                />
                            </div>
                            <p>Schedule</p>
                        </div>

                        <div className="host-boxes">
                            <div className="p-4 mb-2 d-inline-block border rounded bg-white">
                                <img
                                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Ticket.svg"
                                    alt=""
                                    width="55"
                                />
                            </div>
                            <p>Ticket</p>
                        </div>

                        <div className="host-boxes">
                            <div className="p-4 mb-2 d-inline-block border rounded bg-white">
                                <img
                                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Earn.svg"
                                    alt=""
                                    width="55"
                                />
                            </div>
                            <p>Earn</p>
                        </div>

                        <div className="host-boxes">
                            <div className="p-4 mb-2 d-inline-block border rounded bg-white">
                                <img
                                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Interact.svg"
                                    alt=""
                                    width="50"
                                />
                            </div>
                            <p>Interact</p>
                        </div>
                    </div>
                </section>
                <section className="text-center container-fluid">
                    <h2>Tools we provide</h2>
                    <p>
                        All the tools you need – in one place, with the ability to manage,
                        tailor and improve as you go
                    </p>
                    <div className="host-box-dtl">
                        <div className="host-boxes">
                            <div className="p-4 mb-2 d-inline-block border rounded bg-white">
                                <img
                                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Live.svg"
                                    alt=""
                                    width="50"
                                />
                            </div>
                            <p>Live</p>
                        </div>

                        <div className="host-boxes">
                            <div className="p-4 mb-2 d-inline-block border rounded bg-white">
                                <img
                                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Payment.svg"
                                    alt=""
                                    width="50"
                                />
                            </div>
                            <p>Payments</p>
                        </div>

                        <div className="host-boxes">
                            <div className="p-4 mb-2 d-inline-block border rounded bg-white">
                                <img
                                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Analytics.svg"
                                    alt=""
                                    width="50"
                                />
                            </div>
                            <p>Analytics</p>
                        </div>

                        <div className="host-boxes">
                            <div className="p-4 mb-2 d-inline-block border rounded bg-white">
                                <img
                                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Support.svg"
                                    alt=""
                                    width="65"
                                />
                            </div>
                            <p>Support</p>
                        </div>
                    </div>
                </section>
                <section>
                    <div className="container">
                        <h2 className="text-center mb-3 pb-3">Getting started - as easy as...</h2>
                        <div className="card-deck">
                            <div className="card">
                                <div className="card-body">
                                    <h5 className="card-title"><span>1</span>Input your details</h5>
                                    <p className="card-text">
                                        Enter your details such as the name and description of your
                                        experience/event/venue.
                                    </p>
                                </div>
                            </div>
                            <div className="card">
                                <div className="card-body">
                                    <h5 className="card-title"><span>2</span>Build the experience</h5>
                                    <p className="card-text">
                                        Upload images, pricing and schedule.
                                    </p>
                                </div>
                            </div>
                            <div className="card">
                                <div className="card-body">
                                    <h5 className="card-title"><span>3</span>Be ready to host</h5>
                                    <p className="card-text">
                                        Once it is confirmed to meet our quality standards, your
                                        experience/event/venue will be on sale online.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div >
        );
    }
}