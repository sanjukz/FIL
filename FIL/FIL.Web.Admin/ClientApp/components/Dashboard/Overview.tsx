import * as React from 'react';

function Overview(props: any) {
  return (
    <div className="py-4 mb-2">
      <div className="row">
        <div className="col-sm-4">
          <div className="media bg-white shadow-sm p-3">
            <img
              className="align-self-start mr-3"
              src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/total-tickets-sold.svg"
              width="51"
              alt="Total tickets sold"
            />
            <div className="media-body">
              <h5 className="mt-0">Total tickets sold</h5>
              <p className="display-4 mb-0 font-weight-bold">769</p>
            </div>
          </div>
        </div>
        <div className="col-sm-4">
          <div className="media bg-white shadow-sm p-3">
            <img
              className="align-self-start mr-3"
              src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/backstage-Passes.svg"
              width="36"
              alt="Backstage Passes"
            />
            <div className="media-body">
              <h5 className="mt-0">Backstage Passes</h5>
              <p className="display-4 mb-0 font-weight-bold">89</p>
            </div>
          </div>
        </div>
        <div className="col-sm-4">
          <div className="media bg-white shadow-sm p-3">
            <img
              className="align-self-start mr-3"
              src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/admin-images/icons/revenue.svg"
              width="50"
              alt="Total tickets sold"
            />
            <div className="media-body">
              <h5 className="mt-0">Revenue</h5>
              <p className="display-4 mb-0 font-weight-bold">$28,000</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Overview;
