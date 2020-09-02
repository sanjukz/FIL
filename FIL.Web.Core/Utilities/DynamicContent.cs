using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Web.Core.Utilities
{
    public class DynamicContent
    {
        public class PlacePage
        {
            public class SeeAndDoCategory
            {
                public const string Title = "Feel placeName, cityName, countryName - history, tickets, souvenirs, reviews - FeelitLIVE";
                public const string Description = "Feel placeName when you visit cityName, countryName. Discover its history & legacy, buy entry tickets, shop souvenirs, explore its architecture, book ground transport, find reviews, ratings & tips.";
            }
            public class EatAndDrink
            {
                public const string Title = "Dine in, eat at or enjoy your favourite drink at placeName, when you travel to cityName, countryName - FeelitLIVE";
                public const string Description = "Dine in, eat at or enjoy your favorite drink at placeName, when you travel to cityName, countryName. Explore the atmosphere and menu at FeelitLIVE. Search the nearest location, check food prices, purchase vouchers ahead of time and book a reservation while reading reviews, no waiting in lines.";
            }
            public class ShopLocal
            {
                public const string Title = "Shop locally at placeName, while visiting cityName, countryName - buy souvenirs, certificates and vouchers online - FeelitLIVE";
                public const string Description = "Shop locally at placeName while visiting cityName, countryName. FeelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
            }
            public class ExperiencesAndActivities
            {
                public const string Title = "Feel placeName, when you visit cityName, countryName - book experiences, activities, & outdoor adventures - FeelitLIVE";
                public const string Description = "Feel placeName when you visit cityName, countryName. Book your experiences outdoor adventures, activities, & find reviews, ratings & tips.";
            }
            public class LiveStream
            {
                public const string Title = "Feel it LIVE | placeName via Online Live Stream";
                public const string Description = "Feel placeName Live on FeelitLIVE. Buy tickets, interact with host artists and influencers, find reviews and ratings.";
            }
        }

        public class GenericLocationPage
        {
            public const string Title = "Feel locationName: discover, plan, book bespoke itineraries, travel, share";
            public const string Description = "Feel locationName by first discovering it, then planning and booking your bespoke trip itinerary, immersing in the local culture and sharing and reminiscing about it.";
        }

        public class CategoryPages
        {
            public class SeeAndDo
            {
                public const string Title = "Things to See & Do around the world - history, tickets, souvenirs, architecture, reviews & ratings, tips - FeelitLIVE";
                public const string Description = "Places to see and things to do as you travel around the world. Discover their history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
                public class SubCategory
                {
                    public const string Title = "SubCategoryName around the world - history, tickets, souvenirs, architecture, culture, reviews & ratings, tips - FeelitLIVE";
                    public const string Description = "Visit SubCategoryName around the world. Discover the history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
                    public class Country
                    {
                        public const string Title = "SubCategoryName around countryName - history, tickets, souvenirs, architecture, culture, reviews & ratings, tips - FeelitLIVE";
                        public const string Description = "Visit SubCategoryName in countryName. Discover the history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
                    }
                    public class City
                    {
                        public const string Title = "SubCategoryName around  cityName, countryName - history, tickets, souvenirs, architecture, culture, reviews & ratings, tips - FeelitLIVE";
                        public const string Description = "Visit SubCategoryName in cityName, countryName. Discover the history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
                    }
                }

                public class Country
                {
                    public const string Title = "Things to See & Do around countryName - history, tickets, souvenirs, architecture, reviews &amp; ratings, tips - FeelitLIVE";
                    public const string Description = "Places to see and things to do when you visit countryName. Discover their history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
                }
                public class City
                {
                    public const string Title = "Things to See & Do around cityName, countryName - history, tickets, souvenirs, architecture, reviews & ratings, tips - FeelitLIVE";
                    public const string Description = "Places to see and things to do when you visit  cityName, countryName. Discover their history & legacy, buy entry tickets, shop souvenirs, explore their architecture, find reviews, ratings & tips.";
                }
            }

            public class EatAndDrinks
            {
                public const string Title = "Visit the best restaurants, cafes and local bars around the world- FeelitLIVE";
                public const string Description = "Visit the best restaurants, cafes, bars and eateries for local delicacies when you travel around the world. Explore menus, the atmosphere and popular items, discover the nightlife, purchase or book ahead of time while reading reviews, no waiting in lines.";
                public class SubCategory
                {
                    public const string Title = "Visit the best SubCategoryName around the world - FeelitLIVE";
                    public const string Description = "Visit the best SubCategoryName around the world to dine in, eat at or enjoy your favorite drink when you travel. Explore menus, the atmosphere and popular items, purchase or book ahead of time while reading reviews and ratings, no waiting in lines.";
                    public class Country
                    {
                        public const string Title = "Visit the best SubCategoryName in countryName - FeelitLIVE";
                        public const string Description = "Visit the best SubCategoryName in countryName to dine in, eat at or enjoy your favorite drink when you travel. Explore menus, the atmosphere and popular items, purchase or book ahead of time while reading reviews and ratings, no waiting in lines.";
                    }
                    public class City
                    {
                        public const string Title = "Visit the best SubCategoryName in cityName, countryName - FeelitLIVE";
                        public const string Description = "Visit the best SubCategoryName in cityName, countryName to dine in, eat at or enjoy your favorite drink when you travel. Explore menus, the atmosphere and popular items, purchase or book ahead of time while reading reviews and ratings, no waiting in lines.";
                    }
                }

                public class Country
                {
                    public const string Title = "Visit the best restaurants, cafes and local bars in countryName - FeelitLIVE";
                    public const string Description = "Visit the best restaurants, cafes, bars and eateries for local delicacies when you travel to countryName. Explore menus, the atmosphere and popular items, discover the nightlife, purchase or book ahead of time while reading reviews, no waiting in lines.";
                }
                public class City
                {
                    public const string Title = "Visit the best restaurants, cafes and local bars in cityName, countryName - FeelitLIVE";
                    public const string Description = "Visit the best restaurants, cafes, bars and eateries for local delicacies when you travel to cityName, countryName. Explore menus, the atmosphere and popular items, discover the nightlife, purchase or book ahead of time while reading reviews, no waiting in lines";
                }
            }
            public class ShopLocal
            {
                public const string Title = "Shop locally while traveling around the world - FeelitLIVE";
                public const string Description = "Shop locally while traveling around the world. FeelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
                public class SubCategory
                {
                    public const string Title = "Shop locally for SubCategoryName traveling around the world. - FeelitLIVE";
                    public const string Description = "Shop locally for SubCategoryName traveling around the world. FeelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
                    public class Country
                    {
                        public const string Title = "Shop locally for SubCategoryName while visiting countryName - FeelitLIVE";
                        public const string Description = " Shop locally for SubCategoryName while visiting countryName. FeelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
                    }
                    public class City
                    {
                        public const string Title = "Shop locally for SubCategoryName while visiting cityName, countryName - FeelitLIVE";
                        public const string Description = "Shop locally for SubCategoryName while visiting cityName, countryName. FeelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
                    }
                }
                public class Country
                {
                    public const string Title = "Shop locally while visiting countryName - FeelitLIVE";
                    public const string Description = "Shop locally while visiting countryName. FeelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
                }
                public class City
                {
                    public const string Title = "Shop locally while visiting cityName, countryName - FeelitLIVE";
                    public const string Description = "Shop locally while visiting cityName, countryName. FeelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
                }
            }
            public class ExperiencesAndActivities
            {
                public const string Title = "Feel the experiences, activities and adventures while travelling around the world - FeelitLIVE";
                public const string Description = "Feel the experiences, activities and adventures while travelling around the world. Book your experiences, outdoor adventures, activities, & find reviews, ratings & tips..";
                public class SubCategory
                {
                    public const string Title = "Book your SubCategoryName experiences and activities around the world - FeelitLIVE";
                    public const string Description = "Book your SubCategoryName experiences and activities around the world.Explore reviews, ratings & tips.";
                    public class Country
                    {
                        public const string Title = "Book your SubCategoryName experiences and activities in countryName - FeelitLIVE";
                        public const string Description = "Book your SubCategoryName experiences and activities in countryName. Explore reviews, ratings & tips.";
                    }
                    public class City
                    {
                        public const string Title = "Book your SubCategoryName experiences and activities in cityName, countryName - FeelitLIVE";
                        public const string Description = "Book your SubCategoryName experiences and activities in  cityName, countryName . Explore reviews, ratings & tips.";
                    }
                }
                public class Country
                {
                    public const string Title = "Feel the experiences, activities and adventures while visiting countryName - FeelitLIVE";
                    public const string Description = "Shop locally while visiting countryName. FeelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
                }
                public class City
                {
                    public const string Title = "Feel the experiences, activities and adventures while visiting cityName, countryName - FeelitLIVE";
                    public const string Description = "Shop locally while visiting cityName, countryName. FeelitLIVE makes it easy to purchase souvenirs online, gift certificates and vouchers. Discover the history, find the nearest locations while on vacation.";
                }

            }
            public class LiveStream
            {
                public const string Title = "Feel it LIVE | Online Live Stream Experiences and Events";
                public const string Description = " Feel experiences and events live online on FeelitLIVE. Buy tickets, interact with host artists and influencers, find reviews and ratings.";
                public class SubCategory
                {
                    public const string Title = "Feel SubCategoryName LIVE | Online Live Stream SubCategoryName Experiences and Events";
                    public const string Description = "Feel SubCategoryName Live on FeelitLIVE. Buy tickets, interact with host artists and influencers, find reviews and ratings.";
                }
            }
        }

        //For Live Online Pages
        public class LiveOnline
        {
            public const string Title = "Host Live Stream Experiences and Events | Get Paid | FeelitLIVE";
            public const string Description = "Host live stream experiences and events right from your home or business location and get paid with FeelitLIVE. Get started by creating an experience or event, schedule and interact and engage with your audience.";
        }
    }
}
