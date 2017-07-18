using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageProcessingOnSharp
{
    public class TIFF : Algorithm
    {
        private static TIFF _instance = null;

        private TIFF()
        {
        }
        
        public static TIFF GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TIFF();
            }
            return _instance;
        }

        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            long compression = (long)parArguments[0];
            EncoderParameters parameters = new EncoderParameters();
            parameters.Param[0] = new EncoderParameter(Encoder.Compression, compression);
            ImageCodecInfo[ ] codecInfos = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo tiffInfo = null;
            foreach (ImageCodecInfo info in codecInfos)
            {
                if (info.FormatDescription.Equals("TIFF"))
                {
                    tiffInfo = info;
                }
            }
            Stream result = new MemoryStream();
            Bitmap tempImage = new Bitmap(parOriginalImage);
            tempImage.Save(result, tiffInfo, parameters);
            return result;
        }

        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            return parCompressedImage;
        }

        public override string GetFileExtension()
        {
            return "tif";
        }

        public override string ToString()
        {
            return "TIFF";
        }
    }
}
