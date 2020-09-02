export default class UserProfileResponseViewModel {
  userProfile: UserProfile;
}

export class UserProfile {
  id?: number;
  altId: string;
  userName?: string;
  firstName?: string;
  lastName?: string;
  email?: string;
  dob?: string;
  gender?: string;
  phoneCode?: string;
  phoneNumber?: string;
  signupmethod?: number;
}
