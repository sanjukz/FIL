class Venue {
    public id: number;
    public altId: string;
    public name: string;
    public addressLineOne: string;
    public addressLineTwo: string;
    public cityId: number;
    public latitude: string;
    public longitude: string;
    public hasImages: boolean;
    public prefix: string;
}

class VenueWithCityName extends Venue {
    public city: string;
}

export class FeelNearbyQueryResult {
    public latitude: number;
    public longitude: number;
    public nearbyPlaces: VenueWithCityName[];
}
