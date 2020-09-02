import { CategoryPageResponseViewModel } from '../CategoryPageResponseViewModel';

export class CityCountryViewModel {
    cityCountry?: CityCountryResult[];
    countryCategoryCounts?: CityCountryResult[];
	countryAllPlace?: CategoryPageResponseViewModel[];
	countryState?: CityCountryResult[];
	cityData?: CityResult;
	countryPlace?:CityCountryResult[];
}

export class CityCountryResult {
	name: string;
  altId: string;
  id: number;
	count: number;
}

export class CityResult {
	category: {};
	categoryEvents: any[];
}
