/* global google */
var google: any;
import * as React from "React";
import { autobind } from "core-decorators";
import Yup from "yup";
import { EventCreationViewModel } from "../../models/EventCreation/EventCreationViewModel";
import { Formik, Field, Form } from "formik";
import Select from "react-select";
import axios from "axios";
import { EditorState } from "draft-js";
import "./event.css";
import CKEditor from "react-ckeditor-component";
import Newloader from "../../components/NewLoader/NewLoader";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import PlaceAutocomplete from "./PlaceAutocomplete";
import { AmenityCreationViewModel } from "../../../ClientApp/models/EventCreation/AmenityCreationViewModel";
import { gets3BaseUrl } from "../../../ClientApp/utils/imageCdn";

interface ISaveEvent {
    onSubmit: (values: EventCreationViewModel) => void;
    eventCat: any;
    name: any;
    description: any;
    metaDetails: any;
    termsAndConditions: any;
    requestAmenities: any;
    amenities: any;
    createPlace: any;
    saveAmenity: any;
    addedAmenityId: any;
    EventSaveSuccessful: boolean;
    EventSaveFailure: boolean;
    RequestSavedDataFromEventId: any;
    SavedEventData: any;
    allStoreState: any;
}
interface ImageModel {
    fileName: string;
    type: string;
    size: string;
}

export default class AddEvent extends React.Component<ISaveEvent, any> {
    constructor(props) {
        super(props);
        this.state = {
            isLoading: false,
            imgTile1:
                `{${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`,
            imgTile2:
                `{${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`,
            imgTile3:
                `{${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`,
            imgDescBanner:
                `{${gets3BaseUrl()}/places/about/learn-more-banner-placeholder.jpg`,
            imgInventoryPageBanner:
                `{${gets3BaseUrl()}/places/InnerBanner/sml-banner-placeholder.jpg`,
            imgGallery:
                `{${gets3BaseUrl()}/places/InnerBanner/sml-banner-placeholder.jpg`,
            eventId: 0,
            detailAccordianEditComponent: <div />,
            isDetailEdit: false,
            isDescriptionEdit: false,
            isImagesEdit: false,
            isSEOEdit: false,
            isPropsDataCalled: false,
            editorState: EditorState.createEmpty(),
            selectedCategory: null,
            selectedTags: null,
            selectedSubCategory: null,
            selectedAmenity: "",
            isEditMode: false,
            street: "",
            route: "",
            locality: "",
            state: "",
            city: "",
            country: "",
            zip: "",
            customizedAmenity: "",
            placename: "",
            location: "",
            lat: 18.5204,
            long: 73.8567,
            countryshort: "",
            pictures: [],
            showRemoveTileSliderImage1: false,
            showRemoveTileSliderImage2: false,
            showRemoveTileSliderImage3: false,
            showRemoveDescBanner: false,
            showRemoveIventoryPageBanner: false,
            showRemoveGalleryImages: false,
            tilesliderimages: [],
            description: "",
            history: "",
            highlights: "",
            immersiveexperience: "",
            archdetail: "",
            archdetailId: "",
            historyId: "",
            immersiveexperienceId: "",
            highlightsId: "",
            title: "",
            metaTitle: "",
            metsDescription: "",
            metaTags: "",
            isCategoryEdit: false,
            isSubCategoryEdit: false,
            isEminitiesEdit: false,
            altId: "",
            metatags: "",
            metatitle: "",
            metadescription: "",
            minHourTimeDuration: "",
            minMinuteTimeDuration: "",
            maxHourTimeDuration: "",
            maxMinuteTimeDuration: "",
            isAddNewAmenity: false,
            address: "",
            addressLine1: "",
            addressLine2: "",
            isShow: false,
            isFeelGuide: false
        };
        this.onDrop = this.onDrop.bind(this);
    }
    public initialValues: any;
    placeSearch: any;
    autocomplete: any;
    categories = [];
    tags = [];
    subcategories = [];
    allcategories = [];
    amenities = [];
    editId: any;
    editIdArray: any;
    //Image uploading data membersthis
    clickedFieldId: any;
    clickedPlaceholderId: any;
    clickedBanrDescFieldId: any;
    clickedBanrDescPlaceholderId: any;
    clickedInventoryDescFieldId: any;
    clickedInventoryDescPlaceholderId: any;
    clickedGalleryFieldId: any;
    clickedGalleryPlaceholderId: any;
    clickedPlaceMapFieldId: any;
    clickedPlaceMapPlaceholderId: any;
    clickedTimelineFieldId: any;
    clickedTimelinePlaceholderId: any;
    clickedImmersiveFieldId: any;
    clickedImmersivePlaceholderId: any;
    tilesliderimages = [];
    imagesToAppendInFormData = [];


    onDrop(picture) {
        this.setState({
            pictures: this.state.pictures.concat(picture)
        });
    }

    @autobind
    notify() {
        toast.success('Please add zipcode', {
            position: "top-center",
            autoClose: 6000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true
        });
        this.setState({ isShow: true });
    }

    public componentWillMount() { }

    public disableCKEditor() {
        var element = document.getElementsByClassName("cke");
        for (var i = 0; i < element.length; i++) {
            element[i].classList.add("disable-cke");
        }
    }

    public enableCKEditor() {
        var element = document.getElementsByClassName("cke");
        for (var i = 0; i < element.length; i++) {
            element[i].classList.remove("disable-cke");
        }
    }

