import * as React from 'react';
import { GuestData } from "../../models/Cart/CartDataViewModel";
import { EventFrequencyType } from '../../Enum/EventFrequencyType';

export default class BSPUpgrade extends React.Component<any, any>{
    state = {
        isShowModal: false,
        btnId: -1,
        donationAmount: 0,
        cartData: this.props.cartData,
    }

    addTicket = (e, item) => {

        localStorage.removeItem("cartItems");
        localStorage.removeItem("currentCartItems");
        var cartData = this.state.cartData;
        for (var i = 0; i < cartData.length; i++) {
            if (cartData[i].eventTicketAttributeId == item.eventTicketAttributeId) {
                cartData[i].quantity = 1;
                var newGuest: GuestData = {
                    guestId: cartData[i].guestList.length + 1,
                    firstName: "",
                    lastName: "",
                    nationality: "",
                    documentTypeId: "",
                    documentNumber: "",
                };
                cartData[i].isTiqetsPlace = false;
                cartData[i].guestList.push(newGuest);
                cartData[i].timeSlot = "";
                cartData[i].timeStamp = new Date();
                cartData[i].isHohoPlace = false;
                cartData[i].isBSPUpgrade = true;
                cartData[i].overridedAmount = this.props.props.LiveOnline.userDetails.price;
                cartData[i].scheduleDetailId = this.props.props.LiveOnline.userDetails.eventDetail.eventFrequencyType == EventFrequencyType.Recurring ?
                    this.props.props.LiveOnline.userDetails.scheduleDetailId : 0;
                var cartItems = cartData.filter(function (val) {
                    return val.quantity > 0
                });
                localStorage.setItem('currentCartItems', JSON.stringify(cartItems));
                localStorage.setItem('cartItems', JSON.stringify(cartItems));
                sessionStorage.setItem('referralId', this.props.props.LiveOnline.userDetails.transactionId.toString())
                break;
            }
        }
        this.setState({ isShowModal: false });
        window.open(
            `/checkout?user=${this.props.props.LiveOnline.userDetails.userAltId}`,
            '_blank' // <- This makes it open in a new window.
        );
    }

    render() {
        return (<>
            < button
                className="btn btn-outline-primary ml-3"
                onClick={() => { this.addTicket(this, this.state.cartData[0]); }}
            >
                Upgrade to Backstage Pass
            </button>
        </>);
    }
}