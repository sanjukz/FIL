import * as React from "react";
import { IApplicationState } from "../stores";
import { connect } from "react-redux";
import { actionCreators as sessionActionCreators, ISessionProps } from "shared/stores/Session";
import { bindActionCreators } from "redux";
import { Link, RouteComponentProps } from "react-router-dom";
import ReportingTableComponent from "../../ClientApp/components/TableComponent";
import * as SellerStore from "../stores/Seller";
import FILLoader from "../../ClientApp/components/Loader/FILLoader";
import { autobind } from "core-decorators";
import * as numeral from "numeral";
import { Modal } from 'antd';
import { gets3BaseUrl } from "../utils/imageCdn";

type SellerProps = SellerStore.ISellerProps
    & typeof SellerStore.actionCreators
    & ISessionProps
    & typeof sessionActionCreators
    & RouteComponentProps<any>;

enum tab {
    draft = 1,
    submitForApproval,
    liveTab,
    pastTab
};

enum buttonType {
    Manage = 1,
    Edit,
    Preview
}

class MyFeels extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            currentTab: tab.submitForApproval,
            isAPICall: false,
            isStripeSuccess: false
        };
    }

    public componentDidMount() {
        if (this.props.session.isAuthenticated) {
            //  this.props.requestSellerEventData(this.props.session.user.altId);
        }
        if (window) {
            this.setState({ origin: window.location.origin });
        }
        if (sessionStorage.getItem("isEventCreation") != null) {
            this.setState({ isEventCreation: true });
        }
        if (this.props.location && this.props.location.state && this.props.location.state.isStripeSuccess) {
            this.success();
        }
    }

    @autobind
    public onChnageTab(selectedTab) {
        this.setState({ currentTab: selectedTab });
    }

    @autobind
    public handleSelect(row, e, button) {
        var selectedPlace = this.props.seller.events.events.filter(function (item) {
            return item.name == row.name
        });
        let myfeel = this.props.seller.events.myFeelDetails.filter((item) => {
            return item.name == row.name
        });
        if (myfeel[0].parentCategoryId == 98) {
            this.props.history.push({ pathname: `/edit-event/` + selectedPlace[0].altId, state: { selectedPlaceName: selectedPlace[0].name, selectedPlaceAltId: selectedPlace[0].altId } });
        } else {
            this.props.history.push({ pathname: `/edit/` + selectedPlace[0].altId, state: { selectedPlaceName: selectedPlace[0].name, selectedPlaceAltId: selectedPlace[0].altId } });
        }
    }

    @autobind
    public onAPICall() {
        if (this.props.session.isAuthenticated) {
            this.props.requestSellerEventData(this.props.session.user.altId);
            this.setState({ isAPICall: true });
        }
    }

    public parseDateLocal(s) {
        var b = s.split(/\D/);
        var date = new Date(s);
        return new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000)
    }

    success = () => {
        let that = this;
        Modal.success({
            title: 'Event submitted successfully for approval.',
            centered: true,
            onOk: () => {
                that.setState({ isStripeSuccess: false })
            }
        });
    }

    render() {
        var columns = [
            {
                Header: "By Place Name",
                accessor: "name"
            },
            {
                Header: "Date and Time Created",
                accessor: "dateTime"
            },
            {
                Header: "Action",
                accessor: "button",
                Cell: ({ row }) => (
                    <div className="dropdown text-center">
                        <button className="btn btn-sm btn-primary dropdown-toggle" type="button" id="placeSubmitButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Action
                        </button>
                        <div className="dropdown-menu dropdown-menu-right" aria-labelledby="placeSubmitButton">
                            <a onClick={this.handleSelect.bind(this, row, buttonType.Edit)} className="dropdown-item" href="javascript:void(0)">Edit </a>
                        </div>
                    </div>
                )
            }
        ];
        if (this.props.session.isAuthenticated && !this.state.isAPICall) {
            this.onAPICall();
        }
        var that = this;
        var tableData = [];
        var data = this.props.seller.events;
        var id = [];
        var name = [];
        var dateTime = [];
        var that = this;
        var currentTabdata = [];

        if (this.state.currentTab == tab.pastTab) {
            currentTabdata = data.events.filter(function (item) {
                let findDisabledEventDetails = that.props.seller.events.eventDetails && that.props.seller.events.eventDetails.filter(val => {
                    return val.eventId == item.id && !val.isEnabled
                });
                let findAllEventDetails = that.props.seller.events.eventDetails && that.props.seller.events.eventDetails.filter(val => {
                    return val.eventId == item.id
                });
                if (!item.isEnabled && findDisabledEventDetails && findDisabledEventDetails.length == findAllEventDetails.length) {
                    return item
                }
            });
        }

        if (this.state.currentTab == tab.liveTab) {
            currentTabdata = data.events.filter(function (item) {
                if (item.isEnabled) {
                    return item
                }
            });
        }

        if (this.state.currentTab == tab.submitForApproval) {
            currentTabdata = data.events.filter(function (item) {
                let findDisabledEventDetails = that.props.seller.events.eventDetails && that.props.seller.events.eventDetails.filter(val => {
                    return val.eventId == item.id && val.isEnabled
                });
                if (!item.isEnabled && findDisabledEventDetails && findDisabledEventDetails.length > 0) {
                    return item
                }
            });
        }

        currentTabdata.map(function (item, index) {
            var date = that.parseDateLocal(item.createdUtc).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + that.parseDateLocal(item.createdUtc).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(that.parseDateLocal(item.createdUtc).getHours()).format('00') + ":" + numeral(that.parseDateLocal(item.createdUtc).getMinutes()).format('00');
            id.push(index + 1);
            name.push(item.name);
            dateTime.push(date);
        });

        for (var i = 0; i < currentTabdata.length; i++) {
            let newData = {
                id: id[i],
                name: name[i],
                dateTime: dateTime[i],
            };
            tableData.push(newData);
        }

        if (this.props.seller.isFetchSuccess) {
            return <div className="myplaces  position-relative">
                <div className="row position-relative page-banner border shadow">
                    <div className="w-100"><img className="w-100" src={`${gets3BaseUrl()}/feelAdmin/my-feels-banner.jpg`} /></div>
                </div>
                {(data.events.length > 0) && <nav className="pb-4">
                    <div className="nav justify-content-center text-uppercase fee-tab" id="nav-tab" role="tablist">
                        <a onClick={() => this.onChnageTab(tab.draft)} className={`nav-item nav-link ${this.state.currentTab == tab.draft ? 'active' : ''}`} id="nav-Inventory-tab" data-toggle="tab" href="#nav-Inventory" role="tab" aria-controls="nav-Inventory" aria-selected="true">Draft</a>
                        <a onClick={() => this.onChnageTab(tab.submitForApproval)} className={`nav-item nav-link ${this.state.currentTab == tab.submitForApproval ? 'active' : ''}`} id="nav-Inventory-tab" data-toggle="tab" href="#nav-Inventory" role="tab" aria-controls="nav-Inventory" aria-selected="true">Awaiting Approval</a>
                        <a onClick={() => this.onChnageTab(tab.liveTab)} className={`nav-item nav-link ${this.state.currentTab == tab.liveTab ? 'active' : ''}`} id="nav-Details-tab" data-toggle="tab" href="#nav-Details" role="tab" aria-controls="nav-Details" aria-selected="false">Published</a>
                        <a onClick={() => this.onChnageTab(tab.pastTab)} className={`nav-item nav-link ${this.state.currentTab == tab.pastTab ? 'active' : ''}`} id="nav-contact-tab" data-toggle="tab" href="#nav-contact" role="tab" aria-controls="nav-contact" aria-selected="false">Past</a>
                    </div>
                </nav>}
                {(data.events.length > 0) && <div>
                    <ReportingTableComponent
                        myTableData={tableData}
                        myTableColumns={columns}
                        isMyFeel={true} />
                </div>}
                {(data.events.length == 0) && <div className="text-center">
                    <p><i className="fa fa-map-marker fa-4 text-danger" aria-hidden="true"></i></p>
                    <h5>Make it happen!</h5>
                    <p>FeelitLIVE makes it easy to plan, promote and manage your places.</p>
                </div>}
                < div className="text-center pt-3 pb-3">
                    <Link to={that.state.isEventCreation ? "/host-online" : "/createplace"} className="btn btn-primary"><i className="fa fa-plus"></i> Add New</Link>
                </div>
            </div>
        } else {
            return <FILLoader />
        }
    }
}

export default connect(
    (state: IApplicationState) => ({ session: state.session, seller: state.sellar }),
    (dispatch) => bindActionCreators({ ...sessionActionCreators, ...SellerStore.actionCreators }, dispatch)
)(MyFeels);