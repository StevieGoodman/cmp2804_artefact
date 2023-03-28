using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmp2804
{
    public class ButtonLight : MonoBehaviour
    {
        public GameObject button;
        public GameObject lightRed;
        public GameObject lightGreen;

        void Start()
        {
            lightGreen.SetActive(false);
        }
           

         

        // Update is called once per frame
        void Update()
        {
            if (button.tag == "pressed")
            {
                lightRed.SetActive(false);
                lightGreen.SetActive(true);
            }
            
        }
    }
}
