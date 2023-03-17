using cmp2804.Math;
using UnityEngine;

namespace cmp2804.Characters.Movement
{
    public class PositionMovement : Movement
    {
        private void Start()
        {
            Target = new Target(gameObject);
        }

        /// <summary>
        /// Increments the position of the character by 1 frame.
        /// </summary>
        protected override void Increment()
        {
            var distance = MovementState.moveSpeed * Time.deltaTime;
            var offset = Target.GetVector(transform) * distance;
            RigidBody.MovePosition(RigidBody.position + offset);
        }
    }
}