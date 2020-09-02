import * as React from "react";
import Slider from "react-slick";
import "../../css/carousel.css";
import { Link } from "react-router-dom";
import { gets3BaseUrl } from "../../utils/imageCdn";

export default class CenterMode extends React.Component<any, any> {
    state = {
        s3BaseUrl: gets3BaseUrl(),
        baseImgixUrl: 'https://feelitlive.imgix.net/images'
    }
    render() {
        const { baseImgixUrl } = this.state;
        const settings = {
            className: "centerSlide",
            centerMode: true,
            infinite: true,
            slidesToShow: 3,
            speed: 500,
            centerPadding: "80px",

        };
        let countries = [];
        var c = this.props;
        this.props.countryPlace && this.props.countryPlace.length > 0
            ? this.props.countryPlace.map(item =>
                item.name.toLowerCase() === "u.k."
                    ? (countries[2] = item)
                    : item.name.toLowerCase() === "india"
                        ? (countries[1] = item)
                        : item.name.toLowerCase() === "spain"
                            ? (countries[3] = item)
                            : item.name.toLowerCase() === "u.s.a."
                                ? (countries[0] = item)
                                : item.name.toLowerCase() === "italy"
                                    ? (countries[4] = item)
                                    : item.name.toLowerCase() === "australia"
                                        ? (countries[5] = item)
                                        : item.name.toLowerCase() === "france"
                                            ? (countries[6] = item)
                                            : item.name.toLowerCase() === "new zealand"
                                                ? (countries[7] = item)
                                                : null
            )
            : [];
        return (
            <div className="slick-cust-style">
                <Slider {...settings}>
                    {countries.length > 0
                        ? countries.map((item, index) => {
                            return (
                                <div key={index}>
                                    <Link
                                        to={`/country/${
                                            item.name.indexOf(" ") >= 0
                                                ? item.name
                                                    .split(" ")
                                                    .join("-")
                                                    .toLowerCase()
                                                : item.name
                                                    .split(".")
                                                    .join("")
                                                    .toLowerCase()
                                            }?country=${item.id}`}
                                    >
                                        <div className="card text-white border-0">
                                            <img
                                                src={`${this.state.baseImgixUrl}/places/tiles/${item.altId}-country.jpg?auto=format&fit=crop&h=376&w=600&crop=entropy&q=55`}
                                                className="card-img"
                                                alt="London 150+ feels"
                                                onError={e => {
                                                    e.currentTarget.src =
                                                        `${this.state.s3BaseUrl}/places/tiles/tiles-placeholder.jpg`;
                                                }}
                                            />
                                            <div className="card-img-overlay">
                                                <h5 className="card-title m-0">
                                                    {`${item.name}`}
                                                    <small className="d-block">{`${item.count}+ feels`}</small>
                                                </h5>
                                            </div>
                                        </div>
                                    </Link>
                                </div>
                            );
                        })
                        : null}
                </Slider>
            </div>
        );
    }
}