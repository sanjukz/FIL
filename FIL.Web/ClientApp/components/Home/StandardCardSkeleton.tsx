import * as React from "react";
import ContentLoader from "react-content-loader";

export const FeelStandardPlaceholder: React.SFC<any> = (props) => (
    <ContentLoader
        height={170}
        width={165}
        speed={1}
        primaryColor="#d9d9d9"
        secondaryColor="#ecebeb"
    >
        <rect x="0" y="0" rx="0" ry="0" width="200" height="120" />
        <rect x="0" y="125" rx="0" ry="0" width="160" height="14" />
        <rect x="0" y="142" rx="0" ry="0" width="120" height="8" />
        <rect x="0" y="153" rx="0" ry="0" width="80" height="8" />
        <rect x="90" y="153" rx="0" ry="0" width="80" height="8" />
        <rect x="0" y="165" rx="0" ry="0" width="80" height="8" />
    </ContentLoader>
);