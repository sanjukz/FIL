export default class CountryDataViewModel {
	countries: CountryModel[];
}


export class CountryModel  { 
	altId: string;
	name: string;
	isoAlphaTwoCode: string;
	isoAlphaThreeCode: string;
	numcode: number;
	phonecode: number;
}
