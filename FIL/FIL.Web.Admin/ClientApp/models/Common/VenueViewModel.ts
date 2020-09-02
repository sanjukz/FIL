export default class Venue {
    id: number;
    name: string;
    altId: string;
    cityId: number;
    addressLineOne: string;
    addressLineTwo: string;
    latitude: string;
    longitude: string;
    hasImages: boolean;
    prefix: string;
}

export class LiveOnlineTransactionDetailResponseModel {
    eventId: number;
    eventCategoryId: number;
    subCategoryDisplayName: string;
    startDateTime: string;
}