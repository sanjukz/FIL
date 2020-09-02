//get the base S3 cdn url randomized between 1 - 4
export const ParseDateToLocal = (s: any) => {
    var b = s.split(/\D/);
    var date = new Date(s);
    return new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000)
};