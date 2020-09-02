import { SessionViewModel } from "shared/models/SessionViewModel";

export class GoogleSignInResponseViewModel  { 
    success: boolean;
    session: SessionViewModel;
    isLockedOut?: boolean;
    requiresTwoFactor?: boolean;
    isNotAllowed?: boolean;
  }
