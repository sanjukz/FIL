import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../index";
import { ImageViewModel, ImageModel } from "../../models/CreateEventV1/ImageViewModel";
import { eventImageService } from "../../services/CreateEventV1/EventImage";

export interface IEventImageProps {
  EventImage: IEventImageComponentState;
}

export interface IEventImageComponentState {
  isLoading?: boolean;
  isEventImageSuccess?: boolean;
  isEventImageFailure?: boolean;
  isSaveRequest?: boolean;
  eventImage?: any;
}

interface IRequestEventImage {
  type: "REQUEST_EVENT_IMAGE_DATA";
  isSaveRequest: boolean
}

interface IReceiveEventImage {
  type: "RECEIVE_EVENT_IMAGE_DATA";
  eventImage: ImageViewModel;
}

interface IFailureEventImage {
  type: "FAILURE_EVENT_IMAGE_DATA";
}

type KnownAction = IRequestEventImage | IReceiveEventImage | IFailureEventImage;

export const actionCreators = {
  requestEventImage: (eventId: number, callback: (ImageViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/event-images/${eventId}`)
        .then((response) => response.json() as Promise<ImageViewModel>)
        .then((data: ImageViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_IMAGE_DATA", eventImage: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_IMAGE_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_IMAGE_DATA", isSaveRequest: false });
    },
  saveEventImages: (eventImageModel: ImageViewModel, callback: (EventHostsViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_IMAGE_DATA", isSaveRequest: true });
      eventImageService.saveEventImageRequest(eventImageModel)
        .then((response: ImageViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_IMAGE_DATA", eventImage: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_IMAGE_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_IMAGE_DATA" });
          });
    }
};


let unloadedImageModel: ImageModel = {
  eventAltId: null,
  id: 0,
  eventId: 0,
  isBannerImage: false,
  isHotTicketImage: false,
  isPortraitImage: false,
  isVideoUploaded: false
}

let unloadedImageViewModel: ImageViewModel = {
  eventImageModel: unloadedImageModel,
  success: false,
  isDraft: false,
  isValidLink: false
}

const unloadedeventImageState: IEventImageComponentState = {
  isLoading: false,
  eventImage: unloadedImageViewModel,
  isEventImageFailure: false,
  isEventImageSuccess: false,
  isSaveRequest: false
};

export const reducer: Reducer<IEventImageComponentState> = (state: IEventImageComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_IMAGE_DATA":
      return {
        ...state,
        isLoading: true,
        isEventImageFailure: false,
        isEventImageSuccess: false,
        isSaveRequest: action.isSaveRequest,
        eventImage: unloadedeventImageState
      };
    case "RECEIVE_EVENT_IMAGE_DATA":
      return {
        ...state,
        isLoading: false,
        isEventImageFailure: false,
        isEventImageSuccess: true,
        isSaveRequest: false,
        eventImage: action.eventImage
      };
    case "FAILURE_EVENT_IMAGE_DATA":
      return {
        ...state,
        isLoading: false,
        isEventImageFailure: true,
        isEventImageSuccess: false,
        isSaveRequest: false,
        eventImage: unloadedeventImageState
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedeventImageState;
};
