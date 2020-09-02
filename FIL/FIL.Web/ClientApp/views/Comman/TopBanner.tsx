import * as React from "react";

export default class TopBanner extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
        }
    }
    render() {
        var fallbackImg = this.props.fallbackImage;
        return (
            <div className="inner-banner photogallery" >
                <img src={this.props.img} alt="top banner image" className="card-img"
                    onError={(e) => {
                        e.currentTarget.src = fallbackImg
                    }} />
            </div>
        );
    }
}
