import * as React from "react";

export class CreateSeats extends React.Component<any, any> {
    public render() {
        return <div className="pt-20 pb-15">
            <div className="create-event-seats clearfix">
                <div className="venue-layout bg-gray-darker">
                    <div className="container-fluid">
                        <div className="row venue-builder collapsed">
                            <div className="pb-10 text-center bg-gray-light">
                                <div className="row">
                                    <div className="col-md-3">
                                        <div className="stand collapsed"><a href="#" className="btn btn-light btn-block">North Stand</a></div>
                                        <div className="stand collapsed"><a href="#" className="btn btn-light btn-block">North Stand</a></div>
                                        <a className="btn-black" href="#"><i className="fa fa-plus"></i></a>
                                    </div>
                                    <div className="col-md-3">
                                        <div className="level collapsed"><a href="#" className="btn btn-light btn-block">300 Level</a></div>
                                        <div className="level collapsed"><a href="#" className="btn btn-light btn-block">400 Level</a></div>
                                        <a className="btn-black" href="#"><i className="fa fa-plus"></i></a>
                                    </div>
                                    <div className="col-md-3">
                                        <div className="block collapsed"><a href="#" className="btn btn-light btn-block">Block A</a></div>
                                        <div className="block collapsed"><a href="#" className="btn btn-light btn-block">Block B</a></div>
                                        <div className="block collapsed"><a href="#" className="btn btn-light btn-block">Block C</a></div>
                                        <div className="block collapsed"><a href="#" className="btn btn-light btn-block">Block D</a></div>
                                        <div className="block collapsed"><a href="#" className="btn btn-light btn-block">Block E</a></div>
                                        <a className="btn-black" href="#"><i className="fa fa-plus"></i></a>
                                    </div>
                                    <div className="col-md-3">
                                        <div className="blocsection collapsed"><a href="#" className="btn btn-light btn-block">Section 1</a></div>
                                        <div className="blocsection collapsed"><a href="#" className="btn btn-light btn-block">Section 2</a></div>
                                        <div className="blocsection collapsed"><a href="#" className="btn btn-light btn-block">Section 3</a></div>
                                        <a className="btn-black" href="#"><i className="fa fa-plus"></i></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="row seat-builder white-txt">
                            <div className="col-md-12">
                                <h3>Seat Layout for Section 3, 400 Level - Block E</h3>
                                <div className="create-event-venue clearfix">
                                    <div className="col-md-9 col-md-offset-2">
                                        <div className="form-group">
                                            <input type="" name="" className="form-control" />
                                        </div>
                                        <div className="form-group text-center">
                                            or
                                        </div>
                                        <div className="form-group text-center pt-20 pb-20">
                                            Build using grid
                                        </div>
                                        <div className="row">
                                            <div className="col-sm-6">
                                                <div className="form-group">
                                                    <input type="" name="" className="form-control" />
                                                </div>
                                            </div>
                                            <div className="col-sm-6">
                                                <div className="form-group">
                                                    <input type="" name="" className="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>;
    }
}
