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

        /// <summary>
        /// Returns singletone instance
        /// </summary>
        /// <returns>Instance</returns>
        public static XInterlacingWithGZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new XInterlacingWithGZIP();
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
            int originalWidth = original.Width;
            int originalHeight = original.Height;
            int moddedWidth = (originalWidth + 1) / 2;
            int moddedHeight = originalHeight;
            byte lastColumnIsOdd = (byte)(originalWidth % 2);
            Bitmap modded = new Bitmap(moddedWidth, moddedHeight, original.PixelFormat);

            BitmapData bdOriginal = original.LockBits(new Rectangle(0, 0, originalWidth, originalHeight),
                                                      ImageLockMode.ReadOnly,
                                                      PixelFormat.Format32bppArgb);
            int[ ] bitsOriginal = new int[originalWidth * originalHeight];
            Marshal.Copy(bdOriginal.Scan0, bitsOriginal, 0, bitsOriginal.Length);
            // --/--
            BitmapData bdModded = modded.LockBits(new Rectangle(0, 0, moddedWidth, moddedHeight),
                                                  ImageLockMode.ReadWrite,
                                                  PixelFormat.Format32bppArgb);
            int[ ] bitsModded = new int[moddedWidth * moddedHeight];
            Marshal.Copy(bdModded.Scan0, bitsModded, 0, bitsModded.Length);

            for (int i = 0; i < moddedHeight; i += 2)
            {
                for (int j = 0; j < moddedWidth; j++)
                {
                    bitsModded[i * moddedWidth + j] = bitsOriginal[i * moddedWidth * 2 + j * 2];
                    if ((j * 2 + 1)< originalWidth)
                    {
                        bitsModded[(i + 1) * moddedWidth + j] = bitsOriginal[(i + 1) * moddedWidth * 2 + j * 2 + 1];
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

        /// <summary>
        /// Inverse application of the algorithm
        /// </summary>
        /// <param name="parCompressedImage">Compressed image stream</param>
        /// <param name="parArguments">List of an arguments (ImageFormat finalFormat)</param>
        /// <returns>Decompressed image stream</returns>
        public override Stream DecompressImage(Stream parCompressedImage, List<object> parArguments)
        {
            GZIP gzip = GZIP.GetInstance();
            Stream interlacedImage = gzip.DecompressImage(parCompressedImage, new List<object>());

            interlacedImage.Position = interlacedImage.Length - 1;
            bool lastColumnIsOdd = interlacedImage.ReadByte() > 0;
            interlacedImage.SetLength(interlacedImage.Length - 1);

            Bitmap interlaced = new Bitmap(interlacedImage);
            int interlacedWidth = interlaced.Width;
            int interlacedHeight = interlaced.Height;
            int reconstructedWidth = interlacedWidth * 2;
            int reconstructedHeight = interlacedHeight;
            if (lastColumnIsOdd)
            {
                reconstructedWidth--;
            }

            Bitmap reconstructed = new Bitmap(reconstructedWidth, reconstructedHeight, interlaced.PixelFormat);

            BitmapData bdInterlaced = interlaced.LockBits(new Rectangle(0, 0, interlacedWidth, interlacedHeight),
                                                          ImageLockMode.ReadOnly,
                                                          PixelFormat.Format32bppArgb);
            int[ ] bitsInterlaced = new int[interlacedWidth * interlacedHeight];
            Marshal.Copy(bdInterlaced.Scan0, bitsInterlaced, 0, bitsInterlaced.Length);
            // --/--
            BitmapData bdReconstructed = reconstructed.LockBits(new Rectangle(0, 0, reconstructedWidth, reconstructedHeight),
                                                                ImageLockMode.ReadWrite,
                                                                PixelFormat.Format32bppArgb);
            int[ ] bitsReconstructed = new int[reconstructedWidth * reconstructedHeight];
            Marshal.Copy(bdReconstructed.Scan0, bitsReconstructed, 0, bitsReconstructed.Length);

            for (int i = 0; i < interlacedHeight; i += 2)
            {
                for (int j = 0; j < interlacedWidth; j++)
                {
                    bitsReconstructed[i * reconstructedWidth + j * 2] = bitsInterlaced[i * interlacedWidth + j];
                    if ((j * 2 + 1) < reconstructedWidth)
                    {
                        bitsReconstructed[(i + 1) * reconstructedWidth + j * 2 + 1] = bitsInterlaced[(i + 1) * interlacedWidth + j];
                    }
                }
            }
            for (int i = 0; i < interlacedHeight; i++)
            {
                int offset = (i + 1) % 2;
                for (int j = offset; j < reconstructedWidth; j += 2)
                {
                    List<int> pixels = new List<int>();
                    if (i > 0)
                    {
                        pixels.Add(bitsReconstructed[(i - 1) * reconstructedWidth + j]);
                    }
                    if ((i + 1) < interlacedHeight)
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
            return "XInterlacing+GZIP";
        }
    }
}
