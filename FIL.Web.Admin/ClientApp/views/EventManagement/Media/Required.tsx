/* Third party imports */
import * as React from "react";

/* Local imports */
import ImageUpload from '../../../components/ImageUpload/ImageUpload'
import { ImageDetailModal } from '../../../components/ImageUpload/utils'

export class Required extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      images: []
    }
  }

  render() {
    return (
      <div data-aos="fade-up" data-aos-duration="1000">< div id="Images" >
        <div className="row">
          <div className="col-sm-6">
            <ImageUpload
              imageInputList={[
                {
                  imageType: 'banner',
                  numberOfFields: 1,
                  imageKey: this.props.imageViewModel.eventImageModel.eventAltId.toUpperCase()
                }
              ]}
              onImageSelect={(item: ImageDetailModal) => {
                this.props.handleImageSelect(item)
              }}
            />
          </div>
          <div className="col-sm-6">
            <ImageUpload
              imageInputList={[
                {
                  imageType: 'tile',
                  numberOfFields: 1,
                  imageKey: this.props.imageViewModel.eventImageModel.eventAltId.toUpperCase()
                }
              ]}
              onImageSelect={(item: ImageDetailModal) => {
                this.props.handleImageSelect(item)
              }}
            />
          </div>
        </div>
      </div>
      </div>
    );
  }
}

