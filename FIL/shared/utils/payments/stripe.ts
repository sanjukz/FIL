import { Channel } from "../../models/enum/Channel";
import { StripeAccount } from "../../models/enum/StripeAccount";
let stripeFns: any = {};

stripeFns.getStripeKeyOrDefault = (currencyType, currentChannel, stripeAccount?: StripeAccount) => {
    const windowVal: any = window as any;
    let stripeKey: string = "pk_live_3lHeU2G6UkNDiQJRrVgAWQ4N";
    if (windowVal && windowVal.stripePublicToken) {
        stripeKey = windowVal.stripePublicToken as string;
    }
    if ((currencyType == "AUD" || currencyType == "SGD" || currencyType == "HKD" || currencyType == "NZD") && stripeKey == "pk_live_3lHeU2G6UkNDiQJRrVgAWQ4N") {
        stripeKey = "pk_live_CPRarVmR1EfWiLJ4SJp2p3pS";
    }
    if (stripeAccount && currentChannel == Channel.feel && stripeAccount == StripeAccount.StripeAustralia) {
        if (windowVal && windowVal.stripeAustraliaPublicToken) {
            stripeKey = windowVal.stripeAustraliaPublicToken as string;
        }
    }
    if (stripeAccount && currentChannel == Channel.feel && stripeAccount == StripeAccount.StripeIndia) {
        if (windowVal && windowVal.stripeIndiaPublicToken) {
            stripeKey = windowVal.stripeIndiaPublicToken as string;
        }
    }
    if (stripeAccount && currentChannel == Channel.feel && stripeAccount == StripeAccount.StripeUk) {
        if (windowVal && windowVal.stripeUkPublicToken) {
            stripeKey = windowVal.stripeUkPublicToken as string;
        }
    }
    if (stripeAccount && currentChannel == Channel.feel && stripeAccount == StripeAccount.StripeSingapore) {
        if (windowVal && windowVal.stripeSingaporePublicToken) {
            stripeKey = windowVal.stripeSingaporePublicToken as string;
        }
    }
    console.log("" + stripeKey); // TODO: XXX: Remove once tested
    console.log("currencyType: " + currencyType); // TODO: XXX: Remove once tested
    console.log("currentChannel: " + currentChannel); // TODO: XXX: Remove once tested
    return stripeKey;
};

stripeFns.proccessPayment = (values, callback) => {
    const windowVal: any = window as any;
    if (!windowVal || !windowVal.stripePublicToken) {
        callback({ error: { message: "Unknown Error" } }, values);
        return;
    }
    windowVal.Stripe(windowVal.stripePublicToken);
    const card: any = {
        number: values.cardNumber.toString(),
        cvc: values.cvv.toString(),
        exp_month: values.expiryMonth.toString(),
        exp_year: values.expiryYear.toString(),
    };

    if (values.zipcode) {
        card.address_zip = values.zipcode;
    }

    windowVal.Stripe.card.createSource(card, (result) => {
        if (!result.error) {
            values.token = result.id;
        }
        callback(result, values);
    });
};

export default stripeFns;
