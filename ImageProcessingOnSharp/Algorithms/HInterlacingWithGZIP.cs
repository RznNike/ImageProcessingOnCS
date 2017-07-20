using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace ImageProcessingOnSharp
{
    public class HInterlacingWithGZIP : Algorithm
    {
        private static HInterlacingWithGZIP _instance = null;

        private HInterlacingWithGZIP()
        {
        }
        
        public static HInterlacingWithGZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new HInterlacingWithGZIP();
            }
            return _instance;
        }

        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            Bitmap original = new Bitmap(parOriginalImage);
            byte lastRowIsOdd = (byte)(original.Height % 2);
            Bitmap modded = new Bitmap(original.Width, (original.Height + 1) / 2, original.PixelFormat);

            BitmapData bdOriginal = original.LockBits(new Rectangle(0, 0, original.Width, original.Height),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb);
            int[ ] bitsOriginal = new int[bdOriginal.Stride / 4 * bdOriginal.Height];
            Marshal.Copy(bdOriginal.Scan0, bitsOriginal, 0, bitsOriginal.Length);
            // --/--
            BitmapData bdModded = modded.LockBits(new Rectangle(0, 0, modded.Width, modded.Height),
                                                  ImageLockMode.ReadWrite,
                                                  PixelFormat.Format32bppArgb);
            int[ ] bitsModded = new int[bdModded.Stride / 4 * bdModded.Height];
            Marshal.Copy(bdModded.Scan0, bitsModded, 0, bitsModded.Length);

            for (int i = 0; i < modded.Width; i++)
            {
                for (int j = 0; j < modded.Height; j++)
                {
                    bitsModded[i + bdModded.Stride / 4 * j] = bitsOriginal[i + bdModded.Stride / 2 * j];
                }
            }
            Marshal.Copy(bitsModded, 0, bdModded.Scan0, bitsModded.Length);
            original.UnlockBits(bdOriginal);
            modded.UnlockBits(bdModded);

            Stream interlacedImage = new MemoryStream();
            ImageFormat format = (ImageFormat)parArguments[1];
            modded.Save(interlacedImage, format);
            interlacedImage.WriteByte(lastRowIsOdd);

            GZIP gzip = GZIP.GetInstance();
            int compressionLevel = (int)parArguments[0];
            Stream compressedImage = gzip.CompressImage(interlacedImage, new List<object>() { compressionLevel });

            return compressedImage;
        }

        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            GZIP gzip = GZIP.GetInstance();
            Stream interlacedImage = gzip.DecompressImage(parCompressedImage, new List<object>());

            interlacedImage.Position = interlacedImage.Length - 1;
            bool lastRowIsOdd = interlacedImage.ReadByte() > 0;
            interlacedImage.SetLength(interlacedImage.Length - 1);

            Bitmap interlaced = new Bitmap(interlacedImage);
            int reconstructedWidth = interlaced.Width;
            int reconstructedHeight = interlaced.Height * 2;
            if (lastRowIsOdd)
            {
                reconstructedHeight--;
            }

            Bitmap reconstructed = new Bitmap(reconstructedWidth, reconstructedHeight, interlaced.PixelFormat);

            BitmapData bdInterlaced = interlaced.LockBits(new Rectangle(0, 0, interlaced.Width, interlaced.Height),
                                                          ImageLockMode.ReadOnly,
                                                          PixelFormat.Format32bppArgb);
            int[ ] bitsInterlaced = new int[bdInterlaced.Stride / 4 * bdInterlaced.Height];
            Marshal.Copy(bdInterlaced.Scan0, bitsInterlaced, 0, bitsInterlaced.Length);
            // --/--
            BitmapData bdReconstructed = reconstructed.LockBits(new Rectangle(0, 0, reconstructed.Width, reconstructed.Height),
                                                                ImageLockMode.ReadWrite,
                                                                PixelFormat.Format32bppArgb);
            int[ ] bitsReconstructed = new int[bdReconstructed.Stride / 4 * bdReconstructed.Height];
            Marshal.Copy(bdReconstructed.Scan0, bitsReconstructed, 0, bitsReconstructed.Length);
            
            int width = interlaced.Width;
            int height = interlaced.Height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    bitsReconstructed[i + width * 2 * j] = bitsInterlaced[i + width * j];
                    bitsReconstructed[i + width * (2 * j + 1)] =
                        this.FindAverageColor(bitsInterlaced[i + width * j], bitsInterlaced[i + width * (j + 1)]);
                }
                bitsReconstructed[i + width * 2 * (height - 1)] = bitsInterlaced[i + width * (height - 1)];
                if (!lastRowIsOdd)
                {
                    bitsReconstructed[i + width * (2 * (height - 1) + 1)] = bitsInterlaced[i + width * (height - 1)];
                }
            }
            Marshal.Copy(bitsReconstructed, 0, bdReconstructed.Scan0, bitsReconstructed.Length);
            interlaced.UnlockBits(bdInterlaced);
            reconstructed.UnlockBits(bdReconstructed);

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
            return "HInterlacing+GZIP";
        }
    }
}
