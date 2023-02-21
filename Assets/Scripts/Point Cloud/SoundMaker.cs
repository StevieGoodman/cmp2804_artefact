using System;
using System.Collections;
using UnityEngine;

namespace cmp2804.Point_Cloud
{
    public class SoundMaker : MonoBehaviour
    {
        [Header("Emission settings")] [SerializeField]
        private bool _useTransformRotation;

        [SerializeField] private bool _emitOnStartup;
        [SerializeField] private float _emissionFrequency;
        [SerializeField] private int _numberOfRays;
        [SerializeField] private float _pointLifespan;

        [Header("Emit zone")] [SerializeField] private Vector3 _direction;
        [SerializeField, Range(0, 360)] private float _angle;
        [SerializeField] private float _raycastDistance;

        private void Start()
        {
            if (_emitOnStartup)
            {
                StartCoroutine(Emit());
            }
        }

        public void StartEmission()
        {
            StopEmission();
            StartCoroutine(Emit());
        }

        public void StopEmission()
        {
            StopAllCoroutines();
        }

        IEnumerator Emit()
        {
            while (true)
            {
                SoundManager.MakeSound(transform, _useTransformRotation ? transform.forward : _direction, _angle,
                    _numberOfRays, _raycastDistance,
                    _pointLifespan);
                yield return new WaitForSeconds(_emissionFrequency);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_useTransformRotation)
            {
                _direction = transform.forward;
            }
            else
            {
                _direction.Normalize();
            }

            Gizmos.DrawWireSphere(transform.position, _raycastDistance);
            //Vector3 up = _direction * Vector3.up;
            Vector3 right = Vector3.Cross(_direction.normalized, Vector3.up + new Vector3(0.05f, 0.0f, 0.05f));
            Vector3 up = Vector3.Cross(_direction.normalized, right + new Vector3(0.05f, 0.0f, 0.05f));
            Quaternion leftRayRotation = Quaternion.AngleAxis(-_angle / 2, up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(_angle / 2, up);
            Quaternion upRayRotation = Quaternion.AngleAxis(-_angle / 2, right);
            Quaternion downRayRotation = Quaternion.AngleAxis(_angle / 2, right);
            Vector3 leftRayDirection = leftRayRotation * _direction;
            Vector3 rightRayDirection = rightRayRotation * _direction;
            Vector3 upRayDirection = upRayRotation * _direction;
            Vector3 downRayDirection = downRayRotation * _direction;
            if (_angle > 180)
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawRay(transform.position, leftRayDirection * _raycastDistance);
            Gizmos.DrawRay(transform.position, rightRayDirection * _raycastDistance);
            Gizmos.DrawRay(transform.position, upRayDirection * _raycastDistance);
            Gizmos.DrawRay(transform.position, downRayDirection * _raycastDistance);
        }
    }
}