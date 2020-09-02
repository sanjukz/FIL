import * as React from "react";
import { gets3BaseUrl } from "../utils/imageCdn";

const FeelForBusiness: React.FunctionComponent<any> = () => {
    React.useEffect(() => {
        if (window) {
            window.scrollTo(0, 0);
        }
    })
    return (
        <>
            <div className="inner-banner photogallery">
                <img
                    src={`${gets3BaseUrl()}/about-feel.jpg`}
                    alt="feel about us"
                    className="card-img"
                />
                <div className="card-img-overlay">
                    <nav aria-label="breadcrumb">
                        <ol className="breadcrumb bg-white m-0 p-1 pl-2 d-inline-block">
                            <li className="breadcrumb-item">
                                <a href="#">Home</a>
                            </li>
                            <li className="breadcrumb-item active" aria-current="page">
                                feel for Business
                  </li>
                        </ol>
                    </nav>
                </div>
            </div>
            <div className="pt-2 container page-content">
                <h2>feel for Business</h2>
                <p>
                    Our belief in personalization doesn't just apply to individuals,
                    but to groups as well. We believe that every team has different
                    goals and a one-size-fit-all approach does not work. Contact our
                    dedicated team now to find out how feelitLIVE can create the
                    perfect plan for your upcoming leadership retreat, company outing,
                    or industry convention, completely catered to your needs.
            </p>
                <p>
                    To submit a request or receive a custom quote, please email{" "}
                    <a href="mailto:business@feelitlive.com">
                        business@feelitlive.com
              </a>{" "}
                    with the following:
            </p>
                <p>"Feel for Business" in the subject line</p>
                <p>Your group's name</p>
                <p>Purpose of your trip</p>
                <p>Number of people</p>
                <p>Location of your trip, if decided</p>
                <p>Dates of arrival, departure, events, etc.</p>
                <p>Any other relevant information we may need to best assist you</p>
            </div>
        </>
    );
}
export default FeelForBusiness;