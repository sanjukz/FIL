import * as React from "react";
import Select from "react-select";

export default class ReportingSubEventComponent extends React.Component<any, any>{
    public render() {
        const eventSelect = this.props.options.map(function (event) {
            if (event.id != null) {
                return { "id": event.id, "label": event.name };
            }
            else {
                return { "label": event };
            }
        });

        return <div>
            <Select
                name="form-field-name"
                options={eventSelect}
                value={this.props.selected}
                onChange={this.props.changeValue}
                placeholder={this.props.placeholder}
            />
        </div>;
    }
}

