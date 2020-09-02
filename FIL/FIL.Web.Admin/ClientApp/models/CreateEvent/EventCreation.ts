// This file was generated from the Models.tst template
//


import { Host } from '../CreateEvent/Host';
import { PlaceCalendarRequestViewModel } from '../PlaceCalendar/PlaceCalendarRequestViewModel';

export class EventCreation {
    title: string;
    eventDescription: string;
    eventCategoryId: string;
    eventHosts: Host[];
    isEdit: boolean;
    eventId: number;
    eventCalendar: PlaceCalendarRequestViewModel;
}
