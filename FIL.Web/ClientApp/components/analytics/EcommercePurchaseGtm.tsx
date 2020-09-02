import * as React from "react";
import * as numeral from "numeral";

declare const window: any;

const EcommercePurchaseGtm: React.FunctionComponent<any> = (props) => {
  React.useEffect(() => {
    window.dataLayer = window.dataLayer || [];
    window.dataLayer.push({
      ecommerce: {
        purchase: {
          actionField: {
            id: `FAP ${props.orderConfirmationData.transaction.id * 6 +
              "0019" +
              props.orderConfirmationData.transaction.id}`, // Transaction ID. Required for purchases and refunds.
            affiliation: "Online Store",
            revenue: `${numeral(
              props.orderConfirmationData.transaction.netTicketAmount
            ).format("00.00")}`, // Total transaction value (incl. tax and shipping)
            tax: `${numeral(
              props.orderConfirmationData.transaction.serviceCharge
            ).format("00.00")}`,
            shipping: `${numeral(
              props.orderConfirmationData.transaction.deliveryCharges
            ).format("00.00")}`,
            coupon: "",
          },
          products: props.orderConfirmationData.orderConfirmationSubContainer.map(
            (item) => {
              return {
                name: item.event.name, // Name or ID is required.
                id: item.event.id,
                price:
                  item.subEventContainer[0].transactionDetail[0].pricePerTicket,
                category: item.subEventContainer[0].eventDetail.name,
                variant: "",
                quantity:
                  item.subEventContainer[0].transactionDetail[0].totalTickets,
                coupon: "", // Optional fields may be omitted or set to empty string.
              };
            }
          ),
        },
      },
    });
  }, []);
  return <></>;
};

export default EcommercePurchaseGtm;
