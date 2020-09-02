import * as React from 'react';
import { Modal } from 'antd';
import { Eventstatuses } from '../../../Enums/Eventstatuses';
import { MasterEventTypes } from '../../../Enums/MasterEventTypes';
import { EventFrequencyType } from '../../../Enums/EventFrequencyType';

export const EventInformation = (props: any) => {
  return <Modal
    title={props.rowData.eventName}
    visible={props.visible}
    onCancel={() => {
      props.onCloseModal()
    }}
    maskClosable
    footer={null}
    centered
  >
    <p>Event: {props.rowData.eventName}</p>
    <p>Event Date: {props.rowData.localStartDateString}{(props.rowData.eventFrequencyType == EventFrequencyType.Recurring || props.rowData.eventFrequencyType == EventFrequencyType.OnDemand) ? ` - ${props.rowData.localEndDateString}` : ""} {props.rowData.timeZoneAbbreviation}</p>
    <p>Type: {MasterEventTypes[props.rowData.masterEventTypeId == 1 ? 3 : props.rowData.masterEventTypeId]}</p>
    <p>Frequency: {EventFrequencyType[props.rowData.eventFrequencyType]}</p>
    <p>Event Status: {Eventstatuses[props.rowData.eventStatusId]}</p>
    <p>Live on site: {props.rowData.isEventEnabled && props.rowData.isEventDetailEnabled ? "Yes" : "No"}</p>
  </Modal>
}