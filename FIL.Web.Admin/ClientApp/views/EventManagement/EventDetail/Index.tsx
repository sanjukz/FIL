/* Third party imports */
import * as React from "react";
import * as _ from "lodash";
import { Select } from 'antd';

/* Local imports */
import { EventDetailViewModel } from "../../../models/CreateEventV1/EventDetailViewModel";
import { setDetailsObject } from "../utils/DefaultObjectSetter";
import { Footer } from "../Footer/FormFooter";
import { Basic } from './Basic';
import { Details } from './Details';
import { MasterEventTypes } from '../../../Enums/MasterEventTypes';
import { removeTags } from "../Utils/apiCaller";

export class Index extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      categories: this.props.props.eventCreation.eventCategoriesList.categories || [],
      eventDetailViewModel: setDetailsObject(this.props.slug, this.props.isBasicTab),
      isSaveRequest: false,
      isChecked: false,
      defaultCategory: 'Select Experience/Event Category',
      eventTypeValue: 1
    }
  }
  form: any;

  componentDidMount() {
    if (this.props.slug) {
      this.props.props.requestEventDetails(this.props.slug, (response: EventDetailViewModel) => {
        if (response.success && response.isValidLink && !response.isDraft) {
          console.log(response);
          response.currentStep = this.state.eventDetailViewModel.currentStep;
          this.setState({
            eventDetailViewModel: response, isChecked: response.eventDetail.itemsForViewer.length == 0,
            eventTypeValue: response.eventDetail.isPrivate ? 2 : 1
          });
        } else {
        }
      });
    }
  }

  getEventCategories = (isAllOnlineParentCategories?: boolean) => {
    let categories = [];
    this.state.categories.map((item) => {
      if ((!isAllOnlineParentCategories && this.state.eventDetailViewModel.eventDetail.parentCategoryId == item.categoryId) || (isAllOnlineParentCategories && item.masterEventTypeId == MasterEventTypes.Online && item.categoryId == 0)) {
        let eventCategory = {
          value: item.value,
          label: item.displayName
        }
        categories.push(eventCategory);
      }
    });
    return categories.sort(function (a, b) {
      return (a.label > b.label) ? 1 : ((b.label > a.label) ? -1 : 0)
    });
  }

  isButtonDisable = () => {
    if (this.props.isBasicTab) {
      return this.state.eventDetailViewModel.eventDetail.defaultCategory &&
        this.state.eventDetailViewModel.eventDetail.name;
    } else {
      return removeTags(this.state.eventDetailViewModel.eventDetail.description) >= 200 && this.state.eventDetailViewModel.eventDetail.description && (!this.state.isChecked ? this.state.eventDetailViewModel.eventDetail.itemsForViewer.filter((val) => { if (val) { return val } }).length > 0 : true);
    }
  }

  getDisplayScreen = (eventCategories, parentCategoriesOption) => {
    return this.props.isBasicTab ? <Basic
      eventDetail={this.state.eventDetailViewModel.eventDetail}
      eventCategories={eventCategories}
      parentCategoriesOption={parentCategoriesOption}
      onChangeTitle={(e) => {
        let eventDetailViewModel = this.state.eventDetailViewModel; eventDetailViewModel.eventDetail.name = e.target.value; this.setState({ eventDetailViewModel: eventDetailViewModel })
      }}
      onChangeCategory={(e) => {
        let key = e;
        let eventDetailViewModel = this.state.eventDetailViewModel;
        eventDetailViewModel.eventDetail.defaultCategory = key.split("+")[1];
        eventDetailViewModel.eventDetail.eventCategories = key.split("+")[0];
        this.setState({ eventDetailViewModel: eventDetailViewModel });
      }}
      eventTypeValue={this.state.eventTypeValue}
      onChangeEventTypeValue={(value) => { this.setState({ eventTypeValue: value }) }}
      onChangeParentCategory={(e) => {
        let key = e;
        let eventDetailViewModel = this.state.eventDetailViewModel;
        eventDetailViewModel.eventDetail.parentCategory = key.split("+")[1];
        eventDetailViewModel.eventDetail.parentCategoryId = key.split("+")[0];
        eventDetailViewModel.eventDetail.defaultCategory = '';
        eventDetailViewModel.eventDetail.eventCategories = '';
        this.setState({ eventDetailViewModel: eventDetailViewModel });
      }}
    /> : <Details
        eventDetail={this.state.eventDetailViewModel.eventDetail}
        error={this.state.error}
        charLength={removeTags(this.state.eventDetailViewModel.eventDetail.description)}
        onChangeItems={(itemArray) => {
          let eventDetailViewModel = this.state.eventDetailViewModel;
          eventDetailViewModel.eventDetail.itemsForViewer = itemArray;
          this.setState({ eventDetailViewModel: eventDetailViewModel })
        }}
        onChangeCheckbox={(e) => {
          this.setState({ isChecked: e.target.checked })
        }}
        onFocusOut={(e) => {
          this.setState({ error: removeTags(this.state.eventDetailViewModel.eventDetail.description) >= 200 ? '' : removeTags(this.state.eventDetailViewModel.eventDetail.description) == 0 ? '200 characters needed' : `${200 - removeTags(this.state.eventDetailViewModel.eventDetail.description)} more ${(200 - removeTags(this.state.eventDetailViewModel.eventDetail.description)) == 1 ? 'character' : 'characters'} needed` })
        }}
        isChecked={this.state.isChecked}
        onChange={(e) => {
          let eventDetailViewModel = this.state.eventDetailViewModel;
          eventDetailViewModel.eventDetail.description = e.editor.getData();
          this.setState({ eventDetailViewModel: eventDetailViewModel, error: '' });
        }}
      />
  }

  render() {
    const { Option } = Select;
    let eventCategories = [];
    let parentCategoriesOption = [];
    let categories = this.getEventCategories();
    let parentCategories = this.getEventCategories(true);
    categories.map((item) => {
      eventCategories.push(<Option value={`${item.value}+${item.label}`} key={`${item.value}+${item.label}`}>{item.label}</Option>)
    })
    parentCategories.map((item) => {
      parentCategoriesOption.push(<Option value={`${item.value}+${item.label}`} key={`${item.value}+${item.label}`}>{item.label}</Option>)
    })
    return (
      <>
        <div data-aos="fade-up" data-aos-duration="1000">
          <form onSubmit={(e: any) => {
            e.preventDefault();
            let eventViewModel = this.state.eventDetailViewModel;
            eventViewModel.eventDetail.isPrivate = this.state.eventTypeValue == 2 ? true : false
            this.setState({ isSaveRequest: true });
            eventViewModel.eventDetail.itemsForViewer = !this.state.isChecked ? eventViewModel.eventDetail.itemsForViewer : [];
            this.props.props.saveEventDetails(eventViewModel, (response: EventDetailViewModel) => {
              if (response.success) {
                if (this.props.isBasicTab) {
                  this.props.changeRoute(response.eventDetail.name, 2, response.completedStep, response.eventDetail.eventId);
                } else {
                  this.props.changeRoute(3, response.completedStep);
                }

                this.setState({ eventDetailViewModel: response });
              } else {

              }
              this.setState({ isSaveRequest: false });
            })
          }}
            ref={(ref) => { this.form = ref; }}
          >
            <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="1">

              <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
                <h3 className="m-0 text-purple">{this.props.isBasicTab ? "Getting the basics right!" : "Getting into the detail"}</h3></nav>
              <div data-aos="fade-up" data-aos-duration="1000">
                <div className="form-group">
                  <div className="row">
                    {this.getDisplayScreen(eventCategories, parentCategoriesOption)}
                  </div>
                </div>

              </div>

            </div>
            <Footer
              isHideCancel={this.props.isBasicTab ? true : false}
              onClickCancel={() => { this.props.changeRoute(1, ""); }}
              isDisabled={!this.isButtonDisable()}
              isSaveRequest={this.state.isSaveRequest}
              onSubmit={() => { this.form.dispatchEvent(new Event('submit')) }} />
          </form>
        </div>
      </>
    );
  }
}

