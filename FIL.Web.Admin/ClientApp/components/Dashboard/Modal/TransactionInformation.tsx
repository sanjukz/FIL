import * as React from 'react';
import axios from "axios";
import { Modal, Spin } from 'antd';
import { showConfirmation } from '../../../views/EventManagement/Confirmation';
import { showNotification } from '../../../views/EventManagement/Notification';

export const TransactionInformation = (props: any) => {
  const [isRequestOrderConfirmation, setIsRequestOrderConfirmation] = React.useState(false);
  return <Modal
    title="Transaction Details"
    visible={props.visible}
    onCancel={() => {
      props.onCloseModal()
    }}
    maskClosable
    footer={null}
    centered
  >
    <Spin tip="Sending email..." spinning={isRequestOrderConfirmation}>
      <p>Confirmation Id: {props.rowData.transactionId}</p>
      <p>Status: <span style={{ color: props.rowData.transactionStatus == 'Success' ? 'green' : props.rowData.transactionStatus == 'UnderPayment' ? 'crimson' : 'black' }}>{props.rowData.transactionStatus}</span></p>
      <p>Event: {props.rowData.eventName}</p>
      <p>Buyer Name: {props.rowData.firstName} {props.rowData.lastName}</p>
      <p>Phone: <a style={{ color: "#6c63ff" }} href={`tel:${(!props.rowData.phoneCode && !props.rowData.phoneNumber) ? "None" : props.rowData.phoneCode ? '+' + props.rowData.phoneCode + props.rowData.phoneNumber : props.rowData.phoneNumber}`}>{(!props.rowData.phoneCode && !props.rowData.phoneNumber) ? "None" : props.rowData.phoneCode ? '+' + props.rowData.phoneCode + props.rowData.phoneNumber : props.rowData.phoneNumber}</a></p>
      <p>Email: <a style={{ color: "#6c63ff" }} href={`mailto:${props.rowData.email}`}>{props.rowData.email}</a></p>
      {(props.rowData.isEventEnabled && props.rowData.isEventDetailEnabled && props.rowData.transactionStatus == "Success") && <p>Order Confirmation: <a
        style={{ color: "#6c63ff" }}
        onClick={() => {
          showConfirmation({
            message: 'Do you want to send Order confirmation email?',
            onConfirmed: () => {
              let orderConfirmationModel = {
                transactionId: props.rowData.transactionAltId,
                confirmationFromMyOrders: false
              };
              setIsRequestOrderConfirmation(true);
              sendOrderConfiramtion(orderConfirmationModel, (response: boolean) => {
                if (response) {
                  showNotification('success', 'Confirmation email has been sent')
                  setIsRequestOrderConfirmation(false)
                } else {
                  showNotification('error', 'There is some issue while sending the email')
                  setIsRequestOrderConfirmation(false)
                }
              })
            }
          })
        }}
        href='javaScript:Void(0)'>Send Order Confirmation Email</a></p>}
    </Spin>
  </Modal >
}

const sendOrderConfiramtion = (order, callback: (boolean) => void) => {
  return axios.post(`${window.location.href.indexOf('dev') > -1 ? `https://dev.feelitlive.com/api/orderconfirmations` : `https://www.feelitlive.com/api/orderconfirmations`}`, order, {
    headers: {
      "Content-Type": "application/json"
    }
  })
    .then((response) => response.data as Promise<any>).then((reponse) => {
      callback(true)
    })
    .catch((error) => {
      callback(false)
    });
}
