import * as React from 'react'
import Modal from 'react-awesome-modal'
import * as _ from 'lodash'
import EventHostForm from './EventHostForm'
import { Drawer } from 'antd'
import { HostRequestModel } from '../../../../models/CreateEvent/Host'

export default class EventHost extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
      isShow: false,
      currentHostId: -1,
      host: []
    }
  }

  onCloseDrawer = () => {
    this.setState({
      visible: false
    })
  }

  findAndReplace = (host, currenctHost, email) => {
    host = host.map((item) => {
      if (item.email == email) {
        item.firstName = currenctHost.firstName
        item.lastName = currenctHost.lastName
        item.email = currenctHost.email
        item.description = currenctHost.description
        item.imageDetail = currenctHost.imageDetail
      }
      return item
    })
    return host
  }

  setHost = (item) => {
    let host = this.state.host
    let currentHost = host.filter((val, index) => {
      return val.email == item.email
    })
    if (currentHost.length > 0) {
      host = this.findAndReplace(host, item, currentHost[0].email)
    } else {
      host.push(item)
    }
    //host = host.map((item, index) => { item.id = ((this.props.isEdit && item.id != -1) ? item.id : index); return item; });
    this.setState({ host: host })
    if (host.length == 1) {
      this.setState({ currentHostId: host[0].id })
    }
    this.props.onSubmitHost(host)
  }

  setDefaultHost = () => {
    let hostArray = []
    this.props.placeDetails.eventHostMappings.map((item) => {
      let hostFormData: HostRequestModel = {
        id: item.id,
        altId: item.altId.toUpperCase(),
        description: item.description,
        email: item.email,
        firstName: item.firstName,
        lastName: item.lastName,
        eventId: item.eventId
      }
      hostArray.push(hostFormData)
    })
    this.setState(
      { host: hostArray, selectedHost: hostArray.length == 1 ? hostArray[0] : undefined, isSetDefaultForm: true },
      () => {
        this.props.onSubmitHost(hostArray)
      }
    )
  }

  render() {
    let that = this
    if (
      this.props.isEdit &&
      !this.state.isSetDefaultForm &&
      this.props.placeDetails &&
      this.props.placeDetails.eventHostMappings
    ) {
      this.setDefaultHost()
    }
    return (
      <div>
        {this.state.host.length < 2 && (
          <div>
            <EventHostForm
              formState={this.state.selectedHost}
              currentHostId={this.state.currentHostId}
              session={this.props.session}
              onAddHostClick={() => {
                this.setState({
                  visible: true,
                  currentHostId: -1,
                  selectedHost: undefined
                })
              }}
              onCloseDrawer={this.onCloseDrawer}
              onSubmit={(item: any) => {
                this.setHost(item)
              }}
            />
          </div>
        )}
        {this.state.host.length > 1 &&
          this.state.host.map((item, index) => {
            return (
              <div key={index} className="card col-sm-6 p-0 mb-2">
                <div className="card-header border-0 p-2">
                  <a
                    href="javascript:void(0)"
                    onClick={() => {
                      that.setState({ selectedHost: item, visible: true, currentHostId: item.id })
                    }}
                  >
                    {item.firstName} {item.lastName}
                  </a>
                  <div
                    className="pull-right"
                    onClick={() => {
                      let host = that.state.host.filter((val) => {
                        return val.id != item.id
                      })
                      that.setState({ host: host, selectedHost: host.length > 0 ? host[0] : this.state.selectedHost })
                    }}
                  >
                    <a href="javascript:void(0)">
                      <i className="fa fa-trash-o text-danger"></i>
                    </a>
                  </div>
                </div>
              </div>
            )
          })}
        <Drawer
          title="Host Information"
          placement={'right'}
          closable={false}
          onClose={this.onCloseDrawer}
          visible={this.state.visible}
          width={800}
        >
          {this.state.visible ? (
            <EventHostForm
              session={this.props.session}
              onAddHostClick={() => {
                this.setState({
                  visible: true,
                  currentHostId: -1,
                  selectedHost: undefined
                })
              }}
              isModal={true}
              formState={this.state.selectedHost}
              currentHostId={this.state.currentHostId}
              onSubmit={(item: any) => {
                this.setHost(item)
                this.setState({ visible: false })
              }}
            />
          ) : (
            <div></div>
          )}
        </Drawer>
      </div>
    )
  }
}
