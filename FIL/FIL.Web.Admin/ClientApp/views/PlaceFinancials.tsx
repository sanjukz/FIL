import * as React from "react";
import { connect } from "react-redux";
import { Formik, Field, Form } from "formik";
import { autobind } from "core-decorators";
import { IApplicationState } from "../stores";
import { bindActionCreators } from "redux";
import * as placeFinancialsStore from "../stores/PlaceFinancials";
import * as CountryStore from "../stores/Country";
import * as StatesStore from "../stores/States";
import * as SaveFinancialsStore from "../stores/SaveFinancials";
import * as FinanceDetailsStore from "../stores/FinanceDetails";
import "./DatePicker.css";
import Select from "react-select";
import PlacesAutocomplete, {
    geocodeByAddress,
    getLatLng,
} from 'react-places-autocomplete';
import Yup from "yup";
import { StateTypesResponseViewModel } from "../models/Finance/StateTypesResponseViewModel";
import { FinanceResponseViewModel } from "../models/Finance/FinanceResponseViewModel";
import { CUSTOM_REGEX } from "../utils/regexExpression";

interface Values {
    financialsAccountFirstName?: string;
    financialsAccountLastNamed?: string;
    financialsAccountLocationPlaceName?: string;
    financeDetails?: any;
    saveResponse?: any;
}

type PlaceFinancialProps = placeFinancialsStore.ICurrencyTypeProps
    & CountryStore.ICountryTypeProps
    & StatesStore.IStatesTypeProps
    & SaveFinancialsStore.ISaveFinancialsProps
    & typeof placeFinancialsStore.actionCreators
    & FinanceDetailsStore.IFinanceDetailsProps
    & typeof FinanceDetailsStore.actionCreators
    & typeof CountryStore.actionCreators
    & typeof StatesStore.actionCreators
    & typeof SaveFinancialsStore.actionCreators
    & Values

class PlaceFinancials extends React.Component<any, any> {
    fromTime: any;
    toTime: any;

    constructor(props) {
        super(props);
        this.state =
        {
            lat: 20.5937,
            long: 78.9629,
            financeTabAccordianEditComponent: <div></div>,
            isFinancialTabEdit: false,
            popUp: <div></div>,
            isSavedSuccesfully: false,
            isCurrencyEdit: false,
            isCountryEdit: false,
            isStateEdit: false,
            bankAccChecking: true,
            bankAccSaving: false,
            financialsAccountType: 'Individual',
            financialsAccountFirstName: '',
            financialsAccountLastName: '',
            id: '',
            financialsAccountBankInfoType: 'Checking',
            financialsAccountBankInfoTaxInfo: true,
            financialsAccountBankName: '',
            financialsAccountBankState: '',
            financialsAccountBankRoutingNo: '',
            financialsAccountBankGST: '',
            financialsAccountBankAccountNo: '',
            financialsAccountBanREAccountNo: '',
            financialsAccountBankPANNo: '',
            financialsAccountBankNickName: '',
            financialsAccountBankAccountGSTInfo: '',
            accHolderInfoIndiv: true,
            accHolderInfoCompany: false,
            stateOptionforDropDown: [],
            selectedCurrency: "India",
            selectedCountry: '',
            selectedState: '',
            countryId: 1,
            country: '',
            stateId: '',
            currencyId: '',
            BankAccountUpdateLater: true,
            placename: '',
            street: '',
            route: '',
            city: '',
            state: '',
            zip: '',
            ishaveGST: 'SellIn',
            sellInGST: false,
            dontSellInGST: false,
            neverGST: false,
            EventId: '',
            EventDetailId: '',
            altId: 0,
            forEdit: false,
            address: "",
            isShow: false,
           pattern: /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/g
        }

    }


    private getSchema() {
        return Yup.object().shape({
            categoryid: Yup.string(),
            subcategoryid: Yup.string(),
            title: Yup.string(),
            location: Yup.string(),
            placename: Yup.string(),
            address1: Yup.string(),
            address2: Yup.string(),
            city: Yup.string(),
            state: Yup.string(),
            zip: Yup.string(),
            amenityId: Yup.string(),
            description: Yup.string(),
            history: Yup.string(),
            highlights: Yup.string(),
            impressiveexperience: Yup.string(),
            archdetail: Yup.string(),
            tilesSliderImages: Yup.string(),
            descpagebannerImage: Yup.string(),
            inventorypagebannerImage: Yup.string(),
            galleryImages: Yup.string(),
            placemapImages: Yup.string(),
            timelineImages: Yup.string(),
            immersiveexpImages: Yup.string(),
            archdetailImages: Yup.string(),
            metatags: Yup.string(),
            metatitle: Yup.string(),
            metadescription: Yup.string()
        });
    }

    resetLocation = () => {

        this.setState({ placename: '', city: '', state: '', zip: '', country: '', lat: 0.00, long: 0.00, });
    }

