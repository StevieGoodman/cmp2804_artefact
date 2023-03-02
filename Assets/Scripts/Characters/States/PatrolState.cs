using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Characters.States
{
    [RequireComponent(typeof(BasicMovement))]
    [HideMonoScript]
    public class PatrolState : SerializedMonoBehaviour, IEnemyState
    {
        // Components
        private BasicMovement _basicMovement;
        
        // Properties
        [Title("Patrol Nodes", "The nodes the enemy will patrol between.")]
        [OdinSerialize, HideLabel] public Queue<Transform> PatrolNodes { get; private set; }
        private readonly TimeSpan _patrolDelay = new(0, 0, 0, 1);

        // Methods
        private void Awake()
        {
            _basicMovement = gameObject.GetComponent<BasicMovement>();
            SetNextMovementTarget();
        }

        public void UpdateState()
        {
            
        }

        public async Task TickState()
        {
            var distanceToTarget = Vector3.Distance(_basicMovement.MoveTarget.Origin, transform.position);
            if (distanceToTarget > 0.1f) return;
            _basicMovement.CanMove = false;
            SetNextMovementTarget();
            await Task.Delay(_patrolDelay);
            _basicMovement.CanMove = true;
        }
        
        /// <summary>
        /// Sets <see cref="moveTarget" /> & <see cref="lookTarget" /> to the position of the next patrol node.
        /// </summary>
        private void SetNextMovementTarget()
        {
            var nextTarget = PatrolNodes.Dequeue();
            PatrolNodes.Enqueue(nextTarget);
            _basicMovement.MoveTarget = new Target(nextTarget);
            _basicMovement.LookTarget = new Target(nextTarget);
        }
    }
}