export default class EventTicketAttribute {
    id: number;
    eventTicketDetailId: number;
    salesStartDateTime: string
    salesEndDatetime: number
    availableTicketForSale: number
    remainingTicketForSale: number
    ticketCategoryDescription: string
    viewFromStand: string
    isSeatSelection: boolean
    price: number
    isInternationalCardAllowed: boolean
    isEMIApplicable: boolean;
    additionalInfo?: string;
    srCitizenDiscount?: string;
}