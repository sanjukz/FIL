import { isEmail, isEmpty, isAlpha } from 'validator';
import { SignInUserModel, SignUpUserModel, ForgetPaswordModel } from "../../models/Auth/UserModel";
import { ILoginValidationResponse, ISignUpValidationResponse, IResetPasswordValidationResponse, IOTPLoginValidationResponse } from "../../models/Auth/ValidationResponse";
import { SendAndValidateOTPFormModel } from "shared/models/SendAndValidateOTPFormModel";
import { LoginWithOTPFormModel } from 'shared/models/LoginWithOTPFormModel';

//Export this to validate login form at submit

export const ValidateLoginForm = (user: SignInUserModel, responseModel: ILoginValidationResponse, isSubmit: boolean) => {
    user.email = user.email.trim();
    user.password = user.password.trim();


    //For Email 
    if (isSubmit || !isEmpty(user.email)) {
        if (isEmpty(user.email)) {
            responseModel.email.isInValid = true;
            responseModel.email.message = "Email is required";
        } else {
            if (!isEmail(user.email)) {
                responseModel.email.isInValid = true;
                responseModel.email.message = "Enter a valid email";
            } else {
                responseModel.email.isInValid = false;
            }
        }
    }
    //For Password 
    if (isSubmit || !isEmpty(user.password)) {
        if (isEmpty(user.password)) {
            responseModel.password.isInValid = true;
            responseModel.password.message = "Password is required";
        } else {
            // if (user.password.length < 8) {
            //     responseModel.password.isInValid = true;
            //     responseModel.password.message = "Your password must be atleast 8 characters.";
            // } else {
            //     responseModel.password.isInValid = false;
            // }
            responseModel.password.isInValid = false;
        }
    }
    return responseModel;
}



// Validation for signUp starts here...

export const ValidateSignUpForm = (user: SignUpUserModel, responseModel: ISignUpValidationResponse,
    isSubmit: boolean) => {

    user.email = user.email.trim();
    user.firstName = user.firstName.trim();
    user.lastName = user.lastName.trim();
    user.password = user.password.trim();
    user.confirmPassword = user.confirmPassword.trim();
    user.phoneNumber = user.phoneNumber.trim();
    user.phoneCode = user.phoneCode.trim();

    //First & Last Name
    if (isSubmit || !isEmpty(user.firstName)) {
        if (isEmpty(user.firstName)) {
            responseModel.firstName.isInValid = true;
            responseModel.firstName.message = "First Name is required";
        }
        else if (!isAlpha(user.firstName)) {
            responseModel.firstName.isInValid = true;
            responseModel.firstName.message = "Please use valid characters for your name.";
        }
        else {
            responseModel.firstName.isInValid = false;
        }
    }
    if (isSubmit || !isEmpty(user.lastName)) {
        if (isEmpty(user.lastName)) {
            responseModel.lastName.isInValid = true;
            responseModel.lastName.message = "Last Name is required";
        }
        else if (!isAlpha(user.lastName)) {
            responseModel.lastName.isInValid = true;
            responseModel.lastName.message = "Please use valid characters for your name.";
        }
        else {
            responseModel.lastName.isInValid = false;
        }
    }

    //Email 
    if (isSubmit || !isEmpty(user.email)) {
        if (isEmpty(user.email)) {
            responseModel.email.isInValid = true;
            responseModel.email.message = "Email is required";
        } else {
            if (!isEmail(user.email)) {
                responseModel.email.isInValid = true;
                responseModel.email.message = "Enter a valid email";
            } else {
                responseModel.email.isInValid = false;
            }
        }
    }

    //phone code and phoneNumber     here phone code is of format {phoneCode}~{altId}
    if (isSubmit || !isEmpty(user.phoneCode)) {
        if (isEmpty(user.phoneCode)) {
            responseModel.phoneCode.isInValid = true;
            responseModel.phoneCode.message = "Phone Code is required";
        } else {
            responseModel.phoneCode.isInValid = false;
        }
    }
    if (isSubmit || !isEmpty(user.phoneNumber)) {
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
    }

    //For Password

    if (isSubmit || !isEmpty(user.password)) {
        if (isEmpty(user.password)) {
            responseModel.password.isInValid = true;
            responseModel.password.message = "Password is required";
        } else {

            //Check for name or email address in password =>Custom Validation Array Index :1
            if (user.password.toLowerCase().includes(user.firstName.toLowerCase())
                || user.password.toLowerCase().includes(user.lastName.toLowerCase())
                || user.password.toLowerCase().includes(user.email.toLowerCase().split('@')[0])) {
                responseModel.password.isInValid = true;
                responseModel.password.customValidations[1].isInvalid = true;
            } else {
                responseModel.password.customValidations[1].isInvalid = false;

            }

            //Check atleast Character lenght 8 =>  Custom Validation Array Index :2
            if (user.password.length < 8) {
                responseModel.password.isInValid = true;
                responseModel.password.customValidations[2].isInvalid = true;
            } else {
                responseModel.password.customValidations[2].isInvalid = false;
            }

            //Check contains a number or symbol => Custom Validation Array Index :3
            if (!hasSpecial(user.password) && !hasNumber(user.password)) {
                responseModel.password.isInValid = false;
                responseModel.password.customValidations[3].isInvalid = true;
            } else {
                responseModel.password.customValidations[3].isInvalid = false;
            }

            //Check for uppercase and lower case => Custom Validation Array Index :4
            if (!hasLowerCase(user.password) || !hasUpperCase(user.password)) {
                responseModel.password.isInValid = false;
                responseModel.password.customValidations[4].isInvalid = true;
            } else {
                responseModel.password.customValidations[4].isInvalid = false;
            }

            //Check for password strenght => Custom Validation Array Index :0
            responseModel.password.customValidations[0].isInvalid = true;
            let filterData = responseModel.password.customValidations.filter((item) => {
                return item.isInvalid == false;
            })

            if (filterData.length >= 4) {      //For all conditions passed
                responseModel.password.isInValid = false;
                responseModel.password.customValidations[0].isInvalid = false;
            } else {
                responseModel.password.customValidations[0].isInvalid = true;
                responseModel.password.isInValid = true;
            }
        }
    }

    //For Confirm Password
    if (isSubmit || !isEmpty(user.confirmPassword)) {
        if (isEmpty(user.confirmPassword)) {
            responseModel.confirmPassword.isInValid = true;
            responseModel.confirmPassword.message = "Confirm password is required";
        }
        else if (user.password !== user.confirmPassword) {
            responseModel.confirmPassword.isInValid = true;
            responseModel.confirmPassword.message = "Password must match";
        } else {
            responseModel.confirmPassword.isInValid = false;
        }

    }

    return responseModel;
}



