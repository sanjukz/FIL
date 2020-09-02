import * as React from 'react';
import { ReactHTMLConverter } from "react-html-converter/browser";

const EventHostDetail = (props) => {
    const converter = ReactHTMLConverter();
    converter.registerComponent('EventInfoAndMap', EventHostDetail);

    let eventHostMappingHeader = props.eventHostMappings.map((item, indx) => {
        let clsName = indx == 0 ? "nav-link active" : "nav-link";
        return <li className="nav-item px-4">
            <a className={clsName} id={`pills-${item.id}`} data-toggle="pill" href={`#pills-home${item.id}`} role="tab" aria-controls={`pills-home${item.id}`} aria-selected="true">
                <img src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/eventHost/${item.altId.toUpperCase()}.jpg`} alt="fap avrat"
                    onError={(e) => {
                        e.currentTarget.src = "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/icons/fapAvtar.png"
                    }} />
            </a>
            <span className="fapTabName mt-2 d-inline-block">{capitalizeFirstLetter(item.firstName)} {capitalizeFirstLetter(item.lastName)}</span>
        </li>
    })

    let eventHostmappingSection = props.eventHostMappings.map((item, index) => {
        let activeClassName = index == 0 ? "tab-pane rounded fade show active text-left" : "tab-pane rounded fade text-left";
        return <div className={activeClassName} id={`pills-home${item.id}`} role="tabpanel"
            aria-labelledby="pills-home-tab"> {converter.convert(item.description)}
        </div>
    });

    return <div className="fapTabs">
        <ul className="nav nav-pills mb-3 justify-content-center text-center" id="pills-tab" role="tablist">
            {eventHostMappingHeader}
        </ul>
        <div className="tab-content text-center" id="pills-tabContent">
            {eventHostmappingSection}
        </div>
    </div>
}
function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}
export default EventHostDetail;