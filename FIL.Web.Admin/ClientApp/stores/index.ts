import * as Session from "shared/stores/Session";
import * as Login from "./Login";
import * as Register from "./Register";
import * as EventData from "./EventData";
import * as AllocationData from "./AllocationManager";
import * as VenueAllData from "./VenueData";
import * as EventWizardData from "./EventWizard";
import * as TransactionReportData from "./Reporting/Report";
import * as TicketLookup from "./TicketLookup";
import * as ChangePassword from "./Account";
import * as FailedTransactionReportData from "./Reporting/FailedTransactionReport";
import * as AuthedRoleFeature from "./AuthedRoleFeature";
import * as ReportFilter from "./Reporting/ReportFilter";
import * as InventoryReport from "./Reporting/InventoryReport";
import * as ScanningReport from "./Reporting/ScanningReport";
import * as Invite from "./InviteManager";
import * as EditInvite from "./EditInvite";
import * as EventCategory from "./EventCategory";
import * as EventSiteId from "./EventSiteId";
import * as EventCreation from "./EventCreation";
import * as EventDetailCreation from "./EventDetailCreation";
import * as EventTicketDetailCreation from "./EventTicketDetailCreation";
import * as PlaceCalendar from "./PlaceCalendar";
import * as Inventory from "./Inventory";
import * as CurrencyType from "./CurrencyType";
import * as CountryType from "./Country";
import * as StatesType from "./States";
import * as FinanceDetails from "./FinanceDetails";
import * as ApproveModerate from "./ApproveModerate";
import * as SellerStore from "./Seller";
import * as Fulfillment from "./Fulfillment";
import * as Description from "./Description";
import * as Redemption from "./Redemption";
import * as EventDetails from "./CreateEventv1/EventDetails";
import * as EventHosts from "./CreateEventv1/EventHosts";
import * as EventSchedule from "./CreateEventv1/EventSchedule";
import * as EventTickets from "./CreateEventv1/EventTickets";
import * as EventFinance from "./CreateEventv1/EventFinance";
import * as EventPerformance from "./CreateEventv1/EventPerformance";
import * as StepDetails from "./CreateEventv1/StepDetails";
import * as EventStep from "./CreateEventv1/EventStep";
import * as EventSponsor from "./CreateEventv1/EventSponsor";
import * as EventImage from "./CreateEventv1/EventImage";
import * as EventReplay from "./CreateEventv1/EventReplay";

export interface IApplicationState extends Session.ISessionApplicationState {
  register: Register.IRegisterState;
  login: Login.ILoginState;
  events: EventData.IEventComponentState;
  allocations: AllocationData.IAllocationComponentState;
  venues: VenueAllData.IVenueComponentState;
  eventWizard: EventWizardData.IEventWizardState;
  transactionReport: TransactionReportData.ITransactionReportComponentState;
  ticketLookup: TicketLookup.ITicketLookupState;
  UserAccount: ChangePassword.IUserAccountState;
  failedTransactionReport: FailedTransactionReportData.IFailedTransactionReportComponentState;
  reportFilter: ReportFilter.IReportFilterState;
  inventoryReport: InventoryReport.IInventoryReportState;
  scanningReport: ScanningReport.IScanningReportComponentState;
  authedRoleFeature: AuthedRoleFeature.IAuthedNavMenuFeatureState;
  invites: Invite.IInviteManagerComponentState;
  editInvites: EditInvite.IInviteState;
  eventcategory: EventCategory.IEventCategoryComponentState;
  eventSiteId: EventSiteId.IEventSiteIdComponentState;
  EventCreation: EventCreation.IEventCategoriesComponentState;
  EventDetailCreation: EventDetailCreation.IEventDetailComponentState;
  EventTicketDetailCreation: EventTicketDetailCreation.IEventTicketDetailComponentState;
  placeCalendar: PlaceCalendar.IPlaceCalendarState;
  currencyType: CurrencyType.ICurrencyTypeComponentState;
  countryType: CountryType.ICountryTypeComponentState;
  statesType: StatesType.IStateTypeComponentState;
  inventory: Inventory.InventoryState;
  financialDetails: FinanceDetails.IFinanceDetailsComponentState;
  ApproveModerate: ApproveModerate.IApproveModerateState;
  sellar: SellerStore.ISellerEventState;
  Fulfillment: Fulfillment.IFulfillmentComponentState;
  Description: Description.DescriptionComponentState;
  Redemption: Redemption.RedemptionComponentState;
  EventDetails: EventDetails.IEventDetailComponentState;
  EventHosts: EventHosts.IEventHostsComponentState;
  EventSchedule: EventSchedule.IEventScheduleComponentState;
  EventTickets: EventTickets.IEventTicketsComponentState;
  EventFinance: EventFinance.IEventFinanceComponentState;
  EventPerformance: EventPerformance.IEventPerformanceComponentState;
  StepDetails: StepDetails.IStepDetailComponentState;
  EventStep: EventStep.IEventStepComponentState;
  EventSponsor: EventSponsor.IEventSponsorComponentState;
  EventImage: EventImage.IEventImageComponentState;
  EventReplay: EventReplay.IEventReplayComponentState;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
  login: Login.reducer,
  register: Register.reducer,
  session: Session.reducer,
  events: EventData.reducer,
  allocations: AllocationData.reducer,
  venues: VenueAllData.reducer,
  eventWizard: EventWizardData.reducer,
  transactionReport: TransactionReportData.reducer,
  ticketLookup: TicketLookup.reducer,
  UserAccount: ChangePassword.reducer,
  failedTransactionReport: FailedTransactionReportData.reducer,
  authedRoleFeature: AuthedRoleFeature.reducer,
  reportFilter: ReportFilter.reducer,
  inventoryReport: InventoryReport.reducer,
  scanningReport: ScanningReport.reducer,
  invites: Invite.reducer,
  editInvites: EditInvite.reducer,
  eventcategory: EventCategory.reducer,
  eventSiteId: EventSiteId.reducer,
  EventCreation: EventCreation.reducer,
  EventDetailCreation: EventDetailCreation.reducer,
  EventTicketDetailCreation: EventTicketDetailCreation.reducer,
  placeCalendar: PlaceCalendar.reducer,
  currencyType: CurrencyType.reducer,
  inventory: Inventory.reducer,
  countryType: CountryType.reducer,
  FinanceDetails: FinanceDetails.reducer,
  statesType: StatesType.reducer,
  ApproveModerate: ApproveModerate.reducer,
  sellar: SellerStore.reducer,
  Fulfillment: Fulfillment.reducer,
  Description: Description.reducer,
  Redemption: Redemption.reducer,
  EventDetails: EventDetails.reducer,
  EventHosts: EventHosts.reducer,
  EventSchedule: EventSchedule.reducer,
  EventTickets: EventTickets.reducer,
  EventFinance: EventFinance.reducer,
  EventPerformance: EventPerformance.reducer,
  StepDetails: StepDetails.reducer,
  EventStep: EventStep.reducer,
  EventSponsor: EventSponsor.reducer,
  EventImage: EventImage.reducer,
  EventReplay: EventReplay.reducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export type IAppThunkAction<TAction> = (dispatch: (action: TAction) => void, getState: () => IApplicationState) => void;
