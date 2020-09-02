export default class EventTicketAttribute {
    id: number;
    eventTicketDetailId: number;
    salesStartDateTime: string;
    salesEndDatetime: string;
    ticketTypeId: string;
    currencyId: number;
    availableTicketForSale: number;
    remainingTicketForSale: number;
    ticketCategoryDescription: string;
    price: number;
    localPrice: number;
    seasonPackage: boolean;
    seasonPackagePrice: number;
    isInternationalCardAllowed?: boolean;
    ticketValidity?: string;
    ticketValidityType?: string;
    ticketCategoryNotes?: string;
    isEMIApplicable?: boolean;
}