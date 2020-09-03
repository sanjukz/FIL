import * as React from "react";
import "./FeelLoader.scss";

export default class FilLoader extends React.Component<any, any> {
    public render() {
        return <div className="site-loader">
            <div className="loader-content">
                <img src="https://static5.feelitlive.com/Images/rasv/rasv-loading.gif" />
                <p style={{ fontSize: "13px" }}>
                    Please wait...
                </p>
            </div>
        </div>
    }
}
