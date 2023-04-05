using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace cmp2804
{
    public class ButtonBehaviour : MonoBehaviour
    {

        public GameObject Button;

        void OnCollisionStay(Collision coll)
        {
            if (coll.gameObject.tag == "Player")
                Button.tag = "pressed";

            else if (coll.gameObject.tag == "Throwable")
                Button.tag = "pressed";
            UnityEngine.Debug.Log("button pressed");


        }   
    }
}
