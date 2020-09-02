/*Third Party Imports */
import * as React from 'react'
import { Tooltip } from 'antd';

export const ToolTip: React.FC<any> = (props: any) => {
  return (<Tooltip title={<p><span>{props.description}</span></p>}>
    <span> <img className="ml-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-info.svg" /></span>
  </Tooltip>)
}

