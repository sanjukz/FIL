
export const getDisabledDates = (ticketCategory: any) => {
    let disabledDates = [];
    let seasonDays = [];
    /* ----------------- Show season days for each year ------------------*/
    if (ticketCategory.ticketCategoryData.eventDetail.length > 0) {
        var startDate = ticketCategory.ticketCategoryData.eventDetail[0].startDateTime;
        var endDate = ticketCategory.ticketCategoryData.eventDetail[0].endDateTime;
        var startYear = new Date(startDate).getFullYear();
        var endYear = new Date(endDate).getFullYear();
        for (var d = startYear; d <= endYear; d++) {
            ticketCategory.ticketCategoryData.seasonTimeModel.map(function (item) {
                var startDate = new Date(item.startDate);
                var endDate = new Date(item.endDate);
                startDate.setFullYear(d);
                endDate.setFullYear(new Date(item.endDate).getFullYear() > d ? new Date(item.endDate).getFullYear() : d);
                for (var currentDate = startDate; currentDate <= endDate; currentDate.setDate(currentDate.getDate() + 1)) {
                    var currentDay = currentDate.getDay();
                    if (currentDay == 0) {
                        currentDay = 7;
                    }
                    var isOffDay = item.daysOpen[currentDay];
                    if (!isOffDay) {
                        disabledDates.push(new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate()));
                    } else {
                        seasonDays.push(new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate()));
                    }
                }
            })
        }
    }
    if (ticketCategory.ticketCategoryData.eventDetail.length > 0) {
        var startDate = ticketCategory.ticketCategoryData.eventDetail[0].startDateTime;
        var endDate = ticketCategory.ticketCategoryData.eventDetail[0].endDateTime;
        for (var d1 = new Date(startDate); d1 <= new Date(endDate); d1.setDate(d1.getDate() + 1)) {
            var currentDay = d1.getDay();
            var isDayExistsInSeason = false;
            var isDayExitsInRegular = false;
            var startYear = new Date(startDate).getFullYear();
            var endYear = new Date(endDate).getFullYear();

            var isDateExistsInSeason = seasonDays.filter(function (item) {
                return item.getTime() == d1.getTime()
            });
            if (isDateExistsInSeason.length > 0) {
                isDayExistsInSeason = true
            }
            if (!isDayExistsInSeason) {
                if (ticketCategory.ticketCategoryData.regularTimeModel.daysOpen.length > 0) {
                    if (currentDay == 0) {
                        currentDay = 7;
                    }
                    var isOffDay = ticketCategory.ticketCategoryData.regularTimeModel.daysOpen[currentDay];
                    if (!isOffDay && (ticketCategory.ticketCategoryData.regularTimeModel.timeModel.length > 0 || ticketCategory.ticketCategoryData.regularTimeModel.customTimeModel.length > 0)) {
                        disabledDates.push(new Date(d1.getFullYear(), d1.getMonth(), d1.getDate()));
                        isDayExitsInRegular = false;

                    }
                }
            }
            if (ticketCategory.ticketCategoryData.seasonTimeModel.length > 0 && !isDayExistsInSeason && !isDayExitsInRegular) {
                disabledDates.push(new Date(d1.getFullYear(), d1.getMonth(), d1.getDate()));
            }
        }
    }
    ticketCategory.ticketCategoryData.placeHolidayDates.map(function (item) {
        var holidayDates = item.leaveDateTime.split("-");
        var holidatDay = holidayDates[2].split("T");
        disabledDates.push(new Date(+holidayDates[0], (+holidayDates[1] - 1), +holidatDay[0]));
    });

    return disabledDates;
}

export const getActiveDate = (disabledDates: any, ticketCategory: any) => {
    let startActiveDate = new Date();
    if (disabledDates.length > 0) {
        for (var d1 = new Date(startActiveDate); d1 <= new Date(Math.max.apply(null, disabledDates)); d1.setDate(d1.getDate() + 1)) {
            var isDateExists = disabledDates.findIndex((x) => {
                return x.toDateString() == d1.toDateString();
            });
            if (isDateExists == -1) {
                startActiveDate = d1;
                break;
            }
        }
    }
    if (ticketCategory.ticketCategoryData != undefined
        && ticketCategory.ticketCategoryData.eventDetail != undefined &&
        ticketCategory.ticketCategoryData.eventDetail.length > 0 &&
        (new Date(startActiveDate.setHours(0, 0, 0, 0)) < new Date(new Date(new Date(ticketCategory.ticketCategoryData.eventDetail[0].startDateTime).getFullYear(), new Date(ticketCategory.ticketCategoryData.eventDetail[0].startDateTime).getMonth(), new Date(ticketCategory.ticketCategoryData.eventDetail[0].startDateTime).getDate()).setHours(0, 0, 0, 0)))) {
        startActiveDate = new Date(new Date(new Date(ticketCategory.ticketCategoryData.eventDetail[0].startDateTime).getFullYear(), new Date(ticketCategory.ticketCategoryData.eventDetail[0].startDateTime).getMonth(), new Date(ticketCategory.ticketCategoryData.eventDetail[0].startDateTime).getDate()).setHours(0, 0, 0, 0));
    }
    return startActiveDate;
}