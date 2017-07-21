using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace ImageProcessingOnSharp
{
    public class XInterlacingWithGZIP : Algorithm
    {
        private static XInterlacingWithGZIP _instance = null;

        private XInterlacingWithGZIP()
        {
        }
        
        public static XInterlacingWithGZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new XInterlacingWithGZIP();
            }
            return _instance;
        }

        public override Stream CompressImage(Stream parOriginalImage, List<object> parArguments)
        {
            Bitmap original = new Bitmap(parOriginalImage);
            byte lastColumnIsOdd = (byte)(original.Width % 2);
            Bitmap modded = new Bitmap((original.Width + 1) / 2, original.Height, original.PixelFormat);

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

            int width = modded.Width;
            int height = modded.Height;
            int originalWidth = original.Width;

            for (int i = 0; i < height; i += 2)
            {
                for (int j = 0; j < width; j++)
                {
                    bitsModded[i * width + j] = bitsOriginal[i * width * 2 + j * 2];
                    if ((j * 2 + 1)< originalWidth)
                    {
                        bitsModded[(i + 1) * width + j] = bitsOriginal[(i + 1) * width * 2 + j * 2 + 1];
                    }
                }
            }
            Marshal.Copy(bitsModded, 0, bdModded.Scan0, bitsModded.Length);
            original.UnlockBits(bdOriginal);
            modded.UnlockBits(bdModded);

            Stream interlacedImage = new MemoryStream();
            ImageFormat format = (ImageFormat)parArguments[1];
            modded.Save(interlacedImage, format);
            interlacedImage.WriteByte(lastColumnIsOdd);

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
            bool lastColumnIsOdd = interlacedImage.ReadByte() > 0;
            interlacedImage.SetLength(interlacedImage.Length - 1);

            Bitmap interlaced = new Bitmap(interlacedImage);
            int reconstructedWidth = interlaced.Width * 2;
            int reconstructedHeight = interlaced.Height;
            if (lastColumnIsOdd)
            {
                reconstructedWidth--;
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

            for (int i = 0; i < height; i += 2)
            {
                for (int j = 0; j < width; j++)
                {
                    bitsReconstructed[i * reconstructedWidth + j * 2] = bitsInterlaced[i * width + j];
                    if ((j * 2 + 1) < reconstructedWidth)
                    {
                        bitsReconstructed[(i + 1) * reconstructedWidth + j * 2 + 1] = bitsInterlaced[(i + 1) * width + j];
                    }
                }
            }
            for (int i = 0; i < height; i++)
            {
                int offset = (i + 1) % 2;
                for (int j = offset; j < reconstructedWidth; j += 2)
                {
                    List<int> pixels = new List<int>();
                    if (i > 0)
                    {
                        pixels.Add(bitsReconstructed[(i - 1) * reconstructedWidth + j]);
                    }
                    if ((i + 1) < height)
                    {
                        pixels.Add(bitsReconstructed[(i + 1) * reconstructedWidth + j]);
                    }
                    if (j > 0)
                    {
                        pixels.Add(bitsReconstructed[i * reconstructedWidth + j - 1]);
                    }
                    if ((j + 1) < reconstructedWidth)
                    {
                        pixels.Add(bitsReconstructed[i * reconstructedWidth + j + 1]);
                    }
                    bitsReconstructed[i * reconstructedWidth + j] = FindAverageColor(pixels);
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

        private int FindAverageColor(List<int> pixels)
        {
            int count = pixels.Count;
            int A = 0;
            int R = 0;
            int G = 0;
            int B = 0;
            for (int i = 0; i < count; i++)
            {
                Color color = Color.FromArgb(pixels[i]);
                A += color.A;
                R += color.R;
                G += color.G;
                B += color.B;
            }

            Color resultColor = Color.FromArgb(A / count, R / count, G / count, B / count);
            return resultColor.ToArgb();
        }

        public override string GetFileExtension()
        {
            return "bmp";
        }

        public override string ToString()
        {
            return "XInterlacing+GZIP";
        }
    }
}
