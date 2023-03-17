using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AI;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [HideMonoScript]
    class NavmeshMovement : SerializedMonoBehaviour
    {
        // Components
        private NavMeshAgent _agent;
        
        // Properties
        [OdinSerialize] public bool CanMove
        {
            get => _agent.enabled;
            set => _agent.enabled = value;
        }

        public Target MoveTarget
        {
            get => new(_agent.destination);
            set => _agent.SetDestination(value.Origin);
        }
        [OdinSerialize] public Target LookTarget { get; set; }
        [OdinSerialize] public MovementState MovementState { private get; set; }

        // Methods
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            SetupAgent();
            MoveTarget = new Target(gameObject);
            LookTarget = new Target(transform.forward);
        }

        private void Update()
        {
            IncrementMovement();
            IncrementRotation();
        }
        
        private void SetupAgent()
        {
            _agent.speed = MovementState.moveSpeed;
            _agent.angularSpeed = 360 / MovementState.rotationSpeed;
        }
        
        public void IncrementMovement()
        {
            _agent.speed = CanMove switch
            {
                true => MovementState.moveSpeed,
                false => 0
            };
        }

        public void IncrementRotation() {}
    }
}