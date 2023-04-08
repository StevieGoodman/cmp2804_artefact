using UnityEngine;
using UnityEngine.Events;

namespace cmp2804.Buttons
{
    public class ButtonBehaviour : MonoBehaviour
    {
        public UnityEvent onToggle = new();
        public bool toggled;
        private const float DebounceDuration = 2f;
        public float debounceTimer;
        
        private void Update()
        {
            UpdateDebounceTimer();
        }

        public void OnCollisionEnter(Collision collision)
        {
            var other = collision.gameObject;
            ToggleButton(other);
        }

        /// <summary>
        /// Updates the debounce timer.
        /// </summary>
        private void UpdateDebounceTimer()
        {
            if (debounceTimer > 0)
                debounceTimer -= Time.deltaTime;
            debounceTimer = Mathf.Clamp(debounceTimer, 0, DebounceDuration);
        }
        
        /// <summary>
        /// Toggles the button and invokes the onToggle event.
        /// </summary>
        /// <param name="other">The GameObject that collided with the button.</param>
        private void ToggleButton(GameObject other)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Throwable")) return;
            if (debounceTimer > 0) return;
            debounceTimer = DebounceDuration;
            toggled = !toggled;
            onToggle.Invoke();
        }
    }
}
