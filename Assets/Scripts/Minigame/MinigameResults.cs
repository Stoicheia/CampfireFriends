using System.Collections.Generic;

namespace Minigame
{
    public struct MinigameResults
    {
        public Dictionary<ItemData, int> GoodItemScores;
        public Dictionary<ItemData, int> GoodItemTotals;
        public int BadItemScore;
        public int BadItemTotal;

        public MinigameResults(Dictionary<ItemData, int> dict, Dictionary<ItemData, int> total, int bad, int badTotal)
        {
            GoodItemScores = dict;
            BadItemScore = bad;
            GoodItemTotals = total;
            BadItemTotal = badTotal;
        }
    }
}