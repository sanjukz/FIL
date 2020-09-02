
export default class SubEventDetailDataResponseViewModel {
    eventDetail: EventDetail[];
    id: any;
    name: any;
}

export class EventDetail {
    id: number;
    name: string;
    venueId: number;
    startDateTime: string;
    endDateTime: string;
    groupId: number;
    metaDetails: string;
    termsAndConditions: string;
    publishedDateTime: string;
}
