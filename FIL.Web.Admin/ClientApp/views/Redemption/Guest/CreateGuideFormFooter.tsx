import * as React from "react";


const CreateGuideFormFooter: React.FunctionComponent<any> = (props) => {
    return (
        <div className="form-group">
            <div className="row bg-white m-0 border rounded shadow-sm py-2">
                <div className="col-6">
                    <button
                        disabled={props.isDisabledPreviousButton}
                        type="button" className="btn btn-link p-0"
                        onClick={props.previousTabHandler}>
                        <i className="fa fa-long-arrow-left mr-2" aria-hidden="true"></i>
                        Previous
                    </button>
                </div>
                <div className="col-6 text-right">
                    <button
                        disabled={props.isDisabledNextButton} type="submit"
                        className="btn btn-link p-0">
                        Next
                        <i className="fa fa-long-arrow-right ml-2" aria-hidden="true"></i>
                    </button>
                </div>
            </div>
        </div>
    );
};

export default CreateGuideFormFooter;