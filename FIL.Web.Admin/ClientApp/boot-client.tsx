//import "@babel/polyfill";
import 'bootstrap';

import * as React from "react";
import * as Sentry from '@sentry/browser';
import { AppContainer } from "react-hot-loader";
import { ConnectedRouter } from "react-router-redux";
import * as ReactDOM from "react-dom";
import { Provider } from "react-redux";
import { Route, BrowserRouter } from "react-router-dom";
import App from "./App";
import configureStore from "./configureStore";
import { IApplicationState } from "./stores";
import { createBrowserHistory } from "history";

//import 'bootstrap/dist/css/bootstrap.css';
//import 'font-awesome/css/font-awesome.css';

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

ReactDOM.render(
    <Provider store={store}>
        <BrowserRouter>
            <Route render={(props) => {
                      return <App {...props} gtmId={gtmId} />;
                  }}/>
        </BrowserRouter>
    </Provider>,
    document.getElementById("react-app")
);
