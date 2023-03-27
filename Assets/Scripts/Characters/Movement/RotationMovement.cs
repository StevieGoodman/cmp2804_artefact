using System;
using cmp2804.Math;
using UnityEngine;

namespace cmp2804.Characters.Movement
{
    class RotationMovement : Movement
    {
        private void Start()
        {
            Target = new Target(transform.forward);
        }

        /// <summary>
        /// Increments the rotation of the character by 1 frame.
        /// </summary>
        protected override void Increment()
        {
            var direction = Target.GetVector(transform) == Vector3.zero ? transform.forward : Target.GetVector(transform);
            var targetRotation = Quaternion.LookRotation(direction);
            var degreesDelta = 360 / MovementState.rotationSpeed * Time.deltaTime;
            var newRotation = Quaternion.RotateTowards(RigidBody.rotation, targetRotation, degreesDelta);
            RigidBody.MoveRotation(newRotation);
        }
    }
}