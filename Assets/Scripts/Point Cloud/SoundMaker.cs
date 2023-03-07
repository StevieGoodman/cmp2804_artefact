using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Point_Cloud
{
    public class SoundMaker : MonoBehaviour
    {
        [OdinSerialize] [Range(0, 360)] private float _angle;

        [Title("Emit zone")] [OdinSerialize] private Vector3 _direction;

        [OdinSerialize] private float _emissionFrequency;

        [OdinSerialize] private bool _emitOnStartup;
        [OdinSerialize] private bool _inverted;
        [OdinSerialize] private int _numberOfRays;
        [OdinSerialize] private float _pointLifespan;
        [OdinSerialize] private float _raycastDistance;

        [Title("Emission settings")] [OdinSerialize]
        private bool _useTransformRotation;

        private void Start()
        {
            if (_emitOnStartup) StartCoroutine(Emit());
        }

        private void OnDrawGizmosSelected()
        {
            if (_useTransformRotation)
                _direction = transform.forward;
            else
                _direction.Normalize();

            Gizmos.DrawWireSphere(transform.position, _raycastDistance);

            var right = Vector3.Cross(_direction.normalized, Vector3.up + new Vector3(0.05f, 0.0f, 0.05f));
            var up = Vector3.Cross(_direction.normalized, right + new Vector3(0.05f, 0.0f, 0.05f));
            var leftRayRotation = Quaternion.AngleAxis(-_angle / 2, up);
            var rightRayRotation = Quaternion.AngleAxis(_angle / 2, up);
            var upRayRotation = Quaternion.AngleAxis(-_angle / 2, right);
            var downRayRotation = Quaternion.AngleAxis(_angle / 2, right);
            var leftRayDirection = leftRayRotation * _direction;
            var rightRayDirection = rightRayRotation * _direction;
            var upRayDirection = upRayRotation * _direction;
            var downRayDirection = downRayRotation * _direction;
            if (_angle > 180) Gizmos.color = Color.red;

            var position = transform.position;
            Gizmos.DrawRay(position, leftRayDirection * _raycastDistance);
            Gizmos.DrawRay(position, rightRayDirection * _raycastDistance);
            Gizmos.DrawRay(position, upRayDirection * _raycastDistance);
            Gizmos.DrawRay(position, downRayDirection * _raycastDistance);
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

        private IEnumerator Emit()
        {
            while (true)
            {
                SoundManager.MakeSound(transform.position, _useTransformRotation ? transform.forward : _direction,
                    _angle,
                    _numberOfRays, _raycastDistance,
                    _pointLifespan, _inverted);
                yield return new WaitForSeconds(_emissionFrequency);
            }
        }
    }
}