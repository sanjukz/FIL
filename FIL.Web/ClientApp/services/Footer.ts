import axios from "axios";
import { NewsLetterSignupFooterResponseDataViewModel } from "../models/Footer/NewsLetterSignupFooterResponseDataViewModel";

function NewsLetterSignUpRegister(NewsLetterSignUpModel) {
    return axios.post("api/newsletter/register", NewsLetterSignUpModel, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<NewsLetterSignupFooterResponseDataViewModel>)
        .catch((error) => {
            alert(error);
        });
}

export const FooterService = {
    NewsLetterSignUpRegister
};
