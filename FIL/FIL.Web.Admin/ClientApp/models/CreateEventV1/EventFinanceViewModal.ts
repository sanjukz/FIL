// This file was generated from the Models.tst template
//

import CurrencyType from "../Common/CurrencyTypeViewModel";

export interface EventFinanceViewModel {
  eventId: number;
  success: boolean;
  isValidLink: boolean;
  isDraft: boolean;
  stripeAccount: number;
  stripeConnectAccountId: string;
  isoAlphaTwoCode?: string;
  eventAltId?: string;
  eventFinanceDetailModel?: EventFinanceDetailModel;
  currencyType?: CurrencyType;
  currentStep?: number;
  completedStep?: string;
}

export interface EventFinanceDetailModel {
  id: number;
  accountTypeId: number;
  firstName: string;
  lastName: string;
  email: string;
  companyName: string;
  phoneCode: number;
  phoneNumber: string;
  accountName: string;
  bankName: string;
  branchCode: string;
  accountNumber: string;
  stateId: number;
  taxId: string;
  currenyId: number;
  countryId: number;
}
