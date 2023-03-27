using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmp2804
{
    


    public class DoorBehaviour : MonoBehaviour
    {
        public GameObject Door;
        public GameObject Button;
        public GameObject Button2;

        // Update is called once per frame
        void Update()
        {
            if (Door.gameObject.tag == "Door" && Button.tag == "pressed")
                Door.SetActive(false);
            else if (Door.tag == "Doorv2" && Button.tag == "pressed" && Button2.tag == "pressed")
                Door.SetActive(false);
        }
    }
}
