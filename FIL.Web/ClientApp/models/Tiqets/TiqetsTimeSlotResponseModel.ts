export default class TiqetsTimeSlotResponseModel {
    timeSlots: TimeSlotResponseModel[];
}


export class TimeSlotResponseModel {
    is_available: boolean;
    timeslot: string;
}