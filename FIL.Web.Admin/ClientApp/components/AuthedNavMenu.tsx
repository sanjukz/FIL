import * as React from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import * as SessionState from "shared/stores/Session";
import { IApplicationState } from "../stores";
import axios from "axios";
import { gets3BaseUrl } from "../utils/imageCdn";
import * as AuthedRoleFeatureState from "../stores/AuthedRoleFeature";
import ImageUpload from "./ImageUpload/ImageUpload";

type UserProps = SessionState.ISessionProps &
    AuthedRoleFeatureState.IAuthedNavMenuFeatureState &
    typeof SessionState.actionCreators &
    typeof AuthedRoleFeatureState.actionCreators;

class AuthedNavMenu extends React.Component<UserProps, any> {
    inputElement: any;
    constructor(props) {
        super(props);
    }

    public componentDidMount() {
        localStorage.setItem("altId", this.props.session.user.altId);
        localStorage.setItem("roleId", this.props.session.user.rolesId.toString());
        this.props.getRoleFeatures(this.props.session.user.altId);
    }

    handleImageUpload = (imageModel) => {
        var formData = new FormData();
        formData.append("file", imageModel.file);
        axios
            .post("/api/upload/profilepicture", formData, {
                headers: {
                    "Content-Type": "multipart/form-data"
                }
            })
            .then(response => {
                if (response.data == true) {
                }
            });
    }

    getMyAccountLink = () => {
        if (typeof window !== 'undefined') {
            switch (window.location.origin) {
                case "https://host.feelitlive.com":
                    return "https://www.feelitlive.com/account";
                case "https://admin.feelitlive.com":
                    return "https://dev.feelitlive.com/account";
                default:
                    return "https://dev.feelitlive.com/account";
            }
        }
    }

    public render() {
        return (
            <div>
                <nav className="navbar navbar-default bg-transparent shadow-none">
                    <div className="container-fluid p-0">
                        <div className="navbar-header w-100">
                            <a
                                className="btn btn-info navbar-btn menutoggle d-md-none"
                                data-toggle="collapse"
                                href="#sidebar"
                                role="button"
                                aria-expanded="false"
                                aria-controls="sidebar"
                            >
                                <i className="fa fa fa-tasks"></i>
                            </a>
                            <div className="pull-right rounded border border-primary p-1 bg-light">
                                <div className="dropleft pull-right">
                                    <a
                                        href="#"
                                        id="dropdownMenuLink"
                                        data-toggle="dropdown"
                                        aria-haspopup="true"
                                        aria-expanded="false"
                                    >
                                        <img
                                            src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/user-profile/fee-review-user-icon.png"
                                            className="rounded-circle"
                                            width="30"
                                            height="30"
                                            alt=""
                                        />
                                    </a>
                                    <div
                                        className="dropdown-menu p-0"
                                        aria-labelledby="dropdownMenuButton"
                                    >
                                        <ImageUpload
                                            imageInputList={[{
                                                imageType: "profile",
                                                numberOfFields: 1,
                                                imageKey: this.props.session.user && this.props.session.user.altId
                                            }]}
                                            onImageSelect={this.handleImageUpload}
                                        />
                                        <a
                                            className="dropdown-item pl-2 pr-2 border-bottom"
                                            href={this.getMyAccountLink()}
                                        >
                                            {this.props.session.user.firstName}
                                        </a>
                                        <a
                                            className="dropdown-item pl-2 pr-2 border-bottom"
                                            href={this.getMyAccountLink()}
                                        >
                                            My Account
                                        </a>
                                        <a
                                            className="dropdown-item pl-2 pr-2 border-bottom"
                                            href="https://www.feelitlive.com/"
                                        >
                                            Switch to "FeelitLIVE"
                                        </a>
                                        <Link
                                            className="dropdown-item pl-2 pr-2"
                                            to="/login"
                                            onClick={this.props.logout}
                                        >
                                            Logout
                                        </Link>
                                    </div>
                                </div>
                            </div>
                            <div className="pull-right pr-3">
                                <div className="position-relative">
                                    <button type="button" className="btn text-purple" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        <i className="fa fa-question-circle" aria-hidden="true"></i>
                                    </button>
                                    <div className="dropdown-menu p-1">
                                        <a href="/terms-and-conditions" target="_blank" className="dropdown-item small p-0 px-2 btn-link">
                                            <i className="fa fa-file-text-o mr-2" aria-hidden="true"></i>
                                            Additional Terms &amp; Conditions</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </nav>
            </div>
        );
    }
}
export default connect(
    (state: IApplicationState) => ({
        session: state.session,
        ...state.authedRoleFeature
    }),
    { ...SessionState.actionCreators, ...AuthedRoleFeatureState.actionCreators }
)(AuthedNavMenu);
