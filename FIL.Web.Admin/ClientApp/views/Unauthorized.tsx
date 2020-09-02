import * as React from 'react'
import { Result, Button } from 'antd';

export const Unauthorized: React.FC<any> = (props: any) => {
  return (<Result
    status="403"
    title="403"
    subTitle="Sorry, you are not authorized to access this page."
    extra={<Button type="primary" onClick={() => {
      window.location.replace('/');
    }} >Back Home</Button>}
  />)
}

