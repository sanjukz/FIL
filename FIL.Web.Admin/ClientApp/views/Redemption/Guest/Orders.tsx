import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../../stores";
import { bindActionCreators } from "redux";
import { ApproveStatus } from "../../../../ClientApp/models/Redemption/ApproveStatus";
import FILLoader from "../../../../ClientApp/components/Loader/FILLoader";
import Modal from 'react-awesome-modal';
import OrderDetailModalComponent from "../../../../ClientApp/components/Redemption/Orders/OrderDetailModalComponent";
import * as numeral from "numeral";
import * as RedemptionStore from "../../../stores/Redemption";
import { ParseDateToLocal } from "../../../utils/ParseDateToLocal";
import { ConfirmGuideResponseModel } from "../../../models/Redemption/ConfirmGuideResponseModel";
import * as _ from "lodash";

type approveModerateGuide = RedemptionStore.RedemptionComponentProps
    & typeof RedemptionStore.actionCreators

class Orders extends React.Component<approveModerateGuide, any> {
    constructor(props) {
        super(props)
        this.state = {
            displayScreen: ApproveStatus.Pending,
            isTab1: true,
            isTab2: false,
            isTab3: false
        }
    }

    componentDidMount() {
        this.props.requestGuideOrders(2);
    }

    handleSelect = (e, button, row) => {
        if (button == 3 || button == 4) {
            this.props.confirmOrder(button, e.transactionId, (item: ConfirmGuideResponseModel) => {
                if (item.success && button == 3) {
                    alert("Order approved successfully");
                    this.props.requestGuideOrders(2);
                } else if (item.success && button == 4) {
                    alert("Order completed successfully");
                    this.props.requestGuideOrders(3);
                }
            });
        }
        this.setState({ isShowConfirmPopup: false });
    }

