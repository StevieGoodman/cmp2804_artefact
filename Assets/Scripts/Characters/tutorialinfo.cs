using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cmp2804
{
    public class tutorialinfo : MonoBehaviour
    {

        public Text text;
        public int id; // Each Info block has its own ID number
        public GameObject info_panel; 

        void OnCollisionEnter (Collision other)
        {
            if (other.collider.name == "Player Character")
            {
                info_panel.SetActive(true);
                switch(id)
            {
                case 1:
                text.text = "Use the W A S D keys to navigate around the environment. Use space to close the textbox";
                break;

                case 2:
                text.text = "Look for a button which is blue, it'll unlock a door";
                break;

                case 3:
                text.text = "Pick up a stone and throw it at the button";
                break;

                case 4:
                text.text = "Watch out for the enemies sight";
                break;

                case 5:
                text.text = "You'll be using all the previous taught skills to get around this freeform area";
                break;
            }
            }
            
        }
    }
}
