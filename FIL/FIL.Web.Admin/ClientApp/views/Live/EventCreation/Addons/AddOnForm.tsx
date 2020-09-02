import * as React from "react";
import { Formik, Form, Field, FormikProps } from "formik";
import Select from "react-select";
import ImageUpload from "../../../../components/ImageUpload/ImageUpload";
import { TicketCategories } from "../../../../models/Inventory/InventoryRequestViewModel";

class AddOnForm extends React.Component<any, any> {
  onSubmitForm = (values) => {
    let data = this.props.currencyOptions.filter(t => t.value == this.props.selectedCurrencyId);
    let tc: TicketCategories = {
      categoryName: values.title,
      eventTicketDetailId: 0,
      pricePerTicket: values.price,
      ticketCategoryId: 0,
      isEventTicketAttributeUpdated: true,
      quantity: values.quantity,
      ticketCategoryDescription: "",
      ticketCategoryNote: "",
      currencyId: data.length > 0 ? data[0].value ? data[0].value : 11 : 11,
      isRollingTicketValidityType: true,
      ticketValidityFixDate: "",
      days: "",
      month: "",
      year: "",
      ticketCategoryTypeId: 2,
      ticketSubCategoryTypeId: 1,
      currencyCountryId: data.length > 0 ? data[0].countryId ? data[0].countryId : 231 : 231
    };

    if (this.props.isDrawer) {
      this.props.onAddAddons(tc);
    } else {
      this.props.onSubmitAddOns(tc)
    }
  }

  render() {
    return (
      <Formik
        enableReinitialize
        initialValues={this.props.selectedItem ? {
          title: this.props.selectedItem.categoryName,
          price: this.props.selectedItem.pricePerTicket,
          quantity: this.props.selectedItem.quantity
        } : {}}
        onSubmit={this.onSubmitForm}
      >
        {(props: FormikProps<any>) => {
          this.props.bindSubmitForm(props.submitForm)
          return (
            <Form>
              <div className="col-sm-12 p-2">
                <div className="collapse multi-collapse show pt-3" id="Addons">
                  <div className="form-group">
                    <div className="row">
                      <div className="col-sm-6">
                        <label >Title</label>
                        <Field
                          name="title"
                          className="form-control"
                          type="text"
                          placeholder="Title"
                          required
                        />
                      </div>
                      <div className="col-sm-6">
                        <label >Quantity</label>
                        <Field
                          name="quantity"
                          className="form-control"
                          type="number"
                          placeholder="Quantity"
                          required
                        />
                      </div>
                    </div>
                    <div className="row">
                      <div className="col-12 col-sm-6">
                        <label >Select Currency</label>
                        <Select
                          name="currency"
                          onChange={e => props.setFieldValue('currency', e)}
                          defaultValue={this.props.currencyOptions.filter(t => t.value == this.props.selectedCurrencyId)[0]}
                          options={this.props.currencyOptions}
                          value={this.props.currencyOptions.filter(t => t.value == this.props.selectedCurrencyId)[0]}
                          isDisabled={true}
                        />
                      </div>
                      <div className="col-12 col-sm-6">
                        <label >Price Per Unit</label>
                        <Field
                          name="price"
                          className="form-control"
                          type="number"
                          placeholder="Price"
                          required
                        />
                      </div>
                    </div>
                    <div className="row">
                      <div className="col-12">
                        <ImageUpload
                          imageInputList={[{ imageType: "tile", numberOfFields: 1 }]}
                          onImageSelect={(imageModel) => {
                            props.setFieldValue('image', imageModel)
                          }}
                          onImageRemove={(imageModel) => { }}
                        />
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </Form>
          )
        }}
      </Formik>
    );
  }
}

export default AddOnForm;
