import * as React from 'react'
import { gets3BaseUrl } from "../../utils/imageCdn";

const s3BaseUrl = gets3BaseUrl();
export const ItinerarySectionV1 = (props) => {
    return (
        <section className="fil-tiles-sec bg-light">
            <div className="container">
                <div className="card-deck">
                    <div className="card bg-light border-0 m-0 my-auto">
                        <div className="h1 mb-3">Just <span className="text-blueberry">"Feelit"</span></div>
                        <p>‘Feelit’ it’s FeelitLIVE’s itinerary planner which allows you to design a customized travel plan, in just 3 simple steps.</p>
                        <div className="row">
                            <div className="col-sm-4 text-center">
                                <img src={`${s3BaseUrl}/fil-images/icon/1-choos-no.svg`} alt="Just feel" />
                                <div className="small">Choose your destination & dates</div>
                            </div>
                            <div className="col-sm-4 text-center">
                                <img src={`${s3BaseUrl}/fil-images/icon/2-choos-no.svg`} alt="Just feel" />
                                <div className="small">Select the type of <span className="d-sm-block">‘feel’ you prefer</span> </div>
                            </div>
                            <div className="col-sm-4 text-center">
                                <img src={`${s3BaseUrl}/fil-images/icon/3-choos-no.svg`} alt="Just feel" />
                                <div className="small">Pick your <span className="d-sm-block">budget range</span></div>
                            </div>
                        </div>
                        <p className="mt-5">That’s it!  Your beskpoke itinerary, built to match your travel style is done, so you can plan and book in just a few minutes!</p>
                    </div>
                    <div className="card bg-light border-0 m-0 text-center my-auto">
                        <img src={`${s3BaseUrl}/fil-images/build-your-itinerary.svg`} alt="Build your itinerary" />
                        <div className="button-group mt-3"><a href="/feel-itineraryplanner" className="btn btn-primary fil-btn">
                            Build your itinerary
                            <img src={`${s3BaseUrl}/fil-images/icon/arrow-right.svg`} className="ml-2" alt="" /></a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    )
}

export default ItinerarySectionV1;