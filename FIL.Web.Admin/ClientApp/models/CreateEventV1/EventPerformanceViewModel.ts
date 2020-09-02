import { EventFrequencyType } from '../../Enums/EventFrequencyType';
export class PerformanceTypeModel {
  id: number;
  eventId: number;
  isEnabled: boolean;
  isVideoUploaded?: boolean;
  performanceTypeId: number;
  onlineEventTypeId: number;
}

export class EventPerformanceViewModel {
  performanceTypeModel: PerformanceTypeModel;
  eventId: number;
  onlineEventType: string;
  success: boolean;
  currentStep?: number;
  completedStep?: string;
  eventAltId?: string;
  eventFrequencyType?: EventFrequencyType;
}
