/**
 * Randomize array element order in-place.
 * Using Durstenfeld shuffle algorithm.
 * @param array [any values]
 */
export const shuffleArray = array => {
  for (var i = array.length - 1; i > 0; i--) {
    var j = Math.floor(Math.random() * (i + 1));
    var temp = array[i];
    array[i] = array[j];
    array[j] = temp;
  }
  return array;
};

/**
 * filter places based on category
 * @param allList 
 * @param category 
 */
export const getFilteredList = (allList, categoryId, categoryName) =>
  allList.length > 0
    ? allList.filter(
        item => (item.eventCategories.indexOf(categoryName) != -1)
      )
    : [];

/** 
 *function for get formatted date
*/
export const formatDate = date => {
  var monthNames = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December"
  ];

  var day = date.getDate();
  var monthIndex = date.getMonth();
  var year = date.getFullYear();

  return day + " " + monthNames[monthIndex] + ", " + year;
};

// array sorting by date

export const sortByDate = (array: any[], order: string) => {
  if(order == "newest" || order == "asc") {
    return array.sort((a, b) => {
      return +(new Date(b.createdUtc)) - +(new Date(a.createdUtc));
    });
  } 
  return array.sort((a:any, b:any) => {
    return +(new Date(a.createdUtc)) - +(new Date(b.createdUtc));
  });
};

export const filterByRatings = (array, ratings) => {
  if(ratings.length > 0) {
    return array.filter(item => {
      return ratings.includes(item.points.toString());
    });
  }
  return array;
};