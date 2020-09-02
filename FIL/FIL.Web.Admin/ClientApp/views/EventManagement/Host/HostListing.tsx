/* Third party imports */
import * as React from 'react'

/* Local imports */
import { Confirmation } from "../Confirmation";

export const HostListing: React.FC<any> = (props: any) => {
  return (props.eventHostViewModel.eventHostMapping.map((item, index) => {
    return (
      <div key={index} className="col-sm-6 p-0 mb-2">
        <div className="rounded p-2 bg-white border">
          <a
            href="javascript:void(0)"
            className="text-muted"
            onClick={() => {
              props.onClickHost(item);
            }}
          >
            <span className="text-blueberry px-2">{index + 1}</span> {item.firstName} {item.lastName}
          </a>
          <Confirmation
            host={item}
            onEdit={() => { props.onClickHost(item) }}
            message={'Are you sure, you want to delete the host?'}
            onDelete={() => {
              props.onDeleteHost(item)
            }} />
        </div>
      </div>
    )
  })
  )
}

