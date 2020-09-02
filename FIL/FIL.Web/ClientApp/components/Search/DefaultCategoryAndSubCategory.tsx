import * as React from 'react';
import { gets3BaseUrl } from "../../utils/imageCdn";


class DefaultCategoryAndSubCategory extends React.Component<any, any>{
    constructor(props) {
        super(props);
        this.state = {
            s3BaseUrl: gets3BaseUrl()
        }
    }
    render() {
        let that = this;
        let defaultResults = this.props.allCategories.map((item, indx) => {

            let filtredSubCat = item.subCategories.filter((filt) => { return filt.displayName != "All" })
            let currentSubCategories = filtredSubCat.map((val) => {
                return <a className="btn btn-sm btn-outline-secondary mr-1 ml-1 mb-2" href={`/c/${item.slug}/${val.slug}?category=${item.eventCategory}&subcategory=${val.eventCategory}`}>
                    {val.displayName}</a>
            })

            return <>
                <div className="search-cat-main clearfix">
                    <div className="search-cat d-inline-block pull-left">
                        <div className="tabs-icon rounded-circle border text-center m-auto"><img src={`${this.state.s3BaseUrl}/category-tab-icon/${item.slug}.svg`} alt="tabs icon" width="20" /></div>
                        <small className="tabs-title d-block">{item.displayName}</small>
                        {(item.masterEventTypeId == 4) && <div><span className="online-tag">Online</span></div>}
                        {(item.masterEventTypeId != 4) && <div><span className="online-tag-irl">In-Real-Life</span></div>}
                    </div>
                    <div className="search-sub-cab d-block">
                        {currentSubCategories}
                    </div>
                </div>
                {indx != that.props.allCategories.length &&
                    <hr />
                }
            </>
        })


        return <>
            {defaultResults}
        </>
    }

}

export default DefaultCategoryAndSubCategory;