import * as React from "react";

export default class SubCategoryTab extends React.PureComponent<any, any> {
    public render() {
        var subCategories = this.props.subCategories.map((item) => {
            return <li className="d-inline-block" onClick={() => { typeof window != "undefined" && window.sessionStorage.setItem("category", item.displayName) }}>
                <a className="btn btn-sm btn-outline-secondary mx-2" href={item.url + item.query}>{item.displayName}</a>
            </li>
        });
        return <div className="page-navigation text-center bg-light py-3">
            <div className="tab-content">
                <ul className="tab-pane fade show active px-2 m-0">
                    {subCategories}
                </ul>
            </div>
        </div>;
    }
}

