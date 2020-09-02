import * as React from "react";
import "./KzLoader.scss";

export default class KzLoader extends React.Component<any, any> {            
    public render() {        
        return <div className="site-loader">
            <div className="loader-content">
                <i className="fa fa-circle-o-notch fa-spin"></i>
                <p style={{ fontSize: "13px" }}> Please wait... </p>
            </div>
        </div>;
    }
}
