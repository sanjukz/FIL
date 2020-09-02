import * as React from "react";
import Slider from "react-slick";
import { gets3BaseUrl } from "../../utils/imageCdn";
import ImageComponent from "../Comman/ImageComponent";

export default class TwoColSections extends React.Component<any, any> {
    public constructor(props) {
        super(props);
        this.state = {
            s3BaseUrl: gets3BaseUrl()
        };
    }

    render() {
        var that = this;
        const settings = {
            className: "",
            infinite: true,
            slidesToShow: this.props.noOfSlide,
            slidesToScroll: 1,
            speed: 500,
            responsive: [
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 2
                    }
                },
                {
                    breakpoint: 570,
                    settings: {
                        slidesToShow: 2
                    }
                },
                {
                    breakpoint: 430,
                    settings: {
                        slidesToShow: 1
                    }
                }
            ]
        };
        var place_Data = [];
        var data = this.props.placeCardsData.map((item) => { // Get distinct places...
            if (place_Data.filter((val) => { return item.name == val.name }).length == 0) {
                place_Data.push(item);
            }
        });
        var all_places = place_Data.map((item) => {
            return <a href={item.url} className="text-decoration-none d-block">
                <div className="card fil-tils">
                    <ImageComponent
                        parentCategorySlug={item.parentCategorySlug}
                        subCategorySlug={item.subCategorySlug}
                        s3BaseUrl={that.state.s3BaseUrl}
                        imgUrl={that.state.s3BaseUrl + "/places/tiles/" + item.altId.toUpperCase() + "-ht-c1.jpg"}
                        isCountry={true}
                    />
                    <div className="card-body p-0 position-relative">
                        <p className="card-title pt-0 m-0">{item.name}
                            <div className="">
                                {item.cityName}
                            </div>
                        </p>
                    </div>
                </div>
            </a>
        });

        return (
            <section className={`feelsBlogHome one-col placesTab sec-spacing fil-tiles-sec ${this.props.bgClass == "" ? 'bg-white' : ''}`} >
                <div className="container p-0">
                    <h4 className="mb-4">{this.props.heading}</h4>
                    <Slider {...settings}>
                        {all_places}
                    </Slider>
                </div>
            </section>
        );
    }
};