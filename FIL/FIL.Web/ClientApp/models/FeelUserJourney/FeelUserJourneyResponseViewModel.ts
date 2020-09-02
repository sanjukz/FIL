import { MasterEventTypes } from '../../Enum/MasterEventTypes'
import { EventFrequencyType } from '../../Enum/EventFrequencyType'

export default class FeelUserJourneyViewModel {
    success: boolean;
    subCategories: SubCategory[];
    geoData: GeoData;
    dynamicPlaceSections: DynamicPlaceSections[];
    allPlaceTiles: DynamicPlaceSections;
    contryPageDetails: CountryPageDetail;
    searchValue: string;
}

export class CountryPageDetail {
    description: string;
    ratings: string;
    reviewCount: string;
    count: number;
    sectionTitle: string;
}

export class SectionDetails {
    heading: string;
    subHeading: string;
    count: number;
    isShowMore: boolean;
    url: string;
    query: string;
}

export class SubCategory {
    sectionDetails: SectionDetails;
    id: number;
    displayName: string;
    slug: string;
    count: number;
    url: string;
    query: string;
    isMainCategory: boolean;
}

export class GeoLocation {
    id: number;
    name: string;
    count: number;
    url: string
    query: string;
}

export class GeoData {
    sectionDetails: SectionDetails
    cities: GeoLocation[];
    states: GeoLocation[];
    countries: GeoLocation[];
    url: string;
}

export class DynamicPlaceSections {
    placeDetails: PlaceDetail[];
    sectionDetails: SectionDetails
}

export class PlaceDetail {
    id: number;
    eventTypeId?: number;
    altId: string;
    name: string;
    slug: string;
    eventDetailId: number;
    eventCategoryId: number;
    parentCategory: string;
    parentCategoryId: number;
    parentCategorySlug: string;
    category: string;
    cityId: number;
    cityName: string;
    stateId: number;
    stateName: string;
    countryId: number;
    countryAltId: string;
    countryName: string;
    categorySlug: string;
    subCategorySlug: string;
    minPrice: number;
    maxPrice: number;
    rating: number;
    currencyId: number;
    currency: string;
    subCategories: string[]
    url: string;
    duration: string;
    masterEventTypeId?: MasterEventTypes;
    eventFrequencyType?: EventFrequencyType;
} 