export const stripeConnectSupportCountries = [
  { country: 'Australia', code: 'AU' },
  { country: 'Austria', code: 'AT' },
  { country: 'Belgium', code: 'BE' },
  { country: 'Brazil ', code: 'BR' },
  { country: 'Canada', code: 'CA' },
  { country: 'Czech Republic', code: 'CZ' },
  { country: 'Denmark', code: 'DK' },
  { country: 'Finland', code: 'FI' },
  { country: 'France', code: 'FR' },
  { country: 'Germany', code: 'DE' },
  { country: 'Hong Kong', code: 'HK' },
  { country: 'Ireland', code: 'IE' },
  { country: 'Japan', code: 'JP' },
  { country: 'Luxembourg', code: 'LU' },
  { country: 'Mexico ', code: 'MX' },
  { country: 'Netherlands', code: 'NL' },
  { country: 'New Zealand', code: 'NZ' },
  { country: 'Norway', code: 'NO' },
  { country: 'Singapore', code: 'SG' },
  { country: 'Spain', code: 'ES' },
  { country: 'Sweden', code: 'SE' },
  { country: 'Switzerland', code: 'CH' },
  { country: 'United Kingdom', code: 'GB' },
  { country: 'United States', code: 'US' },
  { country: 'Italy', code: 'IT' },
  { country: 'Portugal', code: 'PT' }
];

export const isSupportedCurrency = (countries, countryId) => {
  let currentCountry = countries.filter((val) => {
    return val.id == countryId
  });
  if (currentCountry.length > 0) {
    var isSupportedinStripeConnect = stripeConnectSupportCountries.filter((val) => {
      return val.code == currentCountry[0].isoAlphaTwoCode
    });
    if (isSupportedinStripeConnect.length > 0) {
      return true;
    } else {
      return false;
    }
  } else {
    return false;
  }
}

export const getCurrencyOptions = (currencies, countries) => {
  let allSupportedcurrencies = [];
  let nonUSDCurrencies = [];
  let supportedCurrencies = currencies
    .map((item, index) => {
      if (isSupportedCurrency(countries, item.countryId) || item.countryId == 101) {
        return item
      }
    })
    .filter((item) => {
      return item != undefined
    })
  supportedCurrencies.map((val) => {
    if (val.code == 'USD') {
      allSupportedcurrencies.push(val)
    }
  })
  supportedCurrencies.map((val) => {
    if (val.code != 'USD') {
      nonUSDCurrencies.push(val)
    }
  })
  nonUSDCurrencies = nonUSDCurrencies.sort(function (a, b) {
    return a.code > b.code ? 1 : b.code > a.code ? -1 : 0
  })
  nonUSDCurrencies.map((val) => {
    allSupportedcurrencies.push(val)
  })
  return allSupportedcurrencies.map((item) => {
    let currency = {
      label: `${item.code}`,
      value: item.id,
      countryId: item.countryId
    }
    return currency
  })
}

/**
 * format currency names as per FIL
 * @param str takes currency name in string format
 */
function capitalizeCurrency(str: String) {
  if ("swiss frnc" == str.toLowerCase()) {
    str = "swiss franc"
  }
  var currency: any = str.split(" ");
  if (currency.length > 1) {
    currency[0] = currency[0].charAt(0).toUpperCase() + currency[0].slice(1);
    currency[currency.length - 1] = currency[currency.length - 1].charAt(0).toLowerCase() + currency[currency.length - 1].slice(1);
    currency = currency.join(" ");
  } else {
    currency[0] = currency[0].toLowerCase();
    currency[0] = currency[0].charAt(0).toUpperCase() + currency[0].slice(1);
  }
  return currency;
}

export default stripeConnectSupportCountries;



