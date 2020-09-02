const getImageUrl = (path, timestamp, baseUrl) => {

};

const useCdn = () => {
    return window && (window as any).useCdn;
};

const useCdnCount = () => {
    return useCdn() && window && (window as any).s3CdnCount && (window as any).s3CdnCount > 0;
};

const getCdnCount = () => {
    return (window as any).s3CdnCount || 0;
};

const getKzCdnUrl = () => {

};

const getS3CdnUrl = () => {
    if (getCdnCount() > 0) {

    }

    return 
};

export const getS3Url = (path, timestamp) => {
    let baseUrl = (window as any).s3BasePath;
    if (useCdn()) {

    }
};


export const getKzUrl = (path, timestamp)  => {
    let baseUrl = (window as any).basePath;
};