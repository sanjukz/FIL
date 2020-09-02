import * as React from 'react';
import { ReactHTMLConverter } from "react-html-converter/browser";
import { gets3BaseUrl } from "../../utils/imageCdn";

export default class EventDetail extends React.Component<any, any>{
    state = {
        s3BaseUrl: gets3BaseUrl(),
    }

    render() {
        const converter = ReactHTMLConverter();
        converter.registerComponent('EventDetail', EventDetail);
        return (<>
            <section className="container my-4">
                <h4 className="alert-heading">About {this.props.props.LiveOnline.userDetails.event.name}</h4>
                <p>
                    {converter.convert(this.props.props.LiveOnline.userDetails.event.description)}
                </p>
            </section>
            <section className="filTabs container py-4 mb-4 text-center">
                <ul
                    className="nav nav-pills justify-content-center mb-3"
                    id="pills-tab"
                    role="tablist"
                >
                    <li className="nav-item">
                        <a
                            className="nav-link active"
                            id="pills-home-tab"
                            data-toggle="pill"
                            href="#pills-home"
                            role="tab"
                            aria-controls="pills-home"
                            aria-selected="true"
                        >
                            <img
                                src={`${this.state.s3BaseUrl}/eventHost/${this.props.props.LiveOnline.userDetails.eventHostMappings[0].altId.toUpperCase()}.jpg`}
                                onError={e => {
                                    e.currentTarget.src = `${this.state.s3BaseUrl}/icons/fapAvtar.png`
                                }}
                                alt="Host Image "
                            />
                        </a>
                        <small className="fapTabName mt-2 d-block">{`${this.props.props.LiveOnline.userDetails.eventHostMappings[0].firstName} ${this.props.props.LiveOnline.userDetails.eventHostMappings[0].lastName}`}</small>
                    </li>
                </ul>
                <div
                    className="tab-content text-left p-3 bg-light small rounded"
                    id="pills-tabContent"
                >
                    <div
                        className="fade show active"
                        id="pills-home"
                        role="tabpanel"
                        aria-labelledby="pills-home-tab"
                    >
                        {converter.convert(this.props.props.LiveOnline.userDetails.eventHostMappings[0].description)}
                    </div>
                </div>
            </section>
        </>);
    }
}