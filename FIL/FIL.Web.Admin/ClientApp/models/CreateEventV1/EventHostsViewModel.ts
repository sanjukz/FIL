
export class Host {
  eventId: number;
  id: number;
  altId: string;
  firstName: string;
  lastName: string;
  email: string;
  description: string;
  isEnabled: boolean;
  importantInformation?: string;
}

export class EventHostsViewModel {
  eventHostMapping: Host[];
  success: boolean;
  isValidLink: boolean;
  isDraft: boolean;
  eventId: number;
  currentStep?: number;
  completedStep?: string;
}
