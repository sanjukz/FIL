import * as React from 'react';
import { gets3BaseUrl } from "../../utils/imageCdn";


class CategoryAndSubCategory extends React.Component<any, any>{
    constructor(props) {
        super(props);
        this.state = {
            s3BaseUrl: gets3BaseUrl()
        }
    }
    render() {
        let getAlgoliaCategories = this.GetDistinctCategories(this.props.algoliaResults);
        let filteredResults = getAlgoliaCategories.categories.map((item) => {
            let filteredSubCat = getAlgoliaCategories.subCategories.filter((cat) => {
                return cat.categoryName == item.categoryName
            })
            filteredSubCat = filteredSubCat.filter((filt) => { return filt.subCategoryName != "All" })
            let subCategories = filteredSubCat.map((val) => {
                return <a className="btn btn-sm btn-outline-secondary mr-1 ml-1 mb-2" href={`/c/${item.slug}/${val.slug}?category=${item.categoryId}&subcategory=${val.subCategoryId}`}>
                    {val.subCategoryName}</a>
            })
            return <>
                <div className="search-cat-main clearfix">
                    <div className="search-cat d-inline-block pull-left">
                        <div className="tabs-icon rounded-circle border text-center m-auto"><img src={`${this.state.s3BaseUrl}/category-tab-icon/${item.slug}.svg`} alt="tabs icon" width="20" /></div>
                        <small className="tabs-title d-block">{item.categoryName}</small>
                    </div>
                    <div className="search-sub-cab d-block">
                        {subCategories}
                    </div>
                </div>
                <hr />
            </>
        })
        return <div>
            {filteredResults}
        </div>
    }
    GetDistinctCategories(algoliaResults) {
        let distinct_categories = [], that = this,
            filteredCategories = [], SubCategories = [];
        if (algoliaResults && algoliaResults.length > 0) {
            algoliaResults.map(item => {
                if (distinct_categories.indexOf(item.category) == -1) {
                    distinct_categories.push(item.category);
                }
            });
            if (distinct_categories.length > 0) {
                distinct_categories.map(item => {
                    let categoryName = algoliaResults.filter(val => {
                        return val.category == item;
                    });
                    let currentCategory = that.props.allCategories && that.props.allCategories.filter((cat) => {
                        return cat.displayName == categoryName[0].category
                    })
                    if (currentCategory && currentCategory.length > 0) {
                        let data = {
                            categoryName: categoryName[0].category,
                            categoryId: currentCategory[0].eventCategory,
                            slug: currentCategory[0].slug
                        };
                        filteredCategories.push(data);
                    }
                });
            }
            filteredCategories.map((item) => {
                let distinct_subcategories = [];
                let filteredSubCategories = algoliaResults.filter((val) => {
                    return val.category == item.categoryName
                });
                if (filteredSubCategories && filteredSubCategories.length > 0) {
                    filteredSubCategories.map((val) => {
                        if (distinct_subcategories.indexOf(val.subCategory) == -1) {
                            distinct_subcategories.push(val.subCategory);
                        }
                    });
                }
                distinct_subcategories.map((val) => {
                    let currentSubCategoryModel = that.props.allCategories.filter((subcat) => {
                        return subcat.eventCategory == item.categoryId
                    });
                    if (currentSubCategoryModel && currentSubCategoryModel.length > 0) {
                        let filteredSubCatmodel = currentSubCategoryModel[0].subCategories.filter((filtSubCat) => {
                            return filtSubCat.displayName == val
                        })
                        if (filteredSubCatmodel && filteredSubCatmodel.length > 0) {
                            let data = {
                                categoryId: item.categoryId,
                                categoryName: item.categoryName,
                                subCategoryName: val,
                                subCategoryId: filteredSubCatmodel[0].eventCategory,
                                slug: filteredSubCatmodel[0].slug
                            }
                            SubCategories.push(data);
                        }
                    }
                })
            })
        }
        let responseData = {
            categories: filteredCategories,
            subCategories: SubCategories
        }
        return responseData;
    }
}

export default CategoryAndSubCategory;