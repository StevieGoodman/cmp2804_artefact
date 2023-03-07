using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace cmp2804.DistractionMechanic
{
    public class DistractionSource : MonoBehaviour
    {
        public Vector3 Origin;
        private float _radius;

        public DistractionSource(Vector3 origin, float radius)
        {
            this.Origin = origin;
            this._radius = radius;
        }

        private void EmitDistraction()
        {
            Collider[] colliders = Physics.OverlapSphere(Origin, _radius);
            foreach (Collider collider1 in colliders)
            {
                IDistractable distractable = collider1.GetComponent<IDistractable>();
                if (distractable != null)
                {
                    distractable.Distract(this);
                }
            }
        }
    }
}
