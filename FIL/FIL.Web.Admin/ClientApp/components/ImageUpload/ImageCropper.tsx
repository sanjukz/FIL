import * as React from 'react';
import Cropper from 'react-cropper';
import 'cropperjs/dist/cropper.css';
import { Modal, Button } from 'antd';
import { ImageType } from "./utils";

const CROP_ASPECT_RATIO = {
  tile: 508 / 435,
  banner: 64 / 19,
  user: 1,
  profile: 1,
  portrait: 101 / 145,
  addon: 57 / 40,
  sponsor: 1 / 1
}

interface IImageCropperProps {
  onImageCrop: (imageUrl: string) => void;
  imageSourceUrl: string;
  imageType: ImageType;
  isImageCropping: boolean;
}

class ImageCropper extends React.Component<IImageCropperProps, any> {
  cropper: React.RefObject<any>;
  constructor(props) {
    super(props);
    this.state = {
      imageSource: ""
    };
    this.cropper = React.createRef();
  }
  _crop = () => {
    this.setState({
      imageSource: this.cropper.current.getCroppedCanvas().toDataURL()
    });
  }

  render() {
    return (
      <Modal
        closable={false}
        visible={this.props.isImageCropping}
        onOk={(e) => this.props.onImageCrop(this.state.imageSource)}
        footer={[
          <Button
            className="btn-primary text-white"
            onClick={(e) => this.props.onImageCrop(this.state.imageSource)}>
            Crop
                    </Button>,
        ]}
      >
        <Cropper
          ref={this.cropper}
          src={this.props.imageSourceUrl}
          style={{ height: 300, width: '100%' }}
          aspectRatio={CROP_ASPECT_RATIO[this.props.imageType]}
          guides={false}
          crop={this._crop} />
      </Modal>
    );
  }
}

export default ImageCropper;
