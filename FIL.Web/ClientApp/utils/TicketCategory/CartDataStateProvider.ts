import { CartDataViewModel, CartItem } from "../../models/Cart/CartDataViewModel";
import { MasterEventTypes } from "../../Enum/MasterEventTypes";

export const GetCartData = (ticketCategoryDataProps: any) => {
    var ticketCategoryDataModel: CartDataViewModel = {
        cartItems: [],
    };

    var venueData = ticketCategoryDataProps.ticketCategoryData.venue;
    var currentDateCategories = ticketCategoryDataProps.ticketCategoryData.eventTicketDetail.filter(function (val) {
        return val.eventDetailId == ticketCategoryDataProps.ticketCategoryData.eventDetail[0].id
    });
    currentDateCategories.forEach(function (item) {
        let categoryName = ticketCategoryDataProps.ticketCategoryData.ticketCategory.filter(function (val) {
            return val.id == item.ticketCategoryId
        });

        let price = ticketCategoryDataProps.ticketCategoryData.eventTicketAttribute.filter(function (val) {
            return val.eventTicketDetailId == item.id
        });
        let isAddOn = ticketCategoryDataProps.ticketCategoryData.eventTicketDetailTicketCategoryTypeMappings.filter(function (val) {
            if (val.ticketCategoryTypeId == 2 && val.eventTicketDetailId == item.id) {
                return val.eventTicketDetailId == item.id
            }
        });
        let ticketCategoryMapping = ticketCategoryDataProps.ticketCategoryData.eventTicketDetailTicketCategoryTypeMappings.filter(function (val) {
            if (val.eventTicketDetailId == item.id) {
                return val.eventTicketDetailId == item.id
            }
        });
        let ticketCategoryDescription = ticketCategoryDataProps.ticketCategoryData.eventTicketAttribute.filter(function (val) {
            return val.eventTicketDetailId == item.id
        });
        let eventTermsAndConditions = ticketCategoryDataProps.ticketCategoryData.event.termsAndConditions;
        var deliveryOptions = [];
        ticketCategoryDataProps.ticketCategoryData.eventDeliveryTypeDetails.filter(function (val) {
            deliveryOptions.push(val.deliveryTypeId)
        });
        let cartJson: CartItem = {
            altId: ticketCategoryDataProps.eventAltId,
            eventDetailId: ticketCategoryDataProps.ticketCategoryData.eventDetail[0].id,
            name: ticketCategoryDataProps.ticketCategoryData.eventDetail[0].name,
            eventStartDate: ticketCategoryDataProps.ticketCategoryData.eventDetail[0].startDateTime,
            venue: venueData[0].name,
            city: ticketCategoryDataProps.ticketCategoryData.city[0].name,
            ticketCategoryId: categoryName[0].id,
            ticketCategoryName: categoryName[0].name,
            eventTicketAttributeId: price[0].id,
            eventTicketDetail: item.altId,
            currencyId: ticketCategoryDataProps.ticketCategoryData.currencyType.id,
            currencyName: ticketCategoryDataProps.ticketCategoryData.currencyType.code,
            quantity: 0,
            pricePerTicket: price[0].price,
            specialPrice: price[0].specialprice,
            ticketCategoryDescription: ticketCategoryDescription[0].ticketCategoryDescription,
            eventTermsAndConditions: eventTermsAndConditions,
            selectedDate: "",
            isTimeSelection: false,
            placeVisitTime: "",
            guestList: [],
            isAddOn: (isAddOn.length > 0 ? true : false),
            ticketSubCategoryId: ((ticketCategoryMapping.length > 0) ? ticketCategoryMapping[0].ticketCategorySubTypeId : -1),
            ticketCategoryTypeId: ((ticketCategoryMapping.length > 0) ? ticketCategoryMapping[0].ticketCategoryTypeId : -1),
            deliveryOptions: deliveryOptions,
            isItinerary: false,
            isTiqetsPlace: false,
            timeSlot: "",
            timeStamp: new Date(),
            isHohoPlace: false,
            parentCategory: ticketCategoryDataProps.ticketCategoryData.category.slug,
            subCategory: ticketCategoryDataProps.ticketCategoryData.subCategory.slug,
            subCategoryId: ticketCategoryDataProps.ticketCategoryData.subCategory.id,
            categoryId: ticketCategoryDataProps.ticketCategoryData.category.id,
            transactionType: (ticketCategoryDataProps.ticketCategoryData.event.masterEventTypeId == MasterEventTypes.Online && ((ticketCategoryMapping.length > 0 && (categoryName[0].id != 19452 && categoryName[0].id != 12259)) ? ticketCategoryMapping[0].ticketCategoryTypeId : -1) == 1) ? 7 : (ticketCategoryDataProps.ticketCategoryData.event.masterEventTypeId == MasterEventTypes.Online && ((ticketCategoryMapping.length > 0) ? ticketCategoryMapping[0].ticketCategoryTypeId : -1) == 2) ? 8 : 1,
            additionalInfo: price[0].additionalInfo,
            discountAmount: (price[0].srCitizenDiscount && price[0].srCitizenDiscount != "" && price[0].srCitizenDiscount != null) ? price[0].srCitizenDiscount : 0,
            masterEventTypeId: ticketCategoryDataProps.ticketCategoryData.event.masterEventTypeId,
            eventStatusId: ticketCategoryDataProps.ticketCategoryData.event.eventStatusId,
            scheduleDetailId: ticketCategoryDataProps.scheduleDetail ? ticketCategoryDataProps.scheduleDetail.scheduleDetailId : 0
        }
        ticketCategoryDataModel.cartItems.push(cartJson);
    });
    return ticketCategoryDataModel;
};
