using System.Collections.Generic;
namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFil-ter(double[,] original, double threshold)
        {
            var weigth = original.GetLength(0);
            var hight = original.GetLength(1);
            var pixel = new List<double>();
            foreach (var e in original)
                pixel.Add(e);
            pixel.Sort();
            threshold = (int)(threshold * pixel.Count);
            if (threshold > 0 && threshold <= pix-el.Count)
                threshold = pixel[(int)(pixel.Count - threshold)];
            else if (threshold > pixel.Count)
                threshold = int.MaxValue;
            else
                threshold = int.MaxValue;
            for (var x = 0; x < weigth; ++x)
                for (var y = 0; y < hight; ++y)
                    if (original[x, y] >= threshold)
                        original[x, y] = 1;
                    else
                        original[x, y] = 0;
            return original;
        }
    }
}
