import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../../stores";
import { bindActionCreators } from "redux";
import { gets3BaseUrl } from "../../../utils/imageCdn";
import * as CountryStore from "../../../stores/Country";
import * as StatesStore from "../../../stores/States";
import * as CurrencyStore from "../../../stores/CurrencyType";
import * as RedemptionStore from "../../../stores/Redemption";
import { StateTypesResponseViewModel } from "../../../models/Finance/StateTypesResponseViewModel";
import { AlgoliaSearchService, AlgoliaResult } from "../../../services/AlgoliaSearch";
import classnames from "classnames";
import CreateGuideForm from "./CreateGuideForm";
import DocumentsForm from "./DocumentsForm";
import ServicesForm from "./ServicesForm";
import FinancialDetailForm from "./FinancialDetailForm";
import { CreateGuideInputModel } from "../../../models/Redemption/GuideViewModel"
import { GuideResponseModel } from "../../../models/Redemption/GuideViewModel";
import { CitiesResponseModel } from "../../../stores/States";
import KzLoader from "../../../components/Loader/KzLoader";
import { RouteComponentProps } from "react-router-dom";
import { CountryTypeViewModel } from "../../../models/Finance/CountryTypeViewModel";
import { CurrencyTypeViewModel } from "../../../models/CurrencyTypeViewModel";
import { CustomerDocumentType } from "../../../models/Inventory/DocumentTypesResponseViewModel";
import { Language } from "../../../models/Redemption/Language";
import { EditGuideResponseModel } from "../../../models/Redemption/EditGuideResponseModel";
import { StateTypeViewModel } from "../../../models/Finance/StateTypeViewModel";

type createGuideStore = CountryStore.ICountryTypeProps
    & StatesStore.IStatesTypeProps
    & CurrencyStore.ICurrencyTypeProps
    & RedemptionStore.RedemptionComponentProps
    & typeof CountryStore.actionCreators
    & typeof RedemptionStore.actionCreators
    & typeof StatesStore.actionCreators
    & typeof CurrencyStore.actionCreators
    & RouteComponentProps<{ userAltId: string }>;

class CreateGuest extends React.Component<createGuideStore, any> {
    constructor(props) {
        super(props)
        this.state = {
            formData: {
                createGuideForm: {},
                financialDetailForm: {},
                servicesForm: {},
                documentsForm: {}
            },
            tab1: {
                isActive: true,
                isShow: true
            },
            tab2: {
                isActive: false,
                isShow: false
            },
            tab3: {
                isActive: false,
                isShow: false
            },
            tab4: {
                isActive: false,
                isShow: false
            },
            stateOptions: [],
            cityOptions: [],
            placeOptions: [],
            currencyOptions: [],
            countryOptions: [],
            documentOptions: [],
            languageOptions: [],
            isEditGuide: props.match.params.userAltId ? true : false
        };
    }

