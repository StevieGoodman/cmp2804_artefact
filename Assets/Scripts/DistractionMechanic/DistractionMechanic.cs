using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cmp2804.DistractionMechanic
{
    public class DistractionMechanic : MonoBehaviour
    {
        public GameObject throwable;
        public float throwForce = 10f;

        private InputAction _throwAction;
        
        public void Awake()
        {
            _throwAction = new InputAction(binding: "<Mouse>/leftButton");
            _throwAction.performed += ThrowOnPerformed;
        }

        private void OnEnable()
        {
            _throwAction.Enable();
        }

        private void OnDisable()
        {
            _throwAction.Disable();
        }

        private void ThrowOnPerformed(InputAction.CallbackContext context)
        {
            GameObject throwableObject = Instantiate(throwable, transform.position, transform.rotation);

            Collider playerCollider = GetComponent<Collider>();
            Collider throwableCollider = throwableObject.GetComponent<Collider>();

            if (playerCollider != null && throwableCollider != null)
            {
                Physics.IgnoreCollision(playerCollider, throwableCollider);
            }
            
            Rigidbody throwableRigidbody = throwableObject.GetComponent<Rigidbody>();
            throwableRigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);

            DistractionSource distractionSource = new DistractionSource(throwableObject.transform.position, 5f);
        }
    }
}
