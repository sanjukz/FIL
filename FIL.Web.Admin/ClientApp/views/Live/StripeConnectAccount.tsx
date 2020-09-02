import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router-dom";
import { IApplicationState } from "../../stores";
import { bindActionCreators } from "redux";
import Spinner from "../../components/Spinner";
import * as inventoryStore from "../../stores/Inventory";
import * as parse from "url-parse";
import InventoryResponseViewModel from "../../models/Inventory/InventoryResponseViewModel";

type PlaceCalendarProps = inventoryStore.InventoryProps
  & typeof inventoryStore.actionCreators
  & RouteComponentProps<{}>;

class StripeConnectAccount extends React.Component<PlaceCalendarProps, any> {
  constructor(props) {
    super(props);
    this.state = {
    }
  }

  componentDidMount() {
    const data: any = parse(location.search, location, true);
    let code = "", placeAltId = "", placeId = "";
    if (data.query.code) {
      code = data.query.code;
    }
    if (localStorage.getItem('placeAltId') != null && localStorage.getItem('placeAltId') != '0') {
      placeAltId = localStorage.getItem('placeAltId');
    }
    if (localStorage.getItem('placeId') != null && localStorage.getItem('placeId') != '0') {
      placeId = localStorage.getItem('placeId');
    }
    if (code != "" && placeAltId != "") {
      this.props.saveStripeConnectAccount(code, placeAltId, (item: InventoryResponseViewModel) => {
        if (item.success) {
          sessionStorage.setItem("isEventCreation", "true");
          window.location.replace(`/host-online/${placeId}/submission`)
        }
      });
    }
  }

  render() {
    return (
      <div>
        {(this.props.inventory.isStripeAccountSaveRequest) && <Spinner isShowLoadingMessage={true} />}
        {(this.props.inventory.isStripeAccountSaveSuccess) && <div>
          <div className="home-view-wrapper text-center container py-5 my-5">
            <h3>Hey! Stripe connect account created successfully.</h3>
            <a href="/" className="btn bnt-lg site-primery-btn mt-4">Go to home</a>
          </div>
        </div>}
        {(this.props.inventory.isStripeAccountSaveFailed) && <div>
          <div className="home-view-wrapper text-center container py-5 my-5">
            <h3>Hey! There is some issue with the stripe connect account creation.</h3>
            <a href="/" className="btn bnt-lg site-primery-btn mt-4">Go to home</a>
          </div>
        </div>}
      </div >
    );
  }
}
export default connect(
  (state: IApplicationState) => ({ inventory: state.inventory }),
  (dispatch) => bindActionCreators({ ...inventoryStore.actionCreators }, dispatch)
)(StripeConnectAccount);
