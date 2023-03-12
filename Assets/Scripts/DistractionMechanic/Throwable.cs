using System.Collections;
using System.Collections.Generic;
using cmp2804.DistractionMechanic;
using UnityEngine;

namespace cmp2804
{
    public class Throwable : MonoBehaviour, IDistractable
    {
        public void Distract(DistractionSource source)
        {
            Debug.Log($"New distraction at {source.Origin}");
        }
    }
}
