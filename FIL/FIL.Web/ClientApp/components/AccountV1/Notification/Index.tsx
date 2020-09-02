import * as React from 'react';
import {
    Link, RouteComponentProps, Route,
} from "react-router-dom";
import * as AccountStore from "../../../stores/Account";
import KzLoader from "../../../components/Loader/KzLoader";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import { NotificationModel } from '../../../models/Account/NotificationModel';
import { Switch } from 'antd';
import BreadcrumbAndTitle from '../BreadcrumbAndTitle';
import { gets3BaseUrl } from '../../../utils/imageCdn';

type AccountProps = AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;

class Notification extends React.PureComponent<AccountProps, any>{
    constructor(props) {
        super(props);
        this.state = {
            isOptedForMail: false,
            isLoading: true,
            gets3BaseUrl: gets3BaseUrl()
        }
    }

    componentDidMount = () => {
        if (this.props.session.isAuthenticated) {
            this.requestAction(false)
        }
    }

    componentDidUpdate = (nextProps) => {
        if (nextProps.session.isAuthenticated !== this.props.session.isAuthenticated) {
            this.requestAction(false);
        }
    }
    requestAction = (isUpdate: boolean) => {
        let userModel: NotificationModel = {
            shouldUpdate: isUpdate,
            userAltId: this.props.session.user.altId,
            isOptedForMail: this.state.isOptedForMail
        }
        let isOptedForMail = false;
        this.props.notificationAction(userModel, (response: NotificationModel) => {
            if (response.isOptedForMail) {
                isOptedForMail = true;
            }
            this.setState({
                isLoading: false,
                isOptedForMail: isOptedForMail
            })
        });
    }
    onChangeValue = (e) => {
        this.setState({
            isOptedForMail: !this.state.isOptedForMail
        }, () => {
            this.requestAction(true);
        })
    }
    render() {
        if (!this.props.session.isAuthenticated || this.state.isLoading) {
            return <KzLoader />
        }
        return <>
            <div className="container">

                <BreadcrumbAndTitle title={'Notification'} />

                <div className="row">
                    <div className="col-sm-8">
                        <div>
                            <div className="pull-right">
                                <Switch checked={!this.state.isOptedForMail}
                                    onChange={(e) => this.onChangeValue(e)} />
                            </div>
                            <h5>Unsubscribe from all marketing emails</h5>
                            <p>This includes recommendations, travel inspiration, deals things to do in your home city, how
                            FeelitLIVE works, invites and referrals, surveys and research studies, FeelitLIVE for work updates, home hosting tips,
                            experience hosting tips, and promotions.
        </p>
                        </div>

                    </div>
                    <div className="col-sm-4 mt-4 mt-sm-0 pl-md-5">
                        <div className="border p-4">
                            <img
                                src={`${this.state.gets3BaseUrl}/my-account/right-bar-images/notification.svg`}
                                className="img-fluid mb-4"
                                alt=""
                            />
                            <div>
                                <h5 className="mb-3">Would you like to recieve marketing emails?</h5>
                                <p className="m-0">
                                    Here you can subscribe or unsubscribe marketing updates from FeelitLIVE
                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    }
}
export default Notification;