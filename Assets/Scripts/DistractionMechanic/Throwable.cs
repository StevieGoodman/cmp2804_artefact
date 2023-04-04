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
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var distractionSource = new DistractionSource(transform.position, distractionRadius);
            gameObject.GetComponent<SoundMaker>().StartEmission();
        }

        public void DestroyAfterTime(float time)
        {
            Destroy(gameObject, time);
        }
    }
}
