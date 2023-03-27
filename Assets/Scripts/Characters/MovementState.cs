using Sirenix.OdinInspector;
using UnityEngine;

namespace cmp2804.Characters
{
    [CreateAssetMenu(fileName = "Movement state", menuName = "ScriptableObjects/Characters/Movement State", order = 0)]
    public class MovementState : SerializedScriptableObject
    {
        [Title("Movement Speed", "The speed at which the character moves, measured in units per second.")]
        [SerializeField]
        [Range(0, 10)]
        public float moveSpeed;

        [Title("Rotation Speed", "The time it takes to rotate the character 360 degrees, measured in seconds.")]
        [SerializeField]
        [Range(0, 4)]
        public float rotationSpeed;

        [Title("Noise Level", "The noise level of the character when walking.")] [SerializeField] [Range(0, 10)]
        public float noiseLevel;

        [Title("Detection Radius", "The radius the character can be detected within.")] [SerializeField] [Range(0, 10)]
        public float detectionRadius;
    }
}