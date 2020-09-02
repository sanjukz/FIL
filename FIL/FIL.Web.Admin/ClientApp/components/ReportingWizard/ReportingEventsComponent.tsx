import * as React from "react";
import Select from "react-select";

export default class ReportingEventsComponent extends React.Component<any, any>{
    public render() {
        const eventSelect = this.props.options.map(function (event) {
             if (event.altId != null) {
                return { "altId": event.altId, "label": event.name };
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
                placeholder="Please enter event name"
            />
        </div>;
    }
}

