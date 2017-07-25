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

        /// <summary>
        /// Returns singletone instance
        /// </summary>
        /// <returns>Instance</returns>
        public static PNG GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PNG();
            }
            return _instance;
        }

        /// <summary>
        /// Forward application of the algorithm
        /// </summary>
        /// <param name="parOriginalImage">Original image stream</param>
        /// <param name="parArguments">List of an arguments (empty)</param>
        /// <returns>Compressed image stream</returns>
        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            Stream result = new MemoryStream();
            Bitmap tempImage = new Bitmap(parOriginalImage);
            tempImage.Save(result, ImageFormat.Png);
            return result;
        }

        /// <summary>
        /// Inverse application of the algorithm
        /// </summary>
        /// <param name="parCompressedImage">Compressed image stream</param>
        /// <param name="parArguments">List of an arguments (empty)</param>
        /// <returns>Decompressed image stream</returns>
        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            return parCompressedImage;
        }

        /// <summary>
        /// Returns file extension of algorithm inverse application result
        /// </summary>
        /// <returns>Extension without dot (string)</returns>
        public override string GetFileExtension()
        {
            return "png";
        }

        /// <summary>
        /// Overrides original ToString() method
        /// </summary>
        /// <returns>Algorithm name</returns>
        public override string ToString()
        {
            return "PNG";
        }
    }
}
