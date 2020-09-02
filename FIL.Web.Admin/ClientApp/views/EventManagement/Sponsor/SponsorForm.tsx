/* Third party imports */
import * as React from 'react'
import * as _ from 'lodash'
import { Switch } from 'antd'

/* Local imports */
import ImageUpload from '../../../components/ImageUpload/ImageUpload'
import { Footer } from "../Footer/FormFooter";
import { setSponsorModelObject } from "../utils/DefaultObjectSetter";
import { REQUIRED_FINANCE_STEP_ARRAY } from '../../../utils/Constant/Step';
import { getStepDisable } from '../../../utils/Step';

export default class HostForm extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
      sponsor: setSponsorModelObject(this.props.props.slug),
      checked: false
    }
  }
  form: any;

  componentDidMount() {
    if (this.props.selectedSponsorId != 0) {
      let seletedSponsor = this.props.eventSponsorViewModel.sponsorDetails.filter((val: any) => { return val.id == this.props.selectedSponsorId });
      if (seletedSponsor.length > 0) {
        this.setState({ sponsor: { ...seletedSponsor[0] } });
      }
    }
  }

  isButtonDisable = () => {
    return (this.state.sponsor.name);
  }

  render() {
    return (
      <div data-aos="fade-up">
        <form
          onSubmit={(sponsor: any) => {
            sponsor.preventDefault();
            sponsor.stopPropagation();
            this.props.onSubmit(this.state.sponsor);
          }}
          ref={(ref) => {
            this.form = ref
          }}
        >
          <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box mt-4" id="nav-tabContent" key="8">
            <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
              <h3 className="m-0 text-purple">{(this.props.selectedSponsorId == 0 && this.props.eventSponsorViewModel.sponsorDetails.length > 0) ? "Add more sponsors" : "Let’s show them your appreciation"}</h3></nav>
            <div className="col-sm-12 pb-3">
              <div className="form-group pt-3">
                <div className="form-group">
                  <div className="row">
                    <div className="col-12 col-sm-6">
                      <label>Sponsor Name</label>
                      <input
                        name="sponsorName"
                        placeholder="Sponsor Name"
                        className="form-control"
                        value={this.state.sponsor.name}
                        onChange={(e) => {
                          let stateSponsor = this.state.sponsor;
                          stateSponsor.name = e.target.value;
                          this.setState({ sponsor: stateSponsor })
                        }}
                        type="text"
                        required
                      />
                    </div>
                    <div className="col-12 col-sm-6">
                      <label>Sponsor Link</label>
                      <input
                        name="sponsorLink"
                        placeholder={"https://www.sponsor.com"}
                        className="form-control"
                        value={this.state.sponsor.link}
                        onChange={(e) => {
                          let stateSponsor = this.state.sponsor;
                          stateSponsor.link = e.target.value;
                          this.setState({ sponsor: stateSponsor })
                        }}
                        type="text"
                        required
                      />
                    </div>
                  </div>
                </div>
                <div className="form-group">
                  <div className="row">
                    <div className="col-sm-6">
                      <ImageUpload
                        key={this.state.sponsor.altId}
                        imageInputList={[{ imageType: 'sponsor', numberOfFields: 1, imageKey: this.state.sponsor.altId ? this.state.sponsor.altId.toUpperCase() : "" }]}
                        onImageSelect={(item) => {
                          this.props.onImageSelect(item);
                        }}
                        onImageRemove={() => { }}
                      />
                    </div>
                    <div className="col-12 col-sm-6">
                      <label>Priority (Optional)</label>
                      <input
                        name="priority"
                        placeholder="1 (Smaller number ordered higher)"
                        className="form-control"
                        value={this.state.sponsor.priority}
                        onChange={(e) => {
                          let stateSponsor = this.state.sponsor;
                          stateSponsor.priority = e.target.value;
                          this.setState({ sponsor: stateSponsor })
                        }}
                        type="number"
                        required
                      />
                    </div>
                  </div>
                </div>

              </div>
            </div>
          </div>
          <Footer
            isDisabled={!this.isButtonDisable()}
            isShowSkip={true}
            saveText={'Save Sponsor'}
            onClickSkip={() => { this.props.onClickSkip() }}
            isSaveRequest={this.props.props.EventSponsor.isSaveRequest}
            onClickCancel={() => { this.props.onClickCancel() }}
            onSubmit={() => { this.form.dispatchEvent(new Event('submit')) }} />
        </form>
      </div>
    )
  }
}
