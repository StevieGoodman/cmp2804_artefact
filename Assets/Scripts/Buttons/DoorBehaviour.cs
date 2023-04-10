using System.Collections.Generic;
using System.Linq;
using cmp2804.Buttons;
using cmp2804.Point_Cloud;
using UnityEngine;

namespace cmp2804
{

    public class DoorBehaviour : MonoBehaviour
    {
        public List<ButtonBehaviour> buttons;
        
        private void Start()
        {
            AddListeners();
        }

        /// <summary>
        /// Subscribe to the onToggle event of each button.
        /// </summary>
        private void AddListeners()
        {
            foreach (var button in buttons)
                button.onToggle.AddListener(OnButtonToggle);
        }
        
        /// <summary>
        /// Called when any button is toggled.
        /// </summary>
        private void OnButtonToggle()
        {
            var shouldOpenDoor = ShouldOpenDoor();
            Debug.Log("shouldOpenDoor = " + shouldOpenDoor);
            SetDoorOpenState(shouldOpenDoor);
        }

        /// <summary>
        /// Set the door to open or closed.
        /// </summary>
        /// <param name="shouldOpenDoor">Determines if the door is opened or not.</param>
        private void SetDoorOpenState(bool shouldOpenDoor)
        {
            if (shouldOpenDoor) SoundManager.DisableObjectPoints(transform);
            gameObject.SetActive(!shouldOpenDoor);
        }

        /// <summary>
        /// Checks if the door should be opened.
        /// </summary>
        /// <returns>Boolean representing if the door should be opened.</returns>
        private bool ShouldOpenDoor()
        {
            return buttons.All(b => b.toggled);
        }
    }
}
