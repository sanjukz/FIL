import { autobind } from 'core-decorators';
import * as React from 'react';
import { connect } from 'react-redux';
import { IApplicationState } from '../stores';
import { TransactionLocatorFormData } from '../models/TransactionLocator/TransactionLocatorFormData';
import { SubmitFulFilmentFormDetails } from '../models/TransactionLocator/SubmitFulFilmentFormDetails';
import { SubmitFulfilmentResponseModel } from '../models/TransactionLocator/SubmitFulfilmentResponseModel';
import FILLoader from '../components/Loader/FILLoader';
import * as FulfillmentStore from '../../ClientApp/stores/Fulfillment';
import TransactionLocatorComponent from '../components/TransactionLocatorComponent';
import FulFilmentTableComponent from '../components/FulFilmentTableComponent';
var flag = false;
type FulfillmentComponentProps = FulfillmentStore.IFulfillmentComponentState & typeof FulfillmentStore.actionCreators;

class FulfilmentForm extends React.Component<FulfillmentComponentProps, any> {
  constructor(props) {
    super(props);
    this.state = {
      transactionDetailId: '',
      tranDetailAltId: ''
    };
  }
  @autobind
  public generateOTP(phoneNumber, phoneCode, tranDetailAltId) {
    flag = true;
    this.setState({
      tranDetailAltId: tranDetailAltId
    });
    this.props.generatetheOTP(phoneCode, phoneNumber, tranDetailAltId);
  }
  public render() {
    if (this.props.isLoading) {
      return <FILLoader />;
    }
    if (flag && this.props.fetchOTPStatus) {
      alert("Please enter the One Time Password (OTP) we've just sent to the number you provided.");
      flag = false;
    }
    return (
      <div className="card border-0 right-cntent-area pb-5 bg-light">
        <div className="card-body bg-light p-0">
          <div className="container pt-5 page-fluid">
            <TransactionLocatorComponent onSubmit={this.onSubmitProcced} />
            {this.props.fetchFloatTransactionDataSuccess && (
              <FulFilmentTableComponent
                data={this.props.transactionData}
                generateOTP={this.generateOTP}
                submitData={this.submitData}
              />
            )}
          </div>
        </div>
      </div>
    );
  }

  @autobind
  private onSubmitProcced(values: TransactionLocatorFormData) {
    if (
      values.barcodeNumber == undefined &&
      values.firstName == undefined &&
      values.lastName == undefined &&
      values.emailid == undefined &&
      values.confirmationNumber == undefined &&
      values.userMobileNo == undefined
    ) {
      alert('Please fill in at least one mode of fulfillment.');
      return false;
    }

    var details: TransactionLocatorFormData = {
      barcodeNumber: values.barcodeNumber == '' || values.barcodeNumber == undefined ? '0' : values.barcodeNumber,
      firstName: values.firstName == '' || values.firstName == undefined ? '0' : values.firstName,
      lastName: values.lastName == '' || values.lastName == undefined ? '0' : values.lastName,
      emailid: values.emailid == '' || values.emailid == undefined ? '0' : values.emailid,
      confirmationNumber:
        values.confirmationNumber == undefined || values.confirmationNumber == null ? 0 : values.confirmationNumber,
      userMobileNo: values.userMobileNo == '' || values.userMobileNo == undefined ? '0' : values.userMobileNo
    };
    this.props.getFulFilmentDetails(details);
  }
  @autobind
  public submitData(pickupOtp, ticketnumber) {
    var details: SubmitFulFilmentFormDetails = {
      pickupOTP: pickupOtp,
      tranDetailAltId: this.state.tranDetailAltId,
      ticketNumber: ticketnumber
    };
    this.props.SubmitFulFilmentDetails(details, (response: SubmitFulfilmentResponseModel) => {
      if (response.isValid) {
        alert('Submitted Successfully');
      } else {
        alert(response.message);
      }
    });
  }
}

export default connect(
  (state: IApplicationState) => state.Fulfillment,
  FulfillmentStore.actionCreators
)(FulfilmentForm);
