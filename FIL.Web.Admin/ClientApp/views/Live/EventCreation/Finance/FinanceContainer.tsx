import * as React from 'react'
import * as _ from 'lodash'
import FinanceForm from './FinanceForm'
import { Modal as antModal, Button, notification, Tooltip, Drawer } from 'antd'

export default class FinanceContainer extends React.Component<any, any> {
  state = {
    isModalShow: false,
    selectedTicket: null,
    isShow: false,
    isSuccess: false,
    confirmMessage: '',
    isShowConfirmPopup: false,
    visible: false,
    allTickets: []
  }

  render() {
    let that = this
    return (
      <div className="p-3">
        <FinanceForm
          stateOptions={this.props.stateOptions}
          onSubmit={(item) => {
            this.props.onSubmit(item)
          }}
        />
      </div>
    )
  }
}
