export class SearchResponseViewModel {
    cities: CommonSearchResultModel[];
    states: CommonSearchResultModel[];    
    countries: CommonSearchResultModel[];
    categoryEvents: CategorySearchResultModel[];
}

class CommonSearchResultModel {
    countryId: number;
    stateId: number;
    cityId: number;
    countryName?: string;
    stateName?: string;
    cityName?: string;
}

class CategorySearchResultModel {
    parentCategory: string;
    name: string;
    altId: string;
    cityName: string;
    countryName: string;
    redirectUrl: string;
} 