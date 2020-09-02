import * as React from "react";
import { gets3BaseUrl } from "../../../utils/imageCdn";
import { EventLearnPageViewModel } from "../../../models/EventLearnPageViewModel";
import ImageSlider from "./ImageSlider";

interface Iprops {
  eventData: EventLearnPageViewModel;
}
export default class ImageAndVideo extends React.PureComponent<Iprops, any> {
  constructor(props) {
    super(props);
    this.state = {
      s3BaseUrl: gets3BaseUrl(),
      baseImgixUrl: "https://feelitlive.imgix.net/images",
    };
  }
  render() {
    let isMobile = false;
    if (window && window.screen.width < 768) {
      isMobile = true;
    }
    const { s3BaseUrl, baseImgixUrl } = this.state;
    let eventGalleryImages =
      this.props.eventData.eventGalleryImage.length > 0
        ? this.props.eventData.eventGalleryImage[0]
        : null;
    let eventAltId = this.props.eventData.event.altId.toLocaleUpperCase();
    if (!isMobile) {
      return (
        <div className="card-deck overflow-hidden fil-inner-banner-images">
          <div className="card">
            {eventGalleryImages.isVideoUploaded ? (
              <video
                controls
                className="w-100 h-100"
                muted
                autoPlay
                loop
                style={{ objectFit: "cover" }}
              >
                <source
                  src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/videos/banner/${eventAltId}.mp4`}
                  type="video/mp4"
                />
                Your browser does not support the video tag.
              </video>
            ) : (
                <img
                  src={`${baseImgixUrl}/places/tiles/${eventAltId}-ht-c1.jpg?auto=format&fit=crop&h=435&w=508&crop=entropy&q=55`}
                  alt=""
                />
              ) //Hot ticket Image
            }
          </div>

          {(eventGalleryImages.isPortraitImage ||
            eventGalleryImages.isVideoUploaded) && (
              <div className="card">
                {/* Banner Image */}{" "}
                {eventGalleryImages.isPortraitImage &&
                  !eventGalleryImages.isVideoUploaded && (
                    <img
                      src={`${baseImgixUrl}/places/about/${eventAltId}-about.jpg?auto=format&fit=crop&h=435&w=303&crop=entropy&q=55`}
                      alt=""
                    />
                  )}
                {/* Hot ticket Image */}{" "}
                {eventGalleryImages.isVideoUploaded &&
                  !eventGalleryImages.isPortraitImage && (
                    <img
                      src={`${baseImgixUrl}/places/tiles/${eventAltId}-ht-c1.jpg?auto=format&fit=crop&h=435&w=303&crop=entropy&q=55`}
                      alt=""
                    />
                  )}
                {/* if video and portrait both uploaded */}
                {eventGalleryImages.isVideoUploaded &&
                  eventGalleryImages.isPortraitImage && (
                    <>
                      {/* Hot ticket Image */}{" "}
                      <img
                        src={`${baseImgixUrl}/places/tiles/${eventAltId}-ht-c1.jpg?auto=format&fit=crop&h=210&w=303&crop=entropy&q=55`}
                        alt=""
                      />
                      {/* Banner Image */}{" "}
                      <img
                        className="mt-3"
                        src={`${baseImgixUrl}/places/about/${eventAltId}-about.jpg?auto=format&fit=crop&h=210&w=303&crop=entropy&q=55`}
                        alt=""
                      />
                    </>
                  )}
              </div>
            )}
          <div className="card">
            {eventGalleryImages.isPortraitImage ? (
              //portrait image
              <img
                src={`${baseImgixUrl}/places/portrait/${eventAltId}.jpg?auto=format&fit=crop&h=435&w=303&crop=entropy&q=55`}
                alt=""
              />
            ) : (
                //banner image
                (eventGalleryImages.isVideoUploaded) ?
                  <img
                    src={`${baseImgixUrl}/places/about/${eventAltId}-about.jpg?auto=format&fit=crop&h=435&w=303&crop=entropy&q=55`}
                    alt=""
                  />
                  :
                  <img
                    src={`${baseImgixUrl}/places/about/${eventAltId}-about.jpg?auto=format&fit=crop&h=435&w=620&crop=entropy&q=55`}
                    alt=""
                  />
              )}
          </div>
        </div>
      );
    } else {
      return <ImageSlider eventData={this.props.eventData} />;
    }
  }
}
