using cmp2804.Characters.Detection;
using cmp2804.Characters.States;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(Patrol), typeof(Chase))]
    [RequireComponent(typeof(Sight), typeof(Investigate))]
    [HideMonoScript]
    public class EnemyController : SerializedMonoBehaviour
    {
        [Title("State", "The current state defining the enemy's behaviour.")] 
        [OdinSerialize, HideLabel]
        public EnemyState State { get; set; }

        private void Awake()
        {
            State = gameObject.GetComponent<Patrol>();
        }

        public async void FixedUpdate()
        {
            await State.TickState();
            State.UpdateState();
        }
    }
}