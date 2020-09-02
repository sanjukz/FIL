/* Third party imports */
import * as React from "react";

/* Local imports */
import { ImageDetailModal } from '../../../components/ImageUpload/utils'
import { ImageViewModel } from "../../../models/CreateEventV1/ImageViewModel";
import { S3_ACTION, uploadFile, deleteFile } from "../../../utils/S3Configuration"
import { setImageObject } from "../utils/DefaultObjectSetter"
import { Footer } from "../Footer/FormFooter";
import Spinner from "../../../components/Spinner/index";
import { Required } from './Required';
import { Optional } from './Optional';
import { checkAndGetImageSource } from "../../../components/ImageUpload/utils";

export class Index extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      imageViewModel: setImageObject(4, this.props.slug),
      isShowVideo: true,
      isVideoUploadRequest: false,
      progress: 0,
      images: []
    }
  }

  componentDidMount() {
    if (this.props.slug) {
      this.props.props.requestEventImage(this.props.slug, (response: ImageViewModel) => {
        if (response.success) {
          console.log(response);
          this.setState({ imageViewModel: response, isShowVideo: response.eventImageModel.isVideoUploaded });
          checkAndGetImageSource("tile", response.eventImageModel.eventAltId).then((val) => {
            if (val.indexOf(response.eventImageModel.eventAltId.toUpperCase()) > -1) {
              this.setState({ isImageUploaded: true })
            }
          });
        }
      });
    }
  }

  uploadImage = () => {
    let images = this.state.images;
    if (images.length >= 1) {
      images.forEach((item) => {
        var blob = item.file.slice(0, item.file.size, 'image/png')
        let filePath =
          item.path.indexOf('tiles') > -1
            ? `${this.state.imageViewModel.eventImageModel.eventAltId.toUpperCase()}-ht-c1.jpg`
            : item.path.indexOf('about') > -1
              ? `${this.state.imageViewModel.eventImageModel.eventAltId.toUpperCase()}-about.jpg`
              : `${this.state.imageViewModel.eventImageModel.eventAltId.toUpperCase()}.jpg`
        var newFile = new File([blob], `${filePath}`, { type: 'image/png' })
        this.uploadFile(item.path, newFile)
      })
    }
    this.onSubmitEventImage()
  }

  updateFileToS3 = (videoModel: ImageDetailModal, actionType: S3_ACTION) => {
    let currentVideo = videoModel;
    let imageViewModel = this.state.imageViewModel;
    if (S3_ACTION.upload == actionType) {
      var blob = currentVideo.file.originFileObj.slice(0, currentVideo.file.size, 'video/**')
      var newFile = new File([blob], `${currentVideo.id}.mp4`, { type: 'video/**' })
      this.setState({ isVideoUploadRequest: true }, () => {
        this.uploadFile(videoModel.path, newFile, true)
      })
    } else if (S3_ACTION.delete == actionType) {
      imageViewModel.eventImageModel.isVideoUploaded = false;
    }
    this.setState({ imageViewModel: imageViewModel, isShowVideo: S3_ACTION.delete == actionType ? false : this.state.isShowVideo })
  }

  handleImageSelect = (item: ImageDetailModal) => {
    let imageViewModel = this.state.imageViewModel;
    imageViewModel.eventImageModel.isBannerImage = true;
    imageViewModel.eventImageModel.isHotTicketImage = true;
    imageViewModel.eventImageModel.isPortraitImage = item.path.indexOf('portrait') > -1 ? true : imageViewModel.eventImageModel.isPortraitImage;
    let images = [item]
    let imageList = [...this.state.images || []].filter((t) => t.id != item.id)
    if (item.id == 'descbanner') {
      let temp = { ...item }
      temp.path = 'images/places/about'
      images.push(temp)
    }
    this.setState({
      imageViewModel: imageViewModel,
      images: [...imageList, ...images]
    })
  }

  uploadFile = (path, file, isVideoUploadRequest?: boolean) => {
    uploadFile(path, file, (e: any) => {
      if (e) {
        console.log(e);
        if (isVideoUploadRequest) {
          let imageViewModel = this.state.imageViewModel;
          imageViewModel.eventImageModel.isVideoUploaded = true;
          this.setState({ imageViewModel: imageViewModel, isVideoUploadRequest: false, isShowVideo: true });
        }
      } else {
      }
    })
  }

  onSubmitEventImage = () => {
    let imageViewModel = this.state.imageViewModel;
    imageViewModel.currentStep = 4;
    this.props.props.saveEventImages(imageViewModel, (response: ImageViewModel) => {
      if (response.success) {
        this.props.changeRoute(5, response.completedStep);
      }
    })
  }

  render() {
    return (
      <>
        {
          this.state.imageViewModel.eventImageModel.eventAltId ?
            <>

              <div data-aos="fade-up" data-aos-duration="1000">
                <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="3">
                  <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
                    <h3 className="m-0 text-purple">Making it look good</h3></nav>
                  < div id="Images" >
                    <Required
                      imageViewModel={this.state.imageViewModel}
                      handleImageSelect={(image: ImageDetailModal) => {
                        this.handleImageSelect(image);
                      }}
                    />
                  </div>
                </div>

                <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box mt-4" id="nav-tabContent" key="3">
                  <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
                    <h3 className="m-0 text-purple ">Make it even better (optional visuals)</h3></nav>
                  <Optional
                    imageViewModel={this.state.imageViewModel}
                    isShowVideo={this.state.isShowVideo}
                    isVideoUploaded={this.state.imageViewModel.eventImageModel.isVideoUploaded}
                    eventAltId={this.state.imageViewModel.eventImageModel.eventAltId}
                    isVideoUploadRequest={this.state.isVideoUploadRequest}
                    progress={this.state.progress}
                    videoPath={`videos/banner/${this.state.imageViewModel.eventImageModel.eventAltId.toUpperCase()}.mp4`}
                    fileName={this.state.file ? this.state.file.name : ""}
                    handleImageSelect={(image: ImageDetailModal) => {
                      this.handleImageSelect(image);
                    }}
                    onRemoveVideo={() => {
                      let imageViewModel = this.state.imageViewModel;
                      imageViewModel.eventImageModel.isVideoUploaded = false;
                      this.setState({ imageViewModel: imageViewModel, isShowVideo: false });
                    }}
                    onChange={(isShowVideo: boolean) => {
                      this.setState({ isShowVideo: isShowVideo });
                    }}
                    onProgress={(progress) => {
                      this.setState({ progress: progress })
                    }}
                    onUploadRequest={(file: any) => {
                      this.setState({ isVideoUploadRequest: true, file: file });
                    }}
                    onUploadSuccess={() => {
                      let imageViewModel = this.state.imageViewModel;
                      imageViewModel.eventImageModel.isVideoUploaded = true;
                      this.setState({ imageViewModel: imageViewModel, isVideoUploadRequest: false, isShowVideo: true });
                    }}
                  />
                </div>
                <Footer
                  onClickCancel={() => { this.props.changeRoute(3); }}
                  isDisabled={(this.state.images.length > 2 || this.state.isImageUploaded) ? false : true}
                  isSaveRequest={this.props.props.EventImage.isSaveRequest}
                  onSubmit={() => { this.uploadImage() }} />
              </div></> : <Spinner />
        }
      </>
    );
  }
}

