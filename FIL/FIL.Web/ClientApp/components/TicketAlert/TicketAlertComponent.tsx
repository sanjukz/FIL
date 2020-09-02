import * as React from "react";
import TicketAlertForm from './TicketAlertForm';
import { ReactHTMLConverter } from "react-html-converter/browser";
import { TicketAlertRequestViewModel } from "../../models/TicketAlert/TicketAlertRequestViewModel";
import { TicketAlertUserMappingResponseViewModel } from "../../models/TicketAlert/TicketAlertUserMappingResponseViewModel";

export default class TicketAlertComponent extends React.Component<any, any> {
    state = {
    };

    capitalizeFirstLetter = (string) => {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }

    render() {
        const converter = ReactHTMLConverter();
        converter.registerComponent('TicketAlertComponent', TicketAlertComponent);
        return (
            <div className="fil-site fil-exp-landing-page">
                <section className="py-4 bg-light fil-inner-banner">
                    <div className="container position-relative">
                        <nav className="fil-cust-breadcrumb d-none d-md-block">
                            <ol className="breadcrumb rounded-0">
                                <li className="breadcrumb-item">
                                    <a href="/">
                                        Home
                                </a>
                                </li>
                                <li className="breadcrumb-item">
                                    <a href={`c/${this.props.eventData.category.slug}?category=${this.props.eventData.category.id}`}>
                                        {this.props.eventData.category.displayName}
                                    </a>
                                </li>
                                <li className="breadcrumb-item">
                                    <a href={`/c/${this.props.eventData.category.slug}/${this.props.eventData.subCategory.slug}?category=${this.props.eventData.category.id}&subcategory=${this.props.eventData.subCategory.id}`} >{this.props.eventData.subCategory.displayName}
                                    </a>
                                </li>
                                <li className="breadcrumb-item active" aria-current="page">
                                    {this.props.eventData.event.name}
                                </li>
                            </ol>
                        </nav>
                        <div className="card-deck overflow-hidden fil-inner-banner-images">
                            <div className="card">
                                <img src={`https://feelitlive.imgix.net/images/ticket-alert/${this.props.eventData.event.altId.toUpperCase()}.jpg?auto=format&fit=crop&h=435&w=508`}
                                    onError={() => {
                                        return `https://feelitlive.imgix.net/images/places/tiles/${this.props.eventData.event.altId.toUpperCase()}-ht-c1.jpg?auto=format&fit=crop&h=435&w=508&crop=entropy`
                                    }}
                                />
                            </div>
                            <TicketAlertForm
                                countryList={this.props.countryList}
                                eventData={this.props.eventData}
                                ticketAlertResponse={this.props.ticketAlertResponse}
                                onSubmit={(form: TicketAlertRequestViewModel) => {
                                    this.props.onSubmit(form)
                                }} />
                        </div>
                        {this.props.ticketAlertResponse.success && <div className={`col-12 alert alert-success mt-3`} role="alert">
                            You have successfully signed up for the ticket alert. Thanks!
                    </div>}
                        {this.props.ticketAlertResponse.isAlreadySignUp && <div className={`col-12 alert alert-danger mt-3`} role="alert">
                            You have already signed up with same email.
                    </div>}
                    </div>
                </section>
                {(this.props.eventData.event.description) && <section className="exp-sec-pad fil-exp-list-sec">
                    <div className="container">
                        <div className="card-deck">
                            <div className="card">
                                <h3 className="text-purple">{this.props.eventData.event.name}</h3>
                            </div>
                            <div className="card">
                                <>{converter.convert(this.props.eventData.event.description)}</>
                            </div>
                        </div>
                    </div>
                </section>}
                {(this.props.eventData.eventHostMappings && this.props.eventData.eventHostMappings.length > 0) && <section className="exp-sec-pad bg-light">
                    <div className="container">
                        <div className="card-deck">
                            <div className="card">
                                <h3 className="text-purple"> {this.props.eventData.event.id != 15601 ? 'Artist' : 'Panelists'}</h3>
                            </div>
                            <div className="card fil-exp-list">
                                {this.props.eventData.eventHostMappings.map((item, index) => {
                                    return (
                                        <div><span className="font-weight-bold">{this.capitalizeFirstLetter(item.firstName)} {this.capitalizeFirstLetter(item.lastName)},</span> {converter.convert(item.description)}</div>
                                    )
                                })}
                            </div>
                        </div>
                    </div>
                </section>}
            </div>
        );
    }
}