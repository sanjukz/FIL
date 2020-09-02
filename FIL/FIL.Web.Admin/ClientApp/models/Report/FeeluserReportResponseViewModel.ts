
export default class FeelUserReportResponseViewModel {
  feelUsers: FeelUserReport[];
};

export class FeelUserReport {
  firstName: string;
  lastName: string;
  email: string;
  phoneCode: string;
  phoneNumber: string;
  createdDate: string;
  ipAddress: string;
  ipCity: string;
  ipCountry: string;
  ipState: string;
  signUpMethod: string;
  isTransacted?: boolean;
  isOptIn?: string;
  isCreator?: string;
};
