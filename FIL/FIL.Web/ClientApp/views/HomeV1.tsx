import * as React from "react";
import { IApplicationState } from "../stores";
import * as FeelSiteDynamicLayout from "../stores/FeelSiteDynamicLayout";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import "../scss/fil-style.scss";
import { gets3BaseUrl } from "../utils/imageCdn";
import HomeBannerSectionV1 from "../components/Home/HomeBannerV1";
import HomeCategorySectionV1 from "../components/Home/HomeCategorySectionV1";
import LiveOnlineSectionV1 from "../components/Home/LiveOnlineSectionV1";
import IconicSectionV1 from "../components/Home/IconicSection";
import TopDestinationSectionV1 from "../components/Home/TopDestinationSectionV1";
import ItinerarySectionV1 from "../components/Home/ItinerarySectionV1";
import FeelHomeBlogSectionV1 from "../components/Home/FeelHomeBlogV1";
import * as FeelPlacesStore from "../stores/FeelPlaces";
import { Modal } from "antd";

class HomeV1 extends React.PureComponent<any, any> {
  inviteenabled = false;
  state = {
    s3BaseUrl: gets3BaseUrl(),
    isShowModal: false,
  };
  public componentDidMount() {
    this.props.getAllSections(1);
    /* if (window && window.sessionStorage) {
             if (sessionStorage.getItem('isModal') == null) {
                 this.setState({ isShowModal: true });
                 sessionStorage.setItem('isModal', 'true');
             }
         } */
  }

  getSectionBySelectedMenu = () => {
    if (this.props.categoryPlaces.isHideLiveSection) {
      return (
        <>
          <IconicSectionV1 />
          <LiveOnlineSectionV1 />
        </>
      );
    } else {
      return (
        <>
          <LiveOnlineSectionV1 />
          <IconicSectionV1 />
        </>
      );
    }
  };

  public render() {
    let modalWidth = "400";
    if (typeof window != "undefined" && window.screen.width <= 640) {
      modalWidth = "300";
    }
    return (
      <div className="fil-site fil-home-page ">
        <Modal
          visible={this.state.isShowModal}
          title={""}
          maskClosable={false}
          footer={null}
          style={{ bottom: 20, left: 20, top: "auto", position: "fixed" }}
        >
          <a href="ticket-alert">
            <img
              src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/inner-banner/hands-upon-the-plow-popup.jpg"
              style={{ width: "100%" }}
            />
          </a>
        </Modal>
        <HomeBannerSectionV1 />

        <HomeCategorySectionV1 />

        {this.getSectionBySelectedMenu()}

        <TopDestinationSectionV1 />

        <ItinerarySectionV1 />

        <FeelHomeBlogSectionV1 />
      </div>
    );
  }
}

export default connect(
  (state: IApplicationState) => ({
    pageMetaData: state.pageMetaData,
    categoryPlaces: state.categoryPlaces,
  }),
  (dispatch) =>
    bindActionCreators(
      {
        ...FeelSiteDynamicLayout.actionCreators,
        ...FeelPlacesStore.actionCreators,
      },
      dispatch
    )
)(HomeV1);
