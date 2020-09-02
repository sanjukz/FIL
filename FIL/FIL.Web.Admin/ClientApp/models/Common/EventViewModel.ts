import { MasterEventTypes } from '../../Enums/MasterEventTypes'
import { Eventstatuses } from '../../Enums/Eventstatuses'

export default class Event {
  id: number;
  altId: string;
  name: string;
  eventTypeId: number;
  eventCategoryId: string;
  description: string;
  metaDetails: string;
  termsAndConditions: string;
  isEnabled: boolean;
  isPublishedOnSite: boolean;
  publishedDateTime: string;
  createdUtc: string;
  createdBy: string;
  masterEventTypeId?: MasterEventTypes;
  eventStatusId?: Eventstatuses;
}
