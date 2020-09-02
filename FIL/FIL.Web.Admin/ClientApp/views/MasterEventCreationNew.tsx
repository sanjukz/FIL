import { autobind } from "core-decorators";
import * as React from "react";
import { connect } from "react-redux";
import { Link, RouteComponentProps, Route, withRouter } from "react-router-dom";
import { IApplicationState } from "../stores";
import { actionCreators as sessionActionCreators, ISessionProps } from "shared/stores/Session";
import { bindActionCreators } from "redux";
import EventCreation from "./EventCreation";
import axios from "axios";

//Event
import * as EventCreationStore from "../stores/EventCreation";
import { EventCreationViewModel } from "../models/EventCreation/EventCreationViewModel";
import { EventCreationResponseViewModel } from "../models/EventCreation/EventCreationResponseViewModel";

//EventDetail
import { AddEventDetailViewModel } from "../models/EventCreation/AddEventDetailViewModel";
import { EventDetailResposeViewModel } from "../models/EventCreation/EventDetailResposeViewModel";
import * as EventDetailCreation from "../stores/EventDetailCreation";
import GetsubeventViewModel from "../models/EventCreation/GetsubeventViewModel";
import SubEventDetailDataResponseViewModel from "../models/EventCreation/SubEventDetailDataResponseViewModel";
import { SubEventDeleteViewModel } from "../models/EventCreation/SubEventDeleteViewModel";
import { DeleteSubeventResponseViewModel } from "../models/EventCreation/DeleteSubeventResponseViewModel";
//EventticketDetail
import * as EventTicketDetailCreation from "../stores/EventTicketDetailCreation";
import AddEventTicketDetails from "../components/EventCreation/AddEventTicketDetails";
import SaveEventTicketDetailDataViewModel from "../models/EventCreation/SaveEventTicketDetailDataViewModel";
import EventTicketdetailResponseDataViewModel from "../models/EventCreation/EventTicketdetailResponseDataViewModel";
import GetTicketDetailViewModel from "../models/eventCreation/GetTicketDetailViewModel";
import { GetTicketDetailResponseViewModel } from "../models/EventCreation/GetTicketDetailResponseViewModel";
import PlaceCalendarNew from "../views/PlaceCalendarNew";
import PlaceFinancials from "../views/PlaceFinancials";
import { ToastContainer, toast } from 'react-toastify';
import { gets3BaseUrl } from "../utils/imageCdn";
import 'react-toastify/dist/ReactToastify.css';

type EventCreationProps = EventCreationStore.IEventprops
    & typeof EventCreationStore.actionCreators
    & EventDetailCreation.IEventDetailprops
    & typeof EventDetailCreation.actionCreators
    & EventTicketDetailCreation.IEventTicketDetailprops
    & typeof EventTicketDetailCreation.actionCreators
    & ISessionProps
    & typeof sessionActionCreators
    & RouteComponentProps<{}>

class MasterEventCreationNew extends React.Component<EventCreationProps, any>{
    inputElement: any;
    constructor(props) {
        super(props);
        this.state = {
            addedAmenityId: '',
            isShowEventcreation: true,
            isShowEventdetailcreation: false,
            isShowEventTicketdetailcreation: false,
            eventId: null,
            eventDetailId: null,
            valEvent: "",
            valTicketcategory: "",
            valCurrency: "",
            showDetailList: false,
            isEventCreated: false,
            showcomponent: true,
            Isaddmore: false,
            IsShowcomponent: true,
            selectEvent: "",
            selectEventdetailId: "",
            EventStartDate: "",
            EventEndDate: "",
            isInventoryShow: false,
            finance: false,
            isLoading: false,
            isEdit: false,
            isAlertCall: false
        }
    }

    @autobind
    public goTo() {
        if (window.localStorage != undefined) {
            localStorage.setItem('isInventorySaved', 'false');
            this.setState({ isLoading: false, isInventoryShow: false, finance: true });
        }
    }

    public componentDidMount() {
        this.props.session.masterEventCreationContext = this.goTo;
        this.props.requestEventCategories();
        this.props.requestAmenities();

        localStorage.removeItem("isShow");
        let urlparts = window.location.href.split("/");
        var editId = urlparts[urlparts.length - 1];
        var isEdit = false;
        var edit = urlparts[urlparts.length - 2];
        if (edit == "edit") {
            isEdit = true;
        }
        this.setState({ isEdit: isEdit });
        if (isEdit) {
            this.props.getEventSavedData(editId, (item) => { });
        }
    }

