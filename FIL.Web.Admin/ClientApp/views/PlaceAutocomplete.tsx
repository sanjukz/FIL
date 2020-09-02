import { autobind } from "core-decorators";
import * as React from "react";

declare var google;
export default class PlaceAutocomplete extends React.Component<any, any> {
    autocomplete: any;
    componentForm: any;
    place: any;


    constructor(props) {
        super(props);
        this.fillInAddress = this.fillInAddress.bind(this);
    }

    componentDidMount() {
        this.initAutocomplete();
    }

    initAutocomplete() {
        this.autocomplete = new google.maps.places.Autocomplete((this.refs.autoCompletePlaces) as HTMLInputElement, { types: ['geocode'] });
        this.autocomplete.addListener('place_changed', this.fillInAddress);
    }

    fillInAddress() {
        const componentForm = {
            street_number: 'short_name',
            route: 'long_name',
            locality: 'long_name',
            administrative_area_level_1: 'short_name',
            country: 'long_name',
            postal_code: 'short_name'
        };
        // Get the place details from the autocomplete object.
        this.place = this.autocomplete.getPlace();
        /*this.setState({placeResult: this.place.address_components})*/

        for (let component in this.componentForm) {
            (this.refs.component as HTMLInputElement).value = '';
            (this.refs.component as HTMLInputElement).disabled = false;
        }

        // Get each component of the address from the place details
        // and fill the corresponding input on the form.
        var that = this;
        that.place.address_components.forEach((component, index) => {
            const addressType = this.place.address_components[index].types[0];
            if (componentForm[addressType]) {
                const val = that.place.address_components[index][componentForm[addressType]];
                (that.refs[addressType] as HTMLInputElement).value = val;
            }
        });
        var address = (this.refs.autoCompletePlaces as HTMLInputElement).value;
        var city = (this.refs.locality as HTMLInputElement).value;
        var state = (this.refs.administrative_area_level_1 as HTMLInputElement).value;
        var country = (this.refs.country as HTMLInputElement).value;
        var zipcode = (this.refs.postal_code as HTMLInputElement).value;

        this.props.autoCompletePlace(address, city, state, country, zipcode);
    }

    geolocate() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                const geolocation = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
            });
        }
    }

    @autobind
    onZipCodeChange() {
        var zipcode = (this.refs.postal_code as HTMLInputElement).value;
        this.props.onCustomFieldChange(zipcode);
    }

    public render() {
        return <div className="form-group container-fluid">
            <div className="row">
                <div className="col pl-0">
                    <input type="text" name="address" onFocus={this.geolocate}
                        ref="autoCompletePlaces" id="" className="form-control mb-2" placeholder="Enter the place name" />
                    <div style={{ display: 'none' }}>
                        <div className="form-group">
                            <input type="text" name="route" className="form-control" id="" placeholder="Suburb" ref="route" />
                        </div>
                        <div className="form-group">
                            <input type="text" name="street_number" className="form-control" id="" placeholder="Suburb" ref="street_number" />
                        </div>
                    </div>
                    <input type="text" name="city" className="form-control mb-2" ref="locality" placeholder="City" />
                    <div className="row mb-0">
                        <div className="col">
                            <input type="text" name="state" ref="administrative_area_level_1" className="form-control mb-2" placeholder="State" />
                        </div>
                        <div className="col">
                            <input type="text" name="zipcode" className="form-control mb-2" onChange={this.onZipCodeChange} placeholder="Pin/Zip Code" ref="postal_code" />
                        </div>
                    </div>
                    <input type="text" name="country" className="form-control" placeholder="Country" ref="country" />
                </div>
            </div>
        </div>
    }
}
