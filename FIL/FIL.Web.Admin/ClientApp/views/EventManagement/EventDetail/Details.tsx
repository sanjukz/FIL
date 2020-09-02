/*Third Party Imports */
import * as React from 'react'
import CKEditor from "react-ckeditor-component";

/*Local Imports */
import { ToolTip } from "../ToolTip";
import { Items } from './Items';

export const Details: React.FC<any> = (props: any) => {
  let detail = props.eventDetail;
  return (<div className="col-12 mt-2">
    <label >Description
                    <ToolTip description="Write up a short description with the key highlights of your event. Please note that FeelitLIVE may make grammatical corrections. As this is what audiences will read to determine whether to buy a ticket to the event/experience, please make it as pithy and attractive as possible. Please ensure adherence to the FeelitLIVE Additional Terms and Conditions. (Min 200 & Max 1400 characters)" />
    </label>
    <CKEditor
      activeClass="p10"
      content={props.eventDetail.description}
      required
      events={{
        "change": (e) => {
          props.onChange(e);
        },
        "blur": (e) => {
          props.onFocusOut(e)
        }
      }}
    />
    {!props.error && <p className="mt-2">{props.charLength} / 1400</p>}
    {props.error && <p className="mt-2 text-danger">{props.error}</p>}
    {(props.eventDetail.name) && <Items
      eventDetail={props.eventDetail}
      onChangeCheckbox={(e) => {
        props.onChangeCheckbox(e)
      }}
      isChecked={props.isChecked}
      onChangeItems={(itemArray) => {
        props.onChangeItems(itemArray)
      }}
    />}
  </div>)
}

