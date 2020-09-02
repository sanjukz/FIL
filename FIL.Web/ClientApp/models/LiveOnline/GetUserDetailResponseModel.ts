import EventHostMapping from "../../models/Comman/EventHostMapping";
import EventViewModel from "../../models/Comman/EventViewModel";
import { LiveEventDetailsViewModel } from "../../models/LiveOnline/LiveEventDetailsViewModel";
import EventDetailViewModel from "../../models/Comman/EventDetailViewModel";
import EventAttributeViewModel from "../../models/Comman/EventAttributeViewModel";

export class GetUserDetailResponseModel {
    success: boolean;
    userName: string;
    email: string;
    meetingNumber: string;
    apikey: string;
    roleId: string;
    signature: string;
    isAudioAvailable: boolean;
    isVideoAvailable: boolean;
    isChatAvailable: boolean;
    message: string;
    eventName: string;
    eventHostMappings: EventHostMapping[];
    liveEventDetail: LiveEventDetailsViewModel;
    event: EventViewModel;
    eventDetail: EventDetailViewModel;
    eventAttribute: EventAttributeViewModel;
    hostStartTime: string;
    utcTimeNow: string;
    importantInformation?: string;
    isDonate?: boolean;
    userAltId?: string;
    videoEndTimeVimeoString: string;
    videoEndDateTime?: string;
    isUpgradeToBSP?: boolean;
    price?: number;
    transactionId?: number;
    scheduleDetailId?: number;
}
