import * as React from "React";
import InnerBanner from "./InnerBanner";
import { Link } from "react-router-dom";

export const Banner = (props: any) => {
    return <div className="inner-banner">
        <InnerBanner category={props.ticketCategoryData.category} subCategory={props.ticketCategoryData.subCategory} eventAltId={props.eventId} />
        <div className="card-img-overlay">
            <nav aria-label="breadcrumb">
                <ol className="breadcrumb bg-white m-0 p-1 pl-2 d-inline-block">
                    <li className="breadcrumb-item"><Link to="/">Home</Link></li>
                    <li className="breadcrumb-item"><Link to={{
                        pathname: `/c/category/${props.ticketCategoryData.category.slug.trim()}`,
                        search: `?category=${props.ticketCategoryData.event.eventCategoryId}`
                    }}>{props.ticketCategoryData.eventCategoryName}</Link></li>
                    <li className="breadcrumb-item"><Link to={{
                        pathname: `/c/category/${props.ticketCategoryData.eventCategoryName}/${props.ticketCategoryData.eventCategory}`,
                        search: `?category=${props.ticketCategoryData.event.eventCategoryId}`
                    }}> {props.ticketCategoryData.eventCategory}</Link></li>
                    <li className="breadcrumb-item active" aria-current="page"><Link to={"/place/" + props.ticketCategoryData.event.altId.toUpperCase()}>{props.ticketCategoryData.eventDetail[0].name}</Link></li>
                </ol>
            </nav>
        </div>
    </div>
}

