using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageProcessingOnSharp
{
    public class VInterlacingWithGZIP : Algorithm
    {
        private static VInterlacingWithGZIP _instance = null;

        private VInterlacingWithGZIP()
        {
        }
        
        public static VInterlacingWithGZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new VInterlacingWithGZIP();
            }
            return _instance;
        }

        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            Bitmap original = new Bitmap(parOriginalImage);
            original.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Stream rotatedImage = new MemoryStream();
            original.Save(rotatedImage, ImageFormat.Bmp);
            HInterlacingWithGZIP hInterlacing = HInterlacingWithGZIP.GetInstance();
            
            return hInterlacing.CompressImage(rotatedImage, parArguments);
        }

        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            HInterlacingWithGZIP hInterlacing = HInterlacingWithGZIP.GetInstance();
            Stream rotatedImage = hInterlacing.DecompressImage(parCompressedImage, parArguments);
            Bitmap reconstructed = new Bitmap(rotatedImage);
            reconstructed.RotateFlip(RotateFlipType.Rotate270FlipNone);
            Stream reconstructedImage = new MemoryStream();
            ImageFormat format = (ImageFormat)parArguments[0];
            reconstructed.Save(reconstructedImage, format);

            return reconstructedImage;
        }

        private int FindAverageColor(int parColor1, int parColor2)
        {
            Color color1 = Color.FromArgb(parColor1);
            Color color2 = Color.FromArgb(parColor2);
            int A = (color1.A + color2.A) / 2;
            int R = (color1.R + color2.R) / 2;
            int G = (color1.G + color2.G) / 2;
            int B = (color1.B + color2.B) / 2;

            Color resultColor = Color.FromArgb(A, R, G, B);
            return resultColor.ToArgb();
        }

        public override string GetFileExtension()
        {
            return "bmp";
        }

        public override string ToString()
        {
            return "VInterlacing+GZIP";
        }
    }
}
