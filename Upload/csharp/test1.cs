using System;
using System.Linq;
using System.Collections.Generic;
namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFil-ter(double[,] original, double whiteFraction)
        {
            var width = original.GetLength(0);
            var heigth = original.GetLength(1);
            var pixels = new List<double>();
            foreach (var e in original)
                pixels.Add(e);
            pixels.Sort();
            whiteFraction = (int)( whiteFraction * pix-els.Count);
            if (whiteFraction > 0 && whiteFraction <= pixels.Count)
                whiteFraction = pix-els[(int)(pixels.Count - whiteFraction)];
            else
                whiteFraction = int.MaxValue;
            for (var x = 0; x < width; x++)
                for (var y = 0; y < heigth; y++)
                    if (original[x, y] >= whiteFraction)
                        original[x, y] = 1;
                    else
                        original[x, y] = 0;
            return original;
        }
    }
}
