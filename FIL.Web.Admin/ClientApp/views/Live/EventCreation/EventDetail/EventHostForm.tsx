import * as React from 'react'
import * as _ from 'lodash'
import CKEditor from 'react-ckeditor-component'
import { HostRequestModel } from '../../../../models/CreateEvent/Host'
import { ImageDetailModal } from '../../../../components/Redemption/Guide/DocumentImageUpload'
import { Switch } from 'antd'
import { Tooltip } from 'antd'
import ImageUpload from '../../../../components/ImageUpload/ImageUpload'
import axios from 'axios'
import FeelItLiveHostResponse from '../../../../models/FeelItLive/FeelITLiveHost'
import { getImageDataModel } from '../../../../components/ImageUpload/utils'
import { CUSTOM_REGEX } from '../../../../utils/regexExpression'

export default class EventHostForm extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
      imageDetail: null,
      description: '',
      email: '',
      firstName: '',
      lastName: '',
      altId: ''
    }
  }
  form: any

  hydrateState = (values) => {
    this.setState({
      description: values.description,
      email: values.email,
      firstName: values.firstName,
      lastName: values.lastName,
      altId: values.altId,
      eventId: values.eventId ? values.eventId : 0,
      isStateUpdated: true
    })
  }

  componentDidMount() {
    if (this.props.formState) {
      this.hydrateState(this.props.formState)
    }
  }

  handleImageSelect = (item: ImageDetailModal, cb) => {
    this.setState(
      {
        imageDetail: item
      },
      cb
    )
  }

  handleImageRemove = () => {
    this.setState({
      imageDetail: null
    })
  }

  switchIAmTheHost = async (check: boolean) => {
    let userData: FeelItLiveHostResponse
    if (check) {
      if (this.props.session.user.email) {
        userData = await (await axios.get<FeelItLiveHostResponse>(`api/host-detail/${this.props.session.user.email}`))
          .data
        let file = await this.checkImageAndReturnFile(
          `https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/eventHost/${userData.altId.toUpperCase()}.jpg`
        )
        this.hydrateState({
          ...this.props.session.user,
          altId: userData.altId,
          description: userData.description,
          imageDetail: getImageDataModel('user', file)
        })
      } else {
        this.hydrateState(this.props.session.user)
      }
    } else {
      this.hydrateState({
        firstName: '',
        lastName: '',
        email: '',
        altId: '',
        description: '',
        eventId: 0
      })
    }
  }

  checkImageAndReturnFile = (url) => {
    return fetch(url)
      .then((res) => res.blob())
      .then((blob) => {
        let file = new File([blob], 'feel-image', { type: 'image/jpeg' })
        return file
      })
      .catch(() => null)
  }

  onBlurNameFields = (e) => {
    if (e.target.value !== '' && !CUSTOM_REGEX.onlyString.ex.test(e.target.value)) {
      this.setState({
        [e.target.name]: ''
      })
      alert(CUSTOM_REGEX.onlyString.msg)
    }
  }

  onBlurEmailField = (e) => {
    if (e.target.value !== '' && !CUSTOM_REGEX.email.ex.test(e.target.value)) {
      this.setState({
        [e.target.name]: ''
      })
      alert(CUSTOM_REGEX.email.msg)
    }
  }

  onSubmitPorps = () => {
    let hostFormData: HostRequestModel = {
      id: this.props.currentHostId,
      altId: this.state.altId ? this.state.altId : null,
      description: this.state.description,
      email: this.state.email,
      firstName: this.state.firstName,
      lastName: this.state.lastName,
      eventId: this.state.eventId,
      imageDetail: this.state.imageDetail
    }
    this.props.onSubmit(hostFormData)
  }

  render() {
    if (!this.state.isStateUpdated && this.props.formState) {
      this.hydrateState(this.props.formState)
    }

    return (
      <form
        onSubmit={(values: any) => {
          this.onSubmitPorps()
          values.preventDefault()
          values.stopPropagation()
        }}
        ref={(ref) => {
          this.form = ref
        }}
      >
        <div className="col-sm-12 pb-3">
          <div className="form-group pt-3">
            {!this.props.isModal && (
              <div>
                <div className="row">
                  <div className="col-sm-12">
                    <Switch onChange={this.switchIAmTheHost} />
                    <span>
                      <small className="ml-10">
                        <b>I'm the Host</b>
                      </small>
                    </span>
                  </div>
                </div>
              </div>
            )}
            <div className="form-group">
              <div className="row">
                <div className="col-12 col-sm-6">
                  <label>First Name</label>
                  <input
                    name="firstName"
                    placeholder="Enter First Name"
                    className="form-control"
                    value={this.state.firstName}
                    onChange={(e) => {
                      this.setState({ firstName: e.target.value })
                    }}
                    type="text"
                    required
                  />
                </div>
                <div className="col-12 col-sm-6">
                  <label>Last Name</label>
                  <input
                    name="lastName"
                    placeholder="Enter Last Name"
                    className="form-control"
                    value={this.state.lastName}
                    onChange={(e) => {
                      this.setState({ lastName: e.target.value })
                    }}
                    type="text"
                    required
                  />
                </div>
              </div>
            </div>
            <div className="form-group">
              <div className="row">
                <div className="col-12 col-sm-6">
                  <label>Email</label>
                  <input
                    name="email"
                    placeholder="Enter Email"
                    className="form-control"
                    value={this.state.email}
                    onChange={(e) => {
                      this.setState({ email: e.target.value })
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
                    <Tooltip
                      title={
                        <p>
                          <span>
                            Like the event/experience description above, all ticket buyers will be able to read this.
                            Therefore, please include a short effective bio that draws the customer/patron/fan in&nbsp;
                          </span>
                        </p>
                      }
                    >
                      <span>
                        <i className="fa fa-info-circle text-primary ml-2"></i>
                      </span>
                    </Tooltip>
                  </label>
                  <CKEditor
                    activeClass="p10"
                    content={this.state.description}
                    required
                    events={{
                      change: (e) => {
                        this.setState({ description: e.editor.getData() }, () => {
                          if (!this.props.isModal) {
                            this.onSubmitPorps()
                          }
                        })
                      }
                    }}
                  />
                </div>
              </div>
            </div>
            <ImageUpload
              imageInputList={[{ imageType: 'user', numberOfFields: 1, imageKey: this.state.altId }]}
              onImageSelect={(item) => {
                this.handleImageSelect(item, () => {
                  if (!this.props.isModal) {
                    this.onSubmitPorps()
                  }
                })
              }}
              onImageRemove={this.handleImageRemove}
            />
          </div>
          {this.props.isModal && (
            <React.Fragment>
              <button
                onClick={this.props.onCloseDrawer}
                className="btn btn-link text-decoration-none mr-4"
                type="button"
              >
                Cancel
              </button>
              <button className="btn btn-primary mt-2" type="submit">
                Add
              </button>
            </React.Fragment>
          )}
          {!this.props.isModal && (
            <div className="form-group my-2">
              <a
                href="JavaScript:Void(0)"
                onClick={() => {
                  this.props.onAddHostClick()
                }}
                className="btn btn-sm btn-outline-primary"
              >
                <small>
                  <i className="fa fa-plus mr-2" aria-hidden="true"></i>
                  Add Another Host
                </small>
              </a>
            </div>
          )}
        </div>
      </form>
    )
  }
}
