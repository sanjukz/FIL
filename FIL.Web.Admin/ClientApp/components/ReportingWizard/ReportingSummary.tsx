import * as React from "react";

export default class ReportingSummary extends React.Component<any, any>{
    constructor(props) {
        super(props);
        this.state = {
            userId: "",
            roleId: ""
        }
        this.Reduce = this.Reduce.bind(this);
    }
    public componentDidMount() {
        this.setState({ userId: localStorage.getItem('altId') });
        this.setState({ roleId: localStorage.getItem('roleId') });
    }

    public Reduce(inputArray, keyGroupByArray, keyToBeSum) {
        var o = {};
        var reducedResult = inputArray.reduce(function (r, e) {
            var keyArrayTemp = [];
            keyGroupByArray.forEach(function (val) {
                keyArrayTemp.push(e[val]);
            });
            var key = keyArrayTemp.join('|');
            if (!o[key]) {
                var item = {};
                keyGroupByArray.forEach(function (val) {
                    item[val] = e[val];
                });
                o[key] = item;
                o[key].total = 0;
                r.push(o[key]);
            }
            e[keyToBeSum] = +e[keyToBeSum];
            o[key].total = +o[key].total;
            o[key].total += e[keyToBeSum];
            return r;
        }, []);
        return reducedResult;
    }

