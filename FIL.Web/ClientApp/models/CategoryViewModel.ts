// This file was generated from the Models.tst template
//
import { MasterEventTypes } from '../Enum/MasterEventTypes'

export class CategoryViewModel {
    id: number;
    displayName: string;
    slug: string;
    eventCategory?: number;
    order: number;
    isHomePage: boolean;
    categoryId?: number;
    isFeel: boolean;
    eventCategoryId: number;
    subCategories?: CategoryViewModel[];
    category?: string;
    subCategory?: string;
    masterEventTypeId?: MasterEventTypes;
}
