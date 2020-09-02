import * as React from "react";
import AddOnForm from "./AddOnForm";
import { Modal as antModal, Button, notification, Tooltip, Drawer } from 'antd';
import { InventoryRequestViewModel } from "../../../../models/Inventory/InventoryRequestViewModel";
import * as _ from "lodash";

class AddOn extends React.Component<any, any> {
  state = {
    isDrawer: false,
    confirmMessage: "",
    addOnList: [],
    selectedItem: null,
    ticketList: []
  };

  submitAddOnForm = null;

  componentDidMount() {
    if (sessionStorage.getItem("ticketList")) {
      let ticketList = JSON.parse(sessionStorage.getItem("ticketList"));
      if (ticketList) {
        this.setState({ ticketList: ticketList });
      }
    }
  }

  bindSubmitForm = (submitForm) => {
    this.submitAddOnForm = submitForm;
  };

  handleSubmitAddonFrom = (e) => {
    this.submitAddOnForm(e);
  }

  getSubmitButton = () => {
    if (!this.state.isDrawer) {
      return (
        <div className="text-center pt-4 pb-4">
          <div className="line" />
          <a
            href="JavaScript:Void(0)"
            role="button"
            onClick={() => {
              this.props.onClickCancel()
            }}
            className="text-decoration-none mr-2">
            Back
                    </a>
          <span>|</span>
          <a
            href="JavaScript:Void(0)"
            role="button"
            onClick={this.props.onSkipAddons}
            className="text-decoration-none mx-2">
            Skip
                    </a>
          <div className="btn-group">
            <button
              type="button"
              onClick={this.onSubmitAddOns}
              disabled={(this.state.addOnList && this.state.addOnList.length > 0) ? false : true}
              className="btn btn-outline-primary">
              Submit
                        </button>
          </div>
        </div>
      );
    } else {
      return (
        <div className="text-center pt-4 pb-4">
          <div className="line" />
          <a
            href="JavaScript:Void(0)"
            role="button"
            onClick={() => this.setState({
              isDrawer: false
            })}
            className="text-decoration-none mr-2">
            Cancel
                    </a>
          <div className="btn-group">
            <button
              onClick={this.handleSubmitAddonFrom}
              type="button"
              className="btn btn-outline-primary">
              Add
                        </button>
          </div>
        </div>
      );
    }
  }

  error = () => {
    antModal.error({
      title: this.state.confirmMessage,
      centered: true
    });
  }

  onAddAddons = (data) => {
    let currentAddon = this.state.addOnList.filter(val => val.categoryName == data.categoryName);
    if (currentAddon.length > 0) {
      let addOn = this.state.addOnList;
      addOn = addOn.map((item) => {
        if (item.categoryName == data.categoryName) {
          item.categoryName = data.categoryName;
          item.quantity = data.quantity;
          item.currencyId = data.currencyId;
          item.pricePerTicket = data.pricePerTicket;
        }
        return item;
      })
      this.setState({
        addOnList: [...addOn],
        isDrawer: false,
        confirmMessage: "Add-on added!"
      }, () => this.successNotifier('success'));
    } else {
      this.setState({
        addOnList: [...this.state.addOnList, ...[data]],
        isDrawer: false,
        confirmMessage: "Add-on added!"
      }, () => this.successNotifier('success'));
    }
  };

  onSubmitAddOns = () => {
    this.setState({
      addOnList: [...this.state.addOnList],
    }, () => {
      let currencyLength = this.state.addOnList.map((val) => { return val.currencyId }).filter(function (value, index, self) {
        return self.indexOf(value) === index;
      });

      if (currencyLength.length > 1) {
        this.setState({
          confirmMessage: "You can use only one currency per event. Please edit the currency selection."
        }, () => {
          this.error();
        })
        return false;
      }
      let InventoryModel: InventoryRequestViewModel = {
        ticketCategoriesViewModels: [...this.state.ticketList, ...this.state.addOnList],
        deliverTypeId: [1],
        placeAltId: localStorage.getItem("placeAltId") ? localStorage.getItem("placeAltId") : null,
        redemptionDateTime: "",
        redemptionInstructions: "",
        termsAndCondition: "",
        customerIdTypes: [],
        isCustomerIdRequired: false,
        eventDetailAltIds: [],
        redemptionAddress: "test",
        redemptionCity: "test",
        redemptionState: "test",
        redemptionCountry: "test",
        redemptionZipcode: "000",
        refundPolicy: 0,
        customerInformation: [0],
        isEdit: false,
        feeTypes: []
      };
      this.confirm(InventoryModel);
    });
  };

