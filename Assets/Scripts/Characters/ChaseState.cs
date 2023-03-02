using System;
using System.Threading.Tasks;
using cmp2804.Math;
using Sirenix.OdinInspector;
using UnityEngine;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(EnemyController))]
    public class ChaseState : SerializedMonoBehaviour, IEnemyState
    {
        private Movement _movement;
        private Transform _playerCharacterTransform;

        private void Awake()
        {
            _movement = gameObject.GetComponent<Movement>();
            try
            {
                _playerCharacterTransform = GameObject.FindWithTag("Player")?.transform;
            }
            catch (UnityException)
            {
                Debug.LogError("No GameObject with tag \"Player\" could be found");
            }
        }

        public void UpdateState()
        {
            
        }

        public async Task TickState()
        {
            _movement.MoveTarget = new Target(_playerCharacterTransform);
            _movement.LookTarget = new Target(_playerCharacterTransform);
            await Task.Delay(0);
        }
    }
}