    handleEditButtonPressed(editedComponent) {
        if (editedComponent === "Finance") {
            this.setState({
                financeTabAccordianEditComponent: <span className=" pull-right"><i className="fa fa-check" onClick={() => this.handleEditSaveButtonPressed('Finance')} style={{ color: "green", marginRight: 8 }}  ></i><i className="fa fa-times " onClick={() => this.handleEditRevertButtonPressed('Finance')} style={{ color: "red" }} data-icon="&#x25a8;"></i></span>
                , isFinancialTabEdit: false
            });
        }

    }

    handleEditSaveButtonPressed(editedComponent) {
        if (editedComponent === "Finance") {
            document.getElementById('financesaveBtn').click();
            this.setState({
                financeTabAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Finance')} aria-hidden="true"></i>,
                isFinancialTabEdit: true
            });
        }

    }
    handleEditRevertButtonPressed(editedComponent) {
        if (editedComponent === "Finance") {
            this.setState({
                financialTabAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Finance')} aria-hidden="true"></i>,
                isDetailEdit: true
            });
        }

    }
    showSaveButton = true;
    public componentDidMount() {
        if (window) {
            let stripeConnectClientId = (window as any).stripeConnectClientId;
            this.setState({ stripeConnectClientId: stripeConnectClientId, origin: window.location.origin });
        }
        if (localStorage.getItem('placeId') != null && localStorage.getItem('placeId') != '0') {
            this.setState({ EventId: parseInt(localStorage.getItem('placeId'), 0) });
        }
        if (localStorage.getItem('placeAltId') != null && localStorage.getItem('placeAltId') != '0') {
            this.setState({ placeAltId: localStorage.getItem('placeAltId') });
        }
        setTimeout(() => {
            let urlparts = window.location.href.split('/');
            var editId = urlparts[urlparts.length - 1];
            var editIdArray = editId.split("-").length;
            if (editIdArray === 5) {

                this.props.requestFinanceDetailsData(editId, (responseData) => {

                    var response = responseData;
                    if (responseData.id > 0) {
                        if (response) {
                            this.showSaveButton = false;
                            this.setState({
                                forEdit: true,
                                id: response.id,
                                isCurrencyEdit: true,
                                isCountryEdit: true,
                                isStateEdit: true,
                                bankAccChecking: true,
                                bankAccSaving: false,
                                financialsAccountType: 'Individual',
                                financialsAccountFirstName: response.firstName,
                                financialsAccountLastName: response.lastName,
                                financialsAccountBankInfoType: response.financialsAccountBankInfoType,
                                financialsAccountBankInfoTaxInfo: true,
                                financialsAccountBankName: response.bankName,
                                financialsAccountBankState: '',
                                financialsAccountBankRoutingNo: response.routingNo,
                                financialsAccountBankGST: response.gstNo,
                                financialsAccountBankAccountNo: response.accountNo,
                                financialsAccountBanREAccountNo: response.accountNo,
                                financialsAccountBankPANNo: response.panNo,
                                financialsAccountBankNickName: response.bankName,
                                financialsAccountBankAccountGSTInfo: response.financialsAccountBankAccountGSTInfo,
                                accHolderInfoIndiv: true,
                                accHolderInfoCompany: false,
                                stateOptionforDropDown: [],
                                selectedCurrency: "",
                                selectedCountry: '',
                                selectedState: '',
                                currencyId: response.currencyId,
                                countryId: response.countryId,
                                stateId: response.stateId,
                                EventId: response.eventId,
                                BankAccountUpdateLater: response.BankAccountUpdateLater,
                                placename: response.address1,
                                street: response.location,
                                city: response.city,
                                state: response.state,
                                zip: '',
                                ishaveGST: 'SellIn',
                                EventDetailId: response.eventDetailId,
                                financeTabAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Finance')} aria-hidden="true"></i>,
                                isFinancialTabEdit: true,
                            });
                        }
                        else {
                            if (response) {
                                this.setState({
                                    placename: response.location,
                                    city: response.city,
                                    state: response.state,
                                    zip: response.zip,

                                });
                            }
                        }
                    }
                });
            }
        }, 1000);
        this.props.requestCountryTypeData();
        this.props.requestCurrencyTypeData();
    }

    handleCountryChange = (country) => {
        this.setState({ selectedCountry: country, countryId: country.value });
        this.props.requestStatesTypeData(country.altId, (response: StateTypesResponseViewModel) => {
            if (response) {
                var statesArray = [];
                response.states.map(function (data) {
                    let stateItem = {
                        value: data.id,
                        label: data.name
                    };
                    statesArray.push(stateItem);
                });
                //removing duplicate values & unwanted data is coming after the index 46 for value 101
              if (statesArray.length > 0 && country.value == 101) {
                  statesArray = statesArray.slice(0,45)
              }
              statesArray = statesArray.filter((item, index, self) =>
                  index === self.findIndex((t) => (
                      t.label === item.label && t.label === item.label
                  ))
              )
                this.setState({ stateOptionforDropDown: statesArray, selectedState: statesArray[0], stateId: statesArray[0].value })
            }
        });

    }
    handleCurrencyChange = (currency) => {
        this.setState({ selectedCurrency: currency, currencyId: currency.value });
    }
    handleStateChange = (state) => {
        this.setState({ selectedState: state, stateId: state.value });
    }

