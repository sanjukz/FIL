import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../../stores";
import { bindActionCreators } from "redux";
import { RouteComponentProps } from "react-router-dom";
import ReportingTableComponent from "../../../../ClientApp/components/TableComponent";
import { ApproveStatus } from "../../../../ClientApp/models/Redemption/ApproveStatus";
import FILLoader from "../../../../ClientApp/components/Loader/FILLoader";
import * as numeral from "numeral";
import classnames from "classnames";
import * as RedemptionStore from "../../../stores/Redemption";
import { ParseDateToLocal } from "../../../utils/ParseDateToLocal";
import { ConfirmGuideResponseModel } from "../../../models/Redemption/ConfirmGuideResponseModel";
import * as _ from "lodash";

type approveModerateGuide = RedemptionStore.RedemptionComponentProps
    & typeof RedemptionStore.actionCreators
    & RouteComponentProps<{}>

class ApproveModerateGuide extends React.Component<approveModerateGuide, any> {
    constructor(props) {
        super(props)
        this.state = {
            displayScreen: ApproveStatus.Pending,
            isTab1: true,
            isTab2: false
        }
    }

    componentDidMount() {
        this.props.requestAllGuideData(2);
    }

    handleSelect = (e, button, row) => {
        if (button == 3 || button == 4) {
            let findGuide = _.find(this.props.Redemption.allGuides.guideDetails, { email: e.email, placeName: e.placeName });
            this.props.confirmGuide(findGuide.id, button, (item: ConfirmGuideResponseModel) => {
                if (item.success && button == 3) {
                    alert("Guide approved successfully");
                    this.props.requestAllGuideData(2);
                }
            });
        } else if(button == 0) {
            this.props.history.push(`edit-guide/${this.props.Redemption.allGuides.guideDetails[0].altId}`)
        } else {
            return;
        }
    }

    getContentHTML = (tableData, columns) => {
        return (<div>
            {(tableData.length > 0 && this.props.Redemption.fetchAllGuideSuccess && !this.props.Redemption.fetchConfirmRequest && !this.props.Redemption.fetchAllGuideRequest) && <ReportingTableComponent myTableData={tableData} myTableColumns={columns} />}
            {(tableData.length == 0 && this.props.Redemption.fetchAllGuideSuccess && !this.props.Redemption.fetchConfirmRequest && !this.props.Redemption.fetchAllGuideRequest) &&
                <div className="text-center">
                    <div className="pb-3"><img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/feelAdmin/fap-not-found.svg" /></div>
                    <h4>Sorry, there are no {this.state.displayScreen == ApproveStatus.Pending ? 'pending' : 'approved'} guides.</h4>
                    {this.state.displayScreen == ApproveStatus.Pending && <a href="javascript:void(0)" onClick={(e) => {
                        this.props.requestAllGuideData(3);
                        this.setState({ displayScreen: ApproveStatus.Approved })
                    }} className="btn btn-primary mt-3 text-white" >Go to Approved</a>}
                    {this.state.displayScreen == ApproveStatus.Approved && <a href="javascript:void(0)" onClick={(e) => {
                        this.props.requestAllGuideData(2);
                        this.setState({ displayScreen: ApproveStatus.Pending })
                    }} className="btn btn-primary mt-3 text-white">Go to Pending</a>}
                </div>
            }
        </div >)
    }

