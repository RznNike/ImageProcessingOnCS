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

        /// <summary>
        /// Returns singletone instance
        /// </summary>
        /// <returns>Instance</returns>
        public static WaveletWithGZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WaveletWithGZIP();
            }
            return _instance;
        }

        /// <summary>
        /// Forward application of the algorithm
        /// </summary>
        /// <param name="parOriginalImage">Original image stream</param>
        /// <param name="parArguments">List of an arguments (long qualityLevel, ImageFormat interimFormat)</param>
        /// <returns>Compressed image stream</returns>
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

        /// <summary>
        /// Inverse application of the algorithm
        /// </summary>
        /// <param name="parCompressedImage">Compressed image stream</param>
        /// <param name="parArguments">List of an arguments (ImageFormat finalFormat)</param>
        /// <returns>Decompressed image stream</returns>
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

        /// <summary>
        /// Returns file extension of algorithm inverse application result
        /// </summary>
        /// <returns>Extension without dot (string)</returns>
        public override string GetFileExtension()
        {
            return "bmp";
        }

        /// <summary>
        /// Overrides original ToString() method
        /// </summary>
        /// <returns>Algorithm name</returns>
        public override string ToString()
        {
            return "Wavelet+GZIP";
        }
    }
}
