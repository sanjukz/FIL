export default class Transaction {
    channelId: string;
    currencyId: number;
    totalTickets: number;
    grossTicketAmount: number;
    deliveryCharges: number;
    convenienceCharges: number;
    serviceCharge: number
    discountAmount: number;
    netTicketAmount: number;
    discountCode: string;
    transactionStatusId: string;
    iPDetailId: number;
    isEmailSend: boolean;
    isSmsSend: boolean;
    OTP: number;
    isOTPVerified: boolean;
    firstName: string;
    lastName: string;
    phoneCode: string;
    phoneNumber: string;
    emailId: string;
    countryName: string;
    createdUtc: string;
}