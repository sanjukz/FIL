import * as React from "react";
import { RouteComponentProps } from "react-router-dom";
import { Subheader } from "../components/Subheader";
import { Sidebar } from "../components/Sidebar";
import { CountryComponent } from "../components/CountryComponent";
import { StateComponent } from "../components/StateComponent";
import { CityComponent } from "../components/CityComponent";
import { ZipcodeComponent } from "../components/ZipcodeComponent";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import * as IVenueComponentState from "../stores/VenueData";
import "shared/styles/globalStyles/main.scss";
import "./EventTicket.scss";
import CountryCreationform from "../components/form/CountryCreationform";
import StateCreationform from "../components/form/StateCreationform";
import CityCreationform from "../components/form/CityCreationform";
import ZipcodeCreationform from "../components/form/ZipcodeCreationform";
import VenueCreationform from "../components/form/VenueCreationComponent";
import { CountryFormDataViewModel } from "../models/CountryFormDataViewModel";
import { VenueCreationFormDataviewModel } from "../models/VenueCreationFormDataviewModel";
import { autobind } from "core-decorators";
import { StateFormDataViewModel } from "../models/StateFormDataViewModel";
import { CityFormDataViewModel } from "../models/CityFormDataViewModel";
import { ZipcodeFormDataViewModel } from "../models/ZipcodeFormDataViewModel";


type VenueComponentStateProps = IVenueComponentState.IVenueComponentState & typeof IVenueComponentState.actionCreators & RouteComponentProps<{}>;;

const headerNavButtonsData = [{
    ButtonName: "Handover Sheet"
}, {
    ButtonName: "Seat Layout"
}, {
    ButtonName: "Allocation Manager"
}];

export class VenueCreation extends React.Component<VenueComponentStateProps, IVenueComponentState.IVenueComponentState> {
    constructor(props) {
        super(props);
        this.changeCountryVal = this.changeCountryVal.bind(this);
        this.changeStateVal = this.changeStateVal.bind(this);
        this.changeCityVal = this.changeCityVal.bind(this);
        this.changeZipcodeVal = this.changeZipcodeVal.bind(this);
        this.toggleCountry = this.toggleCountry.bind(this);
        this.toggleState = this.toggleState.bind(this);
        this.toggleCity = this.toggleCity.bind(this);
        this.toggleZipcode = this.toggleZipcode.bind(this);
    }
    public componentWillMount() {
        this.props.requestCountryData();
        this.changeCountryVal("");
        this.changeStateVal("");
        this.changeCityVal("");
        this.changeZipcodeVal("");
        this.setState({ isHiddenCountry: true });
        this.setState({ isHiddenState: true });
        this.setState({ isHiddenCity: true });
        this.setState({ isHiddenZip: true });
    }
    public changeCountryVal(e) {
        this.setState({ valCountry: e });
        //this.props.requestStateData(e.id)
    }
    public changeStateVal(e) {
        this.setState({ valState: e });
        // this.props.requestCityData(e.id)
    }
    public changeCityVal(e) {
        this.setState({ valCity: e });
        //this.props.requestZipdata(e.id)
    }
    public changeZipcodeVal(e) {
        this.setState({ valZip: e });
    }

    toggleCountry() {
        this.setState({
            isHiddenCountry: !this.state.isHiddenCountry
        });
    }

    toggleState() {
        this.setState({
            isHiddenState: !this.state.isHiddenState
        });
    }

    toggleCity() {
        this.setState({
            isHiddenCity: !this.state.isHiddenCity
        });
    }

    toggleZipcode() {
        this.setState({
            isHiddenZip: !this.state.isHiddenZip
        });
    }

    render() {
        const countries = this.props.countries.countries;
        //const states = this.props.states.states;
        //const cities = this.props.cities.cities;
        //const zipcode = this.props.zipcodes.zipcodes;
        return <div>
            <Subheader breadcrumbName={"Venue"} breadcrumbDetail={"VenueManagement"} headerNavButtonsData={headerNavButtonsData} />
            <div className="wrapper">
                <Sidebar />
                <div id="content">

                    <CountryComponent /*options={countries}*/ selected={this.state.valCountry} changeValue={this.changeCountryVal} />
                    <div>
                        <button onClick={this.toggleCountry.bind(this)} >
                            Create country
                        </button>
                        {!this.state.isHiddenCountry && <CountryCreationform onSubmit={this.onSubmitCountryCreation} />}
                    </div>

                    <StateComponent selected={this.state.valState} changeValue={this.changeStateVal} />
                    <div>
                        <button onClick={this.toggleState.bind(this)} >
                            Create State
                        </button>
                        {!this.state.isHiddenState && <StateCreationform onSubmit={this.onSubmitStateCreation} />}
                    </div>

                    <CityComponent selected={this.state.valCity} changeValue={this.changeCityVal} />
                    <div>
                        <button onClick={this.toggleCity.bind(this)} >
                            Create City
                        </button>
                        {!this.state.isHiddenCity && <CityCreationform onSubmit={this.onSubmitCityCreation} />}
                    </div>

                    <ZipcodeComponent selected={this.state.valZip} changeValue={this.changeZipcodeVal} />
                    <div>
                        <button onClick={this.toggleZipcode.bind(this)} >
                            Create Zipcode
                        </button>
                        {!this.state.isHiddenZip && <ZipcodeCreationform onSubmit={this.onSubmitZipcodeCreation} />}
                    </div>
                    <VenueCreationform onSubmit={this.onSubmitVenueCreation} />
                </div>
            </div>
        </div>;
    }
    @autobind
    private onSubmitCountryCreation(values: CountryFormDataViewModel) {
        this.props.addCountry(values, (response) => {
            if (response.success) {
                this.props.history.replace("/venuecreation");
            }
            else {
                alert("Country already present");
            }
        });
    }

    @autobind
    private onSubmitStateCreation(values: StateFormDataViewModel) {
        values.countryId = this.state.valCountry.id;
        this.props.addState(values, (response) => {
            if (response.success) {
                this.props.history.replace("/venuecreation");
            }
            else {
                alert("State already present");
            }
        });

    }

    @autobind
    private onSubmitCityCreation(values: CityFormDataViewModel) {
        values.stateId = this.state.valState.id;
        this.props.addCity(values, (response) => {
            if (response.success) {
                this.props.history.replace("/venuecreation");
            }
            else {
                alert("City already present");
            }
        });
    }

    @autobind
    private onSubmitZipcodeCreation(values: ZipcodeFormDataViewModel) {
        values.cityId = this.state.valCity.id;
        this.props.addZipcode(values, (response) => {
            if (response.success) {
                this.props.history.replace("/venuecreation");
            }
            else {
                alert("Venue already present");
            }
        });
    }

    @autobind
    private onSubmitVenueCreation(values: VenueCreationFormDataviewModel) {
        values.cityId = this.state.valCity.id;
        this.props.addVenue(values, (response) => {
            if (response.success) {
                this.props.history.replace("/venuemanager");
            }
        });
    }
}

export default connect(
    (state: IApplicationState) => state.venues,
    IVenueComponentState.actionCreators
)(VenueCreation);
