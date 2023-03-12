using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace cmp2804.DistractionMechanic
{
    public struct DistractionSource
    {
        public Vector3 Origin { get; }
        private float _radius;

        public DistractionSource(Vector3 origin, float radius)
        {
            this.Origin = origin;
            this._radius = radius;
            
            EmitDistraction();
        }

        private void EmitDistraction()
        {
            Collider[] colliders = Physics.OverlapSphere(Origin, _radius);

            foreach (Collider collider in colliders)
            {
                IDistractable distractable = collider.GetComponent<IDistractable>();
                if (distractable != null)
                {
                    distractable.Distract(this);
                }
            }
        }
    }
}
