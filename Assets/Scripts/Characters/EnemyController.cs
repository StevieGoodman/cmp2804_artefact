using cmp2804.Characters.States;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(PatrolState))]
    [HideMonoScript]
    public class EnemyController : SerializedMonoBehaviour
    {
        [Title("State", "The current state defining the enemy's behaviour.")]
        [OdinSerialize, HideLabel] private EnemyState _state;

        private void Awake()
        {
            _state = gameObject.GetComponent<PatrolState>();
        }

        public async void FixedUpdate()
        {
            await _state.TickState();
            _state.UpdateState();
        }
    }
}