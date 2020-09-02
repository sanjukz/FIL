import connectedAuthWrapper from "redux-auth-wrapper/connectedAuthWrapper";
import locationHelperBuilder from "redux-auth-wrapper/history4/locationHelper";
import { connectedRouterRedirect } from "redux-auth-wrapper/history4/redirect";
import Loading from "../components/Loading";

const locationHelper = locationHelperBuilder({});

const userIsAuthenticatedDefaults = {
    authenticatedSelector: (state) => {
        return state.session.isAuthenticated;
    },
    authenticatingSelector: (state) => {
        return state.session.isLoading;
    },
    wrapperDisplayName: "UserIsAuthenticated"
};

export const userIsAuthenticated = connectedAuthWrapper(userIsAuthenticatedDefaults);

export const userIsAuthenticatedRedirect = connectedRouterRedirect({
    ...userIsAuthenticatedDefaults,
    AuthenticatingComponent: Loading,
    redirectPath: "/Login"
});

export const userIsAdminRedirect = connectedRouterRedirect({
    redirectPath: "/",
    allowRedirectBack: false,
    authenticatedSelector: (state) => {
        return state.session.isAuthenticated && state.session.isAdmin;
    },
    predicate: (user) => user.isAdmin,
    wrapperDisplayName: "UserIsAdmin"
});

const userIsNotAuthenticatedDefaults = {
    authenticatedSelector: (state) => {
        return !state.session.isAuthenticated;
    },
    wrapperDisplayName: "UserIsNotAuthenticated"
};

export const userIsNotAuthenticated = connectedAuthWrapper(userIsNotAuthenticatedDefaults);

export const userIsNotAuthenticatedRedirect = connectedRouterRedirect({
    ...userIsNotAuthenticatedDefaults,
    redirectPath: (state, ownProps) => locationHelper.getRedirectQueryParam(ownProps) || "/Login",
    allowRedirectBack: false
});



