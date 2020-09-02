import { CategoryViewModel } from './CategoryViewModel';
import Event from "./Comman/EventViewModel";
import Venue from "./Comman/VenueViewModel";
import City from "./Comman/CityViewModel";
import State from "./Comman/StateViewModel";
import Country from "./Comman/CountryViewModel";
import CategoryEvent from "./Comman/CategoryEventViewModel";
import EventTicketAttribute from "./Comman/EventTicketAttributeViewModel";
import EventTicketDetail from "./Comman/EventTicketDetailViewModel";
import CurrencyType from "./Comman/CurrencyTypeViewModel";
import ReviewsRatingViewModel from "./ReviewsRating/ReviewsRatingViewModel";
import CountryDescription from "./Comman/CountryDescriptionModel";
import CoutryContentMapping from "./Comman/CountryContentMappingModel";
import CityDescriptionModel from "./Comman/CityDescriptionModel";
import { EventFrequencyType } from '../Enum/EventFrequencyType';

export class CategoryPageResponseViewModel {
    event: Event;
    venue: Venue[];
    city: City[];
    state: State[];
    country: Country[];
    rating: ReviewsRatingViewModel[];
    categoryEvent: CategoryEvent;
    eventTicketAttribute: EventTicketAttribute[];
    eventTicketDetail: EventTicketDetail[];
    eventCategories: string[];
    currencyType: CurrencyType;
    eventType: string;
    eventCategory: string;
    parentCategory: string;
    countryDescription: CountryDescription;
    cityDescription: CityDescriptionModel;
    CoutryContentMapping: CoutryContentMapping[];
    duration: string;
    localStartDateTime?: string;
    timeZoneAbbrivation?: string;
    eventFrequencyType?: EventFrequencyType;
}