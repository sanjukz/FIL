import { autobind } from "core-decorators";
import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import { bindActionCreators } from "redux";
import * as DescriptionSrore from "../stores/Description";
import * as CountryStore from "../stores/Country";
import DescriptionInputViewModel from "../models/Description/DescriptionInputViewModel";
import DescriptionResponseViewModel from "../models/Description/DescriptionResponseViewModel";
import CityCountryDescriptionResponseViewModel from "../models/Description/CityCountryDescriptionResponseViewModel";
import CKEditor from "react-ckeditor-component";
import Select from "react-select";

type DescriptionsProps = DescriptionSrore.DescriptionComponentProps
    & CountryStore.ICountryTypeProps
    & typeof CountryStore.actionCreators
    & typeof DescriptionSrore.actionCreators;

class Descriptions extends React.Component<DescriptionsProps, any>{
    inputElement: any;
    constructor(props) {
        super(props)
        this.state = {
            isInventoryShow: false,
            finance: false,
            cityDescription: "",
            countryDescription: "",
            stateDescription: "",
            isCityDescription: true,
            isStateDescription: false,
            isCountryDescription: false,
            selectedCity: null,
            selectedCountry: null,
            selectedState: null
        }
    }

    public componentDidMount() {
        this.props.requestDescriptionSearchData();
        this.props.requestCountryTypeData();
    }

    @autobind
    public onCountryClick() {
        localStorage.setItem("isShow", "true");
        this.setState({ isStateDescription: false, isCountryDescription: true, isCityDescription: false });
    }

    @autobind
    public onCityClick() {
        this.setState({ isStateDescription: false, isCountryDescription: false, isCityDescription: true });
        localStorage.removeItem("isShow");
    }

    @autobind
    public onStateClick() {
        this.setState({ isStateDescription: true, isCountryDescription: false, isCityDescription: false });
        localStorage.removeItem("isShow");
    }

    @autobind
    public onCityDescChange(e) {
        var newContent = e.editor.getData();
        this.setState({ cityDescription: newContent });
    }

    @autobind
    public onStateDescChange(e) {
        var newContent = e.editor.getData();
        this.setState({ stateDescription: newContent });
    }

    @autobind
    public onCountryDescChange(e) {
        var newContent = e.editor.getData();
        this.setState({ countryDescription: newContent });
    }

    handleCity = selectedCity => {
        this.setState({ selectedCity: selectedCity }, function () {
            var data = this.getDescriptionData(this);
            this.props.getDescription(data, (response: CityCountryDescriptionResponseViewModel) => {
                if (response.success) {
                    this.setState({ cityDescription: response.description });
                } else {
                    this.setState({ cityDescription: "" });
                }
            });
        });
    };

    handleState = selectedState => {
        this.setState({ selectedState: selectedState }, function () {
            var data = this.getDescriptionData(this);
            this.props.getDescription(data, (response: CityCountryDescriptionResponseViewModel) => {
                if (response.success) {
                    this.setState({ stateDescription: response.description });
                } else {
                    this.setState({ stateDescription: "" });
                }
            });
        });
    };

    handleCountry = selectedCountry => {
        this.setState({ selectedCountry: selectedCountry }, function () {
            var data = this.getDescriptionData(this);
            this.props.getDescription(data, (response: CityCountryDescriptionResponseViewModel) => {
                if (response.success) {
                    this.setState({ countryDescription: response.description });
                } else {
                    this.setState({ countryDescription: "" });
                }
            });
        });
    };

