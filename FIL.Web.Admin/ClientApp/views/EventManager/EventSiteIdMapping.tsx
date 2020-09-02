import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../stores";
import * as EventSiteIdStore from "../../stores/EventSiteId";
import { Formik, Field, Form } from "formik";
import TableComponent from "../../components/TableComponent";
import Yup from "yup";
import { EventSiteIdMappingViewModel } from "../../models/EventSiteIdMappingViewModel";
import "./EventManager.scss";
type EventSiteIdComponentStateProps = EventSiteIdStore.IEventSiteIdComponentState & typeof EventSiteIdStore.actionCreators;

interface Values {
    id: number;
    eventId: number;
    sortOrder: number;
    isEnabled: boolean;
    siteId: number;
}

export class EventSiteIdMapping extends React.Component<EventSiteIdComponentStateProps, EventSiteIdStore.IEventSiteIdComponentState> {
    constructor(props) {
        super(props);
        this.setState({ eventid: 0, sortorder: 0, siteid: 0, isenabled: false });
        this.handleChange = this.handleChange.bind(this);
        this.handleChangeSortOrder = this.handleChangeSortOrder.bind(this);
        this.handleChangeEnable = this.handleChangeEnable.bind(this);
    }
    public initialValues : any;
    public schema : any;
    public selectedplace: any;
    public editscreen: boolean = false;
    public createscreen: boolean = false;
    public componentWillMount() {

        setTimeout(() => {
            let urlparts = window.location.href.split('/');
            let id = urlparts[urlparts.length - 1];
            this.props.requestEventSiteIdMappingdata(id);

        }, 1000);

            this.props.requestSiteData();
    }

    public componentDidMount() {
        this.selectedplace = JSON.parse(localStorage.getItem("selectedplace"));
    }

    handleChange(event) {
        this.setState({ siteid: event.target.value });
    }

    handleChangeEnable(event) {
        this.setState({ isenabled: event.target.checked });
    }
    handleChangeSortOrder(event)
    {
        this.setState({sortorder: event.target.value});
    }
    updateMapping(row) {

        this.editscreen = true;
        this.createscreen = false;
        this.setState({
            eventid: row.original.eventId, siteidmapid: row.original.id,
            sortorder: row.original.sortOrder, isenabled: row.original.isEnabled,
            siteid: row.original.siteId
        });

    }

    createItem() {
        this.editscreen = true;
        this.createscreen = true;
        this.setState({ isenabled: false, siteid: 0, sortorder: 0, siteidmapid: 0 });
    }

