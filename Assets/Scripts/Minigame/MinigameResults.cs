using System.Collections.Generic;

namespace Minigame
{
    public struct MinigameResults
    {
        public Dictionary<ItemData, int> GoodItemScores;
        public int BadItemScore;

        public MinigameResults(Dictionary<ItemData, int> dict, int bad)
        {
            GoodItemScores = dict;
            BadItemScore = bad;
        }
    }
}