import * as React from "react";
import { Link } from "react-router-dom";

export default class StateSearchResult extends React.Component<any, any> {

    public render() {
        var that = this;
        let searchResult = [];
        let states = [];
        if (this.props.algoliaResults.length > 0 && this.props.searchText.trim().length > 0) {
            if (this.props.algoliaResults.length > 0) {
                let get_distinct_states = this.GetDistinctStates(this.props.algoliaResults);
                get_distinct_states = get_distinct_states.filter((item) => { return item.stateName != "" })
                searchResult = get_distinct_states.map(val => {
                    return <a className="btn btn-sm btn-outline-secondary mr-1 ml-1 mb-1" href={`country/${val.countryName.split(/[\d .-]+/).join("").toLowerCase()}/${val.stateName.split(/[\d .-]+/).join("").toLowerCase()}?country=${val.countryId}&state=${val.stateId}`}>
                        {val.stateName}
                    </a>
                });
            }
            else {
                return null;
            }
        }
        else {
            if (this.props.searchSuccess) {
                this.props.searchResult.map((item) => { // Get distinct states...
                    if (states.filter((val) => { return item.stateName == val.stateName }).length == 0) {
                        states.push(item);
                    }
                });
                searchResult = states.map(val => {
                    return <a className="btn btn-sm btn-outline-secondary mr-1 ml-1 mb-2" href={`country/${val.countryName.split(/[\d .-]+/).join("").toLowerCase()}/${val.stateName.split(/[\d .-]+/).join("").toLowerCase()}?country=${val.countryId}&state=${val.stateId}`}>
                        {val.stateName}
                    </a>
                });
            }
        }
        if (searchResult.length) {
            return <div className="border-bottom my-3 pb-3 px-3">
                <h6 className="small">State</h6>
                {searchResult}
                {/* <hr className="my-2" /> */}
            </div>;
        } else {
            return <div></div>
        }
    }
    GetDistinctStates(algoliaResults) {
        let distinct_states = [], filteredStates = [];
        algoliaResults.map((item) => {
            if (distinct_states.indexOf(item.state) == -1 && item.category != "Live Stream") {
                distinct_states.push(item.state);
            }
        });
        if (distinct_states.length > 0) {
            distinct_states.map((item) => {
                let stateId = algoliaResults.filter(val => {
                    return val.state == item
                })
                let data = {
                    countryId: stateId[0].countryId,
                    countryName: stateId[0].country,
                    stateId: stateId[0].stateId,
                    stateName: stateId[0].state
                }
                filteredStates.push(data);
            })

        }
        return filteredStates;
    }
}
