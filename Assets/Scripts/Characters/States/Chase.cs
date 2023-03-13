using System.Threading.Tasks;
using cmp2804.Characters.Detection;
using UnityEngine;
using UnityEngine.AI;

namespace cmp2804.Characters.States
{
    [RequireComponent(typeof(EnemyController), typeof(NavMeshAgent))]
    public class Chase : EnemyState
    {
        private NavMeshAgent _navMeshAgent;
        private Sight _sight;
        private EnemyController _enemyController;
        private Patrol _patrol;
        private Transform _playerCharacterTransform;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _sight = GetComponent<Sight>();
            _enemyController = GetComponent<EnemyController>();
            _patrol = GetComponent<Patrol>();
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
            if (!_sight.PlayerInSight())
                _enemyController.State = _patrol;
        }

        public override async Task TickState()
        {
            _navMeshAgent.destination = _playerCharacterTransform.position;
            await Task.Delay(0);
        }
    }
}