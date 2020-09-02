import * as React from "React";
import { autobind } from "core-decorators";
import ImageGallery from 'react-image-gallery';
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";
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
                'places/about',
                'learn-more-banner',
                this.state.s3BaseUrl
            )
        })
    }

    @autobind
    onBreadcrumClick(parentCat, childCat, isMainCatClick) {
        var data = {
            parentCat: parentCat,
            childCat: childCat,
            isMainCatClick: isMainCatClick
        }
        localStorage.setItem("isBreadCrumClick", "true");
        localStorage.setItem("selectedBreadCrumCat", JSON.stringify(data));
        //   this.props.gotToHome()
        this.props.history.push("/");
    }
    public render() {
        var imageList = this.props.eventImageList;
        var images = [];
        var imageSlider = [];

        for (var i = 1; i <= imageList.length; i++) {
            images.push(`${this.state.s3BaseUrl}/places/about/photo-gallery/` + this.props.eventImageId.toUpperCase() + `-glr-` + i + `.jpg`);
            imageSlider.push({
                original: `${this.state.s3BaseUrl}/places/about/photo-gallery/` + this.props.eventImageId.toUpperCase() + `-glr-` + i + `.jpg`,
                thumbnail: `${this.state.s3BaseUrl}/places/about/photo-gallery/` + this.props.eventImageId.toUpperCase() + `-glr-` + i + `.jpg`,
            });
        }

        const { photoIndex, isOpen } = this.state;
        var countryName = this.props.country && this.props.country.split(' ').join('').split('.').join('');
        var altText = "Feel " + this.props.eventName + " when you visit " + this.props.city + ", " + this.props.country + "";
        return <>
            <div className="inner-banner photogallery mt-5 mt-sm-0">
                <img
                    src={
                        this.props.isCityCountryPage
                            ? `${this.state.s3BaseUrl}/country/${countryName.toLowerCase()}.jpg`
                            : `${this.state.s3BaseUrl}/places/about/${this.props.eventImageId.toUpperCase()}-about.jpg`

                    }
                    onError={e => {
                        e.currentTarget.src = `https://static6.feelitlive.com/images/places/tiles/${this.props.eventCat && this.props.eventCat.trim().toLowerCase().replace(/\s/g, "-").replace(/&/g, "and")}-placeholder.jpg`;
                    }}
                    alt={altText}
                    className="card-img"
                />
                <div className="card-img-overlay">
                    <nav aria-label="breadcrumb">
                        <ol className="breadcrumb bg-white m-0 p-1 pl-2 d-inline-block">
                            <li className="breadcrumb-item"><Link to="/" >Home</Link></li>
                            <li className="breadcrumb-item"><Link to={`/c/${this.props.category.slug}?category=${this.props.category.id}`} >{this.props.category.displayName}</Link></li>
                            <li className="breadcrumb-item"><Link to={`/c/${this.props.category.slug}/${this.props.subCategory.slug}?category=${this.props.category.id}&subcategory=${this.props.subCategory.id}`} >{this.props.subCategory.displayName}</Link></li>
                            {this.props.eventCategoryName === "Country" || this.props.eventCategoryName === "City" ? null : <li className="breadcrumb-item active" aria-current="page">{this.props.eventName}</li>}
                        </ol>
                    </nav>
                </div>
                {(this.props.eventImageList.length > 0) ? (<Button onClick={() => this.setState({ isOpen: true })} className="btn btn-light text-uppercase"> <i className="fa fa-file-image-o"></i> View Photos</Button>) : ""}
                {
                    this.state.isOpen && <div id="myModal" className="modal d-block">
                        <span onClick={() => this.setState({ isOpen: false })} className="close cursor">&times;</span>
                        <ImageGallery items={imageSlider} />
                    </div>
                }
            </div>
        </>;
    }
}

