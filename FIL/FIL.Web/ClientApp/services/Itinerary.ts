import axios from "axios";
import responseModel from "../models/ItinenaryDataResonseModel";
import ItineraryBoardResponseViewModel from "../models/Itinerary/ItineraryBoardResponseViewModel";

function requstItinerary(data) {
    return axios.post("api/v1/getVenueDayByDay/all", data, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<responseModel[][]>)
        .catch((error) => {
            alert(error);
        });
}

function requstItineraryBoard(data) {
    return axios.post("api/v1/ItineraryBoard", data, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<ItineraryBoardResponseViewModel>)
        .catch((error) => {
            alert(error);
        });
}

export const itineraryService = {
    requstItinerary,
    requstItineraryBoard
};
