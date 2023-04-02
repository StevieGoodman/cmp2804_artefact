using cmp2804.Characters.Detection;
using cmp2804.Characters.States;
using cmp2804.DistractionMechanic;
using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(Patrol), typeof(Chase))]
    [RequireComponent(typeof(Sight), typeof(Investigate))]
    [HideMonoScript]
    public class EnemyController : SerializedMonoBehaviour, IDistractable
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
        
        public void Distract(DistractionSource source)
        {
            if (State is Chase) return;
            var investigate = gameObject.GetComponent<Investigate>();
            investigate.Target = new Target(source.Origin);
            State = investigate;
        }
    }
}