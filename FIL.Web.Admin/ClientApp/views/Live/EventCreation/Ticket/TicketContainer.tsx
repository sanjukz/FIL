import * as React from 'react'
import * as _ from 'lodash'
import TicketForm from './TicketForm'
import { TicketCategories, InventoryRequestViewModel } from '../../../../models/Inventory/InventoryRequestViewModel'
import { Modal as antModal, Button, notification, Tooltip, Drawer } from 'antd'

export default class TicketContainder extends React.Component<any, any> {
  state = {
    isModalShow: false,
    selectedTicket: null,
    isShow: false,
    isSuccess: false,
    confirmMessage: '',
    isShowConfirmPopup: false,
    visible: false,
    isSetDefaultForm: false,
    allTickets: []
  }

  setSelectedTicket = (ticket) => {
    this.setState({
      selectedTicket: { ...ticket },
      isModalShow: true
    })
  }

  setAllTickets = (tickets) => {
    this.setState({
      allTickets: [...tickets]
    })
  }

  setModalShow = (flag) => {
    this.setState({
      isModalShow: flag
    })
  }

  findAndReplace = (host, currenctTIcket, categoryName) => {
    host = host.map((item) => {
      if (item.categoryName == categoryName) {
        item.categoryName = currenctTIcket.categoryName
        item.quantity = currenctTIcket.quantity
        item.currencyId = currenctTIcket.currencyId
        item.pricePerTicket = currenctTIcket.pricePerTicket
      }
      return item
    })
    return host
  }

  Confirm = (InventoryModel, ticketCategoryData) => {
    var that = this
    that.props.inventoryDataSubmit(InventoryModel, (response: any) => {
      if (response.success) {
        sessionStorage.setItem('countryId', ticketCategoryData.currencyCountryId.toString())
        that.setState(
          {
            confirmMessage: 'Your ticket information has been saved successfully!'
          },
          () => {
            that.success('success')
          }
        )
      } else {
        that.setState(
          {
            confirmMessage: 'Your ticket information has not been saved! Please try again.'
          },
          () => {
            that.error()
          }
        )
      }
    })
  }

  setTicketCat = (item) => {
    let allTickets = this.state.allTickets
    let currentHost = allTickets.filter((val) => val.categoryName == item.categoryName)
    if (currentHost.length > 0) {
      allTickets = this.findAndReplace(allTickets, item, currentHost[0].categoryName)
    } else {
      allTickets.push(item)
    }
    this.setState({ allTickets: allTickets })
  }

  error = () => {
    antModal.error({
      title: this.state.confirmMessage,
      centered: true
    })
  }

  success = (type) => {
    let that = this
    notification[type]({
      message: that.state.confirmMessage,
      duration: 10,
      top: 10
    })
    if (!this.props.isEdit) {
      this.props.onSubmitTicketCreation()
    }
  }

  onSubmitTickets = () => {
    let ticketCategoryData = []
    ticketCategoryData = this.state.allTickets
    let currencyLength = ticketCategoryData
      .map((val) => {
        return val.currencyId
      })
      .filter(function (value, index, self) {
        return self.indexOf(value) === index
      })
    if (currencyLength.length > 1) {
      this.setState(
        {
          confirmMessage: 'You can use only one currency per event. Please edit the currency selection.'
        },
        () => {
          this.error()
        }
      )
      return false
    }
    let InventoryModel: InventoryRequestViewModel = {
      ticketCategoriesViewModels: ticketCategoryData,
      deliverTypeId: [1],
      placeAltId: this.props.isEdit
        ? this.props.placeAltId
        : localStorage.getItem('placeAltId')
        ? localStorage.getItem('placeAltId')
        : null,
      redemptionDateTime: '',
      redemptionInstructions: '',
      termsAndCondition: '',
      customerIdTypes: [],
      isCustomerIdRequired: false,
      eventDetailAltIds: [],
      redemptionAddress: 'test',
      redemptionCity: 'test',
      redemptionState: 'test',
      redemptionCountry: 'test',
      redemptionZipcode: '000',
      refundPolicy: 0,
      customerInformation: [0],
      isEdit: false,
      feeTypes: []
    }
    let isEventCreated = sessionStorage.getItem('isEventDetailCreated')
    if (isEventCreated) {
      this.props.onSubmitTicketList(this.state.allTickets, () => this.Confirm(InventoryModel, ticketCategoryData[0]))
    } else {
      this.setState(
        {
          confirmMessage: 'Please go back and completely fill and save the information in the Event Details tab.'
        },
        () => {
          this.error()
        }
      )
    }
  }

  setInventortData = () => {
    let allTickets = []
    this.props.inventoryData.ticketCategoryContainer.map((val) => {
      let currency = this.props.currency.filter((currency) => {
        return currency.id == val.eventTicketAttribute.currencyId
      })
      let tc: TicketCategories = {
        categoryName: val.ticketCategory.name,
        eventTicketDetailId: val.eventTicketDetail.id,
        pricePerTicket: val.eventTicketAttribute.price,
        ticketCategoryId: val.ticketCategory.id,
        isEventTicketAttributeUpdated: false,
        quantity: val.eventTicketAttribute.remainingTicketForSale,
        ticketCategoryDescription: val.eventTicketAttribute.ticketCategoryDescription,
        ticketCategoryNote: val.eventTicketAttribute.ticketCategoryNotes,
        currencyId: val.eventTicketAttribute.currencyId,
        isRollingTicketValidityType: true,
        ticketValidityFixDate: '',
        days: '',
        month: '',
        year: '',
        ticketCategoryTypeId: val.ticketCategoryTypeId,
        ticketSubCategoryTypeId: 1,
        currencyCountryId: currency[0].countryId
      }
      allTickets.push(tc)
    })
    this.setState({
      allTickets: allTickets,
      selectedTicket: allTickets.length == 1 ? allTickets[0] : undefined,
      isSetDefaultForm: true
    })
  }