    @autobind
    notify() {
        toast.success('General tab saved successfully', {
            position: "top-center",
            autoClose: 6000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true
        });
        this.setState({ isCreated: true });
    }

    @autobind
    public showDetail() {
        this.setState({
            ShowEventcreation: true,
            isShowEventdetailcreation: true,
            isShowEventTicketdetailcreation: false,
        });
    }

    @autobind
    public showTicketDetail() {
        this.setState({
            ShowEventcreation: true,
            isShowEventdetailcreation: true,
            isShowEventTicketdetailcreation: true,
        });

    }

    @autobind
    public showDetails() {
        this.setState({
            showcomponent: false,
            showDetailList: true,
        })
    }

    @autobind
    public showSubEventForm() {
        this.setState({
            showcomponent: true,
            showDetailList: false,
        })
    }

    @autobind
    public showMoreEventTicketItem() {
        this.setState({
            Isaddmore: true,
            IsShowcomponent: false,
        });
    }

    @autobind
    public getComponent() {
        this.setState({
            IsShowcomponent: true,
            Isaddmore: false,
        });
    }

    @autobind
    public changeVal(e) {
        this.setState({ valEvent: e });
    }

    @autobind
    public changeValTicketCatgory(e) {
        this.setState({ valTicketcategory: e });
    }

    @autobind
    public changecurrencyVal(e) {
        this.setState({ valCurrency: e });
    }

    @autobind
    public selectEventId(e) {
        this.setState({ selectEvent: e });
    }

    @autobind
    public selectEventDetail(e) {
        this.setState({ selectEventdetailId: e });
    }

    @autobind
    public eventCreationSuccess(e) {
        this.setState({ isEventCreated: true });
    }

    @autobind
    public OnsubmitResetEventDetail(values: AddEventDetailViewModel) {
        values.name = "";
        values.metaDetails = "";
        values.startDateTime = "";
        values.endDateTime = "";
    }

    @autobind
    public OnChangeStartDatetime(e) {
        this.setState({ EventStartDate: e })
    }

    @autobind
    public OnChangeEndtime(e) {
        this.setState({ EventEndDate: e })
    }

    @autobind
    handleUploadFile(event) {
        var formData = new FormData();
        formData.append('file', event.target.files[0]);
        this.setState({ isLoading: true });
        axios.post("/api/upload/profilepicture", formData, {
            headers: {
                "Content-Type": "application/json"
            }
        }).then((response) => {
            if (response.data == true) {
                //this.uploadprofileShow(response.data);
            }
        });
    }

    @autobind
    handleUploddProfilePicture() {
        this.inputElement.click();
    }


    handleChange() {

    }

    @autobind
    public onInventoryClick() {
        localStorage.setItem("isShow", "true");
        this.setState({ isInventoryShow: true, finance: false });
    }

    @autobind
    public onGeneralClick() {
        this.setState({ isInventoryShow: false, finance: false });
        localStorage.removeItem("isShow");
    }

    @autobind
    public onFinanceClick() {
        this.setState({ isInventoryShow: false, finance: true });
        localStorage.removeItem("isShow");
    }

