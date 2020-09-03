import * as React from 'react';
import {
    Link, RouteComponentProps
} from "react-router-dom";
import * as AccountStore from "../../../stores/Account";
import FilLoader from "../../../components/Loader/FilLoader";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import EventImageV1 from '../../../components/Home/EventImageV1';
import EventPriceV1 from '../../../components/Home/EventPriceV1';
import EventRatingsV1 from '../../../components/Home/EventRatingV1';
import BreadcrumbAndTitle from '../BreadcrumbAndTitle';

interface IProps {
    gets3BaseUrl: string;
}
type AccountProps = IProps & AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;

function FeelList(props: AccountProps) {
    const [feelLists, SetFeelLists] = React.useState([]);
    const [isLoading, SetLoading] = React.useState(true);

    React.useEffect(() => {
        let wishlistData = [];
        let data = localStorage.getItem("bespokeItems");
        if (data != null && data != '0') {
            let filteredData = JSON.parse(data).filter(val => val.userAltId == props.session.user.altId);
            filteredData.forEach(val => {
                let isPresent = wishlistData.filter((item) => {
                    return item.altId == val.cartItems.altId
                })
                if (isPresent.length == 0) {
                    wishlistData.push(val.cartItems);
                }
            });
        }

        SetFeelLists(wishlistData);
        SetLoading(false);
    }, []);

    const { gets3BaseUrl } = props;


    const GetRightSideBanner = () => {
        return <div className="col-sm-4 mt-4 mt-sm-0 pl-md-5">
            <div className="border p-4">
                <img
                    src={`${gets3BaseUrl}/my-account/right-bar-images/feellist.svg`}
                    className="img-fluid mb-4"
                    alt=""
                />
                <div>
                    <h5 className="mb-3">What is the FeelList?</h5>
                    <p className="m-0">
                        The FeelList is the easiest way to keep track of all the events and experience you are eyeing. Simply,
                        click on the 'purple bug' in the experience listing to add it to the list.
                </p>
                </div>
            </div>
        </div>
    }

    const removeFromFeelList = (item) => {
        let filteredLists = feelLists.filter((val) => {
            return item.altId != val.altId
        });

        let allWishlistexceptCurrent;
        if (
            localStorage.getItem("bespokeItems") !== null &&
            localStorage.getItem("bespokeItems") != "0"
        ) {
            allWishlistexceptCurrent = JSON.parse(
                localStorage.getItem("bespokeItems")
            ).filter(function (val) {
                return (
                    (val.userAltId !== props.session.user &&
                        props.session.user.altId) && val.cartItems.altId != item.altId
                );
            });
        }
        var data = [];
        if (allWishlistexceptCurrent.length > 0) {
            allWishlistexceptCurrent.forEach(function (val) {
                data.push(val);
            });
            localStorage.setItem("bespokeItems", JSON.stringify(data));
            SetFeelLists(filteredLists);
        }
    }
    const checkVenue = (item) => {
        return (
            <div className="iconlink">
                <Link
                    className="text-dark"
                    to={"https://www.google.com/maps/search/" + item.name}
                    target="_blank"
                >
                    {item.cityName + ", " + item.countryName}
                </Link>
            </div>
        );
    }
    if (isLoading) {
        return <FilLoader />
    }
    else {
        if (!feelLists.length) {
            return (
                <div className="container">
                    <BreadcrumbAndTitle title={'FeelList'} />
                    <div className="row">
                        <div className="col-sm-8">
                            <h5 className="mt-2 text-center">You have no recent Feels</h5>
                        </div>
                        {GetRightSideBanner()}
                    </div>
                </div>);
        }
        return <div className="container">
            <BreadcrumbAndTitle title={'FeelList'} />
            <div className="fil-site">
                <section className="fil-tiles-sec pt-0">
                    <div className="container">
                        <div className="row">
                            <div className="col-sm-8">
                                <div className="row">
                                    {feelLists.map((item) => {
                                        return <div className="col-sm-4 mb-4">
                                            <div className="card fil-tils border-0 h-100">
                                                <a href="javascript:void(0)" className="feellist-tag">
                                                    <img src={`${gets3BaseUrl}/icons/live-tag.svg`} alt="Add to your feelList"
                                                        onClick={() => removeFromFeelList(item)}
                                                    />
                                                </a>
                                                <a href={`/place/${item.parentCategorySlug} /${item.slug}/${item.subCategorySlug}`}>
                                                    <EventImageV1 altId={item.altId}
                                                        parentCategory={item.parentCategory} category={item.category}
                                                        key={item.altId} />
                                                </a>
                                                <div className="card-body px-0 pb-0">
                                                    <a href={`/place/${item.parentCategorySlug} /${item.slug}/${item.subCategorySlug}`}
                                                        className="tils-title m-0 text-body">
                                                        {item.name}
                                                    </a>
                                                    {!item.duration && checkVenue(item)}

                                                    {<EventPriceV1
                                                        currency={item.currency}
                                                        currentItem={item}
                                                        minPrice={item.minPrice}
                                                        maxPrice={item.maxPrice}
                                                        duration={item.duration}
                                                    />}

                                                    <div className="fil-tils-tag">
                                                        <span>{item.category}</span>
                                                    </div>

                                                    {!item.duration && <EventRatingsV1 rating={[]} eventAltId={item.altId} />}

                                                </div>
                                            </div>
                                        </div>
                                    })}
                                </div>
                            </div>
                            {GetRightSideBanner()}
                        </div>

                    </div>
                </section>
            </div >
        </div>
    }
}
export default FeelList;
