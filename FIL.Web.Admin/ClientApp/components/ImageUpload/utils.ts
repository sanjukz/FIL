export type ImageType = "tile" | "banner" | "user" | "profile" | "addon" | "sponsor" | "portrait";

export class ImageDetailModal {
  id: string;
  file: any;
  path: string;
}

export class CustomImageInputField {
  sizeMessage: string;
  width: string;
  height: string;
  label: string;
  className: string;
  innerText: string;
  innerTextClassName: string;
  messageClassName: string;
  inputClassName: string;
}

export function getImageDataModel(imageType: ImageType, image: any): ImageDetailModal {
  if (imageType == "tile") {
    let imageTiles: ImageDetailModal = {
      id: "img1View",
      file: image,
      path: 'images/places/tiles'
    };
    return imageTiles;
  }

  if (imageType == "banner") {
    let imageBanner: ImageDetailModal = {
      id: "descbanner",
      file: image,
      path: 'images/places/InnerBanner'
    };
    return imageBanner;
  }

  if (imageType == "user") {
    let imageUser: ImageDetailModal = {
      id: "",
      file: image,
      path: 'images/eventHost'
    };
    return imageUser;
  }

  if (imageType == "addon") {
    let imageUser: ImageDetailModal = {
      id: "",
      file: image,
      path: 'images/add-ons'
    };
    return imageUser;
  }

  if (imageType == "sponsor") {
    let imageUser: ImageDetailModal = {
      id: "",
      file: image,
      path: 'images/sponsor'
    };
    return imageUser;
  }

  if (imageType == "profile") {
    let imageUser: ImageDetailModal = {
      id: "",
      file: image,
      path: ''
    };
    return imageUser;
  }

  if (imageType == "portrait") {
    let imageUser: ImageDetailModal = {
      id: "",
      file: image,
      path: 'images/places/portrait'
    };
    return imageUser;
  }
}

// default image placeholder paths

export const IMG_PLACEHOLDER = {
  tile: "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-upload-img.svg",
  portrait: "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-upload-img.svg",
  banner: "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-upload-img.svg",
  user: "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/user-profile/fee-review-user-icon.png",
  addon: "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-upload-img.svg",
  sponsor: "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-upload-img.svg",
  profile: "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/user-profile/fee-review-user-icon.png"
}

// returns image path if an image is already uploaded or returns fallback image

export async function checkAndGetImageSource(imageType: ImageType, imageKey: string): Promise<string> {
  let imgSrc = "";
  if (imageType == "tile") {
    imgSrc = `https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/places/tiles/${imageKey.toUpperCase()}-ht-c1.jpg`;
  } else if (imageType == "banner") {
    imgSrc = `https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/places/about/${imageKey.toUpperCase()}-about.jpg`;
  } else if (imageType == "user") {
    imgSrc = `https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/eventHost/${imageKey.toUpperCase()}.jpg`;
  } else if (imageType == "addon") {
    imgSrc = `https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/add-ons/${imageKey.toUpperCase()}-add-ons.jpg`;
  } else if (imageType == "sponsor") {
    imgSrc = `https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/sponsor/${imageKey.toUpperCase()}-sponsor.jpg`;
  } else if (imageType == "portrait") {
    imgSrc = `https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/places/portrait/${imageKey.toUpperCase()}.jpg`;
  } else {
    imgSrc = `https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/ProfilePictures/${imageKey.toUpperCase()}.jpg`;
  }

  let img = new Image();
  img.src = imgSrc;

  try {
    await img.decode()
    return imgSrc;
  } catch {
    return IMG_PLACEHOLDER[imageType];
  }
}

