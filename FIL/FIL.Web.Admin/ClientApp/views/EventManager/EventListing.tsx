import * as React from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import { IApplicationState } from "../../stores";
import * as EventListingStore from "../../stores/EventData";
import "./EventManager.scss";

type EventListingComponentStateProps = EventListingStore.IEventComponentState & typeof EventListingStore.actionCreators;
export class EventListing extends React.Component<EventListingComponentStateProps, EventListingStore.IEventComponentState> {

    constructor(props) {
        super(props);
        this.handlechange = this.handlechange.bind(this);
        this.SelectCategory = this.SelectCategory.bind(this);
    }
    public navpath = "edit";

    public componentDidMount() {
        setTimeout(() => {
            if (window.location.href.indexOf("eventsorting") > 0) {
                this.navpath = "eventsiteidmap";
            }
        }, 100);
        this.props.requestEventdata();
        this.props.RequestEventCategories();
    }

    public setSelectedPlace(item) {
        sessionStorage.setItem("isReload", "true");
        localStorage.setItem("selectedplace", JSON.stringify(item));
    }

    public handlechange(event) {
        let val = event.target.value;
        this.props.searchEventdata(val, this.props.CategoriesId, this.props.SubCategories, this.props.categorylist);
    }

    public SelectCategory(selectedcategory) {
        let ispresent = true;
        let category = this.props.CategoriesId;
        let Subcategory = this.props.SubCategories;
        let searchstring = this.props.searchstring;
        this.props.categorylist.map((e, index) => {
            if (e.value === selectedcategory.value || e.value === selectedcategory.categoryId) {
                ispresent = false;
            }
        });
        if (ispresent) {
            if (selectedcategory.categoryId === 0) {
                category.push(selectedcategory.value);
                this.props.eventcategories.categories.forEach((i, index) => {
                    if (i.categoryId === selectedcategory.value) {
                        Subcategory.categories.push(i);
                    }

                });
            }
            else {
                Subcategory.categories.push(selectedcategory);
            }
            var mergedcategories = this.props.categorylist;
            mergedcategories.push(selectedcategory);

            this.props.searchEventdata(searchstring, category, Subcategory, mergedcategories);

        }
    }

    RemoveCategory(item) {
        let mergedcategories = [];
        let Category = [];
        let SubCategory = JSON.parse(JSON.stringify(this.props.SubCategories));
        SubCategory.categories = [];
        let searchstring = this.props.searchstring;
        this.props.categorylist.forEach((data, index) => {
            if (data.value !== item.value) {
                mergedcategories.push(data);
            }
        });

        if (item.categoryId === 0) {
            this.props.CategoriesId.forEach((data, index) => {
                if (data !== item.value) {
                    if (Category.indexOf(data) < 0) {
                        Category.push(data);
                    }
                }
            });
            this.props.SubCategories.categories.forEach((data, index) => {
                if (data.categoryId !== item.value) {
                    if (SubCategory.categories.indexOf(data) < 0)
                        SubCategory.categories.push(data);
                }
            });
        }
        else {
            this.props.SubCategories.categories.forEach((data, index) => {
                if (data.categoryId !== item.value) {
                    if (SubCategory.categories.indexOf(data) < 0)
                        SubCategory.categories.push(data);
                }
            })
        }
        if (Category.length == 0 && SubCategory.categories.length == 0 && searchstring == null) {
            this.props.requestSearchEventdata(searchstring, Category, SubCategory, mergedcategories);
        }
        else this.props.searchEventdata(searchstring, Category, SubCategory, mergedcategories);
    }


