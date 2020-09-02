import * as React from "react";
import { connect } from "react-redux";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import { IApplicationState } from "../stores";
import GoogleTagManager from "./analytics/GoogleTagManager";
import FooterBarV1 from "./footer/FooterBarV1";
import NavMenuV1 from "../components/NavMenu/NavMenuV1";
import { CategoryViewModel } from "../models/CategoryViewModel";
import {
    actionCreators as categoryActionCreators,
    ICategoryProps
} from "../stores/Category";
import { bindActionCreators } from "../../node_modules/redux";
import { actionCreators as nearMeActionCreators, IFeelNearMeProps, KnownAction } from "../stores/FeelNearMe";
import SignInSignUp from "./SignInSignUp";
import AuthedNavMenuV1 from "./NavMenu/AuthedNavMenuV1";
import * as PubSub from 'pubsub-js';

interface ILayoutProps {
    gtmId: string;
    categories: CategoryViewModel[];
    url: string;
}

type LayoutProps = ILayoutProps &
    ISessionProps &
    typeof sessionActionCreators &
    ICategoryProps &
    typeof categoryActionCreators &
    IFeelNearMeProps &
    typeof nearMeActionCreators;

class Layout extends React.PureComponent<LayoutProps, any> {

    state = {
        isSiginSignUpShow: false,
        isLoginPage: false,
        showSignInModal: true,
        redirectUrl: ""
    }
    private getMenu() {
        let isHome = this.props.url == "/";
        if ((typeof window !== "undefined") &&
            (window.location.pathname.indexOf("host-a-feel") == -1 && window.location.pathname.indexOf("create-online-experience") == -1)) {
            if (this.props.session.isAuthenticated) {
                return <AuthedNavMenuV1
                    isOverlay={isHome}
                    categories={this.props.categories}
                    onLogout={(e) => this.logout(e)}
                    session={this.props.session}
                    requestSiteContent={this.props.requestSiteContent}
                    clearSearchAction={this.props.clearSearchAction}
                    searchSuccess={this.props.category.searchSuccess}
                    clearSearchSuccess={this.props.category.clearSearchSuccess}
                    searchResult={this.props.category.searchResult}
                    content={this.props.category.content}
                    defaultSearchCities={this.props.category.defaultSearchCities}
                    defaultSearchStates={this.props.category.defaultSearchStates}
                    defaultSearchCountries={this.props.category.defaultSearchCountries}
                    searchAction={this.props.searchAction}
                    getCategoryEventsByPaginationidex={
                        this.props.getCategoryEventsByPaginationidex
                    }
                    changeNearMeData={this.props.changeNearMeData}
                    getNearByPlacesByPagination={this.props.getNearbyPlacesByPagination}
                    getNearByLocations={this.props.getNearByLocations}
                    nearMe={this.props.nearMe}
                />

            } else {
                return <NavMenuV1 isOverlay={isHome}
                    categories={this.props.categories}
                    getNearByPlacesByPagination={this.props.getNearbyPlacesByPagination}
                    requestSiteContent={this.props.requestSiteContent}
                    clearSearchAction={this.props.clearSearchAction}
                    searchSuccess={this.props.category.searchSuccess}
                    clearSearchSuccess={this.props.category.clearSearchSuccess}
                    searchResult={this.props.category.searchResult}
                    content={this.props.category.content}
                    defaultSearchCities={this.props.category.defaultSearchCities}
                    defaultSearchStates={this.props.category.defaultSearchStates}
                    defaultSearchCountries={this.props.category.defaultSearchCountries}
                    searchAction={this.props.searchAction}
                    getCategoryEventsByPaginationidex={this.props.getCategoryEventsByPaginationidex}
                    changeNearMeData={this.props.changeNearMeData}
                    getNearByLocations={this.props.getNearByLocations}
                    nearMe={this.props.nearMe}
                    showSignInSignUp={this.showSignInSignUp}
                />
            }
        } else {
            return <div></div>
        }
    }

    logout = (e) => {
        this.setState({ showSignInModal: false, redirectUrl: "" })
        this.props.logout();
    }

    showSignInSignUp = (isLoginPage, url) => {
        this.setState({ isSiginSignUpShow: true, isLoginPage: isLoginPage, showSignInModal: true, redirectUrl: url })
    }
    closeSignInSignUp = (e) => {
        this.setState({ showSignInModal: false, redirectUrl: "" })
    }
    componentDidMount = () => {
        PubSub.subscribe('SHOW_LOGIN', this.subscriberData.bind(this));
    }
    public subscriberData(msg, data) {
        this.showSignInSignUp(true, null)
    }

    public render() {
        return (
            <div className="d-inline">
                <GoogleTagManager gtmId={this.props.gtmId} />
                {this.getMenu()}
                {this.props.children}

                {this.state.isSiginSignUpShow &&
                    <SignInSignUp
                        isSignUp={!this.state.isLoginPage}
                        history={null}
                        showSignInModal={this.state.showSignInModal}
                        closeSignInSignUp={(e) => this.closeSignInSignUp(e)}
                        isCheckout={false}
                        modalFlag={this.state.isLoginPage}
                    />
                }
                <FooterBarV1 session={this.props.session} showSignInSignUp={this.showSignInSignUp} />
            </div>
        );
    }
}

export default connect(
    (state: IApplicationState, ownProps) => ({
        session: state.session,
        category: state.category,
        nearMe: state.nearMe,
        ...ownProps
    }),
    dispatch =>
        bindActionCreators(
            { ...sessionActionCreators, ...categoryActionCreators, ...nearMeActionCreators },
            dispatch
        )
)(Layout);