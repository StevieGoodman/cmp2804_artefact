using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(Movement))]
    [HideMonoScript]
    public class PatrolState : SerializedMonoBehaviour, IEnemyState
    {
        // Components
        private Movement _movement;
        
        // Properties
        [Title("Patrol Nodes", "The nodes the enemy will patrol between.")]
        [OdinSerialize, HideLabel] public Queue<Transform> PatrolNodes { get; private set; }
        private readonly TimeSpan _patrolDelay = new(0, 0, 0, 1);

        // Methods
        private void Awake()
        {
            _movement = gameObject.GetComponent<Movement>();
            SetNextMovementTarget();
        }

        public void UpdateState()
        {
            
        }

        public async Task TickState()
        {
            var distanceToTarget = Vector3.Distance(_movement.MoveTarget.Origin, transform.position);
            if (distanceToTarget > 0.1f) return;
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