    hydrateForm = (values: EditGuideResponseModel) => {
        this.setState({
            formData: {
                ...this.state.formData,
                createGuideForm: {
                    destinationname: [...this.state.placeOptions].filter(t => values.guidePlaceMappings.includes(t.value)),
                    firstName: values.user.firstName,
                    lastName: values.user.lastName,
                    email: values.user.email,
                    language: [...this.state.languageOptions].filter(t => values.guideDetail.languageId.split(",").includes(t.value.toString())),
                    phoneCode: null,
                    phoneNumber: values.user.phoneNumber,
                    address1: values.userAddressDetail.addressLine1,
                    address2: values.userAddressDetail.addressLine2,
                    residentCountry: {
                        label: values.userAddressDetailMapping.residentialCountry.name,
                        value: values.userAddressDetailMapping.residentialCountry.id,
                        phonecode: values.userAddressDetailMapping.residentialCountry.phoneCode
                    },
                    residentState: {
                        label: values.userAddressDetailMapping.residentialState.name,
                        value: values.userAddressDetailMapping.residentialState.id,
                    },
                    residentCity: {
                        label: values.userAddressDetailMapping.residentialCity.name,
                        value: values.userAddressDetailMapping.residentialCity.id
                    },
                    zip: values.userAddressDetail.zipcode
                },
                financialDetailForm: {
                    accountType: values.masterFinanceDetails.accountTypeId,
                    currency: [...this.state.currencyOptions].filter(t => t.value == values.masterFinanceDetails.currenyId)[0],
                    financeCountry: [...this.state.countryOptions].filter(t => t.value == values.masterFinanceDetails.countryId)[0],
                    bankAccountType: values.masterFinanceDetails.bankAccountTypeId,
                    bankName: values.masterFinanceDetails.bankName,
                    accountNumber: values.masterFinanceDetails.accountNumber,
                    branchCode: values.masterFinanceDetails.branchCode,
                    financeState: {
                        label: values.userAddressDetailMapping.financialState.name,
                        value: values.userAddressDetailMapping.financialState.id
                    },
                    routingNumber: values.masterFinanceDetails.routingNumber,
                    isTax: values.masterFinanceDetails.taxId == "" ? false : true,
                    taxId: values.masterFinanceDetails.taxId
                },
                servicesForm: {
                    selectedServices: values.services.map(t => ({ id: t.id, name: t.name, description: t.description })),
                    serviceNotes: values.guideServices[0].notes
                },
                documentsForm: {
                    document: [...this.state.documentOptions].filter(t => t.value == values.guideDocumentMappings[0].documentID)[0]
                }
            }
        });
    };

    componentDidMount() {
        this.props.requestCountryTypeData((res) => {
            this.setState({
                countryOptions: this.mapPropsToOptions<CountryTypeViewModel>(res.countries, "name", "id", "phonecode")
            });
        });
        this.props.requestCurrencyTypeData(res => {
            this.setState({
                currencyOptions: this.mapPropsToOptions<CurrencyTypeViewModel>(res.currencyTypes, "name", "id")
            });
        });
        this.props.requestDocumentTypeData(res => {
            this.setState({
                documentOptions: this.mapPropsToOptions<CustomerDocumentType>(res.documentTypes, "documentType", "id")
            });
        });
        this.props.requestLanguages(res => {
            this.setState({
                languageOptions: this.mapPropsToOptions<Language>(res.languages, "name", "id")
            });
        });
        if (this.state.isEditGuide) {
            this.props.requestEditGuideDetail(this.props.match.params.userAltId, (res) => {
                this.hydrateForm(res);
            });
        }
    }

    submitFormHandler = () => {
        let values = this.state.formData;
        let saveGuideModel: CreateGuideInputModel = {
            eventIDs: values.createGuideForm.destinationname.map(item => item.value),
            firstName: values.createGuideForm.firstName,
            lastName: values.createGuideForm.lastName,
            email: values.createGuideForm.email,
            addressLineOne: values.createGuideForm.address1,
            addressLineTwo: values.createGuideForm.address2,
            zip: values.createGuideForm.zip,
            residentCountryAltId: values.createGuideForm.residentCountry.value,
            residentStateId: values.createGuideForm.residentState.id,
            residentCityId: values.createGuideForm.residentCity.id,
            languageId: values.createGuideForm.language.map(item => item.value),
            phoneCode: values.createGuideForm.phonecode.phonecode,
            phoneNumber: values.createGuideForm.phoneNumber,
            sellerType: values.financialDetailForm.sellerType,
            currencyId: values.financialDetailForm.currency.value,
            financeCountryAltId: values.financialDetailForm.financeCountry.value,
            bankAccountType: values.financialDetailForm.bankAccountType || 1,
            accountType: values.financialDetailForm.accountType || 1,
            bankName: values.financialDetailForm.bankName,
            accountNumber: values.financialDetailForm.accountNumber,
            branchCode: values.financialDetailForm.branchCode,
            financeStateId: values.financialDetailForm.financeState.id,
            taxId: values.financialDetailForm.taxId,
            routingNumber: values.financialDetailForm.routingNumber,
            services: values.servicesForm.selectedServices,
            serviceNotes: values.servicesForm.serviceNotes,
            document: values.documentsForm.document.value,
            isEditGuide: this.state.isEditGuide
        };
        this.props.saveGuide(saveGuideModel, (response: GuideResponseModel) => {
            if (response.success) {
                alert("Guide Created Successful!")
            } else {
                alert("Guide Creation Failed!")
            }
        });
    };

