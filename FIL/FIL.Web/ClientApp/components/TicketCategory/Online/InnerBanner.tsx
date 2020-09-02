import * as React from "React";
import { checkPlaceholderImg } from "../../../utils/checkPlacehoderImg";
import { gets3BaseUrl } from "../../../utils/imageCdn";

export default class InnerBanner extends React.Component<any, any> {
    public constructor(props) {
        super(props);
        this.state = {
            photoIndex: 0,
            isOpen: false,
            s3BaseUrl: gets3BaseUrl()
        };
    }

    async componentDidMount() {
        this.setState({
            imgUrl: await checkPlaceholderImg(
                this.props.category.slug,
                this.props.subCategory.slug,
                'places/InnerBanner',
                'sml-banner',
                this.state.s3BaseUrl
            )
        })
    }

    public render() {
        return <img src={`${this.state.s3BaseUrl}/places/InnerBanner/` + this.props.eventAltId.toUpperCase() + `.jpg`} onError={(e) => {
            // e.currentTarget.src = this.state.imgUrl
        }} alt="" className="card-img" />;
    }
}

