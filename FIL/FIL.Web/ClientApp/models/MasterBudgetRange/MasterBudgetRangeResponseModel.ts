import CurrencyTypeViewModel from "../Comman/CurrencyTypeViewModel";

export class MasterBudgetRangeResponseModel {
    currencyTypes: CurrencyTypeViewModel[];
    masterBudgetRanges: MasterBudgetRange[];
}

export class MasterBudgetRange {
    id: number;
    currencyId: number;
    minPrice: number;
    maxPrice: number;
    sortOrder: number;
}