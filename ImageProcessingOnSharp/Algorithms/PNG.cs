using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageProcessingOnSharp
{
    public class PNG : Algorithm
    {
        private static PNG _instance = null;

        private PNG()
        {
        }

        public static PNG GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PNG();
            }
            return _instance;
        }

        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            //long qualityLevel = (long)parArguments[0];
            EncoderParameters parameters = new EncoderParameters();
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 100);
            ImageCodecInfo[ ] codecInfos = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo pngInfo = null;
            foreach (ImageCodecInfo info in codecInfos)
            {
                if (info.FormatDescription.Equals("PNG"))
                {
                    pngInfo = info;
                }
            }
            Stream result = new MemoryStream();
            Bitmap tempImage = new Bitmap(parOriginalImage);
            tempImage.Save(result, pngInfo, parameters);
            return result;
        }

        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            return parCompressedImage;
        }

        public override string GetFileExtension()
        {
            return "png";
        }

        public override string ToString()
        {
            return "PNG";
        }
    }
}
