using System;
using System.Collections;
using System.Collections.Generic;
using cmp2804.DistractionMechanic;
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
            DistractionSource distractionSource = new DistractionSource(transform.position, distractionRadius);
        }

        public void DestroyAfterTime(float time)
        {
            Destroy(gameObject, time);
        }
    }
}
