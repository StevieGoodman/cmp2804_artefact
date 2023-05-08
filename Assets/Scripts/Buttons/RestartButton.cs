using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cmp2804
{
    public class RestartButton : MonoBehaviour
    {
        
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        private void OnEnable()
        {
            Time.timeScale = 0f;
        }
    }
}
