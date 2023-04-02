using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace cmp2804
{
    
    //assign the door, and however many buttons that the door will use, to the respective game Objects below// 

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

            //make sure the door has "Door" tag if only one button will open it.//
            
            //make sure the door has "Doorv2" tag if two buttons will open it.//


            //make sure the buttons have the correct script attached to them.//

        }
    }
}
