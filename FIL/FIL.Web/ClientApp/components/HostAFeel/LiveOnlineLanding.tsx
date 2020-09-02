import * as React from "react";
import "../../scss/_hostafeel.scss";
import LiveOnlineImageSlide from "./LiveOnlineImageSlide";
const Typed = require("typed.js");

let delayTimer, typed;
export default class LiveOnlineLanding extends React.Component<any, any> {
  state = {
    svgSource:
      "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/host-icon/Fitness-className.svg",
  };

  redirectToAdmin = (view = "") => {
    let feelAdminUrl = `https://host.feelitlive.com/verifyauth/`;
    if (window.origin.indexOf("dev") > -1) {
      feelAdminUrl = `https://devadmin.feelitlive.com/verifyauth/`;
    }

    if (this.props.session.isAuthenticated && view) {
      window.open(
        `${feelAdminUrl}${this.props.session.user.altId}?view=${view}`,
        "_blank" // <- This makes it open in a new window.
      );
    } else if (this.props.session.isAuthenticated) {
      window.open(
        feelAdminUrl + this.props.session.user.altId,
        "_blank" // <- This makes it open in a new window.
      );
    } else {
      let url = "";
      if (view) {
        url = `${feelAdminUrl}userAltId?view=${view}`;
      } else {
        url = `${feelAdminUrl}userAltId?view=${view}`;
      }
      this.props.showSignInSignUp(true, url);
    }
  };

