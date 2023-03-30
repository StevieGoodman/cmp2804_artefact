using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cmp2804
{
    public class Panelswitch : MonoBehaviour
    {
  

        void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
