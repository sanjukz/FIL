import { MasterEventTypes } from "../../Enum/MasterEventTypes";

export class TicketValidationModel {
    catError: string;
    isValid: boolean
}

export const checkIsTiqetCategoryValid = (ticketCategoryData: any, item: any) => {
    let currentEventTicketDetailId = ticketCategoryData.eventTicketAttribute.filter((val) => {
        return item.eventTicketAttributeId == val.id
    });
    if (currentEventTicketDetailId && currentEventTicketDetailId.length > 0) {
        if (ticketCategoryData.validWithVariantModel != undefined && ticketCategoryData.validWithVariantModel.length > 0) {
            let validWithVariantModel = ticketCategoryData.validWithVariantModel.filter((val) => {
                return val.eventTicketDetailId == currentEventTicketDetailId[0].eventTicketDetailId
            });
            if (validWithVariantModel && validWithVariantModel.length > 0) {
                let validWithEventTicketDetail = ticketCategoryData.eventTicketAttribute.filter((val) => {
                    return validWithVariantModel[0].validWithEventTicketDetailId.indexOf(val.eventTicketDetailId) >= 0
                });
                let reqdTicketCats = [];
                validWithEventTicketDetail.map((val) => {
                    let eventTicketDetail = ticketCategoryData.eventTicketDetail.filter((id) => {
                        return id.id == val.eventTicketDetailId
                    });
                    let ticketCat = ticketCategoryData.ticketCategory.filter((cat) => {
                        return cat.id == eventTicketDetail[0].ticketCategoryId
                    });
                    let ticketCartData = JSON.parse(localStorage.getItem("currentCartItems"));
                    //check if the category already in the cart
                    let isPresent = ticketCartData.filter((cartdata) => {
                        return cartdata.ticketCategoryId == ticketCat[0].id
                    });
                    if (isPresent && isPresent.length == 0) {
                        reqdTicketCats.push(ticketCat[0].name);
                    }
                })
                if (reqdTicketCats.length > 0) {
                    let reqdStrings = "";
                    reqdTicketCats.map((val, indx) => {
                        reqdStrings = reqdStrings + val;
                        if (indx < reqdTicketCats.length - 1) {
                            reqdStrings = reqdStrings + ",";
                        }
                    });
                    let ticketValidationModel: TicketValidationModel = {
                        isValid: false,
                        catError: `You need to add ${reqdStrings} to continue`
                    }
                    return ticketValidationModel;
                } else {
                    let ticketValidationModel: TicketValidationModel = {
                        isValid: true,
                        catError: ""
                    }
                    return ticketValidationModel;
                }
            }
            else {
                let ticketValidationModel: TicketValidationModel = {
                    isValid: true,
                    catError: ""
                }
                return ticketValidationModel;
            }
        } else {
            let ticketValidationModel: TicketValidationModel = {
                isValid: true,
                catError: ""
            }
            return ticketValidationModel;
        }
    }
    else {
        let ticketValidationModel: TicketValidationModel = {
            isValid: true,
            catError: ""
        }
        return ticketValidationModel;
    }
}



