
export class Sponsor {
  eventId: number;
  id: number;
  altId: string;
  name: string;
  link: string;
  priority: number;
  isEnabled: boolean;
}

export class EventSponsorViewModel {
  sponsorDetails: Sponsor[];
  success: boolean;
  isValidLink: boolean;
  isDraft: boolean;
  eventId: number;
  currentStep?: number;
  completedStep?: string;
}
