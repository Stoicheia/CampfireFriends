using System.Collections.Generic;
using System.Linq;

namespace Minigame
{
    public struct MinigameResults
    {
        public Dictionary<ItemData, int> GoodItemScores;
        public Dictionary<ItemData, int> GoodItemTotals;
        public int BadItemScore;
        public int BadItemTotal;

        public List<ItemData> GoodItems => GoodItemScores.Select(x => x.Key).ToList();

        public MinigameResults(Dictionary<ItemData, int> dict, Dictionary<ItemData, int> total, int bad, int badTotal)
        {
            GoodItemScores = dict;
            BadItemScore = bad;
            GoodItemTotals = total;
            BadItemTotal = badTotal;
        }

        public float CalculateAccuracyPercent()
        {
            int numerator =  GoodItemScores.Select(x => x.Value).Aggregate((x,y) => x+y)
                + BadItemTotal - BadItemScore;
            int denominator = GoodItemTotals.Select(x => x.Value).Aggregate((x, y) => x + y)
                              + BadItemTotal;
            return (float) 100 * numerator / denominator;
        }
    }
}