  render() {
    return (
      <div className="fil-landing-page hostAfeel-page">
        <section className="head-sec container text-center text-sm-left">
          <div className="row">
            <div data-aos="fade-up" className="col-sm-5">
              <h1 className="text-body pt-md-5 mt-md-4">
                Empowering you to create and earn from digital experiences
              </h1>
              <div className="h5 font-weight-normal">
                Boost your revenue and engage with your audiences by
                effortlessly bringing your artistry and expertise online
              </div>
              <div className="btnGroup">
                <button
                  onClick={() => this.redirectToAdmin("3")}
                  className="btn btn-primary"
                >
                  Get Started for FREE
                  <img
                    className="ml-2 mt-0"
                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/arrow-right.svg"
                  />
                </button>
              </div>
            </div>
            <div data-aos="fade-down" className="col-sm-7 text-right">
              <img
                src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-landing-page/fil-banner-animation.gif"
                alt=""
                className="img-fluid"
                style={{ maxWidth: "555px", width: "100%" }}
              />
            </div>
          </div>
        </section>
        <section className="text-center">
          <div className="container">
            <h2 data-aos="zoom-in" className="mb-5">
              How it works
            </h2>
            <div data-aos="fade-up" data-aos-duration="1000" className="row">
              <div className="col-sm-4">
                <p>
                  <img
                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-landing-page/create-profile2.png"
                    alt=""
                    className="img-fluid"
                    width="240"
                  />
                </p>
                <h5 className="text-body mt-4">Create your experience</h5>
                <div>
                  Enter your event/experience information such as event name,
                  description, info about the host, and finish it all by
                  uploading some attention-grabbing images
                </div>
              </div>

              <div className="col-sm-4">
                <p>
                  <img
                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-landing-page/ticketing-schedule.png"
                    alt=""
                    className="img-fluid"
                    width="240"
                  />
                </p>
                <h5 className="text-body mt-4">Setup schedule and ticketing</h5>
                <div>
                  Select the time(s) and date(s) when you want your event to
                  live-stream. Then choose the type(s) of tickets you want for
                  the event and the audience access level and set prices for
                  each.
                </div>
              </div>

              <div className="col-sm-4">
                <p>
                  <img
                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-landing-page/host-2.png"
                    alt=""
                    className="img-fluid"
                    width="240"
                  />
                </p>
                <h5 className="text-body mt-4">Get ready to host</h5>
                <div>
                  After your event is confirmed to meet our quality standards,
                  the event will be on sale online, and you will receive the
                  private live stream link for your upcoming event(s)
                </div>
              </div>
            </div>
          </div>
        </section>

        <section className="bg-blueberry text-center one-platform-sec">
          <div className="container">
            <h2 data-aos="zoom-in" className="text-white">
              One platform,
              <span className="d-block">
                all the features you need and want
              </span>
            </h2>
            <div data-aos="fade-up" data-aos-duration="1000" className="row">
              <div className="col">
                <img
                  src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-landing-page/fil-online-exp-features.png"
                  alt=""
                  className="img-fluid"
                />
              </div>
            </div>
          </div>
        </section>

        <LiveOnlineImageSlide />

        <section className="fil-about-us">
          <div className="container">
            <h2 data-aos="zoom-in" className="mb-5 text-center">
              What they say about us
            </h2>
            <div data-aos="fade-up" data-aos-duration="1000" className="row">
              <div className="col-sm-5 my-auto">
                <img
                  src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-landing-page/fedback.png"
                  alt=""
                  width="378"
                  className="img-fluid"
                />
              </div>
              <div className="col-sm-7 testimonial">
                <div className="shadow border bg-white p-5 float-left mb-5 position-relative">
                  <div className="text-center">
                    <img
                      src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-landing-page/testimonial/dr-Jeffrey-benson.jpg"
                      alt=""
                      width="88"
                      className="rounded-circle mb-3 shadow"
                    />
                  </div>
                  <p>
                    FeelitLIVE provided us with a platform to host our digital
                    world premiere, and our audience and singers a chance to
                    experience live performances together once again. We loved
                    the high-quality streaming performance, and the interactive
                    “backstage pass” where we could all celebrate after the
                    concert.
                  </p>
                  <div className="text-right text-primary">
                    <small>
                      Dr. Jeffrey Benson, Director of Choral Activities, San
                      Jose State University;{" "}
                      <span className="d-sm-block">
                        President, California Choral Directors' Association, San
                        Jose, CA
                      </span>
                    </small>
                  </div>
                </div>
                <div className="shadow border bg-white p-5 float-right mb-5 position-relative">
                  <div className="text-center">
                    <img
                      src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-landing-page/testimonial/miyuki-mori.jpg"
                      alt=""
                      width="88"
                      className="rounded-circle mb-3 shadow"
                    />
                  </div>
                  <p>
                    I really appreciate how FeelitLIVE provides truly
                    customer-first support and service, and the live-stream
                    quality was just fantastic, beyond my expectations! A big
                    thanks to the team from the bottom of my heart.
                  </p>
                  <div className="text-right text-primary">
                    <small>
                      Miyuki Mori, Voice Teacher, San Jose, California
                    </small>
                  </div>
                </div>
                <div className="shadow border bg-white p-5 float-left mb-5 position-relative">
                  <div className="text-center">
                    <img
                      src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-landing-page/testimonial/scott-glysson.jpg"
                      alt=""
                      width="88"
                      className="rounded-circle mb-3 shadow"
                    />
                  </div>
                  <p>
                    We recently created and hosted our first ever virtual
                    concert on FeelitLIVE and were thrilled with how it went.
                    The live streaming experience and backstage interactions
                    were so well done they almost mimicked real life. We were
                    even still able to receive a nice revenue and allow others
                    to share in our hard work. The customer service we received
                    was fantastic. We will use the platform again for our future
                    events.
                  </p>
                  <div className="text-right text-primary">
                    <small>
                      Scott Glysson, Director of Choral Activities and Vocal
                      Studies, Cal Poly, San Luis Obispo, CA
                    </small>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

        <section className="fil-faq-sec bg-light">
          <div className="container">
            <h2 data-aos="zoom-in" className="text-center mb-5">
              Frequently Asked Questions{" "}
            </h2>
            <div className="card-deck">
              <div
                className="card my-auto border-0 bg-transparent faq-img-card "
                data-aos="fade-up"
                data-aos-duration="1000"
              >
                <img
                  src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/fil-landing-page/faq-image.svg"
                  alt=""
                />
              </div>
              <div
                className="card border-0 bg-transparent"
                data-aos="fade-up"
                data-aos-duration="1000"
              >
                <div className="accordion" id="faq-one">
                  <div className="card">
                    <div className="card-header" id="faq-head-one">
                      <button
                        className="btn btn-link btn-block text-left"
                        type="button"
                        data-toggle="collapse"
                        data-target="#faq-head-one-callaps"
                        aria-expanded="false"
                        aria-controls="faq-head-one-callaps"
                      >
                        I want to create an experience/event. What do I need to
                        get started?
                      </button>
                    </div>

                    <div
                      id="faq-head-one-callaps"
                      className="collapse hide"
                      aria-labelledby="faq-head-one"
                      data-parent="#faq-one"
                    >
                      <div className="card-body">
                        Create your account, and sign in. It is a simple three
                        screen process and our quick explanations will step you
                        through it.
                      </div>
                    </div>
                  </div>
                  <div className="card">
                    <div className="card-header" id="faq-head-two">
                      <button
                        className="btn btn-link btn-block text-left"
                        type="button"
                        data-toggle="collapse"
                        data-target="#faq-head-two-callaps"
                        aria-expanded="false"
                        aria-controls="faq-head-two-callaps"
                      >
                        You say I can host for free. But how much will it really
                        cost?
                      </button>
                    </div>

                    <div
                      id="faq-head-two-callaps"
                      className="collapse hide"
                      aria-labelledby="faq-head-two"
                      data-parent="#faq-one"
                    >
                      <div className="card-body">
                        We charge 20% of the value of the tickets sold. This
                        includes all costs – the virtual venue, the streaming
                        and hosting and the payment charges. You only pay once
                        tickets sell and you don’t pay anything else.
                      </div>
                    </div>
                  </div>
                  <div className="card">
                    <div className="card-header" id="faq-head-three">
                      <button
                        className="btn btn-link btn-block text-left"
                        type="button"
                        data-toggle="collapse"
                        data-target="#faq-head-three-callaps"
                        aria-expanded="false"
                        aria-controls="faq-head-three-callaps"
                      >
                        I don’t want to sell tickets - I want to host a free
                        event. How will that work?
                      </button>
                    </div>

                    <div
                      id="faq-head-three-callaps"
                      className="collapse hide"
                      aria-labelledby="faq-head-three"
                      data-parent="#faq-one"
                    >
                      <div className="card-body">
                        You can set up your event. We will get in touch with you
                        to finalize the details.
                      </div>
                    </div>
                  </div>
                  <div className="card">
                    <div className="card-header" id="faq-head-four">
                      <button
                        className="btn btn-link btn-block text-left"
                        type="button"
                        data-toggle="collapse"
                        data-target="#faq-head-four-callaps"
                        aria-expanded="false"
                        aria-controls="faq-head-four-callaps"
                      >
                        How will I stream my experience?
                      </button>
                    </div>

                    <div
                      id="faq-head-four-callaps"
                      className="collapse hide"
                      aria-labelledby="faq-head-four"
                      data-parent="#faq-one"
                    >
                      <div className="card-body">
                        You can stream directly from your computer/mobile device
                        using the link we will provide. For live musical
                        performances we recommend using an encoder software such
                        as OBS - we will provide you with your custom RTMP link
                        which you will paste into your encoder software.
                      </div>
                    </div>
                  </div>
                  <div className="card">
                    <div className="card-header" id="faq-head-five">
                      <button
                        className="btn btn-link btn-block text-left"
                        type="button"
                        data-toggle="collapse"
                        data-target="#faq-head-five-callaps"
                        aria-expanded="false"
                        aria-controls="faq-head-five-callaps"
                      >
                        When and how do I get paid?
                      </button>
                    </div>

                    <div
                      id="faq-head-five-callaps"
                      className="collapse hide"
                      aria-labelledby="faq-head-five"
                      data-parent="#faq-one"
                    >
                      <div className="card-body">
                        We transfer the net proceeds to your bank account within
                        5 business days of the experience/event getting over
                      </div>
                    </div>
                  </div>
                  <div className="card">
                    <div className="card-header" id="faq-head-six">
                      <button
                        className="btn btn-link btn-block text-left"
                        type="button"
                        data-toggle="collapse"
                        data-target="#faq-head-six-callaps"
                        aria-expanded="false"
                        aria-controls="faq-head-six-callaps"
                      >
                        How many people can attend my event?
                      </button>
                    </div>

                    <div
                      id="faq-head-six-callaps"
                      className="collapse hide"
                      aria-labelledby="faq-head-six"
                      data-parent="#faq-one"
                    >
                      <div className="card-body">
                        Thousands can attend. We can host up to a 40,000.
                      </div>
                    </div>
                  </div>
                  <div className="card">
                    <div className="card-header" id="faq-head-seven">
                      <button
                        className="btn btn-link btn-block text-left"
                        type="button"
                        data-toggle="collapse"
                        data-target="#faq-head-seven-callaps"
                        aria-expanded="false"
                        aria-controls="faq-head-seven-callaps"
                      >
                        Can I interact with my viewers?
                      </button>
                    </div>

                    <div
                      id="faq-head-seven-callaps"
                      className="collapse hide"
                      aria-labelledby="faq-head-seven"
                      data-parent="#faq-one"
                    >
                      <div className="card-body">
                        Absolutely, You decide the various interactive methods
                        when you add the ticket types to your experience/event.
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>
      </div>
    );
  }
}
