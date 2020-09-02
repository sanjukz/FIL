import * as React from "react";
import { BlogPost } from "../../models/Blog/FeelBlogModel";
import { stripHtmlTags } from "../../utils/stripHtmlTags";

interface IPostProps {
  post: BlogPost;
}

const BlogPost: React.SFC<IPostProps> = ({ post }) => {
  return (
    <div className="col-12 col-sm-6 col-lg-4 mb-3">
      <a
        href={post.link}
        target="_blank"
        className="text-decoration-none d-block"
      >
        <div className="card">
          <img
            style={{
              objectFit: "cover",
              width: "100%",
              height: "200px"
            }}
            src={post.jetpack_featured_media_url}
            className="card-img-top"
            alt="Blog Post Image"
          />
          <div className="card-body position-relative">
            <h6
              className="card-title m-0"
              dangerouslySetInnerHTML={{ __html: post.title.rendered }}
            ></h6>
            <p className="card-text">
              <small>{`${stripHtmlTags(post.excerpt.rendered).substr(
                0,
                150
              )}...`}</small>
            </p>
            <p className="card-text">
              <small className="text-muted text-underline">Read More...</small>
            </p>
          </div>
        </div>
      </a>
    </div>
  );
};

export default BlogPost;