    public render() {
        var cityClassName = "nav-item nav-link";
        var countryClassName = "nav-item nav-link ";
        var stateClassName = "nav-item nav-link ";

        var cityTabClassName = "tab-pane fade d-none";
        var countryTabClassName = "tab-pane fade d-none";
        var stateTabClassName = "tab-pane fade d-none";

        var displayClass = "none";
        if (!this.state.isStateDescription && !this.state.isCountryDescription) {
            cityClassName = "nav-item nav-link active";
            cityTabClassName = "tab-pane fade show active";
        } else if (!this.state.isStateDescription && !this.state.isCityDescription) {
            countryClassName = "nav-item nav-link active";
            countryTabClassName = "tab-pane fade show active";
            displayClass = "show";
        } else if (!this.state.isCountryDescription && !this.state.isCityDescription) {
            stateClassName = "nav-item nav-link active";
            stateTabClassName = "tab-pane fade show active";
        }

        var cities = [];
        var countries = [];
        var states = [];
        if (this.props.Description && this.props.Description.citiesResult) {
            this.props.Description.citiesResult.itinerarySerchData.map(function (item, index) {
                var data = {
                    value: index,
                    label: item.cityName + " - " + item.countryName
                };
                cities.push(data);
            });
        }

        if (this.props.Description && this.props.Description.citiesResult) {
            this.props.Description.citiesResult.feelStateData.map(function (item, index) {
                var data = {
                    value: item.stateId,
                    label: item.stateName + " - " + item.countryName
                };
                states.push(data);
            });
        }

        if (this.props.countryType.countryTypes) {
            this.props.countryType.countryTypes.countries.map(function (item, index) {
                var data = {
                    value: index,
                    label: item.name
                };
                countries.push(data);
            });
        }

        return (
            <div className="card border-0 right-cntent-area pb-5 bg-light">
                <div className="card-body bg-light p-0">
                    <div>
                        <nav className="pb-4">
                            <div className="nav justify-content-center text-uppercase fee-tab" id="nav-tab" role="tablist">
                                <a onClick={this.onCityClick} className={cityClassName} id="nav-Details-tab" data-toggle="tab" href="#nav-Details" role="tab" aria-controls="nav-Details">CITY</a>
                                <a onClick={this.onStateClick} className={stateClassName} id="nav-Details-tab" data-toggle="tab" href="#nav-Details" role="tab" aria-controls="nav-Details">STATE</a>
                                <a onClick={this.onCountryClick} className={countryClassName} id="nav-Inventory-tab" data-toggle="tab" href="#nav-Inventory" role="tab" aria-controls="nav-Inventory">COUNTRY</a>
                            </div>
                        </nav>

                        <div className="tab-content bg-white rounded shadow-sm pt-3" id="nav-tabContent">
                            <div
                                className={cityTabClassName}
                                id="nav-Details"
                                role="tabpanel"
                                aria-labelledby="nav-Details-tab"
                            >
                                <form onSubmit={this.saveDescriptionData} className="d-block w-100 mt-3 pb-3 mb-3">
                                    <div className="col-sm-12">
                                        <div className="form-group">
                                            <label>
                                                Select city{" "}
                                                <span className="text-danger">*</span>{" "}
                                            </label>
                                            <Select
                                                required
                                                options={cities}
                                                placeholder="Select city"
                                                onChange={this.handleCity}
                                                value={this.state.selectedCity}
                                            />
                                        </div>
                                        <div className="form-group">
                                            <label>
                                                Description{" "}
                                                <span className="text-danger">*</span>{" "}
                                            </label>
                                            <CKEditor
                                                isReadOnly={true}
                                                activeClass="p10"
                                                content={this.state.cityDescription}
                                                events={{
                                                    "change": this.onCityDescChange.bind(this),
                                                }}
                                            />
                                        </div>
                                    </div>
                                    <div className="text-center pb-4">
                                        <button type="submit" className="btn btn-outline-primary">Submit</button>
                                    </div>
                                </form>
                            </div>
                            <div
                                className={stateTabClassName}
                                id="nav-Details"
                                role="tabpanel"
                                aria-labelledby="nav-Details-tab"
                            >
                                <form onSubmit={this.saveDescriptionData} className="d-block w-100 mt-3 pb-3 mb-3">
                                    <div className="col-sm-12">
                                        <div className="form-group">
                                            <label>
                                                Select state{" "}
                                                <span className="text-danger">*</span>{" "}
                                            </label>
                                            <Select
                                                required
                                                options={states}
                                                placeholder="Select state"
                                                onChange={this.handleState}
                                                value={this.state.selectedState}
                                            />
                                        </div>
                                        <div className="form-group">
                                            <label>
                                                Description{" "}
                                                <span className="text-danger">*</span>{" "}
                                            </label>
                                            <CKEditor
                                                isReadOnly={true}
                                                activeClass="p10"
                                                content={this.state.stateDescription}
                                                events={{
                                                    "change": this.onStateDescChange.bind(this),
                                                }}
                                            />
                                        </div>
                                    </div>
                                    <div className="text-center pb-4">
                                        <button type="submit" className="btn btn-outline-primary">Submit</button>
                                    </div>
                                </form>
                            </div>
                            <div className={countryTabClassName} id="nav-Inventory" role="tabpanel" aria-labelledby="nav-Inventory-tab">
                                <form onSubmit={this.saveDescriptionData} className="d-block w-100 mt-3 pb-3 mb-3">
                                    <div className="col-sm-12">
                                        <div className="form-group">
                                            <label>
                                                Select country{" "}
                                                <span className="text-danger">*</span>{" "}
                                            </label>
                                            <Select
                                                required
                                                options={countries}
                                                placeholder="Select country"
                                                onChange={this.handleCountry}
                                                value={this.state.selectedCountry}
                                            />
                                        </div>
                                        <div className="form-group">
                                            <label>
                                                Description{" "}
                                                <span className="text-danger">*</span>{" "}
                                            </label>
                                            <CKEditor
                                                isReadOnly={true}
                                                activeClass="p10"
                                                content={this.state.countryDescription}
                                                events={{
                                                    "change": this.onCountryDescChange.bind(this),
                                                }}
                                            />
                                        </div>
                                    </div>
                                    <div className="text-center pb-4">
                                        <button type="submit" className="btn btn-outline-primary">Submit</button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div >
                </div>
            </div>)
    }

    @autobind
    private getDescriptionData(e) {
        var city = this.state.isCityDescription ? this.state.selectedCity.label.split(" - ") : null;
        const descriptionModel: DescriptionInputViewModel = {
            description: this.state.isCityDescription ? this.state.cityDescription : this.state.isStateDescription ? this.state.stateDescription : this.state.countryDescription,
            isCountryDescription: this.state.isCountryDescription,
            isStateDescription: this.state.isStateDescription,
            isCityDescription: this.state.isCityDescription,
            stateId: this.state.isStateDescription ? this.state.selectedState.value : 0,
            name: this.state.isCityDescription ? city[0] : this.state.isStateDescription ? this.state.selectedState.label : this.state.selectedCountry.label
        }
        return descriptionModel;
    }

    @autobind
    private saveDescriptionData(e) {
        e.preventDefault();
        var data = this.getDescriptionData(e);
        this.props.saveDescription(data, (response: DescriptionResponseViewModel) => {
            if (response.success) {
                alert("Description saved successfully!!");
            }
        });
    }
}

export default connect(
    (state: IApplicationState) => ({ Description: state.Description, countryType: state.countryType }),
    (dispatch) => bindActionCreators({ ...DescriptionSrore.actionCreators, ...CountryStore.actionCreators }, dispatch)
)(Descriptions);