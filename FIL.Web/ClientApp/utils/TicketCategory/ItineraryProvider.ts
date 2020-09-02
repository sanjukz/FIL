import { MasterEventTypes } from "../../Enum/MasterEventTypes";

export const checkIsValidItinerary = (ticketCategoryData: any,
    state: any,
    hasHohoTimeSlots: boolean,
    isAttendEvent: boolean,
    selectedDate: any) => {

    var data = state.cartData.filter(function (item) {
        return item.ticketCategoryTypeId == 1 || item.ticketCategoryTypeId == -1
    });
    let hasTiqetTimeSlots = false, hasHohoPlaceTimeSlots = false;
    if (ticketCategoryData.tiqetsCheckoutDetails && ticketCategoryData.tiqetsCheckoutDetails.hasTimeSlot) {
        hasTiqetTimeSlots = ticketCategoryData.event.eventSourceId == 6 && ticketCategoryData.tiqetsCheckoutDetails.hasTimeSlot;
    }
    if (ticketCategoryData.event.eventSourceId == 3 && ticketCategoryData.citySightSeeingTicketDetail.ticketClass != 1) {
        hasHohoPlaceTimeSlots = true;
    }
    if (hasHohoPlaceTimeSlots && !state.selectedTimeSlot) {
        alert("Please select time.")
        return false
    }
    if (((hasTiqetTimeSlots || hasHohoTimeSlots) ? selectedDate == null : !state.dateSelected) && !isAttendEvent) {
        alert("Please select date");
    } else {
        let ticketCartData = JSON.parse(localStorage.getItem("currentCartItems"));

        let childCartData = ticketCartData.filter(cart => cart.ticketCategoryId == 84);
        let adultCartData = ticketCartData.filter(cart => cart.ticketCategoryId !== 84);

        var childQty = ticketCartData.filter(cart => cart.ticketCategoryName.toLowerCase().includes("child"));
        var InfantQty = ticketCartData.filter(cart => cart.ticketCategoryName.toLowerCase().includes("infant"));
        var seniorQty = ticketCartData.filter(cart => cart.ticketCategoryName.toLowerCase().includes("senior"));
        var adultQty = ticketCartData.filter(cart => cart.ticketCategoryName.toLowerCase().includes("adult"));

        if (hasTiqetTimeSlots || hasHohoPlaceTimeSlots) {
            ticketCartData.map((item) => {
                item.selectedDate = new Date(selectedDate).toString()
            })
            localStorage.setItem('currentCartItems', JSON.stringify(ticketCartData));
        }
        if ((InfantQty.length >= 1 && (adultQty.length == 0 || seniorQty.length == 0)) || childQty.length >= 1 && adultQty.length == 0) {
            alert("Please select at least one adult");
            return false;
        }
        var categories = [];
        data.forEach(function (val) {
            if (val.guestList.length > 0) {
                val.guestList.forEach(function (guest) {
                    categories.push(guest);
                });
            }
        });
        if (adultCartData.length == 0) {
            alert("Please add tickets");
        } else {
            var guestList = [];

            var allCartItems = (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0' && localStorage.getItem('cartItems') != 'undefined') ? JSON.parse(localStorage.getItem("cartItems")) : [];
            var currentCartItems = (localStorage.getItem('currentCartItems') != null && localStorage.getItem('currentCartItems') != '0') ? JSON.parse(localStorage.getItem("currentCartItems")) : [];

            allCartItems = allCartItems.filter(function (item) { /* remove already added cart items for same day and event */
                // var selectedDate = new Date(item.selectedDate);
                var isAlreadyExistsForPlace = false;
                currentCartItems.map(function (val) {
                    // var currentSelectedDate = new Date(val.selectedDate);
                    if (val.altId == item.altId) {
                        isAlreadyExistsForPlace = true;
                    }
                });
                if (!isAlreadyExistsForPlace) {
                    return item;
                }
            })
            currentCartItems.forEach(function (val) {
                allCartItems.push(val);
            });
            localStorage.setItem('cartItems', JSON.stringify(allCartItems));
            return true;
        }
    }
}

export const checkIsLiveOnlineEvents = () => {
    var allCartItems = (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0' && localStorage.getItem('cartItems') != 'undefined') ? JSON.parse(localStorage.getItem("cartItems")) : [];
    let filteredCartItems = allCartItems.filter((item) => {
        return item.masterEventTypeId == MasterEventTypes.Online
    });
    if (filteredCartItems.length > 0) {
        //remove other items from cart if live onlive event is the latest added
        let sortedItems=filteredCartItems.sort(TimeSort);
        let filteredSortedItem=filteredCartItems.filter((val)=>{return val.altId==sortedItems[sortedItems.length-1].altId})
        localStorage.setItem('cartItems',JSON.stringify(filteredSortedItem));
        return true;
    } else {
        return false;
    }
}
function TimeSort(a,b){
    if(a.timeStamp>b.timeStamp){
        return 1;
    }
    else if(a.timeStamp<b.timeStamp){
        return -1;
    }
    else{
        return 0;
    }
}