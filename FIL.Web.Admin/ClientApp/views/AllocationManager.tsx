import * as React from "react";
import { Subheader } from "../components/Subheader";
import { Sidebar } from "../components/Sidebar";
import { Searchbar } from "../components/Searchbar";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import * as IAllocationComponentState from "../stores/AllocationManager";
import TableComponent from "../components/TableComponent";
import "shared/styles/globalStyles/main.scss";
import "./EventTicket.scss";

type AllocationComponentStateProps = IAllocationComponentState.IAllocationComponentState & typeof IAllocationComponentState.actionCreators;

const columns = [{
    Header: "Stand",
    accessor: "stand"
}, {
    Header: "Block",
    accessor: "block"
}, {
    Header: "Level",
    accessor: "level"
}, {
    Header: "Tickets Available",
    accessor: "ticketsAvailable"
}, {
    Header: "TBD",
    accessor: "tbd"
}, {
    Header: "Action",
    Cell: (props) => <span className="number"><a href="#">View</a></span>
}];

const headerNavButtonsData = [{
    ButtonName: "Handover Sheet"
}, {
    ButtonName: "Seat Layout"
}];

export class AllocationManager extends React.Component<AllocationComponentStateProps, IAllocationComponentState.IAllocationComponentState> {
    public componentWillMount() {
        this.props.requestAllocationdata();
    }
    public render() {
        const data = this.props.allocations.allocationsData;
        return <div>
            <Subheader breadcrumbName={"Events"} breadcrumbDetail={"Allocation Manager"} headerNavButtonsData={headerNavButtonsData} />
            <div className="wrapper">
                <Sidebar />
                <div id="content">
                    <Searchbar />
                    <h3>Collapsible Sidebar Using Bootstrap 3</h3>
                    <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                    <div className="table table-striped table-bordered example-table">
                        <TableComponent myTableData={data} myTableColumns={columns} />
                    </div>;
                </div>
            </div>
        </div>;
    }
}
export default connect(
    (state: IApplicationState) => state.allocations,
    IAllocationComponentState.actionCreators
)(AllocationManager);
