import * as React from 'react';
import axios from "axios";
import BlogPost from "./BlogPost";
import { lazyload } from 'react-lazyload';

@lazyload({
    height: 200,
    once: true,
    offset: 200
})
export default class FeelHomeBlogSection extends React.PureComponent<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            posts: []
        };
    }

    async componentDidMount() {
        const res = await axios.get("https://feelitlive.blog/wp-json/wp/v2/posts");
        this.setState({
            posts: res.data.splice(0, 3)
        });
    }

    render() {
        return (
            <section className="feelsBlogHome sec-spacing">
                <div className="container p-0">
                    <h4 className="mb-4">Why “feel” and not simply travel!</h4>
                    <div id="feelsBlogHome" className="carousel slide" data-ride="carousel">
                        <div className="carousel-inner">
                            <div className="carousel-item active" data-interval="10000">
                                <div className="row">
                                    {this.state.posts.map((item) => {
                                        return <BlogPost key={item.id} post={item} />
                                    })}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section >
        );
    }
}
