export default class EventDetail {
    id: number;
    eventId: number;
    name: string;
    altId: string;
    venueId: number;
    startDateTime: string;
    endDateTime: string;
    groupId: number;
    isEnabled?: boolean;
    metaDetails: string;
    termsAndConditions: string;
    publishedDateTime: string;
}