    public render() {
        var userId = this.state.userId;
        var roleId = this.state.roleId;
        var reportTransactionsInfo = this.props.reportTransactionsInfo;
        var reportData = this.props.reportData;
        if (this.props.showSummary == true) {
            var currencyWisenoOfTicket = [];
            var currencyWisegrossTicketAmount = [];
            var currencyWisediscountAmount = [];
            var currencyWisenetTicketAmount = [];
            var currencyWiseconvenienceCharges = [];
            var currencyWiseServiceTax = [];
            var currencyWiseDeliveryCharge = [];
            var currencyWiseTotalFees = [];
            var currencyWiseTotalTransactedAmountLC = [];
            var currencyWiseTotalTransactedAmountUSD = [];
            var currencyWiseSummary = [];
            var currencyWiseSummaryTemp = [];

            var currencySummaryColumns = ["CurrencyName", "NoOfTicket", "GrossTicketAmount", "DiscountAmount", "NetTicketAmount", "ConvenienceCharges", "ServiceTax", "CourierCharge", "TotalFees", "TotalTransactedAmountLC", "TotalTransactedAmountUSD"];
            var currencySummaryColumnsDisplayName = ["Currency", "No of Tickets", "Gross Ticket Amount", "Discount Amount", "Net Ticket Amount", "Service & Handling / Transaction Fee", "GST / Applicable Taxes", "Delivery Charge", "Total Fees", "Total Transacted Amount (LC)", "Total Transacted Amount (USD)"];
            var channelWisenoOfTicket = [];
            var channelWisegrossTicketAmount = [];
            var channelWisediscountAmount = [];
            var channelWisenetTicketAmount = [];
            var channelWiseconvenienceCharges = [];
            var channelWiseServiceTax = [];
            var channelWiseDeliveryCharge = [];
            var channelWiseTotalFees = [];
            var channelWiseTotalTransactedAmountLC = [];
            var channelWiseTotalTransactedAmountUSD = [];
            var channelWiseSummary = [];
            var channelWiseSummaryTemp = [];

            var channelSummaryColumns = ["Channel", "NoOfTicket", "GrossTicketAmount", "DiscountAmount", "NetTicketAmount", "ConvenienceCharges", "ServiceTax", "CourierCharge", "TotalFees", "TotalTransactedAmountLC", "TotalTransactedAmountUSD"];
            var channelSummaryColumnsDisplayName = ["Channel", "No of Tickets", "Gross Ticket Amount", "Discount Amount", "Net Ticket Amount", "Service & Handling / Transaction Fee", "GST / Applicable Taxes", "Delivery Charge", "Total Fees", "Total Transacted Amount (LC)", "Total Transacted Amount (USD)"];
            var ticketTypeWisenoOfTicket = [];
            var ticketTypeWisegrossTicketAmount = [];
            var ticketTypeWisediscountAmount = [];
            var ticketTypeWisenetTicketAmount = [];
            var ticketTypeWiseconvenienceCharges = [];
            var ticketTypeWiseServiceTax = [];
            var ticketTypeWiseDeliveryCharge = [];
            var ticketTypeWiseTotalFees = [];
            var ticketTypeWiseTotalTransactedAmountLC = [];
            var ticketTypeWiseTotalTransactedAmountUSD = [];
            var ticketTypeWiseSummary = [];
            var ticketTypeWiseSummaryTemp = [];

            var ticketTypeSummaryColumns = ["TiketType", "NoOfTicket", "GrossTicketAmount", "DiscountAmount", "NetTicketAmount", "ConvenienceCharges", "ServiceTax", "CourierCharge", "TotalFees", "TotalTransactedAmountLC", "TotalTransactedAmountUSD"];
            var ticketTypeSummaryColumnsDisplayName = ["Ticket Type", "No of Tickets", "Gross Ticket Amount", "Discount Amount", "Net Ticket Amount", "Service & Handling / Transaction Fee", "GST / Applicable Taxes", "Delivery Charge", "Total Fees", "Total Transacted Amount (LC)", "Total Transacted Amount (USD)"];
            var paymentMethodWisenoOfTicket = [];
            var paymentMethodWisegrossTicketAmount = [];
            var paymentMethodWisediscountAmount = [];
            var paymentMethodWisenetTicketAmount = [];
            var paymentMethodWiseconvenienceCharges = [];
            var paymentMethodWiseServiceTax = [];
            var paymentMethodWiseDeliveryCharge = [];
            var paymentMethodWiseTotalFees = [];
            var paymentMethodWiseTotalTransactedAmountLC = [];
            var paymentMethodWiseTotalTransactedAmountUSD = [];
            var paymentMethodWiseSummary = [];
            var paymentMethodWiseSummaryTemp = [];

            var paymentMethodSummaryColumns = ["PaymentType", "NoOfTicket", "GrossTicketAmount", "DiscountAmount", "NetTicketAmount", "ConvenienceCharges", "ServiceTax", "CourierCharge", "TotalFees", "TotalTransactedAmountLC", "TotalTransactedAmountUSD"];
            var paymentMethodSummaryColumnsDisplayName = ["Payment Method", "No of Tickets", "Gross Ticket Amount", "Discount Amount", "Net Ticket Amount", "Service & Handling / Transaction Fee", "GST / Applicable Taxes", "Delivery Charge", "Total Fees", "Total Transacted Amount (LC)", "Total Transacted Amount (USD)"];
            var summaryTotalNoOfTickets = 0;
            var summaryTotalGrossTicketAmount = 0;
            var summaryTotalDiscountAmount = 0;
            var summaryTotalNetTicketAmount = 0;
            var summaryTotalConvenienceCharges = 0;
            var summaryTotalServiceTax = 0;
            var summaryTotalCourierCharge = 0;
            var summaryTotalFees = 0;
            var summaryTotalTransactedAmount = 0;
            var summaryTotalTransactedAmountUSD = 0;

            //------CurrencyWiswSummary
            currencyWisenoOfTicket = this.Reduce(reportTransactionsInfo, ['currencyName'], 'noOfTicket');
            currencyWisegrossTicketAmount = this.Reduce(reportTransactionsInfo, ['currencyName'], 'grossTicketAmount');
            currencyWisediscountAmount = this.Reduce(reportTransactionsInfo, ['currencyName'], 'discountAmount');
            currencyWisenetTicketAmount = this.Reduce(reportTransactionsInfo, ['currencyName'], 'netTicketAmount');
            currencyWiseconvenienceCharges = this.Reduce(reportTransactionsInfo, ['currencyName'], 'convenienceCharges');
            currencyWiseServiceTax = this.Reduce(reportTransactionsInfo, ['currencyName'], 'serviceTax');
            currencyWiseDeliveryCharge = this.Reduce(reportTransactionsInfo, ['currencyName'], 'courierCharge');
            currencyWiseTotalTransactedAmountLC = this.Reduce(reportTransactionsInfo, ['currencyName'], 'totalTransactedAmount');

            for (var i = 0; i <= currencyWisenoOfTicket.length - 1; i++) {
                var currencyType = reportData.currencyType.filter(function (et1) {
                    return et1.code == currencyWisenoOfTicket[i].currencyName;
                });
                var exchangeRate;
                currencyWiseSummary = [];
                currencyWiseSummary.push(currencyWisenoOfTicket[i].currencyName);
                currencyWiseSummary.push(currencyWisenoOfTicket[i].total);
                currencyWiseSummary.push(currencyWisegrossTicketAmount[i].total.toFixed(2));
                currencyWiseSummary.push(currencyWisediscountAmount[i].total.toFixed(2));
                currencyWiseSummary.push(currencyWisenetTicketAmount[i].total.toFixed(2));
                currencyWiseSummary.push(currencyWiseconvenienceCharges[i].total.toFixed(2));
                currencyWiseSummary.push(currencyWiseServiceTax[i].total.toFixed(2));
                currencyWiseSummary.push(currencyWiseDeliveryCharge[i].total.toFixed(2));
                currencyWiseSummary.push((currencyWiseconvenienceCharges[i].total + currencyWiseServiceTax[i].total + currencyWiseDeliveryCharge[i].total).toFixed(2));
                currencyWiseSummary.push(currencyWiseTotalTransactedAmountLC[i].total.toFixed(2));
                if (currencyType.length > 0) {
                    exchangeRate = currencyType[0].exchangeRate > 0 ? currencyType[0].exchangeRate : 1;
                } else {
                    exchangeRate = 1;
                }

                //var exchangeRate = currencyType[0].exchangeRate > 0 ? currencyType[0].exchangeRate : 1;
                currencyWiseSummary.push((parseFloat(currencyWiseTotalTransactedAmountLC[i].total) / parseFloat(exchangeRate)).toFixed(2));
                currencyWiseSummaryTemp.push(currencyWiseSummary);
            }

            //ChannelWiswSummary                
            channelWisenoOfTicket = this.Reduce(reportTransactionsInfo, ['channel'], 'noOfTicket');
            channelWisegrossTicketAmount = this.Reduce(reportTransactionsInfo, ['channel'], 'grossTicketAmount');
            channelWisediscountAmount = this.Reduce(reportTransactionsInfo, ['channel'], 'discountAmount');
            channelWisenetTicketAmount = this.Reduce(reportTransactionsInfo, ['channel'], 'netTicketAmount');
            channelWiseconvenienceCharges = this.Reduce(reportTransactionsInfo, ['channel'], 'convenienceCharges');
            channelWiseServiceTax = this.Reduce(reportTransactionsInfo, ['channel'], 'serviceTax');
            channelWiseDeliveryCharge = this.Reduce(reportTransactionsInfo, ['channel'], 'courierCharge');
            channelWiseTotalTransactedAmountLC = this.Reduce(reportTransactionsInfo, ['channel'], 'totalTransactedAmount');
            var reducedResult = this.Reduce(reportTransactionsInfo, ['channel', 'currencyName'], 'totalTransactedAmount');
            for (var i = 0; i <= channelWisenoOfTicket.length - 1; i++) {
                var channelWiseCurrency = reducedResult.filter(function (et1) {
                    return et1.channel == channelWisenoOfTicket[i].channel;
                });
                var totalTransactedAmount = 0;
                if (channelWiseCurrency.length > 0) {
                    channelWiseCurrency.forEach(function (val) {
                        var currencyType = reportData.currencyType.filter(function (et1) {
                            return et1.code == val.currencyName;
                        });
                        var exchangeRate;
                        if (currencyType.length > 0) {
                            exchangeRate = currencyType[0].exchangeRate > 0 ? currencyType[0].exchangeRate : 1;
                        } else {
                            exchangeRate = 1;
                        }
                        //var exchangeRate = currencyType[0].exchangeRate > 0 ? currencyType[0].exchangeRate : 1;
                        totalTransactedAmount += parseFloat(val.total) / parseFloat(exchangeRate);
                    });
                }
                channelWiseSummary = [];
                channelWiseSummary.push(channelWisenoOfTicket[i].channel);
                channelWiseSummary.push(channelWisenoOfTicket[i].total);
                channelWiseSummary.push(channelWisegrossTicketAmount[i].total.toFixed(2));
                channelWiseSummary.push(channelWisediscountAmount[i].total.toFixed(2));
                channelWiseSummary.push(channelWisenetTicketAmount[i].total.toFixed(2));
                channelWiseSummary.push(channelWiseconvenienceCharges[i].total.toFixed(2));
                channelWiseSummary.push(channelWiseServiceTax[i].total.toFixed(2));
                channelWiseSummary.push(channelWiseDeliveryCharge[i].total.toFixed(2));
                channelWiseSummary.push((channelWiseconvenienceCharges[i].total + channelWiseServiceTax[i].total + channelWiseDeliveryCharge[i].total).toFixed(2));
                channelWiseSummary.push(channelWiseTotalTransactedAmountLC[i].total.toFixed(2));
                channelWiseSummary.push(totalTransactedAmount.toFixed(2));
                channelWiseSummaryTemp.push(channelWiseSummary);
            }

            //-------------TicketTypeSummary
            ticketTypeWisenoOfTicket = this.Reduce(reportTransactionsInfo, ['ticketType'], 'noOfTicket');
            ticketTypeWisegrossTicketAmount = this.Reduce(reportTransactionsInfo, ['ticketType'], 'grossTicketAmount');
            ticketTypeWisediscountAmount = this.Reduce(reportTransactionsInfo, ['ticketType'], 'discountAmount');
            ticketTypeWisenetTicketAmount = this.Reduce(reportTransactionsInfo, ['ticketType'], 'netTicketAmount');
            ticketTypeWiseconvenienceCharges = this.Reduce(reportTransactionsInfo, ['ticketType'], 'convenienceCharges');
            ticketTypeWiseServiceTax = this.Reduce(reportTransactionsInfo, ['ticketType'], 'serviceTax');
            ticketTypeWiseDeliveryCharge = this.Reduce(reportTransactionsInfo, ['ticketType'], 'courierCharge');
            ticketTypeWiseTotalTransactedAmountLC = this.Reduce(reportTransactionsInfo, ['ticketType'], 'totalTransactedAmount');
            var reducedResult = this.Reduce(reportTransactionsInfo, ['ticketType', 'currencyName'], 'totalTransactedAmount');
            for (var i = 0; i <= ticketTypeWisenoOfTicket.length - 1; i++) {
                var ticketTypeCurrency = reducedResult.filter(function (et1) {
                    return et1.ticketType == ticketTypeWisenoOfTicket[i].ticketType;
                });
                var totalTransactedAmount = 0;
                if (ticketTypeCurrency.length > 0) {
                    ticketTypeCurrency.forEach(function (val) {
                        var currencyType = reportData.currencyType.filter(function (et1) {
                            return et1.code == val.currencyName;
                        });
                        var exchangeRate;
                        if (currencyType.length > 0) {
                            exchangeRate = currencyType[0].exchangeRate > 0 ? currencyType[0].exchangeRate : 1;
                        } else {
                            exchangeRate = 1;
                        }
                        //var exchangeRate = currencyType[0].exchangeRate > 0 ? currencyType[0].exchangeRate : 1;
                        totalTransactedAmount += parseFloat(val.total) / parseFloat(exchangeRate);
                    });
                }

                ticketTypeWiseSummary = [];
                ticketTypeWiseSummary.push(ticketTypeWisenoOfTicket[i].ticketType);
                ticketTypeWiseSummary.push(ticketTypeWisenoOfTicket[i].total);
                ticketTypeWiseSummary.push(ticketTypeWisegrossTicketAmount[i].total.toFixed(2));
                ticketTypeWiseSummary.push(ticketTypeWisediscountAmount[i].total.toFixed(2));
                ticketTypeWiseSummary.push(ticketTypeWisenetTicketAmount[i].total.toFixed(2));
                ticketTypeWiseSummary.push(ticketTypeWiseconvenienceCharges[i].total.toFixed(2));
                ticketTypeWiseSummary.push(ticketTypeWiseServiceTax[i].total.toFixed(2));
                ticketTypeWiseSummary.push(ticketTypeWiseDeliveryCharge[i].total.toFixed(2));
                ticketTypeWiseSummary.push((ticketTypeWiseconvenienceCharges[i].total + ticketTypeWiseServiceTax[i].total + ticketTypeWiseDeliveryCharge[i].total).toFixed(2));
                ticketTypeWiseSummary.push(ticketTypeWiseTotalTransactedAmountLC[i].total.toFixed(2));
                ticketTypeWiseSummary.push(totalTransactedAmount.toFixed(2));
                ticketTypeWiseSummaryTemp.push(ticketTypeWiseSummary);
            }

            ////-------------paymentMethodSummary
            paymentMethodWisenoOfTicket = this.Reduce(reportTransactionsInfo, ['modeofPayment'], 'noOfTicket');
            paymentMethodWisegrossTicketAmount = this.Reduce(reportTransactionsInfo, ['modeofPayment'], 'grossTicketAmount');
            paymentMethodWisediscountAmount = this.Reduce(reportTransactionsInfo, ['modeofPayment'], 'discountAmount');
            paymentMethodWisenetTicketAmount = this.Reduce(reportTransactionsInfo, ['modeofPayment'], 'netTicketAmount');
            paymentMethodWiseconvenienceCharges = this.Reduce(reportTransactionsInfo, ['modeofPayment'], 'convenienceCharges');
            paymentMethodWiseServiceTax = this.Reduce(reportTransactionsInfo, ['modeofPayment'], 'serviceTax');
            paymentMethodWiseDeliveryCharge = this.Reduce(reportTransactionsInfo, ['modeofPayment'], 'courierCharge');
            paymentMethodWiseTotalTransactedAmountLC = this.Reduce(reportTransactionsInfo, ['modeofPayment'], 'totalTransactedAmount');
            var reducedResult = this.Reduce(reportTransactionsInfo, ['modeofPayment', 'currencyName'], 'totalTransactedAmount');
            for (var i = 0; i <= paymentMethodWisenoOfTicket.length - 1; i++) {
                var paymentMethodCurrency = reducedResult.filter(function (et1) {
                    return et1.modeofPayment == paymentMethodWisenoOfTicket[i].modeofPayment;
                });
                var totalTransactedAmount = 0;
                if (paymentMethodCurrency.length > 0) {
                    paymentMethodCurrency.forEach(function (val) {
                        var currencyType = reportData.currencyType.filter(function (et1) {
                            return et1.code == val.currencyName;
                        });
                        var exchangeRate;
                        if (currencyType.length > 0) {
                            exchangeRate = currencyType[0].exchangeRate > 0 ? currencyType[0].exchangeRate : 1;
                        } else {
                            exchangeRate = 1;
                        }
                        //var exchangeRate = currencyType[0].exchangeRate > 0 ? currencyType[0].exchangeRate : 1;
                        totalTransactedAmount += parseFloat(val.total) / parseFloat(exchangeRate);
                    });
                }
                paymentMethodWiseSummary = [];
                paymentMethodWiseSummary.push(paymentMethodWisenoOfTicket[i].modeofPayment);
                paymentMethodWiseSummary.push(paymentMethodWisenoOfTicket[i].total);
                paymentMethodWiseSummary.push(paymentMethodWisegrossTicketAmount[i].total.toFixed(2));
                paymentMethodWiseSummary.push(paymentMethodWisediscountAmount[i].total.toFixed(2));
                paymentMethodWiseSummary.push(paymentMethodWisenetTicketAmount[i].total.toFixed(2));
                paymentMethodWiseSummary.push(paymentMethodWiseconvenienceCharges[i].total.toFixed(2));
                paymentMethodWiseSummary.push(paymentMethodWiseServiceTax[i].total.toFixed(2));
                paymentMethodWiseSummary.push(paymentMethodWiseDeliveryCharge[i].total.toFixed(2));
                paymentMethodWiseSummary.push((paymentMethodWiseconvenienceCharges[i].total + paymentMethodWiseServiceTax[i].total + paymentMethodWiseDeliveryCharge[i].total).toFixed(2));
                paymentMethodWiseSummary.push(paymentMethodWiseTotalTransactedAmountLC[i].total.toFixed(2));
                paymentMethodWiseSummary.push(totalTransactedAmount.toFixed(2));
                paymentMethodWiseSummaryTemp.push(paymentMethodWiseSummary);
            }

            for (var i = 0; i <= currencyWisenoOfTicket.length - 1; i++) {
                var currencyType = reportData.currencyType.filter(function (et1) {
                    return et1.code == currencyWisenoOfTicket[i].currencyName;
                });
                var exchangeRate;
                summaryTotalNoOfTickets += currencyWisenoOfTicket[i].total;
                summaryTotalGrossTicketAmount += currencyWisegrossTicketAmount[i].total;
                summaryTotalDiscountAmount += currencyWisediscountAmount[i].total;
                summaryTotalNetTicketAmount += currencyWisenetTicketAmount[i].total;
                summaryTotalConvenienceCharges += currencyWiseconvenienceCharges[i].total;
                summaryTotalServiceTax += currencyWiseServiceTax[i].total;
                summaryTotalCourierCharge += currencyWiseDeliveryCharge[i].total;
                summaryTotalFees += (currencyWiseconvenienceCharges[i].total + currencyWiseServiceTax[i].total + currencyWiseDeliveryCharge[i].total);
                summaryTotalTransactedAmount += currencyWiseTotalTransactedAmountLC[i].total;
                if (currencyType.length > 0) {
                     exchangeRate = currencyType[0].exchangeRate > 0 ? currencyType[0].exchangeRate : 1;
                } else {
                    exchangeRate = 1;
                }
                
                summaryTotalTransactedAmountUSD += (parseFloat(currencyWiseTotalTransactedAmountLC[i].total) / parseFloat(exchangeRate));
            }

            var currencySummaryHeaders = currencySummaryColumnsDisplayName.map(item => {
                return <th>{item}</th>
            });

            var channelSummaryHeaders = channelSummaryColumnsDisplayName.map(item => {
                return <th>{item}</th>
            });

            var ticketTypeSummaryHeaders = ticketTypeSummaryColumnsDisplayName.map(item => {
                return <th>{item}</th>
            });

            var paymentMethodSummaryHeaders = paymentMethodSummaryColumnsDisplayName.map(item => {
                return <th>{item}</th>
            });

            var currencySummaryData = currencyWiseSummaryTemp.map(item => {
                return (
                    <tr>
                        {item.map(i => {
                            return <td>{i}</td>
                        })}
                    </tr>
                )
            });

            var channelSummaryData = channelWiseSummaryTemp.map(item => {
                return (
                    <tr>
                        {item.map(i => {
                            return <td>{i}</td>
                        })}
                    </tr>
                )
            });

            var ticketTypeSummaryData = ticketTypeWiseSummaryTemp.map(item => {
                return (
                    <tr>
                        {item.map(i => {
                            return <td>{i}</td>
                        })}
                    </tr>
                )
            });

            var paymentMethodSummaryData = paymentMethodWiseSummaryTemp.map(item => {
                return (
                    <tr>
                        {item.map(i => {
                            return <td>{i}</td>
                        })}
                    </tr>
                )
            });

            var summaryTotals = [];
            summaryTotals.push(summaryTotalNoOfTickets);
            summaryTotals.push(summaryTotalGrossTicketAmount.toFixed(2));
            summaryTotals.push(summaryTotalDiscountAmount.toFixed(2));
            summaryTotals.push(summaryTotalNetTicketAmount.toFixed(2));
            summaryTotals.push(summaryTotalConvenienceCharges.toFixed(2));
            summaryTotals.push(summaryTotalServiceTax.toFixed(2));
            summaryTotals.push(summaryTotalCourierCharge.toFixed(2));
            summaryTotals.push(summaryTotalFees.toFixed(2));
            summaryTotals.push(summaryTotalTransactedAmount.toFixed(2));
            summaryTotals.push(summaryTotalTransactedAmountUSD.toFixed(2));

            var allSummaryTotals = summaryTotals.map(item => {
                return <td><b>{item}</b></td>
            });
        }
       
            return <div>
                {
                    this.props.showSummary == true ?
                        <div>
                            <section className="summary">
                                <div className="container-fluid container-xl">
                                    <h5 className="mt-0">Currency Wise Summary</h5>
                                    <div className="table-responsive bg-white mb-20 bdr-1">
                                        <table className="table table-striped table-condensed mb-0 table-prices">
                                            <thead>
                                                <tr>
                                                    {currencySummaryHeaders}
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {currencySummaryData}
                                                <tr>
                                                    <td><b>Total</b></td>
                                                    {allSummaryTotals}
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <h5 className="mt-20">Channel Wise Summary</h5>
                                    <div className="table-responsive bg-white mb-20 bdr-1">
                                        <table className="table table-striped table-condensed mb-0 table-prices">
                                            <thead>
                                                <tr>
                                                    {channelSummaryHeaders}
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {channelSummaryData}
                                                <tr>
                                                    <td><b>Total</b></td>
                                                    {allSummaryTotals}
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <h5 className="mt-20">Ticket Type Summary</h5>
                                    <div className="table-responsive bg-white mb-20 bdr-1">
                                        <table className="table table-striped table-condensed mb-0 table-prices">
                                            <thead>
                                                <tr>
                                                    {ticketTypeSummaryHeaders}
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {ticketTypeSummaryData}
                                                <tr>
                                                    <td><b>Total</b></td>
                                                    {allSummaryTotals}
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <h5 className="mt-20">Payment Mode Wise Summary</h5>
                                    <div className="table-responsive bg-white mb-20 bdr-1">
                                        <table className="table table-striped table-condensed mb-0 table-prices">
                                            <thead>
                                                <tr>
                                                    {paymentMethodSummaryHeaders}
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {paymentMethodSummaryData}
                                                <tr>
                                                    <td><b>Total</b></td>
                                                    {allSummaryTotals}
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </section>
                        </div> : ""
                }
            </div>;
    }
}
