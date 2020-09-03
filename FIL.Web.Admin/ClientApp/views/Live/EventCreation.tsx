import * as React from 'react'
import { connect } from 'react-redux'
import { actionCreators as sessionActionCreators, ISessionProps } from 'shared/stores/Session'
import { Modal, Button, Tooltip, notification } from 'antd'
import { NavLink, Link, Redirect, Route, Switch } from 'react-router-dom'
import { RouteComponentProps } from 'react-router-dom'
import { IApplicationState } from '../../stores'
import { bindActionCreators } from 'redux'
import FILLoader from '../../components/Loader/FILLoader'
import * as placeCalendarStore from '../../stores/PlaceCalendar'
import * as EventCreationStore from '../../stores/EventCreation'
import * as currencyTypeStore from '../../stores/CurrencyType'
import * as inventoryStore from '../../stores/Inventory'
import * as countryStore from '../../stores/Country'
import { Formik, Form, Field, FormikProps } from 'formik'
import EventDetail from '../Live/EventCreation/EventDetail/EventDetail'
import * as StatesStore from '../../stores/States'
import EventHost from '../Live/EventCreation/EventDetail/EventHost'
import EventCalendar from '../Live/EventCreation/EventDetail/EventCalendar'
import PlaceCalendarResponseViewModel from '../../models/PlaceCalendar/PlaceCalendarResponseViewModel'
import * as S3FileUpload from 'react-s3'
import TicketContainer from './EventCreation/Ticket/TicketContainer'
import FinanceContainer from './EventCreation/Finance/FinanceContainer'
import ImageUpload from '../../components/ImageUpload/ImageUpload'
import { ImageDetailModal } from '../../components/ImageUpload/utils'
import { Helmet } from 'react-helmet'
import AddOn from './EventCreation/Addons/AddOn'
import VideoUpload from '../../components/VideoUpload/VideoUpload'

type PlaceCalendarProps = EventCreationStore.IEventprops &
  placeCalendarStore.IPlaceCalendarProps &
  inventoryStore.InventoryProps &
  currencyTypeStore.ICurrencyTypeProps &
  StatesStore.IStatesTypeProps &
  countryStore.ICountryTypeProps &
  typeof EventCreationStore.actionCreators &
  typeof currencyTypeStore.actionCreators &
  typeof inventoryStore.actionCreators &
  typeof StatesStore.actionCreators &
  typeof placeCalendarStore.actionCreators &
  typeof countryStore.actionCreators &
  ISessionProps &
  typeof sessionActionCreators &
  RouteComponentProps<{ placeAltId: string }>

enum buttons {
  details = 1,
  tickets,
  addons,
  finance
}

const performanButtonCheckBox = {
  undefined: [],
  individual: [
    {
      id: 1,
      label: 'Pre-recorded video',
      name: 'preRecorded'
    },
    {
      id: 2,
      label: 'Live performance',
      name: 'livePerformance'
    }
  ],
  group: [
    {
      id: 1,
      label: 'Pre-recorded Video',
      name: 'preRecorded'
    },
    {
      id: 2,
      label: 'Live performance with artists collocated',
      name: 'artistsCollocated'
    },
    {
      id: 3,
      label: 'Live performance of group members, each performing sequentially',
      name: 'performingSequentially'
    }
  ]
}

class EventCreation extends React.Component<PlaceCalendarProps, any> {
  constructor(props) {
    super(props)
    this.state = {
      displayScreen: 0,
      isShowConfirmPopup: false,
      isEdit: props.match.params.placeAltId ? true : false,
      placeAltId: props.match.params.placeAltId,
      isDisabled: true,
      visible: false,
      eventDetailSections: {
        one: true,
        two: false,
        three: false,
        four: false
      },
      images: [],
      performanButtonCheckBox: {
        individual: {
          preRecorded: false,
          livePerformance: false
        },
        group: {
          preRecorded: false,
          artistsCollocated: false,
          performingSequentially: false
        }
      },
      ticketList: []
    }
  }

