import * as React from 'React';
import { connect } from "react-redux";
import { IApplicationState } from "../../stores/index";
import * as AllCategories from "../../stores/AllCategories";
import * as CurrentSelectedCategory from "../../stores/CurrentSelectedCategory";
import { bindActionCreators } from "redux";
import * as isEqual from "lodash/isEqual";
import HomeCategorySkeleton from "./HomeCategorySkeleton";
import { gets3BaseUrl } from "../../utils/imageCdn";
import { MasterEventTypes } from "../../Enum/MasterEventTypes";
import * as FeelPlacesStore from "../../stores/FeelPlaces";

type HomeCategorySectionProps = AllCategories.IAllCategoriesProps
    & CurrentSelectedCategory.ICurrentSelectedCategoryProps
    & FeelPlacesStore.IFeelPlacesState
    & typeof AllCategories.actionCreators
    & typeof FeelPlacesStore.actionCreators
    & typeof CurrentSelectedCategory.actionCreators;

class HomeCategorySectionV1 extends React.PureComponent<any, any> {
    state = {
        s3BaseUrl: gets3BaseUrl(),
        masterCategoryType: MasterEventTypes.Online
    }
    componentDidMount() {
        if (this.props.allCategories.isLoading == false && this.props.allCategories.fetchSuccess == false) {
            this.props.getAllCategories();
        }
    }

    setSelectedCategory = (item) => {
        this.props.setCurrentSelectedCategory(item);
        typeof window != "undefined" && window.sessionStorage.setItem("category", item.displayName);
    }

    public render() {
        if (this.props.allCategories.fetchSuccess) {
            let allCategories = this.props.allCategories.allCategories.filter((val) => {
                if (this.state.masterCategoryType == MasterEventTypes.Online) {
                    return val.masterEventTypeId == this.state.masterCategoryType
                } else {
                    return val.masterEventTypeId != MasterEventTypes.Online
                }
            })
            return (
                <div>
                    <section className="container fil-home-tab">
                        <a
                            href="javascript:Void(0)"
                            className={`btn p-0 btn-outline-cust new-tag ${this.state.masterCategoryType == MasterEventTypes.Online ? 'active' : ''}`}
                            onClick={() => {
                                this.setState({ masterCategoryType: MasterEventTypes.Online }, () => {
                                    this.props.updateShowHideState(false)
                                })
                            }}>
                            Online Experiences
                        </a>
                        <a
                            href="javascript:Void(0)"
                            className={`btn p-0 btn-outline-cust ${this.state.masterCategoryType == MasterEventTypes.InRealLife ? 'active' : ''}`}
                            onClick={() => {
                                this.setState({ masterCategoryType: MasterEventTypes.InRealLife }, () => {
                                    this.props.updateShowHideState(true)
                                })
                            }} >
                            In-Real-Life Experiences
                        </a>
                    </section>
                    <section className="fil-cat bg-light text-center">
                        <div className="container">
                            {this.props.allCategories.fetchSuccess && allCategories.map((item, index) => (
                                <div className="cat-list d-inline-block">
                                    <a href={`/c/${item.slug}?category=${item.eventCategory}`}>
                                        <span className="shadow-sm"><img src={`${this.state.s3BaseUrl}/category-tab-icon/${item.slug}${isEqual(item, this.props.selectedCategory.currentSelectedCategory) ? "-active" : ""}.svg`} alt="tabs icon" /></span>
                                        {item.displayName}
                                    </a>
                                </div>))
                            }
                        </div>
                    </section >
                </div>
            );
        } else {
            return <section className="fil-cat bg-light text-center">
                <div className="container">
                    <div className="cat-list d-inline-block">
                        <HomeCategorySkeleton />
                    </div>
                    <div className="cat-list d-inline-block">
                        <HomeCategorySkeleton />
                    </div>
                    <div className="cat-list d-inline-block">
                        <HomeCategorySkeleton />
                    </div>
                    <div className="cat-list d-inline-block">
                        <HomeCategorySkeleton />
                    </div>
                    <div className="cat-list d-inline-block">
                        <HomeCategorySkeleton />
                    </div>
                    <div className="cat-list d-inline-block">
                        <HomeCategorySkeleton />
                    </div>
                    <div className="cat-list d-inline-block">
                        <HomeCategorySkeleton />
                    </div>
                    <div className="cat-list d-inline-block">
                        <HomeCategorySkeleton />
                    </div>
                    <div className="cat-list d-inline-block">
                        <HomeCategorySkeleton />
                    </div>

                </div>
            </section >
        }
    }
}

const mapState = (state: IApplicationState) => ({
    categoryPlaces: state.categoryPlaces,
    allCategories: state.allCategories,
    selectedCategory: state.currentSelectedCategory
});
const mapDispatch = (dispatch) => bindActionCreators({
    ...AllCategories.actionCreators,
    ...CurrentSelectedCategory.actionCreators,
    ...FeelPlacesStore.actionCreators
}, dispatch);

export default connect(mapState, mapDispatch)(HomeCategorySectionV1);