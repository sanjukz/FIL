export default class TransactionDeliveryDetail {
    id: number;
    transactionDetailId: number;
    deliveryTypeId: string;
    pickupBy?: number;
    secondaryName: string;
    secondaryContact: string;
    secondaryEmail: string;
    courierAddress: string;
    courierZipcode?: number;
    deliveryStatus: string;
    deliveryDate: string;
    deliverdTo: string;
    courierServiceId: string;
    trackingId: string;
}
