import * as React from "react";
import Modal from 'react-awesome-modal';
import CKEditor from "react-ckeditor-component";
import { Service } from "../../../models/Redemption/Service";
import * as _ from "lodash";

export default class ServiceModalComponent extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            isShow: false,
            service: '',
            description: '',
            serviceId: -1,
            isHydrateState: false,
            selectedServices: []
        };
    }

    componentDidUpdate(prevProps, prevState) {
        if (this.props.services.length > 0 && prevState.isHydrateState == false) {
            this.setState({
                isHydrateState: true,
                selectedServices: [...this.props.services]
            });
        }
    }

    render() {
        let that = this;
        return (
            <div>
                {this.state.selectedServices.map((item, index) => {
                    return (
                        <div key={item.id} className="card col-sm-6 p-0 mb-2">
                            <div className="card-header border-0 p-2">
                                <a href="javascript:void(0)" onClick={() => {
                                    that.setState({
                                        serviceId: item.id,
                                        service: item.name,
                                        description: item.description,
                                        isShow: true
                                    });
                                }}>{item.name}</a>
                                <div className="pull-right">
                                    <a href="javascript:void(0)"><i className="fa fa-trash-o text-danger"
                                        onClick={() => {
                                            let services = that.state.selectedServices;
                                            _.remove(services, { id: item.id });
                                            that.setState({
                                                selectedServices: services,
                                                serviceId: -1,
                                                service: '',
                                                description: ''
                                            });
                                        }} ></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                    );
                })
                }
                <div className="form-group my-2">
                    <a href="JavaScript:Void(0)"
                        onClick={() => {
                            this.setState({
                                isShow: true,
                                service: '',
                                description: '',
                                serviceId: -1
                            });
                        }} className="btn btn-sm btn-outline-primary">
                        <small>
                            <i className="fa fa-plus mr-2" aria-hidden="true"></i>
                            Add New Service
                        </small>
                    </a>
                </div>
                <Modal
                    visible={this.state.isShow}
                    width="800"
                    effect="fadeInUp"
                    onClickAway={() => this.setState({ isShow: false })} >
                    <form className="p-3"
                        onSubmit={(item: any) => {
                            let allServices = that.state.selectedServices;
                            let findService = _.find(allServices, { id: that.state.serviceId });
                            if (findService) {
                                allServices = allServices.map((val) => {
                                    if (val.id == that.state.serviceId) {
                                        val.name = that.state.service;
                                        val.description = that.state.description;
                                    }
                                    return val;
                                });
                            } else {
                                let serviceModal: Service = {
                                    description: this.state.description,
                                    name: that.state.service,
                                    id: 0
                                }
                                allServices.push(serviceModal);
                            }
                            allServices = allServices.map((item, index) => {
                                item.id = index;
                                return item;
                            })
                            this.setState({ selectedServices: allServices, isShow: false, description: '', service: '', serviceId: -1 });
                            this.props.onClickSubmit(allServices);
                            item.preventDefault();
                            item.stopPropagation();
                        }}
                    >
                        <div>
                            <label className="font-weight-bold"> Service Name </label>
                            <input
                                name="service"
                                placeholder="Enter Service Name"
                                value={this.state.service}
                                onChange={(e) => {
                                    that.setState({ service: e.target.value });
                                }}
                                className="form-control"
                                type="service" required />
                        </div>
                        <div className="mt-2">
                            <label className="font-weight-bold"> Service Description </label>
                            <CKEditor
                                activeClass="p10"
                                content={this.state.description}
                                required
                                events={{
                                    "change": (e) => {
                                        this.setState({ description: e.editor.getData() })
                                    }
                                }}
                            />
                        </div>
                        <div>
                            <button className="btn btn-primary mt-2" type="submit" >Save</button>
                        </div>
                    </form>
                </Modal>
            </div >
        )
    }
}
