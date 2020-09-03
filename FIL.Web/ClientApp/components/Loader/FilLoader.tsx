import * as React from "react";
import "./FilLoader.scss";

export default class FilLoader extends React.Component<any, any> {            
    public render() {        
        return <div className="site-loader">
            <div className="loader-content">
                <i className="fa fa-circle-o-notch fa-spin"></i>
                <p style={{ fontSize: "13px" }}> Please wait... </p>
            </div>
        </div>;
    }
}
