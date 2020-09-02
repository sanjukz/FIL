import { SessionViewModel } from "./SessionViewModel";
import { UserViewModel } from "./UserViewModel"


export class LoginWithOTPResponseModel {
    phoneCode: string;
    phoneNumber: string;
    isAdditionalFieldsReqd?: boolean;
    emailAlreadyRegistered?: boolean;
    success: boolean;
    session: SessionViewModel;
    user: UserViewModel;
}
