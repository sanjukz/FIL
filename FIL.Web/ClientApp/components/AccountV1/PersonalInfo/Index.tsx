import * as React from 'react';
import { gets3BaseUrl } from "../../../utils/imageCdn";
import { RouteComponentProps } from "react-router-dom";
import * as AccountStore from "../../../stores/Account";
import FilLoader from "../../../components/Loader/FilLoader";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import UserProfileResponseViewModel, { UserProfile } from '../../../models/Account/UserProfileResponseViewModel';
import {
    IPersonalInfo,
    defaultValidateFirstName, defaultValidateLastName, defaultValidatePhoneNumber, defaultValidatePhoneCode,
    defaultValidateDOBNumber
} from '../../../models/Account/ValidationResponse';
import FormDetails from "./FormDetails";
import { MobileExistModel } from '../../../models/Account/MobileExistModel';
import BreadcrumbAndTitle from '../BreadcrumbAndTitle';
import { ValidateFields } from "../../../utils/MyAccount/Validation";
import { message } from 'antd';

type AccountProps = AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;

export default class PersonalInfo extends React.PureComponent<AccountProps, any> {
    constructor(props) {
        super(props);
        this.state = {
            gets3BaseUrl: gets3BaseUrl(),
            user: {
                id: 0,
                altId: "",
                userName: "",
                firstName: "",
                lastName: "",
                email: "",
                dob: "",
                gender: "",
                phoneCode: "",
                phoneNumber: ""
            },
            validateResponse: null,
            phoneCodeOptions: null,
            isMobileValid: true,
            showOtp: false
        }
    }

    componentDidMount = () => {
        if (this.props.session.isAuthenticated) {
            if (this.props.account.userProfile.userProfile.id == null) {
                this.requestUserDetails();
                this.props.requestCountryData();
            } else if (this.props.account.userProfile.userProfile.id != null
                || this.props.account.userProfile.userProfile.id != 0) {
                if (this.props.account.userProfile.userProfile.altId.toLocaleLowerCase()
                    != this.props.session.user.altId.toLocaleLowerCase()) {
                    this.requestUserDetails();
                    this.props.requestCountryData();
                } else {
                    this.setValues(this.props.account.userProfile.userProfile);
                }
            }
            else {
                this.setValues(this.props.account.userProfile.userProfile);
            }
        }
        let validationResponse: IPersonalInfo = {
            firstName: defaultValidateFirstName,
            lastName: defaultValidateLastName,
            phoneCode: defaultValidatePhoneCode,
            phoneNumber: defaultValidatePhoneNumber,
            dob: defaultValidateDOBNumber
        }
        this.setState({ validateResponse: validationResponse })
    }

    componentDidUpdate = (nextProps) => {
        if (nextProps.session.isAuthenticated !== this.props.session.isAuthenticated) {
            this.props.requestCountryData();
            this.requestUserDetails();
        }
        if ((nextProps.account.countryList.countries != this.props.account.countryList.countries) ||
            (this.props.account.countryList.countries && this.props.account.countryList.countries.length > 0
                && !this.state.phoneCodeOptions)) {
            this.getPhoneCodeOptions(this.props.account.countryList);
        }
    }

    requestUserDetails = () => {
        let userProfile: UserProfile = {
            altId: this.props.session.user.altId
        }
        let user: UserProfileResponseViewModel = {
            userProfile: userProfile
        };
        this.getOrUpdateAction(user);
    }

    setValues = (userProfile: UserProfile) => {
        const { user } = this.state;
        user.dob = this.getFormatedDate(user);
        this.setState({ user: userProfile })
    }

    onChangeValue = (e) => {
        const { validateResponse } = this.state;
        let user = Object.assign({}, this.state.user);
        if (e._isAMomentObject) {                 //For DOB
            user.dob = e._d;
        } else {
            user[e.target.name] = e.target.value;
            let validationResponse = ValidateFields(user, validateResponse, e.target.name);
            this.setState({
                validateResponse: validationResponse, user: user,
                isMobileValid: true
            })
        }
        this.setState({ user: user });
    }

