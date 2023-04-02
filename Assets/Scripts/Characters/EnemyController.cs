using System;
using cmp2804.Characters.Detection;
using cmp2804.Characters.States;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

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
            await State.TickState();
            State.UpdateState();

            if (State == gameObject.GetComponent<Patrol>())
            {
                enemyStateIndicator.GetComponent<Image>().sprite = enemyStatePatrol;
            }
            else if (State == gameObject.GetComponent<Investigate>())
            {
                enemyStateIndicator.GetComponent<Image>().sprite = enemyStateInvestigate;
            }
            else if (State == gameObject.GetComponent<Chase>())
            {
                enemyStateIndicator.GetComponent<Image>().sprite = enemyStateChase;
            }
        }
    }
}