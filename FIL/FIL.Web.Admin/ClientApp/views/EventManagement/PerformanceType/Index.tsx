/* Third party imports */
import * as React from "react";
import { Checkbox } from 'antd';

/*Local imports */
import { EventPerformanceViewModel } from "../../../models/CreateEventV1/EventPerformanceViewModel";
import { performanceTypeModel, getFlagStatus } from "../../../utils/PerformanceType"
import { Footer } from "../Footer/FormFooter";
import { showNotification } from "../Notification";
import Spinner from "../../../components/Spinner";
import { setPerformanceTypeObject } from "../utils/DefaultObjectSetter";
import { EventFrequencyType } from '../../../Enums/EventFrequencyType';
import { Optional } from '../Media/Optional';
import { S3_ACTION } from "../../../utils/S3Configuration"

export class Index extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      eventPerformanceViewModel: setPerformanceTypeObject(this.props.slug),
      isShowVideo: this.props.isShowVideo,
      isVideoUploadRequest: this.props.isVideoUploadRequest,
      progress: this.props.progress,
      performanceTypeModel: performanceTypeModel
    }
  }

  componentDidMount() {
    if (this.props.slug) {
      this.props.props.requestEventPerformance(this.props.slug, (response: EventPerformanceViewModel) => {
        if (response.success) {
          console.log(response);
          this.setState({ eventPerformanceViewModel: response, isShowVideo: response.performanceTypeModel ? response.performanceTypeModel.isVideoUploaded : false }, () => {
            response.currentStep = this.state.eventPerformanceViewModel.currentStep;
            let performaceTypeModel = this.state.performanceTypeModel;
            performaceTypeModel.map((val) => {
              val.isChecked = this.getFlagStatus(val);
            })
            this.setState({ performaceTypeModel: performaceTypeModel });
          });
        } else {
          let eventPerformanceViewModel = this.state.eventPerformanceViewModel;
          eventPerformanceViewModel.eventFrequencyType = response.eventFrequencyType;
          eventPerformanceViewModel.eventAltId = response.eventAltId;
          this.setState({ performaceTypeModel: eventPerformanceViewModel });
        }
      });
    }
  }

  getPerformanceFlagValue = (eventPerformanceViewModel) => {
    let performanceModelState = this.state.performanceTypeModel;
    let performanceValue = 0;
    performanceModelState.map((val: any) => {
      if (val.isChecked && val.performanceTypeId == eventPerformanceViewModel.performanceTypeModel.performanceTypeId) {
        performanceValue += val.onlineEventTypeId
      }
    });
    return performanceValue;
  }

  getFlagStatus = (currentPerformanceType) => {
    let performanceValue = this.state.eventPerformanceViewModel.performanceTypeModel.onlineEventTypeId;
    return getFlagStatus(currentPerformanceType, performanceValue)
  }

  setFlagStatus = () => {
    const eventPerformanceViewModel = this.state.eventPerformanceViewModel;
    eventPerformanceViewModel.performanceTypeModel.onlineEventTypeId = this.getPerformanceFlagValue(eventPerformanceViewModel);
    this.setState({ performanceTypeModel: performanceTypeModel });
  }

  getCurrentPerformanceModel = () => {
    return this.state.performanceTypeModel.filter((val: any) => { return val.performanceTypeId == this.state.eventPerformanceViewModel.performanceTypeModel.performanceTypeId })
  }

  isButtonDisable = () => {
    const eventPerformanceViewModel = this.getCurrentPerformanceModel();
    if (eventPerformanceViewModel.filter((val) => val.isChecked).length > 0) {
      return false
    } else {
      return true;
    }
  }

  onSubmit = () => {
    let saveEventPerformanceViewModel = this.state.eventPerformanceViewModel;
    this.props.props.saveEventPerformance(saveEventPerformanceViewModel, (response: EventPerformanceViewModel) => {
      if (response.success) {
        this.props.changeRoute(6, response.completedStep);
      } else {
        showNotification('error', 'Darn! Some error with saving performance details. Please try again.')
      }
    })
  }

  confirm = () => {
    var confirm = window.confirm('Video upload is in progress, do you want to cancel the upload?')
    if (confirm) {
      return true
    }
  }

  render() {
    let selectedPerformanceModel = this.getCurrentPerformanceModel();
    let perFormanceJSX = selectedPerformanceModel.map((val, index) => {
      return <div
        className="custom-control custom-checkbox d-inline-block p-0 pr-4 align-top "
      >
        <Checkbox
          checked={this.getFlagStatus(val)}
          disabled={this.state.eventPerformanceViewModel.eventFrequencyType == EventFrequencyType.OnDemand ? (val.index == 0 || val.index == 2 ? false : true) : false}
          onChange={(e) => {
            let eventPerformanceViewModel = this.state.eventPerformanceViewModel;
            let performanceTypeModel = this.state.performanceTypeModel;
            eventPerformanceViewModel.performanceTypeModel.onlineEventTypeId = val.onlineEventTypeId;
            performanceTypeModel[val.index].isChecked = e.target.checked;
            this.setState({
              eventPerformanceViewModel: eventPerformanceViewModel,
              performanceTypeModel: performanceTypeModel
            }, () => {
              this.setFlagStatus();
            });
          }}
        >
          {val.name}
        </Checkbox>
      </div>
    })
    return (
      <div data-aos="fade-up" data-aos-duration="1000">
        {!this.props.props.EventPerformance.isLoading || this.props.props.EventPerformance.isSaveRequest ?
          <div data-aos="fade-up" data-aos-duration="1000">
            <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="4">
              <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
                <h3 className="m-0 text-purple">Defining your performance type</h3></nav>
              <h5>Performance  type</h5>
              <p className="d-block clearfix">
                What kind of performance do you wish to conduct?
                </p>
              <div className="btn-group custom-toggle-btn mb-4" role="group" aria-label="Basic example">
                <button
                  onClick={(e: any) => {
                    let eventPerformanceViewModel = this.state.eventPerformanceViewModel;
                    eventPerformanceViewModel.performanceTypeModel.performanceTypeId = 1;
                    this.setState({ eventPerformanceViewModel: eventPerformanceViewModel }, () => { this.setFlagStatus() })
                  }}
                  type="button"
                  className={
                    this.state.eventPerformanceViewModel.performanceTypeModel.performanceTypeId == 1
                      ? 'btn btn-outline-primary active'
                      : 'btn btn-outline-primary'
                  }
                >
                  Individual
                </button>
                <button
                  onClick={(e: any) => {
                    let eventPerformanceViewModel = this.state.eventPerformanceViewModel;
                    eventPerformanceViewModel.performanceTypeModel.performanceTypeId = 2;
                    this.setState({ eventPerformanceViewModel: eventPerformanceViewModel }, () => { this.setFlagStatus() })
                  }} type="button"
                  className={
                    this.state.eventPerformanceViewModel.performanceTypeModel.performanceTypeId == 2
                      ? 'btn btn-outline-primary active'
                      : 'btn btn-outline-primary'
                  }
                >
                  Group
               </button>
              </div>
              <div className="row">
                <div className="col-12 mt-3">
                  {perFormanceJSX}
                </div>
              </div>
              {this.state.eventPerformanceViewModel.performanceTypeModel.onlineEventTypeId % 2 != 0 && <>
                <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
                  <h5>Upload video</h5></nav>
                <Optional
                  isShowVideo={this.state.isShowVideo || false}
                  isPerformanceTab={true}
                  isVideoUploaded={this.state.eventPerformanceViewModel.performanceTypeModel.isVideoUploaded}
                  eventAltId={this.state.eventPerformanceViewModel.eventAltId}
                  fileName={this.props.file ? this.props.file.name : ''}
                  isVideoUploadRequest={this.state.isVideoUploadRequest}
                  videoPath={`videos/performance/${this.state.eventPerformanceViewModel.eventAltId.toUpperCase()}.mp4`}
                  progress={this.props.progress || 0}
                  onChange={(isShowVideo: boolean) => {
                    this.setState({ isShowVideo: isShowVideo });
                  }}
                  onRemoveVideo={() => {
                    let eventPerformanceViewModel = this.state.eventPerformanceViewModel;
                    eventPerformanceViewModel.performanceTypeModel.isVideoUploaded = false;
                    this.setState({ eventPerformanceViewModel: eventPerformanceViewModel, isShowVideo: false });
                  }}
                  onUploadRequest={(file: any) => {
                    this.setState({ isVideoUploadRequest: true })
                    this.props.onUploadRequest(file)
                  }}
                  onProgress={(progress) => {
                    this.setState({ progress: progress })
                    this.props.onProgress(progress)
                  }}
                  onUploadSuccess={() => {
                    let eventPerformanceViewModel = this.state.eventPerformanceViewModel;
                    eventPerformanceViewModel.performanceTypeModel.isVideoUploaded = true;
                    this.setState({
                      eventPerformanceViewModel: eventPerformanceViewModel,
                      isVideoUploadRequest: false,
                      isShowVideo: true
                    }, () => {
                      this.props.onUploadSuccess()
                    });
                  }}
                />
              </>}
            </div>
            <Footer
              onClickCancel={() => {
                if (this.state.isVideoUploadRequest) {
                  if (this.confirm()) {
                    this.props.changeRoute(4);
                  }
                } else {
                  this.props.changeRoute(4);
                }
              }}
              isDisabled={this.isButtonDisable() || (this.props.props.StepDetails.stepDetails.eventStatus == 6 && this.props.props.StepDetails.stepDetails.isTransacted)}
              isSaveRequest={this.props.props.EventPerformance.isSaveRequest}
              onSubmit={() => {
                if (this.state.isVideoUploadRequest) {
                  if (this.confirm()) {
                    this.onSubmit();
                  }
                } else {
                  this.onSubmit();
                }
              }} />
          </div> : <Spinner />
        }
      </div>
    );
  }
}

