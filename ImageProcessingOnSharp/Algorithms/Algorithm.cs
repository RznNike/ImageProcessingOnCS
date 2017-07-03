using System;
using System.Collections.Generic;
using System.IO;

namespace ImageProcessingOnSharp
{
    public abstract class Algorithm
    {
        public abstract Stream CompressImage(Stream parOriginalImage, List<Object> parArguments);

        public abstract Stream DecompressImage(Stream parCompressedImage, List<Object> parArguments);
    }
}