//Call this to initialize for default values for login
export const GetDefaultLoginValidationValues = (responseModel: ILoginValidationResponse) => {

    responseModel.email.isInValid = false;
    responseModel.password.isInValid = false;
    return responseModel;
}

//Call this to initialize for default values for sign up
export const GetDefaultSignupValidationValues = (responseModel: ISignUpValidationResponse) => {
    if (responseModel != null) {
        responseModel.email.isInValid = false;
        responseModel.password.isInValid = false;
        responseModel.firstName.isInValid = false;
        responseModel.lastName.isInValid = false;
        responseModel.phoneNumber.isInValid = false;
        responseModel.phoneCode.isInValid = false;
        responseModel.confirmPassword.isInValid = false;
    }
    else {
        let tempModel = {
            isInValid: false,
            message: ""
        }
        responseModel = {
            firstName: tempModel,
            lastName: tempModel,
            confirmPassword: tempModel,
            email: tempModel,
            password: tempModel,
            phoneCode: tempModel,
            phoneNumber: tempModel
        }
    }
    return responseModel;
}
//Call this to initialize for default values for otp login
export const GetDefaultOTPValidationValues = (responseModel: IOTPLoginValidationResponse) => {

    responseModel.phoneCode.isInValid = false;
    responseModel.phoneNumber.isInValid = false;
    responseModel.firstName.isInValid = false;
    responseModel.firstName.isInValid = false;
    responseModel.email.isInValid = false;
    return responseModel;
}

export const ValidateResetPassword = (user: ForgetPaswordModel, responseModel: IResetPasswordValidationResponse, isSubmit: boolean) => {
    user.password = user.password.trim();
    user.confirmPassword = user.confirmPassword.trim();

    //For Password

    if (isSubmit || !isEmpty(user.password)) {
        if (isEmpty(user.password)) {
            responseModel.password.isInValid = true;
            responseModel.password.message = "Password is required";
        } else {

            //Check for name or email address in password =>Custom Validation Array Index :1
            if (user.password.toLowerCase().includes(user.firstName.toLowerCase())
                || user.password.toLowerCase().includes(user.lastName.toLowerCase())
                || user.password.toLowerCase().includes(user.email.toLowerCase().split('@')[0])) {
                responseModel.password.isInValid = true;
                responseModel.password.customValidations[1].isInvalid = true;
            } else {
                responseModel.password.customValidations[1].isInvalid = false;

            }

            //Check atleast Character lenght 8 =>  Custom Validation Array Index :2
            if (user.password.length < 8) {
                responseModel.password.isInValid = true;
                responseModel.password.customValidations[2].isInvalid = true;
            } else {
                responseModel.password.customValidations[2].isInvalid = false;
            }

            //Check contains a number or symbol => Custom Validation Array Index :3
            if (!hasSpecial(user.password) && !hasNumber(user.password)) {
                responseModel.password.isInValid = false;
                responseModel.password.customValidations[3].isInvalid = true;
            } else {
                responseModel.password.customValidations[3].isInvalid = false;
            }

            //Check for uppercase and lower case => Custom Validation Array Index :4
            if (!hasLowerCase(user.password) || !hasUpperCase(user.password)) {
                responseModel.password.isInValid = false;
                responseModel.password.customValidations[4].isInvalid = true;
            } else {
                responseModel.password.customValidations[4].isInvalid = false;
            }

            //Check for password strenght => Custom Validation Array Index :0
            responseModel.password.customValidations[0].isInvalid = true;
            let filterData = responseModel.password.customValidations.filter((item) => {
                return item.isInvalid == false;
            })

            if (filterData.length >= 4) {      //For all conditions passed
                responseModel.password.isInValid = false;
                responseModel.password.customValidations[0].isInvalid = false;
            } else {
                responseModel.password.customValidations[0].isInvalid = true;
                responseModel.password.isInValid = true;
                responseModel.password.message = "";
            }
        }
    }

    //For Confirm Password
    if (isSubmit || !isEmpty(user.confirmPassword)) {
        if (isEmpty(user.confirmPassword)) {
            responseModel.confirmPassword.isInValid = true;
            responseModel.confirmPassword.message = "Confirm password is required";
        }
        else if (user.password !== user.confirmPassword) {
            responseModel.confirmPassword.isInValid = true;
            responseModel.confirmPassword.message = "Password must match";
        } else {
            responseModel.confirmPassword.isInValid = false;
        }

    }

    //For Change password when we have old password
    if (user.oldPassword != undefined) {
        if (isSubmit || !isEmpty(user.oldPassword)) {
            if (isEmpty(user.oldPassword)) {
                responseModel.oldPassword.isInValid = true;
                responseModel.oldPassword.message = "Old password is required";
            } else {
                responseModel.oldPassword.isInValid = false;
            }
        }
    }

    return responseModel;

}

