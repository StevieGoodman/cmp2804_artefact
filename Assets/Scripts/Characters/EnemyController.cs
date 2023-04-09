using System;
using System.Diagnostics;
using cmp2804.Characters.Detection;
using cmp2804.Characters.States;
using cmp2804.DistractionMechanic;
using cmp2804.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

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

        public Canvas enemyStateCanvas;
        public GameObject enemyStateIndicator;
        public Sprite enemyStatePatrol;
        public Sprite enemyStateInvestigate;
        public Sprite enemyStateChase;

        private Camera _camera;
        
        private void Awake()
        {
            State = gameObject.GetComponent<Patrol>();
            enemyStateIndicator.GetComponent<Image>().sprite = enemyStatePatrol;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            enemyStateCanvas.transform.rotation = _camera.transform.rotation;
        }

        public async void FixedUpdate()
        {
            State.TickState();
            State.UpdateState();
            
            enemyStateIndicator.GetComponent<Image>().sprite = State switch
            {
                Patrol => enemyStatePatrol,
                Investigate => enemyStateInvestigate,
                Chase => enemyStateChase,
                _ => null
            };
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