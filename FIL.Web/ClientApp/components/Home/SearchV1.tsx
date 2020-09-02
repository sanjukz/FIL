import * as React from "react";
import { autobind } from "core-decorators";
import CategorySearchResult from "../Search/CategorySearchResult";
import CitySearchResult from "../Search/CitySearchResult";
import StateSearchResult from "../Search/StateSearchResult";
import CountrySearchResult from "../Search/CountrySearchResult";
import * as isEmpty from "lodash/isEmpty";
import { gets3BaseUrl } from "../../utils/imageCdn";
import DefaultCategoryAndSubCategory from "../Search/DefaultCategoryAndSubCategory";
const Typed = require('typed.js');

let delayTimer, typed;

export default class SearchV1 extends React.PureComponent<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            defaultSearchValue: "",
            isTyping: false,
            searchedValue: "",
            searchedKeywords: [],
            s3BaseUrl: gets3BaseUrl()
        }
    }

    public getSearchResults = (e) => {
        let target = e.target;
        if (target) {
            let val = target.value;
            if (val != "") {
                typed.stop();
                this.setState({ isTyping: true, searchedValue: val });
            } else {
                typed.start();
                this.setState({ isTyping: false, searchedValue: "" });
            }
            if (val && val.trim().length > 0) {
                this.props.getAlgoliaResults(val);
            }
            else if (val.trim().length == 0) {
                this.props.clearSearchAction();
            }
        }
        this.props.clearSearchAction();
    }
    @autobind
    public setCommanSearchKeyword(search) {
        var data = [];
        var filter = "";
        if (this.state.searchedKeywords != undefined) {
            var allSearch = this.state.searchedKeywords;
            var isAlreadyExist = allSearch.filter(function (item) {
                return item == search;
            });
            if (isAlreadyExist.length == 0) {
                allSearch.push(search);
                allSearch.map(function (item) {
                    data.push(item);
                    filter = filter + item + ",";
                });
                data.push(search);
            }
            this.setState({ searchedKeywords: allSearch });
        } else {
            var searchInputs = [];
            searchInputs.push(search);
            data.push(search);
            filter = search;
            this.setState({ searchedKeywords: searchInputs });
        }
        filter = filter.replace(/(^,)|(,$)/g, ""); // trim by comma
        this.props.setCommanSearchKeyword(filter);
        this.setState({ defaultSearchValue: search });
    }
    @autobind
    public setCountrySearchKeyword(search) {
        var data = [];
        if (this.state.searchedKeywords != undefined) {
            var allSearch = this.state.searchedKeywords;
            var isAlreadyExist = allSearch.filter(function (item) {
                return item == search;
            });
            if (isAlreadyExist.length == 0) {
                allSearch.push(search);
                allSearch.map(function (item) {
                    data.push(item);
                });
                data.push(search);
            }
            this.setState({ searchedKeywords: allSearch });
        } else {
            var searchInputs = [];
            searchInputs.push(search);
            data.push(search);
            this.setState({ searchedKeywords: searchInputs });
        }
        this.setState({ defaultSearchValue: search });
    }
    @autobind
    public setStateSearchKeyword(search) {
        var data = [];
        if (this.state.searchedKeywords != undefined) {
            var allSearch = this.state.searchedKeywords;
            var isAlreadyExist = allSearch.filter(function (item) {
                return item == search;
            });
            if (isAlreadyExist.length == 0) {
                allSearch.push(search);
                allSearch.map(function (item) {
                    data.push(item);
                });
                data.push(search);
            }
            this.setState({ searchedKeywords: allSearch });
        } else {
            var searchInputs = [];
            searchInputs.push(search);
            data.push(search);
            this.setState({ searchedKeywords: searchInputs });
        }
        this.setState({ defaultSearchValue: search });
    }
    @autobind
    public setCitySearchKeyword(search) {
        var data = [];
        if (this.state.searchedKeywords != undefined) {
            var allSearch = this.state.searchedKeywords;
            var isAlreadyExist = allSearch.filter(function (item) {
                return item == search;
            });
            if (isAlreadyExist.length == 0) {
                allSearch.push(search);
                allSearch.map(function (item) {
                    data.push(item);
                });
                data.push(search);
            }
            this.setState({ searchedKeywords: allSearch });
        } else {
            var searchInputs = [];
            searchInputs.push(search);
            data.push(search);
            this.setState({ searchedKeywords: searchInputs });
        }
        this.setState({ defaultSearchValue: search });
    }

    public onRemoveFilteredSearch(item) {
        var data = this.state.searchedKeywords;
        var i = -1;
        var filter = "";
        if (data.length > 0) {
            data.map(function (item) {
                filter = filter + item + ",";
            });
            filter = filter.replace(/(^,)|(,$)/g, ""); // trim by comma
            this.setState({ searchedKeywords: data });
            this.props.setCommanSearchKeyword(filter);
        } else {
            this.setState({ searchedKeywords: undefined });
            this.props.setCommanSearchKeyword(filter);
        }
        if (window.sessionStorage.getItem('searchKeyword') != null) {
            let searchKeywords = window.sessionStorage.getItem('searchKeyword').split(',');
            let filteredSearch = searchKeywords.filter(function (val) {
                return val != item
            });
            let filteredKeywords = "";
            filteredSearch.map(function (filteredKeyword, index) {
                if (index < filteredSearch.length - 1) {
                    filteredKeywords = filteredKeywords + filteredKeyword + ","
                }
                else {
                    filteredKeywords = filteredKeywords + filteredKeyword;
                }
            })
            sessionStorage.setItem('searchKeyword', filteredKeywords.trim())
        }
    }

    public onRemoveAllSearch = () => {
        this.props.clearSearchAction();
        this.setState({
            searchedKeywords: undefined,
            isTyping: false,
            searchedValue: ""
        });
        window.sessionStorage.removeItem('searchKeyword')
    }

    componentDidMount() {
        if (window.sessionStorage.getItem('searchKeyword') != null && window.sessionStorage.getItem('searchKeyword') != "") {
            var that = this;
            let searchedKeywords = window.sessionStorage.getItem('searchKeyword').split(",");
            searchedKeywords.map(function (item) {
                that.state.searchedKeywords.push(item);
            })
        }
        const options = {
            strings: ['What kind of “feel” are you looking for today?', 'What kind of “feel” are you looking for today? Try Piano Recital',
                'What kind of “feel” are you looking for today? Try Yoga Session', 'What kind of “feel” are you looking for today? Try Cooking Class',
                'What kind of “feel” are you looking for today? Try Seminar', 'What kind of “feel” are you looking for today? Try Gym Workout',
                'What kind of “feel” are you looking for today? Try Jazz Night'],
            // strings: ['Where would you like to “feel” a place today?', 'Where would you like to “feel” a place today? Try Italy', 'How would you like to “feel” a place today? Try Hop-On Hop-Offs', 'Where would you like to “feel” a place today? Try Australia', 'Where would you like to “feel” a place today? Try Agra'],
            typeSpeed: 50,
            attr: 'placeholder',
            backSpeed: 70,
            loop: true,
            showCursor: true,
            smartBackspace: true,
            cursorChar: '|'
        };

        typed = new Typed('#dropdownMenuButton', options);
    }
    componentWillUnmount() {
        typed.destroy();
    }
    public render() {
        let anyResults =
            !isEmpty(this.props.searchResult.categoryEvents) ||
            !isEmpty(this.props.searchResult.cities) ||
            !isEmpty(this.props.searchResult.countries) ||
            !isEmpty(this.props.searchResult.states);

        let showResults =
            (this.props.searchSuccess || this.props.clearSearchSuccess) && anyResults;
        let classNames = "search-dropdown bg-white p-3 border dropdown-menu";
        return (
            <div className="site-search">
                <div className="input-group search-input overflow-hidden shadow">
                    <div className="input-group-prepend">
                        <span className="input-group-text border-0 bg-white" id="basic-addon1"> <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/fil-search.svg" alt="FIL Search" width="24" /> </span>
                    </div>

                    <input type="text" className="form-control border-0" id="dropdownMenuButton"
                        aria-label="Username" aria-describedby="basic-addon1"
                        onChange={this.getSearchResults}
                    />

                </div>
                <div className="search-drop-list shadow bg-white">
                    {this.props.allCategories && this.props.allCategories.length > 0 && this.props.algoliaResults.length == 0 &&

                        <DefaultCategoryAndSubCategory {...this.props} />
                    }
                    {((this.props.searchSuccess || this.props.clearSearchSuccess) || (this.props.algoliaResults.length > 0)) && (
                        <CountrySearchResult
                            searchSuccess={this.props.searchSuccess}
                            emptySearch={this.props.clearSearchSuccess}
                            searchResult={this.props.searchResult.countries}
                            setCountrySearchKeyword={this.setCommanSearchKeyword}
                            defaultCountries={this.props.defaultCountries}
                            innerSearch={false}
                            algoliaResults={this.props.algoliaResults}
                            searchText={this.state.searchedValue}
                        />
                    )}

                    {(this.props.searchSuccess &&
                        showResults &&
                        this.props.searchResult.states.length > 0) || (this.props.algoliaResults.length > 0) && (
                            <StateSearchResult
                                searchSuccess={this.props.searchSuccess}
                                emptySearch={this.props.clearSearchSuccess}
                                searchResult={this.props.searchResult.states}
                                setStateSearchKeyword={this.setCommanSearchKeyword}
                                defaultStates={this.props.defaultStates}
                                innerSearch={false}
                                algoliaResults={this.props.algoliaResults}
                                searchText={this.state.searchedValue}
                            />
                        )}
                    {(this.props.searchSuccess &&
                        showResults &&
                        this.props.searchResult.cities.length > 0) || (this.props.algoliaResults.length > 0) && (
                            <CitySearchResult
                                searchSuccess={this.props.searchSuccess}
                                emptySearch={this.props.clearSearchSuccess}
                                searchResult={this.props.searchResult.cities}
                                setCitySearchKeyword={this.setCommanSearchKeyword}
                                defaultCities={this.props.defaultCities}
                                innerSearch={false}
                                algoliaResults={this.props.algoliaResults}
                                searchText={this.state.searchedValue}
                            />
                        )}
                    {(showResults || this.props.algoliaResults.length > 0) && (
                        <CategorySearchResult
                            searchSuccess={this.props.searchSuccess}
                            emptySearch={this.props.clearSearchSuccess}
                            searchResult={this.props.searchResult.categoryEvents}
                            searchText={this.state.searchedValue}
                            getAlgoliaResults={this.props.getAlgoliaResults}
                            algoliaResults={this.props.algoliaResults}
                            allCategories={this.props.allCategories}
                        />
                    )}
                </div>
            </div>
        )

    }
}
