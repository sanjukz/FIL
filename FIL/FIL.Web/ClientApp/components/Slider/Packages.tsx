import * as React from "react";
import * as getSymbolFromCurrency from "currency-symbol-map";
import { gets3BaseUrl } from "../../utils/imageCdn";

let count = 1;
export default class Packages extends React.PureComponent<any, any> {
    public getMinPrice(item) {
        var prices = item.map(val => val.price);
        if (prices.length == 1) {
            return prices[0];
        } else {
            return Math.min(...prices);
        }
    }

    public getMaxPrice(item) {
        var prices = item.map(val => val.price);
        if (prices.length == 1) {
            return prices[0];
        } else {
            return Math.max(...prices);
        }
    }
    render() {
        let categoryPackages =
            this.props.category &&
                this.props.category.packages &&
                this.props.category.packages.categoryEvents
                ? this.props.category.packages.categoryEvents.filter(
                    item =>
                        item.event.name.toLowerCase().indexOf("legends in lauderhill") !=
                        -1 ||
                        item.event.name.toLowerCase().indexOf("giants in georgetown") !=
                        -1 ||
                        item.event.name
                            .toLowerCase()
                            .indexOf("powerplays in port-of-spain") != -1
                )
                : [];
        return (
            <div>
                {categoryPackages.length > 10 ? (
                    <section className="feelsBlogHome container-fluid pt-4 pb-4 bg-light mt-3">
                        <div className="container p-0">
                            <h4 className="m-0 text-center">Feel like a winner</h4>
                            <p className="text-center">
                                Book all-inclusive packages for the upcoming India tour of West
                Indies
              </p>
                            <div
                                id="feelsBlogHome"
                                className="carousel slide"
                                data-ride="carousel"
                            >
                                <div className="carousel-inner">
                                    <div className="carousel-item active" data-interval="10000">
                                        <div className="row">
                                            {categoryPackages.map((item, index) => (
                                                <div className="col-12 col-sm-6 col-lg-4" key={index}>
                                                    <a
                                                        href={`/place/${item.parentCategory}/${
                                                            item.event.slug
                                                            }/${item.eventCategory.toLocaleLowerCase()}`}
                                                        className="text-decoration-none d-block"
                                                    >
                                                        <div className="card mb-3">
                                                            <img
                                                                src={`${gets3BaseUrl()}/places/tiles/${item.event &&
                                                                    item.event.altId.toUpperCase()}-ht-c1.jpg`}
                                                                className="card-img-top"
                                                                alt="..."
                                                            />
                                                            <div className="card-body position-relative">
                                                                <h6 className="card-title m-0">
                                                                    {item.event && item.event.name
                                                                        ? item.event.name
                                                                        : ""}
                                                                    <div>
                                                                        <small>
                                                                            {`${getSymbolFromCurrency(
                                                                                item.currencyType &&
                                                                                item.currencyType.code
                                                                            )}${this.getMinPrice(
                                                                                item.eventTicketAttribute
                                                                            )}-${this.getMaxPrice(
                                                                                item.eventTicketAttribute
                                                                            )}`}
                                                                        </small>
                                                                    </div>
                                                                </h6>
                                                                <p className="card-text">
                                                                    <small
                                                                        dangerouslySetInnerHTML={{
                                                                            __html:
                                                                                item.event.description !== undefined
                                                                                    ? item.event.description.length > 200
                                                                                        ? `${item.event.description.slice(
                                                                                            0,
                                                                                            190
                                                                                        )}...`
                                                                                        : item.event.description
                                                                                    : null
                                                                        }}
                                                                    />
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </a>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                ) : null}
            </div>
        );
    }
}
