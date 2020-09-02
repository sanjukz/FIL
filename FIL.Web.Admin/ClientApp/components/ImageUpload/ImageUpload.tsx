import * as React from "react";
import { Tooltip } from 'antd';
import ImageInputField from "./ImageInputField";
import { getCustomImageInputField, ImageDetailModal, ImageType } from "./utils";

interface IImageFieldList {
  imageType: ImageType;
  numberOfFields: number;
  imageKey?: string;
}

interface IImageUploadProps {
  imageInputList: IImageFieldList[];
  onImageSelect: (images: ImageDetailModal) => void;
  onImageRemove?: (images: ImageDetailModal) => void;
}

const TOOLTIP_MSG = {
  tile: <p><span>This will display in the landing page of the Live Stream section of our site. Therefore, please select one that helps to draw people in. Please ensure adherence to the FeelitLIVE Additional Terms and Conditions.</span></p>,
  banner: <p><span>This is the banner for your event and will display on your event page. Hence a nice catchy attractive image is needed. Please ensure adherence to the FeelitLIVE Additional Terms and Conditions.</span></p>,
  user: <p><span>This will be used for avatar of the host.</span></p>,
};

class ImageUpload extends React.Component<IImageUploadProps, any> {
  shouldComponentUpdate(nextProps, nextState) {
    if (this.props.imageInputList[0].imageKey == nextProps.imageInputList[0].imageKey) {
      return false;
    }
    return true;
  }

  render() {
    return (
      <>
        {
          this.props.imageInputList.map((fields: IImageFieldList) => {
            return (
              <>
                <p key={fields.imageType} className="font-weight-bold">
                  {getCustomImageInputField(fields.imageType).label}
                  {TOOLTIP_MSG[fields.imageType] ? <Tooltip
                    title={TOOLTIP_MSG[fields.imageType]}>
                    <span><i className="fa fa-info-circle text-primary ml-2" ></i></span>
                  </Tooltip> : null}
                </p>
                {Array(fields.numberOfFields).fill(0).map((imageNumber, index) => {
                  return <ImageInputField
                    key={fields.imageKey}
                    imageType={fields.imageType}
                    onImageSelect={this.props.onImageSelect}
                    onImageRemove={this.props.onImageRemove}
                    imageKey={fields.imageKey}
                  />;
                })}
              </>
            )
          })
        }
      </>
    );
  }
}

export default ImageUpload;
