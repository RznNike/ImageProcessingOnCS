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
        
        public static GZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GZIP();
            }
            return _instance;
        }

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

        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            parCompressedImage.Position = 0;
            GZipStream unzipStream = new GZipStream(parCompressedImage, CompressionMode.Decompress, true);
            Stream decompressedImage = new MemoryStream();
            unzipStream.CopyTo(decompressedImage);
            unzipStream.Close();
            return decompressedImage;
        }

        public override string GetFileExtension()
        {
            return "gz";
        }

        public override string ToString()
        {
            return "GZIP";
        }
    }
}
