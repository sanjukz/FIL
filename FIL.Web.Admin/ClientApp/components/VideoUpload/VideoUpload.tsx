import * as React from "react";
import { Progress } from 'antd';

class VideoUpload extends React.Component<any, any> {
    videoInputField: React.RefObject<HTMLInputElement>;
    constructor(props) {
        super(props);
        this.state = {
            readerSource: "",
            uploadPercent: 0
        };

        this.videoInputField = React.createRef();
    }

    onChangeVideoInput = () => {
        let imageInputNode = this.videoInputField.current;
        if (imageInputNode.files && imageInputNode.files[0]) {
            let reader = new FileReader();

            reader.onload = (e) => {
                this.setState({
                    readerSource: reader.result,
                });
            };

            reader.onprogress = (event) => {
                var percent_complete = Math.ceil((event.loaded / event.total) * 100);
                this.setState({
                    uploadPercent: percent_complete
                });
            }

            reader.onloadstart = (event) => {
                this.setState({
                    uploadPercent: 1
                })
            }

            reader.readAsDataURL(imageInputNode.files[0]);
        }
    };

    onTileClick = () => {
        this.videoInputField.current.click();
    };

    render() {
        return (
            <div className="row">
                <div className="col-6">
                    <input
                        ref={this.videoInputField}
                        onChange={this.onChangeVideoInput}
                        onClick={(e) => this.videoInputField.current.value = null}
                        className="hidden"
                        accept="video/mp4,video/x-m4v,video/*"
                        type="file"
                    />
                    <span
                        className="hyperref"
                        onClick={this.onTileClick}
                    >
                        <img
                            src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/places/tiles/upload-file.jpg`}
                            alt="video thumbnail"
                            width={120}
                            className="rounded mr-1 img-fluid img-thumbnail"
                        />
                    </span>
                </div>
                <div className="col-6">
                    {this.state.uploadPercent !== 0 ?
                        <Progress type="circle" percent={this.state.uploadPercent} width={80} /> :
                        null
                    }
                </div>
            </div>
        )
    }
}

export default VideoUpload;