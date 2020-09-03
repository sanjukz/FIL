import { autobind } from "core-decorators";
import * as React from "react";
import AddEvent from "../components/EventCreation/AddEvent";

export default class EventCreation extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            categories: [],
            isPlaceholder: false,
            isShowComponent: true,
            isShowDetail: false,
            valEventCategories: "",
        }
    }

    public componentDidMount() {

    }

    @autobind
    public showDetail(e) {
        this.setState({
            isShowComponent: false,
            isShowDetail: true,
        })
    }

    public showComponent() {
        this.setState({
            isShowComponent: true,
            isShowDetail: false,
        })
    }

    public showSubEventForm() {
        this.setState({
            isShowComponent: true,
            isShowDetail: false,
        })
    }

    @autobind
    public changeVal(e) {
        this.setState({ valEventCategories: e });
    }

    public render() {
        var categories = this.state.categories;
        var categoriesList = [];

        // return <AddEvent RequestSavedDataFromEventId = {this.props.requestSavedDatafromEventId} 
        //     SavedEventData = { this.props.savedeventdata}
        //     EventSaveFailure = {this.props.EventSaveFailure} 
        //     EventSaveSuccessful = {this.props.EventSaveSuccessful} 
        //     createPlace = {this.props.createPlace} 
        //     saveAmenity = {this.props.saveAmenity} 
        //     amenities= {this.props.amenities} 
        //     eventCat={categoriesav} 
        //     requestAmenities={this.props.requestAmenities} 
        //     onSubmit={this.props.onSubmitEventCreation} 
        //     name={" "} description={" "} metaDetails={" "} termsAndConditions={" "} />;

        if (this.props.fetchEventCategoriesSuccess && !this.props.eventCategories) {
            return <AddEvent RequestSavedDataFromEventId={this.props.requestSavedDatafromEventId}
                SavedEventData={this.props.savedeventdata}
                EventSaveFailure={this.props.EventSaveFailure}
                EventSaveSuccessful={this.props.EventSaveSuccessful}
                createPlace={this.props.createPlace}
                saveAmenity={this.props.saveAmenity}
                addedAmenityId={this.props.addedAmenityId}
                amenities={this.props.amenities}
                eventCat={categoriesList}
                requestAmenities={this.props.requestAmenities}
                allStoreState={this.props.allStoreState}
                onSubmit={this.props.onSubmitEventCreation}
                name={" "} description={" "} metaDetails={" "} termsAndConditions={" "} />;
        }
        if (this.props.fetchEventCategoriesSuccess && this.props.eventCategories) {
            return <AddEvent RequestSavedDataFromEventId={this.props.requestSavedDatafromEventId}
                SavedEventData={this.props.savedeventdata}
                EventSaveFailure={this.props.EventSaveFailure}
                EventSaveSuccessful={this.props.EventSaveSuccessful}
                addedAmenityId={this.props.addedAmenityId}
                createPlace={this.props.createPlace}
                saveAmenity={this.props.saveAmenity}
                amenities={this.props.amenities}
                eventCat={this.props.eventCategories.categories}
                allStoreState={this.props.allStoreState}
                requestAmenities={this.props.requestAmenities}
                onSubmit={this.props.onSubmitEventCreation}
                name={" "} description={" "} metaDetails={" "} termsAndConditions={" "} />;
        }
        else {
            return <span></span>;
        }

        //  if (this.props.fetchEventCategoriesSuccess && this.props.fetchEventTypeSuccess) {
        //     categories = this.props.eventCategories.map((item) => {
        //         return <option >{item}</option>
        //     })
        //     eventType = this.props.eventType.map((item) => {
        //         return <option >{item}</option>
        //     });

        //     var that = this;
        //     var eventLenght = 0;
        //     if (this.props.eventDataFetchSuccess) {
        //         const events = this.props.event;
        //         if (this.props.event != undefined) {
        //             eventLenght = this.props.event.length;
        //         }
        //         // var EventList = events.map(function (val) {
        //         //     
        //         //     return <div className="form-group mb-2 position-relative">
        //         //         <div className="form-control form-control-sm readonly-input">{val.name} </div>
        //         //         <div className="form-group collapse myaccount-error" id={"collapseName" + val.id}>
        //         //             <AddEvent eventCat={categories} eventTypes={eventType} onSubmit={that.props.onSubmitEventCreation} name={val.name} description={val.description} metaDetails={val.metaDetails} termsAndConditions={val.metaDetails} />
        //         //         </div>
        //         //         <div className="float-right">
        //         //             <a className="form-icon form-icon-edit form-icon-sm d-none" role="button" data-toggle="collapse" data-target={"#collapseName" + val.id} aria-expanded="false" aria-controls="collapseExample">
        //         //                 <i className="fa fa-pencil" aria-hidden="true"></i></a>
        //         //         </div>
        //         //     </div>
        //         // });
        //     }

        //     return <div>
        //         <h4 className="mt-0 mb-3">Create Place</h4>

        //         {eventLenght == 0 &&
        //             <AddEvent eventCat={categories} eventTypes={eventType} onSubmit={this.props.onSubmitEventCreation} name={" "} description={" "} metaDetails={" "} termsAndConditions={" "} />
        //         }
        //         {this.props.error && <span className="validate-txt"><br /><p>{this.props.errorMessage}</p></span>}
        //         {this.props.EventSaveSuccessful && <span className="validate-txt"><p style={{ color: "#5bac30" }}>{this.props.alertMessage.body}</p></span>}

        //         {eventLenght > 0 && <div className="user-address">
        //             <div className="mb-3">
        //                 {EventList}
        //                 <hr />
        //             </div>
        //         </div>
        //         } 
        //     </div>;
        // }
        // else {
        //     return (
        //         <div><FILLoader /></div>
        //     );
        // }
    }
}
