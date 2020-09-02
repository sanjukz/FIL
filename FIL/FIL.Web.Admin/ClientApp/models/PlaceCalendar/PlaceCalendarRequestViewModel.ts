// This file was generated from the Models.tst template
//

export class PlaceCalendarRequestViewModel {
    placeAltId: string;
    placeType: number;
    weekOffDays: boolean[];
    placeStartDate: string;
    placeEndDate: string;
    venueAltId: string;
    placeTimings: timeViewModel[];
    holidayDates: string[];
    isEdit: boolean;
    regularTimeModel: regularViewModel;
    seasonTimeModel: seasonViewModel[];
    specialDayModel: specialDayViewModel[];
    isNewCalendar: boolean;
    timeZoneAbbreviation?: string;
    timeZone?: string;
}

export class timeViewModel {
    id: number;
    from: string;
    to: string;
}

export class speecialDateSeasonTimeViewModel {
    day: string;
    time: timeViewModel[];
}

export class seasonViewModel {
    id: number;
    isSameTime: boolean;
    startDate: string;
    endDate: string;
    name: string;
    sameTime: timeViewModel[];
    time: speecialDateSeasonTimeViewModel[];
    daysOpen: boolean[]
}

export class specailDatimeViewModel {
    day: string;
    time: timeViewModel;
}
export class customTimeModelData {
    day: string;
    id: number;
    time: timeViewModel[];
}

export class regularViewModel {
    isSameTime: boolean;
    customTimeModel: customTimeModelData[];
    timeModel: timeViewModel[];
    daysOpen: boolean[];
}

export class specialDayViewModel {
    id: number;
    name: string;
    specialDate: string;
    from: string;
    to: string;
}