import axios from "axios";
import { EventCreationResponseViewModel } from "../models/EventCreation/EventCreationResponseViewModel";
import { EventDetailResposeViewModel } from "../models/EventCreation/EventDetailResposeViewModel";
import SubEventDetailDataResponseViewModel from "../models/EventCreation/SubEventDetailDataResponseViewModel";
import SaveEventDataViewModel from "../models/EventCreation/SaveEventDataViewModel";
import { DeleteSubeventResponseViewModel } from "../models/EventCreation/DeleteSubeventResponseViewModel";
import EventTicketdetailResponseDataViewModel from "../models/EventCreation/EventTicketdetailResponseDataViewModel";
import GetEventsResponseViewModel from "../models/EventCreation/GetEventsResponseViewModel";
import { GetTicketDetailResponseViewModel } from "../models/EventCreation/GetTicketDetailResponseViewModel";


function saveEventData(eventData) {
    return axios.post("api/saveevent/save", eventData, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<EventCreationResponseViewModel>)
        .catch((error) => {
            alert(error);
        });
}

function saveAmenityData(amenity) {
    return axios.post("api/saveamity/save", amenity, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<any>)
        .catch((error) => {
            alert(error);
        });
}

function saveEventDetailData(eventDetailData) {
    if (eventDetailData) {
        return axios.post("api/eventdetail/save", eventDetailData, {
            headers: {
                "Content-Type": "application/json"
            }
        }).then((response) => response.data as Promise<EventDetailResposeViewModel>)
            .catch((error) => {
                alert(error);
            });
    }
}

function getSubEventList(subevent) {
    return axios.post("api/Subeventcategory/all", subevent, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<SubEventDetailDataResponseViewModel>)
        .catch((error) => {
        });
}

function getEventSavedData(id) {
    return axios.get("api/getsavedevent/"+id, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<SaveEventDataViewModel>)
        .catch((error) => {
        });
}

function deleteSubevent(event) {
    return axios.post("api/SubeventcategoryDelete/all", event, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<DeleteSubeventResponseViewModel>)
        .catch((error) => {
        });
}

function saveEventTicketDetailData(eventDetailData) {
    return axios.post("api/EventTicketDetail/save", eventDetailData, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<EventTicketdetailResponseDataViewModel>)
        .catch((error) => {
            alert(error);
        });
}

function getEventsData(event) {
    return axios.post("api/eventsata/all", event, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<GetEventsResponseViewModel>)
        .catch((error) => {
        });
}

function getEventTicketDetailData(ticketDetailData) {
    return axios.post("api/GetAllTicketDetails/all", ticketDetailData, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<GetTicketDetailResponseViewModel>)
        .catch((error) => {
        });
}

export const eventCreationService = {
    saveAmenityData,
    saveEventData,
    saveEventDetailData,
    getSubEventList,
    deleteSubevent,
    saveEventTicketDetailData,
    getEventsData,
    getEventTicketDetailData,
    getEventSavedData,
};
