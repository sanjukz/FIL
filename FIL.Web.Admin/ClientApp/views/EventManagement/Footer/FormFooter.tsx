import * as React from 'react'
import { Button } from "antd";

export const Footer: React.FC<any> = (props: any) => {
  return (
    <div className="text-center pt-4 pb-4">
      {(!props.isHideCancel) && <span className="d-block d-sm-inline mb-2 mb-sm-0"><a
        href="javascript:void(0)"
        style={{ minWidth: "150px", borderRadius: "0.25rem" }}
        onClick={() => { props.onClickCancel() }}
        className="btn btn-outline-primary mr-0 mr-sm-4"
      >
        {props.cancelText ? props.cancelText : 'Back'}
      </a></span>}
      <Button
        size={"large"}
        style={{ minWidth: "150px", borderRadius: "0.25rem" }}
        type={props.isDisabled ? "default" : "primary"}
        disabled={props.isDisabled}
        loading={props.isSaveRequest}
        onClick={() => { props.onSubmit() }}
      >
        {props.saveText ? props.saveText : 'Save & Continue'}
        <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/arrow-right.svg" className="ml-2" alt="" width="6"></img>
      </Button>
      {(props.isShowSkip) && <a
        href="javascript:void(0)"
        onClick={() => { props.onClickSkip() }}
        className="ml-4 btn btn-link"
      >
        Skip Step
      </a>}
    </div>)
}

