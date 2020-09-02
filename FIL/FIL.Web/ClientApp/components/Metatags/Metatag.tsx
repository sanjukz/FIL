import * as React from "react";
import { Helmet } from "react-helmet";

export default class Metatag extends React.Component<any, any> {
  public constructor(props) {
    super(props);
    this.state = {
      currentPath: "",
    };
  }

  public componentDidMount() {
    var path = (window as any).location.href;
    this.setState({
      currentPath: path,
    });
  }
  public render() {
    var eventUrl = this.props.url || "",
      path_name = "",
      homePage = false;
    var metaContent = this.props.metaContent;
    var originUrl = "https://www.feelitlive.com";
    if (typeof window !== "undefined") {
      originUrl = window.location.origin;
    }
    var categoryName = this.props.categoryName;
    var subCategoryName =
      metaContent &&
      metaContent.eventCategory
        .toLowerCase()
        .split(" ")
        .join("-")
        .replace("&", "and");
    var metaDescription =
      "feelitLIVE  by first discovering it, then planning and booking your bespoke trip itinerary, immersing in the local culture and sharing and reminiscing about it.";
    var metaKeywords =
      "feel, places, visit, itineraries, itinerary, plan a trip, holiday, vacation, travel, tourism, monuments, museums, attractions, parks and sanctuaries, wildlife, castles, forts, galleries, spiritual, temples, see and do, eat and drink, shop, experience, explore, rides, monument tickets, museum tickets, attraction tickets, audio guides, light and sound show, local delicacies, cafes, nightlife, circuits, souvenirs, memorabilia, hidden gems, hotels, hotel, maps, map, restaurants, beaches, shopping, flights, resorts, tourist places, flight tickets, resort, air ticket, tours and travels, honeymoon packages, tour, holidays, tour packages, holiday packages, review, sightseeing, airfare, travel package, trip, holiday packages, reviews, tours, trips, holiday, hill stations, local traditions, cruises, private taxi, b&b, bed and breakfast, local hosts, train, royal trains";
    var title =
      "feelitLIVE : discover, plan, book bespoke itineraries, travel, share";
    var canonicalLink = "https://www.feelitlive.com";
    let current_link = "https://www.feelitlive.com/";
    var curURL = "";
    if (typeof window !== "undefined") {
      curURL = (window as any).location.origin;
      canonicalLink = (window as any).location.href;
      if (
        canonicalLink.substring(
          canonicalLink.length - 1,
          canonicalLink.length
        ) == "/"
      ) {
        canonicalLink = canonicalLink.substring(0, canonicalLink.length - 1);
      }
      current_link =
        window.location.origin +
        window.location.pathname +
        window.location.search;
      path_name =
        window.location.pathname.substring(1) + window.location.search;
      homePage = window.location.pathname == "/" ? true : false;
    }
    if (curURL.indexOf("feelitlive") < 0) {
      var siteNameSplit = curURL.split(".");
      if (siteNameSplit.length > 2) {
        var siteName = siteNameSplit[1].replace("feel", "");
        siteName =
          siteName.substring(0, 1).toUpperCase() +
          siteName.substring(1, siteName.length);
        title =
          "Feel " +
          siteName +
          ": discover, plan, book bespoke itineraries, travel, share";
        metaDescription =
          "Feel " +
          siteName +
          " by first discovering it, then planning and booking your bespoke trip itinerary, immersing in the local culture and sharing and reminiscing about it.";
      }
    }
    if (canonicalLink.indexOf("country") > -1) {
      // For City/Country/State
      if (this.props.countryName && this.props.countryName != "") {
        title =
          "Feel " +
          this.props.countryName +
          ": discover, plan, book bespoke itineraries, travel, share";
        metaDescription =
          "Feel " +
          this.props.countryName +
          " by first discovering it, then planning and booking your bespoke trip itinerary, immersing in the local culture and sharing and reminiscing about it.";
      }
    }
    //For Category Landing Pages
    if (
      this.props.isCategoryLandingPage &&
      this.props.isCategoryLandingPage != null &&
      this.props.generic_subcategory_name == null
    ) {
      if (this.props.generic_category == "See & Do") {
        if (this.props.generic_location_name != null) {
          let location_name =
            this.props.generic_countryName != null
              ? this.props.generic_location_name +
                ", " +
                this.props.generic_countryName
              : this.props.generic_location_name;
          title =
            "Things to See & Do around " +
            location_name +
            " - history, tickets, souvenirs, architecture, reviews & ratings, tips - feelitLIVE";
          metaDescription =
            "Places to see and things to do when you visit " +
            location_name +
            ". Discover their history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
        } else {
          title =
            "Things to See & Do around the world - history, tickets, souvenirs, architecture, reviews & ratings, tips - feelitLIVE";
          metaDescription =
            "Places to see and things to do as you travel around the world. Discover their history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
        }
      }
      if (this.props.generic_category == "Eat & Drink") {
        if (this.props.generic_location_name != null) {
          let location_name =
            this.props.generic_countryName != null
              ? this.props.generic_location_name +
                ", " +
                this.props.generic_countryName
              : this.props.generic_location_name;
          title =
            "Visit the best restaurants, cafes and local bars in " +
            location_name +
            " - feelitLIVE";
          metaDescription =
            "Visit the best restaurants, cafes, bars and eateries for local delicacies when you travel to " +
            location_name +
            ". Explore menus, the atmosphere and popular items, discover the nightlife, purchase or book ahead of time while reading reviews, no waiting in lines.";
        } else {
          title =
            "Visit the best restaurants, cafes and local bars around the world- feelitLIVE";
          metaDescription =
            "Visit the best restaurants, cafes, bars and eateries for local delicacies when you travel around the world. Explore menus, the atmosphere and popular items, discover the nightlife, purchase or book ahead of time while reading reviews, no waiting in lines.";
        }
      }
      if (this.props.generic_category == "Shop Local") {
        if (this.props.generic_location_name != null) {
          let location_name =
            this.props.generic_countryName != null
              ? this.props.generic_location_name +
                ", " +
                this.props.generic_countryName
              : this.props.generic_location_name;
          title =
            "Shop locally while visiting " + location_name + " - feelitLIVE";
          metaDescription =
            "Shop locally while visiting " +
            location_name +
            ". feelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
        } else {
          title = "Shop locally while travelling around the world - feelitLIVE";
          metaDescription =
            "Shop locally while travelling around the world. feelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
        }
      }
      if (this.props.generic_category == "Experiences & Activities") {
        if (this.props.generic_location_name != null) {
          let location_name =
            this.props.generic_countryName != null
              ? this.props.generic_location_name +
                ", " +
                this.props.generic_countryName
              : this.props.generic_location_name;
          title =
            "Feel the experiences, activities and adventures while visiting " +
            location_name +
            " - feelitLIVE";
          metaDescription =
            "Shop locally while visiting " +
            location_name +
            ". feelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
        } else {
          title =
            "Feel the experiences, activities and adventures while travelling around the world - feelitLIVE";
          metaDescription =
            "Feel the experiences, activities and adventures while travelling around the world. Book your experiences, outdoor adventures, activities, & find reviews, ratings & tips.";
        }
      }
    }
    // for sub-category landing pages
    if (
      this.props.generic_subcategory_name &&
      this.props.generic_subcategory_name != null
    ) {
      if (this.props.generic_category == "See & Do") {
        if (this.props.generic_location_name != null) {
          let location_name =
            this.props.generic_countryName != null
              ? this.props.generic_location_name +
                ", " +
                this.props.generic_countryName
              : this.props.generic_location_name;
          title =
            this.props.generic_subcategory_name +
            " around " +
            location_name +
            " - history, tickets, souvenirs, architecture, culture, reviews & ratings, tips - feelitLIVE";
          metaDescription =
            "Visit " +
            this.props.generic_subcategory_name +
            " in " +
            location_name +
            ". Discover the history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
        } else {
          title =
            this.props.generic_subcategory_name +
            " around the world - history, tickets, souvenirs, architecture, culture, reviews & ratings, tips - feelitLIVE";
          metaDescription =
            "Visit " +
            this.props.generic_subcategory_name +
            " around the world. Discover the history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
        }
      }
      if (this.props.generic_category == "Eat & Drink") {
        if (this.props.generic_location_name != null) {
          let location_name =
            this.props.generic_countryName != null
              ? this.props.generic_location_name +
                ", " +
                this.props.generic_countryName
              : this.props.generic_location_name;
          title =
            "Visit the best " +
            this.props.generic_subcategory_name +
            " in " +
            location_name +
            "  - feelitLIVE";
          metaDescription =
            " Visit the best " +
            this.props.generic_subcategory_name +
            " in " +
            location_name +
            " to dine in, eat at or enjoy your favourite drink when you travel. Explore menus, the atmosphere and popular items, purchase or book ahead of time while reading reviews and ratings, no waiting in lines.";
        } else {
          title =
            "Visit the best " +
            this.props.generic_subcategory_name +
            " around the world - feelitLIVE";
          metaDescription =
            " Visit the best " +
            this.props.generic_subcategory_name +
            " around the world to dine in, eat at or enjoy your favourite drink when you travel. Explore menus, the atmosphere and popular items, purchase or book ahead of time while reading reviews and ratings, no waiting in lines.";
        }
      }
      if (this.props.generic_category == "Shop Local") {
        if (this.props.generic_location_name != null) {
          let location_name =
            this.props.generic_countryName != null
              ? this.props.generic_location_name +
                ", " +
                this.props.generic_countryName
              : this.props.generic_location_name;
          title =
            "Shop locally for " +
            this.props.generic_subcategory_name +
            " while visiting  " +
            location_name +
            " - feelitLIVE";
          metaDescription =
            "Shop locally for " +
            this.props.generic_subcategory_name +
            " while visiting " +
            location_name +
            ". feelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
        } else {
          title =
            "Shop locally for " +
            this.props.generic_subcategory_name +
            " travelling around the world. - feelitLIVE";
          metaDescription =
            "Shop locally for " +
            this.props.generic_subcategory_name +
            " travelling around the world. feelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
        }
      }
      if (this.props.generic_category == "Experiences & Activities") {
        if (this.props.generic_location_name != null) {
          let location_name =
            this.props.generic_countryName != null
              ? this.props.generic_location_name +
                ", " +
                this.props.generic_countryName
              : this.props.generic_location_name;
          title =
            "Book your " +
            this.props.generic_subcategory_name +
            " experiences and activities in " +
            location_name +
            " - feelitLIVE";
          metaDescription =
            "Book your " +
            this.props.generic_subcategory_name +
            " experiences and activities in " +
            location_name +
            ". Explore reviews, ratings & tips.";
        } else {
          title =
            "Book your " +
            this.props.generic_subcategory_name +
            " experiences and activities around the world - feelitLIVE";
          metaDescription =
            "Book your " +
            this.props.generic_subcategory_name +
            " experiences and activities around the world. Explore reviews, ratings & tips.";
        }
      }
    }
    var placeType;
    let isplacePage = false;
    if (metaContent != "" && metaContent != null && eventUrl != null) {
      isplacePage = true;
      if (metaContent == null) {
        metaDescription = metaDescription;
        metaKeywords = metaKeywords;
      } else {
        metaKeywords = "";
        if (
          metaContent.event.metaDetails != "" &&
          metaContent.event.metaDetails != null
        ) {
          var data = metaContent.event.metaDetails.split("<br/>");
          var descTag;
          var titleTag;
          if (data[0] != "" && data[0] != null) {
            titleTag = data[0].trim();
            title = titleTag.slice(7, -8);
          }
          if (data[1] != "" && data[1] != null) {
            descTag = data[1].trim();
            metaDescription = descTag.slice(34, -2);

            var html = metaDescription;
            var div = document.createElement("div");
            div.innerHTML = html;
            var nonHtmlTagsString = div.innerText;
            var removedNewLines = nonHtmlTagsString.replace(/\n|\r/g, "");
            metaDescription = removedNewLines.replace(/"/g, "'");
          }
          if (data[2] != "" && data[2] != null) {
            placeType = data[2].trim();
          }
        } else {
          let flag = true;
          if (categoryName == "Eat & Drink") {
            title =
              "Dine in, eat at or enjoy your favourite drink at " +
              this.props.title +
              ", when you travel to " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              " - feelitLIVE";
            metaDescription =
              "Dine in, eat at or enjoy your favourite drink at " +
              this.props.title +
              ", when you travel to " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              ". . Explore the atmosphere and menu at feelitLIVE. Search the nearest location, check food prices, purchase vouchers ahead of time and book a reservation while reading reviews, no waiting in lines.";
            flag = false;
          }
          if (categoryName == "Shop Local") {
            title =
              "Shop locally at " +
              metaContent.event.name +
              ",  while visiting " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              " - buy souvenirs, certificates and vouchers online - feelitLIVE";
            metaDescription =
              "Shop locally at " +
              metaContent.event.name +
              ", " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              ". feelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
            flag = false;
            flag = false;
          }
          if (categoryName == "Experiences & Activities") {
            title =
              " Feel " +
              metaContent.event.name +
              " when you visit " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              "- book experiences, activities, & outdoor adventures - feelitLIVE";
            metaDescription =
              "Feel " +
              metaContent.event.name +
              ", " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              ". Book your experiences outdoor adventures, activities, & find reviews, ratings & tips.";
            flag = false;
          } else {
            if (flag) {
              metaDescription =
                "Feel " +
                metaContent.event.name +
                ". Discover its history & legacy, buy entry tickets, shop souvenirs, explore its architecture, book ground transport, find reviews, ratings & tips.";
              if (curURL.indexOf("com/place") > -1) {
                title =
                  "Feel " +
                  metaContent.event.name +
                  ", " +
                  metaContent.city.name +
                  ", " +
                  metaContent.country.name +
                  " - history, tickets, souvenirs, reviews - feelitLIVE";
              } else {
                title =
                  "Feel " +
                  metaContent.event.name +
                  " - history, tickets, souvenirs, reviews - feelitLIVE";
              }
            }
          }
        }

        if (metaDescription == "") {
          metaDescription =
            "feelitLIVE  by first discovering it, then planning and booking your bespoke trip itinerary, immersing in the local culture and sharing and reminiscing about it.";
        }

        if (title != null && title != "") {
          title = title;
        } else if (this.props.title != null) {
          title = this.props.title;
          if (categoryName == "See & Do") {
            // for see & do category place page
            title =
              "Feel " +
              this.props.title +
              ", " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              " - history, tickets, souvenirs, reviews - feelitLIVE";
            metaDescription =
              "Feel " +
              this.props.title +
              " when you visit " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              ". Discover its history & legacy, buy entry tickets, shop souvenirs, explore its architecture, book ground transport, find reviews, ratings & tips.";
          }
          if (categoryName == "Shop Local") {
            title =
              "Shop locally at " +
              metaContent.event.name +
              ",  while visiting " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              " - buy souvenirs, certificates and vouchers online - feelitLIVE";
            metaDescription =
              "Shop locally at " +
              metaContent.event.name +
              ", " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              ". feelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
          }
          if (categoryName == "Experiences & Activities") {
            title =
              " Feel " +
              metaContent.event.name +
              " when you visit " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              "- book experiences, activities, & outdoor adventures - feelitLIVE";
            metaDescription =
              "Feel " +
              metaContent.event.name +
              ", " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              ". Book your experiences outdoor adventures, activities, & find reviews, ratings & tips.";
          }
          if (categoryName == "Eat & Drink") {
            title =
              "Dine in, eat at or enjoy your favourite drink at " +
              this.props.title +
              ", when you travel to " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              " - feelitLIVE";
            metaDescription =
              "Dine in, eat at or enjoy your favourite drink at " +
              this.props.title +
              ", when you travel to " +
              metaContent.city.name +
              ", " +
              metaContent.country.name +
              ". . Explore the atmosphere and menu at feelitLIVE. Search the nearest location, check food prices, purchase vouchers ahead of time and book a reservation while reading reviews, no waiting in lines.";
          }
        } else {
          title =
            "feelitLIVE : discover, plan, book bespoke itineraries, travel, share";
        }
        path_name = eventUrl.substring(1, eventUrl.length - 1);
        canonicalLink = "https://www.feelitlive.com/" + path_name;
      }
    }

    return (
      <>
        <Helmet>
          {/* <meta charSet="utf-8" />
                <title>{title}</title>
                <meta name="description" content={metaDescription} />
                <meta name="keywords" content={metaKeywords} />
                <meta name="og:title" content={title} />
                <meta name="og:description" content={metaDescription} />

                {((this.state.currentPath.indexOf("coming-soon") > -1)
                    || (this.state.currentPath.indexOf("login") > -1) || (this.state.currentPath.indexOf("signup") > -1))
                    || (this.state.currentPath.indexOf("beta.feelaplace") > -1) || (this.state.currentPath.indexOf("dev.feelaplace") > -1)
                    && <meta name="robots" content="noindex, nofollow" />}
                {isplacePage && (!this.props.isDescriptionAvailable) && <meta name="robots" content="noindex, nofollow" />}
                {isplacePage && (metaKeywords != null && metaKeywords != "") && <meta name="keywords" content={metaKeywords} />} */}

          {/* <script type="application/ld+json">
                    {`
										{
											"@context": "https://schema.org",
											"@type": "website",
											"url": "${originUrl}",
                                            "logo": "${gets3BaseUrl()}/logos/feel-aplace.png"								
                                        	}
									`}
                </script> */}
          {/* {isplacePage && metaContent != null && eventUrl.indexOf("/place") > -1 ?
                    <script type="application/ld+json">{`
							   {
									"@context": "http://www.schema.org",
									"@type": "Product",
									"name": "${metaContent.event.name}",
									"startDate": "${metaContent.eventDetail.startDateTime}",
									"url": "${originUrl}/${eventUrl}",
									"logo": "${gets3BaseUrl()}/logos/feel-aplace.png",
									"Description": "${metaDescription}",
									"Location":{
										"@type": "Place",
										"name": "PostalAddress",
										"address": "${metaContent.venue.name}, ${metaContent.city.name}, ${metaContent.state.name}",
										"addressLocality": "${metaContent.city.name}",
										"addressCountry": "${metaContent.country.name}"
									},
								}
							`}
                    </script>
                    : ""} */}
          {/*   search bar markup for google search */}
          {/* {(isplacePage || homePage) &&
                    <script type="application/ld+json">
                        {`
										{
											"@context": "https://schema.org",
											"@type": "WebSite",
											"url": "${originUrl}",
											"potentialAction": {
												"@type": "SearchAction",
												"target": "${originUrl}/advance-search/{search_term_string}",
												"query-input": "required name=search_term_string"
											}		
										}
									`}
                    </script>} */}
          {/* breadcrumb for feel */}
          {/* {isplacePage &&
                    <script type="application/ld+json">
                        {`
                                       {
                                           "@context": "https://schema.org",
                                           "@type": "BreadcrumbList",
                                           "itemListElement": [{
                                             "@type": "ListItem",
                                             "position": 1,
                                             "name": "Home",
                                             "item": "${originUrl}"
                                           },{
                                             "@type": "ListItem",
                                             "position": 2,
                                             "name": "${categoryName}",
                                             "item": "${originUrl}/c/${metaContent.category.slug}?category=${metaContent.category.id}"
                                           },{
                                               "@type": "ListItem",
                                               "position": 3,
                                               "name": "${subCategoryName}",
                                               "item":"${originUrl}/c/${metaContent.category.slug}/${metaContent.subCategory.slug}?category=${metaContent.category.id}subcategory=${metaContent.subCategory.id}"
                                             }]
                                          }
                                   `}
                    </script>
                } */}
          {/* For Alternate Tags */}
          {/* <link rel="alternate" href={`https://www.feelitlive.com/${path_name}`} hrefLang="en" />
                <link rel="alternate" href={`https://www.feelaplace.co.in/${path_name}`} hrefLang="en-in" />
                <link rel="alternate" href={`https://www.feelitlive.com.au/${path_name}`} hrefLang="en-AU" />
                <link rel="alternate" href={`https://www.feelaplace.es/${path_name}`} hrefLang="en-ES" />
                <link rel="alternate" href={`https://www.feelaplace.fr/${path_name}`} hrefLang="en-FR" />
                <link rel="alternate" href={`https://www.feelaplace.co.nz/${path_name}`} hrefLang="en-NZ" />
                <link rel="alternate" href={`https://www.feelaplace.co.uk/${path_name}`} hrefLang="en-GB" />
                <link rel="alternate" href={`https://www.feelaplace.de/${path_name}`} hrefLang="en-DE" /> */}

          {/* For Canonical Tags */}
          {/* <link rel="canonical" href={current_link} />

                {(canonicalLink.indexOf("feelitlive.com") > -1) && <meta name="google-site-verification" content="tfcY2FbPh50UCy_0xvzGwARPpKzqqv5Uwp3M-cySK1o" />}
                {(canonicalLink.indexOf("feelaplace.co.uk") > -1) && <meta name="google-site-verification" content="802aFB41Xsch1PxoaCk5-JTX4HczUk-aOUez08L3CfU" />} */}
        </Helmet>
      </>
    );
  }
}
