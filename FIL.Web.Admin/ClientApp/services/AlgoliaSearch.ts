import axios from "axios";
import * as algoliasearch from "algoliasearch";

const searchClient = algoliasearch(                            // intializing alogolia client
    '8AMLTQYRXE',                                               //ToDo Set this keys in environmental variable
    '122a9a1f411959c3cd46db326c56334a'
);
const index = searchClient.initIndex('Places');

export class AlgoliaResult {
    results: any;
    success: boolean
}

export const getAlgoliaResults = async (keyword: string, callback: any) => {
    index
        .search(keyword)
        .then(({ hits }) => {
            let algoliaResult: AlgoliaResult = {
                results: hits,
                success: true
            }
            return callback(algoliaResult);
        })
        .catch(err => {
            let algoliaResult: AlgoliaResult = {
                results: [],
                success: false
            }
            return callback(algoliaResult);
        });
}

export const AlgoliaSearchService = {
    getAlgoliaResults
};
