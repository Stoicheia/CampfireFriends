using UnityEngine;

namespace Minigame
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class PrimitiveItem : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
    }

}