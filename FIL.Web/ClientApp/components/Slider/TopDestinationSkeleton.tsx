import * as React from 'react'
import ContentLoader from 'react-content-loader'

const TopDestinationSkeleton:React.SFC<any> = props => {
  return (
    <ContentLoader
      height={500}
      width={1644}
      speed={1}
      primaryColor="#f3f3f3"
      secondaryColor="#ecebeb"
    >
      <rect x="473" y="0" rx="0" ry="0" width="700" height="500" />
      <rect x="239" y="63" rx="0" ry="0" width="643" height="274" />
      <rect x="30" y="76" rx="0" ry="0" width="527" height="208" />
      <rect x="762" y="63" rx="0" ry="0" width="643" height="274" />
      <rect x="1087" y="76" rx="0" ry="0" width="527" height="208" />
    </ContentLoader>
  )
};

export default TopDestinationSkeleton