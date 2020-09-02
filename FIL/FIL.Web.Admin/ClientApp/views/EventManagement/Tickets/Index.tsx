/* Third party imports */
import * as React from 'react'
import * as _ from 'lodash'
import { Tabs } from 'antd';

/* Local imports */
import TicketForm from './TicketForm'
import { TicketViewModel } from '../../../models/CreateEventV1/TicketViewModel';
import { DeleteTicketViewModel } from '../../../models/CreateEventV1/DeleteTicketViewModel';
import { setTicketsObject } from "../utils/DefaultObjectSetter";
import { uploadImage } from "../../../utils/S3Configuration"
import { TicketListing } from "./TicketListing";
import { showNotification } from "../Notification";
import { Error } from "../Modal";
import { error } from "../Confirmation";
import { ListFooter } from "../Footer/ListFooter";
import Spinner from "../../../components/Spinner";
import { Donation } from './Donation';

export class Index extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      isShowForm: false,
      selectedHostId: 0,
      selectedTicketId: 0,
      currentMenu: 'Ticket',
      ticketViewModel: setTicketsObject(this.props.slug, this.props.isAddOn ? 2 : 1),
      donateTicketCatId: 12259,
    }
  }

  componentDidMount() {
    if (this.props.slug) {
      this.props.props.requestEventTickets(this.props.slug, this.props.isAddOn ? 2 : 1, (response: TicketViewModel) => {
        if (response.success) {
          response.currentStep = this.state.ticketViewModel.currentStep;
          this.setState({ ticketViewModel: response, isShowForm: response.tickets.filter((val) => { return (val.ticketCategoryId != 19452 && val.ticketCategoryId != 12259) }).length > 0 ? false : true });
        } else {
        }
      });
    } else {
      this.setState({ isShowForm: true });
    }
    this.setState({ donateTicketCatId: window.location.href.indexOf('dev') > -1 ? 12259 : 19452 });
  }

  onDeleteTicket = (ticketAltId: any) => {
    if (this.props.eventStatus == 6 && this.state.ticketViewModel.tickets.length == 1) {
      Error('Event is live on site you need atleast one ticket to continue with booking.');
      return;
    }
    let deleteTicketViewModel: DeleteTicketViewModel = {
      isTicketSold: false,
      success: false,
      completedStep: "",
      currentStep: this.props.isAddOn ? 8 : 7,
      ticketLength: this.state.ticketViewModel.tickets.length,
      etdAltId: ticketAltId,
      eventId: +this.props.slug
    }
    this.props.props.deleteEventTickets(deleteTicketViewModel, (response: DeleteTicketViewModel) => {
      if (response.success && !response.isTicketSold) {
        console.log(response);
        let ticketViewModel = this.state.ticketViewModel;
        ticketViewModel.tickets = ticketViewModel.tickets.filter((val: any) => { return val.ticketAltId != ticketAltId });
        this.setState({ ticketViewModel: ticketViewModel, isShowForm: ticketViewModel.tickets.filter((val) => { return (val.ticketCategoryId != 19452 && val.ticketCategoryId != 12259) }).length > 0 ? false : true })
        showNotification('success', this.props.isAddOn ? `Add-on deleted successfully` : `Ticket deleted successfully`);
        this.props.changeRoute(this.props.isAddOn ? 8 : 7, response.completedStep)
      } else if (response.isTicketSold) {
        showNotification('error',
          this.props.isAddOn ? `You can't delete this Add on as it's already sold` :
            `You can't delete this ticket category as tickets are already sold`
        );
      }
    });
  }

  updateEventStep = () => {
    this.props.changeRoute(11, null, true);
  }

  isShowLoader = () => {
    return (this.props.props.EventTickets.isLoading || this.props.props.EventStep.isLoading) && !this.props.props.EventTickets.isSaveRequest
  }

  isShowListing = () => {
    return !this.state.isShowForm && !this.props.props.EventStep.isLoading
  }

  getFormProps = () => {
    return {
      props: this.props.props,
      selectedTicketId: this.state.selectedTicketId,
      countries: this.props.props.countryType.countryTypes,
      ticketViewModel: this.state.ticketViewModel,
      onClickCancel: () => {
        this.setState({ isShowForm: false }, () => {
          if (this.state.ticketViewModel.tickets.length == 0) {
            this.props.changeRoute(this.props.isAddOn ? 7 : 6, this.state.ticketViewModel.completedStep, true)
          }
        })
      },
      isAddOn: this.props.isAddOn,
      onSubmit: (item: any) => {
        if (this.props.props.EventTickets.tickets.isDraft) {
          error(`Please submit schedule tab before submitting the ${this.props.isAddOn ? 'Add-on' : 'ticket'} tab`, () => {
            this.props.changeRoute(3, null, true);
          });
          return;
        }
        let currentTicket = [];
        currentTicket.push(item);
        let ticketViewModel: TicketViewModel = {
          tickets: currentTicket,
          success: false,
          eventId: this.props.slug ? +this.props.slug : 0,
          currentStep: this.props.isAddOn ? 8 : 7,
        }
        this.props.props.saveEventTickets(ticketViewModel, (response: TicketViewModel) => {
          if (response.success) {
            let tickets = this.state.ticketViewModel.tickets;
            let isExists = false;
            let altId = "";
            tickets = tickets.map((val) => {
              if (val.etdId == response.tickets[0].etdId) {
                isExists = true;
                altId = val.ticketAltId;
                val = { ...response.tickets[0] }
              } else {
                val.currencyId = response.tickets[0].currencyId;
                val.currencyCode = response.tickets[0].currencyCode;
              }
              return val;
            })
            if (!isExists) {
              altId = response.tickets[0].ticketAltId;
              tickets.push(response.tickets[0]);
            }
            response.tickets = tickets;
            this.setState({ isShowForm: false, ticketViewModel: response });
            if (this.state.imageFile) {
              uploadImage(this.state.imageFile, `${altId.toUpperCase()}-add-ons`);
            }
            if (this.state.currentMenu != "Donate") {
              this.props.changeRoute(this.props.isAddOn ? 8 : 7, response.completedStep)
            } else {
              this.props.changeRoute(11, null, true);
            }
          } else {
          }
        })
      },
      onClickSkip: () => {
        this.setState({ isShowForm: false }, () => {
          this.updateEventStep();
        })
      },
      onImageSelect: (item) => {
        this.setState({ imageFile: item });
      }
    }
  }

  getTicket = () => {
    return <>
      {!this.state.isShowForm && (this.isShowListing() || (this.state.selectedTicketId == 0 && this.state.ticketViewModel.tickets.filter((val) => { return (val.ticketCategoryId != 19452 && val.ticketCategoryId != 12259) }).length > 0)) && <div data-aos="fade-up" data-aos-duration="1000">
        <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
          <h3 className="m-0 text-purple">{this.props.isAddOn ? "Maximise your experience revenue through add-ons" : `${(this.state.selectedTicketId == 0 && this.state.ticketViewModel.tickets.filter((val) => { return (val.ticketCategoryId != 19452 && val.ticketCategoryId != 12259) }).length > 0) ? 'Create your tickets' : 'Get them in!'}`}</h3></nav>
        < TicketListing
          ticketViewModel={this.state.ticketViewModel}
          donateTicketCatId={this.state.donateTicketCatId}
          currencyList={this.props.props.currencyType.currencyTypes.currencyTypes}
          isAddOn={this.props.isAddOn}
          onClickTicket={(item: any) => {
            this.setState({ imageFile: undefined, selectedTicketId: item.etdId, isShowForm: true })
          }}
          onDeleteTicket={(ticketAltId: any) => {
            this.onDeleteTicket(ticketAltId)
          }} /></div>
      }
      {this.isShowLoader() && <Spinner />}

      {this.isShowListing() && <ListFooter
        saveText={'Continue'}
        addText={this.props.isAddOn ? '+ Another Add-on' : '+ Add Another Ticket'}
        onClickAddNew={() => {
          this.setState({ imageFile: undefined, isShowForm: true, selectedTicketId: 0 });
        }}
        onClickContinue={() => {
          this.props.changeRoute(this.props.isAddOn ? 11 : 8, this.state.ticketViewModel.completedStep, true);
        }}
      />}
      {this.state.isShowForm ? (<>
        <TicketForm
          {...this.getFormProps()}
        /></>
      ) : (
          <div></div>
        )}
    </>
  }

  render() {
    const { TabPane } = Tabs;
    return (
      <>
        <div data-aos="fade-up" data-aos-duration="1000" className="p-3">
          <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="7">
            {this.props.isAddOn && <Tabs defaultActiveKey={this.state.currentMenu} onTabClick={(key: any, e: any) => {
              this.setState({ currentMenu: key });
            }}>
              <TabPane tab="Add-ons" key="Ticket">
                {this.getTicket()}
              </TabPane>
              <TabPane tab="Donate" key="Donate">
                <Donation
                  donateTicketCatId={this.state.donateTicketCatId} // Dev & prod donate ticket categoryId
                  {...this.getFormProps()}
                />
              </TabPane>
            </Tabs>}
            {!this.props.isAddOn && <>{this.getTicket()}</>}
          </div>
        </div>
      </>
    )
  }
}
