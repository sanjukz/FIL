import axios from "axios";
import { CreateEventFormResponseViewModel } from "../models/CreateEventFormResponseViewModel";

function eventcreation(event) {
    return axios.post("", event, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<CreateEventFormResponseViewModel>)
        .catch((error) => {
        });
}

