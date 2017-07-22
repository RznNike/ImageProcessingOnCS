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
        
        public static TIFF GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TIFF();
            }
            return _instance;
        }

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
                BitmapSource originalBMsource =
                    System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        original.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                WriteableBitmap writableBMoriginal = new WriteableBitmap(originalBMsource);
                encoder.Frames.Add(BitmapFrame.Create(writableBMoriginal));
                encoder.Save(result);
            }
            
            return result;
        }

        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            return parCompressedImage;
        }

        public override string GetFileExtension()
        {
            return "tif";
        }

        public override string ToString()
        {
            return "TIFF";
        }
    }
}
