import * as React from "react";
import { gets3BaseUrl } from "../../utils/imageCdn";
import "../../scss/site.scss";
import "../../scss/_footer.scss";
import "../../scss/static-pages.scss";

const PrivacyPolicy: React.FunctionComponent<any> = () => {
    React.useEffect(() => {
        if (window) {
            window.scrollTo(0, 0)
        }
    })
    return (
        <>
            <div className="inner-banner photogallery">
                <img src={`${gets3BaseUrl()}/header/TermsOfUse.jpg`} alt="Visit Taj Mahal" className="card-img" />
                <div className="card-img-overlay">
                    <nav aria-label="breadcrumb">
                        <ol className="breadcrumb bg-white m-0 p-1 pl-2 d-inline-block">
                            <li className="breadcrumb-item"><a href="#">Home</a></li>
                            <li className="breadcrumb-item active" aria-current="page">Privacy Policy</li>
                        </ol>
                    </nav>
                </div>
            </div>
            <div className="pt-2 pb-2 container page-content">
                <h1 className="h3">Privacy Policy</h1>
                <hr />
                <p><strong>Introduction</strong></p>
                <p>Welcome to feelitLIVE.com! You are very important to us and we really respect your privacy and data. This policy is therefore meant to help you understand as to how and why feelitLIVE.com and all its associated websites gather and use the information you provide when you interact with our website/s and app, and your associated rights. </p>

                <div className="accordion custum-accordian pb-3" id="accordionExample">
                    <div className="card">
                        <div className="card-header" id="headingOne">
                            <h5 className="mb-0">
                                <button className="btn btn-link" type="button" data-toggle="collapse" data-target="#whatwecollect" aria-expanded="true" aria-controls="whatwecollect">
                                    What we collect <span className="collapsed"><p className="accord_arrow"><b><i className="fa fa-angle-down" aria-hidden="true"></i></b></p></span> <span className="expanded"><p className="accord_arrow"><b><i className="fa fa-angle-up" aria-hidden="true"></i></b></p></span>
                                </button>
                            </h5>
                        </div>

                        <div id="whatwecollect" className="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample">
                            <div className="card-body">
                                <p>Please be assured that we collect information that helps us provide our services to you in a complete and secure manner. The data listed below is collected at the time of your interaction with us, either via our website/s or app/s or our customer service channels.</p>
                                <ul>
                                    <li>
                                        <b>Contact data</b>: Your full name, email address, mobile/contact number, mailing address, social media login details only if that is used by you to login or share event information on your social profiles or connect with our customer service channels
					</li>
                                    <li>
                                        <b>Device and technical data</b>: Device and general IP address, location, browsing patterns and history when using our website or app. This information is useful for us as ithelps us get a better understanding of how you’re using our websites and services so that we can  continue to provide the best experience possible
					</li>
                                    <li>
                                        <b>Identification data</b>: As recorded in a govt issued identification document
					</li>
                                    <li>
                                        <b>Preference data</b>: Preferences and choices of types of sites and experience. This is only to the extent you would like us to assist with your visit and help you customize your experience
					</li>
                                    <li>
                                        <b>Customer service interaction data</b>: Such data includes calls made to our customer service center, emails you send with queries or issue resolution, online chats, reaching out to us via any social channel such as Facebook, Instagram, Twitter, are stored in our CRM system for training and quality control purposes
					</li>
                                    <li>
                                        <b>Financial data</b>: credit/debit card /netbanking or account information only as a passthrough to the banking sites to complete the transaction, per mandated 3D secure guidelines
					</li>
                                    <li>
                                        <b>Third party data</b>: Information obtained from third parties such as fraud prevention services and tools to verify information provided by you to help us identify suspicious and fraudulent transactions
					</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div className="card">
                        <div className="card-header" id="headingTwo">
                            <h5 className="mb-0">
                                <button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#ACCESSTOTHESITE" aria-expanded="false" aria-controls="ACCESSTOTHESITE">
                                    Why we collect it<span className="collapsed"><p className="accord_arrow"><b><i className="fa fa-angle-down" aria-hidden="true"></i></b></p></span> <span className="expanded"><p className="accord_arrow"><b><i className="fa fa-angle-up" aria-hidden="true"></i></b></p></span>
                                </button>
                            </h5>
                        </div>
                        <div id="ACCESSTOTHESITE" className="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample">
                            <div className="card-body">
                                <p> <b>We collect information for the various functions mentioned below</b> </p>
                                <ul>
                                    <li><strong>Transaction completion</strong>: The above information is collected in order to successfully process your order, provide you with a proof of purchase and also for customer service related to your order and transaction.</li>
                                    <li><strong>Information / Updates</strong>: To send pertinent and important information/updates related to experiences for which you have purchased tickets or packages, only when needed, to
ensure you have a smooth and joyful experience</li>
                                    <li><strong>Marketing emailers and/or surveys</strong>: To inform you about new experiences and features as well as to help us understand how we can serve you better</li>
                                    <li><strong>Personalization</strong>: To be able to personalize our services to you so as to tailor your experience when visiting a place, and in the future, and also for what types of value
added services and experiences you would like to purchase</li>
                                    <li><strong>Fulfillment of tickets and services</strong>: To ensure that the rightful purchaser is given the tickets and services purchased, identification data is used to cross verify ownership </li>
                                    <li><strong>Issue resolution</strong>: Ability to quickly resolve any issues you may have as well as answer any questions related to our products and services. The history retained helps you from repeating information already provided</li>
                                    <li><strong>Security</strong>: This includes data as needed by security agencies from time to time, to secure the venue or counter any threats. This also includes information needed to prevent frauds </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div className="card">
                        <div className="card-header" id="headingThree">
                            <h5 className="mb-0">
                                <button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#USERCONTENT" aria-expanded="false" aria-controls="USERCONTENT">
                                    Your rights<span className="collapsed"><p className="accord_arrow"><b><i className="fa fa-angle-down" aria-hidden="true"></i></b></p></span> <span className="expanded"><p className="accord_arrow"><b><i className="fa fa-angle-up" aria-hidden="true"></i></b></p></span>
                                </button>
                            </h5>
                        </div>
                        <div id="USERCONTENT" className="collapse" aria-labelledby="headingThree" data-parent="#accordionExample">
                            <div className="card-body">
                                <ul>
                                    <li><strong>Opting out</strong>: You have the ability to opt out of marketing emailers and surveys by clicking on “unsubscribe from the list” at the bottom of our communications which is a standard feature on all our marketing communications. Please note that while this will stop all future marketing emails, transaction history with the associated contact and device details will still be retained </li>
                                    <li><strong>Personal information correction</strong>: Request for correction in your contact and other details stored with us per above</li>
                                    <li><strong>Erasure</strong>: You have the right to request for your information to be deleted from the site. Please send an email to <a href="mailto:support@feelitLIVE.com">support@feelitLIVE.com</a> if you opt for this</li>
                                    <li><strong>Object to processing of your personal data</strong> where we are relying on a legitimate interest (or those of a third party) and there is something about your particular situation which makes you want to object to processing on this ground as you feel it impacts on your fundamental rights and freedoms. You also have the right to object where we are processing your personal data for direct marketing purposes. In some cases, we may demonstrate that we have compelling legitimate grounds to process your information which override your rights and freedoms</li>
                                </ul>

                                <p>feelitLIVE reserves the right to deny you any of the above rights and may provide an explanation as required by applicable laws. Exceptional circumstances include where: </p>

                                <ul>
                                    <li>Acting on your request pursuant to the above rights would result in feelitLIVE being in violation of any statutory or regulatory provisions</li>
                                    <li>An investigating authority or government institution objects to feelitLIVE complying with a customer’s request</li>
                                    <li>The information may, in our reasonable discretion and/or assessment, affect the life or security of an individual, or </li>
                                    <li>Personal Information is collected in connection with an investigation of a breach of contract, suspicion of fraudulent activities or contravention of law</li>
                                </ul>
                            </div>
                        </div>
                    </div>

                    <div className="card">
                        <div className="card-header" id="headingThree">
                            <h5 className="mb-0">
                                <button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#INDEMNIFICATION" aria-expanded="false" aria-controls="INDEMNIFICATION">
                                    Data security<span className="collapsed"><p className="accord_arrow"><b><i className="fa fa-angle-down" aria-hidden="true"></i></b></p></span> <span className="expanded"><p className="accord_arrow"><b><i className="fa fa-angle-up" aria-hidden="true"></i></b></p></span>
                                </button>
                            </h5>
                        </div>
                        <div id="INDEMNIFICATION" className="collapse" aria-labelledby="headingThree" data-parent="#accordionExample">
                            <div className="card-body">
                                <p>Appropriate security measures have been put in place to prevent your personal data from being accidentally lost, used or accessed in an unauthorised way, altered or disclosed. In addition, we limit access to your personal data to those employees, agents, contractors and other third parties who have a business need to know. They will only process your personal data on our instructions and they are subject to a duty of confidentiality.</p>
                                <p>We have put in place procedures to deal with any suspected personal data breach and will notify you and any applicable regulator of a breach where we are legally required to do so. </p>
                                <p>We also highly recommend that you store all your sensitive information securely and not share it with anyone when asked.</p>
                            </div>
                        </div>
                    </div>

                    <div className="card">
                        <div className="card-header" id="headingThree">
                            <h5 className="mb-0">
                                <button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#THIRD-PARTY" aria-expanded="false" aria-controls="THIRD-PARTY">
                                    Data retention<span className="collapsed"><p className="accord_arrow"><b><i className="fa fa-angle-down" aria-hidden="true"></i></b></p></span> <span className="expanded"><p className="accord_arrow"><b><i className="fa fa-angle-up" aria-hidden="true"></i></b></p></span>
                                </button>
                            </h5>
                        </div>
                        <div id="THIRD-PARTY" className="collapse" aria-labelledby="headingThree" data-parent="#accordionExample">
                            <div className="card-body">
                                <p>We will retain your personal data for as long as necessary to fulfil the purposes we collected it for, including for the purposes of satisfying any legal, accounting, or reportingrequirements.</p>
                                <p>We retain your personal data while your account is in existence or as needed to provide you the Website and App. This includes data you or others provided to us and data generated or inferred from your use of our Website/s or App/s.</p>
                                <p>In some circumstances you can ask us to delete your data: please see the section on Your rights above. In certain cases, we may anonymise your personal data (so that it can no longer be associated with you) for research or statistical purposes in which case we may use this information indefinitely without further notice to you.</p>
                            </div>
                        </div>
                    </div>

                    <div className="card">
                        <div className="card-header" id="headingThree">
                            <h5 className="mb-0">
                                <button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#DISCLAIMERS" aria-expanded="false" aria-controls="DISCLAIMERS">
                                    Data sharing<span className="collapsed"><p className="accord_arrow"><b><i className="fa fa-angle-down" aria-hidden="true"></i></b></p></span> <span className="expanded"><p className="accord_arrow"><b><i className="fa fa-angle-up" aria-hidden="true"></i></b></p></span>
                                </button>
                            </h5>
                        </div>
                        <div id="DISCLAIMERS" className="collapse" aria-labelledby="headingThree" data-parent="#accordionExample">
                            <div className="card-body">
                                <p><strong>Who we share data with and why are as below</strong></p>
                                <ul>
                                    <li>Within the feelitLIVE group of companies</li>
                                    <li>Any third party service providers such as hosting or cloud computing who provide the IT infrastructure where our data is stored and/or on which the services are build</li>
                                    <li>Any feelitLIVE Partner/s to enable them to service you, per their Privacy Policies. In such instances we will explicitly inform you during the purchase process to get your consent. You will also have the option to opt out of their marketing emailers</li>
                                    <li>Government agencies or other authorised bodies where permitted or required by law</li>
                                    <li>Any successor to all or part of our business</li>
                                    <li>Third parties who provide goods and services purchased by you (e.g. ticket insurance or merchandise) so that they can process and fulfil your orders.</li>
                                </ul>

                            </div>
                        </div>
                    </div>
                    <div className="card">
                        <div className="card-header" id="headingThree">
                            <h5 className="mb-0">
                                <button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target="#LIMITATIONONLIABILITY" aria-expanded="false" aria-controls="LIMITATIONONLIABILITY">
                                    Cookies<span className="collapsed"><p className="accord_arrow"><b><i className="fa fa-angle-down" aria-hidden="true"></i></b></p></span> <span className="expanded"><p className="accord_arrow"><b><i className="fa fa-angle-up" aria-hidden="true"></i></b></p></span>
                                </button>
                            </h5>
                        </div>
                        <div id="LIMITATIONONLIABILITY" className="collapse" aria-labelledby="headingThree" data-parent="#accordionExample">
                            <div className="card-body">
                                <p>When you interact with us, we and other third party organizations may uses cookie, web beacon technologies and other technologies such as pixel tags. Most browsers will enable you to manage your cookies preferences e.g. have the browser notify you when you receive a new cookie or use it to disable cookies altogether. If you do not wish to allow us and third party organisations to use cookies within our emails, such as pixel tags, the best way to do this is not to enable images when you view our emails. In other words, only view the plain-text of the email. Some web browsers and email clients have settings or extensions available to disable / block such cookies such as Gmail.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>);
}

export default PrivacyPolicy;