using System;
using System.Collections.Generic;
using System.IO;

namespace ImageProcessingOnSharp
{
    /// <summary>
    /// Abstract algorithm class
    /// </summary>
    public abstract class Algorithm
    {
        /// <summary>
        /// Forward application of the algorithm
        /// </summary>
        /// <param name="parOriginalImage">Original image stream</param>
        /// <param name="parArguments">List of an arguments</param>
        /// <returns>Compressed image stream</returns>
        public abstract Stream CompressImage(Stream parOriginalImage, List<Object> parArguments);

        /// <summary>
        /// Inverse application of the algorithm
        /// </summary>
        /// <param name="parCompressedImage">Compressed image stream</param>
        /// <param name="parArguments">List of an arguments</param>
        /// <returns>Decompressed image stream</returns>
        public abstract Stream DecompressImage(Stream parCompressedImage, List<Object> parArguments);

        /// <summary>
        /// Returns file extension of algorithm inverse application result
        /// </summary>
        /// <returns>Extension without dot (string)</returns>
        public abstract String GetFileExtension();
    }
}