  componentDidMount() {
    let pathname = buttons[1]
    for (let path in buttons) {
      console.log(path)
      if (this.props.location.pathname.indexOf(path) > -1) pathname = path
    }
    this.setState({
      displayScreen: buttons[pathname]
    })
    if (sessionStorage.getItem('isReload') == 'true') {
      sessionStorage.removeItem('isReload')
      window.location.reload()
    }
    if (!this.props.currencyType.currencyTypeSuccess) {
      this.props.requestCurrencyTypeData()
    }
    this.props.requestEventCategories((e) => { })
    this.props.getTicketCategoryDetails()
    this.props.requestCountryTypeData()
    this.props.requestStatesTypeData('9FA4AE43-F84C-4D55-99AD-54BD0A29D1AB', (response: any) => {
      this.setState({
        stateOptions: response
      })
    })
    if (window) {
      let stripeConnectClientId = (window as any).stripeConnectClientId
      this.setState({ stripeConnectClientId: stripeConnectClientId, origin: window.location.origin }, () => {
        this.setClientId()
      })
    }

    if (this.props.match && this.props.match.params && this.props.match.params.placeAltId) {
      this.props.requestInentoryData(this.props.match.params.placeAltId, (item) => {
        this.setState({ inventoryData: item })
      })
      this.props.getEventSavedData(this.props.match.params.placeAltId, (item) => {
        this.setState({ placeData: item })
      })
    }
  }

  success = (type) => {
    let that = this
    notification[type]({
      message: that.state.confirmMessage,
      duration: 10,
      top: 10
    })
  }

  error = () => {
    Modal.error({
      title: this.state.confirmMessage,
      centered: true
    })
  }

  Confirm = (values) => {
    let originalValues = { ...values }
    let eventHosts = values.eventHosts.map((item) => {
      return {
        ...item,
        imageDetail: null
      }
    })
    values.eventHosts = eventHosts

    let that = this
    that.props.saveEventDetailCreation(values, (response: PlaceCalendarResponseViewModel) => {
      if (response.success) {
        sessionStorage.setItem("isEventDetailCreated", 'true');
        that.setState(
          {
            eventAltId: response.eventAltId,
            isEventDetailSaved: true,
            confirmMessage: 'Event details saved successfully!',
            isShowConfirmPopup: true
          },
          () => {
            that.uploadImage(originalValues, response)
            that.success('success')
          }
        )
      } else {
        that.setState(
          {
            confirmMessage: 'Darn! Something went wrong. Please recheck and save the event details again.',
            isShowConfirmPopup: true
          },
          () => {
            that.error()
          }
        )
      }
    })
  }

  uploadImage = (originalValues: any, response: PlaceCalendarResponseViewModel) => {
    let images = this.state.images
    if (images.length >= 1) {
      images.forEach((item) => {
        var blob = item.file.slice(0, item.file.size, 'image/png')
        let filePath =
          item.path.indexOf('tiles') > -1
            ? `${this.state.eventAltId.toUpperCase()}-ht-c1.jpg`
            : item.path.indexOf('about') > -1
              ? `${this.state.eventAltId.toUpperCase()}-about.jpg`
              : `${this.state.eventAltId.toUpperCase()}.jpg`
        var newFile = new File([blob], `${filePath}`, { type: 'image/png' })
        this.uploadImageBase(item.path, newFile)
      })
    }

    originalValues.eventHosts.forEach((item) => {
      let currentHost = response.eventHosts.filter((t) => t.email == item.email)[0]
      let imageDetail: ImageDetailModal = item.imageDetail
      if (imageDetail == undefined || imageDetail == null) return
      var blob = imageDetail.file.slice(0, imageDetail.file.size, 'image/png')
      let filePath = `${currentHost.altId.toUpperCase()}.jpg`
      var newFile = new File([blob], `${filePath}`, { type: 'image/png' })
      this.uploadImageBase(imageDetail.path, newFile)
    })
    this.setState({ displayScreen: buttons.tickets })
  }

  uploadImageBase = (path, file) => {
    const config = {
      bucketName: 'feelaplace-cdn',
      dirName: path,
      region: 'us-west-2',
      accessKeyId: 'AKIAYD5645ZATMXE5H2H',
      secretAccessKey: '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'
    }
    S3FileUpload.uploadFile(file, config)
      .then((data) => {
        console.log(data)
      })
      .catch((err) => { })
  }

  handleImageSelect = (item: ImageDetailModal) => {
    let images = [item]
    let imageList = [...this.state.images].filter((t) => t.id != item.id)
    if (item.id == 'descbanner') {
      let temp = { ...item }
      temp.path = 'images/places/about'
      images.push(temp)
    }
    this.setState({
      images: [...imageList, ...images]
    })
  }

