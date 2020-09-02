import axios from "axios";
import { ImageViewModel } from "../../models/CreateEventV1/ImageViewModel";

function saveEventImageRequest(eventImage) {
  return axios.post("api/save/image", eventImage, {
    headers: {
      "Content-Type": "application/json"
    }
  }).then((response) => response.data as Promise<ImageViewModel>)
    .catch((error) => {
    });
}

export const eventImageService = {
  saveEventImageRequest
};

