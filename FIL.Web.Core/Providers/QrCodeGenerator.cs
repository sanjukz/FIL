using BarcodeLib;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;


namespace FIL.Web.Core.Providers
{
    public class QrCodeGenerator
    {
      
        public static void GenerateQrCode(string qrCode)
        {
            Gma.QrCodeNet.Encoding.QrEncoder Encoder = new Gma.QrCodeNet.Encoding.QrEncoder(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.H);
            Gma.QrCodeNet.Encoding.QrCode Code = Encoder.Encode(qrCode);

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(10, QuietZoneModules.Two), System.Drawing.Brushes.Black, System.Drawing.Brushes.White);
            using (FileStream stream = new FileStream($"{Path.Combine(ApplicationPath(), "Images", "QrCodes")}\\{qrCode}.png", FileMode.Create))
            {
                renderer.WriteToStream(Code.Matrix, System.Drawing.Imaging.ImageFormat.Png, stream);
            }
        }

        public static Stream GetQrcodeStream (string qrCode)
        {
            Gma.QrCodeNet.Encoding.QrEncoder Encoder = new Gma.QrCodeNet.Encoding.QrEncoder(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.H);
            Gma.QrCodeNet.Encoding.QrCode Code = Encoder.Encode(qrCode);
            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(10, QuietZoneModules.Two), System.Drawing.Brushes.Black, System.Drawing.Brushes.White);
            var stream = new MemoryStream();
            renderer.WriteToStream(Code.Matrix, System.Drawing.Imaging.ImageFormat.Png, stream);
            stream.Position = 0;
            return stream;
        }

        public static Stream GetBarcodeStream(string barCode)
        {
            Barcode objBarcode = new Barcode();
            Image image = objBarcode.Encode(BarcodeLib.TYPE.CODE39, barCode, Color.Black, Color.White, 500, 60);
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Bmp);
            stream.Position = 0;
            return stream;
        }

        public static string ApplicationPath()
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                                .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            //return appRoot.Substring(0, appRoot.LastIndexOf('\\'));
            return appRoot;
        }
    }
}
