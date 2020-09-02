import * as React from 'react';
import { Popover, Badge } from 'antd';
import { MasterEventTypes } from '../../../../Enums/MasterEventTypes';

let style = {
  cursor: 'pointer'
};

function EventColumn(props: any) {
  let rowData = props.row;
  if (!rowData) {
    return <td></td>
  }
  return (
    <td>
      <div className="media">
        <div className="fil-list-dates">
          <span className="text-primary">{rowData.eventStartDateTimeString.substring(0, 3)} {rowData.eventStartDateTimeString.substring(4, 6)}</span> <br />
          {rowData.eventStartDateTimeString.substring(8, 12)}
        </div>
        <img
          className="align-self-start mt-1 rounded"
          src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/places/tiles/${rowData.altId.toUpperCase()}-ht-c1.jpg`}
          alt="Generic placeholder image"
          width="64"
          onError={(e) => {
            e.currentTarget.src = 'https://static5.feelitlive.com/images/places/tiles/event-placeholder.jpg';
          }}
        />
        <div className="media-body">
          <div className="m-0 font-weight-bold" style={style} onClick={() => {
            {
              if (MasterEventTypes.Online == rowData.masterEventType) {
                window.location.replace(`/host-online/${rowData.id}/basics`)
              } else {
                window.location.replace(`/edit/${rowData.altId}`)
              }
            }
          }} >{rowData.name} {rowData.isTokenize && <Popover content={`This is private experience`} trigger="hover">
            <i className="fa fa-lock mr-2 ml-2" aria-hidden="true"></i>
          </Popover>}
            {(rowData.isShowExclamationIcon) && <Popover content={`Missing information! Please complete ${rowData.currentStep} tab`} trigger="hover">
              <i className="fa fa-exclamation-triangle mr-2 ml-2 text-warning" aria-hidden="true"></i>
            </Popover>}
          </div>
          <div className="fil-tils-tag"><span>{rowData.subCategory}</span></div>
        </div>
      </div>
    </td>
  );
}

export default EventColumn;
