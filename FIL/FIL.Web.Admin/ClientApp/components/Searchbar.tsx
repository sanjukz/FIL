import * as React from "react";

export class Searchbar extends React.Component<{}, {}> {
    public render() {
        return <div className="container-fluid search-head">
            <div className="navbar-header">
                <button type="button" id="sidebarCollapse" className="btn btn-info navbar-btn">
                    <i className="glyphicon glyphicon-align-left"></i>
                    <span>Toggle Sidebar</span>
                </button>
            </div>
            <div className="input-group pull-right">
                <div className="input-group-btn">
                    <button
                        type="button"
                        className="btn btn-default dropdown-toggle"
                        data-toggle="dropdown"
                        aria-haspopup="true"
                        aria-expanded="false">
                        Venues <span className="caret" />
                    </button>
                    <ul className="dropdown-menu">
                        <li><a href="#">Venues</a></li>
                        <li><a href="#">Another action</a></li>
                        <li><a href="#">Something else here</a></li>
                        <li role="separator" className="divider"></li>
                        <li><a href="#">Separated link</a></li>
                    </ul>
                </div>
                <input type="text" className="form-control" aria-label="..." />
            </div>
        </div>;
    }
}
