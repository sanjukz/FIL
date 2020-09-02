
export const convertTime12to24 = (time12h) => {
    var time = time12h;
    var hrs = Number(time.match(/^(\d+)/)[1]);
    var mnts = Number(time.match(/:(\d+)/)[1]);
    var format = time.match(/\s(.*)$/)[1];
    if (format == "pm" && hrs < 12) hrs = hrs + 12;
    if (format == "am" && hrs == 12) hrs = hrs - 12;
    var hours = hrs.toString();
    var minutes = mnts.toString();
    if (hrs < 10) hours = "0" + hours;
    if (mnts < 10) minutes = "0" + minutes;
    return (hours + ":" + minutes);
}

export const scrapeNumbers = (string) => {
    if (!string)
        return '';
    var seen = {};
    var results = [];
    string.match(/\d+/g).forEach(function (x) {
        if (seen[x] === undefined)
            results.push(parseInt(x));
        seen[x] = true;
    });
    return results.toString();
}

export const getOrdinalNum = (number) => {
    let selector;

    if (number <= 0) {
        selector = 4;
    } else if ((number > 3 && number < 21) || number % 10 > 3) {
        selector = 0;
    } else {
        selector = number % 10;
    }

    return number + ['th', 'st', 'nd', 'rd', ''][selector];
};
