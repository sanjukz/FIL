import * as React from 'React';
import Select from "react-select";
import { connect } from "react-redux";
import * as CityCountryStore from "../../stores/CityCountry";
import FeelLoader from "../Loader/FeelLoader";
import { IApplicationState } from '../../stores';
import * as CategoryStore from "../../stores/Category";
import { bindActionCreators } from 'redux';
var countyOptions = [], cityOptions = [], selectedCityData = null, isloading = false;

class StayMap extends React.Component<any, any>{
    state = {
        selectedCountry: '',
        selectedCity: '',
        selectedCityData: null
    }
    componentDidMount() {
        this.props.getCountryCities();
    }
    onChangeCountry = (e) => {
        this.setState({ selectedCountry: e, selectedCity: null })
        cityOptions = [], selectedCityData = null;
        let filteredcityOptions = this.props.category.countryCities.itinerarySerchData.filter((item) => {
            return item.countryName == e.value
        })
        filteredcityOptions.map((item) => {
            let tempData = {
                value: item.cityName,
                label: item.cityName
            }
            cityOptions.push(tempData);
        })
        if (cityOptions.length > 0) {
            cityOptions.sort((a, b) => a.value > b.value ? 1 : -1);
        }
    }
    onChangeCity = (e) => {
        this.setState({ selectedCity: e, })
        selectedCityData = null;
        if (e != null) {
            isloading = true;
            fetch(`https://maps.googleapis.com/maps/api/geocode/json?address=${e.value}&key=AIzaSyDZ4Ik7ENzLWY1tLh1ul8NxhWBdWGK6tQU`)
                .then((res) => res.json())
                .then((data) => {
                    selectedCityData = data.results[0].geometry;
                    isloading = false;
                    this.setState({
                        selectedCityData: data.results[0].geometry
                    });
                });
        }
    }

    render() {
        const { fetchCountryCities, countryCities } = this.props.category;
        if (!fetchCountryCities || isloading) {
            return <FeelLoader />
        } else {
            if (countryCities && countryCities.itinerarySerchData) {
                countyOptions = [];
                countryCities.itinerarySerchData.map((item) => {
                    let isCountryExist = countyOptions.filter((val) => {
                        return item.countryName == val.value
                    })
                    if (isCountryExist && isCountryExist.length == 0) {
                        let tempData = {
                            value: item.countryName,
                            label: item.countryName
                        }
                        countyOptions.push(tempData);
                    }
                })
            }
            return (
                <div >
                    <div className="container pt-4">
                        <div className="row pb-4">
                            <div className="col-md-6">
                                <Select
                                    name="country"
                                    required
                                    placeholder="Enter Country"
                                    onChange={this.onChangeCountry}
                                    isClearable={true}
                                    options={countyOptions}
                                    value={this.state.selectedCountry}
                                />
                            </div>
                            <div className="col-md-6">
                                <Select
                                    name="city"
                                    required
                                    placeholder="Enter City"
                                    onChange={this.onChangeCity}
                                    isClearable={true}
                                    options={cityOptions}
                                    value={this.state.selectedCity}
                                />
                            </div>
                        </div>
                    </div>
                    {selectedCityData != null &&
                        <iframe id="stay22-widget" width="100%" height="460" frameBorder="0" src={`https://www.stay22.com/embed/gm?aid=feelaplace&lat=${selectedCityData.location.lat}&lng=${selectedCityData.location.lng}&venue=Optional%20Text&hidebrandlogo=true`}></iframe>
                    }
                </div>
            )
        }
    }
}

const mapState = (state: IApplicationState) => {
    return {
        countryPlacesCount: state.cityCountry,
        category: state.category
    };
}

const mapDispatch = (dispatch) => bindActionCreators({ ...CityCountryStore.actionCreators, ...CategoryStore.actionCreators }, dispatch);

export default connect(mapState, mapDispatch)(StayMap); 