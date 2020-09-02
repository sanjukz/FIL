export const stripHtmlTags = (text) => {
    var strippedString = text.replace(/(<([^>]+)>)/ig, "");
    strippedString = strippedString.replace(/&#(\d+);/g, "");
    return strippedString;
}