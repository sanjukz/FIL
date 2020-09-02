/**
 * Object containing card types and their RegEx
 * more: 
 */

let acceptedCreditCards = {
    visa: /^4[0-9]{12}(?:[0-9]{3})?$/,
    mastercard: /^5[1-5][0-9]{14}$|^2(?:2(?:2[1-9]|[3-9][0-9])|[3-6][0-9][0-9]|7(?:[01][0-9]|20))[0-9]{12}$/,
    amex: /^3[47][0-9]{13}$/,
};

/**
 * A mapping object from accepted credit card keys to their names
 */

let acceptedCreditCardNames = {
    visa: "Visa",
    mastercard: "Mastercard",
    amex: "American Express"
}

/**
 * Card validation funtion for the above types.
 * Takes card number as string 
 */

export default function validateCard(cardNumber: string) {
    let value = cardNumber.replace(/\D/g, ''); // remove all non digit characters
    let sum = 0;
    let shouldDouble = false;

    // loop through values starting at the rightmost side
    for (let i = value.length - 1; i >= 0; i--) {
        let digit = parseInt(value.charAt(i));

        if (shouldDouble) {
            if ((digit *= 2) > 9) digit -= 9;
        }

        sum += digit;
        shouldDouble = !shouldDouble;
    }

    let valid = (sum % 10) == 0;
    let accepted = false;
    let card;
    // loop through the card type object keys
    Object.keys(acceptedCreditCards).forEach((key) => {
        let regex = acceptedCreditCards[key];
        if (regex.test(value)) {
            accepted = true;
            card = acceptedCreditCardNames[key];
        }
    });

    return {
        valid,
        accepted,
        card
    };
}