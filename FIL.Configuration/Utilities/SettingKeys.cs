namespace FIL.Configuration.Utilities
{
    public class SettingKeys
    {
        public class Api
        {
            public const string HealthCheckFailFileName = "Api.HealthCheckFailFileName";

            public class Database
            {
                public const string ReadOnlyConnectionString = "Api.Database.ReadOnlyConnectionString";
                public const string TransactionalConnectionString = "Api.Database.TransactionalConnectionString";
            }
        }

        public class Foundation
        {
            public class Http
            {
                public const string ApiEndpoint = "Foundation.Http.ApiEndpoint";
            }
        }

        public class Messaging
        {
            public class TextMessages
            {
                public const string From = "Messaging.TextMessages.From";

                public class Twilio
                {
                    public const string AccountSid = "Messaging.TextMessages.Twilio.AccountSid";
                    public const string Token = "Messaging.TextMessages.Twilio.Token";
                    public const string ServiceSid = "Messaging.TextMessages.Twilio.ServiceSid";
                }

                public class Kap
                {
                    public const string ApiUrlFormat = "Messaging.TextMessages.Kap.ApiUrlFormat"; // http://trans.kapsystem.com/api/web2sms.php?workingkey={0}&to={1}&sender={2}&message={3}
                    public const string ApiKey = "Messaging.TextMessages.Kap.ApiKey"; // kapkey:A1fb4f7fde809125a451a54e0450c67d6
                    public const string Sender = "Messaging.TextMessages.Kap.Sender"; // kapsender:KYAZIN
                }

                public class GupShup
                {
                    public const string ApiUrlFormat = "Messaging.TextMessages.GupShup.ApiUrlFormat";
                    public const string UserId = "Messaging.TextMessages.GupShup.UserId";
                    public const string Password = "Messaging.TextMessages.GupShup.Password";
                }
            }

            public class Email
            {
                public class AmazonSES
                {
                    public const string EmailHost = "Messaging.Email.Amazon.SES.EmailHost";
                    public const string EmailPort = "Messaging.Email.Amazon.SES.EmailPort";
                    public const string HostUserName = "Messaging.Email.Amazon.SES.HostUserName";
                    public const string HostUserPwd = "Messaging.Email.Amazon.SES.HostUserPwd";
                }
            }

            public class Aws
            {
                public class Ses
                {
                    public const string AccessKey = "Messaging.Aws.Ses.AccessKey";
                    public const string SecretKey = "Messaging.Aws.Ses.SecretKey";
                    public const string RegionName = "Messaging.Aws.Ses.RegionName";
                }

                public class S3
                {
                    public const string AccessKey = "FileProviders.Aws.S3.AccessKey";
                    public const string SecretKey = "FileProviders.Aws.S3.SecretKey";
                    public const string BucketName = "FileProviders.Aws.S3.BucketName";

                    public class Feel
                    {
                        public const string AccessKey = "FileProviders.Aws.S3.Feel.AccessKey";
                        public const string SecretKey = "FileProviders.Aws.S3.Feel.SecretKey";
                        public const string BucketName = "FileProviders.Aws.S3.Feel.BucketName";
                    }
                }
            }
        }

        public class Aws
        {
            public class S3
            {
                public const string PathName = "FileProviders.Aws.S3.Path";

                public class Zoonga
                {
                    public const string StaticURL = "FileProviders.Aws.S3.Zoonga.Static";

                    public const string AccessKey = "FileProviders.Aws.S3.AccessKey";
                    public const string SecretKey = "FileProviders.Aws.S3.SecretKey";
                    public const string BucketName = "FileProviders.Aws.S3.BucketName";
                }

                public class Feel
                {
                    public const string AccessKey = "FileProviders.Aws.S3.Feel.AccessKey";
                    public const string SecretKey = "FileProviders.Aws.S3.Feel.SecretKey";
                    public const string BucketName = "FileProviders.Aws.S3.Feel.BucketName";

                    public const string StaticURL = "FileProviders.Aws.S3.Feel.Static";
                }
            }
        }

        public class Redis
        {
            public const string ConnectionString = "Redis.ConnectionString";
            public const string InstanceName = "Redis.InstanceName";
            public const string Enabled = "Redis.Enabled";
        }

        public class PaymentGateway
        {
            public class Stripe
            {
                public const string PublishableKey = "PaymentGateway.Stripe.PublishableApiKey";
                public const string SecretKey = "PaymentGateway.Stripe.SecretApiKey";

                public class Feel
                {
                    public const string PublishableKey = "PaymentGateway.Stripe.Feel.PublishableApiKey";
                    public const string SecretKey = "PaymentGateway.Stripe.Feel.SecretApiKey";
                }

                public class Connect
                {
                    public const string ConnectClinetId = "PaymentGateway.Stripe.Connect.Feel.ClientId";
                    public const string ConnectAustraliaClinetId = "PaymentGateway.Stripe.Connect.FeelAustralia.ClientId";
                    public const string ConnectIndiaClinetId = "PaymentGateway.Stripe.Connect.FeelIndia.ClientId";
                    public const string ConnectUkClinetId = "PaymentGateway.Stripe.Connect.FeelUk.ClientId";
                    public const string ConnectSingaporeClinetId = "PaymentGateway.Stripe.Connect.FeelSingapore.ClientId";
                }

                public class FeelAustralia
                {
                    public const string PublishableKey = "PaymentGateway.Stripe.FeelAustralia.PublishableApiKey";
                    public const string SecretKey = "PaymentGateway.Stripe.FeelAustralia.SecretApiKey";
                }

                public class FeelIndia
                {
                    public const string PublishableKey = "PaymentGateway.Stripe.FeelIndia.PublishableApiKey";
                    public const string SecretKey = "PaymentGateway.Stripe.FeelIndia.SecretApiKey";
                }

                public class FeelUk
                {
                    public const string PublishableKey = "PaymentGateway.Stripe.FeelUk.PublishableApiKey";
                    public const string SecretKey = "PaymentGateway.Stripe.FeelUk.SecretApiKey";
                }

                public class FeelISingapore
                {
                    public const string PublishableKey = "PaymentGateway.Stripe.FeelSingapore.PublishableApiKey";
                    public const string SecretKey = "PaymentGateway.Stripe.FeelSingapore.SecretApiKey";
                }

                public class ZoongaAustralia
                {
                    public const string PublishableKey = "PaymentGateway.Stripe.ZoongaAustralia.PublishableApiKey";
                    public const string SecretKey = "PaymentGateway.Stripe.ZoongaAustralia.SecretApiKey";
                }
            }

            public class Paypal
            {
                public const string HostUrl = "PaymentGateway.Paypal.HostUrl";
                public const string EndPoint = "PaymentGateway.Paypal.EndPoint";
                public const string ApiUsername = "PaymentGateway.Paypal.ApiUsername";
                public const string ApiPassword = "PaymentGateway.Paypal.ApiPassword";
                public const string ApiSignature = "PaymentGateway.Paypal.ApiSignature";
                public const string ReturnUrl = "PaymentGateway.Paypal.ReturnUrl";
                public const string CancelUrl = "PaymentGateway.Paypal.CancelUrl";
            }

            public class Icici
            {
                public const string MerchantId = "PaymentGateway.Icici.MerchantId";
                public const string AccessKey = "PaymentGateway.Icici.AccessKey";
                public const string AspUrl = "PaymentGateway.Icici.AspUrl";
                public const string PostUrl = "PaymentGateway.Icici.PostUrl";
                public const string ReturnUrl = "PaymentGateway.Icici.ReturnUrl";
            }

            public class Ccavenue
            {
                public const string MerchantId = "PaymentGateway.Ccavenue.MerchantId";
                public const string AccessKey = "PaymentGateway.Ccavenue.AccessKey";
                public const string WorkingKey = "PaymentGateway.Ccavenue.WorkingKey";
                public const string PostUrl = "PaymentGateway.Ccavenue.PostUrl";
                public const string ReturnUrl = "PaymentGateway.Ccavenue.ReturnUrl";
                public const string FeelReturnUrl = "PaymentGateway.Ccavenue.Feel.ReturnUrl";
            }

            public class Amex
            {
                public const string VpcPaymentServerUrl = "PaymentGateway.Amex.VpcPaymentServerUrl";
                public const string VpcPaymentCaptureServerUrl = "PaymentGateway.Amex.VpcPaymentCaptureServerUrl";
                public const string UserName = "PaymentGateway.Amex.UserName";
                public const string Password = "PaymentGateway.Amex.Password";
                public const string MerchantId = "PaymentGateway.Amex.MerchantId";
                public const string AccessKey = "PaymentGateway.Amex.AccessKey";
                public const string VpcCommand = "PaymentGateway.Amex.VpcCommand";
                public const string VpcVersion = "PaymentGateway.Amex.VpcVersion";
                public const string SecureSecret = "PaymentGateway.Amex.SecureSecret";
            }

            public class Hdfc
            {
                public const string TranportalId = "PaymentGateway.Hdfc.TranportalId";
                public const string TranportalPassword = "PaymentGateway.Hdfc.TranportalPassword";
                public const string EnrollmentVerificationUrl = "PaymentGateway.Hdfc.EnrollmentVerificationUrl";
                public const string TransactionPortalUrl = "PaymentGateway.Hdfc.TransactionPortalUrl";
                public const string PayerAuthenticationUrl = "PaymentGateway.Hdfc.PayerAuthenticationUrl";
                public const string ReturnUrl = "PaymentGateway.Hdfc.ReturnUrl";
            }

            public class AmazonPay
            {
                public const string MerchantId = "PaymentGateway.AmazonPay.MerchantId";
                public const string AccessKey = "PaymentGateway.AmazonPay.AccessKey";
                public const string SecretKey = "PaymentGateway.AmazonPay.SecretKey";
                public const string IsSandbox = "PaymentGateway.AmazonPay.IsSandbox";
                public const string ReturnUrl = "PaymentGateway.AmazonPay.ReturnUrl";
            }

            public class NabTransact
            {
                public const string MerchantId = "PaymentGateway.NabTransact.MerchantId";
                public const string Password = "PaymentGateway.NabTransact.Password";
                public const string PostUrl = "PaymentGateway.NabTransact.PostUrl";
                public const string ReturnUrl = "PaymentGateway.NabTransact.ReturnUrl";
            }
        }

        public class Integration
        {
            public class Feel
            {
                public const string Endpoint = "Integration.Feel.Endpoint";
            }

            public class DTCM
            {
                public const string BaseUrl = "Integration.DTCM.BaseUrl";
                public const string SellerCode = "Integration.DTCM.SellerCode";
                public const string ClientKey = "Integration.DTCM..ClientKey";
                public const string ServerKey = "Integration.DTCM.ServerKey";
            }

            public class KidZania
            {
                public const string PartnerId = "Integration.KidZania.PartnerId";
                public const string Password = "Integration.KidZania.Password";
                public const string CityId = "Integration.KidZania..CityId";
                public const string ParkId = "Integration.KidZania.ParkId";
            }

            public class InfiniteAnalytics
            {
                public const string BaseUrl = "Integration.InfiniteAnalytics.BaseUrl";
            }

            public class HubSpot
            {
                public const string ApiKey = "Integration.HubSpot.ApiKey";
            }

            public class CitySightSeeing
            {
                public const string DistributorId = "Integration.CitySightSeeing.DistributorId";
                public const string Token = "Integration.CitySightSeeing.Token";
                public const string RequestAuthentication = "Integration.CitySightSeeing.RequestAuthentication";
                public const string RequestIdentifier = "Integration.CitySightSeeing.RequestIdentifier";
                public const string Endpoint = "Integration.CitySightSeeing.Endpoint";
            }

            public class ValueRetail
            {
                public const string AuthClientId = "Integration.ValueReatil.AuthClientId";
                public const string AuthClientSecret = "Integration.ValueReatil.AuthClientSecret";
                public const string AuthEndpoint = "Integration.ValueRetail.AuthEndpoint";
                public const string AuthGrantType = "Integration.ValueRetail.AuthGrantType";
                public const string AuthResource = "Integration.ValueRetail.AuthResource";

                public const string AggregatorId = "Integration.ValueReatil.AggregatorId";
                public const string OtaId = "Integration.ValueReatil.OtaId";

                public const string ApiEndpoint = "Integration.ValueRetail.ApiEndpoint";
                public const string SubscriptionKey = "Integration.ValueRetail.SubscriptionKey";
                public const string Token = "Integration.ValueReatil.Token";
            }

            public class ASI
            {
                public const string MonumentEndPoint = "Integration.ASI.Endpoint";
                public const string IntegrationRoot = "Integration.ASI.API.Integration.Root";
                public const string APIKey = "Integration.ASI.API.Key";
                public const string APISalt = "Integration.ASI.API.Salt";
            }

            public class GoogleGeocoding
            {
                public const string APIKey = "Integration.GoogleGeocoding.Key";
            }

            public class Tiqets
            {
                public const string Token = "Integration.Tiqets.Token";
                public const string Endpoint = "Integration.Tiqets.EndPoint";
                public const string PrivateKey = "Integration.Tiqets.PrivateKey";
                public const string CustomerEmail = "Integration.Tiqets.Customer.Email";
                public const string CustomerPhone = "Integration.Tiqets.Customer.Phone";
            }

            public class Algolia
            {
                public const string API_Key = "Integration.Algolia.API.Key";
                public const string APP_ID = "Integration.Algolia.AppId";
                public const string Index = "Integration.Algolia.Index";
                public const string CityIndex = "Integration.Algolia.CityIndex";
            }

            public class POne
            {
                public const string Token = "Integration.POne.Token";
            }

            public class OneSignalAppID
            {
                public const string Com = "Integration.OneSignalAppId.com";
                public const string Uk = "Integration.OneSignalAppId.co.uk";
                public const string India = "Integration.OneSignalAppId.co.in";
                public const string Aus = "Integration.OneSignalAppId.com.au";
                public const string Spain = "Integration.OneSignalAppId.es";
                public const string France = "Integration.OneSignalAppId.fr";
                public const string NewZealand = "Integration.OneSignalAppId.nz";
                public const string Germany = "Integration.OneSignalAppId.de";
            }

            public class Zoom
            {
                public const string API_Key = "Integration.Zoom.APIKey";
                public const string Secret_key = "Integration.Zoom.SecretKey";
                public const string Base_Url = "Integration.Zoom.BaseUrl";
            }

            public class MailChimp
            {
                public const string ApiKey = "Integration.MailChimp.ApiKey";
                public const string CreatorListId = "Integration.MailChimp.CreatorListId";
                public const string BuyerListId = "Integration.MailChimp.BuyerListId";
                public const string StoreId = "Integration.MailChimp.StoreId";
            }
        }

        public class Security
        {
            public const string SecretKeyOne = "Security.Secret.Key.One";
            public const string SecretKeyTwo = "Security.Secret.Key.Two";
        }
    }
}