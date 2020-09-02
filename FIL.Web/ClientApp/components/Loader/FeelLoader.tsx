import * as React from "react";
import "./FeelLoader.scss";

export default class KzLoader extends React.Component<any, any> {
    public render() {
        return <div className="site-loader">
            <div className="loader-content">
                <img src="https://s3-us-west-2.amazonaws.com/zoonga-cdn/Images/rasv/rasv-loading.gif" />
                <p style={{ fontSize: "13px" }}>
                    Please wait...
                </p>
            </div>
        </div>
    }
}
