import * as React from 'react'
import { bankNames } from '../../../../utils/IndianBankNames'
import { Radio, Select } from 'antd'
import { fetch } from 'domain-task'
import { FinanceDetailViewModal } from '../../../../models/Finance/FinanceDetailViewModal'
import { Link } from 'react-router-dom'
import { CUSTOM_REGEX } from '../../../../utils/regexExpression'

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
      screen: 1
    }
  }

  form: any

  componentDidMount() {}

  onChange = (e) => {
    console.log('radio checked', e.target.value)
    this.setState({
      value: e.target.value
    })
  }

  getBankModal = () => {
    const { Option } = Select
    let bankNameArray = []
    let bankName = bankNames
    for (var bank in bankName) {
      bankNameArray.push(
        <Option value={bankName[bank]} key={`${bank}+${bankName[bank]}`}>
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
          <Option value={item.altId} key={`${item.name}+${item.altId}+${item.id}`}>
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

  onBlurConfirmAccountNumber = () => {
    if (this.state.accountNumber != this.state.confirmAccountNumber) {
      this.setState({ meesage: "Account number doesn't match" })
    } else {
      this.setState({ meesage: '' })
    }
  }

  onPasteConfirmAccountNumber = (e) => {
    e.preventDefault()
  }

  validatePhoneNumber = (e) => {
    if (e.target.value !== '' && !CUSTOM_REGEX.phoneNumber.ex.test(e.target.value)) {
      alert(CUSTOM_REGEX.phoneNumber.msg)
      this.setState({
        phoneNumber: ''
      })
    }
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
            let financeDetail: FinanceDetailViewModal = {
              accountType: this.state.value,
              firstName: this.state.firstName,
              lastName: this.state.lastName,
              email: this.state.email,
              companyName: this.state.companyName,
              phoneCode: 91,
              phoneNumber: this.state.phoneNumber,
              accountName: this.state.accountName,
              bankName: this.state.bankName,
              branchCode: `${this.state.ifscCodePrefix}${this.state.branchCode}`,
              accountNumber: this.state.accountNumber,
              stateId: this.state.stateId,
              gstNumber: this.state.gstNumber,
              countryId: 101,
              currencyId: 7,
              eventAltId: localStorage.getItem('placeAltId') ? localStorage.getItem('placeAltId') : null
            }
            this.props.onSubmit(financeDetail)
          } else {
            this.setState({ screen: 2 })
          }
        }}
        ref={(ref) => {
          this.form = ref
        }}
      >
        {this.state.screen == 1 && (
          <div className="form-row">
            <div className="form-group col-12">
              <div>
                Please fill in your bank details. This is to ensure that the net ticket sales amount can be transferred
                to your account. Once you have filled the details successfully, your experience/event will be submitted
                for approval.
                <hr />
              </div>
              <Radio.Group onChange={this.onChange} value={value}>
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
                value={this.state.firstName}
                onChange={(e) => {
                  this.setState({ firstName: e.target.value })
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
                value={this.state.lastName}
                onChange={(e) => {
                  this.setState({ lastName: e.target.value })
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
                value={this.state.email}
                onChange={(e) => {
                  this.setState({ email: e.target.value })
                }}
                className="form-control"
                type="email"
                required
              />
            </div>
            {this.state.value == 2 && (
              <div className="form-group col-12">
                <label>Company Name</label>
                <input
                  name="company"
                  placeholder="Enter Company Name"
                  value={this.state.companyName}
                  onChange={(e) => {
                    this.setState({ companyName: e.target.value })
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
                  this.setState({ phoneCode: phoneCode[0], phoneCodeKey: e })
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
                value={this.state.phoneNumber}
                onChange={(e) => {
                  this.setState({ phoneNumber: e.target.value })
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
                value={this.state.accountName}
                onChange={(e) => {
                  this.setState({ accountName: e.target.value })
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
                defaultValue={this.state.bankName ? this.state.bankName : 'Select Bank Name'}
                onChange={(e: any) => {
                  let selectedBank = e.split('+')
                  this.setState({
                    ifscCodePrefix: selectedBank[0],
                    bankName: selectedBank[1],
                    bankKey: e,
                    branchCode: '',
                    accountNumber: '',
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
                  value={this.state.branchCode}
                  type="text"
                  placeholder={!this.state.ifscCodePrefix ? 'Enter IFSC Code' : '0000123'}
                  className="form-control"
                  onBlur={() => {
                    if (this.state.branchCode && this.state.ifscCodePrefix && this.state.branchCode.length == 7) {
                      this.setState({ branchError: '', branchName: '' }, () => {
                        this.getBranchInfoByIFSC(`${this.state.ifscCodePrefix}${this.state.branchCode}`)
                      })
                    } else if (
                      this.state.branchCode &&
                      this.state.ifscCodePrefix &&
                      this.state.branchCode.length != 7
                    ) {
                      this.setState({ branchError: 'Please enter valid IFSC code', branchName: '' })
                    }
                  }}
                  onChange={(e) => {
                    this.setState({ branchCode: e.target.value })
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
                value={this.state.accountNumber}
                onChange={(e) => {
                  this.setState({ accountNumber: e.target.value })
                  if (e.target.value.length == 9) {
                    this.isBankAccountValid(this.state.accountNumber)
                  }
                }}
                onBlur={() => {
                  this.isBankAccountValid(this.state.accountNumber)
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
                onBlur={this.onBlurConfirmAccountNumber}
                value={this.state.confirmAccountNumber}
                onChange={(e) => {
                  if (this.state.accountNumber != e.target.value) {
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
              <label>GST State {this.state.value == 1 ? '(Optional)' : ''}</label>
              <Select
                showSearch
                size={'large'}
                placeholder="Select State"
                defaultValue={this.state.selectedState ? this.state.selectedState : 'Select State'}
                onChange={(e: any) => {
                  let selectedState = e.split('+')
                  this.setState({ stateId: selectedState[2], selectedState: selectedState[0] })
                }}
              >
                {this.getStateOption()}
              </Select>
            </div>
            <div className="form-group col-12">
              <label>GST Number {this.state.value == 1 ? '(Optional)' : ''} </label>
              <input
                type="text"
                className="form-control"
                placeholder="Enter GST Number"
                value={this.state.gstNumber}
                required={this.state.value == 2 ? true : false}
                onChange={(e) => {
                  this.setState({ gstNumber: e.target.value })
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
            {this.state.screen == 2 && (
              <a
                href="JavaScript:Void(0)"
                onClick={() => {
                  this.setState({ screen: 1 })
                }}
                className="btn btn-outline-primary pull-left"
              >
                Back
              </a>
            )}
            <button
              disabled={
                (this.state.screen == 1 &&
                  (this.state.value == 1 ? true : this.state.companyName ? true : false) &&
                  this.state.firstName &&
                  this.state.lastName &&
                  this.state.phoneNumber) ||
                (this.state.screen == 2 &&
                  (this.state.value == 2 ? (this.state.stateId ? true : false) : true) &&
                  this.state.accountName &&
                  this.state.bankName &&
                  this.state.branchCode &&
                  this.state.accountNumber &&
                  this.state.confirmAccountNumber &&
                  !this.state.branchError &&
                  !this.state.bankAccountNumberError &&
                  this.state.accountNumber == this.state.confirmAccountNumber)
                  ? false
                  : true
              }
              type="submit"
              className="btn btn-outline-primary pull-right"
            >
              {this.state.screen == 1 ? 'Next' : 'Done'}
            </button>
          </div>
        </div>
      </form>
    )
  }
}
