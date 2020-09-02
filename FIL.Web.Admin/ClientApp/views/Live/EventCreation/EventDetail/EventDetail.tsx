import * as React from 'react'
import { Formik, Form, Field, FormikProps } from 'formik'
import { Tooltip } from 'antd'
import * as _ from 'lodash'
import { Select, Radio } from 'antd'
import CKEditor from 'react-ckeditor-component'

export default class EventDetail extends React.Component<any, any> {
  constructor(props) {
    super(props)
    this.state = {
      category: null,
      defaultCategory: 'Select Experience/Event Category'
    }
  }

  render() {
    const { Option } = Select
    let eventCategories = []
    if (this.props.fetchEventCategoriesSuccess) {
      let categories = []
      this.props.categories.categories.map((item) => {
        if (item.categoryId == 98) {
          let eventCategory = {
            value: item.value,
            label: item.displayName
          }
          categories.push(eventCategory)
        }
      })
      categories = categories.sort(function (a, b) {
        return a.label > b.label ? 1 : b.label > a.label ? -1 : 0
      })
      categories.map((item) => {
        eventCategories.push(
          <Option value={item.value} key={`${item.value}+${item.label}`}>
            {item.label}
          </Option>
        )
      })
    }
    if (
      this.props.isEdit &&
      this.props.fetchEventCategoriesSuccess &&
      this.props.placeDetails &&
      this.props.placeDetails.subcategoryid &&
      !this.state.isCategorySet
    ) {
      let category = this.props.categories.categories.filter((item) => {
        return item.value == +this.props.placeDetails.subcategoryid
      })
      if (category.length > 0) {
        this.setState({ defaultCategory: category[0].displayName, isCategorySet: true })
      }
    }

    return (
      <div className="col-sm-12 pb-3">
        <div className="form-group">
          <div className="row">
            <div className="col-sm-6">
              <label className="d-block">
                Title
                <Tooltip
                  title={
                    <p>
                      <span>Enter a nice catchy and short event title</span>
                    </p>
                  }
                >
                  <span>
                    <i className="fa fa-info-circle text-primary ml-2"></i>
                  </span>
                </Tooltip>
              </label>
              <Field name="title" placeholder="Enter Event Title" className="form-control" type="title" required />
            </div>
            <div className="col-sm-6 mt-3 mt-sm-0">
              <label className="d-block">
                Experience/Event Category
                <Tooltip
                  title={
                    <p>
                      <span>
                        Please select the broad category under which your experience/event falls. If you don’t see a
                        category listed please send an email to support@feelitLIVE.com
                      </span>
                    </p>
                  }
                >
                  <span>
                    <i className="fa fa-info-circle text-primary ml-2"></i>
                  </span>
                </Tooltip>
              </label>
              <Select
                showSearch
                size={'large'}
                value={this.state.defaultCategory}
                placeholder="Select Experience/Event Category"
                onChange={(e: any) => {
                  let key = e
                  this.props.props.setFieldValue('eventCategoryId', key.split('+')[0])
                  this.setState({ defaultCategory: key.split('+')[1] })
                }}
              >
                {eventCategories}
              </Select>
            </div>
            <div className="col-12 mt-2">
              <label>
                Description
                <Tooltip
                  title={
                    <p>
                      <span>
                        Write up a short description with the key highlights of your event. Please note that FeelitLIVE
                        may make grammatical corrections. As this is what audiences will read to determine whether to
                        buy a ticket to the event/experience, please make it as pithy and attractive as possible. Please
                        ensure adherence to the FeelitLIVE Additional Terms and Conditions.
                      </span>
                    </p>
                  }
                >
                  <span>
                    <i className="fa fa-info-circle text-primary ml-2"></i>
                  </span>
                </Tooltip>
              </label>

              <CKEditor
                activeClass="p10"
                content={
                  this.props.props && this.props.props.values && this.props.props.values.eventDescription
                    ? this.props.props.values.eventDescription
                    : ''
                }
                required
                events={{
                  change: (e) => {
                    this.props.props.setFieldValue('eventDescription', e.editor.getData())
                  }
                }}
              />
            </div>
          </div>
        </div>
      </div>
    )
  }
}
