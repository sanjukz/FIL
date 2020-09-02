import axios from "axios";
import { UserInviteRequestViewModel } from "../models/Invite/UserInviteRequestViewModel";
import { UserInviteResponseViewModel } from "../models/Invite/UserInviteResponseViewModel";

function sendUserInvite(email) {
    return axios.post("api/invite/sendinvite", email, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<UserInviteResponseViewModel>)
        .catch((error) => {
        });
}

function getSiteInviteConfig(setname)
{
    
    return axios.post("api/invite/getconfig", setname, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => 
        {
            response.data as Promise<UserInviteResponseViewModel>
        })
        .catch((error) => {
        });
}

export const inviteService = {
    
    sendUserInvite,
    getSiteInviteConfig
};
