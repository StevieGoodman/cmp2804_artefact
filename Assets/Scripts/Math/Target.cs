using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Math
{
    [Serializable]
    public struct Target
    {
        // Fields
        [ShowIf("_targetType", TargetType.Transform)]
        [OdinSerialize] private Transform _transformTarget;
        [ShowIf("_targetType", TargetType.Vector)]
        [OdinSerialize] private Vector3 _vectorTarget;
        [OdinSerialize] private TargetType _targetType;
        
        // Properties
        public Vector3 Origin
        {
            get
            {
                return _targetType switch
                {
                    TargetType.Transform => _transformTarget.transform.position,
                    TargetType.Vector => _vectorTarget,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        // Methods
        /// <summary>
        /// Retrieves the vector from the transform to the target.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Vector3 GetVector(Transform transform)
        {
            return _targetType switch
            {
                TargetType.Transform => (_transformTarget.position - transform.position).normalized,
                TargetType.Vector => _vectorTarget.normalized,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        // Constructors
        public Target(GameObject gameObject)
            : this(gameObject.transform) {}
        public Target(Transform transform)
        {
            _targetType = TargetType.Transform;
            _transformTarget = transform;
            _vectorTarget = transform.position;
        }
        public Target(Vector3 vector3)
        {
            _targetType = TargetType.Vector;
            _transformTarget = null;
            _vectorTarget = vector3;
        }
    }

    public enum TargetType
    {
        Transform,
        Vector
    }
}