export const getTicketCategoryRemoveValidationModel = (ticketCategoryData: any, item: any, isTiqetsEvent: boolean) => {
    if (isTiqetsEvent) {
        let that = this;
        let currentEventTicketDetailId = ticketCategoryData.eventTicketAttribute.filter((val) => {
            return item.eventTicketAttributeId == val.id
        });
        if (currentEventTicketDetailId && currentEventTicketDetailId.length > 0 && ticketCategoryData.validWithVariantModel.length
        ) {
            if (ticketCategoryData.validWithVariantModel.length > 0) {
                let validWithVariantModel = ticketCategoryData.validWithVariantModel.filter((val) => {
                    return val.validWithEventTicketDetailId.indexOf(currentEventTicketDetailId[0].eventTicketDetailId) >= 0
                });
                if (validWithVariantModel && validWithVariantModel.length > 0) {
                    let eventTicketDetail = ticketCategoryData.eventTicketDetail.filter((id) => {
                        return id.id == validWithVariantModel[0].eventTicketDetailId
                    });
                    let ticketCat = ticketCategoryData.ticketCategory.filter((cat) => {
                        return cat.id == eventTicketDetail[0].ticketCategoryId
                    });
                    let ticketCartData = JSON.parse(localStorage.getItem("currentCartItems"));
                    let isPresentInCart = ticketCartData.filter((val) => {
                        return val.ticketCategoryId == ticketCat[0].id
                    })
                    if (isPresentInCart && isPresentInCart.length > 0) {
                        let ticketValidationModel: TicketValidationModel = {
                            isValid: false,
                            catError: `You need to remove first ${ticketCat[0].name} to continue`
                        }
                        return ticketValidationModel;
                    } else {
                        let ticketValidationModel: TicketValidationModel = {
                            isValid: true,
                            catError: ""
                        }
                        return ticketValidationModel;
                    }
                } else {
                    let ticketValidationModel: TicketValidationModel = {
                        isValid: true,
                        catError: ""
                    }
                    return ticketValidationModel;
                }

            }
        } else {
            let ticketValidationModel: TicketValidationModel = {
                isValid: true,
                catError: ""
            }
            return ticketValidationModel;
        }
    }
    else {
        let ticketValidationModel: TicketValidationModel = {
            isValid: true,
            catError: ""
        }
        return ticketValidationModel;
    }
}

export const checkIsLiveOnlineCategoryValid = (altId) => {
    let currentCartItems = JSON.parse(localStorage.getItem('currentCartItems'));
    if (currentCartItems && currentCartItems.length > 0) {
        let filteredLiveOnlineItems = currentCartItems.filter((item) => {
            return item.masterEventTypeId == MasterEventTypes.Online && item.altId == altId && item.ticketCategoryTypeId == 1
        });
        if (filteredLiveOnlineItems && filteredLiveOnlineItems.length > 0) {
            let ticketValidationModel: TicketValidationModel = {
                isValid: false,
                catError: "You need only 1 ticket when viewing from one location. If you wish to buy for someone else who is not collocated with you, please make a separate purchase"
            }
            return ticketValidationModel;
        }
        else {
            let ticketValidationModel: TicketValidationModel = {
                isValid: true,
                catError: ""
            }
            return ticketValidationModel;
        }
    } else {
        let ticketValidationModel: TicketValidationModel = {
            isValid: true,
            catError: ""
        }
        return ticketValidationModel;
    }
}

// 19452 -> Donte ticket cat on prod
// 12259 -> Donte ticket cat on dev
export const isDonationExists = (ticketCategoryData: any) => {
    let isDonationCategoryExitsts = ticketCategoryData.filter((val) => {
        return (val.ticketCategoryId == 19452 || val.ticketCategoryId == 12259)
    });
    return (isDonationCategoryExitsts.length > 0 ? true : false)
}

export const isAddOnExists = (ticketCategoryData: any) => {
    let isAddOn = ticketCategoryData.filter((val) => {
        return val.ticketCategoryTypeId == 2
    });
    return (isAddOn.length > 0 ? true : false)
}

export const getOnlyTickets = (ticketCategoryData: any) => {
    let donationCategories = ticketCategoryData.filter((val) => {
        return (val.ticketCategoryId != 19452 && val.ticketCategoryId != 12259 && (val.ticketCategoryTypeId == 1 || val.ticketCategoryTypeId == -1))
    });
    return donationCategories;
}

export const getOnlyAddOns = (ticketCategoryData: any) => {
    let addOns = ticketCategoryData.filter((val) => {
        return (val.ticketCategoryTypeId == 2)
    });
    return addOns;
}

export const getOnlyDonation = (ticketCategoryData: any) => {
    let donationCategories = ticketCategoryData.filter((val) => {
        return (val.ticketCategoryId == 19452 || val.ticketCategoryId == 12259)
    });
    return donationCategories;
}

export const getOnlyBSP = (ticketCategoryData: any) => {
    let bspCategories = ticketCategoryData.filter((val) => {
        return (val.ticketCategoryId == 19350 || val.ticketCategoryId == 12079)
    });
    return bspCategories;
}