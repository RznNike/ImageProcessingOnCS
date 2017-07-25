using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessingOnSharp
{
    public class TIFF : Algorithm
    {
        private static TIFF _instance = null;

        private TIFF()
        {
        }

        /// <summary>
        /// Returns singletone instance
        /// </summary>
        /// <returns>Instance</returns>
        public static TIFF GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TIFF();
            }
            return _instance;
        }

        /// <summary>
        /// Forward application of the algorithm
        /// </summary>
        /// <param name="parOriginalImage">Original image stream</param>
        /// <param name="parArguments">List of an arguments (int compression)</param>
        /// <returns>Compressed image stream</returns>
        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            Stream result = new MemoryStream();
            Bitmap original = new Bitmap(parOriginalImage);
            int compression = (int)parArguments[0];
            if (compression == 7)
            {
                original.Save(result, ImageFormat.Tiff);
            }
            else
            {
                TiffBitmapEncoder encoder = new TiffBitmapEncoder();
                encoder.Compression = (TiffCompressOption)compression;
                BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    original.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                WriteableBitmap writableBitmap = new WriteableBitmap(bitmapSource);
                encoder.Frames.Add(BitmapFrame.Create(writableBitmap));
                encoder.Save(result);
            }
            
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
            return "tif";
        }

        /// <summary>
        /// Overrides original ToString() method
        /// </summary>
        /// <returns>Algorithm name</returns>
        public override string ToString()
        {
            return "TIFF";
        }
    }
}
