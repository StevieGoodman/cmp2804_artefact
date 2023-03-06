using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(Rigidbody))]
    [HideMonoScript]
    public class BasicMovement : SerializedMonoBehaviour
    {
        // Properties
        [OdinSerialize] public bool CanMove { get; set; } = true;
        [OdinSerialize] public Target MoveTarget { get; set; }
        [OdinSerialize] public Target LookTarget { get; set; }
        [Required]
        [OdinSerialize] public MovementState MovementState { private get; set; }

        // Fields
        private Rigidbody _rigidbody;
        
        // Methods
        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            MoveTarget = new Target(gameObject);
            LookTarget = new Target(transform.forward);
        }

        private void Update()
        {
            IncrementMovement();
            IncrementRotation();
        }
        
        public void IncrementMovement()
        {
            if (!CanMove) return;
            var movementOffset = MoveTarget.GetVector(transform) * (MovementState.moveSpeed * Time.deltaTime);
            _rigidbody.MovePosition(_rigidbody.position + movementOffset);
        }

        public void IncrementRotation()
        {
            var lookVector = LookTarget.GetVector(transform);
            if (lookVector == Vector3.zero) return;
            var rotationSpeed = 360 * Time.deltaTime / MovementState.rotationSpeed;
            var newRotation = Quaternion.RotateTowards(
                _rigidbody.rotation,
                Quaternion.LookRotation(lookVector),
                rotationSpeed
            );
            _rigidbody.MoveRotation(newRotation);
        }
    }
}