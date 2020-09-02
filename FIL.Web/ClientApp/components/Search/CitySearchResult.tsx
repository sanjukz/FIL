import * as React from "react";
import { Link } from "react-router-dom";

export default class CitySearchResult extends React.Component<any, any> {

    public render() {
        let searchResult = [];
        let cities = [];
        if (this.props.algoliaResults.length > 0 && this.props.searchText.trim().length > 0) {
            if (this.props.algoliaResults.length > 0) {
                let get_distinct_cities = this.GetDistinctCities(this.props.algoliaResults);
                get_distinct_cities = get_distinct_cities.filter((item) => { return item.cityName != "" })
                searchResult = get_distinct_cities.map(val => <a className="btn btn-sm btn-outline-secondary mr-1 ml-1 mb-1" href={`country/${val.countryName.split(/[\d .-]+/).join("").toLowerCase()}/${val.cityName.split(/[\d .-]+/).join("").toLowerCase()}?country=${val.countryId}&city=${val.cityId}`}>
                    {val.cityName}
                </a>);
            }
            else {
                return null
            }
        } else {

            if (this.props.searchSuccess) {
                this.props.searchResult.map((item) => { // Get distinct cities...
                    if (cities.filter((val) => { return item.cityName == val.cityName }).length == 0) {
                        cities.push(item);
                    }
                });
                searchResult = cities.map(val => <a className="btn btn-sm btn-outline-secondary mr-1 ml-1 mb-2" href={`country/${val.countryName.split(/[\d .-]+/).join("").toLowerCase()}/${val.cityName.split(/[\d .-]+/).join("").toLowerCase()}?country=${val.countryId}&city=${val.cityId}`}>
                    {val.cityName}
                </a>);
            }
        }
        if (searchResult.length) {
            return <div className="border-bottom my-3 pb-3 px-3">
                <h6 className="small">City</h6>
                {searchResult}
                {/* <hr className="my-2" /> */}
            </div>;
        } else {
            return <div></div>
        }
    }
    GetDistinctCities(algoliaResults) {
        let distinct_cities = [], filteredCities = [];
        algoliaResults.map((item) => {
            if (distinct_cities.indexOf(item.city) == -1 && item.category != "Live Stream") {
                distinct_cities.push(item.city);
            }
        });
        if (distinct_cities.length > 0) {
            distinct_cities.map((item) => {
                let cityId = algoliaResults.filter(val => {
                    return val.city == item
                })
                let data = {
                    countryId: cityId[0].countryId,
                    countryName: cityId[0].country,
                    stateId: cityId[0].stateId,
                    cityName: cityId[0].city,
                    cityId: cityId[0].cityId
                }
                filteredCities.push(data);
            })

        }
        return filteredCities;
    }
}
