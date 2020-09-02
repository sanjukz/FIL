import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../stores";
import * as EventCategoryStore from "../../stores/EventCategory";
import { Formik, Field, Form } from "formik";
import TableComponent from "../../components/TableComponent";
import Yup from "yup";
import { EventCategoryMappingViewModel } from "../../models/EventCategoryMappingViewModel";
import "./EventManager.scss";

type EventCategoryComponentStateProps = EventCategoryStore.IEventCategoryComponentState & typeof EventCategoryStore.actionCreators;

interface Values{
    eventid: number;
    parentcategoryid: number;
    subcategoryid: number;
    isenabled: boolean;
    eventCategoryMapId: number;
}


export class EventCategoryMapping extends React.Component<EventCategoryComponentStateProps, EventCategoryStore.IEventCategoryComponentState> {
    constructor(props) {
        super(props);
        this.setState({ eventid: 0, parentcategoryid: 0, subcategoryid: 0, isenabled: false, eventCategoryMapId:0});
        this.handleChange = this.handleChange.bind(this);
        this.handleChangeSub = this.handleChangeSub.bind(this);
        this.handleChangeEnable = this.handleChangeEnable.bind(this);
    }
    public initialValues: any;
    public schema : any;
    public selectedplace: any;
    public editscreen: boolean = false;
    public createscreen: boolean = false;
    public componentWillMount() {
        setTimeout(() => {
            let urlparts = window.location.href.split('/');
            let id = urlparts[urlparts.length-1];
            this.props.requestEventCategoryMappingdata(id);
            this.props.requestEventdata();
        }, 1000);
    }

    public componentDidMount()
    {
        this.selectedplace = JSON.parse(localStorage.getItem("selectedplace"));
    }

    handleChange(event)
    {
        this.setState({parentcategoryid: event.target.value});
    }
    handleChangeSub(event)
    {
        this.setState({subcategoryid: event.target.value});
    }
    handleChangeEnable(event)
    {
        this.setState({isenabled: event.target.checked});
    }
    updateMapping(row){
        // this.props.updateEventCategoryMappingState(row);
        this.editscreen = true;
        this.createscreen = false;
        // this.props.eventCategoryMapId = row.original.id;

        this.setState({
            eventid: row.original.eventId, eventCategoryMapId: row.original.id,
            subcategoryid: row.original.eventCategoryId, isenabled: row.original.isEnabled
        });
    }

    createItem()
    {
        this.editscreen = true;
        this.createscreen = true;
        this.setState({isenabled:false, subcategoryid:0,eventCategoryMapId:0,parentcategoryid:0});
    }

