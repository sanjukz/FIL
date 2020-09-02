import * as React from "react";
import { autobind } from "core-decorators";
import * as Datetime from "react-datetime";
import { CurrencyTypesResponseViewModel } from "../models/Inventory/CurrencyTypesResponseViewModel";
import DocumentTypesResponseViewModel from "../models/Inventory/DocumentTypesResponseViewModel";
import DeliveryTypesResponseViewModel from "../models/Inventory/DeliveryTypesResponseViewModel";
import { InventoryRequestViewModel } from "../models/Inventory/InventoryRequestViewModel";
import DocumentTypesRequestViewModel from "../models/Inventory/DocumentTypesRequestViewModel";
import RefundPolicyResponseViewModel from "../models/Inventory/RefundPolicyResponseViewModel";
import PlaceCalendarResponseViewModel from "../models/PlaceCalendar/PlaceCalendarResponseViewModel";
import { TicketCategories, FeeTypes } from "../models/Inventory/InventoryRequestViewModel";
import { GetPlaceInventoryDataResponseViewModel } from "../models/Inventory/GetPlaceInventoryDataResponseViewModel";
import TicketCategoryTypesResponseViewModel from "../models/TicketCategoryType/TicketCategoryTypesResponseViewModel";
import CustomerInformationResposeViewModel from "../models/CustomerInformation/CustomerInformationResposeViewModel";
import PlaceAutocomplete from "./PlaceAutocomplete";
import Select from 'react-select';
import CKEditor from "react-ckeditor-component";
import Newloader from "../components/NewLoader/NewLoader";
import * as ReactTooltip from 'react-tooltip';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const FEE_TYPES = ["Convenience Charge", "Service Charge", "SMS Charge", "PrintAtHome Charge", "Bank Charge", "MTicket Charge", "Other Charge", "Transaction Fee", "CreditCard Surcharge"];
const VALUE_TYPES = ["Percentage", "Flat"];

let feeTypeOptions = [], valueTypeOptions = [];
interface IInventory {
  currencyType: CurrencyTypesResponseViewModel;
  documentType: DocumentTypesResponseViewModel;
  deliveryType: DeliveryTypesResponseViewModel;
  refundPolicies: RefundPolicyResponseViewModel;
  inventoryData: GetPlaceInventoryDataResponseViewModel;
  disabled: any;
  placeCalendarResponse: PlaceCalendarResponseViewModel;
  onSubmitInventoryData: () => void;
  handleEditButtonPressedInventory: () => void;
  handleEditSaveButtonPressedInventory: () => void;
  handleEditRevertButtonPressedInventory: () => void;
  onSubmitSaveNewCustomerIdType: () => void;
  isEdit: boolean;
  placeAltId: any;
  ticketCategoryType: TicketCategoryTypesResponseViewModel;
  allInventoryStore: any;
  customerInformationControls: CustomerInformationResposeViewModel;
}

var uniqId = 0, summary;

export default class Inventory extends React.Component<IInventory, any> {
  ticketCatName: any;
  currency: any;
  pricePerTicket: any;
  quantity: any;
  ticketCategoryDescription: any;
  ticketCategoryNote: any;
  ticketValidity: any;
  ticketRollingDays: any;
  ticketFixDays: any;
  days: any;
  month: any;
  year: any;
  myRef: any;

  constructor(props) {
    super(props);
    this.state = {
      ticketCategory: [],
      selectedCustomerIdTypes: null,
      selectedDeliveryOptionTypes: null,
      selectedCustomerInformations: null,
      isCustomerIdAllowed: true,
      isCustomerIdAllowedNo: false,
      inventoryType: "paid",
      ticketCategoryNote: "",
      ticketCategoryTermsAndCondition: "",
      redemptionInstruction: "",
      isAddNewCustomerIdType: false,
      isAddCustomTicketValidity: false,
      isFooAndBeverage: false,
      newCustomerIdType: "",
      checkedRefundPolicyId: 4,
      isVenuePickup: false,
      errorMsg: "",
      inventoryDetailAccordianEditComponent: <div></div>,
      isInventoryDetailEdit: false,
      isStateUpdated: false,
      selectedTicketType: [],
      isRegularTicketTypeShow: false,
      isAddOnsTicketTypeShow: false,
      isTicketCategoryShow: false,
      ticketSubCategoryTypeId: 0,
      isTicketCategoryTypeIdUpdated: false,
      isAlertCall: false,
      isCustomerIdTypeSelect: false,
      feeTypesList: [],
      pattern: /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/g
    }
    this.updateContent = this.updateContent.bind(this);
  }

  public componentDidMount() {
    var ticketCat = [];
    uniqId = uniqId + 1;
    var ticketCategoryViewModel: TicketCategories = {
      id: uniqId,
      eventTicketDetailId: 0,
      categoryName: "",
      isEventTicketAttributeUpdated: false,
      ticketCategoryId: 0,
      currencyId: 7,
      pricePerTicket: 0,
      quantity: 0,
      ticketCategoryDescription: "",
      isRollingTicketValidityType: true,
      ticketValidityFixDate: "",
      ticketCategoryNote: "",
      days: "",
      month: "",
      year: "",
      ticketCategoryTypeId: 0,
      ticketSubCategoryTypeId: 0
    }
    ticketCat.push(ticketCategoryViewModel);
    localStorage.removeItem("_Inventory_data");
    localStorage.removeItem("_customer_Type");
    FEE_TYPES.map((item, index) => {
      let tempData = {
        label: item,
        value: index
      };
      feeTypeOptions.push(tempData)
    });
    VALUE_TYPES.map((item, index) => {
      let tempData = {
        label: item,
        value: index
      };
      valueTypeOptions.push(tempData)
    })
    if (!this.props.isEdit) {
      this.setState({ feeTypesList: [...this.state.feeTypesList, 0] })
    }
    this.setState({ ticketCategory: ticketCat });

  }

  @autobind
  notify() {
    toast.success('Your inventory information has been saved successfully!', {
      position: "top-center",
      autoClose: 6000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true
    });
    this.setState({ isCreate: true });
  }

  @autobind
  public onClickAddTicketCategories() {
    var ticketCat = this.state.ticketCategory;
    ticketCat.map(function (item, index) {
      item.id = index + 1;
    });
    uniqId = ticketCat.length;
    var isMainCatExists = [];
    var that = this;
    var isMaxAddOns = false;
    if (this.state.currentTicketType == 2) {
      isMainCatExists = this.state.ticketCategory.filter(function (item) {
        return item.ticketCategoryTypeId == 1 && item.categoryName != ""
      });

      var AddOns = this.state.ticketCategory.filter(function (item) {
        return item.ticketCategoryTypeId == 2 && item.categoryName != ""
      });
      var subType = AddOns.map(function (item) {
        if (item.ticketSubCategoryTypeId != that.state.ticketSubCategoryTypeId) {
          return item.ticketSubCategoryTypeId
        }
      });
      var data = subType.filter(function (value, index, self) {
        return self.indexOf(value) === index;
      });
      if (data.length == 3) {
        isMaxAddOns = true;
      }
    }
    if ((isMainCatExists.length > 0 || this.state.currentTicketType == 1) && !isMaxAddOns) {
      uniqId = uniqId + 1;
      var ticketCategoryViewModel: TicketCategories = {
        id: uniqId,
        eventTicketDetailId: 0,
        isEventTicketAttributeUpdated: false,
        ticketCategoryId: 0,
        categoryName: "",
        currencyId: 7,
        pricePerTicket: 0,
        quantity: 0,
        ticketCategoryDescription: "",
        isRollingTicketValidityType: true,
        ticketValidityFixDate: "",
        ticketCategoryNote: "",
        days: "",
        month: "",
        year: "",
        ticketCategoryTypeId: this.state.currentTicketType,
        ticketSubCategoryTypeId: this.state.ticketSubCategoryTypeId
      }
      ticketCat.push(ticketCategoryViewModel);
      this.updateSummary(ticketCat);
      this.setState({ ticketCategory: ticketCat });
    } else if (!isMaxAddOns && isMainCatExists.length == 0) {
      alert("Please add main inventory before adding add-ons");
    } else if (isMaxAddOns) {
      alert("You can add maximum 3 add-ons");
    }
  }

  @autobind
  getCurrencyInfoByCurrencyName(currenctCurrency) {
    var currency = this.props.currencyType.currencyTypes.filter(function (val) {
      return val.code == currenctCurrency
    });
    return currency;
  }

  @autobind
  getCurrencyInfoByCurrencyId(currenctCurrencyId) {
    var currency = this.props.currencyType.currencyTypes.filter(function (val) {
      return val.id == currenctCurrencyId
    });
    return currency;
  }

