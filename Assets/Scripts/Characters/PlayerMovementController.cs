using Sirenix.OdinInspector;
using UnityEngine;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(Transform), typeof(Rigidbody))]
    [HideMonoScript]
    public class PlayerMovementController : SerializedMonoBehaviour, IMovementController
    {
        [Title("Component Fields", "The components required for the character controller.")] [SerializeField] [Required]
        private Rigidbody rigidBody;

        [Space]
        [Title("Movement State", "The behaviour defining the player's movement.")]
        [SerializeField]
        [Required]
        [HideLabel]
        [InlineEditor]
        private MovementState movementState;

        [Space]
        [Title("Move Direction", "The direction the character is moving towards.")]
        [SerializeField]
        [HideLabel]
        [ReadOnly]
        private Vector3 moveDirection = Vector3.zero;

        private void Update()
        {
            IncrementMovement();
            IncrementRotation();
        }

        public void IncrementMovement()
        {
            var newPosition = rigidBody.position +
                              moveDirection *
                              (Time.deltaTime * movementState.moveSpeed);
            rigidBody.MovePosition(newPosition);
        }

        public void IncrementRotation()
        {
            if (moveDirection == Vector3.zero) return;
            var targetRotation = Quaternion.LookRotation(moveDirection);
            rigidBody.MoveRotation(
                Quaternion.RotateTowards(
                    rigidBody.rotation,
                    targetRotation,
                    360 * Time.deltaTime / movementState.rotationSpeed)
            );
        }

        public void SetMoveDirection(Vector3 direction)
        {
            moveDirection = direction;
        }

        public void SetMovementState(MovementState newState)
        {
            movementState = newState;
        }
    }
}