    getContentHTML = (orderCards) => {
        return (<div>
            {
                (this.props.Redemption.guideOrders.orderDetails.length > 0 && this.props.Redemption.fetchGuideOrdersSuccess && !this.props.Redemption.fetchGuideOrdersRequest && !this.props.Redemption.fetchConfirmOrderRequest) && <div className="container mt-3">
                    <div className="row">
                        <div className="card-deck">
                            {orderCards}
                        </div>
                    </div>
                </div>
            }
            <Modal visible={this.state.isShowOrderDetailPopup} effect="fadeInUp" onClickAway={() => this.setState({ isShowOrderDetailPopup: false })} >
                <div className="container my-3">
                    {(this.state.currentOrder && this.state.isShowOrderDetailPopup) && <div className="card-deck">
                        <div className={"card shadow" + (this.state.displayScreen == ApproveStatus.Pending ? " border-warning" : this.state.displayScreen == ApproveStatus.Approved ? " border-primary" : " border-success")}>
                            <OrderDetailModalComponent
                                currentOrder={this.state.currentOrder}
                                currentUser={this.state.currentUser}
                                screenStatus={this.state.displayScreen}
                            />
                            <div className={"card-footer bg-transparent text-center" + (this.state.displayScreen == ApproveStatus.Pending ? " border-warning" : this.state.displayScreen == ApproveStatus.Approved ? " border-primary" : " border-success")}>
                                <a href="javascript:void(0)" className="btn btn-success" onClick={() => this.setState({ isShowOrderDetailPopup: false })}  >Close</a>
                            </div>
                        </div>
                    </div>}
                </div>
            </Modal>
            <Modal visible={this.state.isShowConfirmPopup} effect="fadeInUp" onClickAway={() => this.setState({ isShowConfirmPopup: false })} >
                <div className="container my-3">
                    <div className="card-deck">
                        <div className={"card shadow" + (this.state.displayScreen == ApproveStatus.Pending ? " border-warning" : this.state.displayScreen == ApproveStatus.Approved ? " border-primary" : " border-success")}>
                            <span className={"card-header bg-transparent text-center border-0" + (this.state.displayScreen == ApproveStatus.Pending ? " border-warning" : this.state.displayScreen == ApproveStatus.Approved ? " border-primary" : " border-success")}>{this.state.confirmMessage}</span>
                            <div className={"card-footer bg-transparent" + (this.state.displayScreen == ApproveStatus.Pending ? " border-warning" : this.state.displayScreen == ApproveStatus.Approved ? " border-primary" : " border-success")}>
                                <a href="javascript:void(0)" className="btn btn-success" onClick={this.handleSelect.bind(this, this.state.currentOrder, this.state.orderChangeStatues)} >Yes</a>
                                <a href="javascript:void(0)" className="btn btn-danger pull-right" onClick={this.handleSelect.bind(this, this.state.currentOrder, this.state.orderChangeStatues)} >No</a>
                            </div>
                        </div>
                    </div>
                </div>
            </Modal>
            {(this.props.Redemption.guideOrders.orderDetails.length == 0 && this.props.Redemption.fetchGuideOrdersSuccess && !this.props.Redemption.fetchGuideOrdersRequest && !this.props.Redemption.fetchConfirmOrderRequest) &&
                <div className="text-center">
                    <div className="pb-3"><img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/feelAdmin/fap-not-found.svg" /></div>
                    <h4>Sorry, there are no {this.state.displayScreen == ApproveStatus.Pending ? 'pending' : this.state.displayScreen == ApproveStatus.Approved ? 'on going' : 'completed'} orders.</h4>
                    {this.state.displayScreen == ApproveStatus.Pending && <a href="javascript:void(0)" onClick={(e) => {
                        this.props.requestGuideOrders(3);
                        this.setState({ displayScreen: ApproveStatus.Approved })
                    }} className="btn btn-primary mt-3 text-white" >Go to On Going</a>}
                    {this.state.displayScreen == ApproveStatus.Approved && <a href="javascript:void(0)" onClick={(e) => {
                        this.props.requestGuideOrders(4);
                        this.setState({ displayScreen: ApproveStatus.Success })
                    }} className="btn btn-primary mt-3 text-white">Go to Completed</a>}
                    {this.state.displayScreen == ApproveStatus.Success && <a href="javascript:void(0)" onClick={(e) => {
                        this.props.requestGuideOrders(2);
                        this.setState({ displayScreen: ApproveStatus.Pending })
                    }} className="btn btn-primary mt-3 text-white">Go to Pending</a>}
                </div>
            }
        </div>)
    }

