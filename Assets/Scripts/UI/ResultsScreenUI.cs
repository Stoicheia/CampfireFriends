using Minigame;
using UnityEngine;

namespace UI
{
    public class ResultsScreenUI : MonoBehaviour
    {
        private MinigameResults _results;

        public void Display(MinigameResults results)
        {
            _results = results;
        }
    }
}