import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";

interface IPickupDetailFormProps {
	onSubmit: (values: any) => void;
	initialValues: any;
	validationSchema: any; // TODO: figure out where Yup SCHEMA went
}

export default class CardDetailForm extends React.Component<IPickupDetailFormProps, any> {
	public render() {
		return <Formik
			onSubmit={this.props.onSubmit}
			initialValues={this.props.initialValues || {}}
			validationSchema={this.props.validationSchema}
		>
			{(props) => (<Form>
				{this.props.children}
                <div className="gradient-bg border row">	
                    <div className="container pt-2 pb-2 text-right">
                        <Button type="submit" className="btn site-primery-btn text-uppercase">Continue</Button> 
                        </div>
				</div> 				
			</Form>)}
		</Formik>
	}
}
