﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageProcessingOnSharp
{
    public class JPEG : Algorithm
    {
        private static JPEG _instance = null;

        private JPEG()
        {
        }
        
        /// <summary>
        /// Returns singletone instance
        /// </summary>
        /// <returns>Instance</returns>
        public static JPEG GetInstance()
        {
            if (_instance == null)
            {
                _instance = new JPEG();
            }
            return _instance;
        }

        /// <summary>
        /// Forward application of the algorithm
        /// </summary>
        /// <param name="parOriginalImage">Original image stream</param>
        /// <param name="parArguments">List of an arguments (long qualityLevel)</param>
        /// <returns>Compressed image stream</returns>
        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            long qualityLevel = (long)parArguments[0];
            EncoderParameters parameters = new EncoderParameters();
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, qualityLevel);
            ImageCodecInfo[] codecInfos = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegInfo = null;
            foreach (ImageCodecInfo info in codecInfos)
            {
                if (info.FormatDescription.Equals("JPEG"))
                {
                    jpegInfo = info;
                }
            }
            Stream result = new MemoryStream();
            Bitmap tempImage = new Bitmap(parOriginalImage);
            tempImage.Save(result, jpegInfo, parameters);
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
            return "jpeg";
        }

        /// <summary>
        /// Overrides original ToString() method
        /// </summary>
        /// <returns>Algorithm name</returns>
        public override string ToString()
        {
            return "JPEG";
        }
    }
}
