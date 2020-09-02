/*Third Party Imports */
import * as React from 'react'
import { Button } from "antd";

/*Local Imports */
import Spinner from "../../../components/Spinner";

//XXX:TODO: Figma TODO
export const getPublishedScreen = (props) => {
  return (<></>)
}

export const getWaitingForApprovalScreen = () => {
  return (<>
    <div className="bg-white shadow-sm mb-2 rounded-box" id="nav-tabContent" key="12">
      <div><img className="w-100" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/strip-top-shade.svg" /></div>

      <div className="px-5 pb-5 text-center">
        <h3 className="mb-4 text-purple">Tada! Your experience has been submitted successfully</h3>
        <h6>
          FeelitLIVE team will review your experience shortly and get back to you via email.</h6>
        <h6 className="mb-4">
          Thank you for choosing FeelitLIVE to create and host your experience.</h6>
        <div><img className="img-fluid" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/strip-exit.svg" /></div>
        <a href="/" className="btn btn-outline-primary" >Take me to my dashboard<i className="fa fa-angle-right ml-1" aria-hidden="true"></i></a>
      </div>
    </div>

  </>)
}

export const getSubmitForApprovalScreen = (props) => {
  return (<>
    <div className="bg-white shadow-sm mb-2 rounded-box" id="nav-tabContent" key="12">
      <div className="px-5 pt-5">
        <h3 className="mb-4 text-purple">You are done!</h3>
        <h6 className="mb-4">
          Your experience is now ready to be submitted for approval</h6>
        <h6 className="mb-4">
          You can review any of the section of your experience by clicking on any of the sections on the menu, or you can submit experience for approval, by clicking on the "Submit" button below.
         </h6>
        <Button
          size={"large"}
          type={"primary"}
          loading={props.props.ApproveModerate.isLoading}
          onClick={() => { props.onSubmit() }}
        >
          Submit <i className="fa fa-angle-right ml-1" aria-hidden="true"></i>
        </Button>
      </div>
      <div className="box-botttom-img"><img className="w-100" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/strip-you-are-done.svg" /></div>
    </div>


  </>)
}

export const getDisplayScreen = (props) => {
  /* Draft */
  if (props.eventStatus == 3) {
    return getSubmitForApprovalScreen(props);
  }
  /* Waiting for Approval */
  else if (props.eventStatus == 4) {
    return getWaitingForApprovalScreen();
  }
  /* Published */
  else if (props.eventStatus == 6) {
    return getPublishedScreen(props);
  }
}

export const Index: React.FC<any> = (props: any) => {
  return (<>
    {(!props.props.ApproveModerate.isLoading) && <>{getDisplayScreen(props)}</>}
    {(props.props.ApproveModerate.isLoading) && <Spinner />}
  </>)
}

