import * as React from 'react'
import Select from 'react-select'
import * as _ from 'lodash'
import { TicketCategories } from '../../../../models/Inventory/InventoryRequestViewModel'
import { isSupportedCurrency } from '../../../../utils/StripeConnectSupportedCountries'

export default class EventHostForm extends React.Component<any, any> {
  priceInputStart: number
  constructor(props) {
    super(props)
    this.state = {}
    this.priceInputStart = 0
  }
  form: any

  getTicketDescription = (ticketCategoryId) => {
    if (ticketCategoryId == 1360) {
      return 'This allows you to view the experience/event'
    } else if (ticketCategoryId == 606) {
      return 'This access allows you to view the experience/event, and interact with the host/artist via chat. Please note, the host/artist will respond according to her/his schedule/program during the event/experience'
    } else if (ticketCategoryId == 19352 || ticketCategoryId == 12080) {
      return 'This access allows you to view the experience/event, and interact with the host/artist via chat and audio. Please note, the host/artist will respond according to her/his schedule/program during the event/experience'
    } else if (ticketCategoryId == 19350 || ticketCategoryId == 12079) {
      return 'This access allows you to view the experience/event, and interact with the host/artist via chat and video. Please note, the host/artist will respond according to her/his schedule/program during the event/experience'
    } else {
      return ''
    }
  }

  componentDidMount() {
    let ticketCategories = this.props.ticketCategoryDetails.ticketCategoryDetails.map((item, index) => {
      let findTicketCat = _.find(this.props.ticketCategoryDetails.ticketCategories, { id: item.ticketCategoryId })
      let ticketCat = {
        label: findTicketCat.name,
        value: item.ticketCategoryId,
        description: item.description
      }
      return ticketCat
    })
    var currencies = []
    var allSupportedcurrencies = []
    var nonUSDCurrencies = []
    let supportedCurrencies = this.props.currency
      .map((item, index) => {
        if (isSupportedCurrency(this.props.countries.countries, item.countryId) || item.countryId == 101) {
          return item
        }
      })
      .filter((item) => {
        return item != undefined
      })
    supportedCurrencies.map((val) => {
      if (val.code == 'USD') {
        allSupportedcurrencies.push(val)
      }
    })
    supportedCurrencies.map((val) => {
      if (val.code != 'USD') {
        nonUSDCurrencies.push(val)
      }
    })
    nonUSDCurrencies = nonUSDCurrencies.sort(function (a, b) {
      return a.code > b.code ? 1 : b.code > a.code ? -1 : 0
    })
    nonUSDCurrencies.map((val) => {
      allSupportedcurrencies.push(val)
    })
    currencies = allSupportedcurrencies.map((item) => {
      let currency = {
        label: `${item.code} (${capitalizeCurrency(item.name)})`,
        value: item.id,
        countryId: item.countryId
      }
      return currency
    })
    this.setState({
      ticketCategories: ticketCategories,
      currencies: currencies
    })
    if (this.props.formState) {
      let currenctTicketCat = ticketCategories.filter((item) => {
        return item.label == this.props.formState.categoryName
      })
      let currency = currencies.filter((item) => {
        return item.value == this.props.formState.currencyId
      })
      this.setState({
        currency: currency[0],
        ticketCat: currenctTicketCat[0],
        description: this.props.formState.description,
        price: this.props.formState.pricePerTicket,
        quantity: this.props.formState.quantity,
        eventTicketDetailId: this.props.formState.eventTicketDetailId
      })
    } else if (this.props.allTickets && this.props.allTickets.length > 0) {
      let currency = currencies.filter((item) => {
        return item.value == this.props.allTickets[0].currencyId
      })
      this.setState({
        currency: currency[0]
      })
    } else {
      let defaultCurrency = currencies.filter((val) => {
        return val.value == 231
      })
      this.setState({
        currency: defaultCurrency[0]
      })
    }
  }

  onPriceChange = (e) => {
    this.priceInputStart = e.target.selectionStart
    let val = e.target.value
    val = val.replace(/([^0-9.]+)/, '')
    val = val.replace(/^(0|\.)/, '')
    const match = /(\d{0,7})[^.]*((?:\.\d{0,2})?)/g.exec(val)
    const value = match[1] + match[2]
    e.target.value = value
    this.setState({ price: value }, () => {
      if (!this.props.isModal && this.state.ticketCat) {
        this.onSubmitPorps()
      }
    })
    if (val.length > 0) {
      e.target.value = Number(value).toFixed(2)
      e.target.setSelectionRange(this.priceInputStart, this.priceInputStart)
      this.setState({ price: Number(value).toFixed(2) }, () => {
        if (!this.props.isModal && this.state.ticketCat) {
          this.onSubmitPorps()
        }
      })
    }
  }

