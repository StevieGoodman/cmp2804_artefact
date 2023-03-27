using System.Threading.Tasks;
using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AI;

namespace cmp2804.Characters.States
{
    [RequireComponent(typeof(EnemyController), typeof(NavMeshAgent))]
    [HideMonoScript]
    class Investigate : EnemyState
    {
        public Target Target
        {
            private get => new(_agent.destination);
            set
            {
                _agent.SetDestination(value.Origin);
                InvestigateTime = InvestigateTimeMax;
            }
        }

        [OdinSerialize] public float InvestigateTime { get; private set; } = InvestigateTimeMax;
        [OdinSerialize] private const float InvestigateTimeMax = 5;
        
        private NavMeshAgent _agent;
        private EnemyController _controller;
        private Patrol _patrol;
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _controller = GetComponent<EnemyController>();
            _patrol = GetComponent<Patrol>();
        }

        public override void UpdateState()
        {
            if (InvestigateTime > 0) return;
            _controller.State = _patrol;
        }
        
        public override async Task TickState()
        {
            InvestigateTime -= Time.deltaTime;
            await Task.Delay(0);
        }
    }
}