    onSaveValue = (name) => {
        if (this.shouldSave()) {
            if (name == "mobile") {
                let user = Object.assign({}, this.state.user);

                let mobileUser: MobileExistModel = {
                    phoneCode: user.phoneCode,
                    phoneNumber: user.phoneNumber
                }
                let validationResponse = ValidateFields(user, this.state.validateResponse, "phoneNumber");
                this.setState({ validateResponse: validationResponse, user: user })
                if (!validationResponse.phoneCode.isInValid && !validationResponse.phoneNumber.isInValid) {
                    this.props.checkMobileExists(mobileUser, (response: MobileExistModel) => {
                        if (response.isExist) {
                            this.setState({ isMobileValid: false, showOtp: false })
                        } else {
                            this.setState({ isMobileValid: true, showOtp: true })
                        }
                    })
                }
            } else {
                let isFormValid = true;
                let stateUser = this.state.user;
                stateUser.dob = this.getFormatedDate(this.state.user);
                let userProfileModel: UserProfileResponseViewModel = {
                    userProfile: stateUser
                };
                if (name == "fullName") {
                    let validationResponse = ValidateFields(stateUser, this.state.validateResponse, "firstName");
                    this.setState({ validateResponse: validationResponse, user: stateUser })
                    if (validationResponse.firstName.isInValid || validationResponse.lastName.isInValid) {
                        isFormValid = false;
                    }
                }
                if (isFormValid) {
                    this.getOrUpdateAction(userProfileModel, true);
                }
            }
        }
    }

    showSuccessAlert = () => {
        if (!this.state.isMobileValid) {
            message.error({
                content: 'Failed to save, please try again',
                duration: 5,
                className: 'mt-5'
            })
        } else {
            message.success({
                content: 'Updated successfully',
                duration: 5,
                className: 'mt-5'
            })
        }
    }

    getOrUpdateAction = (user: UserProfileResponseViewModel, isUpdate?: boolean) => {
        this.props.updateUserDetails(user, (response: UserProfileResponseViewModel) => {
            this.setValues(response.userProfile);
            if (isUpdate && (response.userProfile.id != null || response.userProfile.id != 0)) {
                this.showSuccessAlert();
            }
        });
    }

    getPhoneCodeOptions = (countriesOptions) => {
        if (countriesOptions && countriesOptions.countries) {
            let phoneCodeOptions = countriesOptions.countries
                .filter(item => item.phonecode && item.name)
                .map(item => {
                    return <>
                        <option value={item.phonecode + "~" + item.altId}>
                            {item.name + " (" + item.phonecode + ")"}
                        </option>
                    </>
                });
            this.setState({ phoneCodeOptions: phoneCodeOptions })
        }
    }

    getFormatedDate = (user: UserProfile) => {
        if (!user.dob) {
            return "";
        }
        let date = new Date(user.dob);
        return `${String("0" + date.getDate()).slice(-2)}/${String("0" + (date.getMonth() + 1)).slice(-2)}/${date.getFullYear()}`
    }
    shouldSave = () => {
        const { user } = this.state;
        let userProps = this.props.account.userProfile.userProfile;
        if (user.firstName != userProps.firstName || user.lastName !== userProps.lastName
            || user.gender != userProps.gender || user.dob != userProps.dob ||
            user.phoneCode != userProps.phoneCode || user.phoneNumber != userProps.phoneNumber) {
            return true;
        } else {
            return false;
        }
    }
    render() {
        const { gets3BaseUrl, user, phoneCodeOptions, isMobileValid, showOtp, validateResponse } = this.state;

        if (this.props.account.isLoadingUserProfile || !this.props.session.isAuthenticated
            || this.props.account.isLoadingCountryList || this.props.account.isLoading) {
            return <FilLoader />
        }

        return (<>
            <div className="container">

                <BreadcrumbAndTitle title={'Personal info'} />

                <FormDetails
                    gets3BaseUrl={gets3BaseUrl}
                    user={user}
                    onChangeValue={(e) => this.onChangeValue(e)}
                    onSaveValue={(name) => this.onSaveValue(name)}
                    phoneCodeOptions={phoneCodeOptions}
                    {...this.props}
                    isMobileValid={isMobileValid}
                    showOtp={showOtp}
                    hideOtp={() => this.setState({ showOtp: false })}
                    getFormatedDate={(user) => this.getFormatedDate(user)}
                    validateResponse={validateResponse}
                />
            </div>
        </>)
    }
}