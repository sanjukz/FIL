import { EventFrequencyType } from '../../Enums/EventFrequencyType';

export class StepModel {
  stepId: number;
  name: string;
  sortOrder: number;
  icon: string;
  description: string;
  slug: string;
  isEnabled: boolean;
}

export class StepViewModel {
  eventId: number;
  success: boolean;
  currentStep?: number;
  completedStep?: string;
  eventName?: string;
  eventStatus?: number;
  isTransacted?: boolean;
  eventFrequencyType?: EventFrequencyType;
  stepModel: StepModel[];
}
