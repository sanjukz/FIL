/*Third Party Imports */
import * as React from 'react'
import { Checkbox } from 'antd';

/*Local Imports */
import { ToolTip } from "../ToolTip";

import "../../EventTicket.scss"

export class Items extends React.Component<any, any>{
  constructor(props) {
    super(props)
    this.state = {
      item: this.props.eventDetail.itemsForViewer,
      isChecked: this.props.isChecked
    }
  }

  render() {
    return (
      <div className="mt-4">
        <label >Items viewers will need to bring to participte in this experience
          <ToolTip description="Items viewers will need to bring to participte in this experience." />
        </label>
        <div className="mt-2 participant">
          <Checkbox checked={this.state.isChecked} onChange={(e) => {
          this.setState({ isChecked: e.target.checked }, () => {
            this.props.onChangeCheckbox(e);
          });
          }}>
            Participants don't need to bring anything to take part in this experience
          </Checkbox>
        </div>
        <div className="mt-3">Please add one item at a time, or group similar items together.
</div>
        {!this.state.isChecked && this.props.eventDetail.itemsForViewer.map((val, index) => {
          return <div key={index} className="col-sm-6 p-0 my-3">
            <div className="rounded position-relative">
              <input
                value={val}
                className="form-control"
                onChange={(e) => {
                  let currentItem = this.state.item;
                  currentItem[index] = e.target.value;
                  this.setState({ item: currentItem }, () => {
                    this.props.onChangeItems(this.state.item)
                  })
                }}
              />
              <div
                className="clear-input"
                onClick={() => {
                  let newItem = this.state.item;
                  newItem.splice(index, 1);
                  this.setState({ item: newItem }, () => {
                    this.props.onChangeItems(this.state.item)
                  })
                }}
              >
                <a href="javascript:void(0)">
                  <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/cross-icon.svg"
                    width="24" />
                </a>
              </div>
            </div>
          </div>
        })}
        {!this.state.isChecked && < div className="mt-3" >
          <a className="btn btn-outline-primary" onClick={() => {
            let newItem = this.state.item;
            newItem.push('');
            this.setState({ item: newItem })
          }} >+ Add Item</a></div>}
      </div>
    )
  }
}


