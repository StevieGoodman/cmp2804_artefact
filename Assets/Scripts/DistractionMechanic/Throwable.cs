using System;
using System.Collections;
using System.Collections.Generic;
using cmp2804.DistractionMechanic;
using cmp2804.Point_Cloud;
using UnityEngine;

namespace cmp2804
{
    public class Throwable : MonoBehaviour
    {
        public float distractionRadius = 5f;

        private void OnCollisionEnter(Collision collision)
        {
            gameObject.GetComponent<SoundMaker>().MakeSound(true, distractionRadius);
        }

        public void DestroyAfterTime(float time)
        {
            Destroy(gameObject, time);
        }
    }
}
