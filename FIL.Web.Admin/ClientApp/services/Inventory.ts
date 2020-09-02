import axios from "axios";
import InventoryResponseViewModel  from "../models/Inventory/InventoryResponseViewModel";
import DocumentTypesSaveResponseViewModel from "../models/Inventory/DocumentTypesSaveResponseViewModel";
import PlaceCalendarResponseViewModel from "../models/PlaceCalendar/PlaceCalendarResponseViewModel";

function inventoryRequest(inventory) {
    return axios.post("api/save/inventory", inventory, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<InventoryResponseViewModel>)
        .catch((error) => {
        });
}

function customerIdTypeSaveRequest(customerIdTypeView) {
    return axios.post("api/save/customerIdType", customerIdTypeView, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<DocumentTypesSaveResponseViewModel>)
        .catch((error) => {
        });
}

function saveEventDetail(inventory) {
    return axios.post("api/save/eventDetail", inventory, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<PlaceCalendarResponseViewModel>)
        .catch((error) => {
        });
}

function saveFinanceDetails(inventory) {
    return axios.post("api/save/event/finance", inventory, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<PlaceCalendarResponseViewModel>)
        .catch((error) => {
        });
}

export const inventoryService = {
    inventoryRequest,
    customerIdTypeSaveRequest,
    saveEventDetail,
    saveFinanceDetails
};

