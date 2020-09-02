/* Third party imports */
import * as React from 'react'
import * as _ from 'lodash'

/* Local imports */
import SponsorForm from './SponsorForm'
import { EventSponsorViewModel } from '../../../models/CreateEventV1/EventSponsorViewModel';
import { EventStepViewModel } from "../../../models/CreateEventV1/EventStepViewModel";
import { DeleteEventSponsorViewModel } from '../../../models/CreateEventV1/DeleteEventSponsorViewModel';
import { setSponsorObject, setStepObject } from "../utils/DefaultObjectSetter";
import { updateEventStep } from "../utils/apiCaller"
import { uploadImage } from "../../../utils/S3Configuration"
import Spinner from "../../../components/Spinner";
import { showNotification } from "../Notification";
import { SponsorListing } from "./SponsorListing";
import { ListFooter } from "../Footer/ListFooter";
import { Error } from "../Modal";
import { REQUIRED_FINANCE_STEP_ARRAY } from '../../../utils/Constant/Step';
import { getStepDisable } from '../../../utils/Step';

export class Index extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
      isShowForm: false,
      selectedHostId: 0,
      eventSponsorViewModel: setSponsorObject(this.props.slug)
    }
  }

  componentDidMount() {
    if (this.props.slug) {
      this.props.props.requestEventSponsor(this.props.slug, (response: EventSponsorViewModel) => {
        if (response.success) {
          console.log(response);
          this.setState({ eventSponsorViewModel: response, isShowForm: response.sponsorDetails.length > 0 ? false : true });
        }
      });
    }
  }

  onDeleteSponsor = (altId: any) => {
    this.props.props.deleteSponsor(altId, 10, this.state.eventSponsorViewModel.sponsorDetails.length, (response: DeleteEventSponsorViewModel) => {
      if (response.success) {
        console.log(response);
        response.currentStep = this.state.eventSponsorViewModel.currentStep;
        let eventSponsorViewModel = this.state.eventSponsorViewModel;
        eventSponsorViewModel.sponsorDetails = eventSponsorViewModel.sponsorDetails.filter((val: any) => { return val.altId != altId });
        this.setState({ eventSponsorViewModel: eventSponsorViewModel, isShowForm: eventSponsorViewModel.sponsorDetails.length > 0 ? false : true })
        showNotification('success', `Sponsor deleted successfully`);
        this.props.changeRoute(10, response.completedStep)
      } else if (!response.success) {
        showNotification('error', `Darn! There is some issue with sponsor delete action please try again.`);
      }
    });
  }

  updateEventStep = () => {
    this.props.changeRoute(9, null, true);
  }

  isShowLoader = () => {
    return (this.props.props.EventSponsor.isLoading || this.props.props.EventStep.isLoading) && !this.props.props.EventSponsor.isSaveRequest
  }

  isShowListing = () => {
    return !this.state.isShowForm && !this.props.props.EventStep.isLoading
  }

  render() {
    return (
      <>
        {(this.isShowListing() || (this.state.selectedSponsorId == 0 && this.state.eventSponsorViewModel.sponsorDetails.length > 0)) && <div data-aos="fade-up" data-aos-duration="1000">
          <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="8">
            <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
              <h3 className="m-0 text-purple">Your Sponsors</h3></nav>
            < SponsorListing
              eventSponsorViewModel={this.state.eventSponsorViewModel}
              onClickSponsor={(item: any) => {
                this.setState({ selectedSponsorId: item.id, isShowForm: true, imageFile: undefined })
              }}
              onDeleteSponsor={(item: any) => {
                this.onDeleteSponsor(item.altId)
              }} /></div></div>}

        {this.isShowLoader() && <Spinner />}

        {!this.state.isShowForm && <ListFooter
          addText={"+  Another Sponsor"}
          isShowContinue={getStepDisable(REQUIRED_FINANCE_STEP_ARRAY, this.props.completedStep)}
          onClickAddNew={() => {
            this.setState({ imageFile: undefined, isShowForm: true, selectedSponsorId: 0 });
          }}
          onClickContinue={() => {
            this.props.changeRoute(9, this.state.eventSponsorViewModel.completedStep, true);
          }}
        />}
        {this.state.isShowForm ? (
          <SponsorForm
            props={this.props.props}
            selectedSponsorId={this.state.selectedSponsorId}
            eventSponsorViewModel={this.state.eventSponsorViewModel}
            completedStep={this.props.completedStep}
            onClickCancel={() => {
              this.setState({ isShowForm: false }, () => {
                if (this.state.eventSponsorViewModel.sponsorDetails.length == 0) {
                  this.props.changeRoute(11, this.state.eventSponsorViewModel.completedStep, true)
                }
              })
            }}
            onSubmit={(item: any) => {
              let currentSponsor = [];
              currentSponsor.push(item);
              let eventSponsorViewModel: EventSponsorViewModel = {
                sponsorDetails: currentSponsor,
                success: false,
                eventId: this.props.slug ? +this.props.slug : 0,
                isDraft: false,
                isValidLink: false,
                currentStep: 10
              }
              this.props.props.saveEventSponsor(eventSponsorViewModel, (response: EventSponsorViewModel) => {
                if (response.success) {
                  let sponsors = this.state.eventSponsorViewModel.sponsorDetails;
                  let isExists = false;
                  let altId = "";
                  sponsors = sponsors.map((val) => {
                    if (val.id == response.sponsorDetails[0].id) {
                      isExists = true;
                      altId = val.altId;
                      val = { ...response.sponsorDetails[0] }
                    }
                    return val;
                  })
                  if (!isExists) {
                    sponsors.push(response.sponsorDetails[0]);
                    altId = response.sponsorDetails[0].altId;
                  }
                  response.sponsorDetails = sponsors;
                  this.setState({ isShowForm: false, eventSponsorViewModel: response });
                  if (this.state.imageFile) {
                    uploadImage(this.state.imageFile, `${altId.toUpperCase()}-sponsor`);
                  }
                  this.props.changeRoute(10, response.completedStep)
                } else {
                }
              })
            }}
            onClickSkip={() => {
              this.setState({ isShowForm: false }, () => {
                this.updateEventStep();
              })
            }}
            onImageSelect={(item) => {
              this.setState({ imageFile: item });
            }}
          />
        ) : (
            <div></div>
          )}
      </>
    )
  }
}
