import * as React from "react";
import { checkPlaceholderImg } from "../../utils/checkPlacehoderImg";

export default class ImageComponent extends React.Component<any, any> {
    public constructor(props) {
        super(props);
        this.state = {
            imgUrl: ''
        };
    }

    async componentDidMount() {
        this.setState({
            imgUrl: await checkPlaceholderImg(
                this.props.parentCategorySlug,
                this.props.subCategorySlug,
                'places/tiles',
                'tiles',
                'https://feelitlive.imgix.net/images')
        });
    }

    render() {
        if (this.props.searchImage) {
            return <img src={this.props.imgUrl.replace("feelaplace", "feelitlive")} alt="shop local" width="50"
                onError={(e) => {
                    e.currentTarget.src = this.state.imgUrl
                }} className="mr-2 align-top" />
        } else {
            return <img
                src={
                    this.props.imgUrl.replace("feelaplace", "feelitlive")
                }
                className="card-img-top"
                onError={(e) => {
                    e.currentTarget.src = `https://feelitlive.imgix.net/images/places/tiles/${this.props.subCategorySlug && this.props.subCategorySlug.trim().toLowerCase().replace(/\s/g, "-").replace(/&/g, "and")}-placeholder.jpg?auto=format&fit=crop&${this.props.noOfSlides == 3 ? 'h=270&w=360' : 'h=200&w=270'}&crop=entropy&q=55`;
                    // e.currentTarget.src = `${this.state.imgUrl} ${this.props.isCountry ? '' : '?auto=format&fit=crop&h=159&w=212&crop=entropy&q=55'}`
                }} alt="feelitLIVE"
            />
        }
    }
};
