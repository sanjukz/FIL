
export class EventDetailViewModel {
  success: boolean;
  isValidLink?: boolean;
  isDraft?: boolean;
  eventDetail: EventDetailModel;
  currentStep?: number;
  completedStep?: string;
}

export class EventDetailModel {
  eventId: number;
  eventCategories: string;
  defaultCategory: string;
  parentCategory?: string;
  parentCategoryId?: string;
  name: string;
  itemsForViewer: string[];
  description: string;
  isCreate?: boolean;
  altId: string;
  slug: string;
  isEnabled: boolean;
  isPrivate?: boolean;
}
