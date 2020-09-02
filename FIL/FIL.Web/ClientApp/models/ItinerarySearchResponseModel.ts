export class ItinerarySearchResponseModel {
    itinerarySerchData: responseModel[]
}

class responseModel {
    cityName: string;
    countryName: string;
    cityId: number;
    countryId: number;
}