using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageProcessingOnSharp
{
    public class WaveletWithGZIP : Algorithm
    {
        private static WaveletWithGZIP _instance = null;

        private WaveletWithGZIP()
        {
        }

        public static WaveletWithGZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WaveletWithGZIP();
            }
            return _instance;
        }

        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            Bitmap original = new Bitmap(parOriginalImage);
            int levels = (int)parArguments[0];
            HaarWavelet.ApplyTransform(ref original, true, levels);
            
            Stream waveletStream = new MemoryStream();
            ImageFormat format = (ImageFormat)parArguments[2];
            original.Save(waveletStream, format);

            GZIP gzip = GZIP.GetInstance();
            int compressionLevel = (int)parArguments[1];
            Stream compressedImage = gzip.CompressImage(waveletStream, new List<object>() { compressionLevel });

            return compressedImage;
        }

        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            GZIP gzip = GZIP.GetInstance();
            Stream decompressedWavelet = gzip.DecompressImage(parCompressedImage, new List<object>());

            Bitmap wavelet = new Bitmap(decompressedWavelet);
            int levels = (int)parArguments[0];

            HaarWavelet.ApplyTransform(ref wavelet, false, levels);

            Stream result = new MemoryStream();
            ImageFormat format = (ImageFormat)parArguments[1];
            wavelet.Save(result, format);

            return result;
        }

        public override string GetFileExtension()
        {
            return "bmp";
        }

        public override string ToString()
        {
            return "Wavelet+GZIP";
        }
    }
}
