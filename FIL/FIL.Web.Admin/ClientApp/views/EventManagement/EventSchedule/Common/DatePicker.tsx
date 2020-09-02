/*Third Party Imports */
import * as React from 'react'
import * as moment from 'moment';
import { DatePicker as AntdDatePicker } from 'antd';

export const DatePicker: React.FC<any> = (props: any) => {
  return (<AntdDatePicker
    className="w-100"
    {...(props.dateTime != "" ? {
      value: moment(moment(props.dateTime).toDate(), 'MM/DD/YYYY')
    } : {})}
    onChange={props.onChange}
    disabledDate={!props.startDate ? (d => !d || d.isBefore(new Date().setDate(new Date().getDate() - 1))) : (d => !d || d.isBefore(new Date(props.startDate)))}
    format="MM/DD/YYYY"
    placeholder="MM/DD/YYYY"
    allowClear={false}
  />
  )
}

