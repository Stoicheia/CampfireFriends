using UnityEngine;

namespace Minigame
{
    [CreateAssetMenu(fileName = "Item (proto)", menuName = "Primitive Item", order = 0)]
    public class PrimitiveItem : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
    }

}