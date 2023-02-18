using Sirenix.OdinInspector;
using UnityEngine;

namespace cmp2804.Scriptable_Objects.Characters
{
    [CreateAssetMenu(fileName = "Character Movement Config", menuName = "ScriptableObjects/Characters/Character Movement Config", order = 0)]
    public class CharacterMovementConfig : SerializedScriptableObject
    {
        [Title("Movement Speed", "The speed at which the character moves, measured in units per second.")]
        [Range(0f, 10f), HideLabel]
        public float movementSpeed = 5f;
        
        [Title("Rotation Speed", "The time it takes to rotate the character 360 degrees, measured in seconds.")]
        [Range(0f, 2f), HideLabel]
        public float rotationSpeed = 0.25f;
    }
}