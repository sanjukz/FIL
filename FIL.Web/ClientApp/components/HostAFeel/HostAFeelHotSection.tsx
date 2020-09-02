import * as React from 'react';
import { connect } from "react-redux";
import { IApplicationState } from "../../stores/index";
import * as FeelPlaces from "../../stores/FeelPlaces";
import { actionCreators as sessionActionCreators } from "shared/stores/Session";
import { bindActionCreators } from "redux";
import MonumentTicketsV1 from "../Home/MonumentTicketsV1";
import MonumentTicketSkeleton from '../Home/MonumentTicketSkeleton';
import { lazyload } from 'react-lazyload';

@lazyload({
    height: 200,
    once: true,
    offset: 200
})
class IconicSectionV1 extends React.PureComponent<any, any> {
    componentDidMount() {
        if (!this.props.categoryPlaces.placesBySlug["see-and-do"] ||
            !this.props.categoryPlaces.placesBySlug["see-and-do"].length) {
            this.props.getCategoryPlaces("all-top-29", (response) => { });
        }
    }

    public goToLogin() {
        localStorage.setItem("isRedirectHomePage", "true");
        this.props.history.push("/login");
    }

    render() {
        if (this.props.categoryPlaces.fetchSuccess && this.props.categoryPlaces.placesBySlug["see-and-do"] && this.props.categoryPlaces.placesBySlug["see-and-do"].length == 0) {
            return <></>
        }
        return (this.props.categoryPlaces.placesBySlug["see-and-do"] && this.props.categoryPlaces.placesBySlug["see-and-do"].length) ?
            <div className="hostAfeel-page fil-site">
                <section className="fil-tiles-sec">
                    <div className="container">
                        <h2 style={{ fontSize: "2rem" }} title="Explore other experiences" className="text-center mb-3 pb-3">Explore other experiences</h2>
                        <MonumentTicketsV1
                            categoryData={this.props.categoryPlaces.placesBySlug["see-and-do"]}
                            count={4}
                            session={this.props.session}
                        />
                        <a href="/c/see-and-do?category=29" className="show-all-link">Show all ❯</a>
                    </div>
                </section></div> : <div className="all-feels sec-spacing">
                <div className="nav-tab-content container">
                    <div className="row">
                        <div className="col-sm-12 p-0">
                            <h2 style={{ fontSize: "2rem" }} title="Explore other experiences" className="text-center mb-3 pb-3">Explore other experiences</h2>
                        </div>
                        <MonumentTicketSkeleton number={4} />
                    </div>
                </div>
            </div>
    }
}

const mapState = (state: IApplicationState) => {
    return {
        categoryPlaces: state.categoryPlaces,
        session: state.session
    };
};

const mapDispatch = (dispatch) => bindActionCreators({ ...FeelPlaces.actionCreators, ...sessionActionCreators }, dispatch);

export default connect(mapState, mapDispatch)(IconicSectionV1)
