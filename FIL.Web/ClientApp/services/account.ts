import axios from "axios";
import AddressResponseViewModel from "../models/Account/AddressResponseViewModel";
import CardResponseViewModel from "../models/Account/CardResponseViewModel";
import UserProfileResponseViewModel from "../models/Account/UserProfileResponseViewModel";
import { ChangePasswordResponseViewModel } from "../models/Account/ChangePasswordResponseViewModel";
import { DeleteAddressResponseViewModel } from "../models/Account/DeleteAddressResponseViewModel";
import { DeleteCardResponseViewModel } from "../models/Account/DeleteCardResponseViewModel";
import { SaveAddressResponseViewModel } from "../models/Account/SaveAddressResponseViewModel";
import { SaveCardResponseViewModel } from "../models/Account/SaveCardResponseViewModel";
import { SetDefaultAddressResponseViewModel } from "../models/Account/SetDefaultAddressResponseViewModel";
import { SaveGuestResponseViewModel } from '../models/Account/SaveGuestResponseViewModel'
import { MobileExistModel } from "../models/Account/MobileExistModel";
import { NotificationModel } from "../models/Account/NotificationModel";



function deleteAddress(address) {
    return axios.post("api/useraddress/delete", address, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<DeleteAddressResponseViewModel>)
        .catch((error) => {
        });
}

function deleteCard(card) {
    return axios.post("api/usercard/delete", card, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<DeleteCardResponseViewModel>)
        .catch((error) => {
        });
}

function getAddressList(addressTypeId) {
    return axios.get("api/useraddress/addressTypeId")
        .then((response) => response.data as Promise<AddressResponseViewModel>)
        .catch((error) => {
        });
}

function getUserAddressList(address) {
    return axios.post("api/useraddress/all", address, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<AddressResponseViewModel>)
        .catch((error) => {
        });
}

function getUserCardList(card) {
    return axios.post("api/usercard/all", card, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<CardResponseViewModel>)
        .catch((error) => {
        });
}


function saveAddress(address) {
    return axios.post("api/useraddress", address, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<SaveAddressResponseViewModel>)
        .catch((error) => {
        });
}

function saveCard(card) {
    return axios.post("api/usercard", card, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<SaveCardResponseViewModel>)
        .catch((error) => {
        });
}

function setDefaultAddress(address) {
    return axios.post("api/useraddress/setdefault", address, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<SetDefaultAddressResponseViewModel>)
        .catch((error) => {
        });
}

function saveGuestDetails(guestDetails) {
    return axios.post("api/save/guest-user-details", guestDetails, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<SaveGuestResponseViewModel>)
        .catch((error) => {
            console.log(error);
        });
}
function updateGuestDetails(guestDetails) {
    return axios.post("api/update/guest-user-details", guestDetails, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<SaveGuestResponseViewModel>)
        .catch((error) => {
            console.log(error);
        });
}
function checkMobileExists(userDetails) {
    return axios.post("api/account/mobile/exists", userDetails, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<MobileExistModel>)
        .catch((error) => {
            console.log(error);
        });
}
function updateNotification(userDetails) {
    return axios.post("api/account/notification", userDetails, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<NotificationModel>)
        .catch((error) => {
            console.log(error);
        });
}

function updateUserDetails(user) {
    return axios.post("api/account/personal-info", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<UserProfileResponseViewModel>)
        .catch((error) => {
            console.log(error);
        });
}

function changePassword(user) {
    return axios.post("api/account/login-and-security", user, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<ChangePasswordResponseViewModel>)
        .catch((error) => {
        });
}

export const accountService = {
    changePassword,
    deleteAddress,
    deleteCard,
    getAddressList,
    getUserAddressList,
    getUserCardList,
    saveAddress,
    saveCard,
    setDefaultAddress,
    saveGuestDetails,
    updateGuestDetails,
    updateUserDetails,
    checkMobileExists,
    updateNotification
};
