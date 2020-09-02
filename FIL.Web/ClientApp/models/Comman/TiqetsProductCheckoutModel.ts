export default class TiqetsProductCheckoutModel {
    id: number;
    productId: string;
    mustKnow: string;
    goodToKnow: string;
    prePurchase: string;
    usage: string;
    excluded: string;
    included: string;
    postPurchase: string;
    hasDynamicPrice: boolean;
    hasTimeSlot: boolean;
}
