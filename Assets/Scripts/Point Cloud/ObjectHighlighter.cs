using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace cmp2804.Point_Cloud
{
    public class ObjectHighlighter : SerializedMonoBehaviour
    {
        [OdinSerialize] public Color highlightColour;
    }
}