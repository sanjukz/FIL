class validateResponses {
    isInvalid: boolean;
}
export interface IValidateResponse {
    isInValid?: boolean;
    message?: string;
    customValidations?: validateResponses[];
}

export interface ILoginValidationResponse {
    email: IValidateResponse
    password: IValidateResponse
}


export const defaultValidateEmail: IValidateResponse = {
    isInValid: false,
    message: null
}


//This is to done to handle custom validation
let validation = [];
for (let i = 0; i < 5; i++) {
    let tempData = {
        IsInvalid: false
    }
    validation.push(tempData);
}
export const defaultValidatePassword: IValidateResponse = {
    isInValid: false,
    message: null,
    customValidations: validation
}
export const defaultValidateConfirmPassword: IValidateResponse = {
    isInValid: false,
    message: null
}
export const defaultValidateOldPassword: IValidateResponse = {
    isInValid: false,
    message: null
}
export const defaultValidateFirstName: IValidateResponse = {
    isInValid: false,
    message: null
}
export const defaultValidateLastName: IValidateResponse = {
    isInValid: false,
    message: null
}
export const defaultValidatePhoneCode: IValidateResponse = {
    isInValid: false,
    message: null
}
export const defaultValidatePhoneNumber: IValidateResponse = {
    isInValid: false,
    message: null
}
export interface ISignUpValidationResponse {
    firstName: IValidateResponse,
    lastName: IValidateResponse,
    email: IValidateResponse,
    password: IValidateResponse,
    confirmPassword: IValidateResponse,
    phoneCode: IValidateResponse,
    phoneNumber: IValidateResponse
}


export interface IResetPasswordValidationResponse {
    confirmPassword: IValidateResponse
    password: IValidateResponse,
    oldPassword?: IValidateResponse
}

export interface IOTPLoginValidationResponse {
    phoneCode: IValidateResponse,
    phoneNumber: IValidateResponse,
    firstName: IValidateResponse,
    lastName: IValidateResponse,
    email: IValidateResponse
}