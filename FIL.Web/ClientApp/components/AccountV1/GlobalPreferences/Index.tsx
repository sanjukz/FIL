import * as React from 'react';
import {
    Link, RouteComponentProps, Route, Switch
} from "react-router-dom";
import * as AccountStore from "../../../stores/Account";
import FilLoader from "../../../components/Loader/FilLoader";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import * as PubSub from "pubsub-js";
import {
    setCookieAndReload,
    getCurrencyList,
} from "../../../utils/currencyFormatter";
import BreadcrumbAndTitle from '../BreadcrumbAndTitle';
import { gets3BaseUrl } from '../../../utils/imageCdn';

type AccountProps = AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;

class GlobalPreferences extends React.PureComponent<AccountProps, any>{
    constructor(props) {
        super(props);
        this.state = {
            currencyDropdown: "none",
            selectCurrency: "Change Currency",
            gets3BaseUrl: gets3BaseUrl()
        };
    }
    componentDidMount() {
        var origin = (window as any).location.pathname;
        var currencydropdownhtml = "none";
        if (
            origin == "/" ||
            origin.indexOf("/place/") !== -1 ||
            origin.indexOf("/ticket-purchase-selection/") !== -1
        ) {
            currencydropdownhtml = "block";
        }
        this.setState({ currencyDropdown: currencydropdownhtml });

        //read cookie value
        var nameEQ = "user_currency=";
        var readCookie = "";
        var ca = document.cookie.split(";");
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == " ") c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) {
                readCookie = c.substring(nameEQ.length, c.length);
            }
        }

        if (readCookie != "") {
            this.setState({ selectCurrency: readCookie });
        }

        PubSub.subscribe("UPDATE_CURRENCY_TAB", this.toggleCurrencyTab.bind(this));
    }
    public toggleCurrencyTab() {
        var origin = (window as any).location.pathname;
        var currencydropdownhtml = "none";
        if (
            origin.indexOf("/checkout") !== -1 ||
            origin.indexOf("/payment") !== -1
        ) {
            currencydropdownhtml = "none";
        } else {
            currencydropdownhtml = "block";
        }
        this.setState({ currencyDropdown: currencydropdownhtml });
    }
    getFormatName = () => {
        var shortCurrency = getCurrencyList().shortCurrency;
        var fullCurrency = getCurrencyList().fullCurrency;

        var currentCurrencyIndex = shortCurrency.indexOf(this.state.selectCurrency);

        var currentCurrency = this.state.selectCurrency;
        shortCurrency.sort(function (x, y) {
            return x == currentCurrency ? -1 : y == currentCurrency ? 1 : 0;
        });

        fullCurrency.splice(0, 0, fullCurrency.splice(currentCurrencyIndex, 1)[0]);

        let filtredData = fullCurrency[shortCurrency.indexOf(this.state.selectCurrency)];
        let result = "";
        if (filtredData && filtredData.length > 0) {
            result = filtredData.split('-')[2];
        }
        return result;
    }
    render() {
        var shortCurrency = getCurrencyList().shortCurrency;
        var fullCurrency = getCurrencyList().fullCurrency;

        var currentCurrencyIndex = shortCurrency.indexOf(this.state.selectCurrency);

        var currentCurrency = this.state.selectCurrency;
        shortCurrency.sort(function (x, y) {
            return x == currentCurrency ? -1 : y == currentCurrency ? 1 : 0;
        });

        fullCurrency.splice(0, 0, fullCurrency.splice(currentCurrencyIndex, 1)[0]);

        var showCurrency = shortCurrency.map((item, i) => {
            return (
                <a
                    key={i}
                    className={
                        item == this.state.selectCurrency
                            ? "list-group-item list-group-item-action active"
                            : "list-group-item list-group-item-action"
                    }
                    href="JavaScript:Void(0);"
                    onClick={(e) => setCookieAndReload(item)}
                    dangerouslySetInnerHTML={{ __html: fullCurrency[i] }}
                />
            );
        });
        return <>
            <div className="container">

                <BreadcrumbAndTitle title={'Global preferences'} />

                <div className="row">
                    <div className="col-sm-8">
                        {/* <section>
                            <div>
                                <span className="font-weight-bold">Preferred Language</span>
                                <a
                                    className="btn btn-outline-primary btn-sm float-right"
                                    data-toggle="collapse"
                                    href="#lang"
                                    role="button"
                                    aria-expanded="false"
                                    aria-controls="lang"
                                >
                                    Edit</a>
                                <div className="collapse pt-3" id="lang">
                                </div>
                            </div>
                        </section>
                        <hr /> */}
                        <section>
                            <div>
                                <p className="font-weight-bold">Preferred Currency</p>
                                <span className="font-weight-bold">{this.state.selectCurrency} -{this.getFormatName()}</span>
                                <a
                                    className="btn btn-outline-primary btn-sm float-right"
                                    data-toggle="collapse"
                                    href="#currency"
                                    role="button"
                                    aria-expanded="false"
                                    aria-controls="currency"
                                >
                                    Edit</a>
                                <div className="collapse pt-3" id="currency">
                                    <div className="list-group acc-curency-brop">{showCurrency}</div>
                                </div>
                            </div>
                        </section>
                    </div>
                    <div className="col-sm-4 mt-4 mt-sm-0 pl-md-5">
                        <div className="border p-4">
                            <img
                                src={`${this.state.gets3BaseUrl}/my-account/right-bar-images/global-preferences.svg`}
                                className="img-fluid mb-4"
                                alt=""
                            />
                            <div>
                                <h5 className="mb-3">Your global preferences</h5>
                                <p className="m-0">
                                    Changing your currency updates
                                    how you see prices.
                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    }
}
export default GlobalPreferences;