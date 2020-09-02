import responseModel from "../../models/ItinenaryDataResonseModel";

export default class ItineraryBoardResponseViewModel {
    itineraryBoardData: responseModel[][];
    success: boolean;
    isValidDandD:boolean;
    isSourceCountZero:boolean;
    isTargetDateExceed:boolean;
}
