/* Third party imports */
import * as React from 'react'
import * as _ from 'lodash'
import { Input, Select as AntdSelect, InputNumber, Switch } from 'antd';

/* Local imports */
import { ToolTip } from "../ToolTip";
import { DiscountValueType } from '../../../Enums/DiscountType';

export const DiscountForm: React.FC<any> = (props: any) => {
  const [isShowAdditionalSettings, setAdditionalSettings] = React.useState(false);
  const { Option } = AntdSelect;
  return (<div data-aos="fade-up">
    <nav onClick={() => { setAdditionalSettings(!isShowAdditionalSettings) }} style={{ cursor: 'pointer' }} className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
      <h5 className="mb-0 mt-4">Additional ticket settings
      <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/head-right-arrow.svg" className="ml-2" />
      </h5></nav>
    {isShowAdditionalSettings && <>
      <div className="mb-3">
        <label>Active promo code</label>
        <div className="d-inline-block pl-3">
          <Switch checked={props.ticketModel.isDiscountEnable} onChange={(check) => {
            props.onChangePromoCodeSwitch(check);
          }} />
        </div>
      </div>
      <div className="form-group m-0">
        <div className="row">
          <div className="col-sm-4">
            <label className="d-block">Ticket promo code (optional)</label>
            <input
              name={'Enter promocode code'}
              type="text"
              className="form-control"
              placeholder="Enter promocode code (letters and numbers only)"
              value={props.ticketModel.promoCode ? props.ticketModel.promoCode : ""}
              onChange={(e) => {
                props.onChangePromoCode(e.target.value);
              }}
            />
          </div>
          <div className="col-sm-4">
            <label className="d-block">Promo code type</label>
            <AntdSelect
              placeholder="Select type"
              style={{ width: '100%' }}
              value={props.ticketModel.discountValueType ? props.ticketModel.discountValueType == DiscountValueType.Flat ? 'Flat' : 'Percentage' : "Select type"}
              onChange={(e) => {
                props.onChangePromoCodeType(e)
              }}
            >
              <Option value="1">Percentage</Option>
              <Option value="2">Flat</Option>
            </AntdSelect>
          </div>
          <div className="col-sm-4">
            {!(props.ticketModel.discountValueType == DiscountValueType.Percentage) ? <> <label className="d-block">Currency & Price</label>
              <Input.Group compact>
                <AntdSelect
                  showSearch
                  style={{ width: 100 }}
                  value={props.ticketModel.currencyCode ? props.ticketModel.currencyCode : "USD"}
                  onChange={(e: any) => {
                    props.onChangePromoCodeCurreny(e)
                  }}>
                  {props.currencies}
                </AntdSelect>
                <InputNumber
                  min={0}
                  step={0.01}
                  placeholder='00.00'
                  value={props.ticketModel.discountAmount}
                  type="number"
                  onChange={(e) => {
                    props.onChangeDiscountAmount(e)
                  }} />
              </Input.Group> </> : <> <label className="d-block">Percentage</label><InputNumber
                min={0}
                max={100}
                value={props.ticketModel.discountPercentage ? props.ticketModel.discountPercentage : ""}
                formatter={value => `${value}%`}
                parser={value => value.replace('%', '')}
                onChange={(e) => {
                  props.onChangeDiscountPercentage(e)
                }}
              /></>}
          </div>
        </div>
      </div>
    </>}
  </div>
  )
}
