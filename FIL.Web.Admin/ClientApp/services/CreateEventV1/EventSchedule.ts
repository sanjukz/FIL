import axios from "axios";
import { EventScheduleViewModel } from "../../models/CreateEventV1/EventScheduleViewModel";

function saveEventScheduleRequest(eventSchedule) {
  return axios.post("api/save/event-schedule", eventSchedule, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventScheduleViewModel>)
    .catch((error) => {
    });
}

function saveBulkInsertRequest(eventSchedule) {
  return axios.post("api/schedule/bulk-insert", eventSchedule, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventScheduleViewModel>)
    .catch((error) => {
    });
}

function saveBulkRescheduleRequest(eventSchedule) {
  return axios.post("api/reschedule/bulk-reschedule", eventSchedule, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventScheduleViewModel>)
    .catch((error) => {
    });
}

function saveSingleRescheduleRequest(eventSchedule) {
  return axios.post("api/reschedule/single", eventSchedule, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventScheduleViewModel>)
    .catch((error) => {
    });
}

function deleteBulkScheduleRequest(eventSchedule) {
  return axios.post("api/schedule/bulk-delete", eventSchedule, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventScheduleViewModel>)
    .catch((error) => {
    });
}

function deleteSingleScheduleRequest(eventSchedule) {
  return axios.post("api/schedule/delete", eventSchedule, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<EventScheduleViewModel>)
    .catch((error) => {
    });
}

export const eventScheduleService = {
  saveEventScheduleRequest,
  saveBulkInsertRequest,
  saveBulkRescheduleRequest,
  saveSingleRescheduleRequest,
  deleteBulkScheduleRequest,
  deleteSingleScheduleRequest
};

