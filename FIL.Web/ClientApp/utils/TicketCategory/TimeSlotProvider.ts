
const daysInWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

export class TimeSlotModel {
    timeSlot: any[];
    isTimeTypeExists: false
}

export const getTimeSlot = (date: any, eventDetail: any, ticketCategoryData: any) => {
    var isTimeTypeExists = false;
    var timeModel = [];
    var isSeasonDate = false;
    var isSpecialDay = false;

    /*----------- Special Day Time Type ------------------*/
    ticketCategoryData.specialDayModel.map(function (item) {
        isTimeTypeExists = true;
        if (new Date(item.specialDate) == date) {
            isSpecialDay = true;
            var newTimeModel = {
                index: 1,
                fromTime: item.from,
                toTime: item.to,
            };
            timeModel.push(newTimeModel);
        }
    });

    /*----------- Season Time Type ------------------*/
    if (!isSpecialDay) {
        ticketCategoryData.seasonTimeModel.map(function (currentSeason) {
            isTimeTypeExists = true;
            var isDayExistsInSeason = false;
            var startYear = new Date(eventDetail.startDateTime).getFullYear();
            var endYear = new Date(eventDetail.endDateTime).getFullYear();
            for (var d = startYear; d <= endYear; d++) {
                var startDate = new Date(currentSeason.startDate);
                var endDate = new Date(currentSeason.endDate);
                startDate.setFullYear(d);
                endDate.setFullYear(new Date(currentSeason.endDate).getFullYear() > d ? new Date(currentSeason.endDate).getFullYear() : d);
                startDate.setHours(0, 0, 0);
                endDate.setHours(0, 0, 0);
                date.setHours(0, 0, 0);
                if (date >= startDate && date <= endDate) {
                    var currentDays = date.getDay();
                    if (currentDays == 0) {
                        currentDays = 7;
                    }
                    var isOffDay = currentSeason.daysOpen[currentDays];
                    if (isOffDay) {
                        isDayExistsInSeason = true;
                        break;
                    }
                }
                if (isDayExistsInSeason) {
                    break;
                }
            }
            if (isDayExistsInSeason && timeModel.length == 0) {
                isSeasonDate = true;
                if (currentSeason.isSameTime) {
                    var index = 0;
                    currentSeason.sameTime.map(function (item) {
                        var newTimeModel = {
                            index: index + 1,
                            fromTime: item.from,
                            toTime: item.to,
                        };
                        index = index + 1;
                        timeModel.push(newTimeModel);
                    });
                } else if (!currentSeason.isSameTime) {
                    var currentDay = date.getDay();
                    if (currentDay == 0) {
                        currentDay = 7;
                    }
                    var selectedDay = daysInWeek[currentDay - 1];
                    currentSeason.time.map(function (item) {
                        if (item.day == selectedDay) {
                            var index = 0;
                            item.time.map(function (item) {
                                var newTimeModel = {
                                    index: index + 1,
                                    fromTime: item.from,
                                    toTime: item.to,
                                };
                                index = index + 1;
                                timeModel.push(newTimeModel);
                            })
                        }
                    });
                }
            }
        });
    }
    /*-------- Regular Time Type -------------*/
    if (!isSeasonDate && !isSpecialDay) {
        if (ticketCategoryData.regularTimeModel.isSameTime) {
            var index = 0;
            ticketCategoryData.regularTimeModel.timeModel.map(function (item) {
                isTimeTypeExists = true;
                var newTimeModel = {
                    index: index + 1,
                    fromTime: item.from,
                    toTime: item.to,
                };
                index = index + 1;
                timeModel.push(newTimeModel);
            });
        } else if (!ticketCategoryData.regularTimeModel.isSameTime) {
            var currentDay = date.getDay();
            if (currentDay == 0) {
                currentDay = 7;
            }
            var selectedDay = daysInWeek[currentDay - 1];
            ticketCategoryData.regularTimeModel.customTimeModel.map(function (item) {
                isTimeTypeExists = true;
                if (item.day == selectedDay) {
                    var index = 0;
                    item.time.map(function (item) {
                        var newTimeModel = {
                            index: index + 1,
                            fromTime: item.from,
                            toTime: item.to,
                        };
                        index = index = 1;
                        timeModel.push(newTimeModel);
                    })
                }
            });
        }
    }
    let timeSlotModel: TimeSlotModel = {
        isTimeTypeExists: isTimeTypeExists,
        timeSlot: timeModel
    }
    return timeSlotModel;
};