    public render() {
        var options = [{}, {}];
        var generalClassName = "nav-item nav-link";
        var inventoryClassName = "nav-item nav-link ";
        var financeClassName = "nav-item nav-link ";

        var generalTabContentClassName = "tab-pane fade d-none";
        var inventoryTabContentClassName = "tab-pane fade d-none";
        var financeTabContentClassName = "tab-pane fade d-none";

        if (this.props.eventCreation.isShowSuccessAlert == true && !this.state.isAlertCall) {
            if (this.props.eventCreation.isAlreadyExists) {
                alert("Place already exists with same name. Please edit the existed place or create new place with different name. Thanks!");
            } else {
                this.notify();
                this.setState({ isAlertCall: true });
            }
        }

        var displayClass = "none";
        if (!this.state.isInventoryShow && !this.state.finance) {
            generalClassName = "nav-item nav-link active";
            generalTabContentClassName = "tab-pane fade show active";
        } else if (this.state.isInventoryShow && !this.state.finance) {
            inventoryClassName = "nav-item nav-link active";
            inventoryTabContentClassName = "tab-pane fade show active";
            displayClass = "show";
        } else if (this.state.finance && !this.state.isInventoryShow) {
            financeClassName = "nav-item nav-link active";
            financeTabContentClassName = "tab-pane fade show active";
        }

        return (
            <div className="card border-0 right-cntent-area pb-5 bg-light">
                <div className="card-body bg-light p-0">
                    <div>
                        {(this.props.eventCreation.isShowSuccessAlert) && <ToastContainer
                            position="top-center"
                            autoClose={6000}
                            hideProgressBar={false}
                            newestOnTop={false}
                            closeOnClick
                            rtl={false}
                            pauseOnFocusLoss
                            draggable
                            pauseOnHover
                        />}
                        {(this.state.isEdit && this.props.eventCreation.savedData != undefined) && <div className="row position-relative">
                            <div className="w-100">
                                <img className="w-100"
                                    src={`${gets3BaseUrl()}/places/InnerBanner/` + this.props.eventCreation.savedData.altId.toUpperCase() + ".jpg"}
                                    onError={(e) => {
                                        e.currentTarget.src = `${gets3BaseUrl()}/places/about/learn-more-banner-placeholder.jpg`
                                    }}
                                />
                            </div>
                        </div>}
                        <nav className="pb-4">
                            <div className="nav justify-content-center text-uppercase fee-tab" id="nav-tab" role="tablist">
                                <a onClick={this.onGeneralClick} className={generalClassName} id="nav-Details-tab" data-toggle="tab" href="#nav-Details" role="tab" aria-controls="nav-Details">GENERAL</a>
                                <a onClick={this.onInventoryClick} className={inventoryClassName} id="nav-Inventory-tab" data-toggle="tab" href="#nav-Inventory" role="tab" aria-controls="nav-Inventory">INVENTORY</a>
                                <a onClick={this.onFinanceClick} className={financeClassName} id="nav-contact-tab" data-toggle="tab" href="#nav-contact" role="tab" aria-controls="nav-contact">FINANCIAL</a>
                            </div>
                        </nav>

                        <div className="tab-content bg-white rounded shadow-sm pt-3" id="nav-tabContent">
                            <div
                                className={generalTabContentClassName}
                                id="nav-Details"
                                role="tabpanel"
                                aria-labelledby="nav-Details-tab"
                            >
                                <EventCreation
                                    addedAmenityId={this.state.addedAmenityId}
                                    requestEventCategories={this.props.requestEventCategories}
                                    onSubmitEventCreation={this.onSubmitEventCreation.bind(this)}
                                    fetchEventCategoriesSuccess={this.props.eventCreation.fetchEventCategoriesSuccess}
                                    eventCategories={this.props.eventCreation.eventCategoriesList}
                                    eventDataFetchSuccess={this.props.eventCreation.eventDataFetchSuccess}
                                    error={this.props.eventCreation.error}
                                    EventSaveSuccessful={this.props.eventCreation.EventSaveSuccessful}
                                    errorMessage={this.props.eventCreation.errorMessage}
                                    requestAmenities={this.props.requestAmenities}
                                    amenities={this.props.eventCreation.amenityList}
                                    createPlace={this.saveEventData}
                                    saveAmenity={this.saveAmenityData}
                                    EventSaveFailure={this.props.eventCreation.EventSaveFailure}
                                    requestSavedDatafromEventId={this.props.getEventSavedData}
                                    savedeventdata={this.props.eventCreation.savedData}
                                    allStoreState={this.props.eventCreation}
                                    isShow={(!this.state.finance && !this.state.isInventoryShow) ? true : false}
                                />
                            </div>
                            <div className={inventoryTabContentClassName} id="nav-Inventory" role="tabpanel" aria-labelledby="nav-Inventory-tab">
                                <PlaceCalendarNew />
                                {/*<PlaceFinancials />  */}
                            </div>
                            <div className={financeTabContentClassName} id="nav-contact" role="tabpanel" aria-labelledby="nav-contact-tab">
                                {(this.state.finance) && <PlaceFinancials />}
                            </div>
                        </div>
                    </div >
                </div>
            </div>)
    }

    @autobind
    private saveEventData(values) {
        values.userAltId = this.props.session.user.altId;
        this.setState({ isLoading: true });
        if (this.state.isEdit) {
            values.isEdit = true;
        } else {
            values.isEdit = false;
        }
        this.props.saveEvent(values, (response: EventCreationResponseViewModel) => {
            if (response.success == true) {
                if (!this.state.isEdit) {
                    this.setState({ isLoading: false, isInventoryShow: true, finance: false });
                }
            } else {
                alert("Error with status code 404");
            }
        });
    }

    isOpenForEdit() {
        let urlparts = window.location.href.split('/');
        var edit = urlparts[urlparts.length - 2];
        if (edit == "edit")
            return true;
        else return false;
    }
    @autobind
    setStateFromCalendarChild() {
        this.setState({ isLoading: false, isInventoryShow: false, finance: true });
    }


