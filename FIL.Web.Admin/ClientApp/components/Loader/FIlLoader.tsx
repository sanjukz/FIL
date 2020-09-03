import * as React from "react";
import "./FILLoader.scss";

export default class FILLoader extends React.Component<any, any> {            
    public render() {        
        return <div className="site-loader">
            <div className="loader-content">
                <i className="fa fa-circle-o-notch fa-spin"></i>
                <p style={{ fontSize: "13px" }}> Please wait... </p>
            </div>
        </div>;
    }
}
