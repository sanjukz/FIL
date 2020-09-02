import responseModel from "../../models/ItinenaryDataResonseModel";

export class RootObject {
    lanes: Lane[];
}

export class Lane {
    id: string;
    title: string;
    label: string;
    style: Style;
    cards: Card[];
}

export class Card {
    id: string;
    title: string;
    label: string;
    description: string;
}

export class Style {
    width: number;
}

export default class ItineraryBoardInputViewModel {
    rootObject: RootObject;
    cardId: string;
    isDelete: boolean;
    sourceLaneId: string;
    targetLaneId: string;
    position: number;
    cardDetails: Card;
    ItineraryBoardData: responseModel[][];
    ItineraryBoardCopyData: responseModel[][];
    isAddNew: boolean;
    placeId: number;
}
