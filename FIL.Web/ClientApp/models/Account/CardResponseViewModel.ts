export default class CardResponseViewModel {
    userCards: UserCards[];
}

export class UserCards {
    altId: string;
    nameOnCard: string;
    cardNumber: number;
    expiryMonth: string;
    expiryYear: string;
    cardTypeId: number;
}
