import { MasterEventTypes } from '../../Enums/MasterEventTypes';

export default class MyFeelResponseViewModel {
  success: boolean;
  myFeels: MyFeel[];
}

export class MyFeel {
  id: number;
  name: string;
  altId: string;
  slug: string;
  isEnabled: boolean;
  isTokenize?: boolean;
  isPastEvent: boolean;
  isShowExclamationIcon: boolean;
  completedStep: string;
  currentStep: string;
  eventStatusId: number
  eventStartDateTime: string;
  eventEndDateTime: string;
  eventStartDateTimeString: string;
  eventEndDateTimeString: string;
  timeZoneAbbrivation: string;
  timeZoneOffset: string;
  subCategory: string;
  eventUrl: string;
  parentCategory: string;
  ticketForSale: number
  soldTicket: number;
  userEmail: string;
  roleId: number;
  eventCreatedDateTime: string;
  masterEventType: MasterEventTypes;
}
