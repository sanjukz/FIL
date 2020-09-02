import * as React from "react";
import { BlogPost } from "../../models/Blog/FeelBlogModel";
import { stripHtmlTags } from "../../utils/stripHtmlTags";

interface IPostProps {
    post: BlogPost;
}

const BlogPostV1: React.SFC<IPostProps> = ({ post }) => {
    return (
        <div className="col-sm-4">
            <div className="card rounded-box shadow border-0 h-100">
                <img src={`https://feelitlive.imgix.net/images/blogs/${post.id}.jpg?auto=format&fit=crop&h=221&w=360&crop=entropy&q=55`} alt="FIL Blog" />
                <div className="card-body">
                    <h5 className="card-title m-0" dangerouslySetInnerHTML={{ __html: post.title.rendered }}></h5>
                    <p className="card-text iconlink mb-1" dangerouslySetInnerHTML={{ __html: `${stripHtmlTags(post.excerpt.rendered).substr(0, 150)}...` }} />
                    <a href={post.link}
                        target="_blank" className="card-text">Read more »</a>
                </div>
            </div>
        </div>
    );
};

export default BlogPostV1;