    handleFirstNameChange = (e) => {

        this.setState({ financialsAccountFirstName: e.target.value });
    }

    handlefinancialsAccountLastNameChange = (e) => {
        this.setState({ financialsAccountLastName: e.target.value });
    }
    handlefinancialsAccountLocationPlaceNameChange = (e) => {
        this.setState({ financialsAccountLocationPlaceName: e.target.value });
    }
    handlefinancialsAccountLocationAddress1Change = (e) => {
        this.setState({ financialsAccountLocationAddress1: e.target.value });
    }
    handlefinancialsAccountLocationAddress2Change = (e) => {
        this.setState({ financialsAccountLocationAddress2: e.target.value });
    }
    handlefinancialsAccountLocationCityChange = (e) => {
        this.setState({ financialsAccountLocationCity: e.target.value });
    }
    handlefinancialsAccountLocationStateChange = (e) => {
        this.setState({ financialsAccountLocationState: e.target.value });
    }
    handlefinancialsAccountLocationPostalChange = (e) => {
        this.setState({ financialsAccountLocationPostal: e.target.value });
    }
    handlefinancialsfinancialsAccountBankInfoTypeChangeSaving = (e) => {
        this.setState({ bankAccSaving: true, bankAccChecking: false, financialsAccountBankInfoType: "Saving" });
    }
    handlefinancialsfinancialsAccountBankInfoTypeChangeChecking = (e) => {
        this.setState({ bankAccChecking: true, bankAccSaving: false, financialsAccountBankInfoType: "Checking" });
    }
    handlefinancialsAccountBankInfoTaxInfoChange = (e) => {
        this.setState({ financialsAccountBankInfoTaxInfo: e.target.value });
    }
    handlefinancialsAccountBankNameChange = (e) => {
        this.setState({ financialsAccountBankName: e.target.value });
    }
    handlefinancialsAccountBankStateChange = (e) => {
        this.setState({ financialsAccounState: e.target.value });
    }
    handlefinancialsAccountBankRoutingNoChange = (e) => {
        this.setState({ financialsAccountBankRoutingNo: e.target.value });
    }
    handlefinancialsAccountBankGSTChange = (e) => {
        this.setState({ financialsAccountBankGST: e.target.value });
    }
  handlefinancialsAccountBankAccountNoChange = (e) => {
      if (e.target.value.indexOf('.') === -1) {
        this.setState({ financialsAccountBankAccountNo: e.target.value });
      }
    }
    handlefinancialsAccountBankREAccountNoChange = (e) => {
      if (e.target.value.indexOf('.') === -1) {
        this.setState({ financialsAccountBankREAccountNo: e.target.value });
      }
    }
    handlefinancialsAccountBankPANNoChange = (e) => {
        this.setState({ financialsAccountBankPANNo: e.target.value });
    }
    handlefinancialsAccountBankNickNameChange = (e) => {
        this.setState({ financialsAccountBankNickName: e.target.value });
    }
    handlefinancialsAccountBankUpdateLaterChange = (e) => {

        if (this.state.BankAccountUpdateLater === true) {
            this.setState({ BankAccountUpdateLater: false });
        }
        else {
            this.setState({ BankAccountUpdateLater: true });
        }
    }
    handlefinancialsAccountBankGSTInfoChange = (e) => {
        if (this.state.financialsAccountBankInfoTaxInfo === true) {
            this.setState({ financialsAccountBankInfoTaxInfo: false });
        }
        else {
            this.setState({ financialsAccountBankInfoTaxInfo: true });
        }

    }

    handleAccIndivClicked = (e) => {
        this.setState({ accHolderInfoIndiv: true, accHolderInfoCompany: false, financialsAccountType: "Individual" });
    }

    handlesellinGSTClicked = (e) => {
        this.setState({ sellInGST: true, dontSellInGST: false, neverGST: false, financialsAccountBankAccountGSTInfo: 'SellIn', });
    }

    handleDontSellClicked = (e) => {
        this.setState({ sellInGST: false, dontSellInGST: true, neverGST: false, financialsAccountBankAccountGSTInfo: 'DontSellIn', });
    }

    handleNEverSellClicked = (e) => {
        this.setState({ sellInGST: false, dontSellInGST: false, neverGST: true, financialsAccountBankAccountGSTInfo: 'NeverSellIn', });
    }
    handleAccCompanyClicked = (e) => {
        this.setState({ accHolderInfoIndiv: false, accHolderInfoCompany: true, financialsAccountType: "Company" });
    }

    handleChange = (address) => {
        this.setState({ placename: address });
    }

    onBlurOnlyStringField = (e) => {
        if (e.target.value !== "" && !CUSTOM_REGEX.onlyString.ex.test(e.target.value)) {
            this.setState({
                [e.target.name]: ""
            });
            alert(CUSTOM_REGEX.onlyString.msg);
        }
    }

