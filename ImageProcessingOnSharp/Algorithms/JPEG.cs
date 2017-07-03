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
            EncoderParameters parameters = new EncoderParameters();
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 1);
            Stream result = new MemoryStream();
            Bitmap tempImage = new Bitmap(parOriginalImage);
            tempImage.Save(result, null, parameters);
            return result;
        }

        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "JPEG";
        }
    }
}