    getAlgoliaResult = (val: string) => {
        if (val != "" && val.length > 2) {
            AlgoliaSearchService.getAlgoliaResults(val, (result: AlgoliaResult) => {
                if (result.success) {
                    this.setState({
                        placeOptions: this.mapPropsToOptions<any>(result.results, "name",
                            "objectID",
                            "city",
                            "country",
                            "placeImageUrl")
                    });
                } else {
                    this.setState({
                        placeOptions: []
                    });
                }
            });
        }
    };

    getDefaultStateOption = (country) => {
        this.props.requestStatesTypeData(
            country.value,
            (response: StateTypesResponseViewModel) => {
                this.setState({
                    stateOptions: this.mapPropsToOptions<StateTypeViewModel>(response.states, "name", "id")

                });
            });
    };

    getDefaultCityOption = (state) => {
        this.props.requestCitiesData(
            state.value,
            (response: CitiesResponseModel) => {
                let cityOptions = [];
                response.cities.map((data) => {
                    let stateItem = {
                        value: data.altId,
                        label: data.name,
                        id: data.id
                    };
                    cityOptions.push(stateItem);
                });
                this.setState({
                    cityOptions
                });
            });
    };

    mapPropsToOptions = <T extends {}>(props: T[], label: string, value: string, ...rest: any[]) => {
        return props.map(t => {
            let temp = {
                label: t[label],
                value: t[value]
            };
            for (const key of rest) {
                temp[key] = t[key]
            }
            return temp;
        });
    };

    getAcitveTabButtonClass = (tab) => {
        return classnames("nav-item nav-link", { active: tab.isActive, disabled: !tab.isShow });
    };

    getAcitveTabClass = (tab) => {
        return classnames("tab-pane fade show", { active: tab.isActive });
    };

    onTabChange = (tab: string) => {
        this.setState({
            tab1: {
                ...this.state.tab1,
                isActive: false,
            },
            tab2: {
                ...this.state.tab2,
                isActive: false,
            },
            tab3: {
                ...this.state.tab3,
                isActive: false,
            },
            tab4: {
                ...this.state.tab4,
                isActive: false,
            },
            [tab]: {
                isActive: true,
                isShow: true
            }
        });
    };

    saveFormSegmentState = (segment, values, callback) => {
        this.setState({
            formData: {
                ...this.state.formData,
                [segment]: { ...values }
            }
        }, callback);
    };

