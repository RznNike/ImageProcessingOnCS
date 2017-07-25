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

        /// <summary>
        /// Returns singletone instance
        /// </summary>
        /// <returns>Instance</returns>
        public static VInterlacingWithGZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new VInterlacingWithGZIP();
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
            original.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Stream rotatedImage = new MemoryStream();
            original.Save(rotatedImage, ImageFormat.Bmp);
            HInterlacingWithGZIP hInterlacing = HInterlacingWithGZIP.GetInstance();
            
            return hInterlacing.CompressImage(rotatedImage, parArguments);
        }

        /// <summary>
        /// Inverse application of the algorithm
        /// </summary>
        /// <param name="parCompressedImage">Compressed image stream</param>
        /// <param name="parArguments">List of an arguments (ImageFormat finalFormat)</param>
        /// <returns>Decompressed image stream</returns>
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
            return "VInterlacing+GZIP";
        }
    }
}
