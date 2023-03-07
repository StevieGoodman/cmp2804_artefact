using System.Threading.Tasks;
using cmp2804.Math;
using UnityEngine;

namespace cmp2804.Characters.States
{
    [RequireComponent(typeof(EnemyController))]
    public class ChaseState : EnemyState
    {
        private BasicMovement _basicMovement;
        private Transform _playerCharacterTransform;

        private void Awake()
        {
            _basicMovement = gameObject.GetComponent<BasicMovement>();
            try
            {
                _playerCharacterTransform = GameObject.FindWithTag("Player")?.transform;
            }
            catch (UnityException)
            {
                Debug.LogError("No GameObject with tag \"Player\" could be found");
            }
        }

        public override void UpdateState()
        {
            
        }

        public override async Task TickState()
        {
            _basicMovement.MoveTarget = new Target(_playerCharacterTransform);
            _basicMovement.LookTarget = new Target(_playerCharacterTransform);
            await Task.Delay(0);
        }
    }
}