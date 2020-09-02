import * as React from "react";
import { gets3BaseUrl } from "../../../utils/imageCdn";

export default class DocumentImageUpload extends React.Component<any, any> {

    public constructor(props) {
        super(props);
        this.state = { 
            imgTile1: `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`,
        }
    }

    clickedPlaceholderId: any;
    imagesToAppendInFormData = [];
    clickedFieldId: any;
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
        element.click();
    };

    handleTilesInputFileChange = (event: any) => {
        //hiddent input file change
        const files = Array.from(event.target.files);
        var placeholderid = this.clickedPlaceholderId;
        var element = document.getElementById(
            this.clickedFieldId
        ) as HTMLInputElement;
        if (element.files && element.files[0]) {
            var reader = new FileReader();
            reader.onload = function () {
                var imgEle = document.getElementById(placeholderid) as HTMLImageElement;
                imgEle.src = reader.result as string;
            };
            let fileSize = element.files[0].size/1024/1024;
            if (fileSize > 1 ) {
                alert('File size should not above 1MB');
            } else {
                reader.readAsDataURL(element.files[0]);
                this.tilesliderimages = event.target.files[0];
                this.props.uploadImage(this.tilesliderimages);
            }            
        }
    };

    render() {
        return (
            <div className="form-group">
                <label className="d-block">Document image</label>
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
                <span
                    onClick={() =>
                        this.handleRemove(
                            "img1View",
                            `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`
                        )
                    }
                    className="remove-button text-primary"
                >
                    <i className="fa fa-remove" aria-hidden="true" />
                </span>
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
            </div>
        )
    }
}
