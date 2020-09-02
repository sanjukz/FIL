/* Third party imports */
import * as React from 'react'
import { Radio, Select } from 'antd'
import { fetch } from 'domain-task'
import { Link } from 'react-router-dom'

/* Local imports */
import { EventFinanceDetailModel } from '../../../models/CreateEventV1/EventFinanceViewModal'
import { setFinanceObject } from "../utils/DefaultObjectSetter";
import { bankNames } from '../../../utils/IndianBankNames'
import { Footer } from "../Footer/FormFooter";

export interface BranchDetail {
  ifscCode: string
  bankName: string
  micr: string
  branch: string
  address: string
  city: string
  district: string
  state: string
  pinCode: string
}

export default class FinanceForm extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
      screen: 1,
      eventFinanceViewModel: setFinanceObject()
    }
  }
  form: any

  componentDidMount() {
    if (this.props.eventFinanceViewModel && this.props.eventFinanceViewModel.eventFinanceDetailModel) {
      this.setState({
        eventFinanceViewModel: { ...this.props.eventFinanceViewModel.eventFinanceDetailModel },
        confirmAccountNumber: this.props.eventFinanceViewModel.eventFinanceDetailModel.accountNumber
      }, () => {
        let selectedState = this.props.stateOptions.states.filter((val) => { return val.id == this.state.eventFinanceViewModel.stateId });
        if (selectedState.length > 0) {
          this.setState({ selectedState: selectedState[0].name })
        }
      });
    }
  }

  getBankModal = () => {
    const { Option } = Select
    let bankNameArray = []
    let bankName = bankNames
    for (var bank in bankName) {
      bankNameArray.push(
        <Option value={`${bank}+${bankName[bank]}`} key={`${bank}+${bankName[bank]}`}>
          {bankName[bank]}
        </Option>
      )
    }
    return bankNameArray
  }

  getStateOption = () => {
    const { Option } = Select
    let stateArray = []
    if (this.props.stateOptions && this.props.stateOptions.states) {
      this.props.stateOptions.states.map((item) => {
        stateArray.push(
          <Option value={`${item.name}+${item.altId}+${item.id}`} key={`${item.name}+${item.altId}+${item.id}`}>
            {item.name}
          </Option>
        )
      })
    }
    return stateArray
  }

  isIfscCodeValid = (ifscCode: string) => {
    let isvalid = /^[A-Za-z]{4}\d{7}$/.test(ifscCode)
    return isvalid
  }

  isBankAccountValid = (bankAccount: string) => {
    if (/^\d{9,18}$/.test(bankAccount)) {
      this.setState({ bankAccountNumberError: '' })
    } else {
      this.setState({ bankAccountNumberError: 'Please enter valid bank account number' })
    }
  }

  getBranchInfoByIFSC = (ifscCode) => {
    if (this.isIfscCodeValid(ifscCode)) {
      fetch(`https://web.investmentz.com/api/Bank/GetIFSCDetail?IFSCCode=${ifscCode}`)
        .then((response) => response.json() as Promise<BranchDetail>)
        .then(
          (data: BranchDetail) => {
            this.setState({ branchName: `${data.branch}, ${data.city}`, branchError: '' })
          },
          (error) => {
            this.setState({ branchName: '', branchError: '' })
          }
        )
    } else {
      this.setState({ branchName: '', branchError: 'Please enter valid IFSC code' })
    }
  }

  onPasteConfirmAccountNumber = (e) => {
    e.preventDefault()
  }

  isButtonDisable = () => {
    var pattern = /^[a-zA-Z0-9\-_]+(\.[a-zA-Z0-9\-_]+)*@[a-z0-9]+(\-[a-z0-9]+)*(\.[a-z0-9]+(\-[a-z0-9]+)*)*\.[a-z]{2,4}$/;
    return !!(this.state.screen == 1 &&
      (this.state.eventFinanceViewModel.accountTypeId == 1 ? true : this.state.eventFinanceViewModel.companyName ? true : false) &&
      this.state.eventFinanceViewModel.firstName &&
      this.state.eventFinanceViewModel.lastName &&
      pattern.test(this.state.eventFinanceViewModel.email) &&
      this.state.eventFinanceViewModel.phoneNumber) ||
      !!(this.state.screen == 2 &&
        (this.state.eventFinanceViewModel.accountTypeId == 2 ? (this.state.eventFinanceViewModel.stateId ? true : false) : true) &&
        this.state.eventFinanceViewModel.accountName &&
        this.state.eventFinanceViewModel.bankName &&
        this.state.eventFinanceViewModel.branchCode &&
        this.state.eventFinanceViewModel.accountNumber &&
        this.state.confirmAccountNumber &&
        !this.state.branchError &&
        !this.state.bankAccountNumberError &&
        this.state.eventFinanceViewModel.accountNumber == this.state.confirmAccountNumber)
  }

  render() {
    const { Option } = Select
    const radioStyle = {
      display: 'block',
      height: '30px',
      lineHeight: '30px'
    }
    const { value } = this.state
    return (
      <form
        className="text-left p-4 m-auto bg-light rounded mt-4 shadow-sm border"
        style={{ maxWidth: '500px' }}
        onSubmit={(values: any) => {
          values.preventDefault()
          values.stopPropagation()
          if (this.state.screen == 2) {
            let eventFinanceViewModel: EventFinanceDetailModel = {
              id: this.state.eventFinanceViewModel.id,
              accountTypeId: this.state.eventFinanceViewModel.accountTypeId,
              firstName: this.state.eventFinanceViewModel.firstName,
              lastName: this.state.eventFinanceViewModel.lastName,
              email: this.state.eventFinanceViewModel.email,
              companyName: this.state.eventFinanceViewModel.companyName,
              phoneCode: 91,
              phoneNumber: this.state.eventFinanceViewModel.phoneNumber,
              accountName: this.state.eventFinanceViewModel.accountName,
              bankName: this.state.eventFinanceViewModel.bankName,
              branchCode: this.state.eventFinanceViewModel.branchCode,
              accountNumber: this.state.eventFinanceViewModel.accountNumber,
              stateId: this.state.eventFinanceViewModel.stateId,
              taxId: this.state.eventFinanceViewModel.taxId,
              countryId: 101,
              currenyId: 7
            }
            this.props.onSubmit(eventFinanceViewModel)
          } else {
            this.setState({ screen: 2 })
          }
        }}
        ref={(ref) => {
          this.form = ref
        }}
      >
        <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="3">
          <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
            <h3 className="m-0 text-purple">Connecting your account to Stripe</h3></nav>
          {this.state.screen == 1 && (
            <div className="form-row">
              <div className="form-group col-12">
                <div>
                  Please fill in your bank details. This is to ensure that the net ticket sales amount can be transferred
                  to your account. Once you have filled the details successfully, your experience/event will be submitted
                  for approval.
                <hr />
                </div>
                <Radio.Group onChange={(e: any) => {
                  let eventFinanceViewModel = this.state.eventFinanceViewModel;
                  eventFinanceViewModel.accountTypeId = e.target.value;
                  this.setState({
                    eventFinanceViewModel: eventFinanceViewModel
                  })
                }} value={this.state.eventFinanceViewModel.accountTypeId}>
                  <Radio style={radioStyle} value={1}>
                    <b>Individual</b>
                  </Radio>
                  <Radio style={radioStyle} value={2}>
                    <b>Company</b>
                  </Radio>
                </Radio.Group>
              </div>
              <div className="form-group col-12">
                <label>First Name</label>
                <input
                  name="firstName"
                  placeholder="Enter First Name"
                  className="form-control"
                  value={this.state.eventFinanceViewModel.firstName}
                  onChange={(e) => {
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.firstName = e.target.value;
                    this.setState({
                      eventFinanceViewModel: eventFinanceViewModel
                    })
                  }}
                  type="text"
                  required
                />
              </div>
              <div className="form-group col-12">
                <label>Last Name</label>
                <input
                  name="lastName"
                  placeholder="Enter Last Name"
                  value={this.state.eventFinanceViewModel.lastName}
                  onChange={(e) => {
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.lastName = e.target.value;
                    this.setState({
                      eventFinanceViewModel: eventFinanceViewModel
                    })
                  }}
                  className="form-control"
                  type="text"
                  required
                />
              </div>
              <div className="form-group col-12">
                <label>Email</label>
                <input
                  name="email"
                  placeholder="Enter Email"
                  value={this.state.eventFinanceViewModel.email}
                  onChange={(e) => {
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.email = e.target.value;
                    this.setState({
                      eventFinanceViewModel: eventFinanceViewModel
                    })
                  }}
                  className="form-control"
                  type="email"
                  required
                />
              </div>
              {this.state.eventFinanceViewModel.accountTypeId == 2 && (
                <div className="form-group col-12">
                  <label>Company Name</label>
                  <input
                    name="company"
                    placeholder="Enter Company Name"
                    value={this.state.eventFinanceViewModel.companyName}
                    onChange={(e) => {
                      let eventFinanceViewModel = this.state.eventFinanceViewModel;
                      eventFinanceViewModel.companyName = e.target.value;
                      this.setState({
                        eventFinanceViewModel: eventFinanceViewModel
                      })
                    }}
                    className="form-control"
                    type="company"
                    required
                  />
                </div>
              )}
              <div className="form-group col-3">
                <label>Country Code</label>
                <Select
                  showSearch
                  size={'large'}
                  placeholder="Country Code"
                  defaultValue={'+91-india'}
                  onChange={(e: any) => {
                    let phoneCode = e.split('-')
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.phoneCode = phoneCode[0];
                    this.setState({
                      eventFinanceViewModel: eventFinanceViewModel,
                      phoneCodeKey: e
                    })
                  }}
                >
                  <Option value={`+91-india`} key={`+91-india`}>
                    +91 IN
                </Option>
                </Select>
              </div>
              <div className="form-group col-sm-9">
                <label>Phone Number</label>
                <input
                  name="phoneNumber"
                  placeholder="1234567890"
                  value={this.state.eventFinanceViewModel.phoneNumber}
                  onChange={(e) => {
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.phoneNumber = e.target.value;
                    this.setState({
                      eventFinanceViewModel: eventFinanceViewModel
                    })
                  }}
                  className="form-control"
                  type="number"
                  required
                />
              </div>
            </div>
          )}
          {this.state.screen == 2 && (
            <div className="form-row">
              <div className="form-group col-12">
                <label>Account Name</label>
                <input
                  name="name"
                  placeholder="Enter Account Name"
                  value={this.state.eventFinanceViewModel.accountName}
                  onChange={(e) => {
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.accountName = e.target.value;
                    this.setState({
                      eventFinanceViewModel: eventFinanceViewModel
                    })
                  }}
                  className="form-control"
                  type="name"
                  required
                />
              </div>
              <div className="form-group col-12">
                <label>Bank Name</label>
                <Select
                  showSearch
                  size={'large'}
                  placeholder="Select Bank Name"
                  defaultValue={this.state.eventFinanceViewModel.bankName ? this.state.eventFinanceViewModel.bankName : 'Select Bank Name'}
                  onChange={(e: any) => {
                    let selectedBank = e.split('+')
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.bankName = selectedBank[1];
                    eventFinanceViewModel.branchCode = '';
                    eventFinanceViewModel.accountNumber = '';
                    this.setState({
                      eventFinanceViewModel: eventFinanceViewModel,
                      ifscCodePrefix: selectedBank[0],
                      confirmAccountNumber: ''
                    })
                  }}
                >
                  {this.getBankModal()}
                </Select>
              </div>
              <div className="form-group col-12">
                <label>Branch Code</label>
                <div className="input-group">
                  {this.state.ifscCodePrefix && (
                    <div className="input-group-prepend">
                      <span className="input-group-text">{this.state.ifscCodePrefix}</span>
                    </div>
                  )}
                  <input
                    value={this.state.eventFinanceViewModel.branchCode}
                    type="text"
                    placeholder={!this.state.ifscCodePrefix ? 'Enter IFSC Code' : '0000123'}
                    className="form-control"
                    onBlur={() => {
                      if (this.state.eventFinanceViewModel.branchCode && this.state.ifscCodePrefix && this.state.eventFinanceViewModel.branchCode.length == 7) {
                        this.setState({ branchError: '', branchName: '' }, () => {
                          this.getBranchInfoByIFSC(`${this.state.ifscCodePrefix}${this.state.eventFinanceViewModel.branchCode}`)
                        })
                      } else if (
                        this.state.eventFinanceViewModel.branchCode &&
                        this.state.ifscCodePrefix &&
                        this.state.eventFinanceViewModel.branchCode.length != 7
                      ) {
                        this.setState({ branchError: 'Please enter valid IFSC code', branchName: '' })
                      }
                    }}
                    onChange={(e) => {
                      let eventFinanceViewModel = this.state.eventFinanceViewModel;
                      eventFinanceViewModel.branchCode = e.target.value;
                      this.setState({ eventFinanceViewModel: eventFinanceViewModel })
                    }}
                    aria-label="Enter Branch Code"
                  />
                  {this.state.branchError && <div className="col-sm-12 text-danger small">{this.state.branchError}</div>}
                  {this.state.branchName && <div className="col-sm-12 small text-success">{this.state.branchName}</div>}
                </div>
              </div>
              <div className="form-group col-12">
                <label>Account Number</label>
                <input
                  type="number"
                  className="form-control"
                  name="accountNumber"
                  placeholder="Enter Account Number"
                  value={this.state.eventFinanceViewModel.accountNumber}
                  onChange={(e) => {
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.accountNumber = e.target.value;
                    this.setState({ eventFinanceViewModel: eventFinanceViewModel })
                    if (e.target.value.length == 9) {
                      this.isBankAccountValid(this.state.eventFinanceViewModel.accountNumber)
                    }
                  }}
                  onBlur={() => {
                    this.isBankAccountValid(this.state.eventFinanceViewModel.accountNumber)
                  }}
                  required
                />
                {this.state.bankAccountNumberError && (
                  <div className="col-sm-12 text-danger small">{this.state.bankAccountNumberError}</div>
                )}
              </div>
              <div className="form-group col-12">
                <label>Confirm Account Number</label>
                <input
                  type="number"
                  className="form-control"
                  name="accountNumber"
                  placeholder="Confirm Account Number"
                  value={this.state.confirmAccountNumber}
                  onChange={(e) => {
                    if (this.state.eventFinanceViewModel.accountNumber != e.target.value) {
                      this.setState({ meesage: "Account number doesn't match", confirmAccountNumber: e.target.value })
                    } else {
                      this.setState({ meesage: '', confirmAccountNumber: e.target.value })
                    }
                  }}
                  onPaste={this.onPasteConfirmAccountNumber}
                  required
                />
                {this.state.meesage && <div className="col-sm-12 text-danger small">{this.state.meesage}</div>}
              </div>
              <div className="form-group col-12">
                <label>GST State {this.state.eventFinanceViewModel.accountTypeId == 1 ? '(Optional)' : ''}</label>
                <Select
                  showSearch
                  size={'large'}
                  placeholder="Select State"
                  defaultValue={this.state.selectedState ? this.state.selectedState : 'Select State'}
                  onChange={(e: any) => {
                    let selectedState = e.split('+')
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.stateId = selectedState[2];
                    this.setState({ eventFinanceViewModel: eventFinanceViewModel, selectedState: selectedState[0] })
                  }}
                >
                  {this.getStateOption()}
                </Select>
              </div>
              <div className="form-group col-12">
                <label>GST Number {this.state.eventFinanceViewModel.accountTypeId == 1 ? '(Optional)' : ''} </label>
                <input
                  type="text"
                  className="form-control"
                  placeholder="Enter GST Number"
                  value={this.state.eventFinanceViewModel.taxId}
                  required={this.state.eventFinanceViewModel.value == 2 ? true : false}
                  onChange={(e) => {
                    let eventFinanceViewModel = this.state.eventFinanceViewModel;
                    eventFinanceViewModel.taxId = e.target.value;
                    this.setState({ eventFinanceViewModel: eventFinanceViewModel })
                  }}
                />
              </div>
            </div>
          )}
          <div className="form-row">
            <div className="col-12 border-top pt-3">
              {this.state.screen == 2 && (
                <div>
                  By clicking Done, you agree to the{' '}
                  <Link to="/terms-and-conditions" target="_blank" className="btn-link">
                    Additional Terms and Conditions
                </Link>{' '}
                and you confirm that the information you have provided is complete and correct.
                </div>
              )}

            </div>
          </div>
        </div>
        <Footer
          isDisabled={!this.isButtonDisable()}
          isSaveRequest={this.props.props.EventFinance.isSaveRequest}
          onClickCancel={() => { this.setState({ screen: 1 }) }}
          saveText={this.state.screen == 1 ? 'Next' : 'Save'}
          cancelText={'Back'}
          isHideCancel={this.state.screen == 2 ? false : true}
          onSubmit={() => { this.form.dispatchEvent(new Event('submit')) }} />
      </form>
    )
  }
}
