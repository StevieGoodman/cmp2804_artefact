using System;
using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Characters
{
    [HideMonoScript]
    public class Movement : SerializedMonoBehaviour
    {
        // Properties
        [OdinSerialize] public bool CanMove { get; set; } = true;
        [OdinSerialize] public Target MoveTarget { get; set; }
        [OdinSerialize] public Target LookTarget { get; set; }
        [Required]
        [OdinSerialize] public MovementState MovementState { private get; set; }

        // Methods
        private void Awake()
        {
            MoveTarget = new Target(gameObject);
            LookTarget = new Target(transform.forward);
        }

        private void Update()
        {
            IncrementMovement();
            IncrementRotation();
        }
        
        private void IncrementMovement()
        {
            if (!CanMove) return;
            transform.position += MoveTarget.GetVector(transform) * (MovementState.moveSpeed * Time.deltaTime);
        }

        private void IncrementRotation()
        {
            var lookVector = LookTarget.GetVector(transform);
            if (lookVector == Vector3.zero) return;
            var rotationSpeed = 360 * Time.deltaTime / MovementState.rotationSpeed;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(lookVector),
                rotationSpeed
            );
        }
    }
}