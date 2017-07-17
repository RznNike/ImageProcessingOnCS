using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageProcessingOnSharp
{
    public class JPEG : Algorithm
    {
        private static JPEG _instance = null;

        private JPEG()
        {
        }
        
        public static JPEG GetInstance()
        {
            if (_instance == null)
            {
                _instance = new JPEG();
            }
            return _instance;
        }

        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            long qualityLevel = (long)parArguments[0];
            EncoderParameters parameters = new EncoderParameters();
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, qualityLevel);
            ImageCodecInfo[] codecInfos = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegInfo = null;
            foreach (ImageCodecInfo info in codecInfos)
            {
                if (info.FormatDescription.Equals("JPEG"))
                {
                    jpegInfo = info;
                }
            }
            Stream result = new MemoryStream();
            Bitmap tempImage = new Bitmap(parOriginalImage);
            tempImage.Save(result, codecInfos[1], parameters);
            return result;
        }

        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            return parCompressedImage;
        }

        public override string GetFileExtension()
        {
            return "jpeg";
        }

        public override string ToString()
        {
            return "JPEG";
        }
    }
}
