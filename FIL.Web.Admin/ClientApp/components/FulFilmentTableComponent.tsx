import * as React from 'react';
import ReactTable from 'react-table';
import S3FileUpload from 'react-s3';

export default class FulFilmentTableComponent extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      selectedRow: null,
      OTP: '',
      ticketNumber: ''
    };
  }

  private handleChange = (row) => {
    this.setState({
      selectedRow: row
    });
  };

  private generateOTP = () => {
    let { transactionDetailAltId, phoneCode, phoneNumber } = this.state.selectedRow;
    this.props.generateOTP(phoneNumber, phoneCode, transactionDetailAltId);
  };

  private submitData = () => {
    if (this.state.OTP == '') {
      alert('Please enter OTP to continue');
    } else {
      this.props.submitData(this.state.OTP, this.state.ticketNumber);
    }
  };

  private onchangeOTP = (e) => {
    this.setState({
      OTP: e.target.value
    });
  };

  private onchangeTicketNumber = (e) => {
    this.setState({
      ticketNumber: e.target.value
    });
  };

  public handleselectedFile = (e) => {
    const config = {
      bucketName: 'fulfl',
      dirName: this.state.selectedRow,
      region: 'us-west-2',
      accessKeyId: 'AKIAIOQ6KTGXD2WDD5GQ',
      secretAccessKey: '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'
    };
    S3FileUpload.uploadFile(e.target.files[0], config).catch((err) => console.error(err));
  };

  public render() {
    const columns = [
      {
        Header: 'Sr No',
        accessor: 'srNo'
      },
      {
        Header: 'Select Transaction',
        accessor: 'button',
        Cell: ({ row }) => (
          <div className="text-center">
            <form>
              <input
                type="radio"
                name="transactionid"
                value={row}
                onChange={() => this.handleChange(row)}
                checked={
                  (this.state.selectedRow && this.state.selectedRow.confirmationNumber) == row.confirmationNumber
                }
              />
            </form>
          </div>
        )
      },
      {
        Header: 'Confirmation Number',
        accessor: 'confirmationNumber'
      },
      {
        Header: 'Date/Time',
        accessor: 'transactiondate'
      },
      {
        Header: 'Email ID',
        accessor: 'email'
      },
      {
        Header: 'Mobile',
        accessor: 'userMobileNumber'
      },
      {
        Header: 'Cust. Name',
        accessor: 'buyerName'
      },
      {
        Header: 'Event Name',
        accessor: 'eventName'
      },
      {
        Header: ' Event Date',
        accessor: 'eventDate'
      },
      {
        Header: ' Ticket Category Name',
        accessor: 'ticketCategoryName'
      },
      {
        Header: ' Total Tickets',
        accessor: 'totalTicket'
      },
      {
        Header: ' Gross Ticket Amount',
        accessor: 'grossTicketAmount'
      },

      {
        Header: ' Channel',
        accessor: 'channel'
      },
      {
        Header: ' Country',
        accessor: 'countryName'
      },
      {
        Header: 'TransactionDetailAltId',
        accessor: 'transactionDetailAltId',
        show: false
      },
      {
        Header: 'Phone Code',
        accessor: 'phoneCode',
        show: false
      }
    ];

    let { data } = this.props;

    let tableData = data.transactionInfos.map((item) => ({
      srNo: item.srNo,
      confirmationNumber: item.confirmationNumber,
      transactiondate: item.createdUtc,
      email: item.userEmailId,
      userMobileNumber: item.userMobileNumber,
      eventName: item.eventName,
      buyerName: item.buyerName,
      totalTicket: item.totalTicket,
      grossTicketAmount: item.grossTicketAmount,
      eventDate: item.eventDate,
      ticketCategoryName: item.ticketCategoryName,
      channel: item.channel,
      paymentGateway: item.paymentGateway,
      payConfNumber: item.payConfNumber,
      transactionStatus: item.transactionStatus,
      countryName: item.countryName,
      transactionDetailId: item.transactionDetailId,
      transactionDetailAltId: item.transactionDetailAltId,
      phoneCode: item.phoneCode
    }));

    var defaultPageSize = data.transactionInfos.length < 10 ? data.transactionInfos.length : 10;

    return (
      <div>
        <div className="table table-striped table-bordered example-table mt-30">
          <ReactTable
            data={tableData}
            columns={columns}
            defaultPageSize={defaultPageSize}
            filterable={true}
            minRows={0}
            className="-striped -highlight"
          />
        </div>
        {this.state.selectedRow && (
          <div>
            <div className="row">
              <div className="col-md-5 mb-2">
                <div className="input-group mb-2">
                  <input
                    type="text"
                    className="form-control col-6"
                    placeholder="Enter OTP*"
                    onChange={this.onchangeOTP}
                  />
                  <button
                    type="button"
                    style={{ marginLeft: '50px' }}
                    onClick={this.generateOTP}
                    className="btn btn-primary"
                  >
                    Generate OTP
                  </button>
                </div>
              </div>
            </div>
            <input
              type="text"
              className="form-control col-6"
              placeholder="Enter Ticket Number"
              onChange={this.onchangeTicketNumber}
            />{' '}
            <br />
            <div className="form-group">
              <label>Upload Document</label>
              <input className="form-control-file" type="file" name="" id="" onChange={this.handleselectedFile} />
            </div>
            <div className="text-center">
              <button type="button" onClick={this.submitData} className="btn btn-primary">
                Submit
              </button>
            </div>
          </div>
        )}
      </div>
    );
  }
}
