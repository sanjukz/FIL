import * as React from "react";
import { Link } from "react-router-dom";
import { gets3BaseUrl } from "../utils/imageCdn";

export class Sidebar extends React.Component<{}, {}> {

    public render() {
        return <nav id="sidebar">
            <div className="sidebar-header p-2 m-3 border shadow bg-white rounded text-purple">
                <h3 className="m-0 text-center">
                    Host a feel
                </h3>
                <strong>
                    <img src={`${gets3BaseUrl()}/feelAdmin/feel-heart-white-logo.png`} alt="logo1" width="30" />
                </strong>
            </div>
            {/* <div className="dashboard-btn p-3">
                <a href="#"><i className="menu-icon fa fa-dashboard mr-2"></i>Dashboard</a>
            </div> */}
            <ul className="list-unstyled components">
                <li className="active">
                    <a href="#feelPlaceManagement" data-toggle="collapse" aria-expanded="false">
                        <i className="fa fa-map-marker" aria-hidden="true"></i>
                        feel/place Management
                    </a>
                    <ul className="collapse list-unstyled" id="feelPlaceManagement">
                        <li><Link to="/createplace">Add/New</Link></li>
                        <li><Link to="/eventlisting">Edit</Link></li>
                        <li><Link to="/myfeels">My feels</Link></li>
                        <li><a href="/approve-moderate">Approve/Moderate</a></li>
                        {/* <li><Link to="/eventsorting">Sequence</Link></li> */}
                        <li><Link to="/eventsorting">Site Mapping</Link></li>
                    </ul>
                </li>
                <li>
                    <a href="#UserManagement" data-toggle="collapse" aria-expanded="false">
                        <i className="fa fa-users" aria-hidden="true"></i>
                        User Management
                    </a>
                    <ul className="collapse list-unstyled" id="UserManagement">
                        <li><Link to="/invitemanager" >Manage Invites </Link></li>
                    </ul>
                </li>
                <li>
                    <a href="#TransactionReport" data-toggle="collapse" aria-expanded="false">
                        <i className="fa fa-bar-chart" aria-hidden="true"></i>
                        Reports
                    </a>
                    <ul className="collapse list-unstyled" id="TransactionReport">
                        <li><Link to="/transactionreport" >Transaction</Link></li>
                    </ul>
                </li>
            </ul>
        </nav>;
    }
}