    render() {
        var columns = [
            {
                Header: "Sr. No.",
                accessor: "id",
                minWidth: 30,
            },
            {
                Header: "Name",
                accessor: "name"
            },
            {
                Header: "Email",
                accessor: "email"
            },
            {
                Header: "Phone",
                accessor: "phone"
            },
            {
                Header: "City",
                accessor: "cityName",
            },
            {
                Header: "State",
                accessor: "stateName",
            },
            {
                Header: "Country",
                accessor: "countryName",
            },
            {
                Header: "Place",
                accessor: "placeName",
            },
            {
                Header: "Created Date",
                accessor: "createdDate",
            },
            {
                Header: "Approved Date",
                accessor: "approvedDate",
            },
            {
                Header: "Approved By",
                accessor: "approvedBy",
            }];
        if (this.state.displayScreen == ApproveStatus.Pending) {
            let data = {
                Header: "Action",
                accessor: "button",
                Cell: ({ row }) => (
                    <div className="dropdown text-center">
                        <button className="btn btn-sm btn-primary dropdown-toggle" type="button" id="placeSubmitButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Action
                        </button>
                        <div className="dropdown-menu dropdown-menu-right" aria-labelledby="placeSubmitButton">
                            <a onClick={this.handleSelect.bind(this, row, ApproveStatus.Approved)} className="dropdown-item" href="javascript:void(0)">Approve</a>
                            <a onClick={this.handleSelect.bind(this, row, ApproveStatus.Pending)} className="dropdown-item" href="javascript:void(0)">Reject </a>
                            <a onClick={this.handleSelect.bind(this, row, ApproveStatus.None)} className="dropdown-item" href="javascript:void(0)">Edit </a>
                        </div>
                    </div>)
            };
            let status = {
                Header: "Status",
                accessor: "status",
            }
            columns.push(status);
            columns.push(data);
        }

        if (this.state.displayScreen == ApproveStatus.Approved) {
            let data = {
                Header: "Status",
                accessor: "button",
                Cell: ({ row }) => (
                    <div className="text-center">
                        <button className="btn btn-sm btn-success " type="button" aria-haspopup="true" aria-expanded="false">
                            Approved
                        </button></div>)
            };
            columns.push(data);
        }

        var tableData = [];
        var data = this.props.Redemption.allGuides;
        var id = [];
        var guideId = [];
        var name = [];
        var email = [];
        var phone = [];
        var cityName = [];
        var stateName = [];
        var countryName = [];
        var placeName = [];
        var createdDate = [];
        var approvedDate = [];
        var approvedBy = [];
        var status = [];

        data.guideDetails.map(function (item, index) {
            var user = data.approvedByUsers.filter(function (val) {
                return val.altId == item.approvedBy
            });
            id.push(index + 1);
            guideId.push(item.id);
            name.push(item.firstName + ' ' + item.lastName);
            email.push(item.email);
            phone.push((item.phonceCode && item.phoneNumber) ? ('+' + item.phonceCode + ' - ' + item.phoneNumber) : (!item.phonceCode && item.phoneNumber) ? item.phoneNumber : '--');
            cityName.push(item.cityName);
            stateName.push(item.stateName);
            countryName.push(item.countryName);
            placeName.push(item.placeName);
            createdDate.push(ParseDateToLocal(item.createdUtc).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + ParseDateToLocal(item.createdUtc).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(ParseDateToLocal(item.createdUtc).getHours()).format('00') + ":" + numeral(ParseDateToLocal(item.createdUtc).getMinutes()).format('00'));
            approvedDate.push(item.approvedUtc ? (ParseDateToLocal(item.approvedUtc).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + ParseDateToLocal(item.approvedUtc).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(ParseDateToLocal(item.approvedUtc).getHours()).format('00') + ":" + numeral(ParseDateToLocal(item.approvedUtc).getMinutes()).format('00')) : "--");
            approvedBy.push(user.length > 0 ? user[0].firstName + " " + user[0].lastName : "--");
            status.push(item.approveStatusId == "None" ? "--" : item.approveStatusId)
        });

        for (var i = 0; i < data.guideDetails.length; i++) {
            let newData = {
                id: id[i],
                guideId: guideId[i],
                name: name[i],
                email: email[i],
                phone: phone[i],
                cityName: cityName[i],
                stateName: stateName[i],
                countryName: countryName[i],
                placeName: placeName[i],
                createdDate: createdDate[i],
                approvedDate: approvedDate[i],
                approvedBy: approvedBy[i],
                status: status[i]
            };
            tableData.push(newData);
        }

        return <div>
            <nav className="pb-4">
                <div className="nav justify-content-center text-uppercase fee-tab" id="nav-tab" role="tablist">
                    <a
                        className={"nav-item nav-link" + (this.state.displayScreen == ApproveStatus.Pending ? ' active' : '')}
                        onClick={(e) => {
                            this.props.requestAllGuideData(2);
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
                            this.props.requestAllGuideData(3);
                            this.setState({ displayScreen: ApproveStatus.Approved })
                        }}
                        id="nav-contact-tab"
                        data-toggle="tab"
                        href="#nav-contact"
                        role="tab"
                        aria-controls="nav-contact">
                        Approved
                        </a>
                </div>
            </nav>
            <div className="tab-content bg-white rounded shadow-sm pt-3" id="nav-tabContent">
                {this.state.displayScreen == ApproveStatus.Pending && <div
                    className={"tab-pane fade show" + (this.state.displayScreen == ApproveStatus.Pending ? ' active' : '')}
                >
                    {this.getContentHTML(tableData, columns)}
                </div>}
                {this.state.displayScreen == ApproveStatus.Approved && <div
                    className={"tab-pane fade show" + (this.state.displayScreen == ApproveStatus.Approved ? ' active' : '')}
                >
                    {this.getContentHTML(tableData, columns)}
                </div>}
            </div>
            {(this.props.Redemption.fetchConfirmRequest || this.props.Redemption.fetchAllGuideRequest) && <FILLoader />}
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
)(ApproveModerateGuide);
