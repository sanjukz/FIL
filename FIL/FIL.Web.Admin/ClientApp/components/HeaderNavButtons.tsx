import * as React from "react";
import { NavLink } from "react-router-dom";

export class HeaderNavButtons extends React.Component<any, any> {
    public render() {
        var buttonList = this.props.headerNavButtonsData.map(function (val) {
            return <li className="btn-custom"><NavLink to={"/" + val.ButtonName.replace(" ", "").toLowerCase()}>{val.ButtonName}</NavLink></li>
        });
        return <div className="col-sm-7 text-right">
            <div className="btn-group">
                {buttonList}
            </div>
        </div>;
    }
}
