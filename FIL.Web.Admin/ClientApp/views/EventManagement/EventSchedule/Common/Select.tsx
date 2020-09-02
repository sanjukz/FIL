/*Third Party Imports */
import * as React from 'react'
import { Select as AntdSelect } from 'antd';

export const Select: React.FC<any> = (props: any) => {
  return (<AntdSelect
    showSearch
    style={{ width: "100%" }}
    value={props.value ? props.value : props.placeHolder}
    onChange={(e: any) => {
      props.onChange(e);
    }}>
    {props.options}
  </AntdSelect>
  )
}

