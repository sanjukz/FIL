import * as React from 'react';
import * as S3FileUpload from 'react-s3';
import { message } from 'antd';

class UploadProfile extends React.PureComponent<any, any>{
    state = {
        baseImgixUrl: 'https://feelitlive.imgix.net/images'
    };
    public clickedBanrDescFieldId: any; clickedBanrDescPlaceholderId: any;
    handleDescBannerClick = (id, imgPlaceholderId) => {
        this.clickedBanrDescFieldId = id;
        this.clickedBanrDescPlaceholderId = imgPlaceholderId;
        var element = document.getElementById(
            this.clickedBanrDescFieldId
        ) as HTMLInputElement;
        element.click();
    };
    public handleselectedFile(e) {
        var placeholderid = this.clickedBanrDescPlaceholderId;
        var element = document.getElementById(
            this.clickedBanrDescFieldId
        ) as HTMLInputElement;
        var blob = e.target.files[0].slice(0, e.target.files[0].size, 'image/png');
        var newFile = new File([blob], `${this.props.session.user.altId.toUpperCase()}.png`, { type: 'image/png' });
        if (element.files && element.files[0]) {
            var reader = new FileReader();
            reader.onload = function () {
                var imgEle = document.getElementById(placeholderid) as HTMLImageElement;
                imgEle.src = reader.result as string;
            };
            reader.readAsDataURL(element.files[0]);
        }
        const config = {
            bucketName: 'feelaplace-cdn',
            dirName: 'images/feel-users-profilePic',
            region: 'us-west-2',
            accessKeyId: 'AKIAYD5645ZATMXE5H2H',
            secretAccessKey: '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',
        }
        S3FileUpload
            .uploadFile(newFile, config)
            .then(data => {
                this.showSuccessAlert();
            })
            .catch(err => {
                throw err;
            });
    }
    showSuccessAlert = () => {
        message.success({
            content: 'Profile picture updated successfully',
            duration: 5,
            className: 'mt-5'
        })
    }
    render() {
        return <>
            <span
                onClick={() =>
                    this.handleDescBannerClick(
                        "profilePic",
                        "profilePicView"
                    )
                }
                className="hyperref"
            >
                <img
                    id="profilePicView"
                    src={`${this.props.gets3BaseUrl}/feel-users-profilePic/${this.props.session.user && this.props.session.user.altId.toUpperCase()}.png`}
                    onError={(e) => {
                        var userPic = this.props.session.user.altId ? `${this.props.gets3BaseUrl}/my-account/icons/fil-user.svg` : `${this.props.gets3BaseUrl}/my-account/icons/fil-user.svg`
                        e.currentTarget.src = userPic
                    }}
                    alt="FIL User Avatar"
                    width="100"
                    height="100"
                    className="rounded-circle mb-2"
                />
            </span>
            <span onClick={() =>
                this.handleDescBannerClick(
                    "profilePic",
                    "profilePicView"
                )
            }
                className="d-block hyperref text-purple font-weight-bold"><u>Update photo</u></span>

            <input type="file" name="" accept="image/*" id="profilePic" className="d-none" onChange={this.handleselectedFile.bind(this)} />
        </>
    }
}
export default UploadProfile;