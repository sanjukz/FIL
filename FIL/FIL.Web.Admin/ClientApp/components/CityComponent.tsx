import * as React from "react";
import Select from "react-select";

export class CityComponent extends React.Component<any, any>{
    public render() {
        return <div className="container-fluid search-head">
            <div className="input-group pull-right">
                <div className="input-group-btn">
                    <button type="button" className="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <span className="caret"></span></button>
                    <ul className="dropdown-menu">
                    </ul>
                </div>
                <Select
                    name="form-field-name"
                    placeholder="select City"
                />
            </div>
        </div>;
    }
}