export const IsValidSignUp = (inputValidation: ISignUpValidationResponse) => {
    if (!inputValidation.firstName.isInValid && !inputValidation.lastName.isInValid &&
        !inputValidation.email.isInValid && !inputValidation.phoneCode.isInValid &&
        !inputValidation.phoneNumber.isInValid && !inputValidation.password.isInValid &&
        !inputValidation.confirmPassword.isInValid) {
        return true;
    }
    else {
        return false;
    }
}

//OTP Login 
export const ValidateOTPLogin = (user: SendAndValidateOTPFormModel, responseModel: IOTPLoginValidationResponse, isSubmit: boolean) => {
    user.phoneCode = user.phoneCode.trim();
    user.phoneNumber = user.phoneNumber.trim();

    //phone code and phoneNumber
    if (isSubmit || !isEmpty(user.phoneCode)) {
        if (isEmpty(user.phoneCode)) {
            responseModel.phoneCode.isInValid = true;
            responseModel.phoneCode.message = "Phone Code is required";
        } else {
            responseModel.phoneCode.isInValid = false;
        }
    }
    if (isSubmit || !isEmpty(user.phoneNumber)) {
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
    }
    return responseModel;
}

//OTP Login 
export const ValidateOTPSignUp = (user: LoginWithOTPFormModel, responseModel: IOTPLoginValidationResponse, isSubmit: boolean) => {
    user.email = user.email.trim();
    user.firstName = user.firstName.trim();
    user.lastName = user.lastName.trim();
    //First & Last Name
    if (isSubmit || !isEmpty(user.firstName)) {
        if (isEmpty(user.firstName)) {
            responseModel.firstName.isInValid = true;
            responseModel.firstName.message = "First Name is required";
        }
        else if (!isAlpha(user.firstName)) {
            responseModel.firstName.isInValid = true;
            responseModel.firstName.message = "Please use valid characters for your name.";
        }
        else {
            responseModel.firstName.isInValid = false;
        }
    }
    if (isSubmit || !isEmpty(user.lastName)) {
        if (isEmpty(user.lastName)) {
            responseModel.lastName.isInValid = true;
            responseModel.lastName.message = "Last Name is required";
        }
        else if (!isAlpha(user.lastName)) {
            responseModel.lastName.isInValid = true;
            responseModel.lastName.message = "Please use valid characters for your name.";
        }
        else {
            responseModel.lastName.isInValid = false;
        }
    }

    //Email 
    if (isSubmit || !isEmpty(user.email)) {
        if (isEmpty(user.email)) {
            responseModel.email.isInValid = true;
            responseModel.email.message = "Email is required";
        } else {
            if (!isEmail(user.email)) {
                responseModel.email.isInValid = true;
                responseModel.email.message = "Enter a valid email";
            } else {
                responseModel.email.isInValid = false;
            }
        }
    }

    return responseModel;
}








//To check if string contains symbol
function hasSpecial(str) {
    return /[~`!#$%\^&*+=\-\[\]\\';@,/{}|\\":<>\?]/g.test(str)
}

//To check if string contains number
function hasNumber(myString) {
    return /\d/.test(myString);
}

//To check if string contains lowecase character
function hasLowerCase(string) {
    return /[a-z]/.test(string)
}

//To check if string contains uppercase character
function hasUpperCase(string) {
    return /[A-Z]/.test(string)
}