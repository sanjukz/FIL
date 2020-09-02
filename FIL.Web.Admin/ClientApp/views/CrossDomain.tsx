import * as React from "react";
import { connect } from "react-redux";
import { Link, RouteComponentProps } from "react-router-dom";
import { LoginFormDataViewModel } from "shared/models/LoginFormDataViewModel";
import { IApplicationState } from "../stores";
import * as parse from "url-parse";
import * as LoginStore from "../stores/Login";
import "./Login.scss";
import Spinner from "../components/Spinner";
import { LoginResponseViewModel } from "../../../shared/models/LoginResponseViewModel";

type LoginProps =
  LoginStore.ILoginState &
  typeof LoginStore.actionCreators &
  RouteComponentProps<{ altId: string; }>;

class CrossDomain extends React.Component<LoginProps, {}> {
  state = {
    view: ""
  };

  componentDidMount() {
    localStorage.removeItem('altId');
    localStorage.removeItem('roleId');
    localStorage.removeItem("isLoggedIn");
    const data: any = parse(location.search, location, true);
    if (data.query.view && data.query.view == "2") {
      sessionStorage.setItem("isFeelguide", "true");
    }

    if (data.query.view) {
      this.setState({
        view: data.query.view
      });
    }

    if (this.props.match.params.altId != undefined) {
      var altId = this.props.match.params.altId;
      this.onSubmitLogin();
    }
  }
  render() {
    return <div>
      <Spinner isShowLoadingMessage={true} />
    </div>;
  }

  onSubmitLogin = () => {
    var values: LoginFormDataViewModel = {
      email: this.props.match.params.altId,
      password: '428D28C9-54EC-487F-845E-06EB1294747E'
    }

    this.props.login(values, (response: LoginResponseViewModel) => {
      if (response.success) {
        sessionStorage.setItem("isLoggedIn", "true");
        sessionStorage.setItem("isReload", "true");
        if (this.state.view == "3") {
          sessionStorage.setItem("isEventCreation", "true");
          window.location.replace(response.session.isFeelExists ? '/' : '/host-online/basics')
        } else if (this.state.view == "4") {
          sessionStorage.setItem("isEventCreation", "true");
          window.location.replace("/transactionreport")
        } else {
          window.location.replace("/createplace")
        }
      }
    });
  }
}

export default connect(
  (state: IApplicationState) => state.login,
  LoginStore.actionCreators
)(CrossDomain);
