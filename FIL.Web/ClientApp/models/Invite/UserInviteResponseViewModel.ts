export class UserInviteResponseViewModel {
    userInvite: UserInvite;
}

export class UserInvite {
    id: number;
    userEmail: string;
    userInvite: string;
    websiteId: number;
    isUsed: boolean;
    createdOn: Date;
    createdBy: number;
    usedOn: Date; 
    modifiedOn: Date;
    modifiedBy : number;
}
