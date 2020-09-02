using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Logging.Enums;
using FIL.Web.Core.Providers;
using Microsoft.Extensions.Configuration;

namespace FIL.Web.Core
{
    public interface IAmazonS3FileProvider
    {
        string GetImagePath(string altId, ImageType imageType);
        string GetImagePath(string altId);
        void UploadImage(string altId, ImageType imageType);
        void UploadImage(string path, string altId);
        void UploadFeelImage(string path, string altId);
        void UploadImage(Image image, string altId);
        void UploadFeelImage(Image image, string altId, ImageType imageType);
        void UploadZoongaImage(Image image, string altId, ImageType imageType);
        void UploadHotTicketImage(Image image, string altId);
        void DownloadImage(string altId, ImageType imageType);
        void BroweseAndUploadImage(string filePath, string altId, ImageType imageType);
        void UploadBarodeImage(string altId, ImageType imageType);
        void UploadQrImages(List<MatchSeatTicketInfo> matchSeatTicketInfos, ImageType imageType);
        void UploadQrCodeImage(string altId, ImageType imageType);
        void UploadBlogImage(Image image, int id);
    }

    public class AmazonS3FileProvider : IAmazonS3FileProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ISettings _settings;
        private readonly FIL.Logging.ILogger _logger;
        public AmazonS3FileProvider(IConfiguration configuration, ISettings settings, FIL.Logging.ILogger logger)
        {
            _configuration = configuration;
            _settings = settings;
            _logger = logger;
        }

        public string GetImagePath(string altId, ImageType imageType)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                {
                    GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                    {
                        BucketName = bucketName,
                        Key = ImageFilePathProvider.GetImageFilePath(altId, imageType),
                        Expires = DateTime.Now.AddYears(1)
                    };
                    return s3Client.GetPreSignedURL(request);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return "";
            }
        }

        public string GetImagePath(string altId)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                {
                    GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                    {
                        BucketName = bucketName,
                        Key = $"Images/ProfilePictures/{altId}.jpg",
                        Expires = DateTime.Now.AddMinutes(60)
                    };
                    return s3Client.GetPreSignedURL(request);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return "";

            }
        }

        public void UploadImage(string altId, ImageType imageType)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                using (var stream = QrCodeGenerator.GetQrcodeStream(altId))
                {
                    using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                    {
                        var transferUtility = new TransferUtility(s3Client);
                        {
                            transferUtility.Upload(stream, bucketName, ImageFilePathProvider.GetImageFilePath(altId, imageType));
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void UploadImage(string path, string altId)
        {
            var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
            var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
            var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

            var s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2"));
            var transferUtility = new TransferUtility(s3Client);
            transferUtility.Upload(path, bucketName, $"Images/ProfilePictures/{altId}.jpg");
        }

        public void UploadFeelImage(string path, string altId)
        {
            var clientId = _settings.GetConfigSetting<string>(SettingKeys.Aws.S3.Feel.AccessKey);
            var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Aws.S3.Feel.SecretKey);
            var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Aws.S3.Feel.BucketName);

            var s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2"));
            var transferUtility = new TransferUtility(s3Client);
            transferUtility.Upload(path, bucketName, $"Images/ProfilePictures/{altId}.jpg");
        }

        public void UploadImage(Image image, string altId)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                var stream = new System.IO.MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;

                using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                {
                    var transferUtility = new TransferUtility(s3Client);
                    {
                        transferUtility.Upload(stream, bucketName, ImageFilePathProvider.GetImageFilePath(altId.ToUpper(), ImageType.ProfilePic));
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void UploadFeelImage(Image image, string altId, ImageType imageType)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.Feel.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.Feel.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.Feel.BucketName);
                var stream = new System.IO.MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;

                using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                {
                    var transferUtility = new TransferUtility(s3Client);
                    {
                        transferUtility.Upload(stream, bucketName, ImageFilePathProvider.GetImageFilePath(altId, imageType));
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void UploadZoongaImage(Image image, string altId, ImageType imageType)
        {
            try
            {

                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                var stream = new System.IO.MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;

                using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                {
                    var transferUtility = new TransferUtility(s3Client);
                    {
                        transferUtility.Upload(stream, bucketName, ImageFilePathProvider.GetImageFilePath(altId, imageType));
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void UploadHotTicketImage(Image image, string altId)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                var stream = new System.IO.MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;

                using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                {
                    var transferUtility = new TransferUtility(s3Client);
                    {
                        transferUtility.Upload(stream, bucketName, ImageFilePathProvider.GetImageFilePath(altId, FIL.Contracts.Enums.ImageType.HotTicket));
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void DownloadImage(string altId, ImageType imageType)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                QrCodeGenerator.GenerateQrCode(altId);
                var s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2"));
                var transferUtility = new TransferUtility(s3Client);
                transferUtility.Download(Path.Combine(QrCodeGenerator.ApplicationPath(), ImageFilePathProvider.GetImageFilePath(altId, imageType)), bucketName, ImageFilePathProvider.GetImageFilePath(altId, imageType));
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void BroweseAndUploadImage(string filePath, string altId, ImageType imageType)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                var s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2"));
                var transferUtility = new TransferUtility(s3Client);
                transferUtility.Upload(Path.Combine(filePath), bucketName, ImageFilePathProvider.GetImageFilePath(altId, imageType));
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void UploadBarodeImage(string altId, ImageType imageType)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                using (var stream = QrCodeGenerator.GetBarcodeStream(altId))
                {
                    using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                    {
                        var transferUtility = new TransferUtility(s3Client);
                        {
                            transferUtility.Upload(stream, bucketName, ImageFilePathProvider.GetImageFilePath(altId, imageType));
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void UploadQrCodeImage(string altId, ImageType imageType)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);

                using (var stream = QrCodeGenerator.GetQrcodeStream(altId))
                {
                    using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                    {
                        var transferUtility = new TransferUtility(s3Client);
                        {
                            transferUtility.Upload(stream, bucketName, ImageFilePathProvider.GetImageFilePath(altId, imageType));
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

        public void UploadQrImages(List<MatchSeatTicketInfo> matchSeatTicketInfos, ImageType imageType)
        {
            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.BucketName);
                foreach (var item in matchSeatTicketInfos)
                {
                    using (var stream = QrCodeGenerator.GetQrcodeStream(item.BarcodeNumber))
                    {
                        using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                        {
                            var transferUtility = new TransferUtility(s3Client);
                            {
                                transferUtility.Upload(stream, bucketName, ImageFilePathProvider.GetImageFilePath(item.BarcodeNumber, imageType));
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }
        public void UploadBlogImage(Image image, int id)
        {

            try
            {
                var clientId = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.Feel.AccessKey);
                var clientSecret = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.Feel.SecretKey);
                var bucketName = _settings.GetConfigSetting<string>(SettingKeys.Messaging.Aws.S3.Feel.BucketName);
                var stream = new System.IO.MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;

                using (IAmazonS3 s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2")))
                {
                    var transferUtility = new TransferUtility(s3Client);
                    {
                        transferUtility.Upload(stream, bucketName, $"images/blogs/{id}.jpg");
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
        }

    }

}
