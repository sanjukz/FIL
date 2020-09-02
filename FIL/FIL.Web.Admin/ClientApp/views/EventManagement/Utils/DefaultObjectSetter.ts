import { EventDetailViewModel, EventDetailModel } from "../../../models/CreateEventV1/EventDetailViewModel";
import { EventScheduleViewModel, EventScheduleModel, EventRecurranceInputViewModel } from "../../../models/CreateEventV1/EventScheduleViewModel";
import { EventFrequencyType } from "../../../Enums/EventFrequencyType";
import { OccuranceType } from "../../../Enums/OccuranceType";
import { EventPerformanceViewModel, PerformanceTypeModel } from "../../../models/CreateEventV1/EventPerformanceViewModel";
import { EventHostsViewModel, Host } from '../../../models/CreateEventV1/EventHostsViewModel';
import { TicketViewModel, TicketModel } from '../../../models/CreateEventV1/TicketViewModel';
import { EventSponsorViewModel, Sponsor } from '../../../models/CreateEventV1/EventSponsorViewModel';
import { EventFinanceDetailModel } from '../../../models/CreateEventV1/EventFinanceViewModal'
import { EventStepViewModel } from "../../../models/CreateEventV1/EventStepViewModel";
import { ImageViewModel, ImageModel } from "../../../models/CreateEventV1/ImageViewModel";
import { ReplayViewModel, ReplayModel } from "../../../models/CreateEventV1/ReplayViewModel";

export const setDetailsObject = (slug: string, isBasicTab) => {
  let eventDetailModel: EventDetailModel = {
    altId: "",
    description: "",
    eventCategories: "",
    eventId: slug ? +slug : 0,
    isEnabled: false,
    name: "",
    slug: "",
    defaultCategory: "",
    itemsForViewer: [],
    isPrivate: false
  }
  let eventDetailViewModel: EventDetailViewModel = {
    eventDetail: eventDetailModel,
    success: false,
    currentStep: isBasicTab ? 1 : 2
  }
  return eventDetailViewModel;
}

export const setScheduleObject = (slug: string) => {
  let eventScheduleModel: EventScheduleModel = {
    timeZoneAbbrivation: "",
    dayId: 0,
    endDateTime: "",
    startDateTime: "",
    eventDetailId: 0,
    eventId: slug ? +slug : 0,
    isCreate: false,
    isEnabled: true,
    localEndTime: "",
    localStartTime: "",
    timeZoneText: "",
    localStartDateTime: "",
    localEndDateTime: "",
    timeZoneOffset: "",
    venueId: 0,
    eventFrequencyType: EventFrequencyType.None,
  }
  let eventScheduleViewModel: EventScheduleViewModel = {
    eventScheduleModel: eventScheduleModel,
    success: false,
    currentStep: 3
  }
  return eventScheduleViewModel;
}

export const setRecurranceScheduleObject = (slug: string, startDate: string) => {
  let eventRecurranceScheduleModel: EventRecurranceInputViewModel = {
    timeZoneAbbrivation: "",
    dayIds: "",
    endDateTime: "",
    startDateTime: startDate,
    eventId: slug ? +slug : 0,
    localEndTime: "",
    localStartTime: "",
    timeZoneText: "",
    timeZoneOffset: "",
    scheduleDetailId: 0,
    eventScheduleId: 0,
    eventFrequencyType: EventFrequencyType.Recurring,
    occuranceCount: 0,
    occuranceType: OccuranceType.Weekly,
    success: false
  }
  return eventRecurranceScheduleModel;
}

export const setPerformanceTypeObject = (slug: string) => {
  let performanceTypeModel: PerformanceTypeModel = {
    eventId: slug ? +slug : 0,
    isEnabled: true,
    id: 0,
    onlineEventTypeId: 1,
    isVideoUploaded: false,
    performanceTypeId: 1
  }

  let eventDetailViewModel: EventPerformanceViewModel = {
    performanceTypeModel: performanceTypeModel,
    eventId: slug ? +slug : 0,
    onlineEventType: "",
    success: false,
    eventAltId: "",
    eventFrequencyType: EventFrequencyType.None,
    currentStep: 5
  }
  return eventDetailViewModel;
}

