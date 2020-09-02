class validateResponses {
    isInvalid: boolean;
}
export interface IValidateResponse {
    isInValid?: boolean;
    message?: string;
    customValidations?: validateResponses[];
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
export const defaultValidateDOBNumber: IValidateResponse = {
    isInValid: false,
    message: null
}
export interface IPersonalInfo {
    firstName?: IValidateResponse
    lastName?: IValidateResponse
    phoneCode?: IValidateResponse,
    phoneNumber?: IValidateResponse
    dob?: IValidateResponse
}