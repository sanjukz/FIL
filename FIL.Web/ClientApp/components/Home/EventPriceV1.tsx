import * as React from "react";
import * as numeral from "numeral";
import * as getSymbolFromCurrency from 'currency-symbol-map'

export default class EventPriceV1 extends React.PureComponent<any, any> {

    public render() {
        var symbol = getSymbolFromCurrency(this.props.currency);
        var price;
        if (this.props.minPrice == this.props.maxPrice) {
            if (this.props.minPrice.toString().indexOf('.') > -1)
                price = <p className="m-0">{symbol + numeral(this.props.minPrice).format('0.00') + ' ' + this.props.currency}
                    {(this.props.duration && this.props.duration != null && this.props.duration != "0 minutes") && <span className="d-inline-block px-2">| {this.props.duration}</span>}
                </p>
            else
                price = <p className="m-0">{symbol + this.props.minPrice + ' ' + this.props.currency}
                    {(this.props.duration && this.props.duration != null && this.props.duration != "0 minutes") && <span className="d-inline-block px-2">| {this.props.duration}</span>}
                </p>
        }
        else {
            var minPriceFormat = ''
            var maxPriceFormat = ''
            if ((this.props.minPrice.toString().indexOf('.') > -1) || (this.props.maxPrice.toString().indexOf('.') > -1)) {
                minPriceFormat = numeral(this.props.minPrice).format('0.00')
                maxPriceFormat = numeral(this.props.maxPrice).format('0.00')
            }
            else {
                minPriceFormat = this.props.minPrice
                maxPriceFormat = this.props.maxPrice
            }

            price = <p className="m-0">{symbol + minPriceFormat + ' ' + this.props.currency} <span className="d-inline-block px-1">-</span>  {symbol + maxPriceFormat + ' ' + this.props.currency}
                {(this.props.duration && this.props.duration != null && this.props.duration != "0 minutes") && <span className="d-inline-block px-2">| {this.props.duration}</span>}
            </p>
        }
        return <div className="tils-text">
            {price}
        </div>;
    }
}