  @autobind
  public onValueChange(id, name, e) {
    var ticketCat = this.state.ticketCategory;
    var that = this;
    ticketCat.map(function (item) {
      if (item.id == id) {
        if (name == "ticketCatName") {
          item.categoryName = e.target.value
        } else if (name == "quantity") {
          if (this.state.pattern.test(e.target.value)) {
            return
          }
          item.quantity = e.target.value
          item.isEventTicketAttributeUpdated = true
        } else if (name == "discription") {
          var newContent = e.editor.getData();
          item.ticketCategoryDescription = newContent;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "currency") {
          var currency = that.getCurrencyInfoByCurrencyName(e.target.value);
          item.currencyId = currency[0].id;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "pricePerTicket") {
          item.pricePerTicket = e.target.value;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "note") {
          var newContent = e.editor.getData();
          item.ticketCategoryNote = newContent;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "ticketValidity") {
          item.ticketValidity = that.ticketValidity.value;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "fix") {
          item.isRollingTicketValidityType = false;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "ticketValidityFixDate") {
          item.ticketValidityFixDate = e;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "rolling") {
          item.isRollingTicketValidityType = true;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "days") {
          item.days = e.target.value;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "month") {
          item.month = e.target.value;
          item.isEventTicketAttributeUpdated = true
        } else if (name == "year") {
          item.year = e.target.value;
          item.isEventTicketAttributeUpdated = true
        }
      }
    });
    this.updateSummary(ticketCat);
    this.setState({ ticketCategory: ticketCat });
  }

  onChangeCustomerIdType = (selectedCustomerIdTypes) => {
    if (selectedCustomerIdTypes.length == 0) {
      this.setState({ selectedCustomerIdTypes: null, isAddNewCustomerIdType: false });
    } else {
      var isAddNew = false;
      selectedCustomerIdTypes.map(function (item) {
        if (item.label == "Add New") {
          isAddNew = true;
        }
      });
      if (isAddNew) {
        this.setState({ isAddNewCustomerIdType: true });
      } else {
        var selectedCustomerIdTypes = selectedCustomerIdTypes.filter(function (item) {
          if (item.label != "Add New") {
            return item;
          }
        });
        this.setState({ selectedCustomerIdTypes, isAddNewCustomerIdType: false });
      }
    }
  }

  public onChangeAddNewCustomerIdType(e) {
    this.setState({ newCustomerIdType: e.target.value });
  }

  @autobind
  public onClickSaveNewSelectCustomerType() {
    let documentType: DocumentTypesRequestViewModel = {
      documentType: this.state.newCustomerIdType
    }
    localStorage.setItem("_customer_Type", JSON.stringify(documentType));
    this.props.onSubmitSaveNewCustomerIdType();
  }

  onChangeDeliveryOptionType = (selectedDeliveryOptionTypes) => {
    this.setState({ selectedDeliveryOptionTypes });
    if (selectedDeliveryOptionTypes.length == 0) {
      this.setState({ selectedDeliveryOptionTypes: null });
      this.setState({ isVenuePickup: false });
    } else {
      var isVenuePickup = false;
      selectedDeliveryOptionTypes.map(function (item) {
        if (item.label == "VenuePickup") {
          isVenuePickup = true;
        }
      });
      if (isVenuePickup) {
        this.setState({ isVenuePickup: true });
      } else {
        this.setState({ isVenuePickup: false });
      }
      this.setState({ selectedDeliveryOptionTypes });
    }
  }

  onChangeCustomerInformations = (selectedCustomerInformations) => {
    this.setState({ selectedCustomerInformations });
    if (selectedCustomerInformations.length == 0) {
      this.setState({ selectedCustomerInformations: null });
      this.setState({ isCustomerIdTypeSelect: false });
    } else {
      var isCustomerIdTypeSelect = false;
      selectedCustomerInformations.map(function (item) {
        if (item.label.indexOf("Document") > -1) {
          isCustomerIdTypeSelect = true;
        }
      });
      if (isCustomerIdTypeSelect) {
        this.setState({ isCustomerIdTypeSelect: true });
      } else {
        this.setState({ isCustomerIdTypeSelect: false });
      }
      this.setState({ selectedCustomerInformations });
    }
  }

  @autobind
  public onCustomerIdClick(val, e) {
    if (val == "allow") {
      this.setState({ isCustomerIdAllowed: true, isCustomerIdAllowedNo: false });
    } else {
      this.setState({ isCustomerIdAllowed: false, isCustomerIdAllowedNo: true });
    }
  }

  @autobind
  public onClickSelectCustomerType() {
    this.setState({ isAddNewCustomerIdType: false });
  }

  @autobind
  public onClickCustomTicketType() {
    this.setState({ isAddCustomTicketValidity: false });
  }

  @autobind
  public onClickInventoryType(val, e) {
    this.setState({ inventoryType: val });
  }

  @autobind
  public OnChangeRedemptionDatetime(e) {
    this.setState({ redemptionStartDatetime: e })
  }

  @autobind
  public onChangeTicketCategoryName(evt) {
    var newContent = evt.editor.getData();
    this.setState({
      ticketCategoryNote: newContent
    })
  }

  @autobind
  public onChangeRedemptionInstruction(e) {
    this.setState({ redemptionInstruction: e.target.value });
  }

  @autobind
  public onRefundPolicyClick(refundPolicyId, e) {
    this.setState({ checkedRefundPolicyId: refundPolicyId });
  }

  @autobind
  public autoCompleteAddress(address, city, state, country, zipcode) {
    this.setState({
      RedemptionAddress: address, RedemptionCity: city,
      RedemptionState: state, RedemptionCountry: country,
      RedemptionZipcode: zipcode
    });
  }

  @autobind
  public onCustomFieldChange(zipcode) {
    this.setState({
      billingZipcode: zipcode
    });
  }

  @autobind
  updateContent(newContent) {
    this.setState({
      ticketCategoryTermsAndCondition: newContent
    })
  }

  @autobind
  onChangeTicketCategoryTermsAndCondition(evt) {
    if (this.props.disabled === false) {
      var newContent = evt.editor.getData();
      this.setState({
        ticketCategoryTermsAndCondition: newContent
      })
    }
    else {
      this.setState({
        ticketCategoryTermsAndCondition: ""
      })
    }
  }

  @autobind
  onRemoveCategory(index, e) {
    var data = this.state.ticketCategory;
    if (data.length == 1) {
      data = []
    } else {
      this.state.ticketCategory.map(function (item, currentIndex) {
        if (item.id == index) {
          data.splice(currentIndex, 1);
        }
      })
    }
    this.updateSummary(data);
    this.setState({ ticketCategory: data });
  }

