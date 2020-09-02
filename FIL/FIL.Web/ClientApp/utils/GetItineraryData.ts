import responseModel from "../models/ItinenaryDataResonseModel";
import { RootObject, Lane, Card, Style } from "../models/Itinerary/ItineraryBoardInputViewModel";
let month_names_short = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
let daysInWeek = ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT'];

export const GetItineraryData = (itineraryData: any) => {
    let rootBoject: RootObject = {
        lanes: []
    }
    let lenseArray = [];
    let itemId = 0;
    itineraryData.map((item, currentIndex) => {
        let card = [];
        let data = item;
        let dateLense = "";
        data.map((val, index) => {
            itemId = itemId + 1;
            var date = new Date(val.placeVisitDate).getDate().toString();
            var month = new Date(val.placeVisitDate).getMonth();
            dateLense = date + " " + month_names_short[month];
            var startTime = val.startTime.split(":");
            var endTime = val.endTime.split(":");
            let time = ((startTime.length >= 2) ? (startTime[0] + ":" + startTime[1]) : "") + " - " + ((endTime.length >= 2) ? (endTime[0] + ":" + endTime[1]) : "");
            let currentCardVal: Card = {
                id: val.id.toString(),
                description: `${val.categoryName}`,
                label: `${time}`,
                title: val.eventName
            }
            card.push(currentCardVal);
        });
        let style: Style = {
            width: 300
        }
        let lense: Lane = {
            cards: card,
            id: currentIndex.toString(),
            label: `${dateLense}`,
            style: style,
            title: `Day ${currentIndex + 1} of ${itineraryData.length}`
        }
        lenseArray.push(lense);
    });
    rootBoject.lanes = lenseArray;
    return rootBoject;
};