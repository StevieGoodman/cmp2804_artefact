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


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Door.gameObject.tag == "Door")
            {
                if (Button.tag == "pressed")
                {
                    Door.SetActive(false);
                }
            }
            
            else if (Door.tag == "Doorv2")
            {
                
               
                if (Button.tag == "pressed" && Button2.tag == "pressed" )
                {
                    Door.SetActive(false);
                }
                
                
            }
           

        }
    }
}
