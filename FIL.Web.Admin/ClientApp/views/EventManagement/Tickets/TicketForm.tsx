/* Third party imports */
import * as React from 'react'
import * as _ from 'lodash'
import * as numeral from "numeral";
import { Input, Select as AntdSelect, InputNumber, Switch } from 'antd';

/* Local imports */
import { setTicketModelObject } from "../utils/DefaultObjectSetter";
import ImageUpload from "../../../components/ImageUpload/ImageUpload";
import { getCurrencyOptions } from '../../../utils/StripeConnectSupportedCountries';
import { getOnlineTicketCategoryOption, getTicketDescription, getTicketCategoryNotes } from '../../../utils/OnlineTicketCategories';
import { Footer } from "../Footer/FormFooter";
import { showDeleteConfirm, error } from "../Confirmation";
import { DiscountForm } from "./DiscountForm";
import { DiscountValueType } from '../../../Enums/DiscountType';

export default class TicketForm extends React.Component<any, any> {
  priceInputStart: number
  constructor(props) {
    super(props)
    this.state = {
      ticketModel: setTicketModelObject(this.props.isAddOn ? 2 : 1)
    }
    this.priceInputStart = 0
  }
  form: any

  componentDidMount() {
    if (this.props.selectedTicketId != 0) {
      let selectedTicket = this.props.ticketViewModel.tickets.filter((val: any) => { return val.etdId == this.props.selectedTicketId });
      if (selectedTicket.length > 0) {
        this.setState({ ticketModel: { ...selectedTicket[0] } });
      } else if (this.props.ticketViewModel.tickets.length > 0) {
        let ticketModel = this.state.ticketModel;
        ticketModel.currencyId = this.props.ticketViewModel.tickets[0].currencyId;
        ticketModel.currencyCode = this.props.ticketViewModel.tickets[0].currencyCode;
        this.setState({ ticketModel: ticketModel });
      }
    }
    this.setState({
      ticketCategories: getOnlineTicketCategoryOption(this.props.props.inventory.ticketCategoryDetails),
      currencyOptions: getCurrencyOptions(this.props.props.currencyType.currencyTypes.currencyTypes, this.props.props.countryType.countryTypes.countries)
    })
  }

  isButtonDisable = () => {
    return (this.state.ticketModel.name && this.state.ticketModel.quantity && (this.state.ticketModel.promoCode ? (this.state.ticketModel.discountValueType ? (this.state.ticketModel.discountValueType == DiscountValueType.Flat ? this.state.ticketModel.discountAmount : this.state.ticketModel.discountPercentage) : false) : true));
  }

  calculateClientMoney = () => {
    if (this.state.ticketModel.price > 0) {
      let clientMoney = this.state.ticketModel.price - (20 / 100) * this.state.ticketModel.price;
      return <><div className="text-success f-14">What you will earn: {' ' + this.state.ticketModel.currencyCode} {numeral(clientMoney).format("00.00")}</div></>
    }
  }

  checkIsDiffCurrency = () => {
    let isDiffCurrency = false;
    this.props.ticketViewModel.tickets.forEach((val: any) => {
      if (val.currencyId != this.state.ticketModel.currencyId) {
        isDiffCurrency = true;
        return;
      }
    });
    return isDiffCurrency;
  }

  checkIsSameTicketCategory = () => {
    let isSameTicketCat = false;
    this.props.ticketViewModel.tickets.forEach((val: any) => {
      if (val.ticketCategoryId == this.state.ticketModel.ticketCategoryId && val.etdId != this.state.ticketModel.etdId) {
        isSameTicketCat = true;
        return;
      }
    });
    return isSameTicketCat;
  }