  setClientId = () => {
    let countryId = sessionStorage.getItem('countryId')
    let stripeConnect = this.state.stripeConnectClientId
    let stripeConnectKey = stripeConnect.split(',')
    var country = (this.props.countryType.countryTypes && this.props.countryType.countryTypes.countries) ? this.props.countryType.countryTypes.countries.filter((val) => {
      return val.id == +countryId
    }) : [];
    if (countryId == '13') {
      // Australian Doller
      this.setState({
        stripeConnectKey: stripeConnectKey[1],
        countryId: countryId,
        isoAlphaTwoCode: country.length > 1 ? country[0].isoAlphaTwoCode : 'AU'
      })
    } else if (countryId == '101') {
      // INR
      this.setState({
        stripeConnectKey: stripeConnectKey[2],
        countryId: countryId,
        isoAlphaTwoCode: country.length > 1 ? country[0].isoAlphaTwoCode : 'IN'
      })
    } else {
      this.setState({
        stripeConnectKey: stripeConnectKey[0],
        countryId: countryId,
        isoAlphaTwoCode: country.length > 1 ? country[0].isoAlphaTwoCode : 'US'
      })
    }
  }

  onChangePerformanceCheckox = (e) => {
    this.setState({
      performanButtonCheckBox: {
        ...this.state.performanButtonCheckBox,
        [this.state.performanceButton]: {
          ...this.state.performanButtonCheckBox[this.state.performanceButton],
          [e.target.name]: !this.state.performanButtonCheckBox[this.state.performanceButton][e.target.name]
        }
      }
    })
  }

  onSubmitTicketList = (ticketList, cb) => {
    sessionStorage.setItem("ticketList", JSON.stringify(ticketList));
    this.setState(
      {
        ticketList: ticketList
      },
      cb
    )
  }

  onClickTab = (index: number) => {
    let path = buttons[index]
    this.setState({ displayScreen: index }, () => {
      this.props.history.push(`${this.props.match.url}/${path}`)
    })
  }

