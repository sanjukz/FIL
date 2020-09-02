// This file was generated from the Models.tst template
//
export default class CitiesResponseViewModel {
    itinerarySerchData: responseModel[];
    feelStateData: FeelStateData[];
}

export class responseModel {
    cityName: string;
    countryName: string;
}
export class FeelStateData {
    stateName: string;
    stateId: number;
    countryName: string;
}