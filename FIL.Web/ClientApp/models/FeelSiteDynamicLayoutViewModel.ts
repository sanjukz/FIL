export class FeelSiteDynamicLayoutSectionViewModel {
    public pageName: string;
    public sections: SectionView[];
}

export class PageView
{
    public id: number;
    public pageName: string;
    public isEnabled: boolean;
}

export class SectionView
{
    public id: number;
    public pageViewId: number;
    public sectionName: string;
    public componentName: string;
    public sectionHeading: string;
    public sectionSubHeading: string;
    public endpoint: string;
    public sortOrder: number;
    public isEnabled: boolean;
}