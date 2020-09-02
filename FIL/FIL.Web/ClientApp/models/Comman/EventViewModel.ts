
import { MasterEventTypes } from '../../Enum/MasterEventTypes'
import { Eventstatuses } from '../../Enum/Eventstatuses'

export default class Event {
    id: number;
    eventTypeId: number;
    altId: string;
    name: string;
    eventCategoryId: string;
    description: string;
    metaDetails: string;
    termsAndConditions: string;
    isEnabled: boolean;
    isDelete?: boolean;
    isPublishedOnSite: boolean;
    publishedDateTime: string;
    slug: string;
    masterEventTypeId?: MasterEventTypes;
    eventStatusId?: Eventstatuses;
}