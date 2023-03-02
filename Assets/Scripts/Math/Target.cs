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

    internal enum TargetType
    {
        Transform,
        Vector
    }
}