    public render() {

        const columns = [
            {
                Header: "Site Name",
                accessor: "siteName"
            },
            {
                Header: "Place Name",
                accessor: "eventName"
            },
            {
                Header: "Sort Order",
                accessor: "sortOrder"
            },
            {
                Header: "Is Enabled",
                accessor: "isEnabled",
                Cell: row => (<i className={row.value ? "fa fa-check" : "fa fa-ban"}
                    style={{
                        color: row.value ? '#85cc00'
                            : '#ff2e00'
                    }}
                >
                </i>),
                maxWidth: 100,
                filterable: false
            },
            {
                Header: "Action",
                accessor: "id",
                Cell: row => (<span style={{ cursor: "pointer", color: "blue" }} onClick={() => this.updateMapping(row)} > <i className="fa fa-edit" aria-hidden="true"></i> Edit </span>),
                filterable: false
            }
        ];

        const sitedata = this.props.sitedata.siteData;
        const data = this.props.eventsiteidmappings.eventsiteidmapping;
        if (!sitedata) return <div></div>;
        const siteDropdown = sitedata.filter(p => p.id > 0).map((item) => {
            return <option value={item.id} >{item.siteName}</option>
        });
        let selectedid = this.selectedplace ? this.selectedplace.id : 0;
        if (this.selectedplace && sitedata && sitedata.length > 0 && data) {
            for (let counter = 0; counter < data.length; counter++) {
                data[counter].eventName = this.selectedplace.name;
                let siteid = data[counter].siteId;
                let site = sitedata.find(p => p.id == siteid);
                if (site) {
                    data[counter].siteName = site.siteName
                }
            }
        }

        const schema = this.getSchema();

        if (this.state) {
            this.initialValues = { eventId: selectedid, siteId: this.state.siteid, id: this.state.siteidmapid, isEnabled: this.state.isenabled, sortOrder: this.state.sortorder }
        }
      return (
        <div className="card border-0 right-cntent-area pb-5 bg-light">
            <div className="d-inline">
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
                            {data ? <TableComponent myTableData={data} myTableColumns={columns} /> : null}
                        </div>
                        {this.editscreen ?
                            <Formik
                                initialValues={this.initialValues || {}}
                                validationSchema={schema}
                                onSubmit={(values: Values) => {
                                    setTimeout(() => {
                                        let ui: EventSiteIdMappingViewModel = new EventSiteIdMappingViewModel();
                                        ui.eventid = this.selectedplace.id;
                                        ui.siteid = this.state.siteid;
                                        ui.sortorder = this.state.sortorder;
                                        ui.isenabled = this.state.isenabled;
                                        ui.id = this.state.siteidmapid;
                                        this.props.updateEventSiteId(ui, () => {
                                            this.setState({
                                                sortorder: 0, isenabled:false,
                                                siteid:0
                                            });
                                        });
                                    }, 400);
                                }}
                                render={({ errors, touched, isSubmitting }) => (
                                    <Form>
                                        <div className="row">
                                            {this.createscreen ? <h4>Create Site and place mapping</h4> : null}
                                            {!this.createscreen ? <h4>Update Site and place mapping</h4> : null}
                                        </div>
                                        <div className="row"> </div>
                                        <div className="row">
                                            <div className="col-md-2">
                                                <label>Selected place</label>
                                            </div>
                                            <div className="col-md-3">
                                                {this.selectedplace ?
                                                    <select value={this.initialValues.eventId} name="eventId" className="form-control" disabled >
                                                        <option value={this.selectedplace.id} selected>{this.selectedplace.name}</option>
                                                    </select> : null}
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-2">
                                                <label>Site</label>
                                            </div>
                                            <div className="col-md-3">

                                                <select value={this.initialValues.siteId} onChange={this.handleChange} name="siteId" className="form-control" >
                                                    <option>Select Site</option>
                                                    {siteDropdown}
                                                </select>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-2">
                                                <label>Sort order</label>
                                            </div>
                                            <div className="col-md-3">
                                                <Field type="text" name="sortOrder" onChange={this.handleChangeSortOrder} value={this.initialValues.sortOrder} />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-2">
                                                <label>Enable</label>
                                            </div>
                                            <div className="col-md-3">
                                                <Field checked={this.initialValues.isEnabled} style={{ transform: "scale(1.5)" }} onChange={this.handleChangeEnable} type="checkbox" name="isEnabled" />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-3 col-md-offset-2">
                                                <button type="submit" className="btn btn-primary pull-right">Save</button>
                                            </div>
                                        </div>

                                        {this.props.isSuccess ? <div className="alert alert-success p-10 mt-10 text-left"><small>{this.props.alertMessage}</small></div> : null}
                                        <span style={{ display: "none" }} >
                                            {this.props.isSuccess ? setTimeout(() => {
                                                this.props.requestEventSiteIdMappingdata(this.initialValues.eventId)
                                            }, 1000) : null}</span>
                                        {this.props.isFailure ? <div className="alert alert-danger p-10 mt-10 text-left"><small>{this.props.alertMessage}</small></div> : null}
                                    </Form>
                                )}
                            />
                            : null}
                    </div>
                </div>
          </div>
            </div>);
    }
    private getSchema() {
        return Yup.object().shape({
            eventId: Yup.string(),
            sortOrder: Yup.string(),
            siteId: Yup.string().required("This value is required"),
            isEnabled: Yup.boolean()
        });
    }
}
export default connect(
    (state: IApplicationState) => state.eventSiteId,
    EventSiteIdStore.actionCreators
)(EventSiteIdMapping);
