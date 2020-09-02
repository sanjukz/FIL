import { autobind } from "core-decorators";
import * as React from "react";

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
            administrative_area_level_1: 'long_name',
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
        this.props.onChangeZipCode(zipcode);
    }

    @autobind
    onChangeAddress() {
        var address = (this.refs.autoCompletePlaces as HTMLInputElement).value;
        this.props.onChangeAddress(address);
    }

    @autobind
    onChangeCity() {
        var city = (this.refs.locality as HTMLInputElement).value;
        this.props.onChangeCity(city);
    }

    @autobind
    onChangeState() {
        var state = (this.refs.administrative_area_level_1 as HTMLInputElement).value;
        this.props.onChangeState(state);
    }

    @autobind
    onChangeCountry() {
        var country = (this.refs.country as HTMLInputElement).value;
        this.props.onChangeCountry(country);
    }

    public render() {
        return <div className="col-sm-6 mt-1">
            <h6 className="text-muted mb-4">Billing Address</h6>
            <div className="form-group">
                <input type="text" name="address" onFocus={this.geolocate}
                    ref="autoCompletePlaces" className="form-control" id="" onChange={this.onChangeAddress} placeholder="Apt, Floor, Suite, Street Address, etc." />
            </div>
            <div className="form-group">
                <input type="text" name="city" className="form-control" id="" onChange={this.onChangeCity} placeholder="Suburb" ref="locality" />
            </div>
            <div style={{ display: 'none' }}>
                <div className="form-group">
                    <input type="text" name="route" className="form-control" id="" onChange={this.onChangeCity} placeholder="Suburb" ref="route" />
                </div>
                <div className="form-group">
                    <input type="text" name="street_number" className="form-control" id="" onChange={this.onChangeCity} placeholder="Suburb" ref="street_number" />
                </div>
            </div>
            <div className="form-group">
                <input type="text" name="state" className="form-control" id="" onChange={this.onChangeState} placeholder="State/Province/Region" ref="administrative_area_level_1" />
            </div>
            <div className="form-row">
                <div className="col-sm-6 form-group">
                    <div className="form-group">
                        <input type="text" name="country" className="form-control" id="" onChange={this.onChangeCountry} placeholder="Country" ref="country" />
                    </div>
                </div>
                <div className="col-sm-6 form-group">
                    <div className="form-group">
                        <input type="text" name="zipcode" className="form-control" id="" onChange={this.onZipCodeChange} placeholder="Pin/Zip Code" ref="postal_code" />
                    </div>
                </div>

            </div>
        </div>
    }
}