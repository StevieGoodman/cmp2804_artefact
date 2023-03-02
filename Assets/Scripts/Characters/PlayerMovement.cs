using Sirenix.OdinInspector;
using UnityEngine;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(Transform), typeof(Rigidbody))]
    [HideMonoScript]
    public class PlayerMovement : SerializedMonoBehaviour, IMovement
    {
        [Title("Component Fields", "The components required for the character controller.")]
        [SerializeField]
        internal Rigidbody rigidBody;

        [field: Space]
        
        [field: Title("Movement State", "The behaviour defining the player's movement.")]
        [field: SerializeField, Required, HideLabel, InlineEditor]   
        public MovementState MovementState { get; set; }

        [field: Space] 
        
        [field: Title("Move Direction", "The direction the character is moving towards.")]
        [field: SerializeField, HideLabel, ReadOnly]
        public Vector3 MoveDirection { get; set; } = Vector3.zero;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            IncrementMovement();
            IncrementRotation();
        }

        public void IncrementMovement()
        {
            var newPosition = rigidBody.position + 
                              MoveDirection * 
                              (Time.deltaTime * MovementState.moveSpeed);
            rigidBody.MovePosition(newPosition);
        }

        public void IncrementRotation()
        {
            if (MoveDirection == Vector3.zero) return;
            var targetRotation = Quaternion.LookRotation(MoveDirection);
            var maximumAngle = 360 * Time.deltaTime / MovementState.rotationSpeed;
            var newRotation = Quaternion.RotateTowards(
                rigidBody.rotation, 
                targetRotation,
                maximumAngle);
            rigidBody.MoveRotation(newRotation);
        }
        
        public void SetMoveDirection(Vector3 direction)
        {
            MoveDirection = direction;
        }

        public void SetMovementState(MovementState newState)
        {
            MovementState = newState;
        }
    }
}
