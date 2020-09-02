import { EventTicketAttribute } from "../Checkout/EventTicketDetailDataViewModel";

export class DeliveryOptionsFormDataViewModel {
	transactionId: number;
    deliveryDetail: DeliveryDetail[];
    eventTicketAttributeList?: EventTicketAttribute[]; 
}

export class DeliveryDetail {
	eventTicketAttributeId: number;
	deliveryTypeId: number;
	firstName: string;
	lastName: string;
	email: string;
	phoneCode: string;
	phoneNumber: string;
	representativeFirstName?: string;
	representativeLastName?: string;
	representativeEmail?: string;
	representativePhoneCode?: string;
	representativePhoneNumber?: string;
	courierAddress?: string;
	courierZipcode?: number;
	eventAltid?: string;
}
