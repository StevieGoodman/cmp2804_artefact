using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cmp2804.Characters.Detection;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AI;

namespace cmp2804.Characters.States
{
    [RequireComponent(typeof(EnemyController))]
    [HideMonoScript]
    public class Patrol : EnemyState
    {
        // Components
        private NavMeshAgent _agent;
        private EnemyController _enemyController;
        private Chase _chase;
        private Sight _sight;

        // Properties
        [Title("Patrol Nodes", "The nodes the enemy will patrol between.")]
        [OdinSerialize, HideLabel] public Queue<Transform> PatrolNodes { get; private set; }
        private readonly TimeSpan _patrolDelay = new(0, 0, 0, 1);

        // Methods
        private void Awake()
        {
            _agent = gameObject.GetComponent<NavMeshAgent>();
            _enemyController = gameObject.GetComponent<EnemyController>();
            _chase = gameObject.GetComponent<Chase>();
            _sight = gameObject.GetComponent<Sight>();
            SetNextMovementTarget();
        }

        public override void UpdateState()
        {
            if (_sight.PlayerInSight())
                _enemyController.State = _chase;
        }

        public override async Task TickState()
        {
            if (_agent.remainingDistance != 0) return;
            _agent.isStopped = true;
            SetNextMovementTarget();
            await Task.Delay(_patrolDelay);
            _agent.isStopped = false;
        }
        
        /// <summary>
        /// Sets <see cref="moveTarget" /> & <see cref="lookTarget" /> to the position of the next patrol node.
        /// </summary>
        private void SetNextMovementTarget()
        {
            var nextTarget = PatrolNodes.Dequeue();
            PatrolNodes.Enqueue(nextTarget);
            _agent.SetDestination(nextTarget.position);
        }
    }
}