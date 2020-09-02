import * as React from "react";
import PlacesAutocomplete, {
    geocodeByAddress,
    getLatLng,
} from 'react-places-autocomplete';

interface IPlaceProps {
    name: string;
    class: string;
    placeholder: string;
    autoCompleteAddress: (address: string) => void;
}

export default class Place extends React.Component<IPlaceProps, any> {
    constructor(props) {
        super(props);
        this.state = { address: '' };
    }

    handleChange = address => {
        this.setState({ address });
    };

    handleSelect = address => {
        geocodeByAddress(address)
            .then(results => getLatLng(results[0]))
            .then(latLng => console.log('Success', latLng))
            .catch(error => console.error('Error', error));
    };

    public onAddressClick(data, e) {

        if (this.props.autoCompleteAddress != undefined) {
            this.props.autoCompleteAddress(data);
        }
        this.setState({ address: data });
    }

    render() {
        var that = this;
        var placeholder = "Enter Addresss";
        var className = "";
        if (this.props.placeholder != undefined) {
            placeholder = this.props.placeholder;
        }
        if (this.props.class != undefined) {
            className = this.props.class;
        }
        return (

            <PlacesAutocomplete
                value={this.state.address}
                onChange={this.handleChange}
                onSelect={this.handleSelect}
            >
                {({ getInputProps, suggestions, getSuggestionItemProps, loading }) => (
                    <div className="position-relative">
                        <input
                            name={that.props.name}
                            {...getInputProps({
                                placeholder: placeholder,
                                className: className
                            }) }
                        />
                        <div className="autocomplete-dropdown-container list-group">
                            {loading && <div className="list-group-item">Loading...</div>}
                            {suggestions.map(suggestion => {
                                const className = "list-group-item";
                                // inline style for demonstration purpose
                                const style = suggestion.active
                                    ? { backgroundColor: '#fafafa', cursor: 'pointer' }
                                    : { backgroundColor: '#ffffff', cursor: 'pointer' };
                                return (
                                    <div
                                        {...getSuggestionItemProps(suggestion, {
                                            className,
                                            style,
                                        }) }
                                    >
                                        <span onClick={that.onAddressClick.bind(that, suggestion.description)}><i className="fa fa-map-marker mr-10" aria-hidden="true"></i>{suggestion.description}</span>

                                    </div>
                                );
                            })}
                        </div>
                    </div>
                )}
            </PlacesAutocomplete>
        );
    }
} 