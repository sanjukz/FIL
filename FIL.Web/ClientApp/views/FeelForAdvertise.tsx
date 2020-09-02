import * as React from "react";
import { gets3BaseUrl } from "../utils/imageCdn";

const FeelForAdvertise: React.FC = () => {
  React.useEffect(() => {
    if (window) {
      window.scrollTo(0, 0);
    }
  });
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
                feel for Advertise
              </li>
            </ol>
          </nav>
        </div>
      </div>
      <div className="pt-2 container page-content">
        <h2>feel for Advertise</h2>
        <p>
          Our platform reaches thousands of avid travellers globally. Take
          advantage of this audience by sponsoring a listing, hosting a banner
          ad, commissioning a blog post - the possibilities are endless. Our
          dedicated marketing team will take you through every step of the way,
          from brainstorming to results, and assist you with whatever you may
          need.
        </p>
        <p>
          To submit a request or receive a custom quote, please email{" "}
          <a href="mailto:advertise@feelitLIVE.com">advertise@feelitlive.com</a>{" "}
          with the following:
        </p>
        <p>"Advertise a Feel" in the subject line</p>
        <p>Your business's name</p>
        <p>Goals for your ad (edited)</p>
        <p>Target demographics (age, location, gender, etc.)</p>
        <p>Duration and dates of campaign</p>
        <p>Any other relevant information we may need to best assist you</p>
      </div>
    </>
  );
};
export default FeelForAdvertise;
