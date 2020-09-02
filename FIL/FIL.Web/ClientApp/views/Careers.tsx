import * as React from "react";
import { gets3BaseUrl } from "../utils/imageCdn";

export default class Careers extends React.Component<any, any> {
    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0);
        }
        document.body.classList.add("feel-career");
    }

    componentWillUnmount() {
        document.body.classList.remove("ticket-selection-page");
        document.body.classList.remove("feel-career");
    }

    public render() {
        return (
            <div>
                <div>
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
                                        Careers
                  </li>
                                </ol>
                            </nav>
                        </div>
                    </div>
                    <div className="pt-2 container page-content careers-page">
                        <h2>Careers at feelitLIVE</h2>
                        <h6>Our Values</h6>
                        <p>
                            Collaboration, communication, and creativity are the tenets of our
              office, and everything we create reflects these values. Our team
              hails from a variety of backgrounds, ranging from entrepreneurship
              and technology in Silicon Valley to finance and analytics on Wall
              Street, meaning that our employees have years of collective
              experience to draw from in-house through a simple Slack message.
              Our doors (and chat windows) are always open for each other’s
              thoughts and ideas. Constant communication ensures that we are all
              on the same page and enables us to refine our ideas. We encourage
              outside-the-box thinking and new ideas – after all, it’s how this
              whole thing started.
            </p>

                        <h6>Our Team Spirit</h6>
                        <p>
                            The feelitLIVE team is smooth and cohesive. We each have our own
              role to play as we work together towards one common goal: to make
              the travel experience all it can be. We want people to not just
              travel, but to truly feel something in every activity they partake
              in. Above all else, we want to help our users make their trips
              matter.
            </p>

                        <h6>Our Attitude</h6>
                        <p>
                            This is an office that runs on jet fuel and wanderlust. As a
              travel and tech startup, we are all working on things we feel
              passionately about. Our hours can be odd at times, but sometimes
              the only way to get where you want to go is a 30-hour flight,
              complete with crying babies and last-minute gate changes. We stay
              flexible and optimistic no matter what delay is thrown our way
              because at the end of the day, we know the destination is worth
              it.
            </p>
                        <p>
                            If you love problem-solving, don’t mind multiple stopovers, and
              are always game to discuss the latest tech release, then you might
              just be a perfect fit for the feelitLIVE team.
            </p>
                    </div>
                </div>
            </div>
        );
    }
}