    public render() {

        const columns = [
            {
            Header: "Category",
            accessor: "categoryName"
        },
        {
            Header: "Place Name",
            accessor: "eventName"
        },
         {
            Header: "Is Enabled",
            accessor: "isEnabled",
            Cell: row=>  (<i className={ row.value? "fa fa-check":"fa fa-ban"}
                style={{
                  color: row.value  ? '#85cc00'
                    : '#ff2e00'
                }}
              >
            </i> ),
            maxWidth:100,
            filterable: false
        }, {
            Header: "Action",
            accessor: "id",
            Cell: row => (<span style={{cursor:"pointer",color:"blue"}} onClick={()=>this.updateMapping(row)} > <i className="fa fa-edit" aria-hidden="true"></i> Edit </span>),
            filterable: false
        }
    ];

        const categories = this.props.eventcategories.categories;
        //not working this way due to some bug so using localstorage as a workaround
        //const data = this.props.eventcategorymappings.eventcatmappings;
        let data;
        if (typeof window !== 'undefined') {
            data  = JSON.parse(localStorage.getItem("datamap"));
        }
        //this.props.eventcategorymappings.eventcatmappings
        if (!categories) return <div></div>;
        const parentCategoriesDropdown = categories.filter(p => p.categoryId == 0).map((item) => {
            return <option value={item.value} >{item.displayName}</option>
        });
        const subCategoriesDropdown = categories.filter(p => p.categoryId > 0).map((item) => {
            return <option value={item.value} >{item.displayName}</option>
        });
        let selectedid = this.selectedplace? this.selectedplace.id : 0;
        if(this.selectedplace && categories && categories.length>0)
        {
            for (let counter =0;counter< data.eventcatmapping.length; counter++)
            {
                data.eventcatmapping[counter].eventName = this.selectedplace.name;
                let eventcatid = data.eventcatmapping[counter].eventCategoryId;
                let ctgry = categories.find(p=>p.value==eventcatid);
                if(ctgry)
                {
                    data.eventcatmapping[counter].categoryName = ctgry.displayName
                }
            }
        }

        const schema = this.getSchema();

        if(this.state){
            this.initialValues = { eventid:selectedid, parentcategoryid:0,subcategoryid:this.state.subcategoryid, isenabled:this.state.isenabled, eventCategoryMapId: this.state.eventCategoryMapId}
        }
      return (
        <div className="card border-0 right-cntent-area pb-5 bg-light">
            <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box dashboard-list">
                <div className="content-area">
                    <div className="page-navigation text-center">
                        <div className="tab-content bg-white rounded shadow-sm pt-3">
                            <ul className="bg-light tab-pane fade show active p-3 m-0">
                            </ul>
                        </div>
                    </div>
                    <div className="nav-tab-content bg-white rounded shadow-sm pt-3 mt-2 container">

                    <div className="row">
                      <div className="col-sm-9"><h4> Manage Place and Site mapping </h4></div>
                        <div className="col-sm-3 text-right">
                          <button onClick={() => this.createItem()} className="btn btn-primary">Create</button>
                        </div>
                    </div>

                        <div className="table table-striped table-bordered example-table">
                         {data?<TableComponent myTableData={ data.eventcatmapping } myTableColumns={columns} />:null}
                        </div>
                        {this.editscreen?
                        <Formik
                            initialValues={this.initialValues || {}}
                            validationSchema={schema}
                            onSubmit={(values: Values) => {
                                setTimeout(() => {
                                    let ui: EventCategoryMappingViewModel = new EventCategoryMappingViewModel();
                                    ui.eventid = this.selectedplace.id;
                                    ui.parentcategoryid = this.state.parentcategoryid;
                                    ui.subcategoryid = this.state.subcategoryid;
                                    ui.isenabled = this.state.isenabled;
                                    ui.id = this.state.eventCategoryMapId;

                                    this.props.updateEventCategory(ui, () => {
                                        this.setState({
                                            subcategoryid: 0, isenabled: false, parentcategoryid: 0
                                        });
                                    });
                                }, 400);
                            }}
                            render={({ errors, touched, isSubmitting }) => (
                                <Form>
                                    <div className="row">
                                        {this.createscreen? <h4>Create category for selected place</h4>: null }
                                        {!this.createscreen? <h4>Update category for selected place</h4>: null }
                                    </div>
                                    <div className="row"> </div>
                                    <div className="row">
                                        <div className="col-md-2">
                                            <label>Selected place</label>
                                        </div>
                                        <div className="col-md-3">
                                        { this.selectedplace?
                                            <select value={this.initialValues.eventid} name="eventid" className="form-control" disabled >
                                                <option value={this.selectedplace.id} selected>{this.selectedplace.name}</option>
                                            </select>:null}
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-md-2">
                                            <label>Parent Category</label>
                                        </div>
                                        <div className="col-md-3">

                                            <select onChange={this.handleChange} name="parentcategoryid" className="form-control" >
                                                <option>Select category</option>
                                                {parentCategoriesDropdown}
                                            </select>
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-md-2">
                                            <label>Sub Category</label>
                                        </div>
                                        <div className="col-md-3">
                                            <select value={this.initialValues.subcategoryid} onChange={this.handleChangeSub} required name="subcategoryid" className="form-control" >
                                                <option>Select sub category</option>
                                                {subCategoriesDropdown}
                                            </select>
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-md-2">
                                            <label>Enable</label>
                                        </div>
                                        <div className="col-md-3">
                                        <Field checked={this.initialValues.isenabled} style={{transform: "scale(1.5)"}} onChange={this.handleChangeEnable} type="checkbox"  name="isenabled" />
                                        </div>
                                    </div>
                                    <div className="row">
                                        <div className="col-md-3 col-md-offset-2">
                                            <button type="submit" className="btn btn-primary pull-right">Save</button>
                                        </div>
                                    </div>

                                    { this.props.isSuccess? <div className="alert alert-success p-10 mt-10 text-left"><small>{this.props.alertMessage}</small></div>: null }
                                    <span style={{display:"none"}} >
                                    { this.props.isSuccess? setTimeout(() => {
                                        this.props.requestEventCategoryMappingdata(this.initialValues.eventid)
                                    }, 1000):null}</span>
                                    { this.props.isFailure? <div className="alert alert-danger p-10 mt-10 text-left"><small>{this.props.alertMessage}</small></div>: null }
                                </Form>
                            )}
                        />
                        :null}
                    </div>
                </div>
          </div>
        </div>);
    }
    private getSchema() {
        return Yup.object().shape({
            eventid: Yup.string(),
            parentcategoryid: Yup.string(),
            subcategoryid: Yup.string().required("This value is required"),
            isenabled: Yup.boolean()
        });
    }
}
export default connect(
    (state: IApplicationState) => state.eventcategory,
    EventCategoryStore.actionCreators
)(EventCategoryMapping);
