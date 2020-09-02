import { DiscountValueType } from '../../Enums/DiscountType';

export class ReplayModel {
  startDate: number;
  endDate: string;
  isPaid: boolean;
  isEnabled: boolean;
  price?: number;
  currencyId?: number;
}

export class ReplayViewModel {
  replayDetailModel: ReplayModel[];
  eventId: number;
  eventDetailId?: number;
  isCreate?: boolean;
  success: boolean;
  currentStep?: number;
  isDraft?: boolean;
  completedStep?: string;
}