    public renderMarkers(map, maps) {

        let marker = new maps.Marker({
            position: {
                lat: 18.5204,
                long: 73.8567
            },
            map,
            title: "Hello World!"
        });
    }
    showSaveButton = true;
    public componentDidMount() {
        this.disableCKEditor();
        document.getElementById("nav-Inventory-tab").click();
        document.getElementById("nav-Details-tab").click();
        if (sessionStorage.getItem("isFeelguide")) {
            this.setState({ isFeelGuide: true });
            sessionStorage.removeItem("isFeelguide");
        }
        let urlparts = window.location.href.split("/");
        this.editId = urlparts[urlparts.length - 1];
        this.editIdArray = parseInt(this.editId.split("-").length);
        var isEdit = false;
        var edit = urlparts[urlparts.length - 2];
        if (edit == "edit") {
            isEdit = true;
        }
        if (isEdit) {
            this.showSaveButton = false;
            this.setState({
                isEditMode: true,
                isEminitiesEdit: true,
                isCategoryEdit: true,
                isSubCategoryEdit: true,
                isDetailEdit: true,
                isImagesEdit: true,
                isSEOEdit: true,
                detailAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("Detail")}
                        aria-hidden="true"
                    />
                ),
                ImageAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("Image")}
                        aria-hidden="true"
                    />
                ),
                DescriptionAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("Description")}
                        aria-hidden="true"
                    />
                ),

                SEOAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("SEO")}
                        aria-hidden="true"
                    />
                )
            });
        }
    }

    resetLocation = () => {
        this.setState({
            placename: "",
            city: "",
            state: "",
            zip: "",
            country: "",
            lat: 0.0,
            long: 0.0
        });
    };

    setFetchedInformationForEdit() {
        if (this.editIdArray === 5) {
            if (this.state.isPropsDataCalled === false && this.tags.length > 0) {
                if (this.props.SavedEventData && this.editId.toLocaleLowerCase() == this.editId.toLocaleLowerCase() && this.amenities.length > 0) {

                    var minHourTimeDuration = "";
                    var minMinuteTimeDuration = "";
                    var maxHourTimeDuration = "";
                    var maxMinuteTimeDuration = "";
                    if (this.props.SavedEventData.hourTimeDuration != undefined) {
                        var minMaxTimeDuration = this.props.SavedEventData.hourTimeDuration.split("-");
                        if (minMaxTimeDuration.length >= 2) {
                            var minVisitDuration = minMaxTimeDuration[0].split(":");
                            var maxVisitDuration = minMaxTimeDuration[1].split(":");
                            if (minVisitDuration.length >= 2) {
                                minHourTimeDuration = minVisitDuration[0];
                                minMinuteTimeDuration = minVisitDuration[1];
                            }
                            if (maxVisitDuration.length >= 2) {
                                maxHourTimeDuration = maxVisitDuration[0];
                                maxMinuteTimeDuration = maxVisitDuration[1];
                            }
                        }
                    }
                    this.setState({
                        isDescriptionEdit: true,
                        isLoading: false,
                        imgTile1: `${gets3BaseUrl()}/places/tiles/` + this.props.SavedEventData.altId.toUpperCase() + "-ht-c1.jpg",
                        imgTile2: `${gets3BaseUrl()}/places/tiles/` + this.props.SavedEventData.altId.toUpperCase() + "-ht-c2.jpg",
                        imgTile3: `${gets3BaseUrl()}/places/tiles/` + this.props.SavedEventData.altId.toUpperCase() + "-ht-c3.jpg",
                        imgDescBanner: `${gets3BaseUrl()}/places/about/` + this.props.SavedEventData.altId.toUpperCase() + "-about.jpg",
                        imgInventoryPageBanner: `${gets3BaseUrl()}/places/InnerBanner/` + this.props.SavedEventData.altId.toUpperCase() + ".jpg",
                        imgGallery: `${gets3BaseUrl()}/places/about/photo-gallery/` + this.props.SavedEventData.altId.toUpperCase() + "-glr-1.jpg",
                        altId: this.props.SavedEventData.altId,
                        isPropsDataCalled: true,
                        eventId: this.props.SavedEventData.id,
                        title: this.props.SavedEventData.title,
                        state: this.props.SavedEventData.state,
                        city: this.props.SavedEventData.city,
                        country: this.props.SavedEventData.country,
                        zip: this.props.SavedEventData.zip,
                        placename: this.props.SavedEventData.title,
                        address: this.props.SavedEventData.address1,
                        addressLine1: this.props.SavedEventData.address1,
                        addressLine2: this.props.SavedEventData.address2,
                        description: this.props.SavedEventData.description,
                        history: this.props.SavedEventData.history,
                        highlights: this.props.SavedEventData.highlights,
                        immersiveexperience: this.props.SavedEventData.immersiveexperience,
                        archdetail: this.props.SavedEventData.archdetail,
                        archdetailId: this.props.SavedEventData.archdetailId,
                        historyId: this.props.SavedEventData.historyId,
                        immersiveExperienceId: this.props.SavedEventData
                            .immersiveexperienceId,
                        highlightsId: this.props.SavedEventData.highlightsId,
                        metatags: this.props.SavedEventData.metatags,
                        metatitle: this.props.SavedEventData.metatitle,
                        metadescription: this.props.SavedEventData.metadescription,
                        timeDuration: "",
                        lat: +this.props.SavedEventData.lat,
                        long: +this.props.SavedEventData.long,
                        minHourTimeDuration: minHourTimeDuration,
                        minMinuteTimeDuration: minMinuteTimeDuration,
                        maxHourTimeDuration: maxHourTimeDuration,
                        maxMinuteTimeDuration: maxMinuteTimeDuration,
                    });
                    this.allcategories = this.props.eventCat;
                    var ref = this;

                    var galleryImagesArray = this.props.SavedEventData.galleryImages.split(
                        ","
                    );
                    var placemapImagesArray = this.props.SavedEventData.placemapImages.split(
                        ","
                    );
                    var archdetailImagesArray = this.props.SavedEventData.archdetailImages.split(
                        ","
                    );
                    var timelineImagesArray = this.props.SavedEventData.timelineImages.split(
                        ","
                    );

                    this.disableCKEditor();

                    if (this.state.isCategoryEdit == true && ref.categories.length > 0) {
                        var catId = this.props.SavedEventData.categoryid;
                        this.setState({
                            isCategoryEdit: false,
                            selectedCategory: ref.categories.filter(x => x.value === catId)[0]
                        });
                    }
                    if (
                        this.state.isSubCategoryEdit == true &&
                        ref.allcategories.length > 0
                    ) {
                        var selectedSubCats = [];
                        var selectedTags = [];
                        var subcatsArray = this.props.SavedEventData.subcategoryid.split(
                            ","
                        );
                        var tagArray = this.props.SavedEventData.tagIds.split(
                            ","
                        );
                        for (let i = 0; i < subcatsArray.length; i++) {
                            var selectedSubCatItem = ref.allcategories.filter(
                                x => x.value == parseInt(subcatsArray[i])
                            )[0];
                            if (selectedSubCatItem) {
                                selectedSubCats.push({
                                    value: selectedSubCatItem.value,
                                    label: selectedSubCatItem.displayName
                                });
                            }
                        }

                        for (let i = 0; i < tagArray.length; i++) {
                            var selectedTagsArray = ref.tags.filter(
                                x => x.value == parseInt(tagArray[i])
                            )[0];
                            if (selectedTagsArray) {
                                selectedTags.push({
                                    value: selectedTagsArray.value,
                                    label: selectedTagsArray.label
                                });
                            }
                        }

                        this.setState({
                            isSubCategoryEdit: false,
                            selectedSubCategory: selectedSubCats,
                            selectedTags: selectedTags
                        });
                    }
                    if (this.state.isEminitiesEdit == true && ref.amenities.length > 0) {
                        var selecteAmen = [];
                        var selecteAmenArray = this.props.SavedEventData.amenityId.split(
                            ","
                        );
                        for (let i = 0; i < selecteAmenArray.length; i++) {
                            selecteAmen.push(
                                ref.amenities.filter(
                                    x => x.value == parseInt(selecteAmenArray[i])
                                )[0]
                            );
                        }

                        this.setState({
                            isSubCategoryEdit: false,
                            selectedAmenity: selecteAmen
                        });
                    }
                }
            }
        }
    }



    handleCategoryChange = selectedCategory => {
        this.setState({
            selectedSubCategory: [],
            selectedCategory: selectedCategory
        });
    };

    handleSubCategoryChange = selectedSubCategory => {
        this.setState({ selectedSubCategory: selectedSubCategory });
    };

    handleTagChange = selectedTags => {
        this.setState({ selectedTags: selectedTags });
    };

    @autobind
    public onClickSelectDefaultAmenities() {
        this.setState({ isAddNewAmenity: false });
    }

    @autobind
    public onClickSubmitAmenities() {
        let ai: AmenityCreationViewModel = new AmenityCreationViewModel();
        ai.amenity = this.state.customizedAmenity;
        this.setState({ isLoading: true });
        this.props.saveAmenity(ai);
    }

    handleAmenityChange = selectedAmenity => {

        if (selectedAmenity.length <= 10) {
            if (selectedAmenity.length == 0) {
                this.setState({ selectedAmenity: [], isAddNewAmenity: false });
            } else {
                var isAddNew = false;
                selectedAmenity.map(function (item) {
                    if (item.label == "Add New") {
                        isAddNew = true;
                    }
                });
                if (isAddNew) {
                    this.setState({ isAddNewAmenity: true });
                } else {
                    var selectedAmenity = selectedAmenity.filter(function (item) {
                        if (item.label != "Add New") {
                            return item;
                        }
                    });
                    this.setState({ selectedAmenity, isAddNewAmenity: false });
                }
            }
        }
        else {
            alert("More than 10 Amenities are not allowed");
        }
    };

    handleMetaTagChange = e => {
        this.setState({ metatags: e.target.value });
    };
    handleMetaTitleChange = e => {
        this.setState({ metatitle: e.target.value });
    };
    handleMetaDescriptionChange = e => {
        this.setState({ metadescription: e.target.value });
    };

    //--------------------Image Upload ------------------------
    //Tiles Slider 3 images
    handleTilesSlidersClick = (id, imgPlaceholderId) => {
        //image item click and id of input field

        this.clickedFieldId = id;
        this.clickedPlaceholderId = imgPlaceholderId;
        var element = document.getElementById(
            this.clickedFieldId
        ) as HTMLInputElement;
        element.click();
    };

    handleTilesInputFileChange = event => {
        //hiddent input file change

        const files = Array.from(event.target.files);
        files.forEach((file, i) => {
            this.imagesToAppendInFormData.push({ name: "Tiles", file: file });
        });
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
            reader.readAsDataURL(element.files[0]);

            if (placeholderid.indexOf("1") > 0) {
                this.setState({ showRemoveTileSliderImage1: true });
            } else if (placeholderid.indexOf("2") > 0) {
                this.setState({ showRemoveTileSliderImage2: true });
            } else if (placeholderid.indexOf("3") > 0) {
                this.setState({ showRemoveTileSliderImage3: true });
            }
            this.tilesliderimages.push(event.target.files[0]);
        }
    };

    handleRemove = (id, url) => {
        var imgEle = document.getElementById(id) as HTMLImageElement;
        imgEle.src = url as string;
        if (id.indexOf("1") > 0) {
            this.setState({ showRemoveTileSliderImage1: false });
        } else if (id.indexOf("2") > 0) {
            this.setState({ showRemoveTileSliderImage2: false });
        } else if (id.indexOf("3") > 0) {
            this.setState({ showRemoveTileSliderImage3: false });
        }
    };

    handleRemoveGalleryClick = (id, imgPlaceholderId) => {
        this.clickedGalleryFieldId = id;
        this.clickedGalleryPlaceholderId = imgPlaceholderId;
        var element = document.getElementById(
            this.clickedGalleryFieldId
        ) as HTMLInputElement;
        element.click();
        if (id.indexOf("1") > 0) {
            this.setState({ showRemoveTileSliderImage1: false });
        }
    };
    handleRemoveBannerDesc = (id, url) => {
        var imgEle = document.getElementById(id) as HTMLImageElement;
        imgEle.src = url as string;
        this.setState({ showRemoveDescBanner: false });
    };

    handleRemovesIventoryPageBanner = (id, url) => {
        var imgEle = document.getElementById(id) as HTMLImageElement;
        imgEle.src = url as string;
        this.setState({ showRemoveIventoryPageBanner: false });
    };
    //-End Image Upload Tiles Slider

    // Description Banner image
    handleDescBannerClick = (id, imgPlaceholderId) => {
        this.clickedBanrDescFieldId = id;
        this.clickedBanrDescPlaceholderId = imgPlaceholderId;
        var element = document.getElementById(
            this.clickedBanrDescFieldId
        ) as HTMLInputElement;
        element.click();
    };
    handleDescBannerInputFileChange = e => {
        const files = Array.from(e.target.files);
        files.forEach((file, i) => {
            this.imagesToAppendInFormData.push({ name: "DescBanner", file: file });
        });
        var placeholderid = this.clickedBanrDescPlaceholderId;
        var element = document.getElementById(
            this.clickedBanrDescFieldId
        ) as HTMLInputElement;
        if (element.files && element.files[0]) {
            var reader = new FileReader();
            reader.onload = function () {
                var imgEle = document.getElementById(placeholderid) as HTMLImageElement;
                imgEle.src = reader.result as string;
            };
            this.setState({ showRemoveDescBanner: true });
            reader.readAsDataURL(element.files[0]);
        }
    };
    handleTitleChange = e => {
        this.setState({ title: e.target.value });
    };
    handleMinHourTimeDurationChange = e => {
        this.setState({ minHourTimeDuration: e.target.value });
    };
    handleMinMinuteTimeDurationChange = e => {
        this.setState({ minMinuteTimeDuration: e.target.value });
    };
    handleMaxHourTimeDurationChange = e => {
        this.setState({ maxHourTimeDuration: e.target.value });
    };
    handleMaxMinuteTimeDurationChange = e => {
        this.setState({ maxMinuteTimeDuration: e.target.value });
    };
    //--End Description Banner image

    //Inventory Page Banner
    handleInventoryBannerClick = (id, imgPlaceholderId) => {
        this.clickedInventoryDescFieldId = id;
        this.clickedInventoryDescPlaceholderId = imgPlaceholderId;
        var element = document.getElementById(
            this.clickedInventoryDescFieldId
        ) as HTMLInputElement;
        element.click();
        this.setState({ showRemoveIventoryPageBanner: true });
    };
    handleInventoryBannerInputFileChange = e => {
        const files = Array.from(e.target.files);
        files.forEach((file, i) => {
            this.imagesToAppendInFormData.push({
                name: "InventoryBanner",
                file: file
            });
        });
        var placeholderid = this.clickedInventoryDescPlaceholderId;
        var element = document.getElementById(
            this.clickedInventoryDescFieldId
        ) as HTMLInputElement;
        if (element.files && element.files[0]) {
            var reader = new FileReader();
            reader.onload = function () {
                var imgEle = document.getElementById(placeholderid) as HTMLImageElement;
                imgEle.src = reader.result as string;
            };
            reader.readAsDataURL(element.files[0]);
        }
    };

    ctr = 0;
    //Gallery Images
    handleGalleryClick = (id, imgPlaceholderId, image) => {
        this.clickedGalleryFieldId = id;
        this.clickedGalleryPlaceholderId = imgPlaceholderId;

        if (image !== "") {
            var placeholderid = this.clickedGalleryPlaceholderId;
            if (placeholderid.indexOf("_") >= 0) {
                var element = document.getElementById(
                    this.clickedGalleryFieldId
                ) as HTMLInputElement;
                var ctr = this.ctr;
                var context = this;
                var imgEle;
                var ele = document.createElement("img");
                ele.setAttribute("width", "120");
                ele.setAttribute("class", "rounded mr-1");
                ele.src = image;
                imgEle = document.getElementById(
                    placeholderid.replace("_", "")
                ) as HTMLImageElement;
                imgEle.parentNode.append(ele);
            }
        } else {
            var element = document.getElementById(
                this.clickedGalleryFieldId
            ) as HTMLInputElement;
            this.setState({ showRemoveGalleryImages: true });
            element.click();
            if (imgPlaceholderId.indexOf("_") >= 0) this.ctr++;
        }
    };
    handleGalleryInputFileChange = e => {
        const files = Array.from(e.target.files);
        files.forEach((file, i) => {
            this.imagesToAppendInFormData.push({ name: e.target.id, file: file });
        });
        var placeholderid = this.clickedGalleryPlaceholderId;
        if (placeholderid.indexOf("_") >= 0) {
            var element = document.getElementById(
                this.clickedGalleryFieldId
            ) as HTMLInputElement;
            var ctr = this.ctr;
            var context = this;
            var imgEle;
            if (element.files && element.files[0]) {
                var reader = new FileReader();
                reader.onload = function () {
                    var ele = document.createElement("img");
                    ele.setAttribute("id", placeholderid + ctr);
                    ele.setAttribute("width", "120");
                    ele.setAttribute("class", "rounded mr-1");
                    ele.src = reader.result as string;
                    imgEle = document.getElementById(
                        placeholderid.replace("_", "")
                    ) as HTMLImageElement;
                    imgEle.parentNode.append(ele);
                    document.getElementById(placeholderid + ctr).innerHTML = "aaa";
                };
                reader.readAsDataURL(element.files[0]);
            }
        } else {
            var element = document.getElementById(
                this.clickedGalleryFieldId
            ) as HTMLInputElement;
            if (element.files && element.files[0]) {
                var reader = new FileReader();
                var imgEle;
                var context = this;
                reader.onload = function () {
                    imgEle = document.getElementById(placeholderid) as HTMLImageElement;
                    imgEle.src = reader.result as string;
                };
                reader.readAsDataURL(element.files[0]);
            }
        }
    };
    handleEditButtonPressed(editedComponent) {
        if (editedComponent === "Detail") {
            this.setState({
                detailAccordianEditComponent: (
                    <span className=" pull-right">
                        <i
                            className="fa fa-check"
                            onClick={() => this.handleEditSaveButtonPressed("Detail")}
                            style={{ color: "green", marginRight: 8 }}
                        />
                        <i
                            className="fa fa-times "
                            onClick={() => this.handleEditRevertButtonPressed("Detail")}
                            style={{ color: "red" }}
                            data-icon="&#x25a8;"
                        />
                    </span>
                ),
                isDetailEdit: false
            });
        }
        if (editedComponent === "Image") {
            this.setState({
                ImageAccordianEditComponent: (
                    <span className=" pull-right">
                        <i
                            className="fa fa-check"
                            onClick={() => this.handleEditSaveButtonPressed("Image")}
                            style={{ color: "green", marginRight: 8 }}
                        />
                        <i
                            className="fa fa-times "
                            onClick={() => this.handleEditRevertButtonPressed("Image")}
                            style={{ color: "red" }}
                            data-icon="&#x25a8;"
                        />
                    </span>
                ),
                isImagesEdit: false
            });
        }
        if (editedComponent === "SEO") {
            this.setState({
                SEOAccordianEditComponent: (
                    <span className=" pull-right">
                        <i
                            className="fa fa-check"
                            onClick={() => this.handleEditSaveButtonPressed("SEO")}
                            style={{ color: "green", marginRight: 8 }}
                        />
                        <i
                            className="fa fa-times "
                            onClick={() => this.handleEditRevertButtonPressed("Image")}
                            style={{ color: "red" }}
                            data-icon="&#x25a8;"
                        />
                    </span>
                ),
                isSEOEdit: false
            });
        }
        if (editedComponent === "Description") {
            this.setState({
                DescriptionAccordianEditComponent: (
                    <span className=" pull-right">
                        <i
                            className="fa fa-check"
                            onClick={() => this.handleEditSaveButtonPressed("Description")}
                            style={{ color: "green", marginRight: 8 }}
                        />
                        <i
                            className="fa fa-times "
                            onClick={() => this.handleEditRevertButtonPressed("Description")}
                            style={{ color: "red" }}
                            data-icon="&#x25a8;"
                        />
                    </span>
                ),
                isDescriptionEdit: false
            });
            this.enableCKEditor();
        }
    }

    componentWillReceiveProps(newProps) {
        if (newProps.trySubmit) newProps.submitForm();
    }
    handleEditSaveButtonPressed(editedComponent) {
        if (editedComponent === "Detail") {
            if (this.state.selectedTags != null && this.state.selectedTags.length == 0) {
                alert("Oops, looks like you missed something! Please go back and select feel tags.");
            } else if (this.state.selectedTags == null) {
                alert("Oops, looks like you missed something! Please go back and select feel tags.");
            } else if (this.state.zip == '' || this.state.zip == undefined) {
                alert("Oops, looks like you missed something! Please go back and enter the zip code.");
            } else if (this.state.minHourTimeDuration == '' || this.state.minMinuteTimeDuration == '' || this.state.maxHourTimeDuration == '' || this.state.maxHourTimeDuration == '') {
                alert("Oops, looks like you missed something! Please go back and enter the minimum and maximum amount of time this Feel will approximately take.");
            } else if (this.state.addressLine1 == "") {
                alert("Oops, looks like you missed something! Please go back and enter the address line 1");
            } else if (this.state.city == "") {
                alert("Oops, looks like you missed something! Please go back and enter the city.");
            } else if (this.state.state == "") {
                alert("Oops, looks like you missed something! Please go back and enter the state.");
            } else if (this.state.country == "") {
                alert("Oops, looks like you missed something! Please go back and enter the country.");
            } else {
                document.getElementById("saveBtn").click();
                this.setState({
                    detailAccordianEditComponent: (
                        <i
                            className="fa fa-edit pull-right"
                            onClick={() => this.handleEditButtonPressed("Detail")}
                            aria-hidden="true"
                        />
                    ),
                    isDetailEdit: true
                });
            }
        }

        if (editedComponent === "Image") {
            document.getElementById("saveBtn").click();
            this.setState({
                ImageAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("Image")}
                        aria-hidden="true"
                    />
                ),
                isImagesEdit: true
            });
        }
        if (editedComponent === "SEO") {
            document.getElementById("saveBtn").click();
            this.setState({
                SEOAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("SEO")}
                        aria-hidden="true"
                    />
                ),
                isSEOEdit: true
            });
        }
        if (editedComponent === "Description") {
            document.getElementById("saveBtn").click();
            this.setState({
                DescriptionAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("Description")}
                        aria-hidden="true"
                    />
                ),
                isSEOEdit: true
            });
            this.disableCKEditor();
        }
    }
    handleEditRevertButtonPressed(editedComponent) {
        if (editedComponent === "Detail") {
            this.setState({
                detailAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("Detail")}
                        aria-hidden="true"
                    />
                ),
                isDetailEdit: true
            });
        }
        if (editedComponent === "Image") {
            this.setState({
                ImageAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("Image")}
                        aria-hidden="true"
                    />
                ),
                isImagesEdit: true
            });
        }
        if (editedComponent === "SEO") {
            this.setState({
                SEOAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("SEO")}
                        aria-hidden="true"
                    />
                ),
                isDetailEdit: true
            });
        }
        if (editedComponent === "Description") {
            this.setState({
                DescriptionAccordianEditComponent: (
                    <i
                        className="fa fa-edit pull-right"
                        onClick={() => this.handleEditButtonPressed("Description")}
                        aria-hidden="true"
                    />
                ),
                isDescriptionEdit: true
            });
            this.disableCKEditor();
        }
    }

    handleUploadFile(callback) {
        var formData = new FormData();
        this.imagesToAppendInFormData.forEach((element, i) => {
            formData.append("files[]", element.file, element.name);
        });
        this.setState({ isLoading: true });
        axios
            .post("api/event/uploadimage", formData, {
                headers: {
                    "Content-Type": "multipart/form-data"
                }
            })
            .then(response => {
                if (response.status == 200) {
                    callback(response.data);
                }
            });
    }

    public handleDescChange = val => {
        this.setState({ description: val.editor.getData() });
    };

    public handleHistoryChange = val => {
        this.setState({ history: val.editor.getData() });
    };
    public handleHighlightsChange = val => {
        this.setState({ highlights: val.editor.getData() });
    };
    public handleImmersiveExperienceChange = val => {
        this.setState({ immersiveexperience: val.editor.getData() });
    };
    public handleArchDetailChange = val => {
        this.setState({ archdetail: val.editor.getData() });
    };

    @autobind
    public onChangeCity(e) {
        this.setState({ city: e.target.value })
    }
    @autobind
    public onChangeAddressLine1(e) {
        this.setState({ addressLine1: e.target.value })
    }
    @autobind
    public onChangeAddressLine2(e) {
        this.setState({ addressLine2: e.target.value })
    }
    @autobind
    public onChangeAmenity(e) {
        this.setState({ customizedAmenity: e.target.value })
    }

    @autobind
    public onChangeState(e) {
        this.setState({ state: e.target.value })
    }

    @autobind
    public onChangeCountry(e) {
        this.setState({ country: e.target.value })
    }

    @autobind
    public onChangeZip(e) {
        this.setState({ zip: e.target.value });
    }

    @autobind
    public autoCompleteAddress(address, city, state, country, zipcode, lat, long, street, route) {
        this.setState({
            street: street, city: city,
            address: address,
            state: state, country: country,
            addressLine1: (street == "" ? route : street + ", " + route),
            zip: zipcode,
            lat: lat,
            addressLine2: "",
            long: long,
            route: route
        });
    }

    @autobind
    public onCustomFieldChange(zipcode) {
        this.setState({
            zip: zipcode
        });
    }

    @autobind
    setCategoryAndSubCategory() {
        var category = this.categories.filter((item) => {
            return item.value == 33
        });
        var selectedSubCategory = [];
        this.props.eventCat.map(function (item) {
            let data = {
                value: item.value,
                label: item.displayName
            };
            if (item.value == 52) {
                selectedSubCategory.push(data);
            }
        });
        this.setState({ selectedCategory: category[0], selectedSubCategory: selectedSubCategory, isFeelGuide: false })
    }

    public render() {
        //use fetched information
        this.setFetchedInformationForEdit();
        //to be removed
        var index = 0;
        var cat, type;
        var that = this;
        const schema = this.getSchema();
        if (this.state) {
            this.initialValues = {
                categoryid: this.state.categoryid,
                subcategoryid: this.state.subcategoryid,
                title: this.state.title,
                location: this.state.location,
                placename: this.state.placename,
                address1: this.state.address1,
                address2: this.state.address2,
                country: this.state.country,
                city: this.state.city,
                state: this.state.state,
                zip: this.state.zip,
                amenityId: this.state.amenityId,
                description: this.state.description,
                history: this.state.history,
                highlights: this.state.highlights,
                impressiveexperience: this.state.immersiveexperience,
                archdetail: this.state.archdetail,
                tilesSliderImages: this.state.tilesSliderImages,
                descpagebannerImage: this.state.descpagebannerImage,
                inventorypagebannerImage: this.state.inventorypagebannerImage,
                galleryImages: this.state.galleryImages,
                placemapImages: this.state.placemapImages,
                timelineImages: this.state.timelineImages,
                immersiveexpImages: this.state.immersiveexpImages,
                archdetailImages: this.state.archdetailImages,
                metatags: this.state.metatags,
                metatitle: this.state.metatitle,
                metadescription: this.state.metadescription,
                timeDuration: this.state.timeDuration,
                lat: this.state.lat,
                long: this.state.long
            };
        }
        var categoryOptions;
        if (this.categories) {
            categoryOptions = this.categories.map(function (data) {
                return <option value={data.value}>{data.label}</option>;
            });
        }
        this.allcategories = this.props.eventCat;
        var ref = this;
        ref.categories = [];
        ref.subcategories = [];
        if (this.props.eventCat) {
            this.props.eventCat.map(function (item) {
                if (item.displayName != "All") {
                    let data = {
                        value: item.value,
                        label: item.displayName
                    };
                    if (item.categoryId == 0) {
                        ref.categories.push(data);
                    } else {
                        if (ref.state.selectedCategory) {
                            if (item.categoryId == ref.state.selectedCategory.value)
                                ref.subcategories.push(data);
                        }
                    }
                }
            });

            if (ref.state.selectedCategory === null)
                this.setState({ selectedCategory: ref.categories[0] })
        }
        if (ref.categories.length > 0 && this.state.isFeelGuide) {
            this.setCategoryAndSubCategory();
        }
        ref.tags = [];
        if (this.props.allStoreState.tags != undefined) {
            this.props.allStoreState.tags.tags.map(function (item) {
                let data = {
                    value: item.id,
                    label: item.name
                };
                ref.tags.push(data);
            })
        }

        if (this.props.amenities) {
            var ref = this;
            ref.amenities = [];
            if (index == 0) {
                var data = {
                    value: index,
                    label: "Add New"
                };
                ref.amenities.push(data);
                index = index + 1;
            }
            if (this.props.amenities.amenities.length > 0) {
                for (var i = 0; i < this.props.amenities.amenities.length; i++) {
                    let data = {
                        value: this.props.amenities.amenities[i].id,
                        label: this.props.amenities.amenities[i].amenity
                    };
                    ref.amenities.push(data);
                }
            }
        }

        if (this.props.addedAmenityId !== "") {
            let data = {
                value: this.props.addedAmenityId,
                label: this.state.customizedAmenity
            };
            if (ref.amenities.filter(m => m.value === this.props.addedAmenityId).length === 0)
                ref.amenities.push(data);
        }

        var hourArray = [];
        var minuteArray = [];
        for (i = 1; i <= 24; i++) {
            hourArray.push(i);
        }
        for (i = 0; i <= 60; i++) {
            minuteArray.push(i);
        }
        var hours = hourArray.map(function (item) {
            return <option>{item}</option>
        })
        var minutes = minuteArray.map(function (item) {
            return <option>{item}</option>
        });
        const MY_API = 'AIzaSyCc3zoz5TZaG3w2oF7IeR - fhxNXi8uywNk';
        var _url;
        if (this.state.address != "" && this.state.country != "" && ref.tags.length > 0) {
            _url = `https://www.google.com/maps/embed/v1/place?key=${MY_API}&q=${this.state.address},${this.state.city},${this.state.country}`;
        } else {
            var addressData = "shivaji nagar, pune";
            var cityName = 'Pune';
            var countryName = 'India'
            _url = `https://www.google.com/maps/embed/v1/place?key=${MY_API}&q=${addressData},${cityName},${countryName}`;
        }

        return (
            <div>
                {(!this.props.allStoreState.isSaveEventRequest) && <div>
                    {(this.state.isShow == true) && < ToastContainer
                        position="top-center"
                        autoClose={6000}
                        hideProgressBar={false}
                        newestOnTop={false}
                        closeOnClick
                        rtl={false}
                        pauseOnFocusLoss
                        draggable
                        pauseOnHover
                    />}
                    <Formik
                        initialValues={this.initialValues || {}}
                        onSubmit={(values: EventCreationViewModel) => {
                            event.preventDefault();
                            if (this.state.minHourTimeDuration == '' || this.state.minMinuteTimeDuration == '' || this.state.maxHourTimeDuration == '' || this.state.maxHourTimeDuration == '') {
                                alert("Oops, looks like you missed something! Please go back and enter the minimum and maximum amount of time this Feel will approximately take.");
                            } else if (this.state.selectedTags == null || this.state.selectedTags.length == 0) {
                                alert("Oops, looks like you missed something! Please go back and select feel tags.");
                            }
                            else {
                                setTimeout(() => {
                                    let selectedamenities = "";
                                    if (this.state.selectedAmenity != "") {
                                        selectedamenities = this.state.selectedAmenity.map(function (
                                            item
                                        ) {
                                            if (item != undefined) {
                                                return item.value;
                                            }
                                        });
                                    }
                                    let selectedsubcategories = this.state.selectedSubCategory.map(
                                        function (item) {
                                            return item.value;
                                        }
                                    );

                                    let selectedTags = this.state.selectedTags.map(
                                        function (item) {
                                            return item.value;
                                        }
                                    );

                                    let selectedCategories = this.state.selectedCategory.value;
                                    let ui: EventCreationViewModel = new EventCreationViewModel();
                                    ui.id = this.state.eventId;
                                    if (this.editIdArray === 5) ui.altId = this.state.altId;
                                    ui.subcategoryid = selectedsubcategories.toString();
                                    ui.categoryid = selectedCategories;
                                    ui.tagIds = selectedTags.toString();
                                    ui.title = this.state.title;
                                    ui.location = this.state.location;
                                    ui.placename = this.state.title;
                                    ui.address1 = this.state.addressLine1;
                                    ui.address2 = this.state.addressLine2;
                                    ui.city = this.state.city;
                                    ui.state = this.state.state;
                                    ui.zip = this.state.zip;
                                    ui.country = this.state.country;
                                    ui.street = this.state.street;
                                    ui.amenityId = selectedamenities.toString();
                                    ui.description = this.state.description;
                                    ui.history = this.state.history;
                                    ui.highlights = this.state.highlights;

                                    ui.immersiveExperience = this.state.immersiveexperience;
                                    ui.archdetail = this.state.archdetail;
                                    ui.archdetailId = this.state.archdetailId;
                                    ui.historyId = this.state.historyId;
                                    ui.immersiveExperienceId = this.state.immersiveExperienceId;
                                    ui.highlightsId = this.state.highlightsId;
                                    ui.tilesSliderImages = "";
                                    ui.descpagebannerImage = "";
                                    ui.inventorypagebannerImage = "";
                                    ui.galleryImages = "";
                                    ui.placemapImages = "";
                                    ui.timelineImages = "";
                                    ui.immersiveexpImages = "";
                                    ui.archdetailImages = "";
                                    ui.metatags = this.state.metatags;
                                    ui.metatitle = this.state.metatitle;
                                    ui.metadescription = this.state.metadescription;
                                    ui.hourTimeDuration = this.state.minHourTimeDuration + ":" + this.state.minMinuteTimeDuration + "-" + this.state.maxHourTimeDuration + ":" + this.state.maxMinuteTimeDuration;
                                    ui.minuteTimeDuration = this.state.minuteTimeDuration;
                                    ui.lat = typeof this.state.lat === "number" ? this.state.lat.toString() : "";
                                    ui.long = typeof this.state.long === "number" ? this.state.long.toString() : "";

                                    this.handleUploadFile(data => {
                                        if (data != null && data.length > 0 && ui.altId == undefined) {
                                            ui.altId = data[0].altId;
                                        }
                                        data.forEach(element => {
                                            if (element.name.toString().includes("ht-c")) {
                                                ui.tilesSliderImages += element.name + ",";
                                            }
                                            else if (element.name.toString().includes("about")) {
                                                ui.descpagebannerImage += element.name + ",";
                                            }
                                            else if (element.name.toString().includes("glr")) {
                                                ui.galleryImages += element.name + ",";
                                            }
                                            else if (element.name.toString().includes("place-plan")) {
                                                ui.placemapImages += element.name + ",";
                                            }
                                            else if (element.name.toString().includes("timeline")) {
                                                ui.timelineImages += element.name + ",";
                                            }
                                            else if (element.name.toString().includes("immersive-experience")) {
                                                ui.immersiveexpImages += element.name + ",";
                                            }
                                            else if (element.name.toString().includes("architectural-detail")) {
                                                ui.archdetailImages += element.name + ",";
                                            }
                                            else {
                                                ui.inventorypagebannerImage += element.name + ",";
                                            }
                                        });
                                        this.setState({ isLoading: true });
                                        this.props.createPlace(ui);
                                    });
                                }, 400);
                            }
                        }}
                        render={({
                            errors,
                            touched,
                            isSubmitting,
                            setFieldValue,
                            values
                        }) => (
                                <Form
                                    className="d-block w-100 pb-3 mb-3"
                                >
                                    <div className="col-sm-12">
                                        <a
                                            className="place-listing active"
                                            data-toggle="collapse"
                                            href="#PlaceDetail"
                                            role="button"
                                            aria-expanded="true"
                                            aria-controls="PlaceDetail"
                                        >
                                            <span className="rounded-circle border border-primary">
                                                1
                                            </span>
                                            Details
                                        </a>
                                        {this.state.detailAccordianEditComponent}
                                        <div
                                            className="collapse multi-collapse show pt-3"
                                            id="PlaceDetail"
                                        >
                                            <div className="form-group">
                                                <label>
                                                    Title <span className="text-danger">*</span>{" "}
                                                </label>
                                                <Field
                                                    required
                                                    className="form-control"
                                                    disabled={this.state.isDetailEdit}
                                                    value={this.state.title}
                                                    onChange={this.handleTitleChange}
                                                    placeholder="Give it a short distinct name"
                                                    type="text"
                                                    name="title"
                                                />
                                            </div>
                                            <div className="form-group">
                                                <label>
                                                    Select your feel category{" "}
                                                    <span className="text-danger">*</span>
                                                </label>
                                                <Select
                                                    name="categoryid"
                                                    options={this.categories}
                                                    placeholder="Select your feel category"
                                                    onChange={this.handleCategoryChange}
                                                    value={this.state.selectedCategory}
                                                    required
                                                    inputProps={{ readOnly: true }}
                                                    disabled={this.state.isDetailEdit}
                                                />
                                            </div>
                                            <div className="form-group">
                                                <label>
                                                    Select your feel sub category{" "}
                                                    <span className="text-danger">*</span>{" "}
                                                </label>
                                                <Select
                                                    name="subcategoryid"
                                                    required
                                                    isMulti
                                                    options={this.subcategories}
                                                    placeholder="Select your feel sub category"
                                                    onChange={this.handleSubCategoryChange}
                                                    value={this.state.selectedSubCategory}
                                                    disabled={this.state.isDetailEdit}
                                                />
                                            </div>
                                            <div className="form-group">
                                                <label>
                                                    Select your feel tags{" "}
                                                    <span className="text-danger">*</span>
                                                </label>
                                                <Select
                                                    name="tags"
                                                    options={this.tags}
                                                    placeholder="Select your feel tags"
                                                    onChange={this.handleTagChange}
                                                    value={this.state.selectedTags}
                                                    isMulti
                                                    required
                                                    inputProps={{ readOnly: true }}
                                                    disabled={this.state.isDetailEdit}
                                                />
                                            </div>
                                            <div className="row">
                                                <div className="form-group col-sm-6">
                                                    <label>
                                                        Fast-paced duration <span className="text-danger">*</span>
                                                    </label>
                                                    <div className="row">

                                                        <div className="form-group col-sm-6">
                                                            <Field
                                                                className="form-control"
                                                                component="select"
                                                                disabled={this.state.isDetailEdit}
                                                                onChange={this.handleMinHourTimeDurationChange}
                                                                name="hourTimeDuration"
                                                                required
                                                            >
                                                                {(this.state.minHourTimeDuration == "") && <option value='' selected disabled>Select Hours</option>}
                                                                {(this.state.minHourTimeDuration != "") && <option>{this.state.minHourTimeDuration}</option>}
                                                                {hours}
                                                            </Field>
                                                        </div>
                                                        <div className="form-group col-sm-6">
                                                            <Field
                                                                className="form-control"
                                                                component="select"
                                                                disabled={this.state.isDetailEdit}
                                                                onChange={this.handleMinMinuteTimeDurationChange}
                                                                name="minuteTimeDuration"
                                                                required
                                                            >
                                                                {(this.state.minMinuteTimeDuration == "") && <option value='' selected disabled>Select Minutes</option>}
                                                                {(this.state.minMinuteTimeDuration != "") && <option>{this.state.minMinuteTimeDuration}</option>}
                                                                {minutes}
                                                            </Field>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div className="form-group col-sm-6">
                                                    <label>
                                                        Leisurely duration <span className="text-danger">*</span>
                                                    </label>
                                                    <div className="row">
                                                        <div className="form-group col-sm-6">
                                                            <Field
                                                                className="form-control"
                                                                component="select"
                                                                disabled={this.state.isDetailEdit}
                                                                onChange={this.handleMaxHourTimeDurationChange}
                                                                name="hourTimeDuration1"
                                                                required
                                                            >
                                                                {(this.state.maxHourTimeDuration == "") && <option value='' selected disabled>Select Hours</option>}
                                                                {(this.state.maxHourTimeDuration != "") && <option>{this.state.maxHourTimeDuration}</option>}
                                                                {hours}
                                                            </Field>
                                                        </div>
                                                        <div className="form-group col-sm-6">
                                                            <Field
                                                                className="form-control"
                                                                component="select"
                                                                disabled={this.state.isDetailEdit}
                                                                onChange={this.handleMaxMinuteTimeDurationChange}
                                                                name="minuteTimeDuration1"
                                                                required
                                                            >
                                                                {(this.state.maxMinuteTimeDuration == "") && <option value='' selected disabled>Select Minutes</option>}
                                                                {(this.state.maxMinuteTimeDuration != "") && <option>{this.state.maxMinuteTimeDuration}</option>}
                                                                {minutes}
                                                            </Field>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>



                                            <div className="form-group">
                                                <div className="row">
                                                    <div className="col-sm-6 col-12">
                                                        <label>
                                                            Address/ Location <span className="text-danger">*</span>{" "}
                                                        </label>
                                                        <PlaceAutocomplete state={this.state} autoCompletePlace={this.autoCompleteAddress} onCustomFieldChange={this.onCustomFieldChange} value={this.state.address} disabled={this.state.isDetailEdit} isEditMode={this.state.isEditMode} />

                                                        <div className="row mb-0">
                                                            <div className="col-sm-6 col-12">
                                                                <input
                                                                    disabled={this.state.isDetailEdit}
                                                                    name="street"
                                                                    type="text"
                                                                    className="form-control mb-2"
                                                                    placeholder="Address Line 1"
                                                                    value={this.state.addressLine1}
                                                                    onChange={this.onChangeAddressLine1}
                                                                    required
                                                                />
                                                            </div>
                                                            <div className="col-sm-6 col-12">
                                                                <input
                                                                    disabled={this.state.isDetailEdit}
                                                                    name="state"
                                                                    type="text"
                                                                    className="form-control mb-2"
                                                                    placeholder="Address Line 2"
                                                                    value={this.state.addressLine2}
                                                                    onChange={this.onChangeAddressLine2}
                                                                />
                                                            </div>
                                                        </div>
                                                        <div className="row mb-0">
                                                            <div className="col-sm-6 col-12">
                                                                <input
                                                                    disabled={this.state.isDetailEdit}
                                                                    required
                                                                    name="city"
                                                                    type="text"
                                                                    className="form-control mb-2"
                                                                    placeholder="City"
                                                                    value={this.state.city}
                                                                    onChange={this.onChangeCity}
                                                                />
                                                            </div>
                                                            <div className="col-sm-6 col-12">
                                                                <input
                                                                    disabled={this.state.isDetailEdit}
                                                                    name="state"
                                                                    type="text"
                                                                    className="form-control mb-2"
                                                                    placeholder="State"
                                                                    value={this.state.state}
                                                                    onChange={this.onChangeState}
                                                                />
                                                            </div>
                                                        </div>

                                                        <div className="row">
                                                            <div className="col-sm-6 col-12">
                                                                <input
                                                                    disabled={this.state.isDetailEdit}
                                                                    name="country"
                                                                    type="text"
                                                                    className="form-control mb-2"
                                                                    placeholder="Country"
                                                                    value={this.state.country}
                                                                    onChange={this.onChangeCountry}
                                                                />
                                                            </div>
                                                            <div className="col-sm-6 col-12">
                                                                <input
                                                                    required
                                                                    disabled={this.state.isDetailEdit}
                                                                    name="zip"
                                                                    type="text"
                                                                    className="form-control mb-2"
                                                                    placeholder="Zip/Postal"
                                                                    value={this.state.zip}
                                                                    onChange={this.onChangeZip}
                                                                />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div className="col-sm-6 col-12 pl-0 text-center">
                                                        <iframe src={_url} width="100%" height="310" frameBorder="0" className="iframes border" allowFullScreen></iframe >
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="form-group">
                                                {(!this.state.isAddNewAmenity) && <label>
                                                    Amenities and Features
                                                </label>}
                                                {(!this.state.isAddNewAmenity) && <Select
                                                    name="amenityId"
                                                    isMulti
                                                    options={this.amenities}
                                                    placeholder="Select Amenities and Features"
                                                    onChange={this.handleAmenityChange}
                                                    value={this.state.selectedAmenity}
                                                    disabled={this.state.isDetailEdit}
                                                />}
                                                {(this.state.isAddNewAmenity) &&
                                                    <div>
                                                        <label>Add new amenity</label>
                                                        <div className="input-group mb-2">
                                                            <input type="text" value={this.state.customizedAmenity}
                                                                onChange={this.onChangeAmenity} className="form-control" placeholder="Add new amenity" aria-label="Search term" aria-describedby="basic-addon" />
                                                            <div className="input-group-append">
                                                                <a onClick={this.onClickSubmitAmenities} className="btn btn-secondary">Save</a>
                                                            </div>
                                                        </div>
                                                        <a href="JavaScript:Void(0)" onClick={this.onClickSelectDefaultAmenities} className="btn btn-sm btn-outline-primary"> <small><i className="fa fa-refresh mr-2" aria-hidden="true"></i>Select default Amenities</small> </a>
                                                    </div>
                                                }
                                            </div>
                                        </div>
                                    </div>
                                    {/* <!-- End Tab Details--> */}

                                    <div className="line" />
                                    <div className="col-sm-12">
                                        <a
                                            className="place-listing"
                                            data-toggle="collapse"
                                            href="#PlaceDescription"
                                            role="button"
                                            aria-expanded="false"
                                            aria-controls="PlaceDescription"
                                        >
                                            <span className="rounded-circle border border-primary place-listing">
                                                2
                    </span>
                                            Description
                  </a>
                                        {this.state.DescriptionAccordianEditComponent}
                                        <div
                                            className="collapse multi-collapse pt-3"
                                            id="PlaceDescription"
                                        >
                                            <div className="form-group">
                                                <label>Description</label>

                                                <CKEditor
                                                    name="description"
                                                    placeholder="Something About the feel..."
                                                    activeclassName="p5"
                                                    disabled={true}
                                                    content={this.state.description}
                                                    contentEditable={false}
                                                    events={{
                                                        change: this.handleDescChange
                                                    }}
                                                />
                                                {/* <Field component="textarea" name="description" className="form-control" rows={3} placeholder="Something About the feel..." /> */}
                                            </div>
                                            <div className="form-group">
                                                <label>History</label>
                                                <CKEditor
                                                    name="history"
                                                    activeclassName="p5"
                                                    content={this.state.history}
                                                    disabled={true}
                                                    events={{
                                                        change: this.handleHistoryChange,
                                                        disabled: true
                                                    }}
                                                />
                                                {/* <Field component="textarea" name="history" className="form-control" rows={3} placeholder="History here..." /> */}
                                            </div>
                                            <div className="form-group">
                                                <label>Highlights/Nuggets</label>
                                                <CKEditor
                                                    name="highlights"
                                                    activeclassName="p5"
                                                    content={this.state.highlights}
                                                    events={{
                                                        change: this.handleHighlightsChange
                                                    }}
                                                />
                                                {/* <Field component="textarea" name="highlights" className="form-control" rows={3} placeholder=" Highlights/Nuggets here..." /> */}
                                            </div>
                                            <div className="form-group">
                                                <label>Immersive experience</label>
                                                <CKEditor
                                                    name="impressiveexperience"
                                                    activeclassName="p5"
                                                    content={this.state.immersiveexperience}
                                                    events={{
                                                        change: this.handleImmersiveExperienceChange
                                                    }}
                                                />
                                                {/* <Field component="textarea" name="impressiveexperience" className="form-control" rows={3} placeholder=" Experience here..." /> */}
                                            </div>
                                            <div className="form-group">
                                                <label>Architectural detail</label>
                                                <CKEditor
                                                    name="archdetail"
                                                    activeclassName="p5"
                                                    content={this.state.archdetail}
                                                    events={{
                                                        change: this.handleArchDetailChange
                                                    }}
                                                />
                                                {/* <Field component="textarea" name="archdetail" className="form-control" rows={3} placeholder="Architectural detail here..." /> */}
                                            </div>
                                        </div>
                                    </div>

                                    <div className="line" />
                                    <div className="col-sm-12">
                                        <a
                                            className="place-listing"
                                            data-toggle="collapse"
                                            href="#PlaceImages"
                                            role="button"
                                            aria-expanded="false"
                                            aria-controls="PlaceImages"
                                        >
                                            <span className="rounded-circle border border-primary place-listing">
                                                3
                    </span>
                                            Images
                  </a>
                                        {this.state.ImageAccordianEditComponent}
                                        <div
                                            className="collapse multi-collapse pt-3"
                                            id="PlaceImages"
                                        >
                                            <div className="form-group">
                                                <label className="d-block">Tiles slider images</label>
                                                <Field
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
                                                        width="80"
                                                        className="rounded mr-1 image thumbnail"
                                                    />
                                                </span>
                                                <span
                                                    onClick={() =>
                                                        this.handleRemove("img2View", this.state.imgTile2)
                                                    }
                                                    className="remove-button text-primary"
                                                >
                                                    <i className="fa fa-remove" aria-hidden="true" />
                                                </span>
                                                <span
                                                    onClick={() =>
                                                        this.handleTilesSlidersClick("img1", "img2View")
                                                    }
                                                    className="hyperref"
                                                >
                                                    <img
                                                        id="img2View"
                                                        src={this.state.imgTile2}
                                                        onError={(e) => {
                                                            e.currentTarget.src = `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`
                                                        }}
                                                        alt="place thumb"
                                                        width="80"
                                                        className="rounded mr-1 image thumbnail"
                                                    />
                                                </span>
                                                <span
                                                    onClick={() =>
                                                        this.handleRemove("img3View", this.state.imgTile3)
                                                    }
                                                    className="remove-button text-primary"
                                                >
                                                    <i className="fa fa-remove" aria-hidden="true" />
                                                </span>
                                                <span
                                                    onClick={() =>
                                                        this.handleTilesSlidersClick("img1", "img3View")
                                                    }
                                                    className="hyperref"
                                                >
                                                    <img
                                                        id="img3View"
                                                        src={this.state.imgTile3}
                                                        onError={(e) => {
                                                            e.currentTarget.src = `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`
                                                        }}
                                                        alt="place thumb"
                                                        width="80"
                                                        className="rounded image thumbnail"
                                                    />
                                                </span>

                                                <small className="form-text text-muted mb-2">
                                                    We recommend using at least a 277px x 175px image that's
                                                    no larger than 500kb.
                      </small>
                                            </div>
                                            <div className="form-group">
                                                <label className="d-block">Description page banner</label>

                                                <input
                                                    disabled={this.state.isImagesEdit}
                                                    onChange={this.handleDescBannerInputFileChange}
                                                    className="hidden"
                                                    accept="image/*"
                                                    type="file"
                                                    id="descbanner"
                                                    name="descpagebannerImage"
                                                />
                                                {this.state.showRemoveDescBanner && (
                                                    <span
                                                        onClick={() =>
                                                            this.handleRemoveBannerDesc(
                                                                "descbannerview",
                                                                `${gets3BaseUrl()}/places/about/learn-more-banner-placeholder.jpg`
                                                            )
                                                        }
                                                        className="remove-button text-primary"
                                                    >
                                                        <i className="fa fa-remove" aria-hidden="true" />
                                                    </span>
                                                )}

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
                                                        onError={(e) => {
                                                            e.currentTarget.src = `${gets3BaseUrl()}/places/about/learn-more-banner-placeholder.jpg`
                                                        }}
                                                        alt="place thumb"
                                                        className="rounded image thumbnail desc-page-bnr"
                                                    />
                                                </span>
                                                <small className="form-text text-muted mb-2">
                                                    We recommend using at least a 1920px x 570px image
                        that's no larger than 1MB.{" "}
                                                </small>
                                            </div>
                                            <div className="form-group">
                                                <label className="d-block">Inventory page banner</label>

                                                <Field
                                                    disabled={this.state.isImagesEdit}
                                                    onChange={this.handleInventoryBannerInputFileChange}
                                                    className="hidden"
                                                    accept="image/*"
                                                    type="file"
                                                    id="inventorybanner"
                                                    name="inventorypagebannerImage"
                                                />
                                                {this.state.showRemoveIventoryPageBanner && (
                                                    <span
                                                        onClick={() =>
                                                            this.handleRemovesIventoryPageBanner(
                                                                "inventorybannerview",
                                                                `${gets3BaseUrl()}/places/about/learn-more-banner-placeholder.jpg`
                                                            )
                                                        }
                                                        className="remove-button text-primary"
                                                    >
                                                        <i className="fa fa-remove" aria-hidden="true" />
                                                    </span>
                                                )}

                                                <span
                                                    onClick={() =>
                                                        this.handleInventoryBannerClick(
                                                            "inventorybanner",
                                                            "inventorybannerview"
                                                        )
                                                    }
                                                    className="hyperref"
                                                >
                                                    <img
                                                        id="inventorybannerview"
                                                        src={this.state.imgInventoryPageBanner}
                                                        onError={(e) => {
                                                            e.currentTarget.src = `${gets3BaseUrl()}/places/about/learn-more-banner-placeholder.jpg`
                                                        }}
                                                        alt="place thumb"
                                                        className="rounded image thumbnail inv-page-bnr"
                                                    />
                                                </span>
                                                <small className="form-text text-muted mb-2">
                                                    We recommend using at least a 1920px x 350px image
                                                    that's no larger than 1MB.
                      </small>
                                            </div>
                                            <div className="form-group">
                                                <label className="d-block">Gallery images</label>
                                                <Field
                                                    disabled={this.state.isImagesEdit}
                                                    type="file"
                                                    onChange={this.handleGalleryInputFileChange}
                                                    className="hidden"
                                                    accept="image/*"
                                                    id="gallery"
                                                    name="galleryImages"
                                                />
                                                <small className="form-text text-muted mb-2">
                                                    We recommend using at least a 2160x1080px image that's
                        no larger than 1MB.{" "}
                                                </small>

                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick("gallery", "galleryview", "")
                                                    }
                                                    id="gallerySpan"
                                                    className="hyperref"
                                                >
                                                    <img
                                                        id="galleryview"
                                                        src={this.state.imgGallery}
                                                        onError={(e) => {
                                                            e.currentTarget.src = `${gets3BaseUrl()}/feelAdmin/placeholder-image.jpg`
                                                        }}
                                                        alt="place thumb"
                                                        width="120"
                                                        className="rounded mr-1 image thumbnail"
                                                    />
                                                </span>

                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick("gallery", "galleryview_", "")
                                                    }
                                                    id="gallerySpan1"
                                                    className="ml-2 text-primary hyperref"
                                                >
                                                    <i
                                                        className="fa fa-plus"
                                                        aria-hidden="true"
                                                        id="gallerySpan2"
                                                    />
                                                </span>
                                            </div>
                                            <div className="form-group">
                                                <label className="d-block">Place map/plan</label>
                                                <Field type="hidden" name="placemapImages" />
                                                <Field
                                                    disabled={this.state.isImagesEdit}
                                                    onChange={this.handleGalleryInputFileChange}
                                                    className="hidden"
                                                    accept="image/*"
                                                    type="file"
                                                    id="placemapImages"
                                                    name="galleryImages"
                                                />

                                                <small className="form-text text-muted mb-2">
                                                    We recommend using at least a 2160x1080px image that's
                        no larger than 1MB.{" "}
                                                </small>
                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick(
                                                            "placemapImages",
                                                            "placemapImagesView",
                                                            ""
                                                        )
                                                    }
                                                    className="hyperref"
                                                >
                                                    <img
                                                        src={`${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`}
                                                        id="placemapImagesView"
                                                        alt="place thumb"
                                                        width="120"
                                                        className="rounded mr-1 image thumbnail"
                                                    />
                                                </span>
                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick(
                                                            "placemapImages",
                                                            "placemapImagesView_",
                                                            ""
                                                        )
                                                    }
                                                    className="ml-2 text-primary hyperref"
                                                >
                                                    <i className="fa fa-plus" aria-hidden="true" />
                                                </span>
                                            </div>
                                            <div className="form-group">
                                                <label className="d-block">Timeline</label>
                                                <Field type="hidden" name="timelineImages" />
                                                <Field
                                                    disabled={this.state.isImagesEdit}
                                                    onChange={this.handleGalleryInputFileChange}
                                                    className="hidden"
                                                    accept="image/*"
                                                    type="file"
                                                    id="timelineImages"
                                                    name="timelineImages"
                                                />

                                                <small className="form-text text-muted mb-2">
                                                    We recommend using at least a 2160x1080px image that's
                        no larger than 1MB.{" "}
                                                </small>
                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick(
                                                            "timelineImages",
                                                            "timeLineImagesImagesView",
                                                            ""
                                                        )
                                                    }
                                                    className="hyperref"
                                                >
                                                    <img
                                                        src={`${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`}
                                                        id="timeLineImagesImagesView"
                                                        alt="place thumb"
                                                        width="120"
                                                        className="rounded mr-1 image thumbnail"
                                                    />
                                                </span>
                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick(
                                                            "timelineImages",
                                                            "timeLineImagesImagesView_",
                                                            ""
                                                        )
                                                    }
                                                    className="ml-2 text-primary hyperref"
                                                >
                                                    <i className="fa fa-plus" aria-hidden="true" />
                                                </span>
                                            </div>
                                            <div className="form-group">
                                                <label className="d-block">Immersive experience</label>
                                                <Field type="hidden" name="immersiveImages" />
                                                <Field
                                                    disabled={this.state.isImagesEdit}
                                                    onChange={this.handleGalleryInputFileChange}
                                                    className="hidden"
                                                    accept="image/*"
                                                    type="file"
                                                    id="immersiveImages"
                                                    name="timelineImages"
                                                />

                                                <small className="form-text text-muted mb-2">
                                                    We recommend using at least a 2160x1080px image that's
                        no larger than 1MB.{" "}
                                                </small>
                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick(
                                                            "immersiveImages",
                                                            "immersiveImagesView",
                                                            ""
                                                        )
                                                    }
                                                    className="hyperref"
                                                >
                                                    <img
                                                        src={`${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`}
                                                        id="immersiveImagesView"
                                                        alt="place thumb"
                                                        width="120"
                                                        className="rounded mr-1 image thumbnail"
                                                    />
                                                </span>
                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick(
                                                            "immersiveImages",
                                                            "immersiveImagesView_",
                                                            ""
                                                        )
                                                    }
                                                    className="ml-2 text-primary hyperref"
                                                >
                                                    <i className="fa fa-plus" aria-hidden="true" />
                                                </span>
                                            </div>
                                            <div className="form-group">
                                                <label className="d-block">Architectural detail</label>
                                                <Field type="hidden" name="archdetailImages" />
                                                <Field
                                                    disabled={this.state.isImagesEdit}
                                                    onChange={this.handleGalleryInputFileChange}
                                                    className="hidden"
                                                    accept="image/*"
                                                    type="file"
                                                    id="archdetailImages"
                                                    name="timelineImages"
                                                />

                                                <small className="form-text text-muted mb-2">
                                                    We recommend using at least a 2160x1080px image that's
                                                    no larger than 1MB.
                      </small>

                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick(
                                                            "archdetailImages",
                                                            "archdetailImagesImagesView",
                                                            ""
                                                        )
                                                    }
                                                    className="hyperref"
                                                >
                                                    <img
                                                        src={`${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`}
                                                        id="archdetailImagesImagesView"
                                                        alt="place thumb"
                                                        width="120"
                                                        className="rounded mr-1 image thumbnail"
                                                    />
                                                </span>
                                                <span
                                                    onClick={() =>
                                                        this.handleGalleryClick(
                                                            "archdetailImages",
                                                            "archdetailImagesImagesView_",
                                                            ""
                                                        )
                                                    }
                                                    className="ml-2 text-primary hyperref"
                                                >
                                                    <i className="fa fa-plus" aria-hidden="true" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="line" />

                                    <div className="col-sm-12">
                                        <a
                                            className="place-listing"
                                            data-toggle="collapse"
                                            href="#PlaceSEO"
                                            role="button"
                                            aria-expanded="false"
                                            aria-controls="PlaceSEO"
                                        >
                                            <span className="rounded-circle border border-primary place-listing">
                                                4
                    </span>{" "}
                                            SEO Tags
                  </a>
                                        {this.state.SEOAccordianEditComponent}
                                        <div className="collapse multi-collapse pt-3" id="PlaceSEO">
                                            <div className="form-group">
                                                <label>Meta Tags</label>
                                                <Field
                                                    disabled={this.state.isSEOEdit}
                                                    name="metatags"
                                                    type="text"
                                                    value={this.state.metatags}
                                                    onChange={this.handleMetaTagChange.bind(this)}
                                                    className="form-control"
                                                    placeholder="Meta Tags here"
                                                />
                                            </div>
                                            <div className="form-group">
                                                <label>Meta Title</label>
                                                <Field
                                                    name="metatitle"
                                                    disabled={this.state.isSEOEdit}
                                                    value={this.state.metatitle}
                                                    type="text"
                                                    className="form-control"
                                                    onChange={this.handleMetaTitleChange.bind(this)}
                                                    placeholder="Meta Title here"
                                                />
                                            </div>
                                            <div className="form-group">
                                                <label>Meta Description</label>

                                                <Field
                                                    component="textarea"
                                                    disabled={this.state.isSEOEdit}
                                                    value={this.state.metadescription}
                                                    name="metadescription"
                                                    onChange={this.handleMetaDescriptionChange.bind(this)}
                                                    className="form-control"
                                                    rows={3}
                                                    placeholder="Meta Description here"
                                                />
                                            </div>
                                        </div>
                                    </div>
                                    <div className="line" />
                                    <div className="text-center pt-4 pb-4">
                                        <a href="#" className="text-decoration-none mr-4">Cancel</a>
                                        <div className="btn-group">
                                            <button type="button" className="btn btn-outline-primary dropdown-toggle" data-toggle="dropdown"
                                                aria-haspopup="true" aria-expanded="false">
                                                Save
          </button>
                                            <div className="dropdown-menu">
                                                <a className="dropdown-item disabled">Save as draft</a>

                                                {!this.showSaveButton &&
                                                    <button
                                                        id="saveBtn"
                                                        name="saveBtn"
                                                        type="submit"
                                                        style={{ display: 'none' }}
                                                        className="dropdown-item"
                                                    >
                                                        Save (continue editing)
                                    </button>}

                                                {this.showSaveButton &&
                                                    <button
                                                        id="saveBtn"
                                                        name="saveBtn"
                                                        type="submit"
                                                        style={{ display: 'block' }}
                                                        className="dropdown-item"
                                                    >
                                                        Save (continue editing)
                                    </button>}
                                                <a className="dropdown-item disabled">Save & submit for approval</a>
                                                <a className="dropdown-item disabled">Save & add another</a>
                                            </div>
                                        </div>
                                    </div>

                                </Form>
                            )}
                    />
                </div>}
                {
                    (this.props.allStoreState.isSaveEventRequest) && <Newloader />
                }
            </div>
        );

    }

    private getSchema() {
        return Yup.object().shape({
            categoryid: Yup.string(),
            subcategoryid: Yup.string(),
            title: Yup.string(),
            location: Yup.string(),
            placename: Yup.string(),
            address1: Yup.string(),
            address2: Yup.string(),
            city: Yup.string(),
            state: Yup.string(),
            zip: Yup.string(),
            amenityId: Yup.string(),
            description: Yup.string(),
            history: Yup.string(),
            highlights: Yup.string(),
            impressiveexperience: Yup.string(),
            archdetail: Yup.string(),
            tilesSliderImages: Yup.mixed(),
            descpagebannerImage: Yup.string(),
            inventorypagebannerImage: Yup.string(),
            galleryImages: Yup.string(),
            placemapImages: Yup.string(),
            timelineImages: Yup.string(),
            immersiveexpImages: Yup.string(),
            archdetailImages: Yup.string(),
            metatags: Yup.string(),
            metatitle: Yup.string(),
            metadescription: Yup.string()
        });
    }
}