    handleSelect = (address) => {

        var current = this;
        geocodeByAddress(address)
            .then(results => {
                current.setState({ placename: results[0].formatted_address });

                var prevval = '';
                var zip = '';
                var country = '';
                var city = '';
                var state = '';
                results[0].address_components.map(function (adrcomp) {
                    if (adrcomp.types.toString().indexOf('sublocality') >= 0 || adrcomp.types.toString().indexOf('route') >= 0) {
                        prevval += adrcomp.long_name + " ";
                    }
                    // if(adrcomp.types.toString().indexOf('locality')>=0)
                    // {
                    //     this.setState({location:adrcomp.long_name});
                    // }
                    else if (adrcomp.types.toString().indexOf('administrative_area_level_1') >= 0) {
                        current.setState({ state: adrcomp.long_name });
                        state = adrcomp.long_name;
                    }
                    else if (adrcomp.types.toString().indexOf('administrative_area_level_2') >= 0 || adrcomp.types.toString().indexOf('locality') >= 0) {
                        current.setState({ city: adrcomp.long_name });
                        if (!city)
                            city = adrcomp.long_name;
                    }
                    else if (adrcomp.types.toString().indexOf('country') >= 0) {
                        current.setState({ country: adrcomp.long_name });
                        current.setState({ countryshort: adrcomp.short_name });
                        country = adrcomp.long_name;
                    }
                    else if (adrcomp.types.toString().indexOf('postal_code') >= 0) {
                        current.setState({ zip: adrcomp.short_name });
                        zip = adrcomp.long_name;
                    }
                });

                var street = results[0].formatted_address.replace(country, '').replace(city, '').replace(zip, '').replace(state, '');
                street = street.replace(/(^[,\s]+)|([,\s]+$)/g, '');
                current.setState({ street: street });

                this.setState({ location: prevval });
                getLatLng(results[0]).then(function (latlong) {
                    current.setState({ lat: parseInt(latlong.lat), long: parseInt(latlong.lng) });
                });
            });
    }
    @autobind
    public autoCompleteAddress(address, city, state, country, zipcode, lat, long, street, route) {
        this.setState({
            street: (route == "" ? street : route + ", " + street), city: city,
            address: address,
            state: state, country: country,
            zip: zipcode,
            lat: lat,
            long: long,
            route: route
        });
    }

    @autobind
    public onChangeState(e) {
        this.setState({ state: e.target.value })
    }

    @autobind
    public onChangeCountry(e) {
        this.setState({ country: e.target.value })
    }

    @autobind
    public onChangeZip(e) {
        this.setState({ zip: e.target.value })

    }

    @autobind
    public onChangeCity(e) {
        this.setState({ city: e.target.value })
    }

    @autobind
    public onChangeStreet(e) {
        this.setState({ street: e.target.value })
    }
    @autobind
    public onChangeRoute(e) {
        this.setState({ route: e.target.value })
    }

    @autobind
    public onCustomFieldChange(zipcode) {
        this.setState({
            zip: zipcode
        });
    }

    onPasteConfirmAccountNumber = (e) => {
        e.preventDefault();
    }

