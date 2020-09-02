/*Third Party Imports */
import * as React from 'react'
import { Input, Select as AntdSelect, InputNumber, Switch, DatePicker } from 'antd';
import * as moment from 'moment';

/*Local Imports */
import { getCurrencyOptions } from '../../../utils/StripeConnectSupportedCountries';

export class ReplayForm extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
    }
  }

  componentDidMount() {
    this.setState({
      currencyOptions: getCurrencyOptions(this.props.props.currencyType.currencyTypes.currencyTypes, this.props.props.countryType.countryTypes.countries)
    })
  }

  render() {
    const { Option } = AntdSelect;
    let currencies = [];
    let activeCurrencies = this.state.currencyOptions || [];
    activeCurrencies.map((item) => {
      currencies.push(<Option value={`${item.value}+${item.label}`} key={`${item.value}+${item.label}`}>{item.label}</Option>)
    })
    return (
      <>
        <p className="font-weight-bold my-3">Would like to keep you experience available for <span className="text-blueberry">{this.props.isPaidReplay ? 'paid' : 'free'}</span> replay
          <Switch
            checked={this.props.replayViewModel.isEnabled}
            onChange={(e) => {
              this.props.onChangeReplaySwitch(e);
            }}
            className="ml-5" /></p>
        <p>Select the timeframe when you would like your experience to be available for {this.props.isPaidReplay ? 'paid' : 'free'} replay.</p>
        <div className="row">
          <div className="form-group mb-2 position-relative col-sm-4">
            <label className="d-block">From</label>
            <DatePicker
              className="w-100"
              {...((this.props.replayViewModel.isEnabled && this.props.replayViewModel.startDate) ? {
                value: moment(moment(this.props.replayViewModel.startDate).toDate(), 'MM/DD/YYYY')
              } : {})}
              onChange={this.props.onChangeStartDate}
              disabledDate={!this.props.startDate ? (d => !d || d.isBefore(new Date().setDate(new Date().getDate() - 1))) : (d => !d || d.isBefore(new Date(this.props.startDate)))}
              format="MM/DD/YYYY"
              placeholder="MM/DD/YYYY"
            />
          </div>
          <div className="form-group mb-2 position-relative col-sm-4">
            <label className="d-block">To</label>
            <DatePicker
              className="w-100"
              {...((this.props.replayViewModel.isEnabled && this.props.replayViewModel.endDate) ? {
                value: moment(moment(this.props.replayViewModel.endDate).toDate(), 'MM/DD/YYYY')
              } : {})}
              onChange={this.props.onChangeEndDate}
              disabledDate={!this.props.startDate ? (d => !d || d.isBefore(new Date().setDate(new Date().getDate() - 1))) : (d => !d || d.isBefore(new Date(this.props.startDate)))}
              format="MM/DD/YYYY"
              placeholder="MM/DD/YYYY"
            />
          </div>
          {(this.props.isPaidReplay) && <div className="col-sm-4">
            <label>Currency and price for replay</label>
            <Input.Group compact>
              <AntdSelect
                showSearch
                value={this.props.props.currencyType.currencyTypes.currencyTypes.filter((val) => {
                  return val.id == this.props.replayViewModel.currencyId
                })[0].code}
                onChange={(e) => {
                  let currencyId = e.split('+')[0];
                  this.props.onChangeCurrency(currencyId);
                }}
                style={{ width: 100 }} >
                {currencies}
              </AntdSelect>
              <InputNumber
                min={0}
                step={0.01}
                value={this.props.replayViewModel.isEnabled ? this.props.replayViewModel.price : 0}
                onChange={(e) => {
                  this.props.onChangePrice(e);
                }}
                placeholder='00.00'
                type="number" />
            </Input.Group>
          </div>}
        </div>

      </>
    )
  }
}

