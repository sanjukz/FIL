import * as React from "React";
import StandardFeelCard from "../../views/Comman/StandardFeelCard";

export default class BespokeItinerary extends React.Component<any, any> {
    public constructor(props) {
        super(props);
        this.state = { bespokeData: "" };
    }

    public componentDidMount() {
        let data = localStorage.getItem("bespokeItems");

        if (data != null && data != '0') {
            let filteredData = JSON.parse(data).filter(val => val.userAltId == this.props.session.user && this.props.session.user.altId);
            var wishlistData = [];
            filteredData.forEach(val => {
                wishlistData.push(val.cartItems)
            });

            this.loadBespokeItems(wishlistData);
            this.setState({
                bespokeData: wishlistData
            });
        }
    }

    public loadBespokeItems(data) {
        if (data != "") {
            this.setState({ bespokeData: data });
        }
        else {
            this.setState({ bespokeData: "" });
        }
    }

    public render() {
        var data = this.state.bespokeData;
        if (data != "") {
            return <div className="pt-4">
                <h4>
                    Your feelList
                </h4>
                <section className="recommended-feels one-col pb-5 mb-5">
                    <div className="container p-0">
                        <div id="RecommendedFeels">
                            <div className="nav-tab-content">
                                <StandardFeelCard
                                    display={true}
                                    placeCardsData={data}
                                    session={this.props.session}
                                    isWishList={true}
                                />
                            </div>
                        </div>
                    </div>
                </section>
                <div className="clearfix"></div>
            </div>;
        } else {
            return <div>
            </div>;
        }
    }
}
