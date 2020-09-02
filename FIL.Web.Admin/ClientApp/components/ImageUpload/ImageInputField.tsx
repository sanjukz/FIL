import * as React from "react";
import { Modal } from 'antd';
import ImageCropper from "./ImageCropper";
import {
  getImageDataModel,
  ImageDetailModal,
  getCustomImageInputField,
  checkAndGetImageSource,
  IMG_PLACEHOLDER,
  ImageType
} from "./utils";

interface IImageInputFieldProps {
  imageType: ImageType;
  imageKey?: string;
  onImageSelect: (images: ImageDetailModal) => void;
  onImageRemove?: (images: ImageDetailModal) => void;
}

class ImageInputField extends React.PureComponent<IImageInputFieldProps, any> {

  imageInputElement: React.RefObject<HTMLInputElement>;

  constructor(props) {
    super(props);
    this.state = {
      customImageFieldAttributes: getCustomImageInputField(props.imageType),
      readImageSource: "",
      fallbackImage: IMG_PLACEHOLDER[props.imageType],
      isImageCropping: false,
      confirmMessage: ""
    };

    this.imageInputElement = React.createRef();
  }

  async componentDidMount() {
    if (this.props.imageKey) {
      let fallbackImage = await checkAndGetImageSource(this.props.imageType, this.props.imageKey);
      this.setState({
        fallbackImage
      });
    }
  }

  onImageClick = () => {
    let imageInputNode = this.imageInputElement.current;
    if (imageInputNode.files && imageInputNode.files[0]) {
      let fileSize = imageInputNode.files[0].size / 1024 / 1024;
      if (this.props.imageType == "tile" && fileSize > 1) {
        this.setState({ confirmMessage: 'Image size should be below 1MB' }, () => { this.error() });
        return;
      }

      if (this.props.imageType == "banner" && fileSize > 2) {
        this.setState({ confirmMessage: 'Image size should be below 2MB' }, () => { this.error() });
        return;
      }

      let reader = new FileReader();
      reader.onload = (e) => {
        this.setState({
          readImageSource: reader.result,
          isImageCropping: true,
        });
      };
      reader.readAsDataURL(imageInputNode.files[0]);
    }
  }

  onImageCrop = async (imageSourceUrl: string) => {
    this.setState({
      readImageSource: imageSourceUrl,
      isImageCropping: false
    });
    let file = await this.base64ToFile(imageSourceUrl);
    this.props.onImageSelect(getImageDataModel(this.props.imageType, file))
  };

  base64ToFile = (url) => {
    return fetch(url)
      .then(res => res.blob())
      .then(blob => {
        let file = new File([blob], "feel-image", { type: "image/jpeg" });
        return file;
      });
  };

  onTileClick = () => {
    this.imageInputElement.current.click();
  }

  error = () => {
    Modal.error({
      title: this.state.confirmMessage,
      centered: true
    });
  }

  componentWillUnmount() {
    this.imageInputElement.current.value = null;
  }

  render() {
    return (
      <>
        <input type="hidden" name="placemapImages" />
        <input
          ref={this.imageInputElement}
          onChange={this.onImageClick}
          onClick={(e) => this.imageInputElement.current.value = null}
          className="hidden"
          accept="image/*"
          type="file"
          id="img1"
          name="tilesSliderImages"
        />
        <span
          className={this.state.customImageFieldAttributes.inputClassName}
          onClick={this.onTileClick}
        >
          <img
            src={this.state.readImageSource == "" ?
              this.state.fallbackImage :
              this.state.readImageSource}
            alt="image thumbnail"
            width={this.state.customImageFieldAttributes.width}
            className={this.state.customImageFieldAttributes.className}
          />
          <div className={this.state.customImageFieldAttributes.innerTextClassName}> {this.state.customImageFieldAttributes.innerText}</div>
        </span>

        <div className={this.state.customImageFieldAttributes.messageClassName}>
          {this.state.customImageFieldAttributes.sizeMessage}
        </div>
        <ImageCropper
          onImageCrop={this.onImageCrop}
          imageSourceUrl={this.state.readImageSource}
          imageType={this.props.imageType}
          isImageCropping={this.state.isImageCropping} />
      </>
    );
  }
}

export default ImageInputField;
