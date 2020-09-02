import * as React from 'react';
import './NewLoader.css';

const NewLoader = () => {
    return (
        <div className="newSpinner m-auto">
            <div className="bounce1" />
            <div className="bounce2" />
            <div className="bounce3" />
        </div>
    );
}

export default NewLoader;