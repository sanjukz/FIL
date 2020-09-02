import { EventTabs } from "../Enums/EventTabs";

export const getActiveTab = (pathName: string) => {
  for (var tab in EventTabs) {
    let tabDetail = EventTabs[tab] as any;
    if (tabDetail.url && pathName.indexOf(tabDetail.url) > -1) {
      return tabDetail.id;
    }
  }
}
