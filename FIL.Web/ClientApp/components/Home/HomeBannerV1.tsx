import * as React from 'react';
import { IApplicationState } from "../../stores";
import SearchV1 from "./SearchV1";
import { bindActionCreators } from 'redux';
import * as SiteAsset from "../../stores/SiteAsset";
import * as CategoryStore from "../../stores/Category";
import * as AllCategories from "../../stores/AllCategories";
import { connect } from 'react-redux';
import { gets3BaseUrl } from "../../utils/imageCdn";

class HomeBannerSectionV1 extends React.PureComponent<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            commanSearchKeyword: "",
            citySearchKeyword: "",
            stateSearchKeyword: "",
            countrySearchKeyword: "",
            defaultSearchValue: "",
            searchKeyword: "",
            s3BaseUrl: gets3BaseUrl(),
            baseImgixUrl: 'https://feelitlive.imgix.net/images'
        };
    }

    public clearSearchAction = () => {
        this.props.clearSearchAction();
        this.setState({
            citySearchKeyword: "",
            stateSearchKeyword: "",
            countrySearchKeyword: "",
            defaultSearchValue: "",
            searchKeyword: ""
        });
    };

    public getCurrentSlug = () => {
        var slug = "all-top-29";
        if (localStorage.getItem('currentSlug') != null) {
            slug = localStorage.getItem('currentSlug');
        }
        return slug;
    };

    public setCommanSearchKeyword = (search) => {
        localStorage.removeItem("_previous_places");
        var slug = this.getCurrentSlug();
        this.setState({
            searchKeyword: search
        });
        window.sessionStorage.setItem('searchKeyword', search);
        this.props.getCategoryEventsByPaginationidex(1, slug, search);
    }


    componentDidMount() {
        if (!this.props.siteAsset.fetchSuccess) {
            this.props.requestSiteAssest();
        }
        this.props.getAllCategories();
    }

    render() {
        let that = this;
        const { baseImgixUrl } = this.state;
        return <section className="site-banner">
            <div className="container">
                <div className="card-deck">
                    <div className="card border-0 m-0 my-auto">
                        <h1 className="text-purple m-0"><div>The world</div> is yours to feel!</h1>
                        <div className="subtitle">
                            Online or In-Real-Life.
                            <div>Discover, plan, and experience everything your way.</div>
                        </div>
                        <SearchV1
                            clearSearchAction={this.clearSearchAction}
                            searchAction={this.props.searchAction}
                            searchSuccess={this.props.category.searchSuccess}
                            clearSearchSuccess={this.props.category.clearSearchSuccess}
                            searchResult={this.props.category.searchResult}
                            setCommanSearchKeyword={this.setCommanSearchKeyword}
                            defaultSearchValue={this.state.defaultSearchValue}
                            defaultCities={this.props.siteAsset.defaultSearchCities}
                            defaultStates={this.props.siteAsset.defaultSearchStates}
                            defaultCountries={this.props.siteAsset.defaultSearchCountries}
                            siteLevel={this.props.siteAsset.jumbotron.siteLevel}
                            getCategoryEvents={this.props.getCategoryEvents}
                            getAlgoliaResults={this.props.getAlgoliaResults}
                            algoliaResults={this.props.category.algoliaResults}
                            allCategories={this.props.allCategories.allCategories} />
                    </div>
                    <div className="card border-0 m-0">
                        <div className="row">
                            <div className="col-sm-6 p-0">
                                <div className="card border-0 overflow-hidden shadow rounded-box">
                                    <img src={`${baseImgixUrl}/fil-images/online-experiences.gif?q=55`}
                                        className="card-img-top" alt="Online Experiences" />
                                    <div className="card-body">
                                        <h5 title="Online Experiences" className="card-title small m-0">Online Experiences</h5>
                                        <p className="iconlink m-0">Explore the world from the comfort of your home.</p>
                                    </div>
                                </div>
                            </div>
                            <div className="col-sm-6 p-0">
                                <div className="card border-0 overflow-hidden shadow rounded-box">
                                    <img src={`${baseImgixUrl}/fil-images/fil-irl.gif?q=55`}
                                        className="card-img-top" alt="In-Real-Life" />
                                    <div className="card-body">
                                        <h5 title="In-Real-Life" className="card-title small m-0">In-Real-Life</h5>
                                        <p className="iconlink m-0">Nearby or far away, discover places and experiences that inspire you.</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    }
}

const mapState = (state: IApplicationState) => {
    return {
        siteAsset: state.siteAsset,
        category: state.category,
        allCategories: state.allCategories
    };
};

const mapProps = dispatch => bindActionCreators({ ...SiteAsset.actionCreators, ...CategoryStore.actionCreators, ...AllCategories.actionCreators }, dispatch);

export default connect(mapState, mapProps)(HomeBannerSectionV1);