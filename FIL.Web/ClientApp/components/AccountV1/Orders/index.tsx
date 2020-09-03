import * as React from 'react';
import ShowOrders from './ShowOrders';
import {
    Link, RouteComponentProps, Route, Switch
} from "react-router-dom";
import * as AccountStore from "../../../stores/Account";
import FilLoader from "../../../components/Loader/FilLoader";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import { UserOrderRespnseViewModel } from "../../../models/Account/UserOrderRespnseViewModel";
import BreadcrumbAndTitle from '../BreadcrumbAndTitle';
import { gets3BaseUrl } from '../../../utils/imageCdn';

type AccountProps = AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;

class Orders extends React.PureComponent<AccountProps, any>{
    constructor(props) {
        super(props);
        this.state = {
            isLoading: true,
            gets3BaseUrl: gets3BaseUrl()
        }
    }
    componentDidMount = () => {
        if (this.props.session.isAuthenticated) {
            if (this.props.account.userOrders && this.props.account.userOrders.transaction.length == 0) {
                this.requestOrderDetails();
            }
            else {
                if (this.props.account.userOrders.transaction.length > 0) {
                    this.setState({ isLoading: false })
                }
            }
        }
    }
    componentDidUpdate = (nextProps) => {
        if (nextProps.session.isAuthenticated !== this.props.session.isAuthenticated) {
            this.requestOrderDetails();
        }
    }

    requestOrderDetails = () => {
        this.props.getUserOrdertData(this.props.session.user.altId);
        this.setState({ isLoading: false })
    }
    render() {
        if (this.props.account.isLoadingUserOrderList || !this.props.session.isAuthenticated
            || this.state.isLoading) {
            return <FilLoader />
        }
        return <div className="container">

            <BreadcrumbAndTitle title={'Orders'} />

            <ShowOrders {...this.props} gets3BaseUrl={this.state.gets3BaseUrl} />
        </div>
    }
}

export default Orders;