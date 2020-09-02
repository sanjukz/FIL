import * as React from "react";
import SearchV1 from "../components/Home/SearchV1";
import { bindActionCreators } from 'redux';
import { IApplicationState } from "../stores";
import * as SiteAsset from "../stores/SiteAsset";
import * as CategoryStore from "../stores/Category";
import * as AllCategories from "../stores/AllCategories";
import { connect } from 'react-redux'; 

// export const NoMatchPage = () => {
//   return (
//     <div className="home-view-wrapper text-center container py-5 my-5">
//       <h3>404 - Page Not found</h3>
//       <a href="/" className="btn bnt-lg site-primery-btn mt-4">
//         Go To Home
//       </a>
//     </div>
//   );
// };
const NoMatchPage = (props: any) => {

  function getCurrentSlug(){
        var slug = "all-top-29";
        if (localStorage.getItem('currentSlug') != null) {
            slug = localStorage.getItem('currentSlug');
        }
        return slug;
  };
  
  function setCommanSearchKeyword(search) {
      localStorage.removeItem("_previous_places");
      var slug = getCurrentSlug();
      window.sessionStorage.setItem('searchKeyword', search);
      this.props.getCategoryEventsByPaginationidex(1, slug, search);
  }

  const baseImgixUrl = 'https://feelitlive.imgix.net/images';
  return (
    <div className="fil-site fil-home-page">
      <section className="site-banner">
            <div className="container">
                <div className="card-deck">
                    <div className="card border-0 m-0 my-auto">
                        <h1 className="text-purple m-0"><div>Oh hey there...</div> This is awkward</h1>
                        <div className="subtitle">
                        It seems, the page you are looking for decided to have some ‘me time’ to enjoy one of our entertaining experiences, and it’s currently unavailable.
                            <div className="mt-4">Sorry about that! It’s not you. It’s definitely us!</div>
                        </div>
                        <SearchV1
                            clearSearchAction={props.clearSearchAction}
                            searchAction={props.searchAction}
                            searchSuccess={props.category.searchSuccess}
                            clearSearchSuccess={props.category.clearSearchSuccess}
                            searchResult={props.category.searchResult}
                            setCommanSearchKeyword={setCommanSearchKeyword}
                            defaultSearchValue={""}
                            defaultCities={props.siteAsset.defaultSearchCities}
                            defaultStates={props.siteAsset.defaultSearchStates}
                            defaultCountries={props.siteAsset.defaultSearchCountries}
                            siteLevel={props.siteAsset.jumbotron.siteLevel}
                            getCategoryEvents={props.getCategoryEvents}
                            getAlgoliaResults={props.getAlgoliaResults}
                            algoliaResults={props.category.algoliaResults}
                            allCategories={props.allCategories.allCategories}
                          />
                    </div>
                    <div className="card border-0 m-0">
                      <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-404.svg"
                        className="w-100" alt="fil 404" />
                    </div>
                </div>
            </div>
        </section>
    </div>
  );
};

const mapState = (state: IApplicationState) => {
  return {
      siteAsset: state.siteAsset,
      category: state.category,
      allCategories: state.allCategories
  };
};

const mapProps = dispatch => bindActionCreators({ ...SiteAsset.actionCreators, ...CategoryStore.actionCreators, ...AllCategories.actionCreators }, dispatch);

export default connect(mapState, mapProps)(NoMatchPage);