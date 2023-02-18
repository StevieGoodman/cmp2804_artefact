using System;
using cmp2804.Scriptable_Objects.Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(Transform), typeof(Rigidbody))]
    [HideMonoScript]
    public class PlayerCharacterController : SerializedMonoBehaviour, ICharacterController {
        
        [Title("Character Movement Config", "The configuration for the character movement.")]
        [SerializeField, Required, HideLabel, InlineEditor]
        private CharacterMovementConfig characterMovementConfig;
        
        [Space]
        
        [Title("Component Fields", "The components required for the character controller.")]
        [SerializeField, Required]
        private Rigidbody rigidBody;
        
        [Space]
        
        [Title("Move Direction", "The direction the character is moving towards.")]
        [SerializeField, HideLabel, ReadOnly]
        private Vector3 moveDirection = Vector3.zero;

        private void Update()
        {
            IncrementMovement();
            IncrementRotation();
        }

        public void IncrementMovement()
        {
            rigidBody.MovePosition(rigidBody.position +
                                   moveDirection * (Time.deltaTime * characterMovementConfig.movementSpeed));
        }

        public void IncrementRotation()
        {
            if (moveDirection == Vector3.zero) return;
            var targetRotation = Quaternion.LookRotation(moveDirection);
            rigidBody.MoveRotation(
                Quaternion.RotateTowards(
                    rigidBody.rotation, 
                    targetRotation,
                    360 * Time.deltaTime / characterMovementConfig.rotationSpeed)
                );

        }
        
        public void SetMoveDirection(InputAction.CallbackContext context)
        {
            var direction = (Vector3)context.ReadValue<Vector2>();
            moveDirection = new Vector3(direction.x, 0, direction.y);
        }
    }
}
