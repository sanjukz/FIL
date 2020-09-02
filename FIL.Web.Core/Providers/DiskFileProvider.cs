
using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Web.Core.Providers
{
    public interface IDiskFileProvider
    {
        string GetImagePath(string altId, ImageType imageType);
    }

    public class DiskFileProvider : IDiskFileProvider
    {
        public string GetImagePath(string altId, ImageType imageType)
        {
            return Path.Combine(QrCodeGenerator.ApplicationPath(), ImageFilePathProvider.GetImageFilePath(altId, imageType));
        }

        public string GetQrCodeImagePath(string altId, ImageType imageType)
        {
            QrCodeGenerator.GenerateQrCode(altId);
            return Path.Combine(QrCodeGenerator.ApplicationPath(), ImageFilePathProvider.GetImageFilePath(altId, imageType));
        }
    }
}
