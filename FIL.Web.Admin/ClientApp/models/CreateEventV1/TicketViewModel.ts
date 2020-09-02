import { DiscountValueType } from '../../Enums/DiscountType';

export class TicketModel {
  etdId: number;
  name: string;
  description: string;
  ticketCategoryId: number;
  ticketCategoryNotes?: string;
  currencyId: number;
  currencyCode: string;
  quantity: number;
  isEnabled: boolean;
  price: number;
  ticketCategoryTypeId?: number;
  totalQuantity?: number;
  remainingQuantity?: number;
  ticketAltId?: string;
  promoCode?: string;
  discountValueType?: DiscountValueType;
  discountAmount?: number;
  discountPercentage?: number;
  isDiscountEnable?: boolean;
  donationAmount1?: number;
  donationAmount2?: number;
  donationAmount3?: number;
}

export class TicketViewModel {
  tickets: TicketModel[];
  eventId: number;
  eventDetailId?: number;
  isCreate?: boolean;
  success: boolean;
  currentStep?: number;
  isDraft?: boolean;
  completedStep?: string;
}
