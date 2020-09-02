export class SignUpUserModel {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
    phoneNumber: string;
    phoneCode: string;
    isMailOpt: boolean;
}


export class SignInUserModel {
    email: string;
    password: string;
}

export class ForgetPaswordModel {
    password: string;
    confirmPassword: string;
    firstName?: string;
    lastName?: string;
    email?: string;
    oldPassword?: string;
}