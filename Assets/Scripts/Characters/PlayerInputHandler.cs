using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(PlayerMovementController))]
    [HideMonoScript]
    public class PlayerInputHandler : SerializedMonoBehaviour
    {
        [Title("Component Fields", "The components required for the character controller.")] [SerializeField] [Required]
        private PlayerMovementController movementController;

        [Title("Movement States", "The possible states the player can move in.")] [SerializeField] [Required]
        private MovementState crawlState;

        [SerializeField] [Required] private MovementState crouchState;

        [SerializeField] [Required] private MovementState walkState;

        [SerializeField] [Required] private MovementState jogState;

        public void SetMoveDirection(InputAction.CallbackContext context)
        {
            var direction = (Vector3)context.ReadValue<Vector2>();
            movementController.SetMoveDirection(new Vector3(direction.x, 0, direction.y));
        }

        public void Crawl(InputAction.CallbackContext context)
        {
            SetMovementState(context, crawlState);
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            SetMovementState(context, crouchState);
        }

        public void Jog(InputAction.CallbackContext context)
        {
            SetMovementState(context, jogState);
        }

        private void SetMovementState(InputAction.CallbackContext context, MovementState movementState)
        {
            if (context.performed)
                movementController.SetMovementState(movementState);
            else if (context.canceled)
                movementController.SetMovementState(walkState);
        }
    }
}