import * as React from "react";
import Select from "react-select";

export class ZipcodeComponent extends React.Component<any, any>{
    public render() {
        const zipcodeSelect = this.props.options.map(function (zip) {
            return { "id": zip.id, "label": zip.postalcode };
        });
        return <div className="container-fluid search-head">
            <div className="input-group pull-right">
                <div className="input-group-btn">
                    <button type="button" className="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <span className="caret"></span></button>
                    <ul className="dropdown-menu">
                    </ul>
                </div>
                <Select
                    name="form-field-name"
                    options={zipcodeSelect}
                    value={this.props.selected}
                    onChange={this.props.changeValue}
                    placeholder="select Zipcode"
                />
            </div>
        </div>;
    }
}