    public render() {
        var context = this;
        const schema = this.getSchema();
        var currencyTypeOptions = [];
        var countryOptions = [];
        var stateOptions = [];

        if (this.props.currencyType.currencyTypes) {
            currencyTypeOptions = this.props.currencyType.currencyTypes.currencyTypes.map(function (data) {
                return {
                    value: data.id,
                    label: data.name,
                };
            }).sort((a, b) => {
                var nameA = a.label.toUpperCase(); // ignore upper and lowercase
                var nameB = b.label.toUpperCase(); // ignore upper and lowercase
                if (nameA < nameB) {
                  return -1;
                }
                if (nameA > nameB) {
                  return 1;
                }
                return 0;
              });;

            if ((this.state.selectedCurrency === "" || this.state.selectedCurrency === undefined) && this.state.selectedCurrency !== currencyTypeOptions[0] && this.state.isCurrencyLoaded != true) {

                this.setState({ selectedCurrency: currencyTypeOptions[0], isCurrencyLoaded: true })
            }
            if (this.state.isCurrencyEdit == true && currencyTypeOptions.length > 0 && this.state.isCurrencyEditLoaded != true) {
                this.setState({ isCurrencyEdit: false, isCurrencyEditLoaded: true, selectedCurrency: currencyTypeOptions.filter(x => x.value == this.state.currencyId)[0] });
            }
        }

        if (this.props.countryType.countryTypes.countries) {
            var i = 1;

            this.props.countryType.countryTypes.countries.map(function (data) {
                let countryItem = {
                    value: data.id,
                    label: data.name,
                    altId: data.altId
                };

                countryOptions.push(countryItem);
            });

            if ((this.state.selectedCountry === "" || this.state.selectedCountry === undefined) && this.state.selectedCountry !== countryOptions[0] && this.state.isStateSelect != true) {
                this.setState({ selectedCountry: countryOptions[0], isStateSelect: true });
                if (this.state.isCountryEdit == false && countryOptions.length > 0) {
                    this.props.requestStatesTypeData(countryOptions[0].altId, (response: StateTypesResponseViewModel) => {
                        if (response) {
                            var statesArray = [];
                            response.states.map(function (data) {
                                let stateItem = {
                                    value: data.id,
                                    label: data.name,
                                    altId: data.altId
                                };

                                statesArray.push(stateItem);
                            });

                            if (this.state.isStateEdit == true && statesArray.length > 0) {
                                this.setState({ isStateEdit: false, selectedState: statesArray.filter(x => x.value == 5)[0] });
                            }

                            if (this.state.stateOptionforDropDown !== statesArray)
                                this.setState({ stateOptionforDropDown: statesArray, selectedState: statesArray[0] })

                        }
                    });
                }
            }


            if (this.state.isCountryEdit == true && countryOptions.length > 0 && this.state.isCountrySelect != true) {
                this.setState({ isCountryEdit: false, isCountrySelect: true, selectedCountry: countryOptions.filter(x => x.value == this.state.countryId)[0] });

                if (this.state.countryId !== 0) {
                    this.props.requestStatesTypeData(countryOptions.filter(x => x.value == this.state.countryId)[0].altId, (response: StateTypesResponseViewModel) => {
                        if (response) {
                            var statesArray = [];
                            response.states.map(function (data) {
                                let stateItem = {
                                    value: data.id,
                                    label: data.name,
                                    altId: data.altId
                                };

                                statesArray.push(stateItem);
                            });
                            if (this.state.isStateEdit == true && statesArray.length > 0) {
                                this.setState({ isStateEdit: false, selectedState: statesArray.filter(x => x.value == 6)[0] });
                            }

                            this.setState({ stateOptionforDropDown: statesArray, selectedState: statesArray[0] })

                        }
                    });
                }
            }
        }

        if (this.props.statesType) {

            var i = 1;

            // countryOptions = this.props.countryType.countryTypes.countries.map(function (data) {
            //     return <option value={i++} >{data.name}</option>
            // });
        }

        /* if (localStorage.getItem('altId') != null && localStorage.getItem('altId') != '0' && this.state.altId !== parseInt(localStorage.getItem('altId'), 0) && this.state.isAltIdSet != true) {
             this.setState({ altId: parseInt(localStorage.getItem('altId'), 0), isAltIdSet: true });
         } */

        if (this.state.forEdit == true && this.state.financialsAccountBankAccountGSTInfo === "SellIn" && this.state.isSellInGST != true) {

            this.setState({ sellInGST: true, dontSellInGST: false, neverGST: false, isSellInGST: true });
        }
        if (this.state.forEdit == true && this.state.financialsAccountBankAccountGSTInfo === "DontSellIn" && this.state.isSellGST != true) {

            this.setState({ sellInGST: false, dontSellInGST: true, neverGST: false, isSellGST: true });
        }
        if (this.state.forEdit == true && this.state.financialsAccountBankAccountGSTInfo === "NeverSellIn" && this.state.isNeverGST != true) {

            this.setState({ sellInGST: false, dontSellInGST: false, neverGST: true, isNeverGST: true });
        }

        const MY_API = 'AIzaSyCc3zoz5TZaG3w2oF7IeR - fhxNXi8uywNk';
        var _url;
        if (this.state.address != "" && this.state.country != "") {
            _url = `https://www.google.com/maps/embed/v1/place?key=${MY_API}&q=${this.state.address},${this.state.city},${this.state.country}`;
        } else {
            var addressData = "shivaji nagar, pune";
            var cityName = 'Pune';
            var countryName = 'India'
            _url = `https://www.google.com/maps/embed/v1/place?key=${MY_API}&q=${addressData},${cityName},${countryName}`;
        }
        return (
            <Formik
                initialValues={{}}
                validationSchema={schema}
                onSubmit={(values: Values) => {
                    if (this.state.financialsAccountBankAccountNo != this.state.financialsAccountBankREAccountNo) {
                        alert("Account Number and Re-entered account doesn't match");
                    }
                    else {
                        setTimeout(() => {
                            let ui: FinanceResponseViewModel = new FinanceResponseViewModel();
                            ui.currencyId = this.state.currencyId;
                            ui.countryId = this.state.countryId;
                            ui.stateId = this.state.stateId;
                            ui.accountType = this.state.financialsAccountType;
                            ui.firstName = this.state.financialsAccountFirstName;
                            ui.lastName = this.state.financialsAccountLastName;
                            ui.bankAcountType = this.state.financialsAccountBankInfoType;
                            ui.isBankAccountGST = this.state.financialsAccountBankInfoTaxInfo;
                            ui.bankName = this.state.financialsAccountBankName;
                            ui.routingNo = this.state.financialsAccountBankRoutingNo;
                            ui.gstNo = this.state.financialsAccountBankGST;
                            ui.accountNo = this.state.financialsAccountBankAccountNo;
                            ui.panNo = this.state.financialsAccountBankPANNo;
                            ui.accountNickName = this.state.financialsAccountBankNickName;
                            ui.isUpdatLater = this.state.BankAccountUpdateLater;
                            ui.financialsAccountBankAccountGSTInfo = this.state.financialsAccountBankAccountGSTInfo;
                            ui.ishaveGST = this.state.financialsAccountBankAccountGSTInfo;
                            ui.location = this.state.location;
                            ui.placename = this.state.street;
                            ui.address1 = this.state.placename;
                            ui.address2 = this.state.placename;
                            ui.city = this.state.city;
                            ui.state = this.state.state;
                            ui.zip = this.state.zip;
                            ui.street = this.state.street;
                            ui.country = this.state.country;
                            ui.eventId = this.state.EventId;
                            ui.id = this.state.id;
                            ui.eventDetailId = this.state.EventDetailId;
                            ui.altId = this.state.altId;

                            this.props.saveFinancialDetails(ui, (response) => {
                                this.setState({ isSavedSuccesfully: true });
                            });
                        }, 400);
                    }
                }}
                render={({ errors, touched, isSubmitting }) => (
                    <div >
                        <Form>
                            <div >
                                <div>
                                    <div className="col-sm-12 pb-3">
                                        <a className="place-listing active"><span className="rounded-circle border border-primary place-listing">1</span> Financial Details of Seller/Inventory Owner</a>{this.state.financeTabAccordianEditComponent}
                                        <div className="form-group pt-3">
                                            <div className="row">
                                                <div className="col-12">
                                                    <label>Name </label>
                                                </div>
                                                <div className="col-12 col-sm-6">
                                                    <Field
                                                        disabled={this.state.isFinancialTabEdit}
                                                        type="text"
                                                        name="financialsAccountFirstName"
                                                        value={this.state.financialsAccountFirstName}
                                                        className="form-control mb-2 mb-sm-0"
                                                        placeholder="First name"
                                                        onChange={this.handleFirstNameChange.bind(this)}
                                                        required />

                                                </div>
                                                <div className="col-12 col-sm-6">
                                                    <Field
                                                        disabled={this.state.isFinancialTabEdit}
                                                        type="text" name="financialsAccountLastNamed"
                                                        value={this.state.financialsAccountLastName}
                                                        className="form-control"
                                                        placeholder="Last name"
                                                        onChange={this.handlefinancialsAccountLastNameChange.bind(this)}
                                                        required />
                                                </div>
                                            </div>
                                        </div>

                                        <div className="form-group">
                                            <label className="d-block">Seller/Inventory Owner Information</label>
                                            <div className="custom-control custom-radio custom-control-inline p-0">
                                                <label style={{ marginLeft: 5, marginBottom: 10, marginRight: 10 }} >
                                                    <Field disabled={this.state.isFinancialTabEdit} style={{ transform: "scale(1.5)", marginRight: 10 }} checked={this.state.accHolderInfoIndiv} onChange={this.handleAccIndivClicked} type="radio" name="isused2" />
                                                    Individual</label>
                                            </div>


                                            <div className="custom-control custom-radio custom-control-inline">
                                                <label style={{ marginLeft: 5, marginBottom: 10 }} >
                                                    <Field disabled={this.state.isFinancialTabEdit} style={{ transform: "scale(1.5)", marginRight: 10 }} checked={this.state.accHolderInfoCompany} onChange={this.handleAccCompanyClicked} type="radio" name="isused2" />
                                                    {/* <label className="custom-control-label" >Company</label> */}
                                                    {/* checked={this.state.accHolderInfoCompany} */}
                                                    Company</label>
                                            </div>
                                        </div>

                                        <div className="form-group">
                                            <div className="row">
                                                <div className="col-12 col-sm-6">
                                                    <label >Currency</label>
                                                    <Select
                                                        value={this.state.selectedCurrency}
                                                        options={currencyTypeOptions}
                                                        onChange={this.handleCurrencyChange}
                                                        disabled={this.state.isFinancialTabEdit}
                                                        required
                                                    />
                                                </div>
                                                <div className="col-12 col-sm-6">
                                                    <label >In which country will you be paid?</label>
                                                    <Select name="countryid"
                                                        value={this.state.selectedCountry}
                                                        options={countryOptions}
                                                        onChange={this.handleCountryChange}
                                                        disabled={this.state.isFinancialTabEdit}
                                                        required
                                                    />
                                                </div>
                                            </div>
                                        </div>

                                        {(this.state.countryId != 231 && this.state.countryId != 230 && this.state.countryId != 13) && <div className="form-group">
                                            <div className="row">
                                                <div className="col-12 col-sm-6">
                                                    <label >Location <span className="text-danger">*</span> </label>

                                                    <div className="row mb-0">
                                                        <div className="col-12 col-sm-6">
                                                            <Field
                                                                disabled={this.state.isFinancialTabEdit}
                                                                name="street"
                                                                type="text"
                                                                className="form-control mb-2"
                                                                placeholder="Address Line 1"
                                                                value={this.state.street}
                                                                onChange={this.onChangeStreet}
                                                            />
                                                        </div>
                                                        <div className="col-12 col-sm-6">
                                                            <input
                                                                disabled={this.state.isFinancialTabEdit}
                                                                name="state"
                                                                type="text"
                                                                className="form-control mb-2"
                                                                placeholder="Address Line 2"
                                                                value={this.state.route}
                                                                onChange={this.onChangeRoute}
                                                            />
                                                        </div>
                                                    </div>
                                                    <Field disabled={this.state.isFinancialTabEdit} onChange={this.onChangeCity} name="city" type="text" className="form-control mb-2" placeholder="City" value={this.state.city} />
                                                    <div className="row">
                                                        <div className="col-12 col-sm-6">
                                                            <Field disabled={this.state.isFinancialTabEdit} onChange={this.onChangeState} name="state" type="text" className="form-control mb-2" placeholder="State" value={this.state.state} />
                                                        </div>
                                                        <div className="col-12 col-sm-6">
                                                            <Field disabled={this.state.isFinancialTabEdit} onChange={this.onChangeZip} name="zip" type="text" className="form-control mb-2" placeholder="Zip/Postal" value={this.state.zip} required />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>}
                                        {(this.state.countryId != 231 && this.state.countryId != 230 && this.state.countryId != 13) && <div className="form-group">
                                        <div className="row">
                                            <div className="col-sm-6">
                                                <div className="form-group">
                                                    <label className="d-block">Bank account information</label>
                                                    <div className="custom-control custom-radio custom-control-inline p-0">

                                                        <label style={{ marginLeft: 5, marginBottom: 10 }} >
                                                            <Field disabled={this.state.isFinancialTabEdit} style={{ transform: "scale(1.5)", marginRight: 10 }} checked={this.state.bankAccChecking} onChange={this.handlefinancialsfinancialsAccountBankInfoTypeChangeChecking} type="radio" name="isused" />
                                                            Checking</label>

                                                    </div>
                                                    <div className="custom-control custom-radio custom-control-inline">

                                                        <label style={{ marginLeft: 5, marginBottom: 10 }} >
                                                            <Field disabled={this.state.isFinancialTabEdit} style={{ transform: "scale(1.5)", marginRight: 10 }} checked={this.state.bankAccSaving} onChange={this.handlefinancialsfinancialsAccountBankInfoTypeChangeSaving} type="radio" name="isused1" />
                                                            Savings</label>

                                                    </div>
                                                </div>

                                                <div className="form-group">
                                                    <label>Bank name</label>
                                                    <input disabled={this.state.isFinancialTabEdit} type="text" className="form-control" placeholder="eg. Wells Fargo, Chase, Bank of America" value={this.state.financialsAccountBankName}
                                                        onChange={this.handlefinancialsAccountBankNameChange}
                                                        required
                                                    >
                                                    </input>
                                                </div>

                                                <div className="form-group">
                                                    <label>Routing number</label>
                                                    <input disabled={this.state.isFinancialTabEdit} type="text" className="form-control" placeholder="XXXXXXXXX" value={this.state.financialsAccountBankRoutingNo}
                                                        onChange={this.handlefinancialsAccountBankRoutingNoChange}
                                                        required
                                                    />

                                                </div>


                                                <div className="form-group">
                                                    <div className="row">
                                                        <div className="col-12">
                                                            <label>Account number</label>
                                                        </div>
                                                        <div className="col-sm-6">
                                                            <input
                                                                disabled={this.state.isFinancialTabEdit}
                                                                type="text"
                                                                className="form-control mb-2 mb-sm-0"
                                                                placeholder="Account number"
                                                                value={this.state.financialsAccountBankAccountNo}
                                                                onChange={this.handlefinancialsAccountBankAccountNoChange}
                                                                required
                                                            />

                                                        </div>
                                                        <div className="col">
                                                            <input
                                                                disabled={this.state.isFinancialTabEdit}
                                                                type="text"
                                                                className="form-control"
                                                                placeholder="Re-enter account number"
                                                                value={this.state.financialsAccountBankREAccountNo}
                                                                onChange={this.handlefinancialsAccountBankREAccountNoChange}
                                                                onPaste={this.onPasteConfirmAccountNumber}
                                                                required
                                                            />

                                                        </div>
                                                    </div>
                                                </div>

                                                <div className="form-group">
                                                    <label>Account Nickname</label>
                                                    <input disabled={this.state.isFinancialTabEdit} type="text" className="form-control" placeholder="e.g. My primary account" value={this.state.financialsAccountBankNickName}
                                                        onChange={this.handlefinancialsAccountBankNickNameChange}
                                                    />
                                                </div>
                                            </div>
                                            <div className="col-sm-6">
                                                <div className="form-group">
                                                    <label>Tax information</label>
                                                    <div className="custom-control custom-checkbox p-0">
                                                        <label  >

                                                            <Field disabled={this.state.isFinancialTabEdit} style={{ transform: "scale(1.5)", marginRight: 10 }} name="GST" checked={this.state.financialsAccountBankInfoTaxInfo} onChange={this.handlefinancialsAccountBankGSTInfoChange} type="checkbox" required />
                                                            I have GST Number   </label>

                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <label >Select Your State</label>
                                                    <Select
                                                        options={this.state.stateOptionforDropDown}
                                                        value={this.state.selectedState}
                                                        onChange={this.handleStateChange}
                                                        disabled={this.state.isFinancialTabEdit}
                                                        required
                                                    />
                                                </div>
                                                <div className="form-group">
                                                    <label>GST Number</label>
                                                    <input disabled={this.state.isFinancialTabEdit} type="text" className="form-control" placeholder="Provisional GSTIN" value={this.state.financialsAccountBankGST}
                                                        onChange={this.handlefinancialsAccountBankGSTChange} />

                                                </div>
                                                <div className="form-group">
                                                    <label >PAN number</label>
                                                    <input disabled={this.state.isFinancialTabEdit} type="text" className="form-control" placeholder="PAN number" value={this.state.financialsAccountBankPANNo}
                                                        onChange={this.handlefinancialsAccountBankPANNoChange} required />

                                                </div>
                                                <div className="form-group">
                                                    <div className="custom-control custom-checkbox">

                                                        <label  >

                                                            <Field disabled={this.state.isFinancialTabEdit} style={{ transform: "scale(1.5)", marginRight: 10 }} name="GST" checked={this.state.BankAccountUpdateLater} onChange={this.handlefinancialsAccountBankUpdateLaterChange} type="checkbox" />
                                                            I will Update Later   </label>

                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <div className="custom-control custom-radio">
                                                        <label className="" >

                                                            <Field disabled={this.state.isFinancialTabEdit} style={{ transform: "scale(1.5)", marginRight: 10 }} checked={this.state.sellInGST} onChange={this.handlesellinGSTClicked} type="radio" name="isused4" />
                                                            I sell in GST Exempted Category</label>

                                                    </div>
                                                    <div className="custom-control custom-radio">

                                                        <label className="" >
                                                            <Field disabled={this.state.isFinancialTabEdit} style={{ transform: "scale(1.5)", marginRight: 10 }} checked={this.state.dontSellInGST} onChange={this.handleDontSellClicked} type="radio" name="isused5" />
                                                            I don’t have it handy and will update later.</label>

                                                    </div>
                                                    <div className="custom-control">
                                                        <label className="" >
                                                            <Field disabled={this.state.isFinancialTabEdit} style={{ transform: "scale(1.5)", marginRight: 10 }} checked={this.state.neverGST} onChange={this.handleNEverSellClicked} type="radio" name="isused6" />
                                                            I have never registered for GST.</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>}
                                    </div>

                                </div>
                                {(!this.showSaveButton && (this.state.countryId != 231 && this.state.countryId != 230 && this.state.countryId != 13)) &&
                                    <div style={{ display: 'none' }} className="text-center pt-4 pb-4">
                                        <a href="#" className="text-decoration-none mr-4">Cancel</a>
                                        <div className="btn-group">
                                            <button type="button" className="btn btn-outline-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                Save
                                        </button>
                                            <div className="dropdown-menu">
                                                <button className="dropdown-item" name="financesave" id="financesaveBtn" type="submit" >Save as draft</button>
                                                <button className="dropdown-item" type="submit" >Save (continue editing)</button>
                                                <button className="dropdown-item" type="submit" >Save & submit for approval</button>
                                                <button className="dropdown-item" type="submit" >Save & add another</button>
                                                {/* <a className="dropdown-item" href="#">Save & add another</a> */}
                                            </div>
                                        </div>
                                    </div>}
                                {(this.showSaveButton && (this.state.countryId != 231 && this.state.countryId != 230 && this.state.countryId != 13)) &&
                                    <div style={{ display: 'block' }} className="text-center pt-4 pb-4">
                                        <a href="#" className="text-decoration-none mr-4">Cancel</a>
                                        <div className="btn-group">
                                            {/* <button className="btn btn-success" type="submit">Update Invite</button> */}
                                            <button type="button" className="btn btn-outline-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                Save
                                        </button>
                                            <div className="dropdown-menu">
                                                <button className="dropdown-item" name="financesave" id="financesaveBtn" type="submit" >Save as draft</button>
                                                <button className="dropdown-item" type="submit" >Save (continue editing)</button>
                                                <button className="dropdown-item" type="submit" >Save & submit for approval</button>
                                                <button className="dropdown-item" type="submit" >Save & add another</button>
                                                {/* <a className="dropdown-item" href="#">Save & add another</a> */}
                                            </div>
                                        </div>
                                    </div>}

                                {(this.state.countryId == 231 || this.state.countryId == 230 || this.state.countryId == 13) && <div>
                                    <div className="home-view-wrapper text-center container py-5 my-5">
                                        <h5>Please create the stripe connect account by clicking link below.</h5>
                                        <a target="_blank" href={`https://connect.stripe.com/express/oauth/authorize?client_id=${this.state.stripeConnectClientId}&redirect_uri=${`${this.state.origin}/stripe-connect/success/${this.state.placeAltId}`}`} className="btn bnt-lg btn-outline-primary mt-4">Click Here</a>
                                    </div>
                                </div>}
                            </div>
                        </Form>
                    </div>
                )}
            />
        );
    }
}




export default connect(
    (state: IApplicationState) => ({ currencyType: state.currencyType, countryType: state.countryType, statesType: state.statesType, financialDetails: state.financialDetails }),
    (dispatch) => bindActionCreators({ ...placeFinancialsStore.actionCreators, ...CountryStore.actionCreators, ...StatesStore.actionCreators, ...FinanceDetailsStore.actionCreators }, dispatch)
)(PlaceFinancials);