  render() {
    let that = this
    if (
      this.props.isEdit &&
      !this.state.isSetDefaultForm &&
      this.props.inventoryData &&
      this.props.inventoryData.ticketCategoryContainer
    ) {
      this.setInventortData()
    }
    return (
      <div className="p-3">
        <a
          className="place-listing active"
          data-toggle="collapse"
          href="#Ticket"
          role="button"
          aria-expanded="false"
          aria-controls="Ticket"
        >
          <span className="rounded-circle border border-primary">1</span>
          Ticket Pricing
        </a>
        <Tooltip
          title={
            <p>
              <span>
                The categories listed in the options below are in the order of typically lowest priced tickets to the
                highest price. Each describes the level of interaction that the audience will have with the host. So,
                General Admission would be only viewing, whereas VIP would allow the audience members to send in
                comments during the live stream. Exclusive Access and Backstage Pass will allow a select few to do a
                live Q&amp;A with the host
              </span>
            </p>
          }
        >
          <span>
            <i className="fa fa-info-circle text-primary ml-2"></i>
          </span>
        </Tooltip>
        {this.state.allTickets.length <= 1 &&
          this.props.currency &&
          this.props.countries.countries &&
          this.props.countries.countries.length > 0 &&
          this.props.currency.length > 0 &&
          this.props.ticketCategoryDetails.ticketCategoryDetails.length > 0 && (
            <div>
              <TicketForm
                currency={this.props.currency}
                allTickets={this.state.allTickets}
                formState={this.state.selectedTicket}
                ticketCategoryDetails={this.props.ticketCategoryDetails}
                countries={this.props.countries}
                onSubmit={(item: any) => {
                  this.setTicketCat(item)
                }}
              />{' '}
            </div>
          )}

        {this.state.allTickets.length > 1 && (
          <div>
            {this.state.allTickets
              .filter((val) => {
                return val.ticketCategoryTypeId != 2
              })
              .map((item, index) => {
                let currency = this.props.currency.filter((t) => t.id == item.currencyId)[0]
                return (
                  <div key={item.categoryName} className="card col-sm-6 p-0 my-2">
                    <div className="card-header border-0 p-2">
                      <a
                        href="javascript:void(0)"
                        onClick={() => {
                          that.setState({ selectedTicket: item, visible: true })
                        }}
                      >{`${item.categoryName} - ${currency.code} ${item.pricePerTicket}`}</a>
                      <div className="pull-right">
                        <a href="javascript:void(0)">
                          <i
                            className="fa fa-trash-o text-danger"
                            onClick={() => {
                              let allTkt = [...this.state.allTickets]
                              _.remove(allTkt, (t) => t.categoryName == item.categoryName)
                              if (allTkt.length == 1) {
                                this.setState({ selectedTicket: allTkt[0] })
                              }
                              this.setAllTickets(allTkt)
                            }}
                          ></i>
                        </a>
                      </div>
                    </div>
                  </div>
                )
              })}
          </div>
        )}
        {this.state.allTickets.length > 0 &&
          this.state.allTickets.length != this.props.ticketCategoryDetails.ticketCategoryDetails.length && (
            <div className="px-2 form-group">
              <a
                href="JavaScript:Void(0)"
                onClick={(item) => {
                  that.setState({ visible: true, selectedTicket: undefined })
                }}
                className="btn btn-sm btn-outline-primary"
              >
                <small>
                  <i className="fa fa-plus mr-2" aria-hidden="true"></i>
                  Add Another Ticket Category
                </small>
              </a>
            </div>
          )}
        <Drawer
          title="Ticket Information"
          placement={'right'}
          closable={false}
          onClose={() => {
            this.setState({ visible: false })
          }}
          visible={this.state.visible}
          width={700}
        >
          {this.state.visible ? (
            <TicketForm
              currency={this.props.currency}
              allTickets={this.state.allTickets}
              formState={this.state.selectedTicket}
              isModal={true}
              ticketCategoryDetails={this.props.ticketCategoryDetails}
              countries={this.props.countries}
              onSubmit={(item: any) => {
                this.setTicketCat(item)
                this.setState({ visible: false })
              }}
            />
          ) : (
            <div></div>
          )}
        </Drawer>
        <div className="text-center pt-4 pb-4">
          <div className="line" />
          <a
            href="JavaScript:Void(0)"
            onClick={() => {
              this.props.onClickCancel()
            }}
            className="text-decoration-none mr-4"
          >
            Back
          </a>
          <div className="btn-group">
            <button
              disabled={this.state.allTickets.length == 0 ? true : false}
              type="submit"
              className="btn btn-outline-primary"
              onClick={this.onSubmitTickets}
            >
              Save & continue
            </button>
          </div>
        </div>
      </div>
    )
  }
}
