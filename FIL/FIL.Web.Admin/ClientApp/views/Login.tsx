import { autobind } from "core-decorators";
import * as React from "react";
import { connect } from "react-redux";
import { Link, RouteComponentProps } from "react-router-dom";
import { LoginFormDataViewModel } from "shared/models/LoginFormDataViewModel";
import { LoginForm } from "../components/form/LoginForm";
import { IApplicationState } from "../stores";
import * as LoginStore from "../stores/Login";
import "./Login.scss";

type LoginProps = LoginStore.ILoginState & typeof LoginStore.actionCreators & RouteComponentProps<{}>;

class Login extends React.Component<LoginProps, {}> {
    public componentDidMount() {
        localStorage.removeItem('altId');
        localStorage.removeItem('roleId');
        localStorage.removeItem("isLoggedIn");
    }
    public render() {
        return <div>
            <h5 className="site-top-title text-center">Feel Suite<sup className="trademark">TM</sup></h5>
            <div className="text-center form-box">
                <h5>Sign In</h5>
                <LoginForm onSubmit={this.onSubmitLogin} />
                {(this.props.invalidCredentials == true) && <span className="validate-txt"><br /><p className="text-danger">Invalid email address or password</p></span>}
            </div>
        </div>;
    }

    @autobind
    private onSubmitLogin(values: LoginFormDataViewModel) {
        this.props.login(values, (response) => {
            if (response.success) {
                sessionStorage.setItem("isLoggedIn", "true");
                if (localStorage.getItem("roleId") == "2") {
                    this.props.history.replace("/myfeels");
                }
                this.props.history.push("/");
                window.location.reload();             
            }
        });
    }
}

export default connect(
    (state: IApplicationState) => state.login,
    LoginStore.actionCreators
)(Login);
