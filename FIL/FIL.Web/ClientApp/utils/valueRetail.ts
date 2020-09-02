export const getDepatureLocations = (data) => {
    let temp = [];
    let locationArray = [];

    for (let i = 0; i < data.length; i++) {
        if (temp.indexOf(data[i].pickupLocation) < 0) {
            temp.push(data[i].pickupLocation);
            locationArray.push(data[i]);
        }
    }
    return locationArray;
};

export const getFilteredLocation = (locationArray, locationName) => {
    let filteredLocations = [];
    for (let i = 0; i < locationArray.length; i++) {
        if (locationArray[i].pickupLocation == locationName) {
            filteredLocations.push(locationArray[i]);
        }
    }
    return filteredLocations;
};

export const getDepartureTimes = (locationArray) => {
    let temp = [];
    let departureTimes = [];

    for (let i = 0; i < locationArray.length; i++) {
        if (temp.indexOf(locationArray[i].pickupTime) < 0) {
            temp.push(locationArray[i].pickupTime);
            departureTimes.push(locationArray[i]);
        }
    }

    return departureTimes;
};

export const getReturnTimes = (locationArray, departureTime) => {
    let temp = [];
    let returnTimes = [];

    for (let location of locationArray) {
        if (location.pickupTime == departureTime && temp.indexOf(location.returnTime) < 0) {
            temp.push(location.returnTime);
            returnTimes.push(location);
        }
    }
    return returnTimes;
};