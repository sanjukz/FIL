import * as React from 'react';
import { gets3BaseUrl } from "../../../utils/imageCdn";
import { connect } from "react-redux";
import { IApplicationState } from "../../../stores";
import {
    Link, RouteComponentProps, Route, Switch
} from "react-router-dom";
import * as AccountStore from "../../../stores/Account";
import KzLoader from "../../../components/Loader/KzLoader";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import BreadcrumbAndTitle from '../BreadcrumbAndTitle';

interface Iprops {
    gets3BaseUrl: string;
}

type AccountProps = Iprops & AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;

const prodHost = 'https://host.feelitlive.com/';
const devHost = 'https://devadmin.feelitlive.com/';

function Host(props: AccountProps) {
    const [tab, SelectTab] = React.useState("online");
    return <div className="container">

        <BreadcrumbAndTitle title={'Host'} />

        <div className="row">
            <div className="col-sm-8">
                <ul
                    className="nav nav-tabs border-bottom text-uppercase"
                    id="my-acc-tab"
                    role="tablist"
                >
                    <li className="nav-item" role="presentation" onClick={() => SelectTab("online")}>
                        <a
                            className="nav-link active"
                            id="online-experiences"
                            data-toggle="pill"
                            href="#pills-online"
                            role="tab"
                            aria-controls="pills-online"
                            aria-selected="true"
                        >ONLINE EXPERIENCES</a >
                    </li>
                    <li className="nav-item" role="presentation" onClick={() => SelectTab("irl")}>
                        <a
                            className="nav-link"
                            id="in-real-life-experiences"
                            data-toggle="pill"
                            href="#pills-inreal"
                            role="tab"
                            aria-controls="pills-inreal"
                            aria-selected="false"
                        >IN-REAL LIFE EXPERIENCES</a>
                    </li>
                    <li className="nav-item" role="presentation" onClick={() => SelectTab("reports")}>
                        <a
                            className="nav-link"
                            id="reports"
                            data-toggle="pill"
                            href="#pills-report"
                            role="tab"
                            aria-controls="pills-report"
                            aria-selected="false"
                        >REPORTS</a>
                    </li>
                </ul>
                <div className="tab-content" id="my-acc-tabContent">
                    <div
                        className="tab-pane fade show active"
                        id="pills-online"
                        role="tabpanel"
                        aria-labelledby="online-experiences"
                    >
                        <div className="mt-4 clearfix">
                            <div className="pull-left">
                                <h5>Empowering you to create and earn from digital experiences</h5>
                                <p>Create and manage your online experiences.</p>
                            </div>
                            <a
                                href={`javascript:void(0)`}
                                onClick={() => {
                                    if (window && window.origin.indexOf('dev') > -1) {
                                        window.open(
                                            `${devHost}verifyauth/${props.session.user.altId}?view=3`,
                                            '_blank' // <- This makes it open in a new window.
                                        );
                                    } else {
                                        window.open(
                                            `${prodHost}verifyauth/${props.session.user.altId}?view=3`,
                                            '_blank' // <- This makes it open in a new window.
                                        );
                                    }
                                }}
                                className={`text-decoration-none pull-right text-purple`}
                            >
                                Manage
                             </a>
                        </div>
                    </div>
                    <div
                        className="tab-pane fade"
                        id="pills-inreal"
                        role="tabpanel"
                        aria-labelledby="in-real-life-experiences"
                    >
                        <div className="mt-4 clearfix">
                            <div className="pull-left">
                                <h5>Create and manage your in-real-life experiences  </h5>
                                <p> List your in-real-life experience at FeelitLIVE and boost your sales</p>
                            </div>
                            <a
                                href={`javascript:void(0)`}
                                onClick={() => {
                                    if (window && window.origin.indexOf('dev') > -1) {
                                        window.open(
                                            `${devHost}verifyauth/${props.session.user.altId}`,
                                            '_blank' // <- This makes it open in a new window.
                                        );
                                    } else {
                                        window.open(
                                            `${prodHost}verifyauth/${props.session.user.altId}`,
                                            '_blank' // <- This makes it open in a new window.
                                        );
                                    }
                                }}
                                className={`text-decoration-none pull-right text-purple`}
                            >
                                Manage
                </a>

                        </div>

                    </div>
                    <div
                        className="tab-pane fade"
                        id="pills-report"
                        role="tabpanel"
                        aria-labelledby="reports"
                    >
                        <div className="mt-4 clearfix">
                            <div className="pull-left">
                                <h5>Report </h5>
                                <p>Find out how your experience/event sales are performing.</p>
                            </div>
                            <a
                                href={`javascript:void(0)`}
                                onClick={() => {
                                    if (window && window.origin.indexOf('dev') > -1) {
                                        window.open(
                                            `${devHost}verifyauth/${props.session.user.altId}?view=4`,
                                            '_blank' // <- This makes it open in a new window.
                                        );
                                    } else {
                                        window.open(
                                            `${prodHost}verifyauth/${props.session.user.altId}?view=4`,
                                            '_blank' // <- This makes it open in a new window.
                                        );
                                    }
                                }}
                                className={`text-decoration-none pull-right text-purple`}
                            >
                                Manage
                </a>

                        </div>

                    </div>

                </div>
            </div>
            <div className="col-sm-4 mt-4 mt-sm-0 pl-md-5">
                <div className="border p-4">
                    <img
                        src={`${props.gets3BaseUrl}/my-account/right-bar-images/host-${tab}.svg`}
                        className="img-fluid mb-4"
                        alt=""
                    />
                    <div>
                        <h5 className="mb-3">{tab == "online" ? 'Manage your online experiences' : `${tab == "irl" ?
                            'Manage your IRL experiences'
                            : 'Downloading your reports'}`}</h5>
                        <p className="m-0">
                            {tab == "online" ? 'Boost your revenue and engage with your audiences by effortlessly bringing your artistry and expertise online' :
                                `${tab == "irl" ? 'List your in-real-life experience at feelitLIVE and boost your sales.'
                                    : 'The reports section will give you access and the option to export your sales performance data.'}`}
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div >
}

export default Host;