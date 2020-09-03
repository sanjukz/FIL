import * as React from "react";
import { Modal } from "react-bootstrap";

export default class FilAlert extends React.Component<any, any> {
    //public componentWillMount() {
    //    this.setState({ visible: true });        
    //}
    
    public componentDidMount() {
        var that = this;
        setTimeout(() => {
            that.setState({ visible: false });
        }, 3000);        
    }

    public render() {        
        return <div className="static-modal">
            <Modal
                show={this.props.visible}
            >
                <Modal.Body>{this.props.children}</Modal.Body>
            </Modal>
        </div>;
    }
}
