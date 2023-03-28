using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cmp2804.Point_Cloud
{
    public class SoundMaker : SerializedMonoBehaviour
    {

        [Title("Emit zone")]

        [OdinSerialize][Range(0, 360)] private float _angle;
        [OdinSerialize] private Vector3 _offset;
        [OdinSerialize] private Vector3 _direction;
        [OdinSerialize][MinValue(0)] private float _emissionFrequency;
        [OdinSerialize] private bool _emitOnStartup;
        [OdinSerialize] private bool _inverted;
        [OdinSerialize][MinValue(0)] private int _numberOfRays;
        [OdinSerialize][MinValue(0)] private float _raycastDistance;

        [Title("Emission settings")]
        [OdinSerialize] private bool _useTransformRotation;
        [OdinSerialize] private float _pointLifespan;
        [OdinSerialize] private LayerMask _layerMask;

        public bool Emitting { get; private set; }

        

        private void Start()
        {
            if (_emitOnStartup) StartEmission();
        }

        private void OnDrawGizmosSelected()
        {
            if (_useTransformRotation)
                _direction = transform.forward;
            else
                _direction.Normalize();

            var position = transform.position + _offset;
            Gizmos.DrawWireSphere(position, _raycastDistance);

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

            Gizmos.DrawRay(position, leftRayDirection * _raycastDistance);
            Gizmos.DrawRay(position, rightRayDirection * _raycastDistance);
            Gizmos.DrawRay(position, upRayDirection * _raycastDistance);
            Gizmos.DrawRay(position, downRayDirection * _raycastDistance);
        }

        [HideIf("Emitting")]
        [DisableInEditorMode]
        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        public void StartEmission()
        {
            StopEmission();
            StartCoroutine(Emit());
            Emitting = true;
        }
        [ShowIf("Emitting")]
        [DisableInEditorMode]
        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        public void StopEmission()
        {
            StopAllCoroutines();
            Emitting = false;
        }

        public void MakeSound()
        {
            SoundManager.MakeSound(transform.position + _offset, _useTransformRotation ? transform.forward : _direction,
                                   _angle,
                                   _numberOfRays, _layerMask,
                                   _raycastDistance, _pointLifespan, _inverted);
        }
        public void MakeSound(float multiplier)
        {
            SoundManager.MakeSound(transform.position + _offset, _useTransformRotation ? transform.forward : _direction,
                                   _angle,
                                   Mathf.RoundToInt(_numberOfRays * multiplier), _layerMask,
                                   _raycastDistance * multiplier, _pointLifespan, _inverted);
        }

        private IEnumerator Emit()
        {
            while (true)
            {
                MakeSound();
                yield return new WaitForSeconds(_emissionFrequency);
            }
        }
    }
}