using UnityEngine;
using UnityEngine.Serialization;

namespace SweetSugar.Scripts.System
{
    // [CreateAssetMenu(fileName = "AdditionalSettings", menuName = "AdditionalSettings", order = 1)]
    public class AdditionalSettings : ScriptableObject
    {
        [Header("Multicolor shouldn't spread a jelly if no jelly under")]
        public bool MulticolorSpreadJellyOnlyUnder;
        
        [Header("Striped should stop on undestroyable")]
        public bool StripedStopByUndestroyable;
        
        [Header("Double multicolor item should destroy SolidBlocks")]
        public bool DoubleMulticolorDestroySolid;
    }
}