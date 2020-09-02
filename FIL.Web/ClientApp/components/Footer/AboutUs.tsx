import * as React from 'react';
import { gets3BaseUrl } from "../../utils/imageCdn";
import "../../scss/site.scss";
import "../../scss/_footer.scss";
import "../../scss/static-pages.scss";

const AboutUs: React.FunctionComponent<any> = () => {
    React.useEffect(() => {
        if (window) {
            window.scrollTo(0, 0)
        }
    });
    return (
        <>
            <div className="inner-banner photogallery">
                <img src={`${gets3BaseUrl()}/about-feel.jpg`} alt="feel about us" className="card-img" />
                <div className="card-img-overlay">
                    <nav aria-label="breadcrumb">
                        <ol className="breadcrumb bg-white m-0 p-1 pl-2 d-inline-block">
                            <li className="breadcrumb-item"><a href="#">Home</a></li>
                            <li className="breadcrumb-item active" aria-current="page">About Us</li>
                        </ol>
                    </nav>
                </div>
            </div>
            <div className="pt-2 pb-2 container page-content">
                <h1 className="h3">About Us</h1>
                <hr />
                <p>We exist to enable travelers and tourists to “feel” a place before, during and after their trips.</p>
                <p>Before we started, we wrote down all the things that are important to us as travelers - discovery, personalization, planning, sharing, easy access, bookability, safety, utilities, supporting local businesses. You name it, and we’ve written it into our code.</p>
                <p>Our team curates content and collections from our partners from all corners of the world providing access and insights that are handcrafted (or machine crafted), local and authentic. feelitLIVE hosts 10+ thousand unique places to travel to in nearly 250+ cities and 70+ countries around the world and we are growing by leaps and bounds everyday.</p>
                <p>Travel and tourism is far more than just flights and hotels. We want you to feel your trips – unlock deeper emotions, have lasting memories of each moment, be energized by every activity, be inspired by every experience.</p>
            </div>
        </>
    );
}

export default AboutUs;