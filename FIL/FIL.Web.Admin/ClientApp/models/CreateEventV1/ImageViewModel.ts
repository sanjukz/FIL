
export class ImageModel {
  id: number;
  eventId: number;
  eventAltId: string;
  isBannerImage: boolean;
  isHotTicketImage: boolean;
  isPortraitImage: boolean;
  isVideoUploaded: boolean;
}

export class ImageViewModel {
  success: boolean;
  currentStep?: number;
  completedStep?: string;
  isValidLink?: boolean;
  isDraft?: boolean;
  eventImageModel: ImageModel;
}
