import * as React from "react";
import { gets3BaseUrl } from "../../../utils/imageCdn";

export class TiqetsContent {
    included: any[];
    excluded: any[];
    mustKnow: any[];
    goodKnow: any[];
    prePurchase: any[];
    usage: any[];
    postPurchase: any[];
}

export const formatTiqetsCheckoutDetails = (detailList: any) => {
    if (detailList != undefined) {
        let splitedList = detailList.split("*").filter((item) => { return item != "" });
        var formattedList = splitedList.map((item) => {
            if (item != "")
                return <li>{item.trim()} </li>
        })
        return formattedList;
    } else {
        let returnArray = [];
        return returnArray;
    }
}

export const getContent = (ticketCategoryData: any) => {
    if (ticketCategoryData.tiqetsCheckoutDetails != undefined && ticketCategoryData.tiqetsCheckoutDetails != null ) {
        let tiqetCheckoutDetails = ticketCategoryData.tiqetsCheckoutDetails;
        let included = formatTiqetsCheckoutDetails(tiqetCheckoutDetails.included);
        let excluded = formatTiqetsCheckoutDetails(tiqetCheckoutDetails.excluded);
        let mustKnow = formatTiqetsCheckoutDetails(tiqetCheckoutDetails.mustKnow);
        let goodKnow = formatTiqetsCheckoutDetails(tiqetCheckoutDetails.goodToKnow);
        let prePurchase = formatTiqetsCheckoutDetails(tiqetCheckoutDetails.prePurchase);
        let usage = formatTiqetsCheckoutDetails(tiqetCheckoutDetails.usage);
        let postPurchase = formatTiqetsCheckoutDetails(tiqetCheckoutDetails.postPurchase);
        let tiqetsContentModel: TiqetsContent = {
            excluded: excluded,
            goodKnow: goodKnow,
            included: included,
            mustKnow: mustKnow,
            postPurchase: postPurchase,
            prePurchase: prePurchase,
            usage: usage
        }
        return <div className="pb-4" id="description">
            <div className="tiqet-details">
                <ul className="nav nav-tabs border-0 d-block text-center">
                    {tiqetsContentModel.included.length > 0 && <li className="d-inline-block align-top"><a className="stretched-link d-block pr-4" data-toggle="tab" href="#WhatsIncluded">
                        <img src={`${gets3BaseUrl()}/icons/WhatsIncluded.png`} alt="" width="18" className="mr-2" />
                        What's Included
                                <div className="upArrow"><img src={`${gets3BaseUrl()}/icons/drop-up-arrow.png`} alt="" width="60" className="" /></div>
                    </a>
                    </li>}
                    {tiqetsContentModel.mustKnow.length > 0 && <li className="d-inline-block align-top"><a className="stretched-link  d-block pr-4" data-toggle="tab" href="#MustKnow">
                        <img src={`${gets3BaseUrl()}/icons/MustKnow.png`} alt="" width="15" className="mr-2" />
                        Must Know
                                <div className="upArrow"><img src={`${gets3BaseUrl()}/icons/drop-up-arrow.png`} alt="" width="60" className="" /></div>
                    </a>

                    </li>}
                    {tiqetsContentModel.goodKnow.length > 0 && <li className="d-inline-block align-top"><a className="stretched-link d-block pr-4" data-toggle="tab" href="#GoodToKnow">
                        <img src={`${gets3BaseUrl()}/icons/GoodToKnow.png`} alt="" width="22" className="mr-2" />
                        Good to Know
                                <div className="upArrow"><img src={`${gets3BaseUrl()}/icons/drop-up-arrow.png`} alt="" width="60" className="" /></div>
                    </a>

                    </li>}
                    {tiqetsContentModel.usage.length > 0 && <li className="d-inline-block align-top"><a className="stretched-link d-block pr-4" data-toggle="tab" href="#UsageID">
                        <img src={`${gets3BaseUrl()}/icons/UsageID.png`} alt="" width="14" className="mr-2" />
                        Usage
                                <div className="upArrow"><img src={`${gets3BaseUrl()}/icons/drop-up-arrow.png`} alt="" width="60" className="" /></div>
                    </a>

                    </li>}
                    {tiqetsContentModel.prePurchase.length > 0 && <li className="d-inline-block align-top"><a className="stretched-link d-block pr-4" data-toggle="tab" href="#PrePurchaseInstruction">
                        <img src={`${gets3BaseUrl()}/icons/PrePurchaseInstruction.png`} alt="" width="22" className="mr-2" />
                        Pre-Purchase Instruction
                                <div className="upArrow"><img src={`${gets3BaseUrl()}/icons/drop-up-arrow.png`} alt="" width="60" className="" /></div>
                    </a>

                    </li>}
                    {tiqetsContentModel.postPurchase.length > 0 && <li className="d-inline-block align-top"><a className="stretched-link d-block pr-4" data-toggle="tab" href="#PostPurchaseInstruction" >
                        <img src={`${gets3BaseUrl()}/icons/PostPurchaseInstruction.png`} alt="" width="18" className="mr-2" />
                        Post-Purchase Instruction
                                <div className="upArrow"><img src={`${gets3BaseUrl()}/icons/drop-up-arrow.png`} alt="" width="60" className="" /></div>
                    </a>
                    </li>}
                    {tiqetsContentModel.excluded.length > 0 && <li className="d-inline-block align-top"><a className="stretched-link d-block pr-4" data-toggle="tab" href="#WhatsExcluded">
                        <img src={`${gets3BaseUrl()}/icons/WhatsExcluded.png`} alt="" width="20" className="mr-2" />
                        What's Excluded
                                <div className="upArrow"><img src={`${gets3BaseUrl()}/icons/drop-up-arrow.png`} alt="" width="60" className="" /></div>
                    </a>
                    </li>}
                </ul>

                <div className="tab-content">
                    {tiqetsContentModel.included.length > 0 && <div className="tab-pane px-2 py-4 shadow" id="WhatsIncluded">
                        <ul>{tiqetsContentModel.included}</ul>
                    </div>}

                    {tiqetsContentModel.mustKnow.length > 0 && <div className="tab-pane px-2 py-4 shadow fade" id="MustKnow">
                        <ul>{tiqetsContentModel.mustKnow}</ul>
                    </div>}

                    {tiqetsContentModel.goodKnow.length > 0 && <div className="tab-pane px-2 py-4 shadow fade" id="GoodToKnow">
                        <ul>{tiqetsContentModel.goodKnow}</ul>
                    </div>}

                    {tiqetsContentModel.usage.length > 0 && <div className="tab-pane px-2 py-4 shadow fade" id="UsageID">
                        <ul>{tiqetsContentModel.usage}</ul>
                    </div>}

                    {tiqetsContentModel.prePurchase.length > 0 && <div className="tab-pane px-2 py-4 shadow fade" id="PrePurchaseInstruction">
                        <ul>{tiqetsContentModel.prePurchase}</ul>
                    </div>}

                    {tiqetsContentModel.postPurchase.length > 0 && <div className="tab-pane px-2 py-4 shadow fade" id="PostPurchaseInstruction">
                        <ul>{tiqetsContentModel.postPurchase}</ul>
                    </div>}

                    {tiqetsContentModel.excluded.length > 0 && <div className="tab-pane px-2 py-4 shadow fade" id="WhatsExcluded">
                        <ul>{tiqetsContentModel.excluded}</ul>
                    </div>}
                </div>
            </div>

        </div>
    } else {
        return <div></div>;
    }
}