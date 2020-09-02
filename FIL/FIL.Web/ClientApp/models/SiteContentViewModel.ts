
export class SiteContentViewModel {    
    content: Content;
    siteBanners: SiteBanner[];
    defaultSearchCities: DefaultCitySearchResult[];
    defaultSearchStates: DefaultStateSearchResult[];
    defaultSearchCountries: DefaultCountrySearchResult[];
}

export class Content {
    altId: string;        
    siteTitle: string;
    siteLogo: string;
    bannerText: string;
    siteLevel: string;
}

export class SiteBanner {
    altId: string;     
    bannerName: string; 
    sortOrder: string;     
}

export class DefaultCitySearchResult {
    altId: string;
    name: string;
    stateAltid: string;
}

export class DefaultStateSearchResult {
    altId: string;
    name: string;
}

export class DefaultCountrySearchResult {
    altId: string;
    name: string;
}
