import * as React from "react";
import { Link, RouteComponentProps } from "react-router-dom";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import * as InviteStore from "../stores/InviteManager";
import TableComponent from "../components/TableComponent";
import "shared/styles/globalStyles/main.scss";
import "./EventTicket.scss";

type InviteComponentStateProps = InviteStore.IInviteManagerComponentState & typeof InviteStore.actionCreators & RouteComponentProps<any>;

export class InviteManager extends React.Component<InviteComponentStateProps, InviteStore.IInviteManagerComponentState> {
    public componentWillMount() {
        this.props.requestInviteData();
        this.props.InviteSummaryData();
    }

    public searchquery = "";
    public isused = false;
    constructor(props) {
        super(props);
        this.handlechange = this.handlechange.bind(this);
        this.handlechangeIsused = this.handlechangeIsused.bind(this);
    }

    public showscreen = false;
    public componentDidMount() {
        var roleId = localStorage.getItem('roleId');
        this.showscreen = true;
    }

    public handlechange(event) {

        let val = event.target.value;
        this.searchquery = val;
        if (val.length > 3) {
            this.props.searchInvitedata(val + "," + this.isused);
        }
    }

    public handlechangeIsused(event) {

        let val = event.target.checked;
        this.isused = val;
        this.props.searchInvitedata(this.searchquery + "," + val);
    }


    public render() {



        const columns = [{
            Header: "Email",
            accessor: "userEmail"
        }, {
            Header: " Is used",
            accessor: "isUsed",
            Cell: row => (<i className={row.value ? "fa fa-check" : "fa fa-ban"}
                style={{
                    color: row.value ? '#85cc00'
                        : '#ff2e00'
                }}
            >
            </i>),
            maxWidth: 100,
            filterable: false
        }, {
            Header: "Invite code",
            accessor: "userInviteCode"
        }, {
            Header: "Action",
            accessor: "id",
            Cell: row => (<span><Link style={{ marginRight: 10 }} onClick={() => this.props.navigateToEdit(row)} to={"/editinvite/" + row.value} > <i className="fa fa-edit" aria-hidden="true"></i> Edit </Link>
                |<span style={{ marginLeft: 10, color: "#337ab7", cursor: "pointer" }} onClick={() => this.props.sendEmail(row)} > <i className="fa fa-envelope" aria-hidden="true"></i> Send email </span>
                {this.props.isEmailSent && this.props.sentId === row.value ? <div className="alert alert-success p-10 mt-10 text-left EmailSentalert"><small>Email Sent Successfully</small></div>
                    : null}
                {this.props.isFailEmail && this.props.sentId === row.value ? <div className="alert alert-danger p-10 mt-10 text-left EmailSentalert"><small>Email Sending Failed</small></div>
                    : null}</span>
            ),
            filterable: false
        }];


        const data = this.props.result.invites;

        return (
          <div className="card border-0 right-cntent-area pb-5 bg-light">
            <div className="card-body bg-light p-0">
              <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box">
                <div className="row">
                    <div className="col-md-4">
                        <div className="card-counter primary">
                            <i className="fa fa-database"></i>
                            <span className="count-numbers">{this.props.summary.totalMails}</span>
                            <span className="count-name">Total Mails</span>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="card-counter danger">
                            <i className="fa fa-envelope"></i>
                            <span className="count-numbers">{this.props.summary.usedMails}</span>
                            <span className="count-name">Used</span>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="card-counter success">
                            <i className="fa fa-ban"></i>
                            <span className="count-numbers">{this.props.summary.unUsedMails}</span>
                            <span className="count-name">UnUsed</span>
                        </div>
                    </div>


                    {/* </div> */}

                </div>

                <div className="d-inline">
                    <div className="content-area">
                        <div className="tab-content bg-white rounded shadow-sm p-10 mt-3 mb-3">
                            <input placeholder="type here to search" className="form-control search" type="text" onChange={this.handlechange} />
                        </div>
                    </div>
                </div>

                <div className="wrapper">
                    {this.showscreen ?
                        <div className="w-100">
                            <div className="row">
                                <div className="col"><h4> Manage Invites </h4></div>
                                <div className="col text-right">
                                    <Link to="/addinvite" className="btn btn-primary">Create</Link>
                                </div>
                            </div>

                            <div className="table table-striped table-bordered example-table">
                                <TableComponent myTableData={data} myTableColumns={columns} />
                            </div>
                        </div>
                        : null}

                </div>
          </div>
        </div >
      </div>
        );
    }
}
export default connect(
    (state: IApplicationState) => state.invites,
    InviteStore.actionCreators
)(InviteManager);
