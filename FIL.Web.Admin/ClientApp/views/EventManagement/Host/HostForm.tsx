/* Third party imports */
import * as React from 'react'
import * as _ from 'lodash'
import CKEditor from 'react-ckeditor-component'
import { Switch } from 'antd'
import axios from 'axios'

/* Local imports */
import ImageUpload from '../../../components/ImageUpload/ImageUpload'
import { Footer } from "../Footer/FormFooter";
import { ToolTip } from "../ToolTip";
import { setHostModelObject } from "../utils/DefaultObjectSetter";
import FeelItLiveHostResponse from '../../../models/FeelItLive/FeelITLiveHost'
import Spinner from "../../../components/Spinner";

export default class HostForm extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
      host: setHostModelObject(this.props.props.slug),
      checked: false
    }
  }
  form: any;

  componentDidMount() {
    if (this.props.selectedHostId != 0) {
      let selectedHost = this.props.eventHostViewModel.eventHostMapping.filter((val: any) => { return val.id == this.props.selectedHostId });
      if (selectedHost.length > 0) {
        this.setState({ host: { ...selectedHost[0] } });
      }
    }
  }

  isButtonDisable = () => {
    var pattern = /^[a-zA-Z0-9\-_]+(\.[a-zA-Z0-9\-_]+)*@[a-z0-9]+(\-[a-z0-9]+)*(\.[a-z0-9]+(\-[a-z0-9]+)*)*\.[a-z]{2,4}$/;
    return (this.state.host.firstName && this.state.host.lastName && pattern.test(this.state.host.email) && this.state.host.description);
  }

  switchToIAmHostDetails = (loggedInHostDetails: any) => {
    let stateHost = this.state.host;
    stateHost.firstName = loggedInHostDetails.firstName;
    stateHost.lastName = loggedInHostDetails.lastName;
    stateHost.email = loggedInHostDetails.email;
    stateHost.description = loggedInHostDetails.description;
    stateHost.altId = loggedInHostDetails.altId
    this.setState({ stateHost: stateHost, isHostDetailRequest: false, checked: true, loggedInHostDetails: loggedInHostDetails });
  }

  switchToNewHost = () => {
    let stateHost = this.state.host;
    stateHost.firstName = stateHost.firstName;
    stateHost.lastName = stateHost.lastName;
    stateHost.email = stateHost.email;
    stateHost.description = stateHost.description;
    stateHost.altId = stateHost.altId
    this.setState({ stateHost: stateHost, isHostDetailRequest: false, checked: false });
  }

  switchIAmTheHost = async (check: boolean) => {
    if (check && !this.state.loggedInHostDetails) {
      if (this.props.props.session.user.email) {
        this.setState({ isHostDetailRequest: true });
        await axios.get<FeelItLiveHostResponse>(`api/host-detail/${this.props.props.session.user.email}`)
          .then((response: any) => {
            if (response.data.altId) {
              let loggedInHostDetails = setHostModelObject(this.props.props.slug);
              loggedInHostDetails.firstName = response.data.firstName;
              loggedInHostDetails.lastName = response.data.lastName;
              loggedInHostDetails.email = response.data.email;
              loggedInHostDetails.description = response.data.description;
              loggedInHostDetails.altId = response.data.altId;
              this.switchToIAmHostDetails(loggedInHostDetails);
            } else {
              this.setState({ isHostDetailRequest: false });
            }
          }).catch((error) => { this.setState({ isHostDetailRequest: false }); })
      }
    } else if (check && this.state.loggedInHostDetails) {
      this.switchToIAmHostDetails(this.state.loggedInHostDetails);
    } else {
      this.switchToNewHost()
    }
  }

  render() {
    return (

      <div data-aos="fade-up" data-aos-duration="1000">
        {(!this.state.isHostDetailRequest) ? <form
          onSubmit={(host: any) => {
            host.preventDefault();
            host.stopPropagation();
            this.props.onSubmit(this.state.host);
          }}
          ref={(ref) => {
            this.form = ref
          }}
        >
          <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="6">
            <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
              <h3 className="m-0 text-purple">{(this.props.selectedHostId == 0 && this.props.eventHostViewModel.eventHostMapping.length > 0) ? "Add more hosts" : "Introducing your hosts"}</h3></nav>
            <div>{this.props.selectedHostId ? 'Edit Host' : 'Create Host'}< ToolTip description="The host is the person/s who is/are conducting the event and who the customers/patrons/fans are buying the ticket to watch and listen. The link to stream the event will be sent to the host to enable the streaming to happen. Therefore, please ensure that the information below is filled in accurately" /></div>
            <div className="form-group pt-3">
              {!this.props.isModal && (
                <div className="fil-create-host pb-3">
                  <div className="d-inline-block pr-5">
                    <span style={{width:"110px"}}>
                      <small className="mr-4">
                        <b>I'm a Host</b>
                      </small>
                    </span>
                    <Switch checked={this.state.checked} onChange={this.switchIAmTheHost} />
                  </div>
                  <div className="d-inline-block">
                    <span  style={{width:"110px"}}>
                      <small className="mr-4">
                        <b>Active Host</b>
                      </small>
                    </span>
                    <Switch checked={this.state.host.isEnabled} onChange={(check) => {
                      let stateHost = this.state.host;
                      stateHost.isEnabled = check;
                      this.setState({ host: stateHost })
                    }} />
                  </div>
                </div>
              )}
              <div className="form-group">
                <div className="row">
                  <div className="col-12 col-sm-6 col-md-4">
                    <label>First Name</label>
                    <input
                      name="firstName"
                      placeholder="Enter first name"
                      className="form-control"
                      value={this.state.host.firstName}
                      onChange={(e) => {
                        let stateHost = this.state.host;
                        stateHost.firstName = e.target.value;
                        this.setState({ host: stateHost })
                      }}
                      type="text"
                      required
                    />
                  </div>
                  <div className="col-12 col-sm-6 col-md-4">
                    <label>Last Name</label>
                    <input
                      name="lastName"
                      placeholder="Enter last name"
                      className="form-control"
                      value={this.state.host.lastName}
                      onChange={(e) => {
                        let stateHost = this.state.host;
                        stateHost.lastName = e.target.value;
                        this.setState({ host: stateHost })
                      }}
                      type="text"
                      required
                    />
                  </div>
                  <div className="col-12 col-sm-6 col-md-4">
                    <label>Email</label>
                    <input
                      name="email"
                      placeholder="Enter email address"
                      className="form-control"
                      value={this.state.host.email}
                      onChange={(e) => {
                        let stateHost = this.state.host;
                        stateHost.email = e.target.value;
                        this.setState({ host: stateHost })
                      }}
                      type="email"
                      required
                    />
                  </div>
                </div>
              </div>
              <div className="form-group">
                <div className="row">
                  <div className="col-12 col-sm-12">
                    <label className="d-block">
                      Bio
                    <ToolTip description="Like the event/experience description above, all ticket buyers will be able to read this.
                            Therefore, please include a short effective bio that draws the customer/patron/fan in&nbsp;" />
                    </label>
                    <CKEditor
                      activeClass="p10"
                      content={this.state.host.description}
                      required
                      events={{
                        change: (e) => {
                          let stateHost = this.state.host;
                          stateHost.description = e.editor.getData();
                          this.setState({ host: stateHost })
                        }
                      }}
                    />
                  </div>
                </div>
              </div>
              <ImageUpload
                key={this.state.host.altId}
                imageInputList={[{ imageType: 'user', numberOfFields: 1, imageKey: this.state.host.altId ? this.state.host.altId.toUpperCase() : "" }]}
                onImageSelect={(item) => {
                  this.props.onImageSelect(item);
                }}
                onImageRemove={() => { }}
              />
            </div>

          </div>
          <Footer
            saveText={'Save Host'}
            isDisabled={!this.isButtonDisable()}
            isSaveRequest={this.props.props.EventHosts.isSaveRequest}
            onClickCancel={() => { this.props.onClickCancel() }}
            onSubmit={() => { this.form.dispatchEvent(new Event('submit')) }} />
        </form> :

          <div className="text-center"><Spinner /></div>
        }
      </div>
    )
  }
}
