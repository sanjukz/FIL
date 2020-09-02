import { autobind } from "core-decorators";
import * as React from "react";

declare var google;
export default class PlaceAutocomplete extends React.Component<any, any> {
    autocomplete: any;
    componentForm: any;
    place: any;

    constructor(props) {
        super(props);
        this.state = {
            isEdit: false
        }
        this.fillInAddress = this.fillInAddress.bind(this);
    }

    componentDidMount() {
        this.initAutocomplete();
    }

    initAutocomplete() {
        if (google != undefined) {
            this.autocomplete = new google.maps.places.Autocomplete((this.refs.autoCompletePlaces) as HTMLInputElement, { types: ['geocode'] });
            this.autocomplete.addListener('place_changed', this.fillInAddress);
        }
    }

    fillInAddress() {
        const componentForm = {
            street_number: 'short_name',
            route: 'long_name',
            locality: 'long_name',
            administrative_area_level_1: 'long_name',
            country: 'long_name',
            postal_code: 'short_name',
            postal_town: 'long_name',
            postal_code_prefix: 'short_name',
            sublocality_level_1: 'long_name',
            sublocality_level_2: 'long_name',
            sublocality_level_3: 'long_name',
            sublocality_level_4: 'long_name',
            sublocality_level_5: 'long_name',
        };
        // Get the place details from the autocomplete object.
        this.place = this.autocomplete.getPlace();
        /*this.setState({placeResult: this.place.address_components})*/

        for (let component in this.componentForm) {
            (this.refs.component as HTMLInputElement).value = '';
            (this.refs.component as HTMLInputElement).disabled = false;
        }
        var geolocation;
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
        var street = (this.refs.street_number as HTMLInputElement).value;
        var route = (this.refs.route as HTMLInputElement).value;

        if (city == "") {
            city = (this.refs.postal_town as HTMLInputElement).value;
            if (city == "") {
                city = (this.refs.sublocality_level_1 as HTMLInputElement).value;
            }
            if (city == "") {
                city = (this.refs.sublocality_level_2 as HTMLInputElement).value;
            }
            if (city == "") {
                city = (this.refs.sublocality_level_3 as HTMLInputElement).value;
            }
            if (city == "") {
                city = (this.refs.sublocality_level_4 as HTMLInputElement).value;
            }
            if (city == "") {
                city = (this.refs.sublocality_level_5 as HTMLInputElement).value;
            }

        }
        if (zipcode == "") {
            zipcode = (this.refs.postal_code_prefix as HTMLInputElement).value;
        }

        (this.refs.locality as HTMLInputElement).value = "";
        (this.refs.administrative_area_level_1 as HTMLInputElement).value = "";
        (this.refs.country as HTMLInputElement).value = "";
        (this.refs.postal_code as HTMLInputElement).value = "";
        (this.refs.street_number as HTMLInputElement).value = "";
        (this.refs.route as HTMLInputElement).value = "";
        (this.refs.postal_town as HTMLInputElement).value = "";
        (this.refs.postal_code_prefix as HTMLInputElement).value = "";
        (this.refs.sublocality_level_1 as HTMLInputElement).value = "";
        (this.refs.sublocality_level_2 as HTMLInputElement).value = "";
        (this.refs.sublocality_level_3 as HTMLInputElement).value = "";
        (this.refs.sublocality_level_4 as HTMLInputElement).value = "";
        (this.refs.sublocality_level_5 as HTMLInputElement).value = "";

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function(position) {
                geolocation = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
            });
        }
        var lat = 18.5204;
        var long = 73.8567;
        try {
            lat = this.place.geometry.viewport.na.g;
            long = this.place.geometry.viewport.ja.g;
        } catch (e) {

        }
        this.props.autoCompletePlace(address, city, state, country, zipcode, lat, long, street, route);
    }

    geolocate() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function(position) {
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
        var address = "Enter the place name";
        if (!this.props.isEditMode) {
            address = "Enter the place name";
        } else {
            address = this.props.value;
        }
        return <div className="form-group mb-0">
            <div className="mb-0">
                <div className="col p-0">
                    <input type="text" name="address" onFocus={this.geolocate} disabled={this.props.disabled}
                        ref="autoCompletePlaces" id="" className="form-control mb-2" placeholder={address} />
                    <div style={{ display: 'none' }}>
                        <div>
                            <div className="form-group">
                                <input type="text" name="street_number" className="form-control" id="" placeholder="street" ref="street_number" />
                            </div>
                            <div className="form-group">
                                <input type="text" name="route" className="form-control" id="" placeholder="route" ref="route" />
                            </div>
                        </div>
                        <input type="text" name="city" className="form-control mb-2" ref="locality" placeholder="City" />
                        <input type="text" name="city" className="form-control mb-2" ref="postal_town" placeholder="City" />
                        <input type="text" name="pinCode" className="form-control mb-2" ref="postal_code_prefix" placeholder="pinCode" />
                        <input type="text" name="city" className="form-control mb-2" ref="sublocality_level_1" placeholder="city" />
                        <input type="text" name="city" className="form-control mb-2" ref="sublocality_level_1" placeholder="city" />
                        <input type="text" name="city" className="form-control mb-2" ref="sublocality_level_2" placeholder="city" />
                        <input type="text" name="city" className="form-control mb-2" ref="sublocality_level_3" placeholder="city" />
                        <input type="text" name="city" className="form-control mb-2" ref="sublocality_level_4" placeholder="city" />
                        <input type="text" name="city" className="form-control mb-2" ref="sublocality_level_5" placeholder="city" />
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
        </div>
    }
}