    render() {
        var that = this;
        var data = this.props.Redemption.guideOrders;
        var orderCards = data.orderDetails.map(function (item, index) {
            var user = data.approvedByUsers ? data.approvedByUsers.filter(function (val) {
                return val.altId == item.orderApprovedBy
            }) : [];

            return <div className="card mb-4 border-success" style={{ minWidth: '18rem', maxWidth: '18rem', maxHeight: '400px' }}>
                <div style={{ cursor: 'pointer' }} onClick={() => {
                    that.setState({
                        isShowOrderDetailPopup: true,
                        currentOrder: item,
                        currentUser: user
                    });
                }} >
                    <h5 className="card-header bg-transparent border-success text-center"><a href="javascript:void(0)">{item.placeName}</a></h5>
                    <div className="card-body">
                        <p className="card-text">Transaction Id: {item.transactionId}</p>
                        <p className="card-text">Visit Date: {item.visitDate ? (ParseDateToLocal(item.visitDate).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + ParseDateToLocal(item.visitDate).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(ParseDateToLocal(item.visitDate).getHours()).format('00') + ":" + numeral(ParseDateToLocal(item.visitDate).getMinutes()).format('00')) : "--"}</p>
                    </div>
                </div>
                {(that.state.displayScreen == ApproveStatus.Pending || that.state.displayScreen == ApproveStatus.Approved) && < div className="card-footer bg-transparent border-success">
                    <a href="javascript:void(0)" className={that.state.displayScreen == ApproveStatus.Approved ? "btn btn-success text-center" : "btn btn-success"} onClick={() => {
                        that.setState({
                            isShowConfirmPopup: true,
                            currentOrder: item,
                            orderChangeStatues: that.state.displayScreen == ApproveStatus.Pending ? ApproveStatus.Approved : ApproveStatus.Success,
                            confirmMessage: `Are you sure, you want to ${that.state.displayScreen == ApproveStatus.Pending ? 'accept' : 'complete'} the current order?`
                        })
                    }} >{that.state.displayScreen == ApproveStatus.Pending ? 'Accept' : 'Complete'}</a>
                    {(that.state.displayScreen == ApproveStatus.Pending) && < a href="javascript:void(0)" className="btn btn-danger pull-right" onClick={() => {
                        that.setState({
                            isShowConfirmPopup: true,
                            currentOrder: item,
                            orderChangeStatues: ApproveStatus.Pending,
                            confirmMessage: `Are you sure, you want to reject the current order?`
                        })
                    }} >Reject</a>}
                </div>}
            </div>
        });
        return <div>
            <nav className="pb-4">
                <div className="nav justify-content-center text-uppercase fee-tab" id="nav-tab" role="tablist">
                    <a
                        className={"nav-item nav-link" + (this.state.displayScreen == ApproveStatus.Pending ? ' active' : '')}
                        onClick={(e) => {
                            this.props.requestGuideOrders(2);
                            this.setState({ displayScreen: ApproveStatus.Pending })
                        }}
                        id="nav-Details-tab"
                        data-toggle="tab"
                        href="#nav-Details"
                        role="tab"
                        aria-controls="nav-Details">
                        Pending
                        </a>
                    <a
                        className={"nav-item nav-link" + (this.state.displayScreen == ApproveStatus.Approved ? ' active' : '')}
                        onClick={(e) => {
                            this.props.requestGuideOrders(3);
                            this.setState({ displayScreen: ApproveStatus.Approved })
                        }}
                        id="nav-Inventory-tab"
                        data-toggle="tab"
                        href="#nav-Inventory"
                        role="tab"
                        aria-controls="nav-Inventory">
                        On Going
                        </a>
                    <a
                        className={"nav-item nav-link" + (this.state.displayScreen == ApproveStatus.Success ? ' active' : '')}
                        onClick={(e) => {
                            this.props.requestGuideOrders(4);
                            this.setState({ displayScreen: ApproveStatus.Success })
                        }}
                        id="nav-contact-tab"
                        data-toggle="tab"
                        href="#nav-contact"
                        role="tab"
                        aria-controls="nav-contact">
                        Completed
                        </a>
                </div>
            </nav>
            <div className="tab-content bg-white rounded shadow-sm p-3" id="nav-tabContent">
                <div
                    className={"tab-pane fade show" + (this.state.displayScreen == ApproveStatus.Pending ? ' active' : '')}
                    id="nav-Details"
                    role="tabpanel"
                    aria-labelledby="nav-Details-tab"
                >
                    {this.getContentHTML(orderCards)}
                </div>
                <div
                    className={"tab-pane fade show" + (this.state.displayScreen == ApproveStatus.Approved ? ' active' : '')}
                    id="nav-Details"
                    role="tabpanel"
                    aria-labelledby="nav-Details-tab"
                >
                    {this.getContentHTML(orderCards)}
                </div>
                <div
                    className={"tab-pane fade show" + (this.state.displayScreen == ApproveStatus.Success ? ' active' : '')}
                    id="nav-Details"
                    role="tabpanel"
                    aria-labelledby="nav-Details-tab"
                >
                    {this.getContentHTML(orderCards)}
                </div>
            </div>
            {(this.props.Redemption.fetchGuideOrdersRequest || this.props.Redemption.fetchConfirmOrderRequest) && <FILLoader />}
        </div>
    }
}

export default connect(
    (state: IApplicationState) => ({
        Redemption: state.Redemption
    }),
    (dispatch) => bindActionCreators({
        ...RedemptionStore.actionCreators
    }, dispatch)
)(Orders);
