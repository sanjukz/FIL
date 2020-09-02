/* Third party imports */
import * as React from 'react'
import { connect } from 'react-redux'
import { actionCreators as sessionActionCreators, ISessionProps } from 'shared/stores/Session'
import { Redirect, Route, Switch } from 'react-router-dom'
import { RouteComponentProps } from 'react-router-dom'
import { bindActionCreators } from 'redux'
import { notification } from 'antd';

/* Local imports */
import { IApplicationState } from '../../stores'
import * as EventCreationStore from '../../stores/EventCreation';
import * as CurrencyTypeStore from '../../stores/CurrencyType';
import * as ICountryStore from '../../stores/Country';
import * as InventoryStore from '../../stores/Inventory'
import * as EventDetailsStore from '../../stores/CreateEventV1/EventDetails';
import * as EventScheduleStore from '../../stores/CreateEventV1/EventSchedule';
import * as EventHostsStore from '../../stores/CreateEventV1/EventHosts';
import * as IEventTicketStore from '../../stores/CreateEventV1/EventTickets';
import * as IEventFinanceStore from '../../stores/CreateEventV1/EventFinance';
import * as IEventPerformanceStore from '../../stores/CreateEventV1/EventPerformance';
import * as IStepDetailsStore from '../../stores/CreateEventV1/StepDetails';
import * as IEventStepStore from '../../stores/CreateEventV1/EventStep';
import * as IEventSponsorStore from "../../stores/CreateEventV1/EventSponsor";
import * as IEventReplayStore from '../../stores/CreateEventV1/EventReplay';
import * as StatesStore from '../../stores/States'
import * as ApproveModerateStore from "../../stores/ApproveModerate";
import * as IEventImageStore from "../../stores/CreateEventV1/EventImage";
import { StepViewModel } from "../../models/CreateEventV1/StepViewModel";
import { getCurrentStep, getCurrentStepBySlug, getCurrentStepByStepId } from "../../utils/Step";
import MetaDetails from "./MetaDetails";
import { Index as Media } from "./Media/Index";
import { Index as EventDetail } from './EventDetail/Index';
import { Index as EventSchedule } from './EventSchedule/Index';
import { Index as Ticket } from './Tickets/Index';
import { Index as Host } from './Host/Index';
import { Index as Finance } from './Finance/Index'
import { Index as PerformanceType } from './PerformanceType/Index'
import { Index as Sponsor } from './Sponsor/Index'
import { Index as Monetization } from './Monetization/Index'
import { Index as Approval } from './Approval/Index'
import { Recurring } from './EventSchedule/Recurring'
import Spinner from "../../components/Spinner";
import { Steps } from "./Steps";
import { UploadStatus } from "../../Enums/UploadStatus";

type IndexProps = ISessionProps
  & EventCreationStore.IEventprops
  & EventDetailsStore.IEventDetailsProps
  & EventScheduleStore.IEventScheduleProps
  & EventHostsStore.IEventHostsProps
  & CurrencyTypeStore.ICurrencyTypeProps
  & InventoryStore.InventoryProps
  & ICountryStore.ICountryTypeProps
  & IEventTicketStore.IEventTicketsProps
  & IEventFinanceStore.IEventFinanceProps
  & IEventPerformanceStore.IEventPerformanceProps
  & StatesStore.IStatesTypeProps
  & IEventStepStore.IEventStepProps
  & IEventSponsorStore.IEventSponsorProps
  & IStepDetailsStore.IStepDetailsProps
  & IEventImageStore.IEventImageProps
  & ApproveModerateStore.IApproveModerateProps
  & IEventReplayStore.IEventReplayProps
  & typeof EventDetailsStore.actionCreators
  & typeof EventCreationStore.actionCreators
  & typeof EventScheduleStore.actionCreators
  & typeof EventHostsStore.actionCreators
  & typeof CurrencyTypeStore.actionCreators
  & typeof InventoryStore.actionCreators
  & typeof ICountryStore.actionCreators
  & typeof StatesStore.actionCreators
  & typeof IEventTicketStore.actionCreators
  & typeof IEventFinanceStore.actionCreators
  & typeof IEventPerformanceStore.actionCreators
  & typeof IEventSponsorStore.actionCreators
  & typeof IStepDetailsStore.actionCreators
  & typeof IEventStepStore.actionCreators
  & typeof IEventImageStore.actionCreators
  & typeof ApproveModerateStore.actionCreators
  & typeof IEventReplayStore.actionCreators
  & typeof sessionActionCreators
  & RouteComponentProps<{}>;

