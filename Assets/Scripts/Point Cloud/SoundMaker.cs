using System;
using System.Collections;
using DG.Tweening;
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
        [OdinSerialize] private bool _useSoundRing = false;

        [ShowIfGroup("_useSoundRing")]
        [TitleGroup("_useSoundRing/Sound Ring Settings")]
        [OdinSerialize] private Material _soundRingMat;
        
        private Transform _soundRing;

        public bool Emitting { get; private set; }



        private void Start()
        {
            if(_useSoundRing)
            {
                GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Quad);
                Destroy(primitive.GetComponent<Collider>());
                primitive.GetComponent<MeshRenderer>().material = new Material(_soundRingMat);
                primitive.SetActive(false);
                _soundRing = primitive.transform;
                _soundRing.SetParent(transform);
                _soundRing.localPosition = Vector3.zero;
                _soundRing.rotation = Quaternion.Euler(90, 0, 0);
            }
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
        /// <summary>
        /// Emits one pulse from the sound maker.
        /// </summary>
        public void MakeSound()
        {
            SoundManager.MakeSound(transform.position + _offset, _useTransformRotation ? transform.forward : _direction,
                                   _angle,
                                   _numberOfRays, _layerMask,
                                   _raycastDistance, _pointLifespan, _inverted);
            if (!_useSoundRing) { return; }
            _soundRing.gameObject.SetActive(true);
            _soundRing.localScale = Vector3.zero;
            _soundRingMat.DOFade(1, 0);
            _soundRing.DOScale(_raycastDistance, 0.5f).OnComplete(() => _soundRing.gameObject.SetActive(false));
            _soundRingMat.DOFade(0, 0.5f);
        }
        /// <summary>
        /// Emits one pulse from the sound maker. The pulse will be louder the closer the multiplier is to 1.
        /// </summary>
        /// <param name="multiplier">A value ranging from 0-1.</param>
        public void MakeSound(float multiplier)
        {
            //https://cdn.discordapp.com/attachments/1059612797073362996/1091481867938693262/image.png
            multiplier = Mathf.Clamp01(multiplier);
            float adjustedMultiplier = Mathf.Pow(Mathf.Log(multiplier + 1) / Mathf.Log(2), 0.4f); 
            SoundManager.MakeSound(transform.position + _offset, _useTransformRotation ? transform.forward : _direction,
                                   _angle * adjustedMultiplier,
                                   Mathf.RoundToInt(_numberOfRays * multiplier), _layerMask,
                                   _raycastDistance * multiplier, _pointLifespan, _inverted);
            if (!_useSoundRing) { return; }
            _soundRing.gameObject.SetActive(true);
            _soundRing.localScale = Vector3.zero;
            _soundRingMat.DOFade(1, 0);
            _soundRing.DOScale(adjustedMultiplier * (adjustedMultiplier * 15), 0.5f).OnComplete(() => _soundRing.gameObject.SetActive(false));
            _soundRingMat.DOFade(0, 0.5f);
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