import { isEmpty, isAlpha } from 'validator';
import { IPersonalInfo } from "../../models/Account/ValidationResponse";
import { UserProfile } from '../../models/Account/UserProfileResponseViewModel';


export const ValidateFields = (user: UserProfile, responseModel: IPersonalInfo, target: string, isSubmit?: boolean) => {
    user.firstName = user.firstName.trim();
    user.lastName = user.lastName.trim();

    if (target == "firstName" || target == "lastName")
        //First Name & Last Name 
        if (isEmpty(user.firstName)) {
            responseModel.firstName.isInValid = true;
            responseModel.firstName.message = "First Name is required";
        }
        else if (!isAlpha(user.firstName)) {
            responseModel.firstName.isInValid = true;
            responseModel.firstName.message = "Please use valid characters for your name.";
        } else {
            responseModel.firstName.isInValid = false;
        }
    if (isEmpty(user.lastName)) {
        responseModel.lastName.isInValid = true;
        responseModel.lastName.message = "Last Name is required";
    }
    else if (!isAlpha(user.lastName)) {
        responseModel.lastName.isInValid = true;
        responseModel.lastName.message = "Please use valid characters for your last name.";
    } else {
        responseModel.lastName.isInValid = false;
    }


    //phone code and phoneNumber     here phone code is of format {phoneCode}~{altId}
    if (isEmpty(user.phoneCode)) {
        responseModel.phoneCode.isInValid = true;
        responseModel.phoneCode.message = "Phone Code is required";
    } else {
        responseModel.phoneCode.isInValid = false;
    }
    let options = {
        strictMode: true,
    }
    let splitPhoneCode = user.phoneCode.split("~");
    let phoneNumberWithCode = "+" + splitPhoneCode[0] + user.phoneNumber;
    if (isEmpty(user.phoneNumber)) {
        responseModel.phoneNumber.isInValid = true;
        responseModel.phoneNumber.message = "Mobile Number is required";
    }
    // else if (!isMobilePhone(phoneNumberWithCode, 'any', options)) {
    //     responseModel.phoneNumber.isInValid = true;
    //     responseModel.phoneNumber.message = "Please use valid phone number.";
    // }
    else {
        responseModel.phoneNumber.isInValid = false;
    }

    //dob
    if (target == "dob") {
        if (isEmpty(user.dob)) {
            responseModel.dob.isInValid = true;
            responseModel.dob.message = "Please select date";

        } else {
            responseModel.dob.isInValid = false;
        }
    }

    return responseModel;
}