import "bootstrap";
import { polyfill } from "es6-promise"; 
import { createBrowserHistory } from "history";
import * as React from "react";
import * as ReactDOM from "react-dom";
import * as Sentry from '@sentry/browser';
import { AppContainer } from "react-hot-loader";
import { Provider } from "react-redux";
import { Route } from "react-router-dom";
import { ConnectedRouter } from "react-router-redux";
import App from "./App";
import configureStore from "./configureStore";
import { IApplicationState } from "./stores";

polyfill();

// Create browser history to use in the Redux store
const history = createBrowserHistory();

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = (window as any).initialReduxState as IApplicationState;
const sentryDsn = (window as any).sentryDsn;
const gtmId = (window as any).googleTagManagerAccountId as string;
Sentry.init({ 
    dsn: sentryDsn,
    release: `${process.env.SENTRY_PROJECT}@${process.env.REVISION}`
});

const store = configureStore(history, initialState);

function renderApp() {
    // This code starts up the React app when it runs in a browser. It sets up the routing configuration
    // and injects the app into a DOM element.
    ReactDOM.render(
        <AppContainer>
            <Provider store={ store }>
                <ConnectedRouter history={ history }>
                    <Route render={(props) => {
                        return <App {...props} gtmId={gtmId} />;
                    }} />
                </ConnectedRouter>
            </Provider>
        </AppContainer>,
        document.getElementById("react-app")
    );
}

renderApp();

// Allow Hot Module Replacement
if (module.hot) {
    module.hot.accept("./App", () => {
        renderApp();
    });
}
