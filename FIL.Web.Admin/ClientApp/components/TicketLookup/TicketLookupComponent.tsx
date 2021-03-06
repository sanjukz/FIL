﻿import * as React from "react";
import TicketLookupTableComponent from "../../components/TicketLookup/TicketLookupTableComponent";
const columns = [{
    Header: "Confirmation  ID",
    accessor: "confirmationId"
},
{
    Header: "Transaction Date & Time(UTC)",
    accessor: "trnasactionDateTme"
},
{
    Header: "PAH Link",
    accessor: "pahLink"
},
{
    Header: "Payment status",
    accessor: "transactionStatus"
},
{
    Header: "Payconfig Number",
    accessor: "payconfigNumber"
},
{
    Header: "Buyer FirstName",
    accessor: "firstName"
},
{
    Header: "Buyer LastName",
    accessor: "lastName"
},
{
    Header: "Buyer Email",
    accessor: "emailId"
},
{
    Header: "PhoneNumber",
    accessor: "phoneNumber"
},
{
    Header: "Event",
    accessor: "event"
},
{
    Header: "SubEvent Name",
    accessor: "eventDetail"
},
{
    Header: "Venue",
    accessor: "venue"
},
{
    Header: "City",
    accessor: "city"
},
{
    Header: "Ticket Category",
    accessor: "ticketCategory"
},
{
    Header: "Barcode Status",
    accessor: "barcodeStatus"
},
{
    Header: "Ticket Quantity",
    accessor: "totalTickets"
},
{
    Header: "Price Per Ticket",
    accessor: "pricePerTicket"
},
{
    Header: "Total Value of Purchase",
    accessor: "netTicketAmount"
},
{
    Header: "Payment Option",
    accessor: "paymentoption"
},
{
    Header: "Currency",
    accessor: "currency"
},
{
    Header: "Delivery Type",
    accessor: "deliveryType"
},
{
    Header: "Delivery To",
    accessor: "pickupBy"
},
];

export default class TicketLookupComponents extends React.Component<any, any> {
    public render() {
        var data = this.props.ticketLookup;
        var transactionId = data.transaction.id;
        var paymentoption = data.paymentOption;
        var payconfigNumber = data.payconfigNumber;
        var transactionStatus = data.transaction.transactionStatusId
        var buyerEmailId = data.transaction.emailId;
        var phoneNumber = data.transaction.phoneNumber;
        var buyerFirstName = data.transaction.firstName;
        var buyerLastName = data.transaction.lastName;
        var LastName = data.transaction.lastName;
        var phoneNumber = data.transaction.phoneNumber;
        var currency = data.currencyType.code;
        var trnasactionDateTme = data.transaction.createdUtc;
        var netTicketAmount = data.transaction.netTicketAmount;

        const ticketData = [];
        this.props.ticketLookup.ticketLookupSubContainer.map(function (item) {
            var event = item.event.name;
            item.subEventContainer.map(function (val) {
                var eventdetail = val.eventDetail.name;
                var totalTicket = [];
                var pricePerTicket = [];
                var venue = val.venue.name;
                var city = val.city.name;
                val.transactionDetail.map(function (transactionDetailItem) {
                    totalTicket.push(transactionDetailItem.totalTickets);
                    pricePerTicket.push(transactionDetailItem.pricePerTicket);
                });
                var ticketCategoryName = [];
                val.ticketCategory.map(function (item) {
                    ticketCategoryName.push(item.name);
                });
                var ticketBarcodeInfo = [];
                var eventTicketDetailId = 0;
                val.matchSeatTicketDetailContainer.map(function (item) {
                    var barcodeStatus = '';
                    item.matchSeatTicketDetail.map(function (a) {
                        var status = '';
                        if (a.entryStatus == true) {
                            status = 'Scanned';
                        }
                        else {
                            status = 'Not Scanned';
                        }
                        barcodeStatus = barcodeStatus + a.barcodeNumber + ': ' + status + ', ';
                    });
                    var barcodeStatus = barcodeStatus.slice(0, -2)
                    ticketBarcodeInfo.push(barcodeStatus);
                });

                var deliveryToInfo = [];
                var deliveryTypeInfo = [];
                val.transactionDeliveryDetail.map(function (item) {
                    deliveryToInfo.push(item.deliveryTo);
                    deliveryTypeInfo.push(item.deliveryTypeId);

                });
                for (var i = 0; i < ticketCategoryName.length; i++) {

                    let newName = {
                        confirmationId: transactionId,
                        transactionStatus: transactionStatus,
                        payconfigNumber: payconfigNumber,
                        firstName: buyerFirstName,
                        lastName: buyerLastName,
                        emailId: buyerEmailId,
                        phoneNumber: phoneNumber,
                        event: event,
                        eventDetail: eventdetail,
                        venue: venue,
                        city: city,
                        ticketCategory: ticketCategoryName[i],
                        barcodeStatus: ticketBarcodeInfo[i],
                        totalTickets: totalTicket[i],
                        pricePerTicket: pricePerTicket[i],
                        paymentoption: paymentoption,
                        currency: currency,
                        deliveryType: deliveryTypeInfo[i],
                        pickupBy: deliveryToInfo[i],
                        trnasactionDateTme: trnasactionDateTme,
                        netTicketAmount: netTicketAmount
                    };
                    ticketData.push(newName);
                }


            });
        });
        return <div className="table table-striped table-bordered example-table">
            <TicketLookupTableComponent myTableData={ticketData} myTableColumns={columns} />
        </div>;
    }
}
