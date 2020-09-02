/*Third Party Imports */
import * as React from 'react'

/*Local Imports */
import Spinner from "../../components/Spinner";
import { getStepStatus, getDisableStatus } from '../../utils/Step';


export class Steps extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      stepModel: this.props.stepModel
    }
  }

  render() {
    if (this.state.stepModel) {
      let leftSteps = this.state.stepModel.stepModel.map((val, index) => {
        /* If event status is published and step is submission then don't show the step */
        if (this.state.stepModel.eventStatus == 6 && val.stepId == 12) {
          return;
        }
        let isDisabledClass = getDisableStatus(this.state.stepModel.completedStep, this.state.stepModel.stepModel, val, this.props.slug) ? "disabled" : "";
        let isCompleted = getStepStatus(this.props, val, this.state.stepModel.currentStep);

        isDisabledClass += isCompleted ? " sub-menu-check" : "";
        let isActive = val.stepId == this.state.stepModel.currentStep ? true : false;
        let iconUrl = isActive ? `${val.name.toLowerCase().replace(/ /g, "-")}-active` : `${val.name.toLowerCase().replace(/ /g, "-")}`;
        isDisabledClass += isActive ? " active" : " ";

        return <a href="javascript:void(0)" key={index} className={`pb-3 ${isDisabledClass}`}
          onClick={(e) => {
            if (!getDisableStatus(this.state.stepModel.completedStep, this.state.stepModel.stepModel, val, this.props.slug)) {
              let stateModel = this.props.stepModel;
              let selectedStateModel = this.props.stepModel.stepModel.filter((val, indx) => { return index == indx })[0];
              stateModel.currentStep = +selectedStateModel.stepId;
              this.setState({ stateModel: stateModel }, () => {
                if (this.props.isVideoUploadRequest && this.state.stepModel.currentStep != 5) {
                  var confirm = window.confirm('Video upload is in progress, do you want to cancel the upload?')
                  if (confirm) {
                    this.props.history.push(`${selectedStateModel.slug}`)
                    this.props.onCancelVideoUpload()
                  }
                } else {
                  this.props.history.push(`${selectedStateModel.slug}`)
                }
              })
            }
          }}
          aria-disabled={getDisableStatus(this.state.stepModel.completedStep, this.state.stepModel.stepModel, val, this.props.slug)}
        ><span className="nav-icon">
            <img src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/sub-menu/${iconUrl}.svg`}
              alt={`${val.name} Icon`} width="13" /></span>{val.name}</a>
      })
      return <>
        {this.props.component ?
          // <div data-aos="fade-up" data-aos-duration="1000">
          <div className="card border-0 bg-light left-main-nav d-none d-md-block" id="fil-collapse-left-menu">
            <div className="card-body p-0">
              <div className="list-group fil-sub-menu">
                {leftSteps}
              </div>
            </div>
          </div>
          // </div>
          : <Spinner />}
      </>
    } else {
      return <Spinner />
    }
  }
}

