import * as React from "react";
import ContentLoader from "react-content-loader";

const HomeCategorySkeleton: React.SFC<any> = (props) => (
  <ContentLoader
    height={120}
    width={120}
    speed={1}
    primaryColor="#d9d9d9"
    secondaryColor="#ecebeb"
    style={{
      maxWidth: "90",
      minWidth: "120px",
      padding: "10px"
    }}
  >
    <circle cx="54" cy="52" r="52" />
    <rect x="0" y="115" rx="0" ry="0" width="111" height="20" />
  </ContentLoader>
);

export default HomeCategorySkeleton;
