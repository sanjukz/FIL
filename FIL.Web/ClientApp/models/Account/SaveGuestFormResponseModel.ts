export  class SaveGuestFormResponseModel {
    guestUserDetails: guestUserDetails[];
}

 class guestUserDetails {
    altId: string;
    userId: number;
    firstName: string;
    lastName: string;
    nationality: string;
    documentType: string;
    documentNumber:string;
}
