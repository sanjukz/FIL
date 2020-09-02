import * as React from "react";
import { gets3BaseUrl } from "../../../utils/imageCdn";
import { Tooltip } from 'antd';

export class ImageDetailModal {
    id: string;
    file: any;
    path: string;
}

export default class DocumentImageUpload extends React.Component<any, any> {

    public constructor(props) {
        super(props);
        this.state = {
            imgTile1: `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`,
            images: [],
            imgDescBanner: `${gets3BaseUrl()}/places/about/learn-more-banner-placeholder.jpg`
        }
    }

    clickedPlaceholderId: any;
    imagesToAppendInFormData = [];
    clickedFieldId: any;
    clickedBanrDescPlaceholderId: any;
    clickedBanrDescFieldId: any;
    tilesliderimages = [];

    handleRemove = (id, url) => {
        var imgEle = document.getElementById(id) as HTMLImageElement;
        imgEle.src = url as string;
    };

    handleTilesSlidersClick = (id, imgPlaceholderId) => {
        //image item click and id of input field
        this.clickedFieldId = id;
        this.clickedPlaceholderId = imgPlaceholderId;
        var element = document.getElementById(
            this.clickedFieldId
        ) as HTMLInputElement;
        this.setState({ currentElement: element }, () => {
            element.click();
        });
    };

    // Description Banner image
    handleDescBannerClick = (id, imgPlaceholderId) => {
        this.clickedBanrDescFieldId = id;
        this.clickedBanrDescPlaceholderId = imgPlaceholderId;
        var element = document.getElementById(
            this.clickedBanrDescFieldId
        ) as HTMLInputElement;
        this.setState({ currentElement: element }, () => {
            element.click();
        });
    };

    handleTilesInputFileChange = (event: any) => {
        let that = this;
        //hiddent input file change
        var placeholderid = this.clickedPlaceholderId;
        var element = this.state.currentElement;
        if (element.files && element.files[0]) {
            var reader = new FileReader();
            reader.onload = function () {
                var imgEle = document.getElementById(placeholderid) as HTMLImageElement;
                imgEle.src = reader.result as string;
            };
            let fileSize = element.files[0].size / 1024 / 1024;
            if (fileSize > 1) {
                alert('File size should not above 1MB');
            } else {
                reader.readAsDataURL(element.files[0]);
                this.tilesliderimages = event.target.files[0];
                let ImageArray = this.state.images;
                ImageArray = ImageArray.filter((item) => {
                    return (item.id != placeholderid && item.path != (that.props.host ? 'images/places/host' : 'images/places/tiles'))
                });

                let imageTiles: ImageDetailModal = {
                    id: placeholderid,
                    file: this.tilesliderimages,
                    path: that.props.host ? 'images/places/host' : 'images/places/tiles'
                }
                ImageArray.push(imageTiles);
                this.setState({ images: ImageArray }, () => {
                    this.props.uploadImage(ImageArray);
                });
            }
        }
    };

    handleDescBannerInputFileChange = event => {
        var placeholderid = this.clickedBanrDescPlaceholderId;
        var element = this.state.currentElement;
        if (element.files && element.files[0]) {
            var reader = new FileReader();
            reader.onload = function () {
                var imgEle = document.getElementById(placeholderid) as HTMLImageElement;
                imgEle.src = reader.result as string;
            };
            let fileSize = element.files[0].size / 1024 / 1024;
            if (fileSize > 2) {
                alert('File size should not above 2MB');
            } else {
                reader.readAsDataURL(element.files[0]);
                let ImageArray = this.state.images;
                ImageArray = ImageArray.filter((item) => { return (item.id != placeholderid && (item.path != 'images/places/about' && item.path != 'places/InnerBanner')) });
                let imageLearnPage: ImageDetailModal = {
                    id: placeholderid,
                    file: event.target.files[0],
                    path: 'images/places/about'
                }
                let imageTicketCat: ImageDetailModal = {
                    id: placeholderid,
                    file: event.target.files[0],
                    path: 'images/places/InnerBanner'
                }
                ImageArray.push(imageLearnPage);
                ImageArray.push(imageTicketCat);
                this.setState({ images: ImageArray }, () => {
                    this.props.uploadImage(ImageArray);
                });
            }
        }
    };

