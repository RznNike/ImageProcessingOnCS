using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ImageProcessingOnSharp
{
    public class GZIP : Algorithm
    {
        private static GZIP _instance = null;

        private GZIP()
        {
        }
        
        /// <summary>
        /// Returns singletone instance
        /// </summary>
        /// <returns>Instance</returns>
        public static GZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GZIP();
            }
            return _instance;
        }

        /// <summary>
        /// Forward application of the algorithm
        /// </summary>
        /// <param name="parOriginalImage">Original image stream</param>
        /// <param name="parArguments">List of an arguments (int compressionLevel)</param>
        /// <returns>Compressed image stream</returns>
        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            parOriginalImage.Position = 0;
            int compressionLevel = (int)parArguments[0];
            Stream compressedImage = new MemoryStream();
            GZipStream zipStream = new GZipStream(compressedImage, (CompressionLevel)compressionLevel, true);
            parOriginalImage.CopyTo(zipStream);
            zipStream.Close();
            return compressedImage;
        }

        /// <summary>
        /// Inverse application of the algorithm
        /// </summary>
        /// <param name="parCompressedImage">Compressed image stream</param>
        /// <param name="parArguments">List of an arguments (empty)</param>
        /// <returns>Decompressed image stream</returns>
        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            parCompressedImage.Position = 0;
            GZipStream unzipStream = new GZipStream(parCompressedImage, CompressionMode.Decompress, true);
            Stream decompressedImage = new MemoryStream();
            unzipStream.CopyTo(decompressedImage);
            unzipStream.Close();
            return decompressedImage;
        }

        /// <summary>
        /// Returns file extension of algorithm inverse application result
        /// </summary>
        /// <returns>Extension without dot (string)</returns>
        public override string GetFileExtension()
        {
            return "gz";
        }

        /// <summary>
        /// Overrides original ToString() method
        /// </summary>
        /// <returns>Algorithm name</returns>
        public override string ToString()
        {
            return "GZIP";
        }
    }
}
