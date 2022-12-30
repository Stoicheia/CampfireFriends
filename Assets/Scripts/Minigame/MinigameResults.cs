using System.Collections.Generic;

namespace Minigame
{
    public struct MinigameResults
    {
        public Dictionary<PrimitiveItem, int> GoodItemScores;
        public int BadItemScore;

        public MinigameResults(Dictionary<PrimitiveItem, int> dict, int bad)
        {
            GoodItemScores = dict;
            BadItemScore = bad;
        }
    }
}