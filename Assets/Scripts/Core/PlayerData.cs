using UnityEngine;

namespace Core
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerData Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
    }
}