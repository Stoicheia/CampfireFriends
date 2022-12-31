using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MinigameScoreUISimple : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void Set(float score)
        {
            _scoreText.text = $"{score:0}";
        }
    }
}