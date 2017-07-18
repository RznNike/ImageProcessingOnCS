using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
            Stream result = new MemoryStream();
            Bitmap tempImage = new Bitmap(parOriginalImage);
            tempImage.Save(result, ImageFormat.Png);
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
