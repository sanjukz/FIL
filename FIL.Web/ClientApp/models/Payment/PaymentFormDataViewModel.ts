// This file was generated from the Models.tst template
//



export class PaymentFormDataViewModel {
    transactionId: number;
    address: string;
    city: string;
    state: string;
    zipcode: string;
    country: string;
    nameOnCard: string;
    cardNumber: string;
    cvv: string;
    expiryMonth: number;
    expiryYear: number;
    cardTypeId: number;
    paymentOption: string;
    cardAltId: number;
    bankAltId: number;
    token: string;
    paymentGateway: number;
}


export class StripePaymentModel {
    number: string;
    cvc: string;
    exp_month: string;
    exp_year: string;
}

