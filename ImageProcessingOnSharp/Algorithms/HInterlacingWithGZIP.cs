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

        /// <summary>
        /// Returns singletone instance
        /// </summary>
        /// <returns>Instance</returns>
        public static HInterlacingWithGZIP GetInstance()
        {
            if (_instance == null)
            {
                _instance = new HInterlacingWithGZIP();
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
            int moddedWidth = originalWidth;
            int moddedHeight = (original.Height + 1) / 2;
            byte lastRowIsOdd = (byte)(originalHeight % 2);
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

            for (int i = 0; i < moddedHeight; i++)
            {
                for (int j = 0; j < moddedWidth; j++)
                {
                    bitsModded[i * moddedWidth + j] = bitsOriginal[i * moddedWidth * 2 + j];
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
            
            int interlacedWidth = interlaced.Width;
            int interlacedHeight = interlaced.Height;

            for (int j = 0; j < interlacedWidth; j++)
            {
                for (int i = 0; i < interlacedHeight - 1; i++)
                {
                    bitsReconstructed[i * interlacedWidth * 2 + j] = bitsInterlaced[i * interlacedWidth + j];
                    bitsReconstructed[(i * 2 + 1) * interlacedWidth + j] =
                        this.FindAverageColor(bitsInterlaced[i * interlacedWidth + j], bitsInterlaced[(i + 1) * interlacedWidth + j]);
                }
                bitsReconstructed[(interlacedHeight - 1) * 2 * interlacedWidth + j] = bitsInterlaced[(interlacedHeight - 1) * interlacedWidth + j];
                if (!lastRowIsOdd)
                {
                    bitsReconstructed[(reconstructedHeight - 1) * interlacedWidth + j] = bitsInterlaced[(interlacedHeight - 1) * interlacedWidth + j];
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
            return "HInterlacing+GZIP";
        }
    }
}
