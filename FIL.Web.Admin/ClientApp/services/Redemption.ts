import axios from "axios";
import { GuideResponseModel } from "../models/Redemption/GuideViewModel";

function SaveGuide(guide) {
    return axios.post("api/Guide/Add", guide, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<GuideResponseModel>)
        .catch((error) => {
            alert(error);
        });
}

export const RedemptionService = {
    SaveGuide
};