import * as React from 'react';
import { View } from '../../utils';

const ActionColumn: React.FC<any> = (props: any) => {
  let rowData = props.row.rowData;
  if (!rowData) {
    return <td></td>
  }
  return (< td style={{ width: '50' }} className="text-right" >
    <div className="btn-group">
      <button
        type="button"
        className="btn btn-sm btn-outline-primary dropdown-toggle"
        data-toggle="dropdown"
        aria-haspopup="true"
        aria-expanded="false"
        aria-hidden="true"
      >Action</button>
      <div className="dropdown-menu dropdown-menu-right p-0">
        {rowData.isEnabled && (
          <a
            href={`${
              window.location.origin.indexOf('dev') > -1
                ? 'https://dev.feelitlive.com'
                : 'https://www.feelitlive.com'
              }${rowData.isTokenize ? `/event/${rowData.slug}/${rowData.altId}` : rowData.eventUrl}`}
            target="_blank"
            className="dropdown-item"
          >
            View
          </a>
        )}
        <a href={`/host-online/${rowData.id}/basics`} className="dropdown-item">
          Edit
        </a>
        {!rowData.isEnabled && props.row.view == View.ApproveModerate && <a href='javascript:Void(0)' onClick={(e) => {
          props.row.onChangeEventStatus(rowData, true)
        }} className="dropdown-item"> Approve & Publish</a>}
        {rowData.isEnabled && props.row.view == View.ApproveModerate && <a href='javascript:Void(0)' onClick={(e) => {
          props.row.onChangeEventStatus(rowData, false)
        }} className="dropdown-item">Deactive</a>}
      </div>
    </div>
  </td >)
}
export default ActionColumn;