class Index extends React.Component<IndexProps, any> {
  constructor(props) {
    super(props)
    this.state = {
      stepModel: IStepDetailsStore.unloadedStepViewModel
    }
  }

  updateStepDetails = (currentStep, completedStep, slug, isRedirect) => {
    let component = getCurrentStepByStepId(this.state.stepModel, currentStep);
    let stepModel = this.state.stepModel;
    stepModel.currentStep = currentStep;
    stepModel.completedStep = completedStep;
    this.setState({ stepModel: stepModel, slug: slug, component: component.slug }, () => {
      if (isRedirect) {
        this.props.history.push(`/host-online/${slug}/${component.slug}`)
      }
    })
  }

  updatePath = (path: any, response: StepViewModel) => {
    let component = path.length > 3 ? path[3] : path[2];
    let slug = path.length > 3 ? path[2] : undefined;
    let currentStepDetail = getCurrentStep(response);
    if (!component) {
      component = "basics"
    }
    if (!slug) {
      this.setState({
        stepModel: response,
        path: (component.indexOf('basics') == -1 || component.indexOf('details') == -1) ? "/host-online/basics" : this.props.location.pathname,
        slug: slug,
        component: (component.indexOf('basics') == -1 || component.indexOf('details') == -1) ? "basics" : component,
      }, () => {
        if (component.indexOf('basics') == -1 || component.indexOf('details') == -1) {
          this.props.history.push("/host-online/basics")
        }
      })
    } else if (slug) {
      if (component.indexOf("basics") > -1) {
        this.setState({
          stepModel: response,
          path: `/host-online/${slug}/${currentStepDetail.slug}`,
          slug: slug,
          component: currentStepDetail.slug,
        }, () => {
          this.props.history.push(`/host-online/${slug}/${currentStepDetail.slug}`)
        })
      } else if (currentStepDetail) {
        currentStepDetail = getCurrentStepBySlug(response, component);
        response.currentStep = currentStepDetail.stepId;
        this.setState({
          stepModel: response,
          path: this.props.location.pathname,
          slug: slug,
          component: component,
        })
      }
    }
  }

  componentDidMount() {
    let path = this.props.location.pathname.split("/");
    let component = path.length > 3 ? path[3] : path[2];
    let slug = path.length > 3 ? path[2] : undefined;
    this.props.requestStepDetails(4, path.length > 3 ? +path[2] : 0, (response: StepViewModel) => {
      if (response.success) {
        this.props.requestEventCategories(() => { });
        this.props.getTicketCategoryDetails()
        this.props.requestCountryTypeData()
        this.props.requestCurrencyTypeData()
        this.props.requestStatesTypeData('9FA4AE43-F84C-4D55-99AD-54BD0A29D1AB', (stateResponse: any) => { })
        this.updatePath(path, response)
      } else {
        this.setState({
          stepModel: response,
          path: this.props.location.pathname,
          slug: slug,
          component: component,
        })
      }
    })
  }

  changeEventStatus = (statusId) => {
    this.props.requestEnableApproveModeratePlace(null, false, +this.state.slug, statusId, (response) => {
      if (response.success) {
        if (statusId == 4) {
          let stepModel = this.state.stepModel;
          stepModel.eventStatus = 4;
          this.setState({ stepModel: stepModel });
        }
      }
    });
  }

