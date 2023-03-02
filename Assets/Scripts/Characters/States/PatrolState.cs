using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AI;

namespace cmp2804.Characters.States
{
    [RequireComponent(typeof(NavmeshMovement))]
    [HideMonoScript]
    public class PatrolState : SerializedMonoBehaviour, IEnemyState
    {
        // Components
        private IMovement _movement;
        private NavMeshAgent _agent;
        
        // Properties
        [Title("Patrol Nodes", "The nodes the enemy will patrol between.")]
        [OdinSerialize, HideLabel] public Queue<Transform> PatrolNodes { get; private set; }
        private readonly TimeSpan _patrolDelay = new(0, 0, 0, 1);

        // Methods
        private void Awake()
        {
            _movement = gameObject.GetComponent<IMovement>();
            _agent = gameObject.GetComponent<NavMeshAgent>();
            SetNextMovementTarget();
        }

        public void UpdateState()
        {
            
        }

        public async Task TickState()
        {
            if (_agent.remainingDistance != 0) return;
            _movement.CanMove = false;
            SetNextMovementTarget();
            await Task.Delay(_patrolDelay);
            _movement.CanMove = true;
        }
        
        /// <summary>
        /// Sets <see cref="moveTarget" /> & <see cref="lookTarget" /> to the position of the next patrol node.
        /// </summary>
        private void SetNextMovementTarget()
        {
            var nextTarget = PatrolNodes.Dequeue();
            PatrolNodes.Enqueue(nextTarget);
            _movement.MoveTarget = new Target(nextTarget);
            _movement.LookTarget = new Target(nextTarget);
        }
    }
}