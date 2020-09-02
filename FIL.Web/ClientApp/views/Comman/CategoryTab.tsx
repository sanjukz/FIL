import * as React from "react";
import { gets3BaseUrl } from "../../utils/imageCdn";

export default class CategoryTab extends React.Component<any, any> {
    public render() {
        return <div className="page-navigation text-center">
            <ul className="nav nav-pills resp-tabs-list pt-3 pb-0">
                {this.props.subCategories.map((item, index) => (
                    <a href={item.url + item.query}>
                        <li>
                            <span className="tabs-icon mb-0">
                                <img src={`${gets3BaseUrl()}/category-tab-icon/${item.slug}.svg`}
                                    alt="tabs icon" />
                            </span>
                            <p className="tabs-title mt-2 m-0">{item.displayName}</p>
                        </li>
                    </a>
                ))}
            </ul>
        </div>
    }
}

