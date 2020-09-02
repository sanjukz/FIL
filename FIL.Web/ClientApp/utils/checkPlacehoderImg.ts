export class ImageClass {
    slug: string;
    imageURL: string;
}

export class ImageClassArray {
    image: ImageClass[]
}

let data: ImageClassArray = {
    image: []
};

export async function checkPlaceholderImg(
    parentCategory: string,
    category: string,
    pagePath: string,
    placeHolderType: string,
    baseURL: string
) {
    let url = null;
    let categoryUrl = category
        ? category
            .split(" ")
            .join("-")
            .replace("&", "and")
            .replace("/", "-")
            .toLowerCase()
        : "";
    let parentCategoryUrl = parentCategory
        ? parentCategory
            .split(" ")
            .join("-")
            .replace("&", "and")
            .replace("/", "-")
            .toLowerCase()
        : "";

    if (pagePath.indexOf('tiles') > -1) { // only tiles image
        let suCategoryImageURL = getImage(categoryUrl);
        if (suCategoryImageURL) {
            return suCategoryImageURL;
        } else {
            suCategoryImageURL = getImage(parentCategoryUrl);
            if (suCategoryImageURL) {
                return suCategoryImageURL;
            }
        }
    }
    //sub-category check
  /*  let checkCategory = await imageExists(
        `${baseURL}/${pagePath}/${categoryUrl}-placeholder.jpg`
    );
    if (!checkCategory) {
        //parent category check
        let checkParentCategory = await imageExists(
            `${baseURL}/${pagePath}/${parentCategoryUrl}-placeholder.jpg`
        );
        if (!checkParentCategory) {
            url = `${baseURL}/${pagePath}/${placeHolderType}-placeholder.jpg`;
        } else {
            url = `${baseURL}/${pagePath}/${parentCategoryUrl}-placeholder.jpg`;
            pushImage(parentCategoryUrl, url, pagePath);
        }
    } else {
        url = `${baseURL}/${pagePath}/${categoryUrl}-placeholder.jpg`;
        pushImage(categoryUrl, url, pagePath);
    } */
    return url;
}

function getImage(slug) {
    let imageObject = data.image.find((val) => {
        return slug == val.slug
    });
    if (imageObject) {
        return imageObject.imageURL
    } else {
        return undefined
    }
}

function pushImage(slug, url, pagePath) {
    if (pagePath.indexOf('tiles') > -1) {
        var image: ImageClass = {
            slug: slug,
            imageURL: url
        };
        data.image.push(image);
    }
}

async function imageExists(url) {
    return new Promise((resolve, reject) => {
        var img = new Image();
        img.onload = function () {
            resolve(true);
        };
        img.onerror = function () {
            reject(false);
            console.log("Image not exists" + url)
        };
        img.src = url;
    });
}
