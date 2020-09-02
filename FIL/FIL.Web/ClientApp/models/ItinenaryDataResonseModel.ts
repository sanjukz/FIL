export class ItineraryDataResponseModel {
    itineraryData: responseModel[]
}

export default class responseModel {
    id: number;
    eventId: number;
    name: string;
    startTime: string;
    endTime: string;
    travelDuration: string;
    cityId: number;
    cityName: string;
    currency: string;
    categoryName: string;
    latitude: string;
    longitude: string;
    price: string;
    eventName: string;
    eventDescription: string;
    eventSlug: string;
    image: string;
    placeVisitDuration: string;
    placeVisitDate: string;
    adultETAId: number;
    childETAId: number;
    adultPrice: number;
    childPrice: number;
}