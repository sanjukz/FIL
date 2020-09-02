/* Third party imports */
import * as React from 'react'
import * as _ from 'lodash'
import { Link } from 'react-router-dom';

/* Local imports */
import FinanceForm from './FinanceForm'
import { EventFinanceViewModel } from "../../../models/CreateEventV1/EventFinanceViewModal";
import Spinner from "../../../components/Spinner";
import { showNotification } from "../Notification";

export class Index extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      eventFinanceViewModel: this.setDefaultObject(),
      isShowFinanceForm: false,
      isStripeAccountLinked: false
    }
  }

  componentDidMount() {
    if (this.props.slug) {
      localStorage.setItem('placeId', `${this.props.slug}`)
      this.props.props.requestEventFinance(this.props.slug, (response: EventFinanceViewModel) => {
        if (response.success) {
          response.currentStep = this.state.eventFinanceViewModel.currentStep;
          if (response.stripeAccount == 3 && response.isValidLink) {
            this.setState({
              stateOptions: this.props.props.statesType.statesTypes,
              isShowFinanceForm: true,
              eventFinanceViewModel: { ...response }
            });
          } else if (response.stripeAccount != 3 && response.stripeConnectAccountId && response.stripeConnectAccountId != null) {
            this.setState({
              eventFinanceViewModel: { ...response },
              isStripeAccountLinked: true
            });
          } else if (response.stripeAccount != 3 && (!response.stripeConnectAccountId || response.stripeConnectAccountId == null)) {
            localStorage.setItem('placeAltId', `${this.props.props.EventFinance.eventFinance.eventAltId}`)
            let stripeConnectClientId = (window as any).stripeConnectClientId;
            let stripeConnectClientKeyArray = stripeConnectClientId ? stripeConnectClientId.split(",") : [];
            if (stripeConnectClientKeyArray.length > 0) {
              if (response.currencyType.id == 13) {
                // Australian Doller
                this.setState({
                  stripeConnectClientId: stripeConnectClientKeyArray[1],
                  eventFinanceViewModel: { ...response },
                  origin: window.location.origin
                })
              }
              else {
                this.setState({
                  stripeConnectClientId: stripeConnectClientKeyArray[0],
                  eventFinanceViewModel: { ...response },
                  origin: window.location.origin
                })
              }
            }
          }
        }
      })
    }
  }

  setDefaultObject = () => {
    let eventFinanceViewModel: EventFinanceViewModel = {
      stripeAccount: 0,
      success: false,
      eventId: this.props.slug ? +this.props.slug : 0,
      eventAltId: "",
      stripeConnectAccountId: "",
      currencyType: undefined,
      eventFinanceDetailModel: undefined,
      isDraft: false,
      isValidLink: false,
      currentStep: 9
    }
    return eventFinanceViewModel;
  }

  render() {
    const props = this.props.props;
    return (
      <div data-aos="fade-up" data-aos-duration="1000" className="p-3" >
        {(!props.EventFinance.isLoading || props.EventFinance.isSaveRequest) ? (this.state.isShowFinanceForm ?
          <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="12">
            < FinanceForm
              stateOptions={this.state.stateOptions}
              eventFinanceViewModel={this.state.eventFinanceViewModel}
              props={props}
              onSubmit={(item: any) => {
                let eventFinanceViewModel: EventFinanceViewModel = {
                  eventId: this.props.slug ? +this.props.slug : 0,
                  eventFinanceDetailModel: { ...item },
                  stripeAccount: this.state.eventFinanceViewModel.stripeAccount,
                  stripeConnectAccountId: this.state.eventFinanceViewModel.stripeConnectAccountId,
                  eventAltId: this.state.eventFinanceViewModel.eventAltId,
                  currencyType: null,
                  isDraft: false,
                  isValidLink: false,
                  success: false,
                  currentStep: 9
                }
                this.props.props.saveEventFinance(eventFinanceViewModel, (response: EventFinanceViewModel) => {
                  if (response.success) {
                    showNotification('success', 'Finance tab saved successfully');
                    this.setState({ eventFinanceViewModel: response });
                    this.props.changeRoute(12, response.completedStep, true)
                  } else {
                  }
                })
              }}
            />
          </div> :
          props.EventFinance.stripeAccount != 3 ? (
            this.state.isStripeAccountLinked ? <>
              <div className="bg-white shadow-sm mb-2 rounded-box" id="nav-tabContent" key="12">
                <div className="px-5 pt-5">
                  <div className="row">
                    <div className="col-sm-6">
                      <h3 className="mb-4 text-purple">Nothing else to do here</h3>
                      <p className="mb-4">Your stripe account has already been connected with our system.</p>
                      <p className="mb-4">You are ready to start to receive you earnings from your events.</p>
                      <a className='btn btn-outline-primary' href='/'>
                        {'Take me to my dashboard'}<i className="fa fa-angle-right ml-1" aria-hidden="true"></i>
                      </a></div>
                    <div className="col-sm-6 text-right"><img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/strip-end.svg"
                      className="img-fluid" /></div>
                  </div>
                </div>
                <div className="box-botttom-img"><img className="w-100" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/strip-end-shade.svg" /></div>
              </div>

            </> : <div data-aos="fade-up" data-aos-duration="1000" >
                <div className="bg-white shadow-sm mb-2 rounded-box" id="nav-tabContent" key="12">
                  <div className="px-5 pt-5">
                    <h3 className="mb-4 text-purple">Connecting your account to Stripe</h3>
                    <h6 className="mb-4">
                      FeelitLIVE uses Stripe to transfer payments to your account. Please create a new account in Stripe by clicking the button below. Stripe is fully PCI and SSL compliant.
                            </h6>
                    <h6 className="mb-4">
                      By clicking “Connect”, you agree to the Additional{' '}
                      <Link to="/terms-and-conditions" target="_blank" className="btn-link">
                        Terms and Conditions
                              </Link>{' '}
                           and you confirm that the information you have provided is complete and correct.
                            </h6>
                    <a
                      href="javascript:void(0)"
                      onClick={() => {
                        window.open(
                          `https://connect.stripe.com/express/oauth/authorize?client_id=${
                          this.state.stripeConnectClientId
                          }&redirect_uri=${`${this.state.origin}/stripe-connect/success/${this.props.props.EventFinance.eventFinance.eventAltId}`}&stripe_user[country]=${
                          this.props.props.EventFinance.eventFinance.isoAlphaTwoCode ? this.props.props.EventFinance.eventFinance.isoAlphaTwoCode : 'US'
                          }`,
                          '_self'
                        )
                      }}
                      className="btn bnt-lg btn-outline-primary"
                    >
                      Connect <i className="fa fa-angle-right ml-1" aria-hidden="true"></i>
                    </a>
                  </div>
                  <div className=""><img className="w-100" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/strip-connect.svg" /></div>
                </div></div>) : <Spinner />) : <Spinner />}
      </div >
    )
  }
}
