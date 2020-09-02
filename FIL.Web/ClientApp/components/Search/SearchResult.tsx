import * as React from "react";
import CitySearchResult from "./CitySearchResult";
import StateSearchResult from "./StateSearchResult";
export default class SearchResult extends React.Component<any, any> {

    public render() {
        var that = this;
        if (this.props.searchSuccess) {
            var searchResult = this.props.searchResult.map(function (val) {
                return <button type="button" onClick={() => { that.props.setSearchKeyword(val.name) }} className="btn btn-sm btn-outline-secondary mr-1 ml-1">{val.name}</button>;
            });
            if (searchResult != "") {
                return <div>
                    {searchResult}
                </div>
            }
            else {
                return <div>
                    <h6 style={{ color: 'black' }}>No result found</h6>
                </div>
            }

        } else if (this.props.emptySearch && !this.props.innerSearch) {
            return <div>
                <button type="button" onClick={() => { this.props.setSearchKeyword("Jaipur") }} className="btn btn-sm btn-outline-secondary mr-1 ml-1">Jaipur</button>
                <button type="button" onClick={() => { this.props.setSearchKeyword("New Delhi") }} className="btn btn-sm btn-outline-secondary mr-1 ml-1">New Delhi</button>
                <button type="button" onClick={() => { this.props.setSearchKeyword("Mumbai") }} className="btn btn-sm btn-outline-secondary mr-1 ml-1">Mumbai</button>
                <button type="button" onClick={() => { this.props.setSearchKeyword("Agra") }} className="btn btn-sm btn-outline-secondary mr-1 ml-1">Agra</button>
                <button type="button" onClick={() => { this.props.setSearchKeyword("Ajmer") }} className="btn btn-sm btn-outline-secondary mr-1 ml-1">Ajmer</button>
                <button type="button" onClick={() => { this.props.setSearchKeyword("Hyderabad") }} className="btn btn-sm btn-outline-secondary mr-1 ml-1">Hyderabad</button>
            </div>;
        }
        else if (this.props.emptySearch && this.props.innerSearch) {
            return <div>
            </div>;
        }
    }
}
