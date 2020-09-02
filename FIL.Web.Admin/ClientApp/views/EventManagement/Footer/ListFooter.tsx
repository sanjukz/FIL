import * as React from 'react'
import { Button } from "antd";

export const ListFooter: React.FC<any> = (props: any) => {
  return (
    <div className="text-center pt-4 pb-4">
      <a className="btn btn-outline-primary mr-0 mr-sm-4 mb-2 mb-sm-0" style={{ minWidth: "150px" }} onClick={() => { props.onClickAddNew() }} >{props.addText}</a>
      {(!props.isShowContinue) &&
        <Button
          size={"large"}
          type={props.isDisabled ? "default" : "primary"}
          disabled={props.isDisabled}
          loading={props.isSaveRequest}
          onClick={() => { props.onClickContinue() }}
        >
          {props.saveText ? props.saveText : 'Save & Continue'}
          <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/arrow-right.svg" className="ml-2" alt="" width="6"></img>
        </Button>
      }
    </div>)
}

