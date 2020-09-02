import * as React from 'react';
import { connect } from "react-redux";
import { IApplicationState } from '../../stores';
import CenterMode from '../Slider/CenterMode';
import TopDestinationSkeleton from "../Slider/TopDestinationSkeleton";
import { lazyload } from 'react-lazyload';
import * as CityCountryStore from "../../stores/CityCountry";
import { bindActionCreators } from 'redux';

@lazyload({
    height: 200,
    once: true,
    offset: 200
})
class TopDestinationSectionV1 extends React.PureComponent<any, any> {
    componentDidMount() {
        if (!this.props.countryPlacesCount.fetchCountryallPlaceData && !this.props.countryPlacesCount.isLoading) {
            this.props.requestCountryPlaceData(32);
        }
    }
    render() {
        let countryCounts = this.props.countryPlacesCount.cityCountry.countryPlace || [];
        if (countryCounts.length > 0) {
            return (
                <section className="fil-tiles-sec fil-top-des feel-top-destination">
                    <div className="container">
                        <h3 title="FeelitLIVE Top Destinations" className="text-purple">FeelitLIVE Top Destinations</h3>
                        <CenterMode countryPlace={countryCounts} />
                    </div>
                </section>
            );
        }
        return (
            <section className="fil-tiles-sec fil-top-des">
                <div className="container">
                    <h3 title="FeelitLIVE Top Destinations" className="text-purple">FeelitLIVE Top Destinations</h3>
                    <TopDestinationSkeleton />
                </div>
            </section>
        );
    }
}

const mapState = (state: IApplicationState) => {
    return {
        countryPlacesCount: state.cityCountry
    };
}
const mapDispatch = (dispatch) => bindActionCreators({ ...CityCountryStore.actionCreators }, dispatch);


export default connect(mapState, mapDispatch)(TopDestinationSectionV1); 