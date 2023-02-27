using System;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(PatrolState))]
    public class EnemyMovement : SerializedMonoBehaviour, IMovement
    {
        [Title("Asset Fields")]
        [SerializeField] private MovementState movementState;

        [Title("Target Transforms")]
        [SerializeField] public Transform moveTarget;
        [SerializeField] public Transform lookTarget;

        [Title("Class Fields")]
        [ShowInInspector, Required] private IEnemyState _state;
        [SerializeField] public bool canMove = true;

        private void Awake()
        {
            _state = gameObject.GetComponent<PatrolState>();
        }

        public async void FixedUpdate()
        {
            await _state.TickState();
            _state.UpdateState();
            IncrementMovement();
            IncrementRotation();
        }

        public void IncrementMovement()
        {
            if (!canMove) return;
            var moveVector = (moveTarget.position - transform.position).normalized;
            transform.position += moveVector * (movementState.moveSpeed * Time.deltaTime);
        }

        public void IncrementRotation()
        {
            var lookVector = (lookTarget.position - transform.position).normalized;
            if (lookVector == Vector3.zero) return;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(lookVector),
                360 * Time.deltaTime / movementState.rotationSpeed
            );
        }
    }
}