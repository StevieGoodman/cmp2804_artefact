using System.Threading.Tasks;
using cmp2804.Characters.Detection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace cmp2804.Characters.States
{
    [RequireComponent(typeof(EnemyController), typeof(NavMeshAgent))]
    public class Chase : EnemyState
    {
        public GameObject GameOverUI;
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

        public override void TickState()
        {
            TryApprehendPlayer();
            if (!_navMeshAgent.destination.Equals(_playerCharacterTransform.position))
                _navMeshAgent.SetDestination(_playerCharacterTransform.position);
        }

        /// <summary>
        /// Checks if the player is within 1.25 units of the enemy, and if so, reloads the current scene.
        /// </summary>
        private void TryApprehendPlayer()
        {
            var distance = Vector3.Distance(transform.position, _playerCharacterTransform.position);
            if (distance < 1.25f)
               GameOverUI.SetActive(true);
               // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}