export function getCustomImageInputField(imageType: ImageType): CustomImageInputField {
  switch (imageType) {
    case "banner":
      return {
        label: "Banner Image",
        sizeMessage: "We recommend using at least a 1240 x 870px image that's no larger than 1mb.",
        width: "40",
        height: "",
        className: "",
        innerText: "1240 x 870px",
        innerTextClassName: "d-inline-block pl-4 text-muted",
        messageClassName: "mt-3 f-14",
        inputClassName: "hyperref rounded-box p-4 border"
      };

    case "tile":
      return {
        label: "Listing Image",
        sizeMessage: "We recommend using at least a 1016 x 870px  image that's no larger than 1mb.",
        width: "40",
        height: "89",
        className: "",
        innerText: "1016 x 870px",
        innerTextClassName: "d-inline-block pl-4 text-muted",
        messageClassName: "mt-3 f-14",
        inputClassName: "hyperref rounded-box p-4 border"
      };

    case "user":
      return {
        label: "Host Image",
        sizeMessage: "We recommend using at least a 300px x 300px image that's no larger than 1mb.",
        width: "80",
        height: "80",
        className: "rounded-circle",
        innerText: "",
        innerTextClassName: "",
        messageClassName: "",
        inputClassName: ""
      };
    case "profile":
      return {
        label: "",
        sizeMessage: "",
        width: "60",
        height: "60",
        className: "rounded-circle mx-auto",
        innerText: "",
        innerTextClassName: "",
        messageClassName: "",
        inputClassName: ""
      };
    case "addon":
      return {
        label: "Add-on Image",
        sizeMessage: "We recommend using at least a 570 x 400px image that's no larger than 1mb.",
        width: "40",
        height: "",
        className: "",
        innerText: "570 x 400px",
        innerTextClassName: "d-inline-block pl-4 text-muted",
        messageClassName: "mt-3 f-14",
        inputClassName: "hyperref rounded-box p-4 border"
      };
    case "sponsor":
      return {
        label: "Logo",
        sizeMessage: "We recommend using at least a 400px x 400px image that's no larger than 1mb.",
        width: "40",
        height: "",
        className: "",
        innerText: "400 x 400px",
        innerTextClassName: "d-inline-block pl-4 text-muted",
        messageClassName: "mt-3 f-14",
        inputClassName: "hyperref rounded-box p-4 border"
      };
    case "portrait":
      return {
        label: "Portrait Image",
        sizeMessage: "We recommend using at least a 606 x 870px image that's no larger than 1mb.",
        width: "40",
        height: "",
        className: "",
        innerText: "606px x 870px",
        innerTextClassName: "pt-2 text-muted",
        messageClassName: "d-inline-block portait-img-txt f-14",
        inputClassName: "hyperref rounded-box py-5 px-3 border text-center"
      };
    default:
      return {
        label: "",
        sizeMessage: "",
        width: "",
        height: "",
        className: "rounded mr-1",
        innerText: "",
        innerTextClassName: "",
        messageClassName: "",
        inputClassName: ""
      };
  }
}

const imagePixelRange = {
  tile: {
    minWidth: 570,
    minHeight: 400,
    maxWidth: 570,
    maxHeight: 400
  },
  banner: {
    minWidth: 1900,
    minHeight: 400,
    maxWidth: 1920,
    maxHeight: 570
  },
  user: {
    minWidth: 200,
    minHeight: 200,
    maxWidth: 800,
    maxHeight: 800
  },
  addon: {
    minWidth: 570,
    minHeight: 400,
    maxWidth: 570,
    maxHeight: 400
  },
  sponsor: {
    minWidth: 400,
    minHeight: 400,
    maxWidth: 400,
    maxHeight: 400
  },
  portrait: {
    minWidth: 615,
    minHeight: 870,
    maxWidth: 615,
    maxHeight: 870
  },
};

export function imagePixelValidation(imageType: ImageType, width: number, height: number): boolean {
  if (width > imagePixelRange[imageType].maxWidth ||
    width < imagePixelRange[imageType].minWidth ||
    height > imagePixelRange[imageType].maxHeight ||
    height < imagePixelRange[imageType].minHeight) {
    return false;
  } else return true
}
