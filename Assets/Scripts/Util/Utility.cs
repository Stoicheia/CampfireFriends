using System;
using System.Collections.Generic;

namespace Util
{
    public static class Utility
    {
        public static float InverseLerpUnclamped(float a, float b, float v)
        {
            return (v - a) / (b - a);
        }
        public static void Shuffle<T>(List<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            int n = list.Count;
            while (n > 1)
            {
                int k = UnityEngine.Random.Range(0, n--);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}