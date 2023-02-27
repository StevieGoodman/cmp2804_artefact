using System.Collections;
using cmp2804.Characters;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class PlayerMovement
    {
        [UnityTest]
        public IEnumerator IncrementMovement()
        {
            SetupPlayerCharacter(out var movement, out var movementState, out var rigidBody);
            var oldPosition = rigidBody.position;
            yield return null;
            var newPosition = rigidBody.position;
            var targetPosition = oldPosition + movement.MoveDirection * (Time.deltaTime * movementState.moveSpeed);
            Assert.AreEqual(
                newPosition, 
                targetPosition);
        }
        
        [UnityTest]
        public IEnumerator IncrementRotation()
        {
            SetupPlayerCharacter(out var movement, out var movementState, out var rigidBody);
            movement.SetMoveDirection(Vector3.left);
            var oldRotation = rigidBody.rotation;
            yield return null;
            var newRotation = rigidBody.rotation;
            var maximumAngle = 360 * Time.deltaTime / movementState.rotationSpeed;
            var targetRotation = Quaternion.RotateTowards(
                oldRotation, 
                newRotation, 
                maximumAngle);
            Assert.AreEqual(newRotation, targetRotation);
        }
        
        private static void SetupPlayerCharacter(
            out cmp2804.Characters.PlayerMovement movement, out MovementState movementState, out Rigidbody rigidBody)
        {
            var player = new GameObject("Player");
            movement = player.AddComponent<cmp2804.Characters.PlayerMovement>();
            movementState = ScriptableObject.CreateInstance<cmp2804.Characters.MovementState>();
            rigidBody = player.GetComponent<Rigidbody>();
            movementState.moveSpeed = 100f;
            movementState.rotationSpeed = 2f;
            movement.MovementState = movementState;
            movement.SetMoveDirection(Vector3.forward);
        }
    }
}
