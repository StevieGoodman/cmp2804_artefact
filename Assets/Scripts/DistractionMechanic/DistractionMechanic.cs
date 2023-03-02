using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cmp2804.Characters
{
    public class DistractionMechanic : MonoBehaviour
    {
        public GameObject throwable;
        public float throwForce = 10f;
        public float objectLifetime = 5f;

        private InputAction _throwAction;
        private Collider _playerCollider;
        
        private void Start()
        {
            _playerCollider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            _throwAction = new InputAction("Throw", InputActionType.Button, "<Mouse>/leftButton");
            _throwAction.Enable();
            _throwAction.performed += _ => ThrowObject();
        }

        private void OnDisable()
        {
            _throwAction.Disable();
            _throwAction.performed -= _ => ThrowObject();
        }

        private void ThrowObject()
        {
            var thrownObject = Instantiate(throwable, transform.position, transform.rotation);
            Physics.IgnoreCollision(thrownObject.GetComponent<Collider>(), _playerCollider);
            thrownObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
            Destroy(thrownObject, objectLifetime);
        }
    }
}
