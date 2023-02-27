using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace cmp2804.Characters
{
    public class PatrolState : SerializedMonoBehaviour, IEnemyState
    {
        [SerializeField] private EnemyMovement movement;
        [SerializeField] private Transform patrolNodesParent;
        private readonly TimeSpan _patrolDelay = new(0, 0, 0, 1);
        private Queue<Transform> _patrolNodes;
        
        private void Awake()
        {
            movement = gameObject.GetComponent<EnemyMovement>();
            _patrolNodes = new Queue<Transform>();
            foreach (Transform patrolNode in patrolNodesParent)
                _patrolNodes.Enqueue(patrolNode);
            SetNextTarget();
        }

        public void UpdateState()
        {
            
        }

        public async Task TickState()
        {
            var distanceToTarget = Vector3.Distance(movement.moveTarget.position, transform.position);
            if (distanceToTarget > 0.1f) return;
            movement.canMove = false;
            SetNextTarget();
            await Task.Delay(_patrolDelay);
            movement.canMove = true;
        }
        
        /// <summary>
        /// Sets <see cref="moveTarget" /> & <see cref="lookTarget" /> to the position of the next patrol node.
        /// </summary>
        private void SetNextTarget()
        {
            var nextTarget = _patrolNodes.Dequeue();
            _patrolNodes.Enqueue(nextTarget);
            movement.moveTarget = nextTarget;
            movement.lookTarget = nextTarget;
        }
    }
}