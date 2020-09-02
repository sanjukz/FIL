import { SessionViewModel } from "shared/models/SessionViewModel";

export class FacebookSignInResponseViewModel {
  success: boolean;
  session: SessionViewModel;
  isLockedOut?: boolean;
  requiresTwoFactor?: boolean;
  isNotAllowed?: boolean;
  isEmailReqd?: boolean;
}
