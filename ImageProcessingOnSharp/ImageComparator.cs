﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingOnSharp
{
    public static class ImageComparator
    {
        public static double CalculateImagesEquality(Stream parImageOriginal, Stream parImageModded)
        {
            Bitmap original = new Bitmap(parImageOriginal);
            Bitmap modded = new Bitmap(parImageModded);

            BitmapData bdOriginal = original.LockBits(new Rectangle(0, 0, original.Width, original.Height),
                                                      ImageLockMode.ReadWrite,
                                                      PixelFormat.Format32bppArgb);
            int[ ] bitsOriginal = new int[bdOriginal.Stride / 4 * bdOriginal.Height];
            Marshal.Copy(bdOriginal.Scan0, bitsOriginal, 0, bitsOriginal.Length);
            // --/--
            BitmapData bdModded = modded.LockBits(new Rectangle(0, 0, modded.Width, modded.Height),
                                                  ImageLockMode.ReadOnly,
                                                  PixelFormat.Format32bppArgb);
            int[ ] bitsModded = new int[bdModded.Stride / 4 * bdModded.Height];
            Marshal.Copy(bdModded.Scan0, bitsModded, 0, bitsModded.Length);

            int pixNumber = bitsOriginal.Length;
            double result = 0;
            for (int i = 0; i < pixNumber; i++)
            {/*
                Color color1 = Color.FromArgb(bitsOriginal[i]);
                Color color2 = Color.FromArgb(bitsModded[i]);
                double delta = (Math.Abs(color1.R - color2.R) + Math.Abs(color1.G - color2.G) + Math.Abs(color1.B - color2.B)) / (255 * 3.0);
                result += delta;*/
                result += Math.Abs(bitsOriginal[i] - bitsModded[i]) / (int.MaxValue / 255.0);
            }
            result = (pixNumber - result) / pixNumber;

            original.UnlockBits(bdModded);
            modded.UnlockBits(bdOriginal);

            return result;
        }
    }
}
