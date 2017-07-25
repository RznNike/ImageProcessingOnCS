using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProcessingOnSharp
{
    /// <summary>
    /// Realization of Haar's wavelet algorithm
    /// </summary>
    public static class HaarWavelet
    {
        /// <summary>
        /// Do a transformation in forward or inverse direction
        /// </summary>
        /// <param name="refImage">Ref to an image</param>
        /// <param name="parForwardDirection">True, if direction is forward</param>
        /// <param name="parLevels">Number of algorithm applications</param>
        public static void ApplyTransform(ref Bitmap refImage, bool parForwardDirection, int parLevels)
        {
            int width = refImage.Width;
            int height = refImage.Height;
            double[ , ] redMatrix = new double[height, width];
            double[ , ] greenMatrix = new double[height, width];
            double[ , ] blueMatrix = new double[height, width];

            BitmapData bitmapData = refImage.LockBits(new Rectangle(0, 0, width, height),
                                                      ImageLockMode.ReadWrite,
                                                      PixelFormat.Format32bppArgb);
            int[ ] bitmapBits = new int[width * height];
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

        private static void ForwardWaveletTransformation(double[ ] parData)
        {
            double[ ] result = new double[parData.Length];

            int halfPosition = parData.Length / 2;
            for (int i = 0; i < halfPosition; i++)
            {
                int ix2 = i * 2;
                result[i] = (parData[ix2] + parData[ix2 + 1]) / 2;
                result[i + halfPosition] = (parData[ix2] - parData[ix2 + 1]) / 2;
            }
            result.CopyTo(parData, 0);
        }

        private static void ForwardWaveletTransformation(double[ , ] parData, int parLevels)
        {
            int rowNumber = parData.GetLength(0);
            int columnNumber = parData.GetLength(1);

            double[ ] row = new double[columnNumber];
            double[ ] column = new double[rowNumber];

            for (int level = 0; level < parLevels; level++)
            {
                for (int i = 0; i < rowNumber; i++)
                {
                    ApplyTransformationToRow(parData, columnNumber, row, i, true);
                }

                for (int j = 0; j < columnNumber; j++)
                {
                    ApplyTransformationToColumn(parData, rowNumber, column, j, true);
                }
            }
        }

        private static void ApplyTransformationToRow(
            double[ , ] parData,
            int parColumnNumber,
            double[ ] parRow,
            int parRowPosition,
            bool parForwardDirection)
        {
            for (int j = 0; j < parColumnNumber; j++)
            {
                parRow[j] = parData[parRowPosition, j];
            }

            if (parForwardDirection)
            {
                ForwardWaveletTransformation(parRow);
            }
            else
            {
                InverseWaveletTransformation(parRow);
            }

            for (int j = 0; j < parColumnNumber; j++)
            {
                parData[parRowPosition, j] = parRow[j];
            }
        }

        private static void ApplyTransformationToColumn(
            double[ , ] parData,
            int parRowNumber,
            double[ ] parColumn,
            int parColumnPosition,
            bool parForwardDirection)
        {
            for (int i = 0; i < parRowNumber; i++)
            {
                parColumn[i] = parData[i, parColumnPosition];
            }

            if (parForwardDirection)
            {
                ForwardWaveletTransformation(parColumn);
            }
            else
            {
                InverseWaveletTransformation(parColumn);
            }

            for (int i = 0; i < parRowNumber; i++)
            {
                parData[i, parColumnPosition] = parColumn[i];
            }
        }

        private static void InverseWaveletTransformation(double[ ] parData)
        {
            double[ ] result = new double[parData.Length];

            int halfPosition = parData.Length / 2;
            for (int i = 0; i < halfPosition; i++)
            {
                int ix2 = i * 2;
                result[ix2] = parData[i] + parData[i + halfPosition];
                result[ix2 + 1] = parData[i] - parData[i + halfPosition];
            }
            result.CopyTo(parData, 0);
        }

        private static void InverseWaveletTransformation(double[ , ] parData, int parLevels)
        {
            int rowNumber = parData.GetLength(0);
            int columnNumber = parData.GetLength(1);

            double[ ] row = new double[columnNumber];
            double[ ] column = new double[rowNumber];

            for (int level = 0; level < parLevels; level++)
            {
                for (int j = 0; j < columnNumber; j++)
                {
                    ApplyTransformationToColumn(parData, rowNumber, column, j, false);
                }

                for (int i = 0; i < rowNumber; i++)
                {
                    ApplyTransformationToRow(parData, columnNumber, row, i, false);
                }
            }
        }
    }
}
