/* Third party imports */
import * as React from 'react'

/* Local imports */
import { Confirmation } from "../Confirmation";

export const SponsorListing: React.FC<any> = (props: any) => {
  return (props.eventSponsorViewModel.sponsorDetails.map((item, index) => {
    return (
      <div key={index} className="col-sm-6 p-0 mb-2">
        <div className="rounded p-2 bg-white border">
          <a
            href="javascript:void(0)"
            className="text-muted"
            onClick={() => {
              props.onClickSponsor(item);
            }}
          >
            {item.name}
          </a>
          <Confirmation
            host={item}
            onEdit={() => {
              props.onClickSponsor(item);
            }}
            message={'Are you sure, you want to delete the sponsor?'}
            onDelete={() => {
              props.onDeleteSponsor(item)
            }} />
        </div>
      </div>
    )
  })
  )
}

