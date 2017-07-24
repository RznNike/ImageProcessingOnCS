using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessingOnSharp
{
    public static class HaarWavelet
    {
        private const double w0 = 0.5;
        private const double w1 = -0.5;
        private const double s0 = 0.5;
        private const double s1 = 0.5;

        public static void ApplyTransform(ref Bitmap refImage, bool parForwardDirection, int parLevels)
        {
            int width = refImage.Width;
            int height = refImage.Height;
            double[ , ] redMatrix = new double[height, width];
            double[ , ] greenMatrix = new double[height, width];
            double[ , ] blueMatrix = new double[height, width];

            BitmapData bitmapData = refImage.LockBits(new Rectangle(0, 0, refImage.Width, refImage.Height),
                                                      ImageLockMode.ReadWrite,
                                                      PixelFormat.Format32bppArgb);
            int[ ] bitmapBits = new int[bitmapData.Stride / 4 * bitmapData.Height];
            Marshal.Copy(bitmapData.Scan0, bitmapBits, 0, bitmapBits.Length);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color color = Color.FromArgb(bitmapBits[i * width + j]);
                    redMatrix[i, j] = Scale(0, 255, -1, 1, color.R);
                    greenMatrix[i, j] = Scale(0, 255, -1, 1, color.G);
                    blueMatrix[i, j] = Scale(0, 255, -1, 1, color.B);
                }
            }

            if (parForwardDirection)
            {
                ForwardWaveletTransformation(redMatrix, parLevels);
                ForwardWaveletTransformation(greenMatrix, parLevels);
                ForwardWaveletTransformation(blueMatrix, parLevels);
            }
            else
            {
                InverseWaveletTransformation(redMatrix, parLevels);
                InverseWaveletTransformation(greenMatrix, parLevels);
                InverseWaveletTransformation(blueMatrix, parLevels);
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int R = (int)Scale(-1, 1, 0, 255, redMatrix[i, j]);
                    int G = (int)Scale(-1, 1, 0, 255, greenMatrix[i, j]);
                    int B = (int)Scale(-1, 1, 0, 255, blueMatrix[i, j]);
                    bitmapBits[i * width + j] = Color.FromArgb(R, G, B).ToArgb();
                }
            }

            Marshal.Copy(bitmapBits, 0, bitmapData.Scan0, bitmapBits.Length);
            refImage.UnlockBits(bitmapData);
        }

        private static double Scale(double parOriginalMin, double parOriginalMax, double parNewMin, double parNewMax, double parValue)
        {
            double value = (parNewMax - parNewMin) / (parOriginalMax - parOriginalMin) * (parValue - parOriginalMin) + parNewMin;
            if (value > parNewMax)
            {
                value = parNewMax;
            }
            else if (value < parNewMin)
            {
                value = parNewMin;
            }
            return value;
        }

        private static void ForwardWaveletTransformation(double[ ] data)
        {
            double[ ] temp = new double[data.Length];

            int h = data.Length >> 1;
            for (int i = 0; i < h; i++)
            {
                int k = (i << 1);
                temp[i] = data[k] * s0 + data[k + 1] * s1;
                temp[i + h] = data[k] * w0 + data[k + 1] * w1;
            }

            for (int i = 0; i < data.Length; i++)
                data[i] = temp[i];
        }

        private static void ForwardWaveletTransformation(double[ , ] data, int iterations)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            double[ ] row = new double[cols];
            double[ ] col = new double[rows];

            for (int k = 0; k < iterations; k++)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < row.Length; j++)
                        row[j] = data[i, j];

                    ForwardWaveletTransformation(row);

                    for (int j = 0; j < row.Length; j++)
                        data[i, j] = row[j];
                }

                for (int j = 0; j < cols; j++)
                {
                    for (int i = 0; i < col.Length; i++)
                        col[i] = data[i, j];

                    ForwardWaveletTransformation(col);

                    for (int i = 0; i < col.Length; i++)
                        data[i, j] = col[i];
                }
            }
        }

        private static void InverseWaveletTransformation(double[ ] data)
        {
            double[ ] temp = new double[data.Length];

            int h = data.Length >> 1;
            for (int i = 0; i < h; i++)
            {
                int k = (i << 1);
                temp[k] = (data[i] * s0 + data[i + h] * w0) / w0;
                temp[k + 1] = (data[i] * s1 + data[i + h] * w1) / s0;
            }

            for (int i = 0; i < data.Length; i++)
                data[i] = temp[i];
        }

        private static void InverseWaveletTransformation(double[ , ] data, int iterations)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            double[ ] col = new double[rows];
            double[ ] row = new double[cols];

            for (int l = 0; l < iterations; l++)
            {
                for (int j = 0; j < cols; j++)
                {
                    for (int i = 0; i < rows; i++)
                        col[i] = data[i, j];

                    InverseWaveletTransformation(col);

                    for (int i = 0; i < col.Length; i++)
                        data[i, j] = col[i];
                }

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < row.Length; j++)
                        row[j] = data[i, j];

                    InverseWaveletTransformation(row);

                    for (int j = 0; j < row.Length; j++)
                        data[i, j] = row[j];
                }
            }
        }
    }
}
