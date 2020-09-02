using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Web.Core.ErrorMessageProviders
{
    public interface IPaymentErrorMessageProvider
    {
        string GetPaymentErrorMessage(PaymentGatewayError PaymentGatewayError);
    }

    public class PaymentErrorMessageProvider : IPaymentErrorMessageProvider
    {
        public string GetPaymentErrorMessage(PaymentGatewayError PaymentGatewayError)
        {
            switch (PaymentGatewayError)
            {
                case PaymentGatewayError.TransactionDeclined:
                    return "The transaction has been declined by the bank. Please use a different card or contact your bank.";
                case PaymentGatewayError.TransactionCancelled:
                    return "The payment has been canceled. Please retry payment to purchase again.";
                case PaymentGatewayError.ExpiredCard:
                    return "Your card has expired. Please use a different card or contact your bank.";
                case PaymentGatewayError.CardDeclined:
                    return "Your card has been declined by the bank. Please use a different card or contact your bank.";
                case PaymentGatewayError.InvalidExpirationDate:
                    return "Your card's expiration date is invalid or does not match.";
                case PaymentGatewayError.InvalidExpirationYear:
                    return "Your card's expiration year is invalid.";
                case PaymentGatewayError.InvalidExpirationMonth:
                    return "Your card's expiration month is invalid.";
                case PaymentGatewayError.InsufficientFunds:
                    return "The transaction was declined due to insufficient funds in your account. Please use a different card or contact your bank.";
                case PaymentGatewayError.InvalidCardNumber:
                    return "Your card number is incorrect. Please check your card number.";
                case PaymentGatewayError.InvalidCvv:
                    return "Your card's security code is incorrect. Please check your card security code.";
                case PaymentGatewayError.ThreeDSecureAuthenticationFailed:
                    return "The cardholder was not authorized or 3-D secure was not supported. 3-D Secure authentication failed.";
                case PaymentGatewayError.InvalidAmount:
                    return "The transaction was declined due to invalid charge amount. We regret the inconvenience. Please restart your buying process.";
                case PaymentGatewayError.SessionExpired:
                    return "The transaction session has expired. Please restart your buying process.";
                case PaymentGatewayError.ErrorInProcessingCard:
                    return "An error occurred while processing your card. Please try again in a little bit or use a different card or contact your bank.";
                case PaymentGatewayError.InvalidCurrency:
                    return "The transaction was declined due to invalid charge currency. Please select another payment option. We regret the inconvenience.";
                case PaymentGatewayError.InvalidToken:
                    return "Your transaction token has been expired or invalid. Please retry payment to purchase again. We regret the inconvenience.";
                case PaymentGatewayError.Unknown:
                    return "Something has gone wrong, We are unable to process the transaction. Please retry payment to purchase again. We regret the inconvenience.";
                case PaymentGatewayError.ReferToCardIssuer:
                    return "This card has been declined, please contact your card issuing bank or try using a different card. In case you do not have another card, please see https://royalshow.com.au/ticket-outlets/  for the list of outlets from where you can also purchase your tickets to the Show.";
                case PaymentGatewayError.InvalidMerchantId:
                    return "Invalid Merchant";
                case PaymentGatewayError.DoNotHonour:
                    return "Do Not Honour (Account may be overdrawn or frozen)";
                case PaymentGatewayError.TransactionCancelledByCustomer:
                    return "Customer Cancellation";
                case PaymentGatewayError.UnauthorizedTransaction:
                    return "Transaction not Permitted to Cardholder";
                case PaymentGatewayError.ExceededTransactionAttemptLimit:
                    return "Exceeds Withdrawal Amount Limits";
                case PaymentGatewayError.RestrictedCard:
                    return "The card being used for purchase has been declined as it appears to restricted by the card issuing bank to certain types of transactions only. Please contact your card issuing bank to attempt to lift restrictions on types of purchases or alternatively please try using a different card. Please see https://royalshow.com.au/ticket-outlets/  for the list of outlets where you can also purchase your tickets to the Show.";
                case PaymentGatewayError.UnableToProcessTransaction:
                    return "Transaction Cannot be Completed";
                case PaymentGatewayError.PaymentSystemError:
                    return "Unable To Connect To Server";
                case PaymentGatewayError.CardNotSupported:
                    return "Card type unsupported";
                case PaymentGatewayError.SuspectedFraud:
                    return "This card has been declined as it has been flagged in the banking system due to certain security measures that are in place to protect against misuse. Given the confidential nature of banking systems, we are unable to share them here. Please try a different card, or if you do not have another card, please see https://royalshow.com.au/ticket-outlets/  for the list of outlets where you can also purchase your tickets to the Show.";
                case PaymentGatewayError.CardHolderAuthorizationFailed:
                    return "Cardholder could not be authenticated";
                default:
                    return "Something has gone wrong, We are unable to process the transaction. Please retry payment to purchase again. We regret the inconvenience.";
            }
        }
    }
}
