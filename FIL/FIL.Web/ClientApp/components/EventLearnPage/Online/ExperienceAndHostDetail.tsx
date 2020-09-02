import * as React from "react";
import { ReactHTMLConverter } from "react-html-converter/browser";
import { EventLearnPageViewModel } from "../../../models/EventLearnPageViewModel";

interface Iprops {
  eventData: EventLearnPageViewModel;
}
function ExperienceAndHostDetail(props: Iprops) {
  const converter = ReactHTMLConverter();
  converter.registerComponent(
    "ExperienceAndHostDetail",
    ExperienceAndHostDetail
  );
  let experiementRequirementsData = [];
  let experienceRequirements = props.eventData.event.metaDetails
    ? props.eventData.event.metaDetails.split("|")
    : [];
  if (experienceRequirements && experienceRequirements.length > 0) {
    experiementRequirementsData = experienceRequirements.filter((val) => {
      return val != "";
    });
    experiementRequirementsData = experiementRequirementsData.map((item) => {
      return <li>{item}</li>;
    });
  }
  let hostDetails = [];
  hostDetails = props.eventData.eventHostMappings.map((item, index) => {
    return (
      <div>
        <img
          src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/eventHost/${item.altId.toUpperCase()}.jpg`}
          alt="fap avrat"
          className="rounded-circle mb-4"
          width="120px"
          height="120px"
          onError={(e) => {
            e.currentTarget.src =
              "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/icons/fapAvtar.png";
          }}
        />
        <h4>
          {capitalizeFirstLetter(item.firstName)}{" "}
          {capitalizeFirstLetter(item.lastName)}
        </h4>
        <p>{converter.convert(item.description)}</p>
        {index < props.eventData.eventHostMappings.length - 1 && <hr />}
      </div>
    );
  });

  return (
    <>
      <section className="exp-sec-pad">
        <div className="container">
          <div className="card-deck">
            <div className="card">
              <h3 className="text-purple">What you’ll experience</h3>
            </div>
            <div className="card">
              <p>{converter.convert(props.eventData.event.description)}</p>
            </div>
          </div>
        </div>
      </section>
      {hostDetails.length > 0 && (
        <section className="exp-sec-pad bg-light">
          <div className="container">
            <div className="card-deck">
              <div className="card">
                <h3 className="text-purple">{props.eventData.event.id == 15721 ? 'About your panelists' : 'About your host'}</h3>
              </div>
              <div className="card">{hostDetails}</div>
            </div>
          </div>
        </section>
      )}
      <section className="exp-sec-pad fil-exp-list-sec">
        <div className="container">
          <div className="card-deck">
            <div className="card">
              <h3 className="text-purple">
                What you’ll need for this experience
              </h3>
            </div>
            <div className="card fil-exp-list">
              {experiementRequirementsData.length > 0 ? (
                <ul>{experiementRequirementsData}</ul>
              ) : (
                  <>
                    You don't need to bring anything to enjoy this fantastic
                    experience
                </>
                )}
            </div>
          </div>
        </div>
      </section>
    </>
  );
}
function capitalizeFirstLetter(string) {
  return string.charAt(0).toUpperCase() + string.slice(1);
}
export default ExperienceAndHostDetail;
