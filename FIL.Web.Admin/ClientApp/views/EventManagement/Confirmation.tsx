/*Third Party Imports */
import * as React from 'react'
import { Modal } from 'antd';

/*Third party constants */
const { confirm } = Modal;

export const showDeleteConfirm = (props) => {
  confirm({
    title: props.message,
    okText: 'Yes',
    okType: 'danger',
    cancelText: 'No',
    onOk() {
      return new Promise((resolve, reject) => {
        setTimeout(Math.random() > 0.5 ? resolve : reject, 1000);
        if (resolve) {
          props.onDelete(props.id);
        }
      }).catch(() => console.log('Oops errors!'));
    },
    onCancel() {
      console.log('Cancel');
    },
  });
}

export const showConfirmation = (props) => {
  confirm({
    title: props.message,
    okText: 'Yes',
    okType: 'primary',
    cancelText: 'No',
    type: 'confirm',
    centered: true,
    onOk() {
      return new Promise((resolve, reject) => {
        setTimeout(Math.random() > 0.5 ? resolve : reject, 1000);
        if (resolve) {
          props.onConfirmed();
        }
      }).catch(() => console.log('Oops errors!'));
    },
    onCancel() {
      console.log('Cancel');
    },
  });
}

export const error = (message, onOk?: () => void) => {
  Modal.error({
    title: message,
    centered: true,
    onOk() {
      if (onOk) {
        onOk();
      }
    }
  });
}

export const Confirmation: React.FC<any> = (props: any) => {
  return (<div
    className="pull-right"
  >
    <a href="javascript:void(0)" className="mr-2" onClick={() => { props.onEdit() }}>
      <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-edit.svg" />
    </a>
    <a href="javascript:void(0)" onClick={() => {
      showDeleteConfirm(props)
    }}>
      <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/fil-trash.svg" />
    </a>
  </div >)
}

