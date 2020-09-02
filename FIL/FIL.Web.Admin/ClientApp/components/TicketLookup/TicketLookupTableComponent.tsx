import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";

export default class TicketLookupTableComponent extends React.Component<any, any> {
    public render() {
        const data = this.props.myTableData;
        const columns = this.props.myTableColumns;
        var defaultPageSize;
        if (data.length <= 1) {
            defaultPageSize = 1
        }
        else {
            defaultPageSize = 10
        }
        return <div className="table table-striped table-bordered example-table">
            <ReactTable
                data={data}
                columns={columns}
                defaultPageSize={defaultPageSize}
                filterable={true}
                minRows={0}
                className="-striped -highlight"
            />
        </div>;
    }
}