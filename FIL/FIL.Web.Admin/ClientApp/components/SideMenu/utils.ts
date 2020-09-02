export interface Feature {
    id: number;
    featureName: string;
    moduleId: number;
    parentFeatureId: number;
    redirectUrl: string;
    isEnabled: boolean;
}

export interface MenuFeature extends Feature {
    childFeatures?: Feature[];
    isActiveNav?: boolean;
}

export function formatMenuItems(featureProps: Feature[], path: string): MenuFeature[] {
    let featureList: MenuFeature[] = [];
    featureProps.forEach((item: Feature) => {
        if (item.parentFeatureId == 0) {
            let tempParent: MenuFeature = { ...item };
            let childFeatures = featureProps.filter(t => t.parentFeatureId == item.id);
            let isActiveNav = childFeatures.some(t => t.redirectUrl == path);
            tempParent.childFeatures = childFeatures;
            tempParent.isActiveNav = isActiveNav;
            featureList.push(tempParent);
        }
    })
    return featureList;
}