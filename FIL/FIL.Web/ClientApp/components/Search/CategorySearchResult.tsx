import * as React from "react";
import { Link, RouteComponentProps } from "react-router-dom";
import ImageComponent from "../../../ClientApp/views/Comman/ImageComponent";
import * as isEmpty from "lodash/isEmpty";
import { gets3BaseUrl } from "../../utils/imageCdn";
import CategoryAndSubCategory from './CategoryAndSubCategory';
import { MasterEventTypes } from '../../Enum/MasterEventTypes';

export default class CategorySearchResult extends React.PureComponent<any, any> {
    state = {
        s3BaseUrl: gets3BaseUrl()
    }
    GetCategoryImage(category) {
        let base_url = this.state.s3BaseUrl + "/category-tab-icon/";
        let cat_img_url;
        switch (category) {
            case "See & Do":
                cat_img_url = base_url + "see-and-do.svg";
                break;
            case "Eat & Drink":
                cat_img_url = base_url + "eat-and-drink.svg";
                break;
            case "Shop Local":
                cat_img_url = base_url + "shop-local.svg";
                break;
            case "Experiences & Activities":
                cat_img_url = base_url + "experience-explore.svg";
                break;
            case "Attend Events":
                cat_img_url = base_url + "attend-events.svg";
                break;
            case "Move Around":
                cat_img_url = base_url + "move-around.svg";
                break;
            case "Stay At":
                cat_img_url = base_url + "stay-at.svg";
                break;
            case "Travel To & From":
                cat_img_url = base_url + "travel-to-from.svg";
                break;
            case "Services & Tips":
                cat_img_url = base_url + "services-tips.svg";
                break;
            default:
                cat_img_url = base_url + "see-and-do.svg";
        }
        return cat_img_url;
    }
    public render() {
        if (this.props.searchSuccess && this.props.algoliaResults.length == 0) {
            var viewMore;
            var searchResult = this.props.searchResult;
            if (this.props.searchResult.length > 10) {
                viewMore = <a href={"/advance-search/" + this.props.searchText} className="btn-link text-secondary"><img src={`${this.state.s3BaseUrl}/header/search-icon.png`} alt="search" title="search icon" className="mr-2" width="13" /><small>View All</small></a>
                searchResult = this.props.searchResult.slice(0, 9);
            }
            var seeAndDoResult = searchResult.filter(e => e.parentCategory == 'SeeAndDo').map(function (val) {
                return <a href={val.redirectUrl} className="btn btn-sm btn-outline-secondary btn-block border-0 text-left"><img src={`${this.state.s3BaseUrl}/category-tab-icon/see-do-tickets.svg`} alt="see do tickets" width="16" className="mr-2" /> {val.name + ", " + val.cityName + ", " + val.countryName} </a>
            });
            var eatAndDrinksResult = searchResult.filter(e => e.parentCategory == 'EatAndDrink').map(function (val) {
                return <a href={val.redirectUrl} className="btn btn-sm btn-outline-secondary btn-block border-0 text-left"><img src={`${this.state.s3BaseUrl}/category-tab-icon/eat-drink-tickets.svg`} alt="eat drink tickets" width="16" className="mr-2" /> {val.name + ", " + val.cityName + ", " + val.countryName}</a>
            });
            var shopLocalResult = searchResult.filter(e => e.parentCategory == 'ShopLocal').map(function (val) {
                return <a href={val.redirectUrl} className="btn btn-sm btn-outline-secondary btn-block border-0 text-left"> <img src={`${this.state.s3BaseUrl}/category-tab-icon/shop-local.svg`} alt="shop local" width="16" className="mr-2" /> {val.name + ", " + val.cityName + ", " + val.countryName}</a>
            });
            var experienceResult = searchResult.filter(e => e.parentCategory == 'ExperienceAndExplore').map(function (val) {
                return <a href={val.redirectUrl} className="btn btn-sm btn-outline-secondary btn-block border-0 text-left"><img src={`${this.state.s3BaseUrl}/category-tab-icon/experience-explore.svg`} alt="experience explore" width="16" className="mr-2" /> {val.name + ", " + val.cityName + ", " + val.countryName} </a>
            });
            return <div>
                {!isEmpty(seeAndDoResult) && <div>
                    {seeAndDoResult}
                </div>}
                {!isEmpty(eatAndDrinksResult) && <div>
                    {eatAndDrinksResult}
                </div>}
                {!isEmpty(shopLocalResult) && <div>
                    {shopLocalResult}
                </div>}
                {!isEmpty(experienceResult) && <div>
                    {experienceResult}
                </div>}
                {!isEmpty(viewMore) && <div>
                    {viewMore}
                </div>}
            </div>;
        } else if (this.props.algoliaResults.length > 0) {          // algolia search results rendered here
            var that = this;
            let host_protocol = window.location.host.indexOf("localhost") >= 0 ? "http://" : "https://";
            let host_url = host_protocol + window.location.host;
            let filteredResults = this.props.algoliaResults.filter((item) => { return item.country != "" })
            filteredResults = this.GetFilterResultForLive(filteredResults);
            let algoliaResults = filteredResults.map((item) => {
                let get_category_image = this.GetCategoryImage(item.category);
                if (that.props.isItinerarySearch) {
                    return <a href="JavaScript:Void(0)" onClick={() => { that.props.getSelectedPlace(item) }}
                        className="btn btn-sm btn-outline-secondary btn-block border-0 text-left">
                        <img src={item.placeImageUrl.replace("feelaplace", "feelitlive")} alt="shop local" width="50"
                            onError={(e) => {
                                e.currentTarget.src = this.state.s3BaseUrl + "/places/tiles/tiles-placeholder.jpg"
                            }} className="mr-2 align-top" />
                        <div className="d-inline-block srh-rel-txt"> <b>{item.name}</b> <br />
                            <div style={{ fontSize: "10px" }}>
                                {item.city + ", " + item.country}
                            </div>
                        </div>
                        <div className="pull-right">
                            <img src={get_category_image} alt="tabs icon"
                                width="16" className="d-inline-block align-top" />
                            <span className="badge badge-primary ml-1">{item.subCategory}</span>
                        </div>
                    </a>
                }
                return <a href={`${host_url}${item && item.url ? item.url : '/' + item.url}`} className="btn btn-sm btn-outline-secondary btn-block border-0 text-left">
                    <ImageComponent
                        parentCategorySlug={item.category}
                        subCategorySlug={item.subCategory}
                        s3BaseUrl={that.state.s3BaseUrl}
                        imgUrl={item.placeImageUrl}
                        searchImage={true}
                    />
                    <div className="d-inline-block srh-rel-txt"> <b>{item.name}</b> <br />
                        {!this.IsOnlineEvent(item) && <div style={{ fontSize: "10px" }}>
                            {item.city + ", " + item.country}
                        </div>}
                    </div>
                    <div className="pull-right">
                        <img src={get_category_image} alt="tabs icon"
                            width="16" className="d-inline-block align-top" />
                        <span className="badge badge-primary ml-1">{item.subCategory}</span>
                    </div>

                </a>
            });
            return <div className="px-3">
                {/* {!that.props.isItinerarySearch && this.props.algoliaResults.length > 0 && <div>
                    <a href={`${host_url}/c/see-and-do/hop-on hop-offs?category=29&subcategory=85`} className="btn btn-sm btn-outline-secondary mr-1 ml-1 mb-1" >Hop-On Hop-Offs</a><hr className="mr-2" /> </div>} */}
                {this.props.allCategories && this.props.allCategories.length > 0 &&
                    <CategoryAndSubCategory  {...this.props} />}
                {algoliaResults}
            </div>
        }
        else if (this.props.emptySearch) {
            return <div>
            </div>;
        }
    }

    GetFilterResultForLive = (inputResults) => {
        let isLiveOnlineExists = inputResults.filter((item) => {
            return this.IsOnlineEvent(item);
        })
        if (isLiveOnlineExists.length > 0) {
            let sortedResults = [];
            let otherItems = inputResults.filter((item) => { return !this.IsOnlineEvent(item); })
            sortedResults = [...isLiveOnlineExists, ...otherItems];
            return sortedResults;
        } else {
            return inputResults;
        }
    }

    IsOnlineEvent = (item) => {
        return this.props.allCategories && this.props.allCategories.filter((val) => { return val.masterEventTypeId == MasterEventTypes.Online && item.category == val.displayName }).length > 0 ? true : false;
    }
}
