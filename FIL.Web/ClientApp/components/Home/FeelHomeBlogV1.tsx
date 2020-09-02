import * as React from 'react';
import axios from "axios";
import BlogPostV1 from "./BlogPostV1";
import { lazyload } from 'react-lazyload';
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { IApplicationState } from "../../stores";
import * as SiteAsset from "../../stores/SiteAsset";
import BlogResponseModelList from '../../models/BlogResponseModel';
import "../../scss/fil-style.scss";

@lazyload({
    height: 200,
    once: true,
    offset: 200
})
class FeelHomeBlogSectionV1 extends React.PureComponent<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            posts: []
        };
    }

    componentDidMount() {
        this.props.requestBlogContent((response: BlogResponseModelList) => {
            this.setState({
                posts: response.blogResponseModelList.splice(0, 3)
            });
        })

    }

    render() {
        return (
            <section className="fil-tiles-sec fil-blog-sec">
                <div className="container">
                    <h3 title="The latest from FeelitLIVE" className="text-purple">The latest from FeelitLIVE</h3>
                    <div className="row fil-latest">
                            {this.state.posts.map((item) => {
                                return <BlogPostV1 key={item.id} post={item} />
                            })}
                    </div>
                </div>
            </section>
        );
    }
}
export default connect(
    (state: IApplicationState) => ({
        siteAsset: state.siteAsset
    }),
    dispatch =>
        bindActionCreators({ ...SiteAsset.actionCreators }, dispatch)
)(FeelHomeBlogSectionV1);