    @autobind
    private saveAmenityData(values) {
        this.setState({ isLoading: true });
        this.props.saveAmenities(values, (response: any) => {
            alert("Amenity saved successfully!");
            if (!this.isOpenForEdit())//Navigation for create case
            {
                localStorage.setItem('isInventory', 'true');
                this.setState({ isLoading: false, isInventoryShow: false, finance: true });
            }
            this.setState({ addedAmenityId: response.id, isLoading: false, isInventoryShow: false, finance: false });

        });
    }

    @autobind
    private onSubmitEventCreation(values: EventCreationViewModel) {
        values.userAltId = this.props.session.user.altId;
        this.props.saveEvent(values, (response: EventCreationResponseViewModel) => {
            if (response.success == true) {
                localStorage.setItem('eventIdToken', response.altId);
                /* var eventId: GeteventViewModel = {
                userId: values.userAltId.toUpperCase(),
                 };
                 // this.props.requestEventCategories();

                 this.showDetail();
                 setTimeout(() => {
                this.props.geEventsData(eventId, (response: GetEventsResponseViewModel) => {
                });
            }, 3000);*/

            } else {
            }
        });
    }

    @autobind
    private onSubmitEventDetails(values: AddEventDetailViewModel) {
        values.eventId = this.state.selectEvent.id,
            values.venueAltId = this.state.valEvent.id,
            values.startDateTime = this.state.EventStartDate,
            values.endDateTime = this.state.EventEndDate
        this.props.saveEventDetail(values, (response: EventDetailResposeViewModel) => {
            if (response.success == true) {
                var id = response.id
                var eventId: GetsubeventViewModel = {
                    eventId: this.state.selectEvent.id
                };
                this.props.requestVenues();
                this.showTicketDetail();
                this.showDetails();
                setTimeout(() => {
                    this.props.getSubeventData(eventId, (response: SubEventDetailDataResponseViewModel) => {
                    });

                }, 3000);


            } else {
            }
        });
    }

    @autobind
    private onSubmitDeleteSubEventDetail(values: SubEventDeleteViewModel) {
        this.props.deleteSubeventAction(values, (response: DeleteSubeventResponseViewModel) => {
            if (response.success) {
                //var eventId: GetsubeventViewModel = {
                //    eventAltId: this.props.match.params.eventId,
                //};
                setTimeout(() => {
                    //this.props.getSubeventData(eventId, (response: SubEventDetailDataResponseViewModel) => {
                    //});
                }, 3000);
            } else {
            }
        });
    }

    @autobind
    private onSubmitEventTicketDetails(values: SaveEventTicketDetailDataViewModel) {
        values.eventDetailId = this.state.selectEventdetailId.id;
        values.ticketCategoryId = this.state.valTicketcategory.id;
        values.remainingTicketForSale = values.availableTicketForSale;
        values.currencyId = this.state.valCurrency.id;
        values.channelId = "Website";
        values.valueTypeId = 2;//dynamic rendering remaining
        values.salesStartDateTime = "2018-08-13 09:30:00.000";
        values.salesEndDatetime = "2018-08-13 09:30:00.000";
        this.props.saveEventTicketDetail(values, (response: EventTicketdetailResponseDataViewModel) => {
            if (response.success == true) {
                alert('Event Created Sucessfully');
                this.eventCreationSuccess(this);
                this.props.requesttickets();
                this.props.requestTicketType();
                this.props.requestChannelType();
                this.props.requestValueType();
                this.props.requestCurrencyType();
                this.showMoreEventTicketItem();
                var eventDetailId: GetTicketDetailViewModel = {
                    eventDetailId: this.state.selectEventdetailId.id,
                };
                setTimeout(() => {
                    this.props.geEventTicketDetailData(eventDetailId, (response: GetTicketDetailResponseViewModel) => {
                    });
                }, 3000);

            } else {
            }
        });
    }
}

export default connect(
    (state: IApplicationState) => ({ session: state.session, eventCreation: state.EventCreation, eventDetailCreation: state.EventDetailCreation, eventTicketDetailCreation: state.EventTicketDetailCreation }),
    (dispatch) => bindActionCreators({ ...sessionActionCreators, ...EventCreationStore.actionCreators, ...EventDetailCreation.actionCreators, ...EventTicketDetailCreation.actionCreators }, dispatch)
)(MasterEventCreationNew);
