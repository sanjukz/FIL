import axios from "axios";
import  PlaceCalendarResponseViewModel  from "../models/PlaceCalendar/PlaceCalendarResponseViewModel";

function placeCalendarCreation(place) {
    return axios.post("api/save/calendar", place, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<PlaceCalendarResponseViewModel>)
        .catch((error) => {
        });
}


export const placeCalendarService = {
    placeCalendarCreation
};

