import * as React from 'react';
import { Switch } from 'antd';
import BreadcrumbAndTitle from '../BreadcrumbAndTitle';

interface Iprops {
    gets3BaseUrl: string;
}
function PrivacyAndSharing(props: Iprops) {
    return <>
        <div className="container">

            <BreadcrumbAndTitle title={'Privacy and Sharing'} />

            <div className="row">
                <div className="col-sm-8">
                    <h5>Pick what you share</h5>
                    <p>feelitLIVE cares about your privacy. Use this screen to choose how you want to share your activity.</p>

                    <div className="mt-5">
                        <div>
                            <div className="pull-right">
                                <Switch disabled={true} />
                            </div>
                            <p className="font-weight-bold"> Share my activity on Facebook</p>
                        </div>
                        <p>Turning this on means your activity will be shared to Facebook, which could include your username, profile photo, and locations you've visited.</p>
                    </div>

                    <hr className="my-4" />

                    <div>
                        <div>
                            <div className="pull-right">
                                <Switch disabled={true} />
                            </div>
                            <p className="font-weight-bold"> Include my profile and listing in search engines</p>
                        </div>
                        <p>Turning this on means search engines, like Google, will display your profile and listing pages in search results.</p>
                    </div>
                </div>
                <div className="col-sm-4 mt-4 mt-sm-0 pl-md-5">
                    <div className="border p-4">
                        <img
                            src={`${props.gets3BaseUrl}/my-account/right-bar-images/personal-info.svg`}
                            className="img-fluid mb-4"
                            alt=""
                        />
                        <div>
                            <h5 className="mb-3">Pick what you share</h5>
                            <p className="m-0">
                                FeelitLIVE cares about your privacy. Use this screen to choose
                                how you want to share your activity.
                </p>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </>
}

export default PrivacyAndSharing;