  render() {
    return (
      <>
        {this.state.component ? <>
          <MetaDetails />
          <section className="px-3 fil-breadcrumb w-100">
            <a href="/" className="text-purple">Dashboard</a> <span className="d-inline-block px-2">/</span>
            {this.state.stepModel.eventName ? this.state.stepModel.eventName : "Create New Experience"}
          </section>
          <section className="px-3 fil-admin-title w-100">
            <h2 className="m-0 clr-link">{this.state.stepModel.eventName ? this.state.stepModel.eventName : "New Event Name"}</h2>
          </section>
          <Steps
            key={`${this.state.component}`}
            component={this.state.component}
            slug={this.state.slug}
            stepModel={this.state.stepModel}
            history={this.props.history}
            isVideoUploadRequest={this.state.isVideoUploadRequest}
            onClick={(component: string) => {
              this.setState({ component: component });
            }}
            onCancelVideoUpload={() => {
              this.setState({ isVideoUploadRequest: false, progress: 0 });
            }}
          />
          <div className="card border-0 right-cntent-area pb-5 bg-light">
            <div className="card-body bg-light p-0">
              <Switch>
                <Redirect exact from="/host-online" to="/host-online/basics" />
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/basics` : `${this.props.match.url}/${this.state.slug}/basics`}
                  exact
                  render={(renderProps) => (
                    (this.props.eventCreation.fetchEventCategoriesSuccess) ? <EventDetail
                      props={this.props}
                      slug={this.state.slug}
                      isBasicTab={true}
                      changeRoute={(eventName, currentStep, completedStep, slug) => {
                        let stepModel = this.state.stepModel;
                        stepModel.eventName = eventName;
                        this.setState({ stepModel: stepModel });
                        this.updateStepDetails(currentStep, completedStep, slug, true);
                      }} /> : <Spinner />
                  )}
                />
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/details` : `${this.props.match.url}/${this.state.slug}/details`}
                >
                  {(this.props.eventCreation.fetchEventCategoriesSuccess) ? <EventDetail
                    props={this.props}
                    slug={this.state.slug}
                    isBasicTab={false}
                    changeRoute={(currentStep, completedStep) => {
                      this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, true);
                    }} /> : <Spinner />}
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/schedule` : `${this.props.match.url}/${this.state.slug}/schedule`}
                >
                  <EventSchedule
                    props={this.props}
                    slug={this.state.slug}
                    changeRoute={(currentStep, completedStep) => {
                      this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, true);
                    }}
                  />
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/recurring-event` : `${this.props.match.url}/${this.state.slug}/recurring-event`}
                >
                  <Recurring
                    props={this.props}
                    slug={this.state.slug}
                    changeRoute={(currentStep, completedStep) => {
                      this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, true);
                    }}
                  />
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/images` : `${this.props.match.url}/${this.state.slug}/images`}
                >
                  <Media
                    props={this.props}
                    slug={this.state.slug}
                    changeRoute={(currentStep, completedStep) => {
                      this.updateStepDetails(currentStep ? currentStep : 5, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, true);
                    }} />
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/performance-type` : `${this.props.match.url}/${this.state.slug}/performance-type`}
                >
                  <PerformanceType
                    key={this.state.key || 1}
                    props={this.props}
                    slug={this.state.slug}
                    file={this.state.file}
                    progress={this.state.progress}
                    isShowVideo={this.state.isShowVideo}
                    isVideoUploadRequest={this.state.isVideoUploadRequest}
                    onChange={(isShowVideo: boolean) => {
                      this.setState({ isShowVideo: isShowVideo });
                    }}
                    onUploadRequest={(file) => {
                      this.setState({ file: file, isVideoUploadRequest: true })
                    }}
                    onProgress={(progress) => {
                      this.setState({ progress: progress })
                    }}
                    onUploadSuccess={() => {
                      this.setState({
                        isShowVideo: true,
                        progress: 0,
                        isVideoUploadRequest: false,
                      })
                    }}
                    changeRoute={(currentStep, completedStep) => {
                      this.setState({ isVideoUploadRequest: false, isShowVideo: false, progress: 0 })
                      this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, true);
                    }} />
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/hosts` : `${this.props.match.url}/${this.state.slug}/hosts`}
                >
                  <Host
                    props={this.props}
                    slug={this.state.slug}
                    eventStatus={this.state.stepModel.eventStatus}
                    changeRoute={(currentStep, completedStep, isRouteChange?: boolean) => {
                      this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, isRouteChange ? true : false);
                    }} />
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/tickets` : `${this.props.match.url}/${this.state.slug}/tickets`}
                >
                  {(this.props.inventory.isTicketCategoryDetailsSuccess
                    && this.props.currencyType.currencyTypes
                    && this.props.currencyType.currencyTypes.currencyTypes
                  ) ? <Ticket
                      key="1"
                      props={this.props}
                      slug={this.state.slug}
                      eventStatus={this.state.stepModel.eventStatus}
                      isAddOn={false}
                      changeRoute={(currentStep, completedStep, isRouteChange?: boolean) => {
                        this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, isRouteChange ? true : false);
                      }} /> : <Spinner />}
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/add-ons` : `${this.props.match.url}/${this.state.slug}/add-ons`}
                >
                  {(this.props.inventory.isTicketCategoryDetailsSuccess
                    && this.props.currencyType.currencyTypes
                    && this.props.currencyType.currencyTypes.currencyTypes
                  ) ? <Ticket
                      key="2"
                      props={this.props}
                      slug={this.state.slug}
                      isAddOn={true}
                      changeRoute={(currentStep, completedStep, isRouteChange?: boolean) => {
                        this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, isRouteChange ? true : false);
                      }} /> : <Spinner />}
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/monetization` : `${this.props.match.url}/${this.state.slug}/monetization`}
                >
                  {(this.props.currencyType.currencyTypes
                    && this.props.currencyType.currencyTypes.currencyTypes
                    && this.props.currencyType.currencyTypes.currencyTypes.length > 0
                    && this.props.countryType.countryTypeSuccess
                  ) ? <Monetization
                      props={this.props}
                      slug={this.state.slug}
                      changeRoute={(currentStep, completedStep, isRouteChange?: boolean) => {
                        this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, isRouteChange ? true : false);
                      }} /> : <Spinner />}
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/sponsor` : `${this.props.match.url}/${this.state.slug}/sponsor`}
                >
                  <Sponsor
                    props={this.props}
                    slug={this.state.slug}
                    completedStep={this.state.stepModel.completedStep}
                    changeRoute={(currentStep, completedStep, isRouteChange?: boolean) => {
                      this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, isRouteChange ? true : false);
                    }} />
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/finance-info` : `${this.props.match.url}/${this.state.slug}/finance-info`}
                >
                  {this.props.statesType.statesTypeSuccess ? <Finance
                    props={this.props}
                    slug={this.state.slug}
                    changeRoute={(currentStep, completedStep, isRouteChange?: boolean) => { this.updateStepDetails(currentStep, completedStep ? completedStep : this.state.stepModel.completedStep, this.state.slug, isRouteChange ? true : false); }} /> : <Spinner />}
                </Route>
                <Route
                  path={!this.state.slug ? `${this.props.match.url}/submission` : `${this.props.match.url}/${this.state.slug}/submission`}
                >
                  <Approval
                    props={this.props}
                    eventStatus={this.state.stepModel.eventStatus}
                    onSubmit={() => {
                      this.changeEventStatus(4)
                    }} />
                </Route>
              </Switch>
            </div>
          </div>
        </> : <Spinner isShowLoadingMessage={true} />
        }
      </>
    )
  }
}
export default connect(
  (state: IApplicationState) => ({
    eventCreation: state.EventCreation,
    EventDetails: state.EventDetails,
    EventSchedule: state.EventSchedule,
    EventHosts: state.EventHosts,
    inventory: state.inventory,
    currencyType: state.currencyType,
    countryType: state.countryType,
    EventTickets: state.EventTickets,
    EventFinance: state.EventFinance,
    EventPerformance: state.EventPerformance,
    statesType: state.statesType,
    StepDetails: state.StepDetails,
    EventStep: state.EventStep,
    ApproveModerate: state.ApproveModerate,
    EventSponsor: state.EventSponsor,
    EventImage: state.EventImage,
    EventReplay: state.EventReplay,
    session: state.session,
  }),
  (dispatch) =>
    bindActionCreators(
      {
        ...EventCreationStore.actionCreators,
        ...EventDetailsStore.actionCreators,
        ...EventScheduleStore.actionCreators,
        ...EventHostsStore.actionCreators,
        ...CurrencyTypeStore.actionCreators,
        ...InventoryStore.actionCreators,
        ...ICountryStore.actionCreators,
        ...IEventTicketStore.actionCreators,
        ...IEventFinanceStore.actionCreators,
        ...IEventPerformanceStore.actionCreators,
        ...StatesStore.actionCreators,
        ...IStepDetailsStore.actionCreators,
        ...IEventStepStore.actionCreators,
        ...ApproveModerateStore.actionCreators,
        ...IEventSponsorStore.actionCreators,
        ...IEventImageStore.actionCreators,
        ...IEventReplayStore.actionCreators,
        ...sessionActionCreators,
      },
      dispatch
    )
)(Index)

