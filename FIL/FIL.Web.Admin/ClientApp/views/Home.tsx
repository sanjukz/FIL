import * as React from "react";
import { RouteComponentProps, Link } from "react-router-dom";

type HomeProps =  RouteComponentProps<{}>;

export default class Home extends React.Component<HomeProps, any> {
    constructor(props) {
        super(props);
        this.state = { isToggleOn: true };

        // This binding is necessary to make `this` work in the callback
        this.handleClick = this.handleClick.bind(this);
    }

    public componentDidMount() {
    }

    handleClick() {
        this.setState(prevState => ({
            isToggleOn: !prevState.isToggleOn
        }));
    }

    render() {
        return (
            <div className="text-center container-fluid">
                <div className="container">
                    <h1> Welcome to the admin dashboard</h1>
                </div>
            </div>
        );
    }
}