  @autobind
  updatedTicketCategoryState() {

    var data = [];
    var customerDocument = [];
    var placeDeliveryTypes = [];
    var that = this;
    if (this.props.inventoryData.ticketCategoryContainer != undefined) {
      uniqId = 0;
      this.props.inventoryData.ticketCategoryContainer.map(function (item) {
        uniqId = uniqId + 1;
        var ticketCategoryViewModel: TicketCategories = {
          id: uniqId,
          ticketCategoryId: item.ticketCategory.id,
          eventTicketDetailId: item.eventTicketDetail.id,
          isEventTicketAttributeUpdated: false,
          categoryName: item.ticketCategory.name,
          currencyId: item.eventTicketAttribute.currencyId,
          pricePerTicket: item.eventTicketAttribute.price,
          quantity: item.eventTicketAttribute.remainingTicketForSale,
          ticketCategoryDescription: item.eventTicketAttribute.ticketCategoryDescription,
          isRollingTicketValidityType: (item.eventTicketAttribute.ticketValidityType == "Rolling" || item.eventTicketAttribute.ticketValidityType == "") ? true : false,
          ticketValidityFixDate: (item.eventTicketAttribute.ticketValidityType == "Fixed") ? (new Date()).toDateString() : "",
          ticketCategoryNote: item.eventTicketAttribute.ticketCategoryNotes,
          days: (item.eventTicketAttribute.ticketValidityType == "Rolling" && item.eventTicketAttribute.ticketValidity != "") ? item.eventTicketAttribute.ticketValidity.split("-")[0] : "",
          month: (item.eventTicketAttribute.ticketValidityType == "Rolling" && item.eventTicketAttribute.ticketValidity != "") ? item.eventTicketAttribute.ticketValidity.split("-")[1] : "",
          year: (item.eventTicketAttribute.ticketValidityType == "Rolling" && item.eventTicketAttribute.ticketValidity != "") ? item.eventTicketAttribute.ticketValidity.split("-")[2] : "",
          ticketCategoryTypeId: item.ticketCategoryTypeId,
          ticketSubCategoryTypeId: item.ticketCategorySubTypeId
        }
        data.push(ticketCategoryViewModel);
      });

      this.props.inventoryData.placeCustomerDocumentTypeMappings.map(function (item, index) {
        var i = 0;
        var customerDocumentType = that.props.inventoryData.customerDocumentTypes.filter(function (val) {
          i = i + 1;
          return val.id == item.customerDocumentType
        });
        var customer_document = {
          value: i,
          label: customerDocumentType[0].documentType
        };
        customerDocument.push(customer_document);
      });

      this.props.inventoryData.eventDeliveryTypeDetails.map(function (item, index) {
        var deliveyIndex = 0;
        var delivertTypesPlace = that.props.inventoryData.deliveryTypes.filter(function (val, currentIndex) {
          if (val == item.deliveryTypeId) {
            deliveyIndex = currentIndex;
            return val;
          }
        });
        if (delivertTypesPlace.length > 0) {
          var delivertTypes_place = {
            value: deliveyIndex,
            label: delivertTypesPlace[0]
          };
          placeDeliveryTypes.push(delivertTypes_place);
        }
      });
      //Ticket Fee Details
      let feeTypeList = [], previous_Categories = [], toRemove = true;
      this.props.inventoryData.ticketCategoryContainer.map((item, index) => {
        if (item.ticketFeeDetails.length > 0) {
          if (toRemove) {
            that.state.feeTypesList.pop();
          }
          toRemove = false;

          item.ticketFeeDetails.map((tktfee) => {
            let shouldInsert = [];
            shouldInsert = feeTypeList.filter((feetypes) => {
              return feetypes.feeId == tktfee.feeId
            })
            if (shouldInsert.length == 0) {
              feeTypeList.push(tktfee);
            }
            feeTypeList.map((feeType) => {
              if (feeType.feeId == tktfee.feeId) {
                feeType.ticketCat ? feeType.ticketCat = feeType.ticketCat + "," + item.ticketCategory.name : feeType.ticketCat = item.ticketCategory.name
              }
              feeType.eventTicketAttributeId = item.eventTicketAttribute.id
            })
          })

        }
        previous_Categories.push(item.ticketCategory.name);
      })

      feeTypeList.map((item, index) => {
        let tempFeeValue = {
          value: item.feeId - 1,
          label: FEE_TYPES[item.feeId - 1]
        }
        let tempValueType = {
          value: item.valueTypeId - 1,
          label: VALUE_TYPES[item.valueTypeId - 1]
        }
        let ticketCat = item.ticketCat.split(',');
        let tempCatValue, tempCatOptions = [];
        if (ticketCat.length == 1) {
          tempCatValue = {
            value: ticketCat,
            label: ticketCat
          }
          tempCatOptions.push(tempCatValue);
        } else if (ticketCat.length > 1) {
          if (ticketCat.length == previous_Categories.length) {
            tempCatValue = {
              value: "All",
              label: "All"
            }
            tempCatOptions.push(tempCatValue);
          } else {
            ticketCat.map((cat) => {
              let tempData = {
                value: cat,
                label: cat
              }
              tempCatOptions.push(tempData);
            })
          }
        }

        this.setState({
          [index + "FeeType"]: tempFeeValue, [index + "ValueType"]: tempValueType,
          [index + "FeeValue"]: item.value,
          [index + "feeCategoryName"]: tempCatOptions
        })
      })

      this.setState({
        ticketCategory: (data.length > 0 ? data : this.state.ticketCategory),
        selectedCustomerIdTypes: customerDocument,
        selectedDeliveryOptionTypes: placeDeliveryTypes,
        ticketCategoryTermsAndCondition: this.props.inventoryData.eventDeliveryTypeDetails.length > 0 ? this.props.inventoryData.eventDeliveryTypeDetails[0].notes : "",
        isStateUpdated: true,
        checkedRefundPolicyId: 4,
        feeTypesList: [...this.state.feeTypesList, ...feeTypeList]
      });
    } else {
      uniqId = 0;
      uniqId = uniqId + 1;
      var ticketCat = [];
      var ticketCategoryViewModel: TicketCategories = {
        id: uniqId,
        eventTicketDetailId: 0,
        categoryName: "",
        isEventTicketAttributeUpdated: false,
        ticketCategoryId: 0,
        currencyId: 7,
        pricePerTicket: 0,
        quantity: 0,
        ticketCategoryDescription: "",
        isRollingTicketValidityType: true,
        ticketValidityFixDate: "",
        ticketCategoryNote: "",
        days: "",
        month: "",
        year: "",
        ticketCategoryTypeId: 0,
        ticketSubCategoryTypeId: 0
      };
      ticketCat.push(ticketCategoryViewModel);
      this.setState({
        ticketCategory: ticketCat,
        selectedCustomerIdTypes: null,
        selectedDeliveryOptionTypes: null,
        ticketCategoryTermsAndCondition: "",
        isStateUpdated: true,
        checkedRefundPolicyId: 4
      });
    }
  }

  public enableField(e, index, isEnable) {
    this.setState({ [index]: isEnable });
  }

  public updateSummary(data) {
    var that = this;
    data = data.filter(function (item) {
      if (item.categoryName !== "") {
        return item;
      }
    });
    if (data.length > 0) {
      summary = data.map(function (val) {
        var ticketSubCat = that.props.ticketCategoryType.ticketCategorySubTypes.filter(function (item) {
          if (item.id == val.ticketSubCategoryTypeId) {
            return item
          }
        });
        var ticketCat = that.props.ticketCategoryType.ticketCategoryTypes.filter(function (item) {
          if (item.id == val.ticketCategoryTypeId) {
            return item
          }
        });
        var currency = that.getCurrencyInfoByCurrencyId(val.currencyId);
        return <tr>
          <td className="text-center">{ticketCat[0].name}</td>
          <td className="text-center">{ticketSubCat[0].name}</td>
          <td className="text-center">{val.categoryName}</td>
          <td className="text-center">{val.quantity}</td>
          <td className="text-center">{currency[0].code + " " + val.pricePerTicket}</td>
          <td className="text-center"><a href="JavaScript:Void(0)" onClick={that.onRemoveCategory.bind(that, val.id)} className="text-decoration-none btn-link mr-4">Delete</a></td>
        </tr>
      });
    } else {
      summary = undefined;
    }
    this.setState({ updatedSummary: true });
  }

  @autobind
  private onTicketTypeClick(val, e) {
    var data = this.state.selectedTicketType;
    var isRegularTicketTypeShow = false;
    var isAddOnsTicketTypeShow = false;
    var that = this;
    var ticketCat = this.props.ticketCategoryType.ticketCategorySubTypes.filter(function (item) {
      if (item.ticketCategoryTypeId == val.id) {
        return item
      }
    });
    var name = ticketCat[0].name;
    if (val.id == 1) {
      isRegularTicketTypeShow = true;
    } else {
      isAddOnsTicketTypeShow = true;
    }
    this.updateSummary(this.state.ticketCategory);
    this.setState({
      currentTicketType: val.id,
      isRegularTicketTypeShow: isRegularTicketTypeShow,
      isAddOnsTicketTypeShow: isAddOnsTicketTypeShow,
      selectedRegularTicketTypeId: ticketCat[0].id,
      selectedAddOnsTicketTypeId: ticketCat[0].id,
      isTicketCategoryShow: true,
      ticketSubCategoryTypeId: ticketCat[0].id,
      inventoryType: name
    });
  }
  @autobind
  onClickAddFeeType(e) {
    this.setState({ feeTypesList: [...this.state.feeTypesList, 0] })
  }
  @autobind
  removeAddFeeType(e) {
    this.state.feeTypesList.pop();
    this.setState({ temp: 45 })
  }
  @autobind
  private onRegularTicketTypeClick(val, e) {
    this.setState({ selectedRegularTicketTypeId: val.id, ticketSubCategoryTypeId: val.id, inventoryType: val.name });
  }

  @autobind
  private updatedTicketCategoryTypeIdState() { // to update category and sub categoryId at page load
    if (this.state.ticketCategory.length > 0) {
      var that = this;
      var ticketCategorySubType = this.props.ticketCategoryType.ticketCategorySubTypes.filter(function (item) {
        return item.ticketCategoryTypeId == that.props.ticketCategoryType.ticketCategoryTypes[0].id
      });
      var ticketCat = this.state.ticketCategory;

      ticketCat[0].ticketCategoryTypeId = this.props.ticketCategoryType.ticketCategoryTypes[0].id;
      ticketCat[0].ticketSubCategoryTypeId = ticketCategorySubType[0].id;
      this.setState({
        currentTicketType: this.props.ticketCategoryType.ticketCategoryTypes[0].id,
        ticketSubCategoryTypeId: ticketCategorySubType[0].id,
        ticketCat: ticketCat,
        isTicketCategoryTypeIdUpdated: true,
      })
    }
  }

  @autobind
  private onAddOnsTicketTypeClick(val, e) {
    this.setState({ selectedAddOnsTicketTypeId: val.id, ticketSubCategoryTypeId: val.id, inventoryType: val.name });
  }

