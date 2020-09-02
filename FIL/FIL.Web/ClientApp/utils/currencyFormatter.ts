/**
 * return currency in '0.00' format & return single value if there zero after fraction
 * @param currency type of param is string or number
 * @returns {currency} return currency in float
 */
export const formatCurrency = (currency) => {
  let val = parseFloat(currency);
  let fix = val.toFixed(2);
  let dec = parseInt(fix.split(".")[1]);
  return dec > 0 ? fix : fix.split(".")[0];
};

const month_names_short = [
  "Jan",
  "Feb",
  "Mar",
  "Apr",
  "May",
  "Jun",
  "Jul",
  "Aug",
  "Sep",
  "Oct",
  "Nov",
  "Dec",
];

//Format Date for online Stream events

export const formatDate = (inputDate) => {
  //Splitting the i/p string
  let inputDateString = inputDate.split(/[^0-9]/);

  let monthName = "";
  let monthIndex = inputDateString[1];
  if (monthIndex[0] == "0") {
    monthName = month_names_short[Number(monthIndex[1]) - 1];
  } else {
    monthName = month_names_short[Number(monthIndex) - 1];
  }
  let hours = Number(inputDateString[3]);
  let mode = "AM";

  if (hours >= 12) {
    if (hours == 12) {
    } else {
      hours = hours - 12;
      if (hours == 12) {
        mode = "AM";
      }
    }
    mode = "PM";
  }
  let hourTime = "";
  if (hours.toString().length == 1) {
    hourTime = hours.toString();
  } else {
    hourTime = hours.toString();
  }
  let result_string =
    inputDateString[2] +
    " " +
    monthName +
    ", " +
    inputDateString[0] +
    "~" +
    hourTime +
    ":" +
    inputDateString[4] +
    " " +
    mode;
  return result_string;
};

export const Convert24hrTo12hrTimeFormat = (time) => {
  let hours = Number(time.split(":")[0]);
  let mode = "AM";
  if (hours >= 12) {
    if (hours == 12) {
    } else {
      hours = hours - 12;
      if (hours == 12) {
        mode = "AM";
      }
    }
    mode = "PM";
  }
  let hourTime = "";
  if (hours.toString().length == 1) {
    hourTime = hours.toString();
  } else {
    hourTime = hours.toString();
  }
  return hourTime + ":" + time.split(":")[1] + " " + mode;
};

//For getting duration for live online events

export const GetDuration = (inputDuration) => {
  if (inputDuration && inputDuration != "") {
    let resultString = "";
    let splitedDuration = inputDuration.split(":");

    if (splitedDuration[0] == "0" || splitedDuration[0] == "00") {
      resultString = splitedDuration[1] + " minutes";
      return resultString;
    } else {
      let min = splitedDuration[1].toString();
      if (min.length == 1 && min[0] == "0") {
        min = "00";
      }
      let hours = splitedDuration[0];
      let hourString = hours == "1" ? " hour" : " hours";
      resultString = hours + ":" + min + " " + hourString;
      return resultString;
    }
  } else {
    return null;
  }
};

// Get Base Price
export const getBasePrice = (eventTicketAttribute: any) => {
  return eventTicketAttribute
    ? eventTicketAttribute[0].price
    : Math.min.apply(
        null,
        this.props.eventData.eventTicketAttribute
          .map((item) => {
            return item.price;
          })
          .filter(Boolean)
      ) !== Infinity
    ? Math.min.apply(
        null,
        eventTicketAttribute
          .map((item) => {
            return item.price;
          })
          .filter(Boolean)
      )
    : 0;
};

//set cookies and reload for currency
export function setCookieAndReload(value) {
  var expires = "";
  var date = new Date();
  date.setTime(date.getTime() + 365 * 24 * 60 * 60 * 1000);
  expires = "; expires=" + date.toUTCString();
  document.cookie = "user_currency=" + (value || "") + expires + "; path=/";
  window.location.reload();
}

export function getCurrencyList() {
  return {
    fullCurrency: [
      "&#36; - AUD - Australian dollar",
      "&#1083;&#1074; - BGN - Bulgarian lev",
      "&#82;&#36; - BRL - Brazilian real",
      "&#36; - CAD - Canadian dollar",
      "&#67;&#72;&#70; - CHF - Swiss franc",
      "&#165; - CNY - Chinese yuan renminbi",
      "&#107;&#114; - DKK - Danish krone",
      "&#x20AC; - EUR - Euro",
      "&#163; - GBP - Pound sterling",
      "&#36; - HKD - Hong Kong dollar",
      "&#107;&#110; - HRK - Croatian kuna",
      "&#65020; - IDR - Indonesian rupiah",
      "&#8362; - ILS - Israeli shekel",
      "₹ - INR - Indian rupee",
      "&#107;&#114; - ISK - Icelandic krona",
      " &#165; - JPY - Japanese yen",
      "&#8361; - KRW - South Korean won",
      "&#36; - MXN - Mexican peso",
      "&#82;&#77; - MYR - Malaysian ringgit",
      "&#107;&#114; - NOK - Norwegian krone",
      "&#36; - NZD - New Zealand dollar",
      "&#36; - PHP - Philippine peso",
      "&#122;&#322; - PLN - Polish zloty",
      "&#108;&#101;&#105; - RON - Romanian leu",
      "&#1088;&#1091;&#1073; - RUB - Russian rouble",
      "&#107;&#114; - SEK - Swedish krona",
      "&#36; - SGD - Singapore dollar",
      "&#3647; - THB - Thai baht",
      "&#8378; - TRY - Turkish lira",
      "&#36; - USD - US dollar",
      "&#82; - ZAR - South African rand",
    ],
    shortCurrency: [
      "AUD",
      "BGN",
      "BRL",
      "CAD",
      "CHF",
      "CNY",
      "DKK",
      "EUR",
      "GBP",
      "HKD",
      "HRK",
      "IDR",
      "ILS",
      "INR",
      "ISK",
      "JPY",
      "KRW",
      "MXN",
      "MYR",
      "NOK",
      "NZD",
      "PHP",
      "PLN",
      "RON",
      "RUB",
      "SEK",
      "SGD",
      "THB",
      "TRY",
      "USD",
      "ZAR",
    ],
  };
}
