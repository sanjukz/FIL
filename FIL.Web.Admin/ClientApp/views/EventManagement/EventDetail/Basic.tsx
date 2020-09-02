/*Third Party Imports */
import * as React from 'react'

/*Local Imports */
import { ToolTip } from "../ToolTip";
import { Select } from 'antd';
import { Radio, Input } from 'antd';
import "../../EventTicket.scss";

const radioStyle = {
  display: 'block',
};
export const Basic: React.FC<any> = (props: any) => {
  const { eventTypeValue } = props;
  return (<>
    <div className="col-sm-6">
      <label className="d-block">Title
                            <ToolTip description="Enter a nice catchy and short event title (Max 50 characters)" />
      </label>
      <input
        name="title"
        placeholder="Enter experience title "
        className="form-control"
        value={props.eventDetail.name}
        onChange={(e) => { props.onChangeTitle(e) }}
        maxLength={50}
        type="title" required />
    </div>
    <div className="col-sm-6">
      <label className="d-block">Experience/Event Category
                    <ToolTip description="Please select the event category under which your experience/event falls. If you don’t see a category listed please send an email to support@feelitLIVE.com" />
      </label>
      <Select
        showSearch
        size={"large"}
        style={{ width: "100%" }}
        value={props.eventDetail.parentCategory ? props.eventDetail.parentCategory : "Select Experience/Event Category"}
        onChange={(e: any) => {
          props.onChangeParentCategory(e)
        }}>
        {props.parentCategoriesOption}
      </Select>
    </div>
    <div className="col-sm-6 mt-3">
      <label className="d-block">Experience/Event Sub Category
                    <ToolTip description="Please select the event subcategory under which your experience/event falls. If you don’t see a category listed please send an email to support@feelitLIVE.com" />
      </label>
      <Select
        showSearch
        size={"large"}
        style={{ width: "100%" }}
        value={props.eventDetail.defaultCategory ? props.eventDetail.defaultCategory : "Select Experience/Event Sub Category"}
        onChange={(e: any) => {
          props.onChangeCategory(e)
        }}>
        {props.eventCategories}
      </Select>
    </div>
    <div className="col-sm-12 mt-4 participant">
      <h5>Privacy Setting</h5>
      <Radio.Group onChange={(e) => props.onChangeEventTypeValue(e.target.value)} value={eventTypeValue}>
        <Radio style={radioStyle} value={1}>
          Public - Discoverable on FeelitLIVE and search engines.
        </Radio>
        <Radio style={radioStyle} className="mt-1" value={2}>
          Private - Anyone with the link can access your page, but it will NOT BE discoverable on FeelitLIVE or search engines.
        </Radio>
      </Radio.Group>
    </div>
  </>)
}