  @autobind
  onChangeSummaryFilter(e) {
    var value = e.target.value.toLowerCase();
    var that = this;
    var data = this.state.ticketCategory.filter(function (item) {
      var isReturn = item.categoryName.toLowerCase().indexOf(value) > -1 || item.pricePerTicket.toString().indexOf(value) > -1 || item.quantity.toString().indexOf(value) > -1;
      var ticketSubCat = that.props.ticketCategoryType.ticketCategorySubTypes.filter(function (val) {
        if (val.id == item.ticketSubCategoryTypeId) {
          return val
        }
      });
      var ticketCat = that.props.ticketCategoryType.ticketCategoryTypes.filter(function (val) {
        if (val.id == item.ticketCategoryTypeId) {
          return val
        }
      });
      isReturn = isReturn || ticketCat[0].name.toLowerCase().indexOf(value) > -1 || ticketSubCat[0].name.toLowerCase().indexOf(value) > -1;
      if (isReturn) {
        return item;
      }
    });
    this.updateSummary(data);
    this.setState({ isFilterShow: true });
  }
  SelectFeeCategory = (index, categoryName) => {
    this.setState({ [index + "feeCategoryName"]: categoryName })
  }
  onChangeFeeType = (index, feeType) => {
    this.setState({ [index + "FeeType"]: feeType })
  }
  onChangeValueType = (index, valueType) => {
    this.setState({ [index + "ValueType"]: valueType })
  }
  onFeeChangeValue = (index, e) => {
    this.setState({ [index + "FeeValue"]: e.target.value })
  }
  public render() {
    var that = this;
    if (this.props.allInventoryStore.isShowSuccessAlert == true && !this.state.isAlertCall) {
      // this.notify();
      // this.setState({ isAlertCall: true });
      //alert("Your inventory information has been saved successfully!");
    }
    var index = 0;
    var deliveryOptionIndex = 0;
    var customerIdType = [], addedCategoryOptions = [];
    var DeliveryOption = [];
    var CustomerInformation = [];
    if (index == 0) {
      var data = {
        value: index,
        label: "Add New"
      };
      customerIdType.push(data);
      index = index + 1;
    }
    this.props.customerInformationControls.customerInformationControls.map(function (item, index) {
      var data = {
        value: index,
        label: item.name
      };
      CustomerInformation.push(data);
    })
    if (this.props.ticketCategoryType && this.props.ticketCategoryType.ticketCategoryTypes) {
      var ticketCategoryTypes = this.props.ticketCategoryType.ticketCategoryTypes.map(function (item, index) {
        var isChecked = false;
        if (item.id == that.state.currentTicketType && that.state.isTicketCategoryShow) {
          isChecked = true;
        }
        return <span className="btn btn-outline-primary">
          <div className="custom-control custom-checkbox">
            <input type="checkbox" checked={isChecked} id={"customCheck" + index} onChange={that.onTicketTypeClick.bind(that, item)} name={item.name} value={item.name} className="custom-control-input" />
            <label className="custom-control-label" htmlFor={"customCheck" + index}>{item.name}</label>
          </div>
        </span>
      });

      var regularTicketCategoryTypes = this.props.ticketCategoryType.ticketCategorySubTypes.map(function (item, index) {
        var isChecked = false;
        if (item.id == that.state.selectedRegularTicketTypeId) {
          isChecked = true;
        }
        if (item.ticketCategoryTypeId == 1) {
          return <span className="btn btn-outline-primary">
            <div className="custom-control custom-checkbox">
              <input type="checkbox" checked={isChecked} id={"regular" + index} onChange={that.onRegularTicketTypeClick.bind(that, item)} name={item.name} value={item.name} className="custom-control-input" />
              <label className="custom-control-label" htmlFor={"regular" + index}>{item.name}</label>
            </div>
          </span>
        }
      });

      var addOnsTicketCategoryTypes = this.props.ticketCategoryType.ticketCategorySubTypes.map(function (item, index) {
        var isChecked = false;
        if (item.id == that.state.selectedAddOnsTicketTypeId) {
          isChecked = true;
        }
        if (item.ticketCategoryTypeId == 2) {
          return <span className="btn btn-outline-primary">
            <div className="custom-control custom-checkbox">
              <input type="checkbox" checked={isChecked} id={"add" + index} onChange={that.onAddOnsTicketTypeClick.bind(that, item)} name={item.name} value={item.name} className="custom-control-input" />
              <label className="custom-control-label" htmlFor={"add" + index}>{item.name}</label>
            </div>
          </span>
        }
      });
    }
    if (this.props.documentType && this.props.documentType.documentTypes)
      this.props.documentType.documentTypes.map(function (item, currentIndex) {
        var isSelectedByCustomer = false;
        if (that.props.isEdit && that.state.selectedCustomerIdTypes != null) {
          var isExist = that.state.selectedCustomerIdTypes.filter(function (val) {
            return val.label == item.documentType
          });
          if (isExist.length > 0) {
            isSelectedByCustomer = true;
          }
        }
        if (!isSelectedByCustomer) {
          var data = {
            value: currentIndex,
            label: item.documentType
          };
          customerIdType.push(data);
        }
      });

    this.props.deliveryType.deliveryTypes.map(function (item, currentIndex) {
      var isSelectedByCustomer = false;
      if (that.props.isEdit && that.state.selectedDeliveryOptionTypes != null) {
        var isExist = that.state.selectedDeliveryOptionTypes.filter(function (val) {
          return val.label == item
        });
        if (isExist.length > 0) {
          isSelectedByCustomer = true;
        }
      }
      if (!isSelectedByCustomer) {
        var data = {
          value: currentIndex,
          label: item
        };
        DeliveryOption.push(data);
        deliveryOptionIndex = deliveryOptionIndex + 1;
      }
    });
    if (this.props.currencyType.currencyTypes)
      var currencies = this.props.currencyType.currencyTypes.map(function (item) {
        return <option>{item.code}</option>
      });
    if (this.props.deliveryType.deliveryTypes)
      var deliveryTypes = this.props.deliveryType.deliveryTypes.map(function (item) {
        return <option >{item}</option>
      });

    var daysArray = [];
    var monthArray = [];
    for (i = 1; i <= 30; i++) {
      daysArray.push(i);
    }
    for (i = 1; i <= 12; i++) {
      monthArray.push(i);
    }
    if (this.props.ticketCategoryType)
      if (this.props.ticketCategoryType.ticketCategoryTypes && this.props.ticketCategoryType.ticketCategoryTypes.length > 0 && !this.state.isTicketCategoryTypeIdUpdated) {
        this.updatedTicketCategoryTypeIdState();
      }

    var validityDays = daysArray.map(function (item) {
      return <option>{item}</option>
    });

    var validityMonth = monthArray.map(function (item) {
      return <option>{item}</option>
    })

    var inventoryDetailAccordianEditComponent = <div></div>;
    if (this.props.disabled === true) {
      inventoryDetailAccordianEditComponent = <i className="fa fa-edit pull-right" onClick={() => this.props.handleEditButtonPressedInventory()} aria-hidden="true"></i>
    }
    if (this.props.disabled === false) {
      inventoryDetailAccordianEditComponent = <span className=" pull-right"><i className="fa fa-check" onClick={() => this.props.handleEditSaveButtonPressedInventory()} style={{ color: "green", marginRight: 8 }}  ></i><i className="fa fa-times " onClick={() => this.props.handleEditRevertButtonPressedInventory()} style={{ color: "red" }} data-icon="&#x25a8;"></i></span>
    }

    if (this.props.allInventoryStore.isInventoryDataSuccess > 0 && !this.state.isStateUpdated && this.props.isEdit) {
      this.updatedTicketCategoryState();
    }
    var i = 0;
    var ticketCategories;
    if (this.props.isEdit && this.state.isStateUpdated) {
      var currentTicketCategoryData = this.state.ticketCategory.filter(function (item) {
        return item.ticketSubCategoryTypeId == that.state.ticketSubCategoryTypeId
      });
      ticketCategories = currentTicketCategoryData.map(function (item) {
        i = i + 1;
        var price = item.pricePerTicket;
        var currency = that.props.currencyType.currencyTypes.filter(function (val) { return val.id == item.currencyId });
        return <div>
          <div className="mb-0">
            <div className="card">
              <div className="card-header p-0" id={"heading" + item.id} >
                <p className="mb-0 pr-3">
                  <button className="btn btn-link" type="button" data-toggle="collapse" data-target={"#collapseOne" + item.id} aria-expanded="true" aria-controls="collapseOne">
                    {item.categoryName == "" ? ("Category " + i) : (item.categoryName)}
                  </button>
                  {(that.state.ticketCategory.length > 1) && <a href="JavaScript:Void(0)" onClick={that.onRemoveCategory.bind(that, item.id)} className="text-decoration-none btn-link text-danger float-right mt-2"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>}
                  {/*<div className="col-12"><h5>Category {i}</h5>{(that.state.ticketCategory.length > 1) && <a href="JavaScript:Void(0)" onClick={that.onRemoveCategory.bind(that, i - 1)} className="text-decoration-none btn-link text-danger float-right"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>}</div>*/}
                </p>
              </div>

              <div id={"collapseOne" + item.id} className="collapse" aria-labelledby={"heading" + item.id} data-parent="#accordionExample">
                <div className="card-body">
                  <div className="row">
                    <div className="col-sm-6 mb-2 pr-1">
                      <div className="form-group">
                        <label>Name</label>
                        <div className="input-group">
                          <input disabled={(that.state["category" + item.id] == undefined || that.state["category" + item.id] == false) ? true : false} id={"ticketCat" + i} defaultValue={item.categoryName} value={item.categoryName} type="text" className="form-control" onChange={that.onValueChange.bind(that, item.id, "ticketCatName")} placeholder={(that.state.inventoryType == "F&B" || that.state.inventoryType == "Souvenirs") ? "Item Name" : "Category Name"} required />
                          {((that.state["category" + item.id] == undefined || that.state["category" + item.id] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "category" + item.id, true)}>
                            <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                          </div>}
                          {((that.state["category" + item.id] == true) ? true : false) && <div className="input-group-append border-success" onClick={() => that.enableField(that, "category" + item.id, false)}>
                            <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                          </div>}
                        </div>
                      </div>
                    </div>

                    <div className="col-sm-6 mb-2 pr-1">
                      <div className="form-group">
                        <label>Quantity</label>
                        <div className="input-group">
                          <input disabled={(that.state["quantity" + item.id] == undefined || that.state["quantity" + item.id] == false) ? true : false} type="text" defaultValue={item.quantity} value={item.quantity} className="form-control" onChange={that.onValueChange.bind(that, item.id, "quantity")} placeholder="Quantity available" required />
                          {((that.state["quantity" + item.id] == undefined || that.state["quantity" + item.id] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "quantity" + item.id, true)}>
                            <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                          </div>}
                          {((that.state["quantity" + item.id] == true) ? true : false) && <div className="input-group-append border-success" onClick={() => that.enableField(that, "quantity" + item.id, false)}>
                            <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                          </div>}
                        </div>
                      </div>
                    </div>

                    <div className="col-sm-12 mb-2">
                      {(that.state.inventoryType == "F&B" || that.state.inventoryType == "Souvenirs") ? <label>Item Description</label> : <label>Ticket Description</label>}
                      <div className="form-group">
                        <div className="input-group">
                          {((that.state["ticketCategoryDescription" + item.id] == undefined || that.state["ticketCategoryDescription" + item.id] == false) ? true : false) && <div className="form-control h-auto" dangerouslySetInnerHTML={{ __html: item.ticketCategoryDescription }} />}
                          {((that.state["ticketCategoryDescription" + item.id] == undefined || that.state["ticketCategoryDescription" + item.id] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "ticketCategoryDescription" + item.id, true)}>
                            <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                          </div>}

                          {((that.state["ticketCategoryDescription" + item.id] == true) ? true : false) && <div className="input-group-append border-success editor-check" onClick={() => that.enableField(that, "ticketCategoryDescription" + item.id, false)}>
                            <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                          </div>}
                        </div>
                      </div>
                      {((that.state["ticketCategoryDescription" + item.id] == true) ? true : false) && <CKEditor
                        isReadOnly={true}
                        activeClass="p10"
                        content={item.ticketCategoryDescription}
                        events={{
                          "change": that.onValueChange.bind(that, item.id, "discription"),

                        }}
                      />}
                    </div>

                    <div className="col-sm-12 mb-2">
                      {(that.state.inventoryType == "F&B" || that.state.inventoryType == "Souvenirs") ? <label>Item Note</label> : <label>Ticket Category Note</label>}
                      <div className="form-group">
                        <div className="input-group">
                          {((that.state["ticketCategoryNote" + item.id] == undefined || that.state["ticketCategoryNote" + item.id] == false) ? true : false) && <div className="form-control h-auto" dangerouslySetInnerHTML={{ __html: item.ticketCategoryNote }} />}
                          {((that.state["ticketCategoryNote" + item.id] == undefined || that.state["ticketCategoryNote" + item.id] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "ticketCategoryNote" + item.id, true)}>
                            <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                          </div>}

                          {((that.state["ticketCategoryNote" + item.id] == true) ? true : false) && <div className="input-group-append border-success editor-check" onClick={() => that.enableField(that, "ticketCategoryNote" + item.id, false)}>
                            <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                          </div>}
                        </div>
                      </div>
                      {((that.state["ticketCategoryNote" + item.id] == true) ? true : false) && <CKEditor
                        isReadOnly={true}
                        activeClass="p10"
                        content={item.ticketCategoryNote}
                        events={{
                          "change": that.onValueChange.bind(that, item.id, "note"),

                        }}
                      />}
                    </div>

                    <div className="col-sm-6 mb-2 pr-1">
                      <div className="form-group">
                        <label>Currency</label>
                        <div className="input-group">
                          <select
                            className="form-control"
                            onChange={that.onValueChange.bind(that, item.id, "currency")}
                            required>
                            {(currency.length > 0) && < option > {currency[0].code}</option>}
                            {currencies}
                          </select>
                        </div>
                      </div>
                    </div>

                    {(that.state.inventoryType != "free") &&
                      <div className="col-sm-6 pl-1">
                        <div className="form-group">
                          <label>Price</label>
                          <div className="input-group">
                            <input disabled={(that.state["pricePerTicket" + item.id] == undefined || that.state["pricePerTicket" + item.id] == false) ? true : false} type="number" defaultValue={price} value={price} className="form-control" onChange={that.onValueChange.bind(that, item.id, "pricePerTicket")} placeholder={(that.state.inventoryType == "F&B" || that.state.inventoryType == "Souvenirs") ? "Price per item" : "Price per category"} required />
                            {((that.state["pricePerTicket" + item.id] == undefined || that.state["pricePerTicket" + item.id] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "pricePerTicket" + item.id, true)}>
                              <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                            </div>}
                            {((that.state["pricePerTicket" + item.id] == true) ? true : false) && <div className="input-group-append border-success" onClick={() => that.enableField(that, "pricePerTicket" + item.id, false)}>
                              <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                            </div>}
                          </div>
                        </div>
                      </div>}
                    <div className="col-sm-12">
                      <label>Select Ticket Validity Type</label>
                      <form>
                        <div className="radio custom-control custom-radio custom-control-inline pl-0">
                          <label>
                            <input className="mr-2" type="radio" value="Rolling"
                              checked={item.isRollingTicketValidityType}
                              onChange={that.onValueChange.bind(that, item.id, "rolling")} />
                                                        Rolling
                                <i data-tip="React-tooltips1" data-for="Rolling" className="fa fa-info-circle text-primary ml-2" />
                            <ReactTooltip place="bottom" id="Rolling" type="info" effect="float">
                              <span style={{ maxWidth: "230px", display: "block" }}>Ticket validity start from the day ticket purchase.</span>
                            </ReactTooltip>
                          </label>
                        </div>
                        <div className="radio custom-control custom-radio custom-control-inline">
                          <label>
                            <input className="mr-2" type="radio" value="Fixed"
                              checked={!item.isRollingTicketValidityType}
                              onChange={that.onValueChange.bind(that, item.id, "fix")} />
                                                        Fixed
                                    <i data-tip="React-tooltips2" data-for="Fixed" className="fa fa-info-circle text-primary ml-2" />
                            <ReactTooltip place="bottom" id="Fixed" type="info" effect="float">
                              <span style={{ maxWidth: "230px", display: "block" }}>This is fixed date till ticket can be valid.</span>
                            </ReactTooltip>
                          </label>
                        </div>
                      </form>
                    </div>
                    {(!item.isRollingTicketValidityType) && <div className="col-sm-12 relative-cal">
                      <label>Select Ticket Validity Date </label>
                      <Datetime inputProps={{ placeholder: 'Select ticket validity date', disabled: false }} value={item.ticketValidityFixDate} onChange={that.onValueChange.bind(that, item.id, "ticketValidityFixDate")} dateFormat="YYYY-MM-DD" timeFormat={true} />
                    </div>}
                    {(item.isRollingTicketValidityType) && <div className="col-sm-12">
                      <label>Select Ticket Validity Period</label>
                      <div className="row mb-0">
                        <div className="col-sm-4">
                          <label>Years</label>
                          <select className="form-control" onChange={that.onValueChange.bind(that, item.id, "year")}>
                            <option selected>{item.days}</option>
                            <option>--select--</option>
                            <option>1</option>
                            <option>2</option>
                            <option>3</option>
                            <option>4</option>
                          </select>
                        </div>
                        <div className="col-sm-4">
                          <label>Months</label>
                          <select className="form-control" onChange={that.onValueChange.bind(that, item.id, "month")}>
                            <option selected>{item.month}</option>
                            <option>--select--</option>
                            {validityMonth}
                          </select>
                        </div>
                        <div className="col-sm-4">
                          <label>Days</label>
                          <select className="form-control" onChange={that.onValueChange.bind(that, item.id, "days")}>
                            <option selected>{item.year}</option>
                            <option>--select--</option>
                            {validityDays}
                          </select>
                        </div>
                      </div>
                    </div>}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      });
    } else if (!this.props.isEdit) {
      var currentTicketCategoryData = this.state.ticketCategory.filter(function (item) {
        return item.ticketSubCategoryTypeId == that.state.ticketSubCategoryTypeId
      });
      ticketCategories = currentTicketCategoryData.map(function (item, index) {
        i = i + 1;
        return <div>
          <div className="card">
            <div className="card-header p-0" id={"headingopen" + item.id} >
              <p className="mb-0 pr-3">
                <button className="btn btn-link" type="button" data-toggle="collapse" data-target={"#collapseTwo" + item.id} aria-expanded="true" aria-controls="collapseOne">
                  {item.categoryName == "" ? ("Category " + i) : (item.categoryName)}
                </button>
                <a href="JavaScript:Void(0)" onClick={that.onRemoveCategory.bind(that, item.id)} className="text-decoration-none btn-link text-danger float-right mt-2"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>
                {/*<div className="col-12"><h5>Category {i}</h5>{(that.state.ticketCategory.length > 1) && <a href="JavaScript:Void(0)" onClick={that.onRemoveCategory.bind(that, i - 1)} className="text-decoration-none btn-link text-danger float-right"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>}</div>*/}
              </p>
            </div>
            <div id={"collapseTwo" + item.id} className="collapse" aria-labelledby={"headingopen" + item.id} data-parent="#accordionExample">
              <div className="card-body">
                <div className="form-group row mb-0">

                  <div className="col-sm-6 mb-2 pr-1">
                    <label>Name</label>
                    <input defaultValue={item.categoryName} type="text" className="form-control" value={item.categoryName} onChange={that.onValueChange.bind(that, item.id, "ticketCatName")} placeholder={(that.state.inventoryType == "F&B" || that.state.inventoryType == "Souvenirs") ? "Item Name" : "Category Name"} required />
                  </div>

                  <div className="col-sm-6 mb-2 pl-1">
                    <label>Quantity</label>
                    <input defaultValue={item.quantity} type="number" className="form-control" value={item.quantity} onChange={that.onValueChange.bind(that, item.id, "quantity")} placeholder="Quantity available" required />
                  </div>
                  <div className="col-sm-12 mb-2">
                    {(that.state.inventoryType == "F&B" || that.state.inventoryType == "Souvenirs") ? <label>Item Description</label> : <label>Ticket Description</label>}
                    <CKEditor
                      isReadOnly={true}
                      activeClass="p10"
                      content={item.ticketCategoryDescription}
                      events={{
                        "change": that.onValueChange.bind(that, item.id, "discription"),

                      }}
                    />

                  </div>
                  <div className="col-sm-12 mb-2">
                    {(that.state.inventoryType == "F&B" || that.state.inventoryType == "Souvenirs") ? <label>Item Note</label> : <label>Ticket Category Note</label>}
                    <CKEditor
                      isReadOnly={true}
                      activeClass="p10"
                      content={item.ticketCategoryNote}
                      events={{
                        "change": that.onValueChange.bind(that, item.id, "note")
                      }}
                    />
                  </div>

                  <div className="col-sm-6 mb-2 pr-1">
                    <label>Currency</label>
                    <select className="form-control" onChange={that.onValueChange.bind(that, item.id, "currency")} required>
                      <option>Please select a currency</option>
                      {currencies}
                    </select>
                  </div>

                  {(that.state.inventoryType != "free") && < div className="col-sm-6 pl-1">
                    <label>Price</label>
                    <input type="number" defaultValue={item.pricePerTicket} className="form-control" value={item.pricePerTicket} onChange={that.onValueChange.bind(that, item.id, "pricePerTicket")} placeholder={(that.state.inventoryType == "F&B" || that.state.inventoryType == "Souvenirs") ? "Price per item" : "Price per category"} required />
                  </div>}
                  {(that.state.inventoryType == "free") && < div className="col-sm-6 pl-1">
                    <label>Price</label>
                    <input type="text" defaultValue={item.pricePerTicket} className="form-control" value={item.pricePerTicket} onChange={that.onValueChange.bind(that, item.id, "pricePerTicket")} placeholder="Free" readOnly={true} />
                  </div>}
                  <div className="col-sm-12">
                    <label>Select Ticket Validity Type</label>
                    <form>
                      <div className="radio custom-control custom-radio custom-control-inline pl-0">
                        <label>
                          <input className="mr-2" type="radio" value="Rolling"
                            checked={item.isRollingTicketValidityType}
                            onChange={that.onValueChange.bind(that, item.id, "rolling")} />
                                                    Rolling
                                <i data-tip="React-tooltips1" data-for="Rolling" className="fa fa-info-circle text-primary ml-2" />
                          <ReactTooltip place="bottom" id="Rolling" type="info" effect="float">
                            <span style={{ maxWidth: "230px", display: "block" }}>Ticket validity start from the day ticket purchase.</span>
                          </ReactTooltip>
                        </label>
                      </div>
                      <div className="radio custom-control custom-radio custom-control-inline">
                        <label>
                          <input className="mr-2" type="radio" value="Fixed"
                            checked={!item.isRollingTicketValidityType}
                            onChange={that.onValueChange.bind(that, item.id, "fix")} />
                                                    Fixed
                                    <i data-tip="React-tooltips2" data-for="Fixed" className="fa fa-info-circle text-primary ml-2" />
                          <ReactTooltip place="bottom" id="Fixed" type="info" effect="float">
                            <span style={{ maxWidth: "230px", display: "block" }}>This is fixed date till ticket can be valid.</span>
                          </ReactTooltip>
                        </label>
                      </div>
                    </form>
                  </div>
                  {(!item.isRollingTicketValidityType) && <div className="col-sm-12">
                    <label>Select Ticket Validity Date </label>
                    <Datetime inputProps={{ placeholder: 'Select ticket validity date', disabled: false }} value={item.ticketValidityFixDate} onChange={that.onValueChange.bind(that, item.id, "ticketValidityFixDate")} dateFormat="YYYY-MM-DD" timeFormat={true} />
                  </div>}
                  {(item.isRollingTicketValidityType) && <div className="col-sm-12">
                    <label>Select Ticket Validity Period</label>
                    <div className="row mb-0">
                      <div className="col-sm-4">
                        <label>Years</label>
                        <select className="form-control" onChange={that.onValueChange.bind(that, item.id, "year")}>
                          <option>0</option>
                          <option>1</option>
                          <option>2</option>
                          <option>3</option>
                          <option>4</option>
                        </select>
                      </div>
                      <div className="col-sm-4">

                        <label>Months</label>
                        <select className="form-control" onChange={that.onValueChange.bind(that, item.id, "month")}>
                          <option>0</option>
                          {validityMonth}
                        </select>
                      </div>
                      <div className="col-sm-4">
                        <label>Days</label>
                        <select className="form-control" onChange={that.onValueChange.bind(that, item.id, "days")}>
                          <option>0</option>
                          {validityDays}
                        </select>
                      </div>
                    </div>
                  </div>}
                </div>
              </div>
            </div>
          </div>
          {(that.state.ticketCategory.length != i) && <div className="line"></div>}
        </div>
      });
    }
    if (currentTicketCategoryData && currentTicketCategoryData.length > 0) {
      currentTicketCategoryData.map((item, index) => {
        if (index == 0) {
          let tempData = {
            value: "All",
            label: "All"
          }
          addedCategoryOptions.push(tempData);
        }
        let tempData = {
          value: item.categoryName,
          label: item.categoryName
        }
        addedCategoryOptions.push(tempData);
      })
    }
    // for Fee Types
    var feeTypes = this.state.feeTypesList.map((item, index) => {
      return <div className="form-row">
        <div className="form-group col-sm-3">
          <label>Select Ticket Category</label>
          < Select
            isMulti
            options={addedCategoryOptions}
            onChange={that.SelectFeeCategory.bind(that, index)}
            value={that.state[index + "feeCategoryName"]}
          />
        </div>
        <div className="form-group col-sm-3">
          <label>Select Fee Type</label>
          < Select
            isMulti
            options={feeTypeOptions}
            onChange={that.onChangeFeeType.bind(that, index)}
            value={that.state[index + "FeeType"]}
          />
        </div>
        <div className="form-group col-sm-3">
          <label>Select Fee Value Type</label>
          < Select
            options={valueTypeOptions}
            onChange={that.onChangeValueType.bind(that, index)}
            value={that.state[index + "ValueType"]}
          />
        </div>
        <div className="form-group col-sm-3">
          <label>Value</label>
          <input type="text" className="form-control" value={that.state[index + "FeeValue"]} onChange={that.onFeeChangeValue.bind(that, index)} />
        </div>
      </div>
    })
    i = 0;
    var refundPolicy = this.props.refundPolicies.refundPolicies.map(function (item) {
      i = i + 1;
      return <div className="custom-control custom-radio" onClick={that.onRefundPolicyClick.bind(that, item.id)}>
        <input type="radio" id={"customRadio1" + i} checked={that.state.checkedRefundPolicyId == i ? true : false} name="customRadio" className="custom-control-input" />
        <label className="custom-control-label" htmlFor={"customRadio" + i}> <small>{item.policy}</small> </label>
      </div>
    });

    var isFilterShow = this.state.ticketCategory.filter(function (item) {
      if (item.categoryName !== "") {
        return item;
      }
    });

    return <div className="col-sm-12">
      {(this.props.allInventoryStore.isShowSuccessAlert) && <ToastContainer
        position="top-center"
        autoClose={6000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />}
      {
        (!this.props.allInventoryStore.isInventorySaveRequest) && <div>
          <form onSubmit={this.onSubmitInventory}>

            <a className="place-listing" data-toggle="collapse" href="#PlaceInventory" role="button" aria-expanded="false" aria-controls="PlaceInventory">
              <span className="rounded-circle border border-primary place-listing">2</span>Inventory Details</a>
            <div className="collapse multi-collapse pt-3" id="PlaceInventory">
              <div className="form-group mt-4">
                <label className="d-block">Select Inventory Classification: <span className="text-danger">*</span> </label>
                <div className="btn-group" role="group" aria-label="Basic example">
                  {ticketCategoryTypes}
                </div>
              </div>
              {(this.state.isRegularTicketTypeShow) && <div className="form-group mt-4 text-nowrap">
                <label className="d-block">Select Inventory Type: <span className="text-danger">*</span> </label>
                <div className="btn-group d-block overflow-auto position" role="group" aria-label="Basic example">
                  {regularTicketCategoryTypes}
                </div>
              </div>}

              {(this.state.isAddOnsTicketTypeShow) && <div className="form-group mt-4 text-nowrap">
                <label className="d-block">Select Inventory Type: <span className="text-danger">*</span> </label>
                <div className="btn-group" role="group" aria-label="Basic example">
                  {addOnsTicketCategoryTypes}
                </div>
              </div>}

              <div className="form-group mt-4">
                {(summary != undefined && this.state.isTicketCategoryShow) && <h6 className="mt-0">Summary</h6>}
                {(isFilterShow.length > 0) &&
                  <div className="input-group col-sm-6 p-0 mb-2">
                    <div className="input-group-prepend">
                      <span className="input-group-text">
                        <i className="fa fa-filter" aria-hidden="true"></i>
                      </span>
                    </div>
                    <input type="text" aria-label="Filter summary" placeholder="Filter summary" onChange={that.onChangeSummaryFilter.bind(that)} className="form-control" />
                  </div>}
                {(summary != undefined && this.state.isTicketCategoryShow) && <div className="table-responsive"><table className="table table-bordered table-condensed m-0">
                  <thead>
                    <tr>
                      <th className="text-center">Inventory Classification</th>
                      <th className="text-center">Inventory/Item Type</th>
                      <th className="text-center">Category/Item Name</th>
                      <th className="text-center">Category/Item Quantity</th>
                      <th className="text-center">Category/Item Price</th>
                      <th className="text-center">Remove</th>
                    </tr>
                  </thead>
                  <tbody>
                    {summary}
                  </tbody>
                </table></div>}
              </div>

              {(this.state.isTicketCategoryShow) && < div >
                <div className="accordion border-bottom" id="accordionExample">
                  {ticketCategories}
                </div>
                <div className="form-group my-2">
                  <a href="JavaScript:Void(0)" onClick={this.onClickAddTicketCategories} className="btn btn-sm btn-outline-primary"> <small><i className="fa fa-plus mr-2" aria-hidden="true"></i> {(this.state.ticketCategory.length == 1) ? "Add Category" : "Add Category"} </small> </a>
                </div>
                <div className="line"></div>
                {(!this.props.isEdit) && < div className="form-group" ref={this.myRef}>
                  <label>Terms & Conditions</label>
                  <CKEditor
                    activeClass="p10"
                    content={this.state.ticketCategoryTermsAndCondition}
                    events={{
                      "change": this.onChangeTicketCategoryTermsAndCondition
                    }}
                    required
                  />
                </div>}

                {(this.props.isEdit) && <div className="form-group">
                  <label>Terms & Conditions</label>
                  <div className="form-group">
                    <div className="input-group">
                      {((that.state["termsAndCondition"] == undefined || that.state["termsAndCondition"] == false) ? true : false) && <div className="form-control" dangerouslySetInnerHTML={{ __html: this.state.ticketCategoryTermsAndCondition }} />}
                      {((that.state["termsAndCondition"] == undefined || that.state["termsAndCondition"] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "termsAndCondition", true)}>
                        <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                      </div>}

                      {((that.state["termsAndCondition"] == true) ? true : false) && <div className="input-group-append border-success editor-check" onClick={() => that.enableField(that, "termsAndCondition", false)}>
                        <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                      </div>}
                    </div>
                  </div>
                  {((that.state["termsAndCondition"] == true) ? true : false) && <CKEditor
                    isReadOnly={true}
                    activeClass="p10"
                    content={this.state.ticketCategoryTermsAndCondition}
                    events={{
                      "change": this.onChangeTicketCategoryTermsAndCondition

                    }}
                  />}
                </div>}

                {(this.state.ticketCategoryTermsAndCondition == "" && this.state.errorMsgTandC != undefined) && < div className="text-danger">
                  {this.state.errorMsgTandC}
                </div>}

                <div className="form-group">
                  <label>Fulfillment Format
                                <i data-tip="React-tooltips3" data-for="Fulfillment" className="fa fa-info-circle text-primary ml-2" />
                    <ReactTooltip place="bottom" id="Fulfillment" type="info" effect="float">
                      <span style={{ maxWidth: "230px", display: "block" }}>Please select delivery option (Need to add pickup instruction and address for venue pickup)</span>
                    </ReactTooltip>
                  </label>
                  < Select
                    isMulti
                    options={DeliveryOption}
                    onChange={this.onChangeDeliveryOptionType}
                    value={this.state.selectedDeliveryOptionTypes}
                  />
                </div>
                <div className="form-group">
                  <label>Fees and Taxes </label>
                  {feeTypes}
                </div>
                <div className="form-group mb-2">
                  <a href="JavaScript:Void(0)" onClick={this.onClickAddFeeType} className="btn btn-sm btn-outline-primary"> <small><i className="fa fa-plus mr-2" aria-hidden="true"></i> Add Fee Type </small> </a>
                  {this.state.feeTypesList.length > 0 && <a href="JavaScript:Void(0)" onClick={this.removeAddFeeType} className="text-decoration-none btn-link text-danger float-right mt-2"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove Fee </small> </a>}
                </div>
                {(this.state.selectedDeliveryOptionTypes == null && this.state.errorMsgDeliveryOption != undefined) && < div className="text-danger">
                  {this.state.errorMsgDeliveryOption}
                </div>}
                <div className="form-group">
                  <label>Select Customer Information</label>
                  < Select
                    isMulti
                    options={CustomerInformation}
                    onChange={this.onChangeCustomerInformations}
                    value={this.state.selectedCustomerInformations}
                  />
                </div>
                {(this.state.isCustomerIdTypeSelect) && <div className="form-group">
                  {(!this.state.isAddNewCustomerIdType) && <label >Customer ID Type</label>}
                  {(!this.state.isAddNewCustomerIdType) && < Select
                    isMulti
                    options={customerIdType}
                    onChange={this.onChangeCustomerIdType}
                    value={this.state.selectedCustomerIdTypes}

                  />}
                  {(this.state.isAddNewCustomerIdType) &&
                    <div>
                      <label>Create Customer Id Type</label>
                      <div className="input-group">
                        <input disabled={this.props.disabled} type="text" value={this.state.newCustomerIdType} onChange={this.onChangeAddNewCustomerIdType.bind(this)} className="form-control" placeholder="Create new customer Id type" aria-label="Search term" aria-describedby="basic-addon" />
                        <div className="input-group-append">

                          <a href="JavaScript:Void(0)" onClick={this.onClickSaveNewSelectCustomerType} className="btn btn-secondary">Save</a>
                        </div>
                      </div>
                      <a href="JavaScript:Void(0)" onClick={this.onClickSelectCustomerType} className="text-decoration-none btn-link"> <small><i className="fa fa-refresh mr-2" aria-hidden="true"></i>Select default customer Id Type</small> </a>
                    </div>
                  }
                </div>}
                {(this.state.selectedCustomerIdTypes == null && this.state.errorMsgCustomerId != undefined) && < div className="text-danger">
                  {this.state.errorMsgCustomerId}
                </div>}
                <div className="form-group">
                  <label className="d-block">Customer ID Number</label>
                  <div className="custom-control custom-radio custom-control-inline">
                    <input type="radio" id="custom1" checked={this.state.isCustomerIdAllowed} onClick={this.onCustomerIdClick.bind(this, "allow")} name="custom1" className="custom-control-input" />
                    <label className="custom-control-label" htmlFor="custom1">Yes</label>
                  </div>
                  <div className="custom-control custom-radio custom-control-inline">
                    <input type="radio" id="custom2" checked={this.state.isCustomerIdAllowedNo} onClick={this.onCustomerIdClick.bind(this, "notAllow")} name="custom2" className="custom-control-input" />
                    <label className="custom-control-label" htmlFor="custom2">No</label>
                  </div>
                </div>
                {(this.state.isVenuePickup) && <div className="form-group">
                  <label>Redemptions/Instructions</label>
                  <textarea className="form-control" rows={3} value={this.state.redemptionInstruction} onChange={this.onChangeRedemptionInstruction.bind(this)} placeholder="Note here..." ></textarea>
                </div>}
                {(this.state.redemptionInstruction == "" && this.state.isVenuePickup && this.state.errorMsgRedemption != undefined) && < div className="text-danger">
                  {this.state.errorMsgRedemption}
                </div>}

                {(this.state.isVenuePickup) && <div className="form-group">
                  <label>Redemption Address</label>
                  <PlaceAutocomplete autoCompletePlace={this.autoCompleteAddress} onCustomFieldChange={this.onCustomFieldChange} />
                </div>}

                {(this.state.isVenuePickup) && <div className="form-group">
                  <label>Redemption Dates & Timings</label>
                  <Datetime inputProps={{ placeholder: 'Select place start date & time', disabled: false }} onChange={this.OnChangeRedemptionDatetime} value={this.state.redemptionStartDatetime} dateFormat="YYYY-MM-DD" timeFormat={true} />
                </div>}

                <div className="form-froup">
                  <label className="d-block">Refund Policy</label>
                  {refundPolicy}
                </div>
                <div className="text-center pb-4">
                  <a href="JavaScript:Void(0)" className="text-decoration-none mr-4">Cancel</a>
                  <button type="submit" className="btn btn-outline-primary">{(this.props.isEdit) ? "Update" : "Submit"}</button>
                </div>
              </div>}
            </div>
          </form>
        </div>}
      {(this.props.allInventoryStore.isInventorySaveRequest) && <Newloader />}
    </div>
  }

  @autobind
  public onSubmitInventory(e) {
    e.preventDefault();
    /* if (this.state.ticketCategoryTermsAndCondition == "") {
         this.setState({ errorMsgTandC: "Please enter terms and condition" });
     } else if (this.state.selectedDeliveryOptionTypes == null) {
         this.setState({ errorMsgDeliveryOption: "Please select Fulfillment Format" });
     } else if (this.state.selectedCustomerIdTypes == null) {
         this.setState({ errorMsgCustomerId: "Please select Customer ID Type" });
     } else if (this.state.redemptionInstruction == "" && this.state.isVenuePickup) {
         this.setState({ errorMsgRedemption: "Please enter Redemptions/Instructions" });
     } */
    if (false) { }
    else {
      var that = this;
      var index = -1;
      var ticketCategoryData = [];
      var deliveryOptionIds = [];
      var customerIdTypes = [];
      var customerInformations = [];

      var catname = this.state.ticketCategory.map(function (item) {
        return item.categoryName
      });

      var catlength = catname.filter(function (value, index, self) {
        return self.indexOf(value) === index;
      });

      if (catlength.length == 0) {
        alert("Category name should be unique...");
      } else {
        var data = this.state.ticketCategory.filter(function (item) {
          if (item.categoryName !== "") {
            return item;
          }
        });
        data.map(function (item) {
          var currency = that.props.currencyType.currencyTypes.filter(function (val) {
            return val.id == item.currencyId
          });
          let ticketCat: TicketCategories = {
            categoryName: item.categoryName,
            eventTicketDetailId: item.eventTicketDetailId,
            pricePerTicket: +item.pricePerTicket,
            ticketCategoryId: item.ticketCategoryId,
            isEventTicketAttributeUpdated: item.isEventTicketAttributeUpdated,
            quantity: +item.quantity,
            ticketCategoryDescription: item.ticketCategoryDescription,
            ticketCategoryNote: item.ticketCategoryNote,
            currencyId: currency[0].id,
            isRollingTicketValidityType: item.isRollingTicketValidityType,
            ticketValidityFixDate: item.ticketValidityFixDate,
            days: item.days,
            month: item.month,
            year: item.year,
            ticketCategoryTypeId: item.ticketCategoryTypeId,
            ticketSubCategoryTypeId: item.ticketSubCategoryTypeId
          }
          ticketCategoryData.push(ticketCat);
        });
        /*  if (this.props.placeCalendarResponse.eventDetails.length == 0) {
              alert("Please add place calendar...");
          } */
        var eventDetailAltIds = [];
        if (this.state.selectedDeliveryOptionTypes != null) {
          this.state.selectedDeliveryOptionTypes.map(function (item) {
            deliveryOptionIds.push(item.value);
          });
        }

        if (this.state.selectedCustomerIdTypes != null) {
          this.state.selectedCustomerIdTypes.map(function (item) {
            customerIdTypes.push(item.value);
          });
        }
        if (this.state.selectedCustomerInformations != null) {
          this.state.selectedCustomerInformations.map(function (item) {
            customerInformations.push(item.value);
          });
        }

        var placeAltId = "4A4A848E-B9F0-4B68-8426-48138EEC51CC";
        if (localStorage.getItem("placeAltId") != null) {
          placeAltId = localStorage.getItem("placeAltId");
        } if (this.props.isEdit) {
          placeAltId = this.props.placeAltId;
        }
        let feeTypeDetailsList = [];
        this.state.feeTypesList.map((item, index) => {
          let categoryName = "";
          if (that.state[index + "feeCategoryName"]) {
            that.state[index + "feeCategoryName"].map((val, index) => {
              categoryName = categoryName + val.label;
              if (index < that.state[index + "feeCategoryName"].length - 1) {
                categoryName = categoryName + ",";
              }
            })
          }
          let tempFeeDetail: FeeTypes = {
            feeTypeId: this.state[index + "FeeType"] ? this.state[index + "FeeType"].value + 1 : null,
            valueTypeId: this.state[index + "ValueType"] ? this.state[index + "ValueType"].value + 1 : null,
            value: this.state[index + "FeeValue"] ? this.state[index + "FeeValue"] : null,
            feeType: this.state[index + "FeeType"] ? this.state[index + "FeeType"].label : null,
            categoryName: categoryName,
            eventTicketAttributeId: (item != 0 && item[0].eventTicketAttributeId != undefined && item[0].eventTicketAttributeId != null) ? item[0].eventTicketAttributeId : 0
          }
          feeTypeDetailsList.push(tempFeeDetail)
        })

        let InventoryModel: InventoryRequestViewModel = {
          ticketCategoriesViewModels: ticketCategoryData,
          deliverTypeId: deliveryOptionIds,
          placeAltId: placeAltId,
          redemptionDateTime: (that.state.redemptionStartDatetime != undefined ? that.state.redemptionStartDatetime : null),
          redemptionInstructions: that.state.redemptionInstruction,
          termsAndCondition: that.state.ticketCategoryTermsAndCondition,
          customerIdTypes: customerIdTypes,
          isCustomerIdRequired: that.state.isCustomerIdAllowed,
          eventDetailAltIds: eventDetailAltIds,
          redemptionAddress: (that.state.RedemptionAddress != undefined ? that.state.RedemptionAddress : "test"),
          redemptionCity: (that.state.RedemptionCity != undefined ? that.state.RedemptionCity : "test"),
          redemptionState: (that.state.RedemptionState != undefined ? that.state.RedemptionState : "test"),
          redemptionCountry: (that.state.RedemptionCountry != undefined ? that.state.RedemptionCountry : "test"),
          redemptionZipcode: (that.state.RedemptionZipcode != undefined ? that.state.RedemptionZipcode : "000"),
          refundPolicy: that.state.checkedRefundPolicyId,
          customerInformation: customerInformations,
          isEdit: (that.props.isEdit ? true : false),
          feeTypes: feeTypeDetailsList
        }
        localStorage.setItem("_Inventory_data", JSON.stringify(InventoryModel));
        this.props.onSubmitInventoryData();
      }
    }
  }
}
