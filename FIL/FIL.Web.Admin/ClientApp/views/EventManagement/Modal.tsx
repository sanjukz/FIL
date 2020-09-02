/*Third Party Imports */
import * as React from 'react'
import { Modal as AntdModal, Button } from 'antd';

export const Success = (message) => {
  AntdModal.success({
    title: message,
    centered: true,
    onOk: () => {
    }
  });
}

export const Error = (message) => {
  AntdModal.error({
    title: message,
    centered: true,
    onOk: () => {
    }
  });
}

export const Modal: React.FC<any> = (props: any) => {
  return (<></>)
}

