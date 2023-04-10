using cmp2804.Point_Cloud;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace cmp2804.Characters
{
    [RequireComponent(typeof(SoundMaker))]
    public class Clap : SerializedMonoBehaviour
    {

        private SoundMaker _soundMaker;

        private void Start()
        {
            _soundMaker = GetComponent<SoundMaker>();

        }
        public void Process(UnityEngine.InputSystem.InputActionPhase phase, double duration)
        {
            switch (phase)
            {
                case UnityEngine.InputSystem.InputActionPhase.Started:
                    break;
                case UnityEngine.InputSystem.InputActionPhase.Canceled:
                    _soundMaker.MakeSound(Mathf.Min(1, (float)duration));

                    break;
                default:
                    break;
            }
        }
    }
}
