import * as React from 'react';
import {
    Link, RouteComponentProps, Route, Switch
} from "react-router-dom";

interface Iprops {
    title: string;
}
function BreadcrumbAndTitle(props: Iprops) {
    return <>
        <p>
            <Link to="/account" className="text-reset text-decoration-none text-body"
            >My Account</Link>
            <span className="px-3"> ❯ </span> {props.title}
        </p>
        <h2 className="mb-5">{props.title}</h2>

    </>
}
export default BreadcrumbAndTitle;