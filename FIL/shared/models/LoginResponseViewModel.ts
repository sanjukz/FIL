import { SessionViewModel } from "./SessionViewModel";

export class LoginResponseViewModel {
    success: boolean;
    session: SessionViewModel;
    isLockedOut?: boolean;
    requiresTwoFactor?: boolean;
    isNotAllowed?: boolean;
    isActivated?: boolean;
    isActiveFeel?: boolean;
}
