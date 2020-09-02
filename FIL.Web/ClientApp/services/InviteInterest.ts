import axios from "axios";
import { InviteInterestRequestViewModel } from "../models/InviteInterest/InviteInterestRequestViewModel";
import { InviteInterestResponseViewModel } from "../models/InviteInterest/InviteInterestResponseViewModel";

function sendUserInviteInterest(inviteinterest:InviteInterestRequestViewModel) {
    
    return axios.post("api/inviteinterest/save", inviteinterest, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<InviteInterestResponseViewModel>)
        .catch((error) => {
        });
}


export const InviteInterestService = {
    
    sendUserInviteInterest
};
