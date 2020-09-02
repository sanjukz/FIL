import * as React from "react";
import Slider from "react-slick";
import { EventLearnPageViewModel } from '../../../models/EventLearnPageViewModel';
import { gets3BaseUrl } from "../../../utils/imageCdn";


interface Iprops {
    eventData: EventLearnPageViewModel;
}
export default class ImageSlider extends React.Component<Iprops, any> {
    constructor(props) {
        super(props);
        this.state = {
            s3BaseUrl: gets3BaseUrl(),
            baseImgixUrl: 'https://feelitlive.imgix.net/images'
        }
    }
    render() {
        var settings = {
            dots: true,
            infinite: true,
            speed: 500,
            slidesToShow: 2,
            slidesToScroll: 1,
            initialSlide: 0,
            autoPlay: true,
            responsive: [
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        };
        const { s3BaseUrl, baseImgixUrl } = this.state;
        let eventGalleryImages = this.props.eventData.eventGalleryImage.length > 0 ? this.props.eventData.eventGalleryImage[0] : null
        let eventAltId = this.props.eventData.event.altId.toLocaleUpperCase();
        return (
            <div>
                <Slider {...settings}>
                    {eventGalleryImages.isVideoUploaded &&
                        <div>
                            <video className="w-100 h-100" muted autoPlay loop style={{ objectFit: "cover" }}>
                                <source src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/videos/banner/${eventAltId}.mp4`} type="video/mp4" />
                                Your browser does not support the video tag.
                    </video>
                        </div>
                    }

                    <div>
                        <img className="w-100 h-auto" src={`${baseImgixUrl}/places/tiles/${eventAltId}-ht-c1.jpg?auto=format&fit=crop&h=300&w=508&crop=entropy`} alt="" />
                    </div>
                    <div>
                        <img className="w-100 h-auto" src={`${baseImgixUrl}/places/about/${eventAltId}-about.jpg?auto=format&fit=crop&h=300&w=508&crop=entropy`} alt="" />
                    </div>
                    {eventGalleryImages.isPortraitImage &&
                        <div>
                            <img className="w-100 h-auto" src={`${baseImgixUrl}/places/portrait/${eventAltId}.jpg?auto=format&fit=crop&h=300&w=508&crop=entropy`} alt="" />
                        </div>}
                </Slider>
            </div>
        );
    }
}