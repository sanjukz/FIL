import * as React from "react";
import { Subheader } from "../components/Subheader";
import { Sidebar } from "../components/Sidebar";
import { Searchbar } from "../components/Searchbar";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import * as IEventComponentState from "../stores/EventData";
import TableComponent from "../components/TableComponent";
import "shared/styles/globalStyles/main.scss";
import "./EventTicket.scss";

type EventComponentStateProps = IEventComponentState.IEventComponentState & typeof IEventComponentState.actionCreators;

const columns = [{
    Header: "Block",
    accessor: "block"
}, {
    Header: "Stand",
    accessor: "stand"
}, {
    Header: "Level",
    accessor: "level"
}, {
    Header: "Capacity",
    accessor: "capacity"
}, {
    Header: "TBD",
    accessor: "tbd"
}];

const headerNavButtonsData = [{
    ButtonName: "Handover Sheet"
}, {
    ButtonName: "Seat Layout"
}, {
    ButtonName: "Allocation Manager"
}];

export class EventTickets extends React.Component<EventComponentStateProps, IEventComponentState.IEventComponentState> {
    public componentWillMount() {
        this.props.requestEventdata();
    }
    public render() {
        const data = this.props.events.venues;
        return <div>
            <Subheader breadcrumbName={"Ticket"} breadcrumbDetail={"EventTicket"} headerNavButtonsData={headerNavButtonsData} />
            <div className="wrapper">
                <Sidebar />
                <div id="content">
                    <Searchbar />
                    <TableComponent myTableData={data} myTableColumns={columns} />
                </div>
            </div>
        </div>;
    }
}
export default connect(
    (state: IApplicationState) => state.events, // Selects which state properties are merged into the component's props
    IEventComponentState.actionCreators                 // Selects which action creators are merged into the component's props
)(EventTickets);
