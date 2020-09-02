import * as React from 'react';
import {
    Link, RouteComponentProps, Route, Switch
} from "react-router-dom";
import { IApplicationState } from "../stores";
import * as AccountStore from "../stores/Account";
import "../scss/myaccount.scss";
import { connect } from "react-redux";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "../../../shared/stores/Session";
import { bindActionCreators } from "redux";
import KzLoader from "../components/Loader/KzLoader";
import { gets3BaseUrl } from "../utils/imageCdn";
import { hpDataFeilds } from "../utils/MyAccount/DataProvider";
import PersonalInfo from "../components/AccountV1/PersonalInfo";
import Host from "../components/AccountV1/Host";
import FeelList from "../components/AccountV1/FeelList";
import Orders from "../components/AccountV1/Orders";
import PrivacyAndSharing from "../components/AccountV1/PrivacyAndSharing";
import LoginAndSecurity from "../components/AccountV1/LoginAndSecurity";
import Notification from "../components/AccountV1/Notification";
import GlobalPreferences from "../components/AccountV1/GlobalPreferences";
import { Modal } from 'antd';
import 'antd/dist/antd.css';

type AccountProps = AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;


class AccountV1 extends React.PureComponent<AccountProps, any>{
    constructor(props) {
        super(props);
        this.state = {
            gets3BaseUrl: gets3BaseUrl(),
            isLoading: true
        }
    }

    componentDidMount = () => {
        if (this.props.session.isAuthenticated) {
            this.setState({ isLoading: false })
        }
    }
    componentDidUpdate = (nextProps) => {
        if (nextProps.session.isRequestRecieved != this.props.session.isRequestRecieved) {
            if (!this.props.session.isAuthenticated) {
                this.showUnAuthorizedAlert();
                this.props.history.push("/");
            } else {
                this.setState({ isLoading: false })
            }
        }
    }

    showUnAuthorizedAlert = () => {
        let secondsToGo = 5;
        const modal = Modal.error({
            title: 'Unauthorized access!',
            content: `Please login to access the resource`,
            onOk: () => this.props.history.push("/")
        });
        setTimeout(() => {
            modal.destroy();
        }, secondsToGo * 1000);
    }

    getHpDetails = () => {
        const { gets3BaseUrl } = this.state;
        return <>
            <h2 className="mb-5">My Account</h2>
            <div className="row text-center">
                {hpDataFeilds.map((item) => {
                    return <div className={`col-sm-4 p-3 ${item.className}`}>
                        <Link to={`${this.props.match.url}${item.route}`}
                            className="card h-100 shadow-sm rounded p-3 text-reset text-decoration-none border-0 text-body">
                            <img
                                src={`${gets3BaseUrl}/my-account/icons/${item.title.toLocaleLowerCase().replace('&', '').replace(/\s\s+/g, ' ').replace(/ /g, '-')}.svg`}
                                alt={`${item.title}`}
                                width="50"
                                className="mx-auto"
                            />
                            <h5 className="mt-3 text-purple">{item.title}</h5>
                            <p className="card-desc"> {item.description} </p>
                        </Link>
                    </div>
                })}
            </div>
        </>
    }

    render() {
        const { gets3BaseUrl, isLoading } = this.state;
        if (isLoading) {
            return <KzLoader />
        }

        return <div className="my-account-flow py-5">
            <div className="container">

                <Switch>
                    <Route
                        exact
                        path="/account"
                        render={renderProps => (
                            this.getHpDetails()
                        )} />

                    <Route
                        path="/account/profile"
                        render={renderProps => (
                            <PersonalInfo
                                {...this.props}
                            />
                        )} />


                    <Route
                        path="/account/global-preferences"
                        render={renderProps => (
                            <GlobalPreferences
                                {...this.props}
                            />
                        )} />

                    <Route
                        path="/account/host"
                        render={renderProps => (
                            <Host
                                {...this.props}
                                gets3BaseUrl={gets3BaseUrl}
                            />
                        )} />

                    <Route
                        path="/account/orders"
                        render={renderProps => (
                            <Orders
                                {...this.props}
                            />
                        )} />

                    <Route
                        path="/account/feellist"
                        render={renderProps => (
                            <FeelList
                                {...this.props}
                                gets3BaseUrl={gets3BaseUrl}
                            />
                        )} />

                    <Route
                        path="/account/login-and-security"
                        render={renderProps => (
                            <LoginAndSecurity
                                {...this.props}
                            />
                        )} />

                    <Route
                        path="/account/privacy-and-sharing"
                        render={renderProps => (
                            <PrivacyAndSharing
                                {...this.props}
                                gets3BaseUrl={gets3BaseUrl}
                            />
                        )} />

                    <Route
                        path="/account/notifications"
                        render={renderProps => (
                            <Notification
                                {...this.props}
                            />
                        )} />
                </Switch>
            </div>
        </div >
    }
}


export default connect(
    (state: IApplicationState) => ({
        account: state.account,
        session: state.session
    }),
    dispatch =>
        bindActionCreators(
            { ...sessionActionCreators, ...AccountStore.actionCreators },
            dispatch
        )
)(AccountV1);