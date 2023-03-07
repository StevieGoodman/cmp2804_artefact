using UnityEngine;
using UnityEngine.Serialization;

namespace cmp2804.Point_Cloud
{
    public class ObjectHighlighter : MonoBehaviour
    {
        [FormerlySerializedAs("HighlightColour")] public Color highlightColour;
    }
}