    render() {
        if ((this.props.Redemption.saveGuideResponse.isSaving && !this.props.Redemption.saveGuideResponse.success) ||
            (this.state.isEditGuide && this.props.Redemption.isLoadingEditGuideDetail)
        ) {
            return <KzLoader />;
        }

        const formatOptionLabel = ({ placeImageUrl, label, city, country }) => (
            <a href="javascript:void(0);" className="btn btn-sm btn-outline-secondary btn-block border-0 text-left"> <img src={placeImageUrl} alt="shop local" width="50"
                onError={(e) => {
                    e.currentTarget.src = gets3BaseUrl() + "/places/tiles/tiles-placeholder.jpg"
                }} className="mr-2 align-top" />
                <div className="d-inline-block srh-rel-txt"> <b>{label}</b> <br />
                    <div style={{ fontSize: "10px" }}>
                        {city + ", " + country}
                    </div>
                </div>
            </a>
        );

        return <div>
            <nav className="pb-4">
                <div className="nav justify-content-center text-uppercase fee-tab" id="nav-tab" role="tablist">
                    <a
                        onClick={this.state.tab1.isShow ? () => this.onTabChange("tab1") : null}
                        className={this.getAcitveTabButtonClass(this.state.tab1)}
                        id="nav-Details-tab"
                        data-toggle="tab"
                        href="#nav-Details"
                        role="tab"
                        aria-controls="nav-Details">
                        Guide Signup
                        </a>
                    <a
                        onClick={this.state.tab2.isShow ? () => this.onTabChange("tab2") : null}
                        className={this.getAcitveTabButtonClass(this.state.tab2)}
                        id="nav-Inventory-tab"
                        data-toggle="tab"
                        href="#nav-Inventory"
                        role="tab"
                        aria-controls="nav-Inventory">
                        Financial Detail
                        </a>
                    <a
                        onClick={this.state.tab3.isShow ? () => this.onTabChange("tab3") : null}
                        className={this.getAcitveTabButtonClass(this.state.tab3)}
                        id="nav-contact-tab"
                        data-toggle="tab"
                        href="#nav-contact"
                        role="tab"
                        aria-controls="nav-contact">
                        Services
                        </a>
                    <a
                        onClick={this.state.tab4.isShow ? () => this.onTabChange("tab4") : null}
                        className={this.getAcitveTabButtonClass(this.state.tab4)}
                        id="nav-contact-tab"
                        data-toggle="tab"
                        href="#nav-contact"
                        role="tab"
                        aria-controls="nav-contact">
                        Documents
                        </a>
                </div>
            </nav>
            <div className="tab-content bg-white rounded shadow-sm pt-3" id="nav-tabContent">
                <div
                    className={this.getAcitveTabClass(this.state.tab1)}
                    id="nav-Details"
                    role="tabpanel"
                    aria-labelledby="nav-Details-tab"
                >
                    <CreateGuideForm
                        placeOptions={this.state.placeOptions}
                        getAlgoliaResult={this.getAlgoliaResult}
                        formatOptionLabel={formatOptionLabel}
                        languageOptions={this.state.languageOptions}
                        countryType={this.props.countryType}
                        saveFormSegmentState={this.saveFormSegmentState}
                        onTabChange={this.onTabChange}
                        countryOptions={this.state.countryOptions}
                        stateOptions={this.state.stateOptions}
                        cityOptions={this.state.cityOptions}
                        getDefaultCityOption={this.getDefaultCityOption}
                        getDefaultStateOption={this.getDefaultStateOption}
                        formState={this.state.formData.createGuideForm}
                    />
                </div>
                <div
                    className={this.getAcitveTabClass(this.state.tab2)}
                    id="nav-Details"
                    role="tabpanel"
                    aria-labelledby="nav-Details-tab"
                >
                    <FinancialDetailForm
                        currencyOptions={this.state.currencyOptions}
                        countryOptions={this.state.countryOptions}
                        stateOptions={this.state.stateOptions}
                        saveFormSegmentState={this.saveFormSegmentState}
                        onTabChange={this.onTabChange}
                        getDefaultStateOption={this.getDefaultStateOption}
                        formState={this.state.formData.financialDetailForm}
                    />
                </div>
                <div
                    className={this.getAcitveTabClass(this.state.tab3)}
                    id="nav-Details"
                    role="tabpanel"
                    aria-labelledby="nav-Details-tab"
                >
                    <ServicesForm
                        onTabChange={this.onTabChange}
                        saveFormSegmentState={this.saveFormSegmentState}
                        formState={this.state.formData.servicesForm}
                    />
                </div>
                <div
                    className={this.getAcitveTabClass(this.state.tab4)}
                    id="nav-Details"
                    role="tabpanel"
                    aria-labelledby="nav-Details-tab"
                >
                    <DocumentsForm
                        documentOptions={this.state.documentOptions}
                        onTabChange={this.onTabChange}
                        saveFormSegmentState={this.saveFormSegmentState}
                        submitFormHandler={this.submitFormHandler}
                        formState={this.state.formData.documentsForm}
                    />
                </div>
            </div>
        </div >
    }
}

export default connect(
    (state: IApplicationState) => ({
        currencyType: state.currencyType,
        countryType: state.countryType,
        statesType: state.statesType,
        Redemption: state.Redemption
    }),
    (dispatch) => bindActionCreators({
        ...CountryStore.actionCreators,
        ...StatesStore.actionCreators,
        ...CurrencyStore.actionCreators,
        ...RedemptionStore.actionCreators
    }, dispatch)
)(CreateGuest);
