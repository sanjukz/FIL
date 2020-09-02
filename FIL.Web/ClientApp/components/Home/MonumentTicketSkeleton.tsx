import * as React from "react";
import { FeelStandardPlaceholder } from "./StandardCardSkeleton";

export default class MonumentTicketSkeleton extends React.Component<any, any> {
    render() {
        let arr = [];
        for (let index = 0; index < this.props.number; index++) {
            let element = (
                <div key={index} className="card">
                    <FeelStandardPlaceholder />
                </div>
            );
            arr.push(element);
        }
        return <div className="card-columns">{arr}</div>;
    }
};