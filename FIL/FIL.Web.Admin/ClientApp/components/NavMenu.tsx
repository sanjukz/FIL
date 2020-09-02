import * as React from "react";
import { gets3BaseUrl } from "../../ClientApp/utils/imageCdn";

export class NavMenu extends React.Component<{}, {}> {
    public render() {
        const image = `${gets3BaseUrl()}/logos/feel-aplace.png`;
        return <nav className="navbar navbar-default row">
            <div className="container-fluid">
                <div className="navbar-header w-100">
                    <div className="pull-right rounded border border-    p-1">
                        <img src={`${gets3BaseUrl()}/logos/fap-live-stream.png`} width="90" className="mt-0" alt="fap live logo" />
                    </div>
                </div>
            </div>
        </nav>;
    }
}
