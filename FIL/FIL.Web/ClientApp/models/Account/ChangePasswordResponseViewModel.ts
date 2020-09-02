import { UserProfile } from "./UserProfileResponseViewModel";

export class ChangePasswordResponseViewModel {
    success: boolean;
    wrongPassword: boolean;
    profile: UserProfile;
}
