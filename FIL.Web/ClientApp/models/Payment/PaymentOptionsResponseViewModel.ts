
export default class PaymentOptionsResponseViewModel {
    bankDetails: BankModel[];
    cashCardDetails: CashCardModel[];

}

export class BankModel {
    altId: string;
    bankName: string;
}

export class CashCardModel {
    altId: string;
    cardName: string;
}
