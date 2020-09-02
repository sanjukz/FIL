export default class TicketCategory {
    id: number;
    name: string;
}

export class TicketCategoryDetailResponseViewModel {
    ticketCategoryDetails: TicketCategoryDetails[];
    ticketCategories?: TicketCategory[]
}

export class TicketCategoryDetails {
    id: number;
    ticketCategoryId: string;
    description: string;
    quantity: number;
}