export const setHostViewModelObject = (slug: string) => {
  let eventHostViewModel: EventHostsViewModel = {
    eventHostMapping: [],
    success: false,
    eventId: slug ? +slug : 0,
    isDraft: false,
    isValidLink: false,
    currentStep: 6,
  }
  return eventHostViewModel;
}

export const setTicketsObject = (slug, isAddOn) => {
  let ticketViewModel: TicketViewModel = {
    tickets: [],
    success: false,
    eventId: slug ? +slug : 0,
    eventDetailId: 0,
    isCreate: false,
    currentStep: isAddOn ? 8 : 7,
  }
  return ticketViewModel;
}

export const setSponsorObject = (slug) => {
  let eventSponsorViewModel: EventSponsorViewModel = {
    sponsorDetails: [],
    success: false,
    eventId: slug ? +slug : 0,
    isDraft: false,
    isValidLink: false,
    currentStep: 10
  }
  return eventSponsorViewModel;
}

export const setSponsorModelObject = (slug) => {
  let sponsor: Sponsor = {
    id: 0,
    altId: null,
    link: "",
    name: "",
    priority: 1,
    eventId: slug ? +slug : 0,
    isEnabled: true
  }
  return sponsor;
}

export const setHostModelObject = (slug) => {
  let host: Host = {
    id: 0,
    altId: "",
    description: "",
    eventId: slug ? +slug : 0,
    isEnabled: true,
    firstName: "",
    lastName: "",
    email: ""
  }
  return host;
}

export const setTicketModelObject = (ticketCategoryTypeId) => {
  let ticketModel: TicketModel = {
    currencyId: 11,
    currencyCode: "USD",
    description: "",
    etdId: 0,
    isEnabled: true,
    name: "",
    price: 0,
    quantity: 0,
    ticketCategoryId: 0,
    ticketCategoryTypeId: ticketCategoryTypeId,
    donationAmount1: 0,
    donationAmount2: 0,
    donationAmount3: 0
  }
  return ticketModel;
}

export const setFinanceObject = () => {
  let eventFinanceViewModel: EventFinanceDetailModel = {
    id: 0,
    accountTypeId: 1,
    firstName: '',
    lastName: '',
    email: '',
    companyName: '',
    phoneCode: 91,
    phoneNumber: '',
    accountName: '',
    bankName: '',
    branchCode: '',
    accountNumber: '',
    stateId: 9,
    taxId: '',
    countryId: 101,
    currenyId: 7
  }
  return eventFinanceViewModel;
}

export const setStepObject = (currentStep, slug) => {
  let eventStepViewModel: EventStepViewModel = {
    completedStep: "",
    currentStep: currentStep,
    eventId: +slug,
    success: false
  }
  return eventStepViewModel;
}

export const setImageObject = (currentStep, slug) => {
  let imageModel: ImageModel = {
    id: 0,
    eventId: +slug,
    eventAltId: null,
    isBannerImage: false,
    isHotTicketImage: false,
    isPortraitImage: false,
    isVideoUploaded: false
  }
  let imageViewModel: ImageViewModel = {
    completedStep: "",
    currentStep: currentStep,
    eventImageModel: imageModel,
    success: false
  }
  return imageViewModel;
}

export const setReplayObject = (currentStep, slug) => {
  let replayPaidModel: ReplayModel = {
    startDate: null,
    endDate: null,
    isEnabled: false,
    isPaid: true,
    currencyId: 11,
    price: 0
  }
  let replayFreeModel: ReplayModel = {
    startDate: null,
    endDate: null,
    isEnabled: false,
    isPaid: false,
    currencyId: 11,
    price: 0
  }
  let replayObject = [];
  replayObject.push(replayPaidModel);
  replayObject.push(replayFreeModel);
  let replayViewModel: ReplayViewModel = {
    completedStep: "",
    eventId: slug,
    currentStep: currentStep,
    replayDetailModel: replayObject,
    success: false
  }
  return replayViewModel;
}
