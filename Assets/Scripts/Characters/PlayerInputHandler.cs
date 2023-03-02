using System.Collections.Generic;
using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(Movement))]
    [HideMonoScript]
    public class PlayerInputHandler : SerializedMonoBehaviour
    {
        [Title("Component Fields", "The components required for the character controller.")]
        private Movement _movement;

        [Title("Movement States", "The possible states the player can move in.")]
        [OdinSerialize] private Dictionary<string, MovementState> _movementStates;

        private void Awake()
        {
            _movement = GetComponent<Movement>();
        }

        public void SetMoveDirection(InputAction.CallbackContext context)
        {
            var direction = (Vector3)context.ReadValue<Vector2>();
            var vector3 = new Vector3(direction.x, 0, direction.y);
            _movement.MoveTarget = new Target(vector3);
            _movement.LookTarget = new Target(vector3);
        }

        public void Crawl(InputAction.CallbackContext context)
        {
            SetMovementState(context, "Crawl");
        }
        
        public void Crouch(InputAction.CallbackContext context)
        {
            SetMovementState(context, "Crouch");
        }

        public void Jog(InputAction.CallbackContext context)
        {
            SetMovementState(context, "Jog");
        }

        private void SetMovementState(InputAction.CallbackContext context, string newStateName)
        {
            if (context.canceled)
                newStateName = "Walk"; 
            _movementStates.TryGetValue(newStateName, out var newState);
            _movement.MovementState = newState;
        }
    }
}