  onSubmitPorps = () => {
    let tc: TicketCategories = {
      categoryName: this.state.ticketCat.label,
      eventTicketDetailId: this.state.eventTicketDetailId ? this.state.eventTicketDetailId : 0,
      pricePerTicket: this.state.price,
      ticketCategoryId: this.state.ticketCat.value,
      isEventTicketAttributeUpdated: false,
      quantity: this.state.quantity ? this.state.quantity : 10000,
      ticketCategoryDescription: this.getTicketDescription(this.state.ticketCat.value),
      ticketCategoryNote: this.getTicketDescription(this.state.ticketCat.value),
      currencyId: this.state.currency.value,
      isRollingTicketValidityType: true,
      ticketValidityFixDate: '',
      days: '',
      month: '',
      year: '',
      ticketCategoryTypeId: 1,
      ticketSubCategoryTypeId: 1,
      currencyCountryId: this.state.currency.countryId
    }
    this.props.onSubmit(tc)
  }

  render() {
    return (
      <form
        onSubmit={(values: any) => {
          values.preventDefault()
          values.stopPropagation()
          this.onSubmitPorps()
        }}
        ref={(ref) => {
          this.form = ref
        }}
      >
        <div className="col-sm-12 p-2">
          <div className="collapse multi-collapse show pt-3" id="Ticket">
            <div className="form-group">
              <div className="row">
                <div className="col-sm-4">
                  <label>Ticket Category</label>
                  <Select
                    name="category"
                    placeholder="Select Ticket Category"
                    value={this.state.ticketCat}
                    options={this.state.ticketCategories}
                    onChange={(item) => {
                      this.setState({ ticketCat: item })
                    }}
                    required
                  />
                </div>
                <div className="col-sm-4">
                  <label>Currency</label>
                  <Select
                    name="category"
                    value={this.state.currency}
                    options={this.state.currencies}
                    onChange={(item) => {
                      this.setState({ currency: item })
                    }}
                    required
                  />
                </div>
                <div className="col-sm-4">
                  <label>Price</label>
                  <input
                    type="text"
                    onChange={this.onPriceChange}
                    className="form-control"
                    placeholder="00.00"
                    value={this.state.price}
                    required
                  />
                </div>
              </div>
            </div>
            <div className="form-group">
              {this.state.ticketCat && this.state.ticketCat == 19352 && (
                <div className="col-12">
                  <label>Quantity</label>
                  <input
                    name="quantity"
                    type="text"
                    className="form-control"
                    placeholder="Enter ticket quantity"
                    onChange={(e) => {
                      this.setState({ quantity: e.target.value })
                    }}
                    value={this.state.quantity}
                    disabled={false}
                  />
                </div>
              )}
              {this.state.ticketCat && (
                <div>
                  <label>Ticket Description</label>
                  <div
                    className="alert alert-dark"
                    dangerouslySetInnerHTML={{ __html: this.state.ticketCat.description }}
                  />
                </div>
              )}
            </div>
          </div>
          {this.props.isModal && (
            <div className="form-group my-2">
              <button type="submit" className="btn btn-sm btn-outline-primary">
                <small>
                  <i className="fa fa-plus mr-2" aria-hidden="true"></i>
                  Add
                </small>
              </button>
            </div>
          )}
        </div>
      </form>
    )
  }
}

/**
 * format currency names as per FIL
 * @param str takes currency name in string format
 */
function capitalizeCurrency(str: String) {
    if ("swiss frnc" == str.toLowerCase()) {
        str = "swiss franc"
    }
    var currency: any = str.split(" ");
    if (currency.length > 1) {
        currency[0] = currency[0].charAt(0).toUpperCase() + currency[0].slice(1);
        currency[currency.length - 1] = currency[currency.length - 1].charAt(0).toLowerCase() + currency[currency.length - 1].slice(1);
        currency = currency.join(" ");
    } else {
        currency[0] = currency[0].toLowerCase();
        currency[0] = currency[0].charAt(0).toUpperCase() + currency[0].slice(1);
    }
    return currency;
}
