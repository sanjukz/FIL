import * as S3FileUpload from 'react-s3'

//get the base S3 cdn url randomized between 1 - 4
export const gets3BaseUrl = () => {
    let min = 0;
    let max = 2;
    let url = "";
    let s3Count = Math.floor(Math.random() * (max - min + 1)) + min;
    if (typeof window != "undefined" && (window as any).urls) {
        max = (window as any).urls.split(",").length > 0 ? (window as any).urls.split(",").length - 1 : 0;
        s3Count = Math.floor(Math.random() * (max - min + 1)) + min;
        url = ((window as any).urls.split(",")[s3Count]) + '/images';
        return url;
    } else { // Fallback will not call
        if (s3Count == 0) {
            s3Count = 1;
        }
        return `https://static${s3Count}.feelitlive.com/images`;
    }
};

//get feel base url for the folder on s3
const getFeelBaseUrl = path => {
    let s3Url = gets3BaseUrl();
    return `${s3Url}/${path}`;
};

// get complete cdn image path
export const getImageCdn = (path, name, ext = "jpg") => {
    let baseUrl = getFeelBaseUrl(path);
    return `${baseUrl}/${name}.${ext}`;
};