  render() {
    const { Option } = AntdSelect;
    let ticketCategories = [];
    let ticketCats = this.state.ticketCategories || [];
    let currencies = [];
    let activeCurrencies = this.state.currencyOptions || [];
    ticketCats.map((item) => {
      ticketCategories.push(<Option value={`${item.value}+${item.label}`} key={`${item.value}+${item.label}`}>{item.label}</Option>)
    })
    activeCurrencies.map((item) => {
      currencies.push(<Option value={`${item.value}+${item.label}`} key={`${item.value}+${item.label}`}>{item.label}</Option>)
    })
    return (
      <div data-aos="fade-up">
        <form
          onSubmit={(values: any) => {
            values.preventDefault();
            values.stopPropagation();
            if (this.checkIsDiffCurrency() && !this.checkIsSameTicketCategory()) {
              let object = {
                id: this.state.ticketModel.etdId,
                message: 'You can keep only one currency per event. This will change the current slected currency for all tickets and add-on',
                onDelete: (e: any) => {
                  this.props.onSubmit(this.state.ticketModel);
                }
              }
              showDeleteConfirm(object);
            } else if (this.checkIsSameTicketCategory()) {
              error(`${this.state.ticketModel.name} ticket category is already created, please select different ticket category.`);
            } else {
              this.props.onSubmit(this.state.ticketModel);
            }

          }}
          ref={(ref) => {
            this.form = ref
          }}
        >
          <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
            <h3 className="m-0 text-purple">{this.props.isAddOn ? `${(this.props.selectedTicketId == 0 && this.props.ticketViewModel.tickets.length > 0) ? 'Add more add-ons' : 'Maximise your experience revenue through add-ons'}` : `${(this.props.selectedTicketId == 0 && this.props.ticketViewModel.tickets.length > 0) ? 'Add more tickets' : 'Get them in!'}`}</h3></nav>
          <div className="mb-3">
            <label>{this.props.isAddOn ? 'Active Add-on' : 'Active ticket'}</label>
            <div className="d-inline-block pl-3">
              <Switch checked={this.state.ticketModel.isEnabled} onChange={(check) => {
                let ticketModel = this.state.ticketModel;
                ticketModel.isEnabled = check
                this.setState({ ticketModel: ticketModel })
              }} />
            </div>
          </div>
          <>
            <div className="form-group m-0">
              <div className="row">
                <div className="col-sm-4">
                  {this.props.isAddOn ? <>
                    <label className="d-block">Add-on Name</label>
                    <input
                      name={this.props.isAddOn ? 'Enter add-on name' : 'name'}
                      type="text"
                      className="form-control"
                      placeholder="Name"
                      value={this.state.ticketModel.name ? this.state.ticketModel.name : ""}
                      onChange={(e) => {
                        let ticketModel = this.state.ticketModel;
                        ticketModel.name = e.target.value;
                        this.setState({ selectedTicket: ticketModel })
                      }}
                    />
                  </> : <><label className="d-block">Ticket Category</label><AntdSelect
                    showSearch
                    size={"large"}
                    style={{ width: "100%" }}
                    value={this.state.ticketModel.name ? this.state.ticketModel.name : "Select Ticket Category"}
                    onChange={(e: any) => {
                      let key = e;
                      let ticketModel = this.state.ticketModel;
                      ticketModel.name = key.split("+")[1];
                      ticketModel.ticketCategoryId = key.split("+")[0];
                      ticketModel.description = getTicketDescription(ticketModel.ticketCategoryId);
                      ticketModel.ticketCategoryNotes = getTicketCategoryNotes(ticketModel.ticketCategoryId);
                      this.setState({ ticketModel: ticketModel });
                    }}>
                    {ticketCategories}
                  </AntdSelect></>}
                </div>
                <div className="col-sm-4">
                  <label>Quantity</label>
                  <input
                    name="quantity"
                    type="number"
                    className="form-control"
                    placeholder="Enter quantity"
                    value={this.state.ticketModel.quantity ? this.state.ticketModel.quantity : ""}
                    onChange={(e) => {
                      let ticketModel = this.state.ticketModel;
                      ticketModel.quantity = e.target.value;
                      this.setState({ selectedTicket: ticketModel })
                    }}
                    disabled={false}
                  />
                </div>
                <div className="col-sm-4">
                  <label>Currency & Price</label>
                  <Input.Group compact>
                    <AntdSelect
                      showSearch
                      style={{ width: 100 }}
                      value={this.state.ticketModel.currencyCode ? this.state.ticketModel.currencyCode : "USD"}
                      onChange={(e: any) => {
                        let key = e;
                        let ticketModel = this.state.ticketModel;
                        ticketModel.currencyCode = key.split("+")[1];
                        ticketModel.currencyId = +key.split("+")[0];
                        this.setState({ ticketModel: ticketModel });
                      }}>
                      {currencies}
                    </AntdSelect>
                    <InputNumber
                      min={0}
                      step={0.01}
                      placeholder='00.00'
                      value={this.state.ticketModel.price}
                      type="number"
                      onChange={(e) => {
                        let ticketModel = this.state.ticketModel;
                        ticketModel.price = e;
                        this.setState({ selectedTicket: ticketModel })
                      }} />
                  </Input.Group>
                  {this.calculateClientMoney()}
                </div>
              </div>
            </div>
            <div className="form-group">
              <div className="row">
              </div>
            </div>
            {(this.state.ticketModel.description && !this.props.isAddOn) ?
              <div>
                <label>Ticket Description</label>
                <div className="alert bg-light f-14 p-2 mb-0">
                  <img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-info.svg" />
                  <span
                    dangerouslySetInnerHTML={{ __html: this.state.ticketModel.description }}
                  />
                </div>
              </div> : <></>}
            {this.props.isAddOn ?
              <div className="row">
                <div className="col-sm-6">
                  <ImageUpload
                    imageInputList={[{ imageType: "addon", numberOfFields: 1, imageKey: this.state.ticketModel.ticketAltId ? this.state.ticketModel.ticketAltId.toUpperCase() : "" }]}
                    onImageSelect={(imageModel) => {
                      this.props.onImageSelect(imageModel);
                    }}
                  />
                </div>
              </div> : <></>}
            {this.state.ticketModel && <DiscountForm
              ticketModel={this.state.ticketModel}
              currencies={currencies}
              onChangePromoCodeSwitch={(check) => {
                let ticketModel = this.state.ticketModel;
                ticketModel.isDiscountEnable = check;
                this.setState({ selectedTicket: ticketModel })
              }}
              onChangePromoCode={(value) => {
                let ticketModel = this.state.ticketModel;
                ticketModel.promoCode = value;
                this.setState({ selectedTicket: ticketModel })
              }}
              onChangePromoCodeType={(promoCodeType) => {
                let ticketModel = this.state.ticketModel;
                ticketModel.discountValueType = promoCodeType;
                this.setState({ selectedTicket: ticketModel })
              }}
              onChangePromoCodeCurreny={(e: any) => {
                let key = e;
                let ticketModel = this.state.ticketModel;
                ticketModel.currencyCode = key.split("+")[1];
                ticketModel.currencyId = +key.split("+")[0];
                this.setState({ ticketModel: ticketModel });
              }}
              onChangeDiscountAmount={(value) => {
                let ticketModel = this.state.ticketModel;
                ticketModel.discountAmount = value;
                this.setState({ ticketModel: ticketModel });
              }}
              onChangeDiscountPercentage={(value) => {
                let ticketModel = this.state.ticketModel;
                ticketModel.discountPercentage = value;
                this.setState({ ticketModel: ticketModel });
              }}
            />}
          </>
          <Footer
            isDisabled={!this.isButtonDisable()}
            isShowSkip={this.props.isAddOn}
            saveText={this.props.isAddOn ? 'Save Add-on' : 'Save Ticket'}
            onClickSkip={() => { this.props.onClickSkip() }}
            isSaveRequest={this.props.props.EventTickets.isSaveRequest}
            onClickCancel={() => { this.props.onClickCancel() }}
            onSubmit={() => { this.form.dispatchEvent(new Event('submit')) }} />
        </form>
      </div>
    )
  }
}
