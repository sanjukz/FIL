import * as React from "react";
import Backdrop from './Backdrop';

let styles: React.CSSProperties = {
    position: 'fixed',
    zIndex: 500,
    backgroundColor: 'white',
    width: '70 %',
    border: '1px solid #ccc',
    boxShadow: '1px 1px 1px black',
    padding: '16px',
    margin: 'auto',
    left: '15 %',
    top: '30 %',
    boxSizing: 'border-box',
    transition: 'all 0.3s ease-out'
};

class Modal extends React.Component<any, any> {
    shouldComponentUpdate(nextProps, nextState) {
        return nextProps.show !== this.props.show || nextProps.children !== this.props.children;
    }

    render() {
        return (
            <>
                <Backdrop show={this.props.show} close={this.props.closeModal} />
                <div style={{
                        ...styles,
                        transform: this.props.show ? 'translateY(0)' : 'translateY(-100vh)',
                        opacity: this.props.show ? 1 : 0
                    }}>
                    {this.props.children}
                </div>
            </>
        );
    }
}

export default Modal;