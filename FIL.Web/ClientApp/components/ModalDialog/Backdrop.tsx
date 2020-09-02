import * as React from "react";

const Backdrop = ({ show, close }) => (
    show ? <div style={{
        width: "100 %",
        height: "100 %",
        backgroundColor: "rgba(0, 0, 0, 0.5)",
        position: "fixed",
        top: 0,
        left: 0,
        zIndex: 100
    }
    } onClick={close}></div> : null
);

export default Backdrop;