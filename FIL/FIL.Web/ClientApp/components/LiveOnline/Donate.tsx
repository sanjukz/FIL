import * as React from 'react';
import * as getSymbolFromCurrency from 'currency-symbol-map';
import { GuestData } from "../../models/Cart/CartDataViewModel";
import { Modal } from 'antd';

export default class Donate extends React.Component<any, any>{
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
                var cartItems = cartData.filter(function (val) {
                    return val.quantity > 0
                });
                localStorage.setItem('currentCartItems', JSON.stringify(cartItems));
                localStorage.setItem('cartItems', JSON.stringify(cartItems));
                break;
            }
        }
        this.setState({ isShowModal: false });
        window.open(
            `/checkout?user=${this.props.props.LiveOnline.userDetails.userAltId}`,
            '_blank' // <- This makes it open in a new window.
        );
    }

    onChangeDonateAmount = (amount, isContinueToItinerary = false) => {
        var data = this.state.cartData;
        data.forEach(function (item) {
            var itemData = item;
            itemData.donationAmount = +amount
        });
        this.setState({ cartData: data });
    }

    render() {
        var symbol = (this.props.props.ticketCategory.fetchEventSuccess && this.props.props.ticketCategory.ticketCategories && this.props.props.ticketCategory.ticketCategories.event) ? getSymbolFromCurrency(this.props.props.ticketCategory.ticketCategories.currencyType.code) : "$";
        return (<>
            < button
                className="btn btn-outline-primary ml-3"
                onClick={() => {
                    this.setState({ isShowModal: true });
                }}
            >
                Donate Now
            </button>
            <Modal
                centered
                footer={null}
                visible={this.state.isShowModal}
                onCancel={(e) => { this.setState({ isShowModal: false }) }}
                maskClosable={false}
            >
                < section className="ticket-details pb-5">
                    <div className="container">
                        <h5 className="mb-3 pink-color text-center"><i className="fa fa-calendar-alt"></i> <a href="javascript:void(0)" className="pink-color btn-lg btn-link p-0">Choose Amount</a></h5>
                        <div className="row justify-content-center">
                            <div className="col-sm-12 text-center">
                                <div className="card">
                                    <div className="card-body">
                                        <div className="row m-0">
                                            <div className="col-sm-6 p-0">
                                                <button type="button" onClick={(e) => {
                                                    this.setState({ btnId: 1, donationAmount: "25" }, () => {
                                                        this.onChangeDonateAmount(25, false);
                                                    })
                                                }} className={`btn ${this.state.btnId == 1 ? 'btn-primary' : 'btn-outline-secondary'}`}>{symbol}25</button>
                                                <button type="button" onClick={(e) => {
                                                    this.setState({ btnId: 2, donationAmount: "50" }, () => {
                                                        this.onChangeDonateAmount(50, false);
                                                    })
                                                }} className={`btn mx-2 ${this.state.btnId == 2 ? 'btn-primary' : 'btn-outline-secondary'}`}>{symbol}50</button>
                                                <button type="button" onClick={(e) => {
                                                    this.setState({ btnId: 3, donationAmount: "100" }, () => {
                                                        this.onChangeDonateAmount(100, false);
                                                    })
                                                }} className={`btn ${this.state.btnId == 3 ? 'btn-primary' : 'btn-outline-secondary'}`}>{symbol}100</button>
                                                <span className="px-3 d-none d-sm-inline-block">|</span>
                                            </div>
                                            <div className="col-sm-6 p-0 mt-3 mt-sm-0">
                                                <div className="input-group">
                                                    <div className="input-group-prepend">
                                                        <span className="input-group-text">{symbol}</span>
                                                    </div>
                                                    <input
                                                        value={this.state.donationAmount}
                                                        type="number"
                                                        placeholder="Enter amount"
                                                        className="form-control"
                                                        onChange={(e) => {
                                                            let amountArray = e.target.value.split(".");
                                                            let amount = amountArray.length > 0 ? amountArray[0] : e.target.value;
                                                            this.setState({ donationAmount: amount, btnId: 4 }, () => {
                                                                this.onChangeDonateAmount(this.state.donationAmount, false)
                                                            });
                                                        }} aria-label="Enter amount" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="footer-bottom text-center pt-4">
                            {(+this.state.donationAmount == 0) && <a href="javascript:void(0)" className="btn-lg btn btn-light opacity-disabled">Continue</a>}
                            {(+this.state.donationAmount > 0) && <a href="javascript:void(0)" onClick={() => { this.addTicket(this, this.state.cartData[0]); }} className="btn-lg site-primery-btn px-5">Continue</a>}
                        </div>
                    </div>
                </section>
            </Modal >
        </>);
    }
}