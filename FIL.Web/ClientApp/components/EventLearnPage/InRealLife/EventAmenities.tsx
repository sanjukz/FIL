import * as React from "React";
import { gets3BaseUrl } from "../../../utils/imageCdn";

export default class EventAmenities extends React.Component<any, any> {
    public render() {
        var Amenities = this.props.amenities.map((item) => {
            return <li className="list-inline-item"><img src={`${gets3BaseUrl()}/amenities/` + item + `.png`} alt="" />{item}</li>
        })
        return <div className="amenities-sec pt-3">
            <h5 className="mb-4">Amenities / Inclusions</h5>
            <ul className="list-inline">
                {Amenities}
            </ul>
        </div>
    }
}