    public render() {
        if (!this.props.eventcategories) return null;
        const data = this.props.events.venues;
        const data1 = this.props.eventcategories.categories;
        let Events = [];
        let category = [];
        let Subcategory = [];

        data1.forEach((i, index) => {
            if (i.categoryId === 0) {


                if (this.props.categorylist.indexOf(i) !== -1) {
                    let a = <span key={index} className="categorylist alreadyselected">
                        {i.displayName}
                    </span>;
                    category.push(a);
                }
                else {
                    let a = <span key={index} className="categorylist" onClick={() => this.SelectCategory(i)}>
                        {i.displayName}
                    </span>;
                    category.push(a);
                }
            }
            else {
                if (this.props.categorylist.indexOf(i) !== -1 || this.props.CategoriesId.indexOf(i.categoryId) !== -1) {
                    let b = <span key={index} className="categorylist alreadyselected">
                        {i.displayName}
                    </span>
                    Subcategory.push(b);
                }
                else {
                    let b = <span key={index} className="categorylist" onClick={() => this.SelectCategory(i)}>
                        {i.displayName}
                    </span>
                    Subcategory.push(b);

                }
            }

        });



        let selectedcategory = this.props.categorylist.map((i, index) => {
            return (
                <span key={index} className="categorylist1 mb-0">
                    {i.displayName}
                    <span className="vM" onClick={() => this.RemoveCategory(i)} ></span>
                </span>);

        });

        if (data) {

            Events = data.map((item) => {

                let categories;
                let subcategory;

                // let sortorder ;
                this.props.eventcategories.categories.forEach((i, index) => {

                    if (i.value === item.eventCategoryId) {
                        let name = i.displayName;
                        subcategory = <span className="badge badge-primary mr-1">{name}</span>;

                        for (let j = 0; j < this.props.eventcategories.categories.length; j++) {
                            if (this.props.eventcategories.categories[j].value === i.categoryId) {
                                categories = <span className="badge badge-primary mr-1">{this.props.eventcategories.categories[j].displayName}</span>;
                                //sortorder=<span className="btn-default categorybutton">{this.props.eventcategories.categories[j].order}</span>;
                            }
                        }
                    }
                });
                // let SortOrder=<button type="button" className="btn btn-default btn-primary btn-sm categorybutton">Sort Order: {item}</button>

                return (<div className="col-md-3">
                    <div className="card-body p-2">
                        <h6 className="card-title m-0 p-0">
                            <span style={{ fontSize: "1.5rem", marginRight: "5" }}>{item.name.substring(0, 25)}</span>
                            <Link to={"/" + this.navpath + "/" + item.altId} onClick={() => this.setSelectedPlace(item)} className="text-dark"><i style={{ fontSize: "15" }} className="fa fa-edit" aria-hidden="true"></i></Link>
                        </h6>
                        <div className="place-tags">
                            {categories}
                            {subcategory}
                        </div>
                    </div>


                </div>)
            });
        }

        return (
            <div className="card border-0 right-cntent-area pb-5 bg-light">
                <div className="card-body bg-light p-0">
                    <div className="d-inline main-search">
                        <div className="content-area">
                            <div className="row" style={{ marginLeft: "20", marginRight: "20" }} >
                                <div className="searchbox">
                                    <small><i className="fa fa-align-justify icon-align fa-2x mt-0"></i></small>
                                    <div className="col-sm-11">
                                        {selectedcategory}
                                        <input placeholder="type here to search"
                                            className="form-control form-control1 search" type="text" onChange={this.handlechange} value={this.props.searchstring} id="searchfield" />
                                    </div>

                                </div>
                                <div className="search-dropdown">
                                    <div className="searchbox">
                                        <p className="mb-1 font-weight-bold">Category</p>
                                        {category}

                                    </div>

                                    <div className="searchbox">
                                        <p className="mb-1 font-weight-bold">Sub Category</p>
                                        {Subcategory}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="d-inline">
                        <div className="content-area">
                            <div className="page-navigation text-center">
                                {/* <div className="tab-content bg-white rounded shadow-sm pt-3 m-10 p-10">
                            <input placeholder="type here to search" className="form-control search" type="text" onChange={this.handlechange} />
                        </div> */}
                            </div>
                            <div className="nav-tab-content bg-white rounded shadow-sm pt-3 container place-listing-edit">
                                <div className="row">
                                    {Events}
                                </div>
                                <div className="pt-2 pb-3 text-center">

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
export default connect(
    (state: IApplicationState) => state.events,
    EventListingStore.actionCreators
)(EventListing);
