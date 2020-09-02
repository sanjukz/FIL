/*Third Party Imports */
import * as React from 'react'
import { TimePicker } from 'antd';
import * as moment from 'moment';

export const Time: React.FC<any> = (props: any) => {
  return (<TimePicker
    className="w-100"
    {...(props.time != "" ? { value: moment(props.time, 'h:mm a') } : {})}
    minuteStep={15}
    use12Hours
    format="h:mm a"
    allowClear={false}
    onChange={(value, val) => {
      props.onChange(val);
    }} />
  )
}

