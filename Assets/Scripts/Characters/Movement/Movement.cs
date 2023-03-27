using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Characters.Movement
{
    public abstract class Movement : SerializedMonoBehaviour
    {
        /// <summary>
        /// Whether the character can move.
        /// </summary>
        [OdinSerialize] public bool CanMove { get; set; } = true;

        /// <summary>
        /// The target to look in the direction of.
        /// </summary>
        [OdinSerialize] public Target Target { get; set; }
        
        /// <summary>
        /// The movement state that defines the characteristics of movement.
        /// </summary>
        [OdinSerialize, InlineEditor] public MovementState MovementState { get; set; }

        protected Rigidbody RigidBody;
        
        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }
        
        protected void Update()
        {
            Increment();
        }
        
        /// <summary>
        /// Increments the movement of the character by 1 frame.
        /// </summary>
        protected abstract void Increment();
    }
}