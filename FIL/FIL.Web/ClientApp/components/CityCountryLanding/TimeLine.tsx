import * as React from 'react';
import { gets3BaseUrl } from "../../utils/imageCdn";

export default class TimeLine extends React.Component<any, any> {
    render() {
        return (
            <section className="featured-feels feelsBlogHome sec-spacing">
                <div className="container p-0">
                    <h4 className="mb-4 text-left">{this.props.titleName}</h4>
                    <div id="feelsBlogHome" className="carousel slide" data-ride="carousel">
                        <div className="carousel-inner">
                            <div className="carousel-item active" data-interval="10000">
                                <div className="row">
                                    <div className="col-12">
                                        <a
                                            href="https://feelaplace.blog/2018/10/27/top-5-reasons-why-its-important-to-feel/"
                                            target="_blank"
                                            className="text-decoration-none d-block"
                                        >
                                            <div className="card mb-3">
                                                <img
                                                    src={`${gets3BaseUrl()}/places/about/in-depth/taj-mahal-timeline.jpg`}
                                                    className="card-img-top"
                                                    alt="..."
                                                />
                                            </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        );
    }
}
