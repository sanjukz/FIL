import * as Session from "shared/stores/Session";
import * as Category from "./Category";
import * as DeliveryOptions from "./DeliveryOptions";
import * as Payment from "./Payment";
import * as PaymentResponse from "./PaymentResponse";
import * as PaymentError from "./PaymentError";
import * as Search from "../stores/Search";
import * as TicketCategory from "./TicketCategory";
import * as EventLearnPage from "./EventLearnPage";
import * as OrderConfirmation from "./OrderConfirmation";
import * as Checkout from "./Checkout";
import * as Login from "./Login";
import * as Register from "./Register";
import * as Account from "./Account";
import * as ActivateUser from "./ActivateUser";
import * as ResetPassword from "./ResetPassword";
import * as CustomerUpdate from "./CustomerUpdate";
import * as Footer from "./Footer";
import * as LoginGoogle from "./LoginGoogle";
import * as LoginFacebook from "./LoginFacebook";
import * as UserInvite from "./Invite";
import * as InviteInterest from "./InviteInterest";
import * as Itinerary from "./Itinerary";
import * as CityCountry from "./CityCountry";
import * as PrintPAH from "./PrintPAH";
import * as FeelSiteDynamicLayout from "./FeelSiteDynamicLayout";
import * as FeelPlaces from "./FeelPlaces";
import * as AllCategories from "./AllCategories";
import * as CurrentSelectedCategory from "./CurrentSelectedCategory";
import * as Transaction from "./Transaction";
import * as SiteAssest from "./SiteAsset";
import * as FeelNearMe from "./FeelNearMe";
import * as FeelUserJourney from "./FeelUserJourney";
import * as LiveOnline from "./LiveOnline";
import * as SignUp from "./SignUp";
import * as SocialLogin from "./SocialLogin";
import * as TicketAlert from "./TicketAlert";

// The top-level state object
export interface IApplicationState extends Session.ISessionApplicationState {
    category: Category.ICategoryState;
    checkout: Checkout.ICheckoutLoginState;
    DeliveryOptions: DeliveryOptions.IDeliveryOptionsState;
    Payment: Payment.IPaymentState;
    PaymentResponse: PaymentResponse.IPaymentResponseState;
    PaymentError: PaymentError.IPaymentErrorResponseState;
    search: Search.ISearchComponentState;
    ticketCategory: TicketCategory.ITicketCategoryPageState;
    eventLearnPage: EventLearnPage.IEventLearnPageComponentState;
    OrderConfirmation: OrderConfirmation.IOrderConfirmationState;
    register: Register.IRegisterState;
    ResetPassword: ResetPassword.IResetPasswordState;
    login: Login.ILoginState;
    CustomerUpdate: CustomerUpdate.ICustomerUpdateComponentState;
    account: Account.IUserAccountState;
    ActivateUser: ActivateUser.IActivateUserPageComponentState;
    footer: Footer.INewsLetterState;
    LoginGoogle: LoginGoogle.ILoginState;
    LoginFacebook: LoginFacebook.ILoginState;
    Invite: UserInvite.IInviteState;
    InviteInterest: InviteInterest.IInviteInterestState;
    Itinerary: Itinerary.ItineraryComponentState;
    cityCountry: CityCountry.ICityCountryState;
    session: Session.ISessionState;
    reprintRequest: PrintPAH.IPrintPAHComponentPropsState;
    pageMetaData: FeelSiteDynamicLayout.IFeelSiteDynamicLayoutState;
    categoryPlaces: FeelPlaces.IFeelPlacesState;
    Transaction: Transaction.ITransactionComponentState;
    allCategories: AllCategories.IAllCategoriesState;
    currentSelectedCategory: CurrentSelectedCategory.ICurrentSelectedCategoryState;
    siteAsset: SiteAssest.ISiteAssestState;
    nearMe: FeelNearMe.IFeelNearMeState;
    feelUserJourney: FeelUserJourney.IFeelUserJourneyState;
    liveOnline: LiveOnline.ILiveOnlineState;
    signUp: SignUp.ISignUpState;
    socialLogin: SocialLogin.ISocialLoginState;
    ticketAlert:TicketAlert.ITicketAlertDataState;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
    session: Session.reducer,
    category: Category.reducer,
    checkout: Checkout.reducer,
    DeliveryOptions: DeliveryOptions.reducer,
    Payment: Payment.reducer,
    PaymentResponse: PaymentResponse.reducer,
    PaymentError: PaymentError.reducer,
    search: Search.reducer,
    ticketCategory: TicketCategory.reducer,
    eventLearnPage: EventLearnPage.reducer,
    OrderConfirmation: OrderConfirmation.reducer,
    register: Register.reducer,
    ResetPassword: ResetPassword.reducer,
    login: Login.reducer,
    CustomerUpdate: CustomerUpdate.reducer,
    account: Account.reducer,
    ActivateUser: ActivateUser.reducer,
    footer: Footer.reducer,
    LoginGoogle: LoginGoogle.reducer,
    LoginFacebook: LoginFacebook.reducer,
    Invite: UserInvite.reducer,
    InviteInterest: InviteInterest.reducer,
    Itinerary: Itinerary.reducer,
    cityCountry: CityCountry.reducer,
    reprintRequest: PrintPAH.reducer,
    pageMetaData: FeelSiteDynamicLayout.reducer,
    categoryPlaces: FeelPlaces.reducer,
    Transaction: Transaction.reducer,
    allCategories: AllCategories.reducer,
    currentSelectedCategory: CurrentSelectedCategory.reducer,
    siteAsset: SiteAssest.reducer,
    nearMe: FeelNearMe.reducer,
    feelUserJourney: FeelUserJourney.reducer,
    liveOnline: LiveOnline.reducer,
    signUp: SignUp.reducer,
    socialLogin: SocialLogin.reducer,
    ticketAlert:TicketAlert.reducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export type IAppThunkAction<TAction> = (
    dispatch: (action: TAction) => void,
    getState: () => IApplicationState
) => void;