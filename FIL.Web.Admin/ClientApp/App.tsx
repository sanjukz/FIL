import * as React from 'react'
import { connect } from 'react-redux'
import { Route } from 'react-router-dom'
import { RouteComponentProps, Switch } from 'react-router-dom'
import { actionCreators as sessionActionCreators, ISessionProps, ISessionState } from 'shared/stores/Session'
import 'shared/styles/globalStyles/main.scss'
import { userIsAuthenticatedRedirect, userIsNotAuthenticatedRedirect } from 'shared/utils/auth'
import Layout from './components/Layout'
import { IApplicationState } from './stores'
import Dashboard from './views/Dashboard'
import AutoLogin from './views/CrossDomain'
import EventTicketDashBoardView from './views/EventTicket'
import LoginView from './views/Login'
import RegisterView from './views/Register'
import UserView from './views/User'
import AllocationManagerView from './views/AllocationManager'
import VenueCreationView from './views/VenueCreation'
import homeView from './views/Home'
import EventWizard from './views/EventWizard'
import TicketLookup from './views/TicketLookup'
import ChangePassword from './views/ChangePassword'
import FeelUserReport from './views/Reporting/FeelUserReport'
import InviteManager from './views/InviteManager'
import EditInvite from './views/EditInvite'
import AddInvite from './views/AddInvite'
import EventListing from './views/EventManager/EventListing'
import EventCategoryMapping from './views/EventManager/EventCategoryMapping'
import EventSiteIdMapping from './views/EventManager/EventSiteIdMapping'
import MasterEventCreation from './views/MasterEventCreation'
import TransactionReportSingleDataModel from './views/Reporting/TransactionReportSingleDataModel'
import ApproveModeratePlace from './views/ApproveModeratePlace'
import MyFeels from './views/MyFeels'
import CreateGuest from './views/Redemption/Guest/CreateGuest'
import ApproveModerateGuide from './views/Redemption/Guest/ApproveModerateGuide'
import Orders from './views/Redemption/Guest/Orders'
import FulfillmentForm from './views/Fulfilmentform'
import MasterEventCreationNew from './views/MasterEventCreationNew'
import Descriptions from './views/Descriptions'
import Index from './views/EventManagement/Index'
import StripeConnectAccount from './views/Live/StripeConnectAccount'
import TermsAndCondition from './views/Live/TermsAndCondition'
import { PageNotFound } from './views/PageNotFound'
import { Unauthorized } from './views/Unauthorized'
import TicketAlertReport from './views/Reporting/TicketAlertReport'
import TransactionLocatorReport from './views/Reporting/TransactionLocatorReport'

/*
 * No Authentication Required
 */
const Login = userIsNotAuthenticatedRedirect(LoginView)
const Register = userIsNotAuthenticatedRedirect(RegisterView)
const home = userIsNotAuthenticatedRedirect(homeView)

/*
 * Authentication Required
 */
const WrappedDashboard = userIsAuthenticatedRedirect(Dashboard)
const IsLoginTransactionReport = userIsAuthenticatedRedirect(TransactionReportSingleDataModel)
/*
 * Authentication Admin Required
 */
// const Admin = userIsAuthenticatedRedirect(userIsAdminRedirect(AdminComponent));
interface IAppProps {
  gtmId: string
}

type AppProps = IAppProps & ISessionProps & typeof sessionActionCreators & RouteComponentProps<IAppProps>

class App extends React.Component<AppProps, any> {
  constructor(props) {
    super(props)
    this.state = {
      isLoggedIn: undefined
    }
  }
  public componentDidMount() {
    this.props.getSession()
  }

  public render() {
    return (
      <>
        {(this.props.session.success && this.props.session.isAuthenticated) && < Layout gtmId={this.props.gtmId} url={this.props.location.pathname}>
          <Switch>
            <Route exact path="/" component={Dashboard} />
            <Route exact path="/login" component={Login} />
            <Route path="/register" component={Register} />
            <Route path="/user" component={UserView} />
            <Route path="/home" component={home} />
            <Route path="/eventticket" component={EventTicketDashBoardView} />
            <Route path="/allocationmanager" component={AllocationManagerView} />
            <Route path="/description-upload" component={Descriptions} />
            <Route path="/venuecreation" component={VenueCreationView} />
            <Route path="/eventwizard" component={EventWizard} />
            <Route path="/transactionreport" component={IsLoginTransactionReport} />
            <Route path="/ticketlookup" component={TicketLookup} />
            <Route path="/changepassword" component={ChangePassword} />
            <Route path="/invitemanager" component={InviteManager} />
            <Route path="/editinvite" component={EditInvite} />
            <Route path="/addinvite" component={AddInvite} />
            <Route path="/eventlisting" component={EventListing} />
            <Route path="/eventsorting" component={EventListing} />
            <Route path="/eventcategory" component={EventCategoryMapping} />
            <Route path="/eventsiteidmap" component={EventSiteIdMapping} />
            <Route path="/create/v1" component={MasterEventCreationNew} />
            <Route path="/createplace" component={MasterEventCreation} />
            <Route path="/edit" component={MasterEventCreation} />
            <Route exact path="/approve-moderate" component={ApproveModeratePlace} />
            <Route path="/myfeels" component={MyFeels} />
            <Route path="/user-report" component={FeelUserReport} />
            <Route path="/fulfillment-form" component={FulfillmentForm} />
            <Route path="/create-guide" component={CreateGuest} />
            <Route path="/edit-guide/:userAltId" component={CreateGuest} />
            <Route exact path="/verifyauth/:altId" component={AutoLogin} />
            <Route exact path="/approve-guide" component={ApproveModerateGuide} />
            <Route exact path="/approve-orders" component={Orders} />
            <Route path="/host-online" component={Index} />
            <Route exact path="/edit-event/:placeAltId" component={Index} />
            <Route exact path="/stripe-connect/success" component={StripeConnectAccount} />
            <Route exact path="/terms-and-conditions" component={TermsAndCondition} />
            <Route exact path="/report/ticket-alert" component={TicketAlertReport} />
            <Route exact path="/report/transaction-locator" component={TransactionLocatorReport} />
            <Route component={PageNotFound} />
          </Switch>
        </Layout>}
        {(this.props.session.success && !this.props.session.isAuthenticated) && (
          <Switch>
            <Route exact path="/" component={Login} />
            <Route exact path="/login" component={Login} />
            <Route exact path="/verifyauth/:altId" component={AutoLogin} />
            <Route component={Unauthorized} />
          </Switch>
        )}
      </>
    )
  }
}

export default connect(
  (state: IApplicationState, ownProps) => ({
    session: state.session,
    ...ownProps
  }),
  sessionActionCreators
)(App)
