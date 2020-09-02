import * as React from "react";
import { autobind } from "core-decorators";
import { connect } from "react-redux";
import CategorySearchResult from "../Search/CategorySearchResult";
import CitySearchResult from "../Search/CitySearchResult";
import StateSearchResult from "../Search/StateSearchResult";
import CountrySearchResult from "../Search/CountrySearchResult";
import * as isEmpty from "lodash/isEmpty";
import * as siteAssetStore from "../../stores/SiteAsset";
import { IApplicationState } from "../../stores/index";
import * as CategoryStore from "../../stores/Category";
import * as PubSub from "pubsub-js";
import { bindActionCreators } from 'redux';

class InnerPageSearch extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            defaultSearchValue: '',
            searchValue: ''
        }
    }
    componentDidMount() {
        if (!this.props.siteAsset.fetchSuccess) {
            this.props.requestSiteAssest();
        }
        PubSub.subscribe('UPDATE_SEARCH_CATEGORY', this.subscriber.bind(this));
    }

    UNSAFE_componentWillMount() {
        this.setState({ defaultSearchValue: sessionStorage.searchKeyword ? sessionStorage.searchKeyword : '' });
    }

    public subscriber(msg, data) {
        this.setState({ defaultSearchValue: data, searchValue: data });
    }

    @autobind
    public getSearchResults(e) {
        let target = e.target;
        if (target) {
            let val = target.value;
            // if (val && val.trim().length > 2) {
            //     let siteLevel = 4;
            //     debounce(() => {
            //         this.props.searchAction(val.trim(), siteLevel);
            //     }, 300)();
            // }
            if (val && val.trim().length > 0) {
                this.props.getAlgoliaResults(val);
                this.setState({ searchValue: val })
            }
            else if (val.trim().length == 0) {
                this.props.clearSearchAction();
                this.setState({ searchValue: val })
            }
        }
        this.props.clearSearchAction();
    }

    @autobind
    public setCountrySearchKeyword(search, e) {
        // this.props.setCountrySearchKeyword(search);
        this.setState({ defaultSearchValue: search });
    }

    @autobind
    public setStateSearchKeyword(search, e) {
        //  this.props.setStateSearchKeyword(search);
        this.setState({ defaultSearchValue: search });
    }

    @autobind
    public setCitySearchKeyword(search, e) {
        // this.props.(search);
        this.setState({ defaultSearchValue: search });
    }

    public render() {
        let anyResults =
            !isEmpty(this.props.category.searchResult.categoryEvents)
        let showResults =
            (this.props.category.searchSuccess || this.props.category.clearSearchSuccess) && anyResults;
        let classNames = "search-dropdown bg-white p-3 border dropdown-menu";
        if (this.props.siteAsset.fetchSuccess) {
            return (
                <div className='inner-search'>
                    <i className="fa fa-search search-icon"></i>
                    <input
                        type="text"
                        className="form-control"
                        placeholder={this.state.defaultSearchValue !== '' ? `${this.state.defaultSearchValue}` : `Where would you like to “feel” a place today?`}
                        id="dropdownInnerSearch"
                        onFocus={() => { this.setState({ defaultSearchValue: '' }) }}
                        onBlur={() => {
                            this.setState({
                                defaultSearchValue: this.state.searchValue
                            })
                        }}
                        data-toggle="dropdown"
                        aria-expanded="false"
                        aria-haspopup="true"
                        onChange={this.getSearchResults}
                    />
                    <div
                        className={anyResults ? classNames : `${classNames} search-hidden`}
                        aria-labelledby="dropdownInnerSearch"
                        x-placement="bottom-start"
                    >
                        {((this.props.category.searchSuccess || this.props.category.clearSearchSuccess) || (this.props.category.algoliaResults.length > 0)) && (
                            <CountrySearchResult
                                searchSuccess={this.props.category.searchSuccess}
                                emptySearch={this.props.category.clearSearchSuccess}
                                searchResult={this.props.category.searchResult.countries}
                                defaultCountries={this.props.siteAsset.defaultSearchCountries}
                                innerSearch={false}
                                algoliaResults={this.props.category.algoliaResults}
                                searchText={this.state.searchValue}
                            />
                        )}
                        {(this.props.category.searchSuccess &&
                            showResults &&
                            this.props.category.searchResult.states.length > 0) || (this.props.category.algoliaResults.length > 0) && (
                                <StateSearchResult
                                    searchSuccess={this.props.category.searchSuccess}
                                    emptySearch={this.props.category.clearSearchSuccess}
                                    searchResult={this.props.category.searchResult.states}
                                    defaultStates={this.props.siteAsset.defaultStates}
                                    innerSearch={false}
                                    algoliaResults={this.props.category.algoliaResults}
                                    searchText={this.state.searchValue}
                                />
                            )}
                        {(this.props.category.searchSuccess &&
                            showResults &&
                            this.props.category.searchResult.cities.length > 0) || (this.props.category.algoliaResults.length > 0) && (
                                <CitySearchResult
                                    searchSuccess={this.props.category.searchSuccess}
                                    emptySearch={this.props.category.clearSearchSuccess}
                                    searchResult={this.props.category.searchResult.cities}
                                    defaultCities={this.props.siteAsset.defaultCities}
                                    innerSearch={false}
                                    algoliaResults={this.props.category.algoliaResults}
                                    searchText={this.state.searchValue}
                                />
                            )}
                        {(showResults || this.props.category.algoliaResults.length > 0) && (
                            <CategorySearchResult
                                searchSuccess={this.props.category.searchSuccess}
                                emptySearch={this.props.category.clearSearchSuccess}
                                searchResult={this.props.category.searchResult.categoryEvents}
                                getAlgoliaResults={this.props.getAlgoliaResults}
                                algoliaResults={this.props.category.algoliaResults}

                            />
                        )}
                    </div>
                </div>
            );
        } else {
            return null;
        }
    }
}
const mapState = (state: IApplicationState) => {
    return {
        siteAsset: state.siteAsset,
        category: state.category
    };
};

const mapProps = dispatch => bindActionCreators({ ...siteAssetStore.actionCreators, ...CategoryStore.actionCreators }, dispatch);

export default connect(mapState, mapProps)(InnerPageSearch);
