import * as React from "react";
import * as IVenueComponentState from "../stores/VenueData";
import Select from "react-select";

type VenueComponentStateProps = IVenueComponentState.IVenueComponentState & typeof IVenueComponentState.actionCreators;

export class CountryComponent extends React.Component<any, any>{
    public render() {
        const countrySelect = this.props.options.map(function (country) {
            return { "id": country.id, "label": country.name };
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
                    options={countrySelect}
                    value={this.props.selected}
                    onChange={this.props.changeValue}
                    placeholder="select country"
                />
            </div>
        </div>;
    }
}
