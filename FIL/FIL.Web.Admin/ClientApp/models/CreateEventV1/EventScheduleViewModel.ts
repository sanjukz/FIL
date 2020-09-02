import { OccuranceType } from '../../Enums/OccuranceType';
import { EventFrequencyType } from '../../Enums/EventFrequencyType';

export class EventScheduleModel {
  eventId: number;
  startDateTime: string;
  endDateTime: string;
  localStartTime: string;
  localEndTime: string;
  timeZoneOffset: string;
  timeZoneText: string;
  timeZoneAbbrivation: string;
  eventDetailId: number;
  venueId: number;
  dayId: number;
  localStartDateTime: string;
  localEndDateTime: string;
  isEnabled: boolean;
  isCreate: boolean;
  eventFrequencyType: EventFrequencyType;
}

export class EventScheduleViewModel {
  eventScheduleModel: EventScheduleModel;
  success: boolean;
  isValidLink?: boolean;
  isDraft?: boolean;
  currentStep?: number;
  completedStep?: string;
}

export class EventRecurranceInputViewModel {
  eventId: number;
  startDateTime: string;
  endDateTime: string;
  localStartTime: string;
  localEndTime: string;
  timeZoneOffset: string;
  timeZoneText: string;
  timeZoneAbbrivation: string;
  success: boolean;
  isValidLink?: boolean;
  isDraft?: boolean;
  currentStep?: number;
  completedStep?: string;
  eventScheduleId: number;
  scheduleDetailId: number;
  occuranceType: OccuranceType;
  eventFrequencyType: EventFrequencyType;
  localStartDateTime?: string;
  localEndDateTime?: string;
  dayIds: string;
  occuranceCount: number;
}

export class EventRecurranceScheduleModel {
  eventScheduleId: number;
  scheduleDetailId: number;
  dayIds: string;
  startDateTime: string;
  endDateTime: string;
  eventScheduleStartDateTime: string;
  eventScheduleEndDateTime: string;
  localStartDateTime?: string;
  localEndDateTime?: string;
  localEventScheduleStartDateTimeString: string;
  localEventScheduleEndDateTimeString: string;
  localStartTime: string;
  localEndTime: string;
  localStartDateString: string;
  localEndDateString: string;
  isEnabled: boolean;
}

export class EventRecurranceResponseViewModel {
  eventRecurranceScheduleModel: EventRecurranceScheduleModel[];
  success: boolean;
  isValidLink?: boolean;
  isDraft?: boolean;
  currentStep?: number;
  completedStep?: string;
}