import * as React from "react";
import { checkPlaceholderImg } from "../../utils/checkPlacehoderImg";

export default class EventImageV1 extends React.Component<any, any> {
    state = {
        imgUrl: ``,
        baseImgixUrl: 'https://feelitlive.imgix.net/images'
    };

    public render() {
        const { baseImgixUrl } = this.state;

        return (
            <img src={`${baseImgixUrl}/places/tiles/${this.props.altId.toLocaleUpperCase()}-ht-c1.jpg?auto=format&fit=crop&${this.props.noOfSlides == 3 ? 'h=270&w=360' : 'h=200&w=270'}&crop=entropy&q=55`}
                onError={e => {
                    e.currentTarget.src = `${baseImgixUrl}/places/tiles/${this.props.category && this.props.category.trim().toLowerCase().replace(/\s/g, "-").replace(/&/g, "and")}-placeholder.jpg?auto=format&fit=crop&${this.props.noOfSlides == 3 ? 'h=270&w=360' : 'h=200&w=270'}&crop=entropy&q=55`;
                }}
                alt="feelitLIVE" />
        );
    }
}
