using System.Threading.Tasks;
using cmp2804.Math;
using Sirenix.OdinInspector;
using UnityEngine;

namespace cmp2804.Characters.States
{
    [RequireComponent(typeof(EnemyController))]
    public class ChaseState : SerializedMonoBehaviour, IEnemyState
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

        public void UpdateState()
        {
            
        }

        public async Task TickState()
        {
            _basicMovement.MoveTarget = new Target(_playerCharacterTransform);
            _basicMovement.LookTarget = new Target(_playerCharacterTransform);
            await Task.Delay(0);
        }
    }
}