// This file was generated from the Models.tst template
//

import { CategoryViewModel } from './CategoryViewModel';

export class CategoryResponseViewModel {
    categories: CategoryViewModel[];
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
