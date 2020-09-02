/*Third Party Imports */
import * as React from 'react'
import { Select as AntdSelect } from 'antd';

/*Local Imports */
import { Footer } from "../Footer/FormFooter";
import { ReplayViewModel } from "../../../models/CreateEventV1/ReplayViewModel";
import { ToolTip } from "../ToolTip";
import { ReplayForm } from './ReplayForm';
import { setReplayObject } from "../utils/DefaultObjectSetter";

export class Index extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
      replayModel: setReplayObject(11, this.props.slug)
    }
  }

  componentDidMount() {
    if (this.props.slug) {
      this.props.props.requestEventReplayDetails(this.props.slug, (response: ReplayViewModel) => {
        if (response.success && response.replayDetailModel.length > 0) {
          console.log(response);
          this.setState({ replayModel: response });
        }
      });
    }
  }

  onClickSkip = (props) => {
    this.props.changeRoute(10, null, true);
  }


  isDisable = () => {
    let paidReplayModel = this.state.replayModel.replayDetailModel.filter((val) => {
      return val.isPaid
    })[0];
    let freeReplayModel = this.state.replayModel.replayDetailModel.filter((val) => {
      return !val.isPaid
    })[0];
    return !((paidReplayModel.isEnabled ? (paidReplayModel.startDate != null && paidReplayModel.endDate != null) : true) && (freeReplayModel.isEnabled ? (freeReplayModel.startDate != null && freeReplayModel.endDate != null) : true))
  }

  render() {
    const { Option } = AntdSelect;
    let currencies = [];
    let activeCurrencies = this.state.currencyOptions || [];
    activeCurrencies.map((item) => {
      currencies.push(<Option value={`${item.value}+${item.label}`} key={`${item.value}+${item.label}`}>{item.label}</Option>)
    });
    return (<>
      <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="3">
        <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
          <h3 className="m-0 text-purple">Extending your experience display life</h3></nav>
        <h5>Paid Replay Option <ToolTip className="ml-2" description={"Paid Replay Option"} /> </h5>
        <ReplayForm
          props={this.props.props}
          onChangeReplaySwitch={(e) => {
            let replayModel = this.state.replayModel;
            replayModel.replayDetailModel.forEach(val => {
              if (val.isPaid) {
                val.isEnabled = e
              }
            });
            this.setState({ replayModel: replayModel })
          }}
          onChangeStartDate={(e) => {
            let replayModel = this.state.replayModel;
            replayModel.replayDetailModel.forEach(val => {
              if (val.isPaid) {
                val.startDate = e
              }
            });
            this.setState({ replayModel: replayModel })
          }}
          onChangeEndDate={(e) => {
            let replayModel = this.state.replayModel;
            replayModel.replayDetailModel.forEach(val => {
              if (val.isPaid) {
                val.endDate = e
              }
            });
            this.setState({ replayModel: replayModel })
          }}
          replayViewModel={this.state.replayModel.replayDetailModel.filter((val) => {
            return val.isPaid
          })[0]}
          onChangePrice={e => {
            let replayModel = this.state.replayModel;
            replayModel.replayDetailModel.forEach(val => {
              if (val.isPaid) {
                val.price = e
              }
            });
            this.setState({ replayModel: replayModel })
          }}
          onChangeCurrency={e => {
            let replayModel = this.state.replayModel;
            replayModel.replayDetailModel.forEach(val => {
              if (val.isPaid) {
                val.currencyId = e
              }
            });
            this.setState({ replayModel: replayModel })
          }}
          isPaidReplay={true} />
      </div>
      <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box mt-4" id="nav-tabContent" key="3">
        <h5>Free Replay Option <ToolTip className="ml-2" description={"Free Replay Option"} /> </h5>
        <ReplayForm
          props={this.props.props}
          onChangeReplaySwitch={(e) => {
            let replayModel = this.state.replayModel;
            replayModel.replayDetailModel.forEach(val => {
              if (!val.isPaid) {
                val.isEnabled = e
              }
            });
            this.setState({ replayModel: replayModel })
          }}
          onChangeStartDate={(e) => {
            let replayModel = this.state.replayModel;
            replayModel.replayDetailModel.forEach(val => {
              if (!val.isPaid) {
                val.startDate = e
              }
            });
            this.setState({ replayModel: replayModel })
          }}
          onChangeEndDate={(e) => {
            let replayModel = this.state.replayModel;
            replayModel.replayDetailModel.forEach(val => {
              if (!val.isPaid) {
                val.endDate = e
              }
            });
            this.setState({ replayModel: replayModel })
          }}
          replayViewModel={this.state.replayModel.replayDetailModel.filter((val) => {
            return !val.isPaid
          })[0]}
          isPaidReplay={false} />
      </div>
      <Footer
        isDisabled={this.isDisable()}
        isShowSkip={true}
        onClickSkip={() => { this.onClickSkip(this.props) }}
        isSaveRequest={this.props.props.EventReplay.isSaveRequest}
        onClickCancel={() => { this.props.changeRoute(8, "", true) }}
        onSubmit={() => {
          let replayModel = this.state.replayModel;
          replayModel.currentStep = 11;
          this.props.props.saveEventReplay(replayModel, (response: ReplayViewModel) => {
            if (response.success) {
              this.props.changeRoute(10, response.completedStep, true);
            }
          })
        }} />
    </>)
  }
}

