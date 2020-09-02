/* Third party imports */
import * as React from "react";
import { Upload, message, Progress } from 'antd';
import { s3Prop } from '../../../utils/S3Configuration';;
import { LoadingOutlined, PlusOutlined, EditTwoTone, DeleteTwoTone } from '@ant-design/icons';
import AWS from "aws-sdk";

/* Local imports */
import ImageUpload from '../../../components/ImageUpload/ImageUpload'
import { ImageDetailModal } from '../../../components/ImageUpload/utils'
import { S3_ACTION } from "../../../utils/S3Configuration"

export class Optional extends React.Component<any, any> {
  videoInputElement: React.RefObject<HTMLInputElement>;
  constructor(props) {
    super(props);
    this.state = {
      progress: 0
    }
    this.videoInputElement = React.createRef();
  }

  render() {
    let data = AWS;
    return (
      <div className="row">
        <div className="col-sm-6">
          {(!this.props.isShowVideo && !this.props.isVideoUploadRequest) && <div>
            {(!this.props.isPerformanceTab) && <p className="font-weight-bold">Upload Video Snippet</p>}
            <Upload
              {...s3Prop}
              showUploadList={false}
              accept="video/mp4,video/x-m4v,video/*,.mkv"
              name="avatar"
              listType="picture-card"
              className="avatar-uploader w-auto"
              data={{
                altId: this.props.eventAltId.toUpperCase(),
                path: this.props.videoPath,
                onUploadRequest: (file: any) => {
                  this.props.onUploadRequest(file);
                },
                onUploadSuccess: () => {
                  this.props.onUploadSuccess();
                },
                onProgress: (progress: any) => {
                  this.props.onProgress(progress)
                },
                onError: (erroMsg: any) => {
                  this.setState({ erroMsg: erroMsg });
                }
              }}
            >
              <div>
                {this.state.loading ? <LoadingOutlined /> : <></>}
                <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/upload-video.svg" />
                {(!this.props.isPerformanceTab) && <div className="text-muted">Max 30 secs</div>}
              </div>
            </Upload>
            {(!this.props.isPerformanceTab) && <div className="d-inline-block portait-img-txt f-14">We recommend using video shot on landscape mode (4:3 ratio), no longer than 30 sec, and no larger than 4mb.</div>}
            {(this.props.isVideoUploaded) &&
              <div><a href="javascript:Void(0)" onClick={() => { this.props.onChange(true); }} >Back to video</a></div>
            }
          </div>}

          {(this.props.isShowVideo && this.props.isVideoUploaded && !this.props.isVideoUploadRequest) && <div>
            <video autoPlay controls height="200" width="300">
              <source src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/${this.props.videoPath}`} id="video_here" />
            </video>
            <div>
              <EditTwoTone className="mr-4" onClick={() => { this.props.onChange(false); }} />
              <DeleteTwoTone onClick={() => { this.props.onRemoveVideo() }} />
            </div>
          </div>}

          {(this.props.isVideoUploadRequest) &&
            <>
              <div>Uploading {this.props.fileName}</div>
              <Progress type="circle" percent={this.props.progress} />
            </>
          }
        </div>
        {(!this.props.isPerformanceTab) && <div className="col-sm-6">
          <ImageUpload
            imageInputList={[
              {
                imageType: 'portrait',
                numberOfFields: 1,
                imageKey: this.props.eventAltId
              }
            ]}
            onImageSelect={(item: ImageDetailModal) => {
              this.props.handleImageSelect(item)
            }}
          />
        </div>}
      </div>
    );
  }
}

