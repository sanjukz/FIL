export default class CurrencyType {
  id: number;
  code: string;
  name: string;
  countryId: number;
  exchangeRate?: number;
  taxes?: number;
}