    render() {
        return (
            <div className="col-12">
                {(!this.props.isHost) && <div className="form-group">
                    <label className="d-block">Image 1
                            <Tooltip title={<p><span>This will display in the landing page of the Live Stream section of our site. Therefore, please select one that helps to draw people in. Please ensure adherence to the general website guidelines.</span></p>}>
                            <span><i className="fa fa-info-circle text-primary ml-2" ></i></span>
                        </Tooltip>
                    </label>
                    <input type="hidden" name="placemapImages" />
                    <input
                        disabled={this.state.isImagesEdit}
                        onChange={e => {
                            this.handleTilesInputFileChange(e);
                        }}
                        className="hidden"
                        accept="image/*"
                        type="file"
                        id="img1"
                        name="tilesSliderImages"
                    />
                    {this.state.images.length > 0 && <span
                        onClick={() =>
                            this.handleRemove(
                                "img1View",
                                `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`
                            )
                        }
                        className="remove-button text-primary"
                    >
                        <i className="fa fa-remove" aria-hidden="true" />
                    </span>}
                    <span
                        onClick={() =>
                            this.handleTilesSlidersClick("img1", "img1View")
                        }
                        className="hyperref"
                    >

                        <img
                            id="img1View"
                            src={this.state.imgTile1}
                            alt="place thumb"
                            onError={(e) => {
                                e.currentTarget.src = `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`
                            }}
                            width="120"
                            className="rounded mr-1"
                        />
                    </span>
                    <small className="form-text text-muted mb-2">
                        We recommend using at least a 600px x 381px image that's no larger than 1mb.
                    </small>
                </div>}
                {(this.props.isHost) && <div className="form-group">
                    <label className="d-block">Image</label>
                    <input type="hidden" name="placemapImages" />
                    <input
                        disabled={this.state.isImagesEdit}
                        onChange={e => {
                            this.handleTilesInputFileChange(e);
                        }}
                        className="hidden"
                        accept="image/*"
                        type="file"
                        id={`imgHost${this.props.hostId}`}
                        name="tilesSliderImages"
                    />
                    {this.state.images.length > 0 && <span
                        onClick={() =>
                            this.handleRemove(
                                `imgHost${this.props.hostId}View`,
                                `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`
                            )
                        }
                        className="remove-button text-primary"
                    >
                        <i className="fa fa-remove" aria-hidden="true" />
                    </span>}
                    <span
                        onClick={() =>
                            this.handleTilesSlidersClick(`imgHost${this.props.hostId}`, `imgHost${this.props.hostId}View`)
                        }
                        className="hyperref"
                    >

                        <img
                            id={`imgHost${this.props.hostId}View`}
                            src={this.state.imgTile1}
                            alt="place thumb"
                            onError={(e) => {
                                e.currentTarget.src = `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`
                            }}
                            width="120"
                            className="rounded mr-1"
                        />
                    </span>
                </div>}
                {(this.props.isEventCreation) && <div className="form-group">
                    <label className="d-block">Image 2
                            <Tooltip title={<p><span>This is the banner for your event and will display on your event page. Hence a nice catchy attractive image is needed. Please ensure adherence to the general website guidelines.</span></p>}>
                            <span><i className="fa fa-info-circle text-primary ml-2" ></i></span>
                        </Tooltip>
                    </label>
                    <input type="hidden" name="placemapImages" />
                    <input
                        disabled={this.state.isImagesEdit}
                        onChange={e => {
                            this.handleDescBannerInputFileChange(e);
                        }}
                        className="hidden"
                        accept="image/*"
                        type="file"
                        id="descbanner"
                        name="descbannerImages"
                    />
                    <span
                        onClick={() =>
                            this.handleRemove(
                                "descbannerview",
                                `${gets3BaseUrl()}/places/about/learn-more-banner-placeholder.jpg`
                            )
                        }
                        className="remove-button text-primary"
                    >
                        <i className="fa fa-remove" aria-hidden="true" />
                    </span>
                    <span
                        onClick={() =>
                            this.handleDescBannerClick(
                                "descbanner",
                                "descbannerview"
                            )
                        }
                        className="hyperref"
                    >

                        <img
                            id="descbannerview"
                            src={this.state.imgDescBanner}
                            alt="place thumb"
                            onError={(e) => {
                                e.currentTarget.src = `${gets3BaseUrl()}/places/about/learn-more-banner-placeholder.jpg`
                            }}
                            width="300"
                            height="89"
                            className="rounded"
                        />
                    </span>
                    <small className="form-text text-muted mb-2">
                        We recommend using at least a 1920px x 570px image that's no larger than 2mb.
                    </small>
                </div>}
            </div>
        )
    }
}
