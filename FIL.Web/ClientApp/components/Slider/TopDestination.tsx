import * as React from 'react';
import { connect } from "react-redux";
import * as CityCountryStore from "../../stores/CityCountry";
import { IApplicationState } from '../../stores';
import { bindActionCreators } from 'redux';
import CenterMode from './CenterMode';
import TopDestinationSkeleton from "./TopDestinationSkeleton";
import { lazyload } from 'react-lazyload';

@lazyload({
    height: 200,
    once: true,
    offset: 200
})
class TopDestinationSection extends React.PureComponent<any, any> {
    render() {
        let countryCounts = this.props.countryPlacesCount.cityCountry.countryPlace || [];
        if (countryCounts.length > 0) {
            return (
                <section className="feel-top-destination sec-spacing">
                    <div className="container p-0">
                        <h4 className="m-0">Feel our Top Destinations</h4>
                        <p className="">Belong here.</p>
                        <CenterMode countryPlace={countryCounts} />
                    </div>
                </section>
            );
        }
        return (
            <section className="feel-top-destination sec-spacing">
                <div className="container p-0">
                    <h4 className="m-0">Feel our Top Destinations</h4>
                    <p className="">Belong here.</p>
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

export default connect(mapState, null)(TopDestinationSection); 