  render() {
    return (
      <div>
        <Helmet>
          <title>Host Live Stream Experiences and Events | Get Paid | FeelitLIVE</title>
          <meta
            name="description"
            content="Host live stream experiences and events right from your home or business location and
                    get paid with FeelitlLIVE. Get started by creating an experience or event, schedule and interact and engage with your audience."
          />
        </Helmet>
        {this.props.currencyType.currencyTypes && this.props.currencyType.currencyTypes.currencyTypes && (
          <div>
            <nav className="pb-4">
              <div className="nav justify-content-center text-uppercase fee-tab" id="nav-tab" role="tablist">
                <Link
                  className={'nav-item nav-link' + (this.state.displayScreen == buttons.details ? ' active' : '')}
                  onClick={() => this.onClickTab(buttons.details)}
                  to={`${this.props.match.url}/details`}
                  aria-controls="nav-Details"
                >
                  EVENT DETAILS
                </Link>
                <Link
                  className={'nav-item nav-link' + (this.state.displayScreen == buttons.tickets ? ' active' : '')}
                  onClick={() => this.onClickTab(buttons.tickets)}
                  to={`${this.props.match.url}/tickets`}
                  aria-controls="nav-contact"
                >
                  TICKETS
                </Link>
                <Link
                  className={'nav-item nav-link' + (this.state.displayScreen == buttons.addons ? ' active' : '')}
                  onClick={() => this.onClickTab(buttons.addons)}
                  to={`${this.props.match.url}/addons`}
                  aria-controls="nav-contact"
                >
                  ADD-ONS
                </Link>
                <Link
                  className={'nav-item nav-link' + (this.state.displayScreen == buttons.finance ? ' active' : '')}
                  onClick={() => this.onClickTab(buttons.finance)}
                  {...(this.state.isEdit ? { style: { cursor: 'not-allowed' } } : {})}
                  to={`${this.props.match.url}/finance`}
                  aria-controls="nav-contact"
                >
                  FINANCIAL INFO
                </Link>
              </div>
            </nav>
            <Switch>
              <Redirect exact from="/host-online" to="/host-online/details" />
              <div className="tab-content bg-white rounded shadow-sm pt-3" id="nav-tabContent">
                <Route
                  path="/host-online/details"
                  render={(renderProps) => (
                    <div
                      className={'tab-pane fade show' + (this.state.displayScreen == buttons.details ? ' active' : '')}
                    >
                      <Formik
                        enableReinitialize
                        initialValues={{
                          isEdit: this.state.isEdit ? true : false,
                          eventId: this.state.isEdit && this.state.placeData ? this.state.placeData.eventId : 0,
                          title: this.state.isEdit && this.state.placeData ? this.state.placeData.title : '',
                          eventCategoryId:
                            this.state.isEdit && this.state.placeData ? this.state.placeData.subcategoryid : '',
                          eventDescription:
                            this.state.isEdit && this.state.placeData ? this.state.placeData.description : ''
                        }}
                        onSubmit={(values: any) => {
                          this.Confirm(values)
                          this.props.history.push('/host-online/tickets')
                          //window.open('/host-online/tickets')
                        }}
                      >
                        {(props: FormikProps<any>) => (
                          <Form>
                            <div className="col-sm-12">
                              <a
                                className={`place-listing active`}
                                data-toggle="collapse"
                                href="#PlaceDetail"
                                role="button"
                                aria-expanded="true"
                                aria-controls="PlaceDetail"
                              >
                                <span className="rounded-circle border border-primary">1</span>
                                Title & Description
                              </a>
                              <div className={`collapse multi-collapse pt-3 show`} id="PlaceDetail">
                                <EventDetail
                                  props={props}
                                  isEdit={this.state.isEdit}
                                  placeDetails={this.state.placeData}
                                  categories={this.props.eventCreation.eventCategoriesList}
                                  fetchEventCategoriesSuccess={this.props.eventCreation.fetchEventCategoriesSuccess}
                                />
                              </div>
                            </div>
                            <div className="line" />
                            <div className="col-sm-12">
                              <a
                                className={`place-listing`}
                                data-toggle="collapse"
                                href="#DateTime"
                                role="button"
                                aria-expanded="false"
                                aria-controls="DateTime"
                              >
                                <span className="rounded-circle border border-primary">2</span>
                                Date & Time
                              </a>
                              <div className={`collapse multi-collapse pt-3`} id="DateTime">
                                <EventCalendar
                                  isEdit={this.state.isEdit}
                                  inventoryData={this.state.inventoryData}
                                  placeAltId={this.state.placeAltId}
                                  onSubmit={(item) => {
                                    props.setFieldValue('eventCalendar', item)
                                  }}
                                />
                              </div>
                            </div>
                            <div className="line" />
                            <div className="col-sm-12">
                              <a
                                className={`place-listing`}
                                data-toggle="collapse"
                                href="#Images"
                                role="button"
                                aria-expanded="false"
                                aria-controls="Images"
                              >
                                <span className="rounded-circle border border-primary">3</span>
                                Event Images
                              </a>
                              <div className={`col-12 collapse multi-collapse pt-3`} id="Images">
                                <ImageUpload
                                  imageInputList={[
                                    {
                                      imageType: 'tile',
                                      numberOfFields: 1,
                                      imageKey: this.props.match.params.placeAltId
                                    },
                                    {
                                      imageType: 'banner',
                                      numberOfFields: 1,
                                      imageKey: this.props.match.params.placeAltId
                                    }
                                  ]}
                                  onImageSelect={this.handleImageSelect}
                                />
                              </div>
                            </div>
                            <div className="line" />
                            <div className="col-sm-12">
                              <a
                                className={`place-listing`}
                                data-toggle="collapse"
                                href="#Performance"
                                role="button"
                                aria-expanded="false"
                                aria-controls="Performance"
                              >
                                <span className="rounded-circle border border-primary">4</span>
                                Performance Details
                              </a>
                              <div className={`collapse multi-collapse pt-3`} id="Performance">
                                <div className="col-sm-12 pb-3">
                                  <div className="form-group pt-3">
                                    <div className="row">
                                      <div className="col-12 col-sm-6">
                                        <p className="d-block clearfix">
                                          What kind of performance do you wish to conduct?
                                        </p>
                                        <div className="btn-group" role="group" aria-label="Basic example">
                                          <button
                                            onClick={() => this.setState({ performanceButton: 'individual' })}
                                            type="button"
                                            className={
                                              this.state.performanceButton == 'individual'
                                                ? 'btn btn-outline-primary active'
                                                : 'btn btn-outline-primary'
                                            }
                                          >
                                            Individual
                                          </button>
                                          <button
                                            onClick={() => this.setState({ performanceButton: 'group' })}
                                            type="button"
                                            className={
                                              this.state.performanceButton == 'group'
                                                ? 'btn btn-outline-primary active'
                                                : 'btn btn-outline-primary'
                                            }
                                          >
                                            Group
                                          </button>
                                        </div>
                                      </div>
                                    </div>
                                    <div className="row">
                                      <div className="col-12 mt-3">
                                        {performanButtonCheckBox[this.state.performanceButton].map((item, index) => {
                                          return (
                                            <div
                                              className="custom-control custom-checkbox d-inline-block p-0 pr-4 align-top "
                                              style={{ maxWidth: '280px' }}
                                            >
                                              <label key={index}>
                                                <input
                                                  type="checkbox"
                                                  className="position-absolute"
                                                  style={{
                                                    top: '6px',
                                                    width: '10px',
                                                    height: '10px'
                                                  }}
                                                  onChange={this.onChangePerformanceCheckox}
                                                  name={item.name}
                                                  checked={
                                                    this.state.performanButtonCheckBox[this.state.performanceButton][
                                                    item.name
                                                    ]
                                                  }
                                                />
                                                <div className="pl-4">{item.label}</div>
                                              </label>
                                            </div>
                                          )
                                        })}
                                      </div>
                                      {this.state.performanButtonCheckBox[this.state.performanceButton] &&
                                        this.state.performanButtonCheckBox[this.state.performanceButton][
                                        'preRecorded'
                                        ] && (
                                          <div className="col-12 col-sm-6 pt-3 mt-2">
                                            <label className="d-block">Upload Video File</label>
                                            <VideoUpload />
                                          </div>
                                        )}
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                            <div className="line" />
                            <div className="col-sm-12">
                              <a
                                className={`place-listing`}
                                data-toggle="collapse"
                                href="#Host"
                                role="button"
                                aria-expanded="false"
                                aria-controls="Host"
                              >
                                <span className="rounded-circle border border-primary">5</span>
                                About the host
                              </a>
                              <Tooltip
                                title={
                                  <p>
                                    <span>
                                      The host is the person/s who is/are conducting the event and who the
                                      customers/patrons/fans are buying the ticket to watch and listen. The link to
                                      stream the event will be sent to the host to enable the streaming to happen.
                                      Therefore, please ensure that the information below is filled in accurately
                                    </span>
                                  </p>
                                }
                              >
                                <span>
                                  <i className="fa fa-info-circle text-primary ml-2"></i>
                                </span>
                              </Tooltip>
                              <div className={`collapse multi-collapse pt-3`} id="Host">
                                <EventHost
                                  isEdit={this.state.isEdit}
                                  placeDetails={this.state.placeData}
                                  formState={[]}
                                  session={this.props.session}
                                  onSubmitHost={(host) => {
                                    props.setFieldValue('eventHosts', host)
                                  }}
                                />
                              </div>
                            </div>
                            <div className="line" />
                            <div className="text-center pt-4 pb-4">
                              <a
                                href="javascript:void(0)"
                                onClick={() => this.props.history.push('/')}
                                className="text-decoration-none mr-4"
                              >
                                Cancel
                              </a>
                              <div className="btn-group">
                                <button
                                  disabled={
                                    props.values.eventCategoryId &&
                                      props.values.eventHosts &&
                                      props.values.eventCalendar
                                      ? false
                                      : true
                                  }
                                  type="submit"
                                  className="btn btn-outline-primary"
                                >
                                  Save & continue
                                </button>
                              </div>
                            </div>
                          </Form>
                        )}
                      </Formik>
                    </div>
                  )}
                />
                <Route
                  path="/host-online/tickets"
                  render={(renderProps) => (
                    <div
                      className={'tab-pane fade show' + (this.state.displayScreen == buttons.tickets ? ' active' : '')}
                    >
                      <TicketContainer
                        currency={this.props.currencyType.currencyTypes.currencyTypes}
                        inventoryDataSubmit={this.props.inventoryDataSubmit}
                        ticketCategoryDetails={this.props.inventory.ticketCategoryDetails}
                        isEventDetailSaved={this.state.isEventDetailSaved}
                        countries={this.props.countryType.countryTypes}
                        isEdit={this.state.isEdit}
                        inventoryData={this.state.inventoryData}
                        placeAltId={this.state.placeAltId}
                        onClickCancel={() => {
                          this.setState({ displayScreen: buttons.details })
                          this.props.history.push('/host-online/details')
                        }}
                        onSubmitTicketCreation={(item) => {
                          if (sessionStorage && sessionStorage.getItem('countryId') != null) {
                            this.setClientId()
                          }
                          this.setState({ displayScreen: buttons.addons, isTicketsScreenSaved: true })
                          this.props.history.push('/host-online/addons')
                        }}
                        onSubmitTicketList={this.onSubmitTicketList}
                      />
                    </div>
                  )}
                />
                <Route
                  path="/host-online/addons"
                  render={(renderProps) => (
                    <div
                      className={'tab-pane fade show' + (this.state.displayScreen == buttons.addons ? ' active' : '')}
                    >
                      <AddOn
                        currencyOptions={this.props.currencyType.currencyTypes.currencyTypes.map((item) => {
                          return {
                            label: item.name,
                            value: item.id,
                            countryId: item.countryId,
                            code: item.code
                          }
                        })}
                        onClickCancel={() => {
                          this.setState({
                            displayScreen: buttons.tickets
                          })
                          this.props.history.push('/host-online/tickets')
                        }}
                        onSkipAddons={() => {
                          this.setState({
                            displayScreen: buttons.finance
                          })
                          this.props.history.push('/host-online/finance')
                        }}
                        inventoryDataSubmit={this.props.inventoryDataSubmit}
                        ticketList={this.state.ticketList}
                      />
                    </div>
                  )}
                />
                <Route
                  path="/host-online/finance"
                  render={(renderProps) => (
                    <div
                      className={'tab-pane fade show' + (this.state.displayScreen == buttons.finance ? ' active' : '')}
                    >
                      {this.state.countryId != '101' && (
                        <div className="p-3">
                          <div className="home-view-wrapper text-center container py-5 my-5">
                            <h6>
                              FeelitLIVE uses Stripe to transfer payments to your account. Please create a new account
                              in Stripe by clicking the button below. Stripe is fully PCI and SSL compliant.
                            </h6>
                            <h6>
                              Once you have filled the details successfully, your event will be submitted for approval.
                            </h6>
                            <div>
                              By clicking Next, you agree to the{' '}
                              <Link to="/terms-and-conditions" target="_blank" className="btn-link">
                                Additional Terms and Conditions
                              </Link>{' '}
                              and you confirm that the information you have provided is complete and correct.
                            </div>
                            <a
                              href="javascript:void(0)"
                              onClick={() => {

                                window.open(
                                  `https://connect.stripe.com/express/oauth/authorize?client_id=${
                                  this.state.stripeConnectKey
                                  }&redirect_uri=${`${this.state.origin}/stripe-connect/success/${this.state.eventAltId}`}&stripe_user[country]=${
                                  this.state.isoAlphaTwoCode
                                  }`,
                                  '_self'
                                )

                                this.setState({ displayScreen: buttons.finance, isTicketsScreenSaved: true })
                              }}
                              className="btn bnt-lg btn-outline-primary mt-4"
                            >
                              Next
                            </a>
                          </div>
                        </div>
                      )}
                      {this.state.countryId == '101' && (
                        <FinanceContainer
                          onSubmit={(item) => {
                            this.props.saveFinanceDetails(item, (item: PlaceCalendarResponseViewModel) => {
                              if (item.success) {
                                this.props.history.push({
                                  pathname: '/myfeels',
                                  state: {
                                    isStripeSuccess: true
                                  }
                                })
                              } else {
                                this.setState(
                                  {
                                    confirmMessage:
                                      'Darn! Something went wrong. Please recheck and save the finance detail again.',
                                    isShowConfirmPopup: true
                                  },
                                  () => {
                                    this.error()
                                  }
                                )
                              }
                            })
                          }}
                          stateOptions={this.state.stateOptions}
                        />
                      )}
                    </div>
                  )}
                />
              </div>
            </Switch>
          </div>
        )}
        {(this.props.inventory.isEventCreationSaveRequest ||
          this.props.inventory.isInventorySaveRequest ||
          this.props.inventory.isFinanceSaveRequest) && <FILLoader />}
      </div>
    )
  }
}
export default connect(
  (state: IApplicationState) => ({
    placeCalendar: state.placeCalendar,
    currencyType: state.currencyType,
    inventory: state.inventory,
    eventCreation: state.EventCreation,
    session: state.session,
    statesType: state.statesType,
    countryType: state.countryType
  }),
  (dispatch) =>
    bindActionCreators(
      {
        ...placeCalendarStore.actionCreators,
        ...currencyTypeStore.actionCreators,
        ...inventoryStore.actionCreators,
        ...StatesStore.actionCreators,
        ...EventCreationStore.actionCreators,
        ...countryStore.actionCreators,
        ...sessionActionCreators
      },
      dispatch
    )
)(EventCreation)
