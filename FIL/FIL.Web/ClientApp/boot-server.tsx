import { createServerRenderer, RenderResult } from "aspnet-prerendering";
import { createMemoryHistory } from "history";
import * as Raven from "raven-js";
import * as React from "react";
import { renderToString } from "react-dom/server";
import { Provider } from "react-redux";
import { Route, StaticRouter } from "react-router-dom";
import { replace } from "react-router-redux";
import App from "./App";
import configureStore from "./configureStore";

export default createServerRenderer((params) => {
    Raven.config(params.data.sentryDsn).install();

    return new Promise<RenderResult>((resolve, reject) => {
        // Prepare Redux store with in-memory history, and dispatch a navigation event
        // corresponding to the incoming URL
        const store = configureStore(createMemoryHistory());
        store.dispatch(replace(params.location));

        // Prepare an instance of the application and perform an inital render that will
        // cause any async tasks (e.g., data access) to begin
        const routerContext: any = {};
        const app = (
            <Provider store={store}>
                <StaticRouter context={routerContext} location={params.location.path}>
                    <Route render={(props) => <App {...props} gtmId={(params.data.googleTagManagerAccountId as string)} />} />
                </StaticRouter>
            </Provider>
        );
        renderToString(app);

        // If there's a redirection, just send this information back to the host application
        if (routerContext.url) {
            resolve({ redirectUrl: routerContext.url });
            return;
        }

        // Once any async tasks are done, we can perform the final render
        // We also send the redux store state, so the client can continue execution where the server left off
        params.domainTasks.then(() => {
            resolve({
                html: renderToString(app),
                globals: {
                    initialReduxState: store.getState(),
                    sentryDsn: params.data.sentryDsn,
                    googleTagManagerAccountId: params.data.googleTagManagerAccountId,
                    stripePublicToken: params.data.stripePublicToken,
                    urls: params.data.urls,
                    stripeIndiaPublicToken: params.data.stripeIndiaPublicToken,
                    stripeAustraliaPublicToken: params.data.stripeAustraliaPublicToken,
                }
            });
        }, reject); // Also propagate any errors back into the host application
    });
});
