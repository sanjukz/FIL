// This file was generated from the Models.tst template
//

import { SessionViewModel } from "shared/models/SessionViewModel";
import { StripeAccount } from "shared/models/enum/StripeAccount";

export class UserCheckoutResponseViewModel {
    success: boolean;
    session: SessionViewModel;
    isLockedOut?: boolean;
    requiresTwoFactor?: boolean;
    isNotAllowed?: boolean;
    transactionId: number;
    transactionAltId: number;
    isTiqetsOrderFailure?: boolean;
    isPaymentByPass: boolean;
    stripeAccount: StripeAccount;
}