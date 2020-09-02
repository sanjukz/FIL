import * as React from "react";
import { HeaderNavButtons } from "../components/HeaderNavButtons";

export class Subheader extends React.Component<any, any> {
    public render() {
        return <div className="bg-dark text-left container-fluid sub-head">
            <div className="row">
                <div className="container">
                    <div className="col-sm-5">
                        <span>{this.props.breadcrumbName}</span>{this.props.breadcrumbDetail}
                    </div>
                    <HeaderNavButtons headerNavButtonsData={this.props.headerNavButtonsData} />
                </div>
            </div>
        </div>;
    }
}
