using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace cmp2804
{
    public class ButtonBehaviour : MonoBehaviour
    {

        public GameObject Player;
        public GameObject Button;
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnCollisionStay(Collision coll)
        {
          
            if (coll.gameObject.tag == "Player")
            {
            
                Button.tag = "pressed";
            }
        }
    }
}
