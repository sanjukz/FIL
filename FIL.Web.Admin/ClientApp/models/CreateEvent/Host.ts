// This file was generated from the Models.tst template
//
import ImageDetailModal from "../../components/Redemption/Guide/DocumentImageUpload";

export class Host {
    eventId: number;
    id: number;
    altId: string;
    firstName: string;
    lastName: string;
    email: string;
    description: string;
}

export class HostRequestModel extends Host {
    imageDetail?: ImageDetailModal;
}
