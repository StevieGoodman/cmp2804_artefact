using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace cmp2804.Characters.States
{
    [RequireComponent(typeof(EnemyController))]
    public class Chase : EnemyState
    {
        private NavMeshAgent _navMeshAgent;
        private Transform _playerCharacterTransform;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _playerCharacterTransform = GetPlayerCharacterTransform();
        }

        private Transform GetPlayerCharacterTransform()
        {
            try
            {
                return GameObject.FindWithTag("Player")?.transform;
            }
            catch (UnityException)
            {
                Debug.LogError("No GameObject with tag \"Player\" could be found");
            }
            return null;
        }

        public override void UpdateState()
        {
            // TODO: Implement this method.
        }

        public override async Task TickState()
        {
            _navMeshAgent.destination = _playerCharacterTransform.position;
            await Task.Delay(0);
        }
    }
}