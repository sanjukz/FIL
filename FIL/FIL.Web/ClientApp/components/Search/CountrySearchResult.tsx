import * as React from "react";
import { Link } from "react-router-dom";
import { gets3BaseUrl } from "../../utils/imageCdn";

export default class CountrySearchResult extends React.Component<any, any> {
  state = {
    s3BaseUrl: gets3BaseUrl()
  };
  public render() {
    let searchResult = [];
    let that = this;
    if (
      (this.props.algoliaResults && this.props.algoliaResults.length > 0) ||
      this.props.searchText.trim().length > 0
    ) {
      if (this.props.algoliaResults.length > 0) {
        let get_distinct_countries = this.GetDistinctCountries(
          this.props.algoliaResults
        );
        searchResult = get_distinct_countries.map(val => (
          <a
            key={val}
            href={`/country/${val.countryName
              .split(/[\d .-]+/)
              .join("")
              .toLowerCase()}?country=${val.countryId}`}
            className="btn btn-sm btn-outline-secondary mr-1 ml-1 mb-1"
          >
            <span className="d-inline-block">{val.countryName}</span>
            <img
              src={`${that.state.s3BaseUrl}/flags/flag-${
                val.countryName.indexOf(".") != -1
                  ? val.countryName
                    .split(".")
                    .join("")
                    .toUpperCase()
                  : val.countryName
                    .split(" ")
                    .join("-")
                    .toLowerCase()
                }.jpg`}
              alt="feel country flag"
              width="25"
              className="ml-2"
            />
          </a>
        ));
      } else {
        return null;
      }
    } else {
      if (this.props.searchSuccess) {
        // searchResult = this.props.searchResult.map((val, index) => (
        //   <a
        //     key={index}
        //     href={`/country/${val.countryName
        //       .split(/[\d .-]+/)
        //       .join("")
        //       .toLowerCase()}?country=${val.countryId}`}
        //     className="btn btn-sm btn-link btn-block border-0 text-left text-secondary"
        //   >
        //     <span className="d-inline-block">{val.countryName}</span>
        //     <img
        //       src={`${this.state.s3BaseUrl}/flags/flag-${
        //         val.countryName.indexOf(".") != -1
        //           ? val.countryName
        //             .split(".")
        //             .join("")
        //             .toUpperCase()
        //           : val.countryName
        //             .split(" ")
        //             .join("-")
        //             .toLowerCase()
        //         }.jpg`}
        //       alt="feel country flag"
        //       width="25"
        //       className="ml-2"
        //     />
        //   </a>
        // ));
      } else if (
        this.props.defaultCountries &&
        this.props.defaultCountries.length
      ) {
        var defaultCountries = this.props.defaultCountries;
        defaultCountries = defaultCountries.filter(val => {
          return val.name != "";
        });
        defaultCountries.sort((a, b) => {
          var nameA = a.name.toUpperCase();
          var nameB = b.name.toUpperCase();
          if (nameA < nameB) {
            return -1;
          }
          if (nameA > nameB) {
            return 1;
          }
          return 0;
        });
        // searchResult = defaultCountries.map((val, index) => {
        //   return (
        //     <a
        //       key={index}
        //       className="btn btn-sm btn-outline-secondary mr-1 ml-1 mb-2"
        //       href={`/country/${val.name
        //         .split(/[\d .-]+/)
        //         .join("")
        //         .toLowerCase()}?country=${val.id}`}
        //     >
        //       {val.name}
        //     </a>
        //   );
        // });
      }
    }
    return searchResult.length ? (
      <div className="border-bottom my-3 pb-3 px-3">
        <h6 className="small">Country</h6>
        {searchResult}
        {/* <hr className="my-2" /> */}
      </div>
    ) : null;
  }
  GetDistinctCountries(algoliaResults) {
    let distinct_countries = [],
      filteredCountries = [];
    algoliaResults.map(item => {
      if (distinct_countries.indexOf(item.country) == -1 && item.category != "Live Stream") {
        distinct_countries.push(item.country);
      }
    });
    if (distinct_countries.length > 0) {
      distinct_countries.map(item => {
        let countryId = algoliaResults.filter(val => {
          return val.country == item;
        });
        let data = {
          countryId: countryId[0].countryId,
          countryName: countryId[0].country
        };
        filteredCountries.push(data);
      });
    }
    return filteredCountries;
  }
}
