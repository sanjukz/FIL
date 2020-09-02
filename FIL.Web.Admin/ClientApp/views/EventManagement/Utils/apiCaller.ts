/* Local imports */
import { EventStepViewModel } from "../../../models/CreateEventV1/EventStepViewModel";

export const updateEventStep = (props, stepObject, callback: (EventSponsorViewModel) => void) => {
  return props.saveEventStep(stepObject, (response: EventStepViewModel) => {
    callback(response);
  })
}

export const removeTags = (str) => {
  let charLength = 0;
  if ((str === null) || (str === ''))
    return charLength;
  else {
    return str.replace(/(<([^>]+)>)/ig, '').replace(/&nbsp/gi, "").length - 1;
  }
}