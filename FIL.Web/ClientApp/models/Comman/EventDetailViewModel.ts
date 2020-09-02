import { EventFrequencyType } from '../../Enum/EventFrequencyType';

export default class EventDetail {
    name: string;
    AltId: string;
    venueId: number;
    startDateTime: string;
    endDateTime: string;
    groupId: number;
    metaDetails: string;
    termsAndConditions: string;
    publishedDateTime: string;
    isEnabled: boolean;
    id?: number;
    eventId?: number;
    eventFrequencyType?: EventFrequencyType;
}
