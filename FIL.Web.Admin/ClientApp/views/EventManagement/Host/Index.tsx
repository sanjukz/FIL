/* Third party imports */
import * as React from 'react'
import * as _ from 'lodash'

/* Local imports */
import HostForm from './HostForm'
import { EventHostsViewModel } from '../../../models/CreateEventV1/EventHostsViewModel';
import { DeleteEventHostViewModel } from '../../../models/CreateEventV1/DeleteEventHostViewModel';
import { setHostViewModelObject } from "../utils/DefaultObjectSetter";
import { uploadImage } from "../../../utils/S3Configuration"
import Spinner from "../../../components/Spinner";
import { showNotification } from "../Notification";
import { HostListing } from "./HostListing";
import { ListFooter } from "../Footer/ListFooter";
import { Error } from "../Modal";

export class Index extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
      isShowDrawer: false,
      selectedHostId: 0,
      eventHostViewModel: setHostViewModelObject(this.props.slug)
    }
  }

  componentDidMount() {
    if (this.props.slug) {
      this.props.props.requestEventHosts(this.props.slug, (response: EventHostsViewModel) => {
        if (response.success) {
          console.log(response);
          this.setState({ eventHostViewModel: response, isShowDrawer: response.eventHostMapping.length > 0 ? false : true });
        } else {
        }
      });
    } else {
    }
  }

  onDeleteHost = (altId: any) => {
    if (this.props.eventStatus == 6 && this.state.eventHostViewModel.eventHostMapping.length == 1) {
      Error('Event is live on site you need atleast one host.');
      return;
    }
    this.props.props.deleteHost(altId, 6, this.state.eventHostViewModel.eventHostMapping.length, (response: DeleteEventHostViewModel) => {
      if (response.success && !response.isHostStreamLinkCreated) {
        console.log(response);
        response.currentStep = this.state.eventHostViewModel.currentStep;
        let eventHostViewModel = this.state.eventHostViewModel;
        eventHostViewModel.eventHostMapping = eventHostViewModel.eventHostMapping.filter((val: any) => { return val.altId != altId });
        this.setState({ eventHostViewModel: eventHostViewModel, isShowDrawer: eventHostViewModel.eventHostMapping.length > 0 ? false : true })
        showNotification('success', `Host deleted successfully`);
        this.props.changeRoute(6, response.completedStep)
      } else if (response.isHostStreamLinkCreated) {
        showNotification('error', `You can't delete this host as stream link is already created for this host`);
      }
    });
  }

  isShowLoader = () => {
    return this.props.props.EventHosts.isLoading && !this.props.props.EventHosts.isSaveRequest
  }

  isShowForm = () => {
    return !this.props.props.EventHosts.isLoading && !this.state.isShowDrawer
  }

  render() {
    return (
      <>
        {this.isShowForm() && <div data-aos="fade-up" data-aos-duration="1000">
          <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="6">
            <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
              <h3 className="m-0 text-purple">Introducing your hosts</h3></nav>
            < HostListing
              eventHostViewModel={this.state.eventHostViewModel}
              onClickHost={(item: any) => {
                this.setState({ selectedHostId: item.id, isShowDrawer: true, imageFile: undefined })
              }}
              onDeleteHost={(item: any) => {
                this.onDeleteHost(item.altId)
              }} /></div>
        </div>}

        {this.isShowLoader() && <Spinner />}

        {this.isShowForm() && <ListFooter
          addText={"+ Add Another Host"}
          saveText={'Continue'}
          onClickAddNew={() => {
            this.setState({ isShowDrawer: true, selectedHostId: 0, imageFile: undefined })
          }}
          onClickContinue={() => {
            this.props.changeRoute(7, this.state.eventHostViewModel.completedStep, true);
          }}
        />}

        {this.state.isShowDrawer ? (
          <>

            <HostForm
              props={this.props.props}
              selectedHostId={this.state.selectedHostId}
              eventHostViewModel={this.state.eventHostViewModel}
              onClickCancel={() => { this.state.eventHostViewModel.eventHostMapping.length == 0 ? this.props.changeRoute(5, this.state.eventHostViewModel.completedStep, true) : this.setState({ isShowDrawer: false }) }}
              onSubmit={(item: any) => {
                let currentHost = [];
                item.importantInformation = '<li>Optimal internet speed to host your Backstage Pass is 50MBPS or higher. Please check your internet speed using websites such as <a target="_blank" href="https://www.speedtest.net/">speedtest.net</a></li><li>The pre-recorded video will play at the set time for each customer</li><li>Please click on host your backstage pass 15 minutes before the end of the performance. This is to ensure that you are Backstage and ready. Please check your camera angle and computer audio ahead of time</li><li>Customers with the Backstage Pass access will click on the Backstage Pass at the end of the performance. Some may click a little earlier as well. You will be able to see how many are in and use the Chat feature to let everyone know when you will start</li><li>Customers have been instructed to stay muted with their videos off to ensure minimal interference and optimal viewing</li><li>Please use the chat window to see what customers are asking if they are raising their hands to speak or ask a question</li><li>You can mute all participants from your controls by clicking on the Participant list in your controls. If you mute all, the participant will not be able to unmute and you will need to unmute each one 8.    You can also request video access to any participant and if s/he accepts, s/he will be visible one at a time or when s/he speaks</li><li>Please note that the internet connectivity at the participants end can impact the overall quality of the interaction and can result in some echo or feedback</li><li>The person speaking will be visible to all customers one at a time.</li>';
                currentHost.push(item);
                let eventHostViewModel: EventHostsViewModel = {
                  eventHostMapping: currentHost,
                  success: false,
                  eventId: this.props.slug ? +this.props.slug : 0,
                  isDraft: false,
                  isValidLink: false,
                  currentStep: 6
                }
                this.props.props.saveEventHosts(eventHostViewModel, (response: EventHostsViewModel) => {
                  if (response.success) {
                    let hosts = this.state.eventHostViewModel.eventHostMapping;
                    let isExists = false;
                    let altId = "";
                    hosts = hosts.map((val) => {
                      if (val.id == response.eventHostMapping[0].id) {
                        isExists = true;
                        altId = val.altId;
                        val = { ...response.eventHostMapping[0] }
                      }
                      return val;
                    })
                    if (!isExists) {
                      hosts.push(response.eventHostMapping[0]);
                      altId = response.eventHostMapping[0].altId;
                    }
                    response.eventHostMapping = hosts;
                    this.setState({ isShowDrawer: false, eventHostViewModel: response });
                    if (this.state.imageFile) {
                      uploadImage(this.state.imageFile, altId.toUpperCase());
                    }
                    this.props.changeRoute(6, response.completedStep)
                  } else {
                  }
                })
              }}
              onImageSelect={(item) => {
                this.setState({ imageFile: item });
              }}
            />
          </>
        ) : (
            <div></div>
          )}
      </>
    )
  }
}
