import constants from "./constants";
import stripe from "./stripe";

export default {
    resolveGateway: (paymentOption: any, currencyType: string) => {
        switch (+paymentOption) {
            case 1:
                if (currencyType !== constants.currencies.inr) {
                    return constants.gateway.stripe;
                } else {
                    return constants.gateway.hdfc;
                }
            case 4:
            case 16:
                return constants.gateway.ccAvenue;
        }
    },

    resolveCardType: (cardNumber: string) => {
        if (!cardNumber || cardNumber.trim() === "") {
            return constants.cardTypes.none;
        }

        if (/^5[1-5]/.test(cardNumber)) {
            return constants.cardTypes.masterCard;
        }

        if (/^4/.test(cardNumber)) {
            return constants.cardTypes.visa;
        }

        if (/^3[47]/.test(cardNumber)) {
            return constants.cardTypes.americanExpress;
        }

        if (/^(5018|5044|5020|5038|6220|6304|6759|676[1-3])/.test(cardNumber)) {
            return  constants.cardTypes.maestro;
        }

        return constants.cardTypes.none;
    },

    resolveCurrencyName: (currencyType: string) => {
        return currencyType === constants.currencies.usd ? "US$" : currencyType;
    },

    processPayment: (formData: any, processPaymentFn: any, paymentCallback: any, errorCallback: any) => {
        if (+formData.paymentGateway === constants.gateway.stripe) {
            /*stripe.proccessPayment(formData, (result, values) => {
                if (result.error) {
                    errorCallback(result.error);
                } else {
                    processPaymentFn(values, paymentCallback);
                }
            });*/
			processPaymentFn(formData, paymentCallback);
        } else {
            processPaymentFn(formData, paymentCallback);
        }
    }
};
