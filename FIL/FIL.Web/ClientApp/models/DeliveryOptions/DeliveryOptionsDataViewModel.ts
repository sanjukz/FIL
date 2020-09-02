import User from "../Comman/UserViewModel";
class EventDeliveryTypeDetail {
    id: number;
    eventDetailId: number;
    deliveryTypeId: string;
    notes: string;
    endDate: string;
}

export class DeliveryOptionsDataViewModel {
    eventDeliveryTypeDetails: EventDeliveryTypeDetail[];
    userDetails: User[];
}
