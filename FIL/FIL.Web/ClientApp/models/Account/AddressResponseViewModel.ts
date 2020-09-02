export default class AddressResponseViewModel {
    userAddresses: UserAddress[];
}

export class UserAddress {
    altId: string;
    firstName: string;
    lastName: string;
    phoneCode: string;
    phoneNumber: number;
    addressLine1: string;
    zipcode: number;
    addressTypeId: number;
    isDefault: boolean;
}
