using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmp2804
{
    public class ButtonLight : MonoBehaviour
    {
        public GameObject button;

        public Material green;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (button.tag == "pressed")
            {
                UnityEngine.Debug.Log("BUTTON PRESSED");
                button.GetComponent<Renderer>().material = green;
            }
            
        }
    }
}