  confirm = (InventoryModel) => {
    this.props.inventoryDataSubmit(InventoryModel, (response: any) => {
      if (response.success) {
        this.setState({
          confirmMessage: "Your Add-ons have been saved successfully!"
        }, () => {
          this.successNotifier('success');
          this.props.onSkipAddons();
        });
      } else {
        this.setState({
          confirmMessage: "Your ticket information has not been saved! Please try again."
        }, () => {
          this.error();
        })
      }
    });
  }

  successNotifier = type => {
    notification[type]({
      message: this.state.confirmMessage,
      duration: 10,
      top: 10
    });
  };

  render() {
    return (
      <div className="p-3">
        <a
          className="place-listing active"
          data-toggle="collapse"
          href="#Ticket"
          role="button"
          aria-expanded="false"
          aria-controls="Ticket"
        >
          <span className="rounded-circle border border-primary">
            1
                    </span>
                Add-ons
                </a>
        <Tooltip title={
          <p >
            <span >
              Add the Add-ons for this event.
                        </span>
          </p>}>
          <span><i className="fa fa-info-circle text-primary ml-2" ></i></span>
        </Tooltip>
        {this.state.addOnList.map((item, index) => {
          let currency = this.props.currencyOptions.filter(t => t.value == item.currencyId)[0];
          return (
            <div key={item.categoryName} className="card col-sm-6 p-0 my-2">
              <div className="card-header border-0 p-2">
                <a href="javascript:void(0)" onClick={() => this.setState({
                  selectedItem: item,
                  isDrawer: true
                })}>
                  {`${item.categoryName} - ${currency.code} ${item.pricePerTicket}`}
                </a>
                <div className="pull-right">
                  <a href="javascript:void(0)"><i className="fa fa-trash-o text-danger"
                    onClick={() => {
                      let allTkt = [...this.state.addOnList];
                      _.remove(allTkt, (t) => t.categoryName == item.categoryName);
                      this.setState({ addOnList: allTkt });
                    }} ></i>
                  </a>
                </div>
              </div>
            </div>
          );
        })}
        <div className="px-2 form-group">
          <a
            href="javaScript:void(0)"
            onClick={() => this.setState({
              selectedItem: null,
              isDrawer: true
            })}
            className="btn btn-sm btn-outline-primary mt-3">
            <small>
              <i className="fa fa-plus mr-2" aria-hidden="true"></i>
              {(this.state.addOnList && this.state.addOnList.length > 0) ? "Add More Add-Ons" : "Add New Add-Ons"}
            </small>
          </a>
        </div>
        {this.getSubmitButton()}

        <Drawer
          title="Add-on Information"
          placement={'right'}
          closable={false}
          onClose={() => { this.setState({ isDrawer: false }) }}
          visible={this.state.isDrawer}
          width={700}
        >
          {
            (this.state.isDrawer) ?
              (<>
                <AddOnForm
                  currencyOptions={this.props.currencyOptions}
                  getSubmitButton={this.getSubmitButton}
                  onAddAddons={this.onAddAddons}
                  onSubmitAddOns={this.onSubmitAddOns}
                  isDrawer={this.state.isDrawer}
                  bindSubmitForm={this.bindSubmitForm}
                  selectedItem={this.state.selectedItem}
                  selectedCurrencyId={this.state.ticketList[0] && this.state.ticketList[0].currencyId}
                />
                {this.getSubmitButton()}
              </>) : null
          }
        </Drawer>
      </div>
    )
  }
}

export default AddOn;
