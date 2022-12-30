using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private Image _fillImg;

        public void SetProgress(float t)
        {
            _fillImg.fillAmount = t;
        }
    }
}