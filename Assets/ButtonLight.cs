using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmp2804
{
    public class ButtonLight : MonoBehaviour
    {
        public GameObject button;

        public Material green;

        // Update is called once per frame
        void Update()
        {
            if (button.tag == "pressed") return;
            button.GetComponent<Renderer>().material = green;
        }
    }
}
