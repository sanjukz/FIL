import { UserViewModel } from "./UserViewModel";
import { ReportingUserViewModel } from "./ReportingUserViewModel";

export class SessionViewModel {
    id: string;
    isAuthenticated: boolean;
    success: boolean;
    user: UserViewModel;
    reportingUser: ReportingUserViewModel;
    expirationSecondsRemaining: number;
    isFeelExists?: boolean;
    intercomHash?: string;
}
