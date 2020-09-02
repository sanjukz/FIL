import axios from "axios";
import { RegistrationResponseViewModel } from 'shared/models/RegistrationResponseViewModel'

function register(user) {
    return axios.post("api/account/register", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<RegistrationResponseViewModel>)
        .catch((error) => {
        });
}

export const userService = {
    register,
};
