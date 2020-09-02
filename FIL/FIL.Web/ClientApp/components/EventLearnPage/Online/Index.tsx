import * as React from 'react';
import "../../../scss/fil-exp-landing.scss";
import ExperienceAndHostDetail from './ExperienceAndHostDetail';
import ImportantThingsAndReviews from './ImportantThingsAndReviews';
import LiveOnlineSectionV1 from '../../../components/Home/LiveOnlineSectionV1';
import { EventLearnPageViewModel } from '../../../models/EventLearnPageViewModel';
import Banner from './Banner';

interface Iprops {
    eventData: EventLearnPageViewModel;
    isAthenticated: boolean;
    onSubmit: (rating: any, review: any) => void;
}
export default class Online extends React.PureComponent<any, any>{
    render() {
        return (
            <div className="fil-site fil-exp-landing-page">

                <Banner eventData={this.props.eventData} isAthenticated={this.props.isAthenticated} />

                <ExperienceAndHostDetail eventData={this.props.eventData} />

                <ImportantThingsAndReviews onSubmit={this.props.onSubmit} {...this.props} isLiveStream={true} />

                <LiveOnlineSectionV1 isExperiencePage={true} />

            </div>
        )
    }
}