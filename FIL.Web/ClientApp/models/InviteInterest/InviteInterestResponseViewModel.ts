export class InviteInterestResponseViewModel {
    userInvite: InviteInterest;
}

export class InviteInterest {
    id: number;
    userEmail: string;
    firstName: string;
    lastName: string;
    nationality: number;
    createdUtc: Date;
    createdBy: number;
    modifiedUtc: Date;
    modifiedBy : number;
}