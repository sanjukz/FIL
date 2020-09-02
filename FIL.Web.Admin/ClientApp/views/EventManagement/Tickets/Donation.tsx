/* Third party imports */
import * as React from 'react'
import { Input, Select as AntdSelect, InputNumber, Switch } from 'antd';

/* Local Imports */
import { getCurrencyOptions } from '../../../utils/StripeConnectSupportedCountries';
import { Footer } from '../Footer/FormFooter';
import { setTicketModelObject } from '../utils/DefaultObjectSetter';

const GetCurrencyComponent = (selectedTicket: any, currencyOptions: any) => {
  return <AntdSelect
    showSearch
    style={{ width: 100 }}
    value={selectedTicket.currencyCode ? selectedTicket.currencyCode : "USD"}
    onChange={(e: any) => {
      let key = e;
      let ticketModel = selectedTicket;
      ticketModel.currencyCode = key.split("+")[1];
      ticketModel.currencyId = +key.split("+")[0];
      this.setState({ ticketModel: ticketModel });
    }}>
    {currencyOptions}
  </AntdSelect>
}

export class Donation extends React.Component<any, any>{
  constructor(props) {
    super(props);
    this.state = {
      selectedTicket: setTicketModelObject(1),
      currencyOptions: []
    }
  }

  componentDidMount() {
    const { Option } = AntdSelect;
    let currency = getCurrencyOptions(this.props.props.currencyType.currencyTypes.currencyTypes, this.props.props.countryType.countryTypes.countries);
    let activeCurrencies = currency || [];
    let currentOptions = [];
    activeCurrencies.map((item) => {
      currentOptions.push(<Option value={`${item.value}+${item.label}`} key={`${item.value}+${item.label}`}>{item.label}</Option>)
    })
    let ticket = this.props.ticketViewModel.tickets.filter((val: any) => { return val.ticketCategoryId == this.props.donateTicketCatId });
    if (ticket.length > 0) {
      this.setState({ selectedTicket: ticket[0] })
    } else {
      let ticket = this.state.selectedTicket;
      ticket.isEnabled = false;
      this.setState({ selectedTicket: ticket });
    }
    this.setState({ currencyOptions: currentOptions });
  }

  render() {
    return <>
      <div className="mb-3">
        <label>Active Donate</label>
        <div className="d-inline-block pl-3">
          <Switch checked={this.state.selectedTicket.isEnabled} onChange={(check) => {
            let ticket = this.state.selectedTicket;
            ticket.isEnabled = check;
            this.setState({ ticket: ticket });
          }} />
        </div>
        <div className="media bg-light rounded-box mb-3 mt-2">
          <img className="align-self-center mx-3" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/addons-note.svg" alt="Donate Image" />
          <div className="media-body p-3">
            <p className="mb-0 font-weight-bold">Please note:</p>Your customers will have the opportunity to donate using the 3 pre-set amounts your stipulate below or by entering their own amount.  </div>
        </div>
        <div className="row">
          <div className="col-sm-4">
            <label>Donation Amount 1</label>
            <Input.Group compact>
              {GetCurrencyComponent(this.state.selectedTicket, this.state.currencyOptions)}
              <InputNumber
                min={0}
                step={0.01}
                placeholder='00.00'
                value={this.state.selectedTicket.donationAmount1}
                type="number"
                onChange={(e) => {
                  let ticketModel = this.state.selectedTicket;
                  ticketModel.donationAmount1 = e as any;
                  this.setState({ ticket: ticketModel });
                }} />
            </Input.Group>
          </div>
          <div className="col-sm-4">
            <label>Donation Amount 1</label>
            <Input.Group compact>
              {GetCurrencyComponent(this.state.selectedTicket, this.state.currencyOptions)}
              <InputNumber
                min={0}
                step={0.01}
                placeholder='00.00'
                value={this.state.selectedTicket.donationAmount2}
                type="number"
                onChange={(e) => {
                  let ticketModel = this.state.selectedTicket;
                  ticketModel.donationAmount2 = e as any;
                  this.setState({ ticket: ticketModel });
                }} />
            </Input.Group>
          </div>
          <div className="col-sm-4">
            <label>Donation Amount 2</label>
            <Input.Group compact>
              {GetCurrencyComponent(this.state.selectedTicket, this.state.currencyOptions)}
              <InputNumber
                min={0}
                step={0.01}
                placeholder='00.00'
                value={this.state.selectedTicket.donationAmount3}
                type="number"
                onChange={(e) => {
                  let ticketModel = this.state.selectedTicket;
                  ticketModel.donationAmount3 = e as any;
                  this.setState({ ticket: ticketModel });
                }} />
            </Input.Group>
          </div>
        </div>
        <Footer
          isDisabled={false}
          isShowSkip={this.props.isAddOn}
          saveText={'Save Donate'}
          onClickSkip={() => { this.props.onClickSkip() }}
          isSaveRequest={this.props.props.EventTickets.isSaveRequest}
          onClickCancel={() => { this.props.onClickCancel() }}
          isHideCancel={true}
          onSubmit={() => {
            let ticket = this.state.selectedTicket;
            ticket.ticketCategoryId = this.props.donateTicketCatId;
            ticket.ticketCategoryTypeId = 1;
            ticket.name = "Donate";
            ticket.quantity = 500;
            this.setState({ ticket: ticket }, () => {
              this.props.onSubmit(this.state.selectedTicket)
            });
          }} />
      </div>
    </>
  }
}

