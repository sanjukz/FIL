import * as React from "react";
import { gets3BaseUrl } from "../../../utils/imageCdn";
import { EventLearnPageViewModel } from "../../../models/EventLearnPageViewModel";
import ImageSlider from "../Online/ImageSlider";
import { Button } from "react-bootstrap";
import ImageGallery from "react-image-gallery";

const ImageAndVideoV1 = (props: any) => {
  const baseImgixUrl = "https://feelitlive.imgix.net/images";
  const s3BaseUrl = gets3BaseUrl();
  const [isOpen, setIsOpen] = React.useState(false);
  let isMobile = false;
  if (window && window.screen.width < 768) {
    isMobile = true;
  }
  let eventGalleryImages =
    props.eventData.eventGalleryImage.length > 2
      ? props.eventData.eventGalleryImage.slice(0, 3)
      : props.eventData.eventGalleryImage.length === 2
      ? props.eventData.eventGalleryImage.slice(0, 2)
      : props.eventData.eventGalleryImage;
  let eventAltId = props.eventData.event.altId.toLocaleUpperCase();
  let imageList = props.eventData.eventGalleryImage;
  let images = [];
  let imageSlider = [];

  for (let i = 1; i <= imageList.length; i++) {
    images.push(
      `${s3BaseUrl}/places/about/photo-gallery/` +
        props.eventData.event.altId.toUpperCase() +
        `-glr-` +
        i +
        `.jpg`
    );
    imageSlider.push({
      original:
        `${s3BaseUrl}/places/about/photo-gallery/` +
        props.eventData.event.altId.toUpperCase() +
        `-glr-` +
        i +
        `.jpg`,
      thumbnail:
        `${s3BaseUrl}/places/about/photo-gallery/` +
        props.eventData.event.altId.toUpperCase() +
        `-glr-` +
        i +
        `.jpg`,
    });
  }
  if (!isMobile) {
    return (
      <div className="card-deck overflow-hidden fil-inner-banner-images position-relative">
        {isOpen && (
          <div className="photogallery">
            <div id="myModal" className="modal d-block">
              <span onClick={() => setIsOpen(false)} className="close cursor">
                &times;
              </span>
              <ImageGallery items={imageSlider} />
            </div>
          </div>
        )}
        {eventGalleryImages != null && eventGalleryImages.length <= 1 && (
          <div className="card" style={{ minWidth: "100%", maxWidth: "100%" }}>
            {/* banner image */}
            <img
              src={`${baseImgixUrl}/places/about/${eventAltId.toUpperCase()}-about.jpg?auto=format&fit=crop&h=570&w=1920&crop=entropy&q=55`}
              alt={
                props.eventData.subCategory.slug &&
                props.eventData.subCategory.slug
              }
              className="rounded-box"
              onError={(e) => {
                e.currentTarget.src = `${baseImgixUrl}/places/tiles/${props
                  .eventData.subCategory.slug &&
                  props.eventData.subCategory
                    .slug}-placeholder.jpg?auto=format&fit=crop&h=570&w=1920&crop=entropy&q=55`;
              }}
            />
          </div>
        )}

        {eventGalleryImages != null && eventGalleryImages.length > 2
          ? eventGalleryImages.map((item, index) => {
              return (
                <div className="card">
                  {/* banner image */}
                  <img
                    src={`${baseImgixUrl}/places/about/photo-gallery/${eventAltId.toUpperCase()}-glr-${index +
                      1}.jpg?auto=format&fit=crop&h=435&w=${
                      index == 0 ? "508" : "303"
                    }&crop=entropy&q=55`}
                    alt=""
                  />
                  {index == 2 ? (
                    <button
                      type="button"
                      onClick={() => setIsOpen(true)}
                      className="btn btn-light btn btn-default gallery-btn"
                    >
                      {" "}
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/view-photo.svg"
                        className="mr-2 mb-1 w-auto"
                      />
                      View photos
                    </button>
                  ) : null}
                </div>
              );
            })
          : null}

        {eventGalleryImages != null &&
          eventGalleryImages.length === 2 &&
          eventGalleryImages.map((item, index) => {
            return (
              <div className="card">
                {/* banner image */}
                <img
                  src={`${baseImgixUrl}/places/about/photo-gallery/${eventAltId.toUpperCase()}-glr-${index +
                    1}.jpg?auto=format&fit=crop&h=435&w=${
                    index == 0 ? "508" : "620"
                  }&crop=entropy&q=55`}
                  alt=""
                />
              </div>
            );
          })}
      </div>
    );
  } else {
    return <ImageSlider eventData={props.eventData} />;
  }